using System.Threading.Tasks;
using IFVM.Execution;
using IFVM.TestUtilities;
using Xunit;

namespace IFVM.Glulx.Tests
{
    public class GlulxInterpreterTests
    {
        [Fact(Skip = "Can't run advent all the way through yet")]
        public async Task run_advent()
        {
            using (var stream = Resources.LoadResource(Resources.Glulx_Advent))
            {
                var machine = await GlulxMachine.CreateAsync(stream);
                var interpreter = new Interpreter(machine);

                interpreter.Execute(machine.StartFunction);
            }
        }
    }
}
