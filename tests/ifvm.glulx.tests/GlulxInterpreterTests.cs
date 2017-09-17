using System.Threading.Tasks;
using IFVM.TestUtilities;
using Xunit;

namespace IFVM.Glulx.Tests
{
    public class GlulxInterpreterTests
    {
        [Fact]
        public async Task run_advent()
        {
            using (var stream = Resources.LoadResource(Resources.Glulx_Advent))
            {
                var machine = await GlulxMachine.CreateAsync(stream);
                machine.Run();
            }
        }
    }
}
