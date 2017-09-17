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

                default:
                    throw new InvalidOperationException($"Invalid statement kind: {statement.Kind}");
            }
        }
    }
}
