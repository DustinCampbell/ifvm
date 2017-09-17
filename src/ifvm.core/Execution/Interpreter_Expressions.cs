using System;
using IFVM.Ast;

namespace IFVM.Execution
{
    public partial class Interpreter
    {
        private uint Execute(AstExpression expression)
        {
            switch (expression.Kind)
            {
                case AstNodeKind.ConstantExpression:
                    return (uint)((AstConstantExpression)expression).Value;

                default:
                    throw new InvalidOperationException($"Invalid expression kind: {expression.Kind}");
            }
        }
    }
}
