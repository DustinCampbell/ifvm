using System;
using System.Collections.Immutable;
using IFVM.Ast;
using IFVM.Core;

namespace IFVM.Glulx.Functions
{
    public class Function
    {
        public FunctionType Type { get; }
        public int Address { get; }
        public AstBody Body { get; }

        private Function(FunctionType type, int address, AstBody body)
        {
            this.Type = type;
            this.Address = address;
            this.Body = body;
        }

        public static Function Read(Memory memory, int address, int ramStart)
        {
            var scanner = memory.CreateScanner(address);
            var type = ReadType(scanner);
            var bodyBuilder = new AstBodyBuilder();
            var addressToLocalMap = ReadLocals(scanner, bodyBuilder);
            var binder = new InstructionBinder(bodyBuilder, addressToLocalMap, ramStart);
            ReadBody(scanner, binder);
            var body = bodyBuilder.ToBody();

            return new Function(type, address, body);
        }

        private static FunctionType ReadType(MemoryScanner scanner)
        {
            FunctionType type;

            var typeByte = scanner.NextByte();
            switch (typeByte)
            {
                case 0xC0:
                    type = FunctionType.StackArgument;
                    break;
                case 0xC1:
                    type = FunctionType.LocalArgument;
                    break;
                default:
                    if (typeByte >= 0xC0 && typeByte <= 0xDF)
                    {
                        throw new InvalidOperationException($"Unknown function type: {typeByte}");
                    }

                    throw new InvalidOperationException($"Non-function: {typeByte}");
            }

            return type;
        }

        private static ImmutableDictionary<int, AstLocal> ReadLocals(MemoryScanner scanner, AstBodyBuilder bodyBuilder)
        {
            var map = ImmutableDictionary.CreateBuilder<int, AstLocal>();
            var address = 0;

            while (true)
            {
                var sizeByte = scanner.NextByte();
                var countByte = scanner.NextByte();

                if (sizeByte == 0 && countByte == 0)
                {
                    break;
                }

                var size = SizeByteToValueSize(sizeByte);

                // The Glulx spec states that locals will have padding to bring values
                // to their natural alignment. We should account for that here. Note that
                // the spec guarantees that the locals will start at a 4-byte boundary.
                if (size != ValueSize.Byte && address % sizeByte != 0)
                {
                    address += sizeByte;
                }

                for (int i = 0; i < countByte; i++)
                {
                    var local = bodyBuilder.DeclareLocal(size);
                    map.Add(address, local);
                    address += sizeByte;
                }
            }

            return map.ToImmutable();
        }

        private static ValueSize SizeByteToValueSize(byte value)
        {
            switch (value)
            {
                case 1: return ValueSize.Byte;
                case 2: return ValueSize.Word;
                case 4: return ValueSize.DWord;

                default: throw new InvalidOperationException($"Invalid value for ValueSize: {value}");
            }
        }

        private static void ReadBody(MemoryScanner scanner, InstructionBinder binder)
        {
            while (true)
            {
                // Note: We read instructions until we find an opcode number that we don't know about.

                var address = scanner.Address;
                var opcodeNumber = ReadOpcodeNumber(scanner);

                if (!Opcodes.TryGetOpcode(opcodeNumber, out var opcode))
                {
                    break;
                }

                var (loadOps, storeOps) = ReadOperands(scanner, opcode);

                var length = scanner.Address - address;

                binder.BindInstruction(opcode, loadOps, storeOps, address, length);
            }
        }

        private static uint ReadOpcodeNumber(MemoryScanner scanner)
        {
            var firstByte = scanner.PeekByte();

            if ((firstByte & 0xC0) == 0xC0)
            {
                return scanner.NextDWord() & 0x0fffffffu;
            }

            if ((firstByte & 0x80) == 0x80)
            {
                return scanner.NextWord() & 0x3fffu;
            }

            return scanner.NextByte() & 0x7fu;
        }

        private static (ImmutableList<Operand> loadOps, ImmutableList<Operand> storeOps) ReadOperands(MemoryScanner scanner, OpcodeDescriptor opcode)
        {
            var loadOpsBuilder = ImmutableList.CreateBuilder<Operand>();
            var storeOpsBuilder = ImmutableList.CreateBuilder<Operand>();

            var count = opcode.OperandCount;
            if (count > 0)
            {
                var typesLength = count / 2;
                if (count % 2 != 0)
                {
                    typesLength++;
                }

                var dataScanner = scanner.Memory.CreateScanner(scanner.Address + typesLength);

                for (int i = 0; i < count; i += 2)
                {
                    var opsBuilder = opcode.OperandKinds[i] == OperandKind.Load
                        ? loadOpsBuilder
                        : storeOpsBuilder;

                    var typeByte = scanner.NextByte();

                    var type = (byte)(typeByte & 0x0f);
                    var operand = ReadOperand(dataScanner, type);
                    opsBuilder.Add(operand);

                    if (i + 1 < count)
                    {
                        opsBuilder = opcode.OperandKinds[i + 1] == OperandKind.Load
                            ? loadOpsBuilder
                            : storeOpsBuilder;

                        type = (byte)(typeByte >> 4);
                        operand = ReadOperand(dataScanner, type);
                        opsBuilder.Add(operand);
                    }
                }

                scanner.SetAddress(dataScanner.Address);
            }

            return (loadOpsBuilder.ToImmutable(), storeOpsBuilder.ToImmutable());
        }

        private static Operand ReadOperand(MemoryScanner scanner, byte type)
        {
            switch (type)
            {
                case 0x0: return new Operand(OperandType.Constant, 0);
                case 0x1: return new Operand(OperandType.Constant, (uint)(sbyte)scanner.NextByte());
                case 0x2: return new Operand(OperandType.Constant, (uint)(short)scanner.NextWord());
                case 0x3: return new Operand(OperandType.Constant, scanner.NextDWord());
                case 0x5: return new Operand(OperandType.Address, scanner.NextByte());
                case 0x6: return new Operand(OperandType.Address, scanner.NextWord());
                case 0x7: return new Operand(OperandType.Address, scanner.NextDWord());
                case 0x8: return new Operand(OperandType.Stack, 0);
                case 0x9: return new Operand(OperandType.LocalAddress, scanner.NextByte());
                case 0xa: return new Operand(OperandType.LocalAddress, scanner.NextWord());
                case 0xb: return new Operand(OperandType.LocalAddress, scanner.NextDWord());
                case 0xd: return new Operand(OperandType.RamAddress, scanner.NextByte());
                case 0xe: return new Operand(OperandType.RamAddress, scanner.NextWord());
                case 0xf: return new Operand(OperandType.RamAddress, scanner.NextDWord());

                default: throw new InvalidOperationException($"Invalid operand type byte: {type}");
            }
        }
    }
}
