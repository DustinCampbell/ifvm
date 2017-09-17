using System;
using IFVM.Ast;

namespace IFVM.Execution
{
    public partial class Interpreter
    {
        private void Evaluate(AstStatement statement)
        {
            switch (statement.Kind)
            {
                case AstNodeKind.LabelStatement:
                    break;

                case AstNodeKind.ExpressionStatement:
                    {
                        var expressionStatement = (AstExpressionStatement)statement;
                        Evaluate(expressionStatement.Expression); // throw away result
                        break;
                    }

                case AstNodeKind.StackPushStatement:
                    {
                        var stackPushStatement = (AstStackPushStatement)statement;
                        var value = Evaluate(stackPushStatement.Value);
                        _machine.Stack.PushDWord(value);
                        break;
                    }

                case AstNodeKind.WriteLocalStatement:
                    {
                        var writeLocalStatement = (AstWriteLocalStatement)statement;
                        var index = (int)Evaluate(writeLocalStatement.Local.Index);
                        var value = Evaluate(writeLocalStatement.Value);
                        _machine.CurrentFrame.WriteLocal(index, value);
                        break;
                    }

                default:
                    throw new NotSupportedException($"Invalid statement kind: {statement.Kind}");
            }
        }
    }
}
