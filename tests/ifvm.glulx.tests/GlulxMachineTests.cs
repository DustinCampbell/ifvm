using System.Threading.Tasks;
using IFVM.Ast;
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
                var machine = await GlulxMachine.CreateAsync(stream);

                Assert.Equal("2.0.0", machine.Header.Version.ToString());
            }
        }

        [Fact]
        public async Task advent_glulx_memlen_is_same_as_memory_size()
        {
            using (var stream = Resources.LoadResource(Resources.Glulx_Advent))
            {
                var machine = await GlulxMachine.CreateAsync(stream);

                Assert.Equal((int)machine.Header.EndMem, machine.Memory.Size);
            }
        }

        [Fact]
        public async Task advent_glulx_stacksize_is_same_as_stack_size()
        {
            using (var stream = Resources.LoadResource(Resources.Glulx_Advent))
            {
                var machine = await GlulxMachine.CreateAsync(stream);

                Assert.Equal((int)machine.Header.StackSize, machine.Stack.Size);
            }
        }

        [Fact]
        public async Task advent_glulx_start_function_has_correct_body()
        {
            using (var stream = Resources.LoadResource(Resources.Glulx_Advent))
            {
                var machine = await GlulxMachine.CreateAsync(stream);
                var body = AstDumper.Dump(machine.StartFunction.Body);

                const string expected = @"
call 48 ()
return 0";

                Assert.Equal(expected.Trim(), body.Trim());
            }
        }

        [Fact]
        public async Task glulxercise_glulx_version_is_3_1_3()
        {
            using (var stream = Resources.LoadResource(Resources.Glulx_Glulxercise))
            {
                var machine = await GlulxMachine.CreateAsync(stream);

                Assert.Equal("3.1.3", machine.Header.Version.ToString());
            }
        }

        [Fact]
        public async Task glulxercise_glulx_start_function_has_correct_body()
        {
            using (var stream = Resources.LoadResource(Resources.Glulx_Glulxercise))
            {
                var machine = await GlulxMachine.CreateAsync(stream);
                var body = AstDumper.Dump(machine.StartFunction.Body);

                const string expected = @"
call 48 ()
return 0";

                Assert.Equal(expected.Trim(), body.Trim());
            }
        }
    }
}
