using System;
using System.Threading.Tasks;
using IFVM.Core;
using IFVM.TestUtilities;
using Xunit;

namespace IFVM.Glulx.Tests
{
    public class GlulxMachineTests
    {
        [Fact]
        public async Task advent_glulx_version_is_2_0_0()
        {
            using (var stream = Resources.LoadResource(Resources.Glulx_Advent))
            {
                var memory = await Memory.CreateAsync(stream);
                var machine = new GlulxMachine(memory);

                Assert.Equal(new Version("2.0.0"), machine.GlulxVersion);
            }
        }

        [Fact]
        public async Task glulxercise_glulx_version_is_3_1_3()
        {
            using (var stream = Resources.LoadResource(Resources.Glulx_Glulxercise))
            {
                var memory = await Memory.CreateAsync(stream);
                var machine = new GlulxMachine(memory);

                Assert.Equal(new Version("3.1.3"), machine.GlulxVersion);
            }
        }
    }
}
