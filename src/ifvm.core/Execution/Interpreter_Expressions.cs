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
                default:
                    throw new InvalidOperationException($"Invalid expression kind: {expression.Kind}");
            }
        }
    }
}
