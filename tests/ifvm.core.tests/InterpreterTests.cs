using IFVM.Ast;
using IFVM.Execution;
using IFVM.TestUtilities;
using Xunit;

namespace IFVM.Core.Tests
{
    public class InterpreterTests
    {
        [Fact]
        public void return_simple_constant_expression()
        {
            var builder = new AstBodyBuilder();
            var label = builder.NewLabel();
            builder.MarkLabel(label);
            builder.Return(
                AstFactory.ConstantExpression(42));

            var function = new TestFunction(builder.ToBody());

            var machine = new TestMachine();
            var interpreter = new Interpreter(machine);

            var result = interpreter.Execute(function);

            Assert.Equal(42u, result);
        }
    }
}
