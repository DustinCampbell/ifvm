using System.Threading.Tasks;
using IFVM.Ast;
using IFVM.Glulx.Functions;
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
label_0:
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
label_0:
    call 48 ()
    return 0";

                Assert.Equal(expected.Trim(), body.Trim());
            }
        }

        [Fact]
        public async Task glulxercise_glulx_function_48_has_correct_body()
        {
            using (var stream = Resources.LoadResource(Resources.Glulx_Glulxercise))
            {
                var machine = await GlulxMachine.CreateAsync(stream);
                var function = machine.ReadFunction(0x48);
                var body = AstDumper.Dump(function.Body);

                const string expected = @"
label_0:
    call fb ()
    local_0 <- 74a
    push call 1c8bd (28c95, 0)
    if (pop != 4d2) then 
        jump label_4
label_1:
    push call 1c8bd (28c95, 1)
    if (pop != 2) then 
        jump label_4
label_2:
    push call 1c8bd (28c95, 2)
    if (pop != 0) then 
        jump label_4
label_3:
    output-char a
    output-string 1d122
    call 1c93c (28c95, 1, 3)
    call 1c93c (28c95, 2, 3)
    output-string 1d147
    restore-undo
    output-string 1d156
    output-num local_0
    output-string 1d161
    quit
label_4:
    output-char a
    call 4e9 ()
    output-char a
    call e61 ()
    call 672 ()
    output-string 1d16d
    output-string 1d1a7
    return 1
";

                Assert.Equal(expected.Trim(), body.Trim());
            }
        }
    }
}
