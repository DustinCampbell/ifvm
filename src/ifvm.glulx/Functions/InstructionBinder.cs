using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using IFVM.Ast;

namespace IFVM.Glulx.Functions
{
    public class InstructionBinder
    {
        private readonly AstBodyBuilder bodyBuilder;
        private readonly ImmutableDictionary<int, AstLocal> addressToLocalMap;
        private readonly Dictionary<int, AstLabel> addressToLabelMap;
        private readonly int ramStart;

        public InstructionBinder(AstBodyBuilder bodyBuilder, ImmutableDictionary<int, AstLocal> addressToLocalMap, int ramStart)
        {
            this.bodyBuilder = bodyBuilder;
            this.addressToLocalMap = addressToLocalMap;
            this.addressToLabelMap = new Dictionary<int, AstLabel>();
            this.ramStart = ramStart;
        }

        private AstLabel GetOrCreateLabel(int address)
        {
            if (!this.addressToLabelMap.TryGetValue(address, out var label))
            {
                label = this.bodyBuilder.NewLabel();
                this.addressToLabelMap.Add(address, label);
            }

            return label;
        }

        public void BindInstruction(
            OpcodeDescriptor opcode, ImmutableList<Operand> loadOperands, ImmutableList<Operand> storeOperands,
            int address, int length)
        {
            // We create a label at every address. Unused labels will be pruned by the AstBodyBuilder later.
            var label = GetOrCreateLabel(address);
            this.bodyBuilder.MarkLabel(label);

            var nextAddress = address + length;

            switch (opcode.Number)
            {
                case Opcodes.op_add:
                    BindAdd(
                        leftOp: loadOperands[0],
                        rightOp: loadOperands[1],
                        storeOp: storeOperands[0]);
                    break;

                case Opcodes.op_aload:
                    BindRead(
                        baseAddressOp: loadOperands[0],
                        offsetOp: loadOperands[1],
                        storeOp: storeOperands[0],
                        size: ValueSize.DWord);
                    break;

                case Opcodes.op_astore:
                    BindWrite(
                        baseAddressOp: loadOperands[0],
                        offsetOp: loadOperands[1],
                        valueOp: loadOperands[2],
                        size: ValueSize.DWord);
                    break;

                case Opcodes.op_call:
                    BindCall(
                        addressOp: loadOperands[0],
                        argumentCountOp: loadOperands[1],
                        storeOp: storeOperands[0]);
                    break;

                case Opcodes.op_callf:
                    BindCall0(
                        addressOp: loadOperands[0],
                        storeOp: storeOperands[0]);
                    break;

                case Opcodes.op_callfi:
                    BindCall1(
                        addressOp: loadOperands[0],
                        arg1Op: loadOperands[1],
                        storeOp: storeOperands[0]);
                    break;

                case Opcodes.op_callfii:
                    BindCall2(
                        addressOp: loadOperands[0],
                        arg1Op: loadOperands[1],
                        arg2Op: loadOperands[2],
                        storeOp: storeOperands[0]);
                    break;

                case Opcodes.op_callfiii:
                    BindCall3(
                        addressOp: loadOperands[0],
                        arg1Op: loadOperands[1],
                        arg2Op: loadOperands[2],
                        arg3Op: loadOperands[3],
                        storeOp: storeOperands[0]);
                    break;

                case Opcodes.op_copy:
                    BindCopy(
                        targetOp: loadOperands[0],
                        storeOp: storeOperands[0],
                        size: ValueSize.DWord);
                    break;

                case Opcodes.op_div:
                    BindDivide(
                        leftOp: loadOperands[0],
                        rightOp: loadOperands[1],
                        storeOp: storeOperands[0]);
                    break;

                case Opcodes.op_glk:
                    BindGlk(
                        identifierOp: loadOperands[0],
                        argumentCountOp: loadOperands[1],
                        storeOperands: storeOperands[0]);
                    break;

                case Opcodes.op_jeq:
                    BindJumpIfEqual(
                        leftOp: loadOperands[0],
                        rightOp: loadOperands[1],
                        jumpOffsetOp: loadOperands[2],
                        nextAddress: nextAddress);
                    break;

                case Opcodes.op_jge:
                    BindJumpIfGreaterThanOrEqual(
                        leftOp: loadOperands[0],
                        rightOp: loadOperands[1],
                        jumpOffsetOp: loadOperands[2],
                        nextAddress: nextAddress);
                    break;

                case Opcodes.op_jgt:
                    BindJumpIfGreaterThan(
                        leftOp: loadOperands[0],
                        rightOp: loadOperands[1],
                        jumpOffsetOp: loadOperands[2],
                        nextAddress: nextAddress);
                    break;

                case Opcodes.op_jle:
                    BindJumpIfLessThanOrEqual(
                        leftOp: loadOperands[0],
                        rightOp: loadOperands[1],
                        jumpOffsetOp: loadOperands[2],
                        nextAddress: nextAddress);
                    break;

                case Opcodes.op_jlt:
                    BindJumpIfLessThan(
                        leftOp: loadOperands[0],
                        rightOp: loadOperands[1],
                        jumpOffsetOp: loadOperands[2],
                        nextAddress: nextAddress);
                    break;

                case Opcodes.op_jne:
                    BindJumpIfNotEqual(
                        leftOp: loadOperands[0],
                        rightOp: loadOperands[1],
                        jumpOffsetOp: loadOperands[2],
                        nextAddress: nextAddress);
                    break;

                case Opcodes.op_jnz:
                    BindJumpIfNotEqualToZero(
                        leftOp: loadOperands[0],
                        jumpOffsetOp: loadOperands[1],
                        nextAddress: nextAddress);
                    break;

                case Opcodes.op_jump:
                    BindJump(
                        jumpOffsetOp: loadOperands[0],
                        nextAddress: nextAddress);
                    break;

                case Opcodes.op_jz:
                    BindJumpIfEqualToZero(
                        leftOp: loadOperands[0],
                        jumpOffsetOp: loadOperands[1],
                        nextAddress: nextAddress);
                    break;

                case Opcodes.op_mod:
                    BindModulo(
                        leftOp: loadOperands[0],
                        rightOp: loadOperands[1],
                        storeOp: storeOperands[0]);
                    break;

                case Opcodes.op_mul:
                    BindMultiply(
                        leftOp: loadOperands[0],
                        rightOp: loadOperands[1],
                        storeOp: storeOperands[0]);
                    break;

                case Opcodes.op_quit:
                    BindQuit();
                    break;

                case Opcodes.op_restoreundo:
                    BindRestoreUndo();
                    break;

                case Opcodes.op_return:
                    BindReturn(
                        expressionOp: loadOperands[0]);
                    break;

                case Opcodes.op_stkcopy:
                    BindStackCopy(
                        countOp: loadOperands[0]);
                    break;

                case Opcodes.op_streamchar:
                    BindStreamChar(
                        characterOp: loadOperands[0]);
                    break;

                case Opcodes.op_streamnum:
                    BindStreamNumber(
                        numberOp: loadOperands[0]);
                    break;

                case Opcodes.op_streamstr:
                    BindStreamString(
                        addressOp: loadOperands[0]);
                    break;

                case Opcodes.op_sub:
                    BindSubtract(
                        leftOp: loadOperands[0],
                        rightOp: loadOperands[1],
                        storeOp: storeOperands[0]);
                    break;

                default: throw new NotSupportedException($"Unsupported opcode: {opcode}");
            }
        }

        private AstExpression BindLoadOperand(Operand operand, ValueSize size = ValueSize.DWord)
        {
            var operandValue = (int)operand.Value;

            switch (operand.Type)
            {
                case OperandType.Address:
                    return AstFactory.ReadMemoryExpression(
                        address: AstFactory.ConstantExpression(operandValue),
                        size: size);

                case OperandType.Constant:
                    return AstFactory.ConstantExpression(operandValue);

                case OperandType.LocalAddress:
                    var local = this.addressToLocalMap[operandValue];
                    return AstFactory.ReadLocalExpression(local);

                case OperandType.RamAddress:
                    return AstFactory.ReadMemoryExpression(
                        address: AstFactory.ConstantExpression(ramStart).Plus(operandValue),
                        size: size);

                case OperandType.Stack:
                    return AstFactory.StackPopExpression();
            }

            throw new InvalidOperationException($"Invalid operand type for load operand: {operand.Type}");
        }

        private void BindStoreOperand(Operand operand, AstExpression value, ValueSize size = ValueSize.DWord)
        {
            var operandValue = (int)operand.Value;

            switch (operand.Type)
            {
                case OperandType.Address:
                    this.bodyBuilder.AddStatement(
                        AstFactory.WriteMemoryStatement(
                            address: AstFactory.ConstantExpression(operandValue),
                            value: value,
                            size: size));
                    break;

                case OperandType.Constant:
                    if (operandValue != 0)
                    {
                        throw new InvalidOperationException("Store operand cannot be a non-zero constant.");
                    }

                    this.bodyBuilder.AddStatement(
                        AstFactory.ExpressionStatement(value));

                    break;

                case OperandType.LocalAddress:
                    var local = this.addressToLocalMap[operandValue];
                    this.bodyBuilder.WriteLocal(local, value);
                    break;

                case OperandType.RamAddress:
                    this.bodyBuilder.AddStatement(
                        AstFactory.WriteMemoryStatement(
                            address: AstFactory.ConstantExpression(ramStart).Plus(operandValue),
                            value: value,
                            size: size));
                    break;

                case OperandType.Stack:
                    this.bodyBuilder.AddStatement(
                        AstFactory.StackPushStatement(value));
                    break;

                default: throw new InvalidOperationException($"Invalid operand type for store operand: {operand.Type}");
            }
        }

        private void BindAdd(Operand leftOp, Operand rightOp, Operand storeOp)
        {
            var left = BindLoadOperand(leftOp);
            var right = BindLoadOperand(rightOp);

            BindStoreOperand(storeOp, left.Plus(right));
        }

        private void BindCall(Operand addressOp, Operand argumentCountOp, Operand storeOp)
        {
            if (argumentCountOp.Type == OperandType.Constant)
            {
                var builder = ImmutableList.CreateBuilder<AstExpression>();
                for (int i = 0; i < argumentCountOp.Value; i++)
                {
                    builder.Add(AstFactory.StackPopExpression());
                }

                var address = BindLoadOperand(addressOp);
                var argumentList = builder.ToImmutable();
                var call = AstFactory.CallExpression(address, argumentList);

                BindStoreOperand(storeOp, call);
            }
            else
            {
                var address = BindLoadOperand(addressOp);
                var argumentCount = BindLoadOperand(argumentCountOp);
                var call = AstFactory.CallWithArgCountExpression(address, argumentCount);

                BindStoreOperand(storeOp, call);
            }
        }

        private void BindCall0(Operand addressOp, Operand storeOp)
        {
            var address = BindLoadOperand(addressOp);
            var call = AstFactory.CallExpression(address, ImmutableList<AstExpression>.Empty);

            BindStoreOperand(storeOp, call);
        }

        private void BindCall1(Operand addressOp, Operand arg1Op, Operand storeOp)
        {
            var address = BindLoadOperand(addressOp);
            var arg1 = BindLoadOperand(arg1Op);
            var call = AstFactory.CallExpression(address, ImmutableList.Create(arg1));

            BindStoreOperand(storeOp, call);
        }

        private void BindCall2(Operand addressOp, Operand arg1Op, Operand arg2Op, Operand storeOp)
        {
            var address = BindLoadOperand(addressOp);
            var arg1 = BindLoadOperand(arg1Op);
            var arg2 = BindLoadOperand(arg2Op);
            var call = AstFactory.CallExpression(address, ImmutableList.Create(arg1, arg2));

            BindStoreOperand(storeOp, call);
        }

        private void BindCall3(Operand addressOp, Operand arg1Op, Operand arg2Op, Operand arg3Op, Operand storeOp)
        {
            var address = BindLoadOperand(addressOp);
            var arg1 = BindLoadOperand(arg1Op);
            var arg2 = BindLoadOperand(arg2Op);
            var arg3 = BindLoadOperand(arg3Op);
            var call = AstFactory.CallExpression(address, ImmutableList.Create(arg1, arg2, arg3));

            BindStoreOperand(storeOp, call);
        }

        private void BindCopy(Operand targetOp, Operand storeOp, ValueSize size)
        {
            var target = BindLoadOperand(targetOp);

            AstExpression expression;
            if (target.Kind == AstNodeKind.ReadLocalExpression && ((AstReadLocalExpression)target).Local.Size != size)
            {
                expression = AstFactory.ConversionExpression(target, size, signed: false);
            }
            else if (size != ValueSize.DWord)
            {
                expression = AstFactory.ConversionExpression(target, size, signed: false);
            }
            else
            {
                expression = target;
            }

            BindStoreOperand(storeOp, expression, size);
        }

        private void BindDivide(Operand leftOp, Operand rightOp, Operand storeOp)
        {
            var left = BindLoadOperand(leftOp);
            var right = BindLoadOperand(rightOp);

            BindStoreOperand(storeOp, left.DividedBy(right));
        }

        private void BindGlk(Operand identifierOp, Operand argumentCountOp, Operand storeOperands)
        {
            var identifier = BindLoadOperand(identifierOp);
            var argumentCount = BindLoadOperand(argumentCountOp);

            BindStoreOperand(storeOperands,
                AstFactory.DispatchExpression(DispatchFunctions.Glk, ImmutableList.Create(identifier, argumentCount)));
        }

        private void BindJump(Operand jumpOffsetOp, int nextAddress)
        {
            var jump = CreateJumpStatement(jumpOffsetOp, nextAddress);
            this.bodyBuilder.AddStatement(jump);
        }

        private void BindJumpIf(AstExpression condition, Operand jumpOffsetOp, int nextAddress)
        {
            var statement = CreateJumpStatement(jumpOffsetOp, nextAddress);
            var branch = AstFactory.BranchStatement(condition, statement);

            this.bodyBuilder.AddStatement(branch);
        }

        private void BindJumpIfEqual(Operand leftOp, Operand rightOp, Operand jumpOffsetOp, int nextAddress)
        {
            var left = BindLoadOperand(leftOp);
            var right = BindLoadOperand(rightOp);
            var condition = AstFactory.EqualToExpression(left, right);

            BindJumpIf(condition, jumpOffsetOp, nextAddress);
        }

        private void BindJumpIfEqualToZero(Operand leftOp, Operand jumpOffsetOp, int nextAddress)
        {
            var left = BindLoadOperand(leftOp);
            var right = AstFactory.Zero;
            var condition = AstFactory.EqualToExpression(left, right);

            BindJumpIf(condition, jumpOffsetOp, nextAddress);
        }

        private void BindJumpIfGreaterThan(Operand leftOp, Operand rightOp, Operand jumpOffsetOp, int nextAddress)
        {
            var left = BindLoadOperand(leftOp);
            var right = BindLoadOperand(rightOp);
            var condition = AstFactory.GreaterThanExpression(left, right);

            BindJumpIf(condition, jumpOffsetOp, nextAddress);
        }

        private void BindJumpIfGreaterThanOrEqual(Operand leftOp, Operand rightOp, Operand jumpOffsetOp, int nextAddress)
        {
            var left = BindLoadOperand(leftOp);
            var right = BindLoadOperand(rightOp);
            var condition = AstFactory.GreaterThanOrEqualToExpression(left, right);

            BindJumpIf(condition, jumpOffsetOp, nextAddress);
        }

        private void BindJumpIfLessThan(Operand leftOp, Operand rightOp, Operand jumpOffsetOp, int nextAddress)
        {
            var left = BindLoadOperand(leftOp);
            var right = BindLoadOperand(rightOp);
            var condition = AstFactory.LessThanExpression(left, right);

            BindJumpIf(condition, jumpOffsetOp, nextAddress);
        }

        private void BindJumpIfLessThanOrEqual(Operand leftOp, Operand rightOp, Operand jumpOffsetOp, int nextAddress)
        {
            var left = BindLoadOperand(leftOp);
            var right = BindLoadOperand(rightOp);
            var condition = AstFactory.LessThanOrEqualToExpression(left, right);

            BindJumpIf(condition, jumpOffsetOp, nextAddress);
        }

        private void BindJumpIfNotEqual(Operand leftOp, Operand rightOp, Operand jumpOffsetOp, int nextAddress)
        {
            var left = BindLoadOperand(leftOp);
            var right = BindLoadOperand(rightOp);
            var condition = AstFactory.NotEqualToExpression(left, right);

            BindJumpIf(condition, jumpOffsetOp, nextAddress);
        }

        private void BindJumpIfNotEqualToZero(Operand leftOp, Operand jumpOffsetOp, int nextAddress)
        {
            var left = BindLoadOperand(leftOp);
            var right = AstFactory.Zero;
            var condition = AstFactory.NotEqualToExpression(left, right);

            BindJumpIf(condition, jumpOffsetOp, nextAddress);
        }

        private AstStatement CreateJumpStatement(Operand jumpOffsetOp, int nextAddress)
        {
            if (jumpOffsetOp.Type != OperandType.Constant)
            {
                throw new InvalidOperationException($"Unexpected operand type for jump offset: {jumpOffsetOp.Type}");
            }

            int jumpOffsetValue = (int)jumpOffsetOp.Value;
            if (jumpOffsetValue == 0 || jumpOffsetValue == 1)
            {
                return AstFactory.ReturnStatement(
                    expression: AstFactory.ConstantExpression(jumpOffsetValue));
            }

            var address = nextAddress + jumpOffsetValue - 2;
            var label = GetOrCreateLabel(address);

            return AstFactory.JumpStatement(label);
        }

        private void BindModulo(Operand leftOp, Operand rightOp, Operand storeOp)
        {
            var left = BindLoadOperand(leftOp);
            var right = BindLoadOperand(rightOp);

            BindStoreOperand(storeOp, left.Modulo(right));
        }

        private void BindMultiply(Operand leftOp, Operand rightOp, Operand storeOp)
        {
            var left = BindLoadOperand(leftOp);
            var right = BindLoadOperand(rightOp);

            BindStoreOperand(storeOp, left.Times(right));
        }

        private void BindQuit()
        {
            this.bodyBuilder.Quit();
        }

        private void BindRestoreUndo()
        {
            this.bodyBuilder.AddStatement(
                AstFactory.RestoreUndoStatement());
        }

        private void BindReturn(Operand expressionOp)
        {
            var expression = BindLoadOperand(expressionOp);

            this.bodyBuilder.Return(expression);
        }

        private void BindStackCopy(Operand countOp)
        {
            var count = BindLoadOperand(countOp);

            this.bodyBuilder.AddStatement(
                AstFactory.StackCopyStatement(count));
        }

        private void BindStreamChar(Operand characterOp)
        {
            var character = BindLoadOperand(characterOp);

            this.bodyBuilder.AddStatement(
                AstFactory.OutputCharStatement(character));
        }

        private void BindStreamNumber(Operand numberOp)
        {
            var number = BindLoadOperand(numberOp);

            this.bodyBuilder.AddStatement(
                AstFactory.OutputNumberStatement(number));
        }

        private void BindStreamString(Operand addressOp)
        {
            var address = BindLoadOperand(addressOp);

            this.bodyBuilder.AddStatement(
                AstFactory.OutputStringStatement(address));
        }

        private void BindSubtract(Operand leftOp, Operand rightOp, Operand storeOp)
        {
            var left = BindLoadOperand(leftOp);
            var right = BindLoadOperand(rightOp);

            BindStoreOperand(storeOp, left.Minus(right));
        }

        private void BindRead(Operand baseAddressOp, Operand offsetOp, Operand storeOp, ValueSize size)
        {
            var baseAddress = BindLoadOperand(baseAddressOp);
            var offset = BindLoadOperand(offsetOp);

            var address = ComputeAddress(baseAddress, offset, size);

            BindStoreOperand(storeOp,
                AstFactory.ReadMemoryExpression(address, size));
        }

        private void BindWrite(Operand baseAddressOp, Operand offsetOp, Operand valueOp, ValueSize size)
        {
            var baseAddress = BindLoadOperand(baseAddressOp);
            var offset = BindLoadOperand(offsetOp);
            var value = BindLoadOperand(valueOp);

            var address = ComputeAddress(baseAddress, offset, size);

            this.bodyBuilder.AddStatement(
                AstFactory.WriteMemoryStatement(address, value, size));
        }

        private AstExpression ComputeAddress(AstExpression baseAddress, AstExpression offset, ValueSize size)
        {
            switch (size)
            {
                case ValueSize.DWord: return baseAddress.Plus(offset.Times(4));
                case ValueSize.Word: return baseAddress.Plus(offset.Times(2));
                case ValueSize.Byte: return baseAddress.Plus(offset);

                default: throw new InvalidOperationException($"Unexpected size: {size}");
            }
        }
    }
}
