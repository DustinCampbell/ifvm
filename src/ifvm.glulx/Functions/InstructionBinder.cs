using System;
using System.Collections.Immutable;
using IFVM.Ast;

namespace IFVM.Glulx.Functions
{
    public class InstructionBinder
    {
        private readonly AstBodyBuilder bodyBuilder;
        private readonly ImmutableDictionary<int, AstLocal> addressToLocalMap;
        private readonly int ramStart;

        public InstructionBinder(AstBodyBuilder bodyBuilder, ImmutableDictionary<int, AstLocal> addressToLocalMap, int ramStart)
        {
            this.bodyBuilder = bodyBuilder;
            this.addressToLocalMap = addressToLocalMap;
            this.ramStart = ramStart;
        }

        public void BindInstruction(
            OpcodeDescriptor opcode, ImmutableList<Operand> loadOperands, ImmutableList<Operand> storeOperands,
            int address, int length)
        {
            switch (opcode.Number)
            {
                case Opcodes.op_call:
                    BindCall(
                        addressOp: loadOperands[0],
                        argumentCountOp: loadOperands[1],
                        storeOp: storeOperands[0]);
                    break;

                case Opcodes.op_return:
                    BindReturn(
                        expressionOp: loadOperands[0]);
                    break;

                default: throw new NotSupportedException($"Unsupposed opcode: {opcode}");
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

        private void BindReturn(Operand expressionOp)
        {
            var expression = BindLoadOperand(expressionOp);

            this.bodyBuilder.Return(expression);
        }

    }
}
