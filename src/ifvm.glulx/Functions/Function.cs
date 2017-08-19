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
        public ImmutableList<LocalFormat> LocalFormats { get; }

        private Function(FunctionType type, int address, ImmutableList<LocalFormat> localFormats)
        {
            this.Type = type;
            this.Address = address;
            this.LocalFormats = localFormats;
        }

        public static Function Read(Memory memory, int address)
        {
            var scanner = memory.CreateScanner(address);
            var type = ReadType(scanner);
            var localFormats = ReadLocalFormats(scanner);

            return new Function(type, address, localFormats);
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

        private static ImmutableList<LocalFormat> ReadLocalFormats(MemoryScanner scanner)
        {
            var builder = ImmutableList.CreateBuilder<LocalFormat>();

            while (true)
            {
                var sizeByte = scanner.NextByte();
                var countByte = scanner.NextByte();

                if (sizeByte == 0 && countByte == 0)
                {
                    break;
                }

                builder.Add(new LocalFormat(SizeByteToValueSize(sizeByte), countByte));
            }

            return builder.ToImmutable();
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
    }
}
