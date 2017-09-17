using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using IFVM.Ast;
using IFVM.Extensions;
using IFVM.Utilities;

namespace IFVM.Execution
{
    public partial class Interpreter
    {
        private static readonly ObjectPool<Stack<AstExpression>> _expressionStacks = new ObjectPool<Stack<AstExpression>>(() => new Stack<AstExpression>());
        private static readonly ObjectPool<Stack<uint>> _evaluationStacks = new ObjectPool<Stack<uint>>(() => new Stack<uint>());

        private void AddToExpressionStack(Stack<AstExpression> stack, AstExpression expression)
        {
            stack.Push(expression);

            // Add any sub-expressions
            switch (expression.Kind)
            {
                case AstNodeKind.ConversionExpression:
                    {
                        var conversion = (AstConversionExpression)expression;
                        AddToExpressionStack(stack, conversion.Expression);
                        break;
                    }

                case AstNodeKind.AddExpression:
                case AstNodeKind.SubtractExpression:
                case AstNodeKind.MultiplyExpression:
                case AstNodeKind.DivideExpression:
                case AstNodeKind.ModuloExpression:
                case AstNodeKind.EqualToExpression:
                case AstNodeKind.NotEqualToExpression:
                case AstNodeKind.LessThanExpression:
                case AstNodeKind.LessThanOrEqualToExpression:
                case AstNodeKind.GreaterThanExpression:
                case AstNodeKind.GreaterThanOrEqualToExpression:
                    {
                        var binary = (AstBinaryExpression)expression;
                        stack.Push(binary.Right);
                        stack.Push(binary.Left);
                        break;
                    }

                case AstNodeKind.CallExpression:
                    {
                        var call = (AstCallExpression)expression;
                        var args = call.Arguments;

                        // Push arguments in reverse order
                        for (int i = args.Count - 1; i >= 0; i--)
                        {
                            AddToExpressionStack(stack, args[i]);
                        }

                        AddToExpressionStack(stack, call.Address);
                        break;
                    }

                case AstNodeKind.ReadMemoryExpression:
                    {
                        var readMemory = (AstReadMemoryExpression)expression;
                        AddToExpressionStack(stack, readMemory.Address);
                        break;
                    }

                case AstNodeKind.DispatchExpression:
                    {
                        var dispatch = (AstDispatchExpression)expression;
                        var args = dispatch.Arguments;

                        // Push arguments in reverse order
                        for (int i = args.Count - 1; i >= 0; i--)
                        {
                            AddToExpressionStack(stack, args[i]);
                        }

                        break;
                    }
            }
        }

        private uint EvaluateExpressionStack(Stack<AstExpression> expressionStack)
        {
            var evaluationStack = _evaluationStacks.Allocate();

            try
            {
                while (expressionStack.TryPop(out var expression))
                {
                    switch (expression.Kind)
                    {
                        case AstNodeKind.ConstantExpression:
                            {
                                var constant = (AstConstantExpression)expression;
                                var value = (uint)constant.Value;
                                evaluationStack.Push(value);
                                break;
                            }

                        case AstNodeKind.ConversionExpression:
                            {
                                Debug.Assert(evaluationStack.Count >= 1);

                                var conversion = (AstConversionExpression)expression;
                                var value = evaluationStack.Pop();

                                switch (conversion.Size)
                                {
                                    case ValueSize.Byte:
                                        value = conversion.Signed
                                            ? (uint)(sbyte)(value & 0xff)
                                            : value & 0xff;
                                        break;

                                    case ValueSize.Word:
                                        value = conversion.Signed
                                            ? (uint)(short)(value & 0xffff)
                                            : value & 0xffff;
                                        break;

                                    case ValueSize.DWord:
                                        break;

                                    default:
                                        throw new NotSupportedException($"Invalid conversion size: {conversion.Size}");
                                }

                                evaluationStack.Push(value);
                                break;
                            }

                        case AstNodeKind.AddExpression:
                            {
                                Debug.Assert(evaluationStack.Count >= 2);

                                var right = evaluationStack.Pop();
                                var left = evaluationStack.Pop();
                                var value = left + right;

                                evaluationStack.Push(value);
                                break;
                            }

                        case AstNodeKind.SubtractExpression:
                            {
                                Debug.Assert(evaluationStack.Count >= 2);

                                var right = evaluationStack.Pop();
                                var left = evaluationStack.Pop();
                                var value = left - right;

                                evaluationStack.Push(value);
                                break;
                            }

                        case AstNodeKind.MultiplyExpression:
                            {
                                Debug.Assert(evaluationStack.Count >= 2);

                                var right = evaluationStack.Pop();
                                var left = evaluationStack.Pop();
                                var value = left * right;

                                evaluationStack.Push(value);
                                break;
                            }

                        case AstNodeKind.DivideExpression:
                            {
                                Debug.Assert(evaluationStack.Count >= 2);

                                var right = (int)evaluationStack.Pop();
                                var left = (int)evaluationStack.Pop();
                                var value = (uint)(left / right);

                                evaluationStack.Push(value);
                                break;
                            }

                        case AstNodeKind.ModuloExpression:
                            {
                                Debug.Assert(evaluationStack.Count >= 2);

                                var right = (int)evaluationStack.Pop();
                                var left = (int)evaluationStack.Pop();
                                var value = (uint)(left % right);

                                evaluationStack.Push(value);
                                break;
                            }

                        case AstNodeKind.EqualToExpression:
                            {
                                Debug.Assert(evaluationStack.Count >= 2);

                                var right = evaluationStack.Pop();
                                var left = evaluationStack.Pop();
                                var value = left == right ? 1u : 0u;

                                evaluationStack.Push(value);
                                break;
                            }

                        case AstNodeKind.NotEqualToExpression:
                            {
                                Debug.Assert(evaluationStack.Count >= 2);

                                var right = evaluationStack.Pop();
                                var left = evaluationStack.Pop();
                                var value = left != right ? 1u : 0u;

                                evaluationStack.Push(value);
                                break;
                            }

                        case AstNodeKind.LessThanExpression:
                            {
                                Debug.Assert(evaluationStack.Count >= 2);

                                var right = evaluationStack.Pop();
                                var left = evaluationStack.Pop();
                                var value = left < right ? 1u : 0u;

                                evaluationStack.Push(value);
                                break;
                            }

                        case AstNodeKind.LessThanOrEqualToExpression:
                            {
                                Debug.Assert(evaluationStack.Count >= 2);

                                var right = evaluationStack.Pop();
                                var left = evaluationStack.Pop();
                                var value = left <= right ? 1u : 0u;

                                evaluationStack.Push(value);
                                break;
                            }

                        case AstNodeKind.GreaterThanExpression:
                            {
                                Debug.Assert(evaluationStack.Count >= 2);

                                var right = evaluationStack.Pop();
                                var left = evaluationStack.Pop();
                                var value = left > right ? 1u : 0u;

                                evaluationStack.Push(value);
                                break;
                            }

                        case AstNodeKind.GreaterThanOrEqualToExpression:
                            {
                                Debug.Assert(evaluationStack.Count >= 2);

                                var right = evaluationStack.Pop();
                                var left = evaluationStack.Pop();
                                var value = left >= right ? 1u : 0u;

                                evaluationStack.Push(value);
                                break;
                            }

                        case AstNodeKind.CallExpression:
                            {
                                var call = (AstCallExpression)expression;

                                Debug.Assert(evaluationStack.Count >= call.Arguments.Count + 1);

                                var args = ImmutableArray.CreateBuilder<uint>(initialCapacity: call.Arguments.Count);
                                args.Count = call.Arguments.Count;

                                for (int i = call.Arguments.Count - 1; i >= 0; i--)
                                {
                                    args[i] = evaluationStack.Pop();
                                }

                                var address = evaluationStack.Pop();

                                var result = _machine.CallFunction((int)address, args.ToImmutable());
                                evaluationStack.Push(result);
                                break;
                            }

                        case AstNodeKind.StackPopExpression:
                            {
                                evaluationStack.Push(_machine.Stack.PopDWord());
                                break;
                            }

                        case AstNodeKind.ReadLocalExpression:
                            {
                                var readLocal = (AstReadLocalExpression)expression;
                                var index = (int)Evaluate(readLocal.Local.Index);

                                evaluationStack.Push(_machine.CurrentFrame.ReadLocal(index));

                                break;
                            }

                        case AstNodeKind.ReadMemoryExpression:
                            {
                                Debug.Assert(evaluationStack.Count >= 1);

                                var readMemory = (AstReadMemoryExpression)expression;
                                var address = evaluationStack.Pop();

                                switch (readMemory.Size)
                                {
                                    case ValueSize.Byte:
                                        evaluationStack.Push(_machine.Memory.ReadByte((int)address));
                                        break;

                                    case ValueSize.Word:
                                        evaluationStack.Push(_machine.Memory.ReadWord((int)address));
                                        break;

                                    case ValueSize.DWord:
                                        evaluationStack.Push(_machine.Memory.ReadDWord((int)address));
                                        break;

                                    default:
                                        throw new NotSupportedException($"Invalid memory size: {readMemory.Size}");
                                }

                                break;
                            }

                        case AstNodeKind.GetMemorySize:
                            {
                                evaluationStack.Push((uint)_machine.Memory.Size);
                                break;
                            }

                        default:
                            throw new NotSupportedException($"Unsupported node kind: {expression.Kind}");
                    }
                }

                Debug.Assert(expressionStack.Count == 0);
                Debug.Assert(evaluationStack.Count == 1);

                return evaluationStack.Pop();
            }
            finally
            {
                _evaluationStacks.ClearAndFree(evaluationStack);
            }
        }

        private uint Evaluate(AstExpression expression)
        {
            // Handle easy cases
            switch (expression.Kind)
            {
                case AstNodeKind.ConstantExpression:
                    return (uint)((AstConstantExpression)expression).Value;
            }

            var expressionStack = _expressionStacks.Allocate();

            try
            {
                AddToExpressionStack(expressionStack, expression);

                return EvaluateExpressionStack(expressionStack);
            }
            finally
            {
                _expressionStacks.ClearAndFree(expressionStack);
            }
        }
    }
}
