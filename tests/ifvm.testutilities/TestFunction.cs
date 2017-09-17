using IFVM.Ast;
using IFVM.Core;

namespace IFVM.TestUtilities
{
    public class TestFunction : Function
    {
        public TestFunction(AstBody body)
            : base(
                  kind: FunctionKind.LocalArgument,
                  address: 0,
                  body: body)
        {
        }
    }
}
