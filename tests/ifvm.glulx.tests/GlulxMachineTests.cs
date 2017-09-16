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
            #region expected
            const string expected = @"
label_0:
    call 48 ()
    return 0";
            #endregion

            using (var stream = Resources.LoadResource(Resources.Glulx_Advent))
            {
                var machine = await GlulxMachine.CreateAsync(stream);
                var body = AstDumper.Dump(machine.StartFunction.Body);

                Assert.Equal(expected.Trim(), body.Trim());
            }
        }

        [Fact]
        public async Task advent_glulx_function_48_has_correct_body()
        {
            #region expected
            const string expected = @"
label_0:
    push 109
    push 1dcf5
    call 104b0 (pop, pop)
    return 1
";
            #endregion

            using (var stream = Resources.LoadResource(Resources.Glulx_Advent))
            {
                var machine = await GlulxMachine.CreateAsync(stream);
                var function = machine.GetFunction(0x48);
                var body = AstDumper.Dump(function.Body);

                Assert.Equal(expected.Trim(), body.Trim());
            }
        }

        [Fact]
        public async Task advent_glulx_function_104b0_has_correct_body()
        {
            #region expected
            const string expected = @"
label_0:
    local_0 <- pop
    local_1 <- pop
    local_2 <- pop
    local_0 <- (local_0 - 2)
    push local_1
    local_3 <- call 11072 (pop)
    if (local_3 != 2) then 
        jump label_4
label_1:
    if (local_2 != 105) then 
        jump label_3
label_2:
    local_4 <- read.4(1c600 + 14)
    write.4(1c600 + 14) <- read.4(1c600 + 10)
    write.4(1c600 + 10) <- local_1
    write.4(1c600 + 18) <- read.4(1c600 + d8)
    local_6 <- call local_1 arg-count: local_0
    write.4(1c600 + 10) <- read.4(1c600 + 14)
    write.4(1c600 + 14) <- local_4
    return local_6
label_3:
    jump label_48
label_4:
    if (local_3 != 3) then 
        jump label_15
label_5:
    if (local_2 != 106) then 
        jump label_7
label_6:
    output-string local_1
    return 1
label_7:
    if (local_2 != 107) then 
        jump label_14
label_8:
    if (local_0 < 2) then 
        jump label_10
label_9:
    local_9 <- pop
    local_8 <- pop
    jump label_11
label_10:
    local_9 <- pop
    local_8 <- 7fffffff
label_11:
    push 48
    local_5 <- call 114fe (pop)
    push 0
    push 1
    push (local_8 - 4)
    push (local_9 + 4)
    push 43
    local_4 <- call 114fe (pop, pop, pop, pop, pop)
    if (local_4 == 0) then 
        jump label_13
label_12:
    push local_4
    push 47
    call 114fe (pop, pop)
    output-string local_1
    push local_5
    push 47
    call 114fe (pop, pop)
    push -1
    push local_4
    glk(44, 2)
    local_8 <- pop
    pop
    write.4(local_9 + (0 * 4)) <- local_8
    return local_8
label_13:
    return 0
label_14:
    jump label_48
label_15:
    if (local_3 == 1) then 
        jump label_17
label_16:
    jump label_48
label_17:
    push read.4(local_1 + (5 * 4))
    if (pop != 1da55) then 
        jump label_26
label_18:
    write.4(1c600 + 0) <- local_2
    if (read.4(1c600 + 0) != 103) then 
        jump label_20
label_19:
    push local_2
    push local_1
    push call 111a4 (pop, pop)
    return pop
label_20:
    if (read.4(1c600 + 0) != 104) then 
        jump label_22
label_21:
    local_9 <- pop
    local_A <- pop
    push local_A
    push local_9
    push local_2
    push local_1
    push call 111a4 (pop, pop, pop, pop)
    return pop
label_22:
    if (read.4(1c600 + 0) == 100) then 
        jump label_25
label_23:
    if (read.4(1c600 + 0) == 102) then 
        jump label_25
label_24:
    if (read.4(1c600 + 0) != 101) then 
        jump label_26
label_25:
    local_9 <- (local_0 + 2)
    push local_2
    push local_1
    local_A <- call 111a4 arg-count: local_9
    return local_A
label_26:
    push local_2
    push local_1
    local_7 <- call 107f3 (pop, pop)
    if (local_7 != 0) then 
        jump label_32
label_27:
    if (local_2 <= 0) then 
        jump label_30
label_28:
    if (local_2 >= 100) then 
        jump label_30
label_29:
    push (4 * local_2)
    local_7 <- (24e27 + pop)
    local_8 <- 4
    jump label_31
label_30:
    jump label_48
label_31:
    jump label_33
label_32:
    push local_2
    push local_1
    local_8 <- call 1087f (pop, pop)
label_33:
    local_9 <- 0
label_34:
    push (4 * local_9)
    if (pop >= local_8) then 
        jump label_47
label_35:
    local_A <- read.4(local_7 + (local_9 * 4))
    if (local_A == -1) then 
        return 0
label_36:
    push local_A
    push call 11072 (pop)
    write.4(1c600 + 0) <- pop
    if (read.4(1c600 + 0) != 2) then 
        jump label_43
label_37:
    local_4 <- read.4(1c600 + 14)
    write.4(1c600 + 14) <- read.4(1c600 + 10)
    write.4(1c600 + 10) <- local_1
    local_5 <- read.4(1c600 + 18)
    if (local_2 != 5) then 
        jump label_39
label_38:
    write.4(1c600 + 18) <- read.4(1c600 + f0)
    jump label_40
label_39:
    write.4(1c600 + 18) <- read.4(1c600 + d8)
label_40:
    stack-copy local_0
    local_6 <- call local_A arg-count: local_0
    write.4(1c600 + 10) <- read.4(1c600 + 14)
    write.4(1c600 + 14) <- local_4
    write.4(1c600 + 18) <- local_5
    if (local_6 == 0) then 
        jump label_42
label_41:
    return local_6
label_42:
    jump label_46
label_43:
    if (read.4(1c600 + 0) != 3) then 
        jump label_45
label_44:
    output-string local_A
    output-char a
    return 1
label_45:
    return local_A
label_46:
    local_9 <- (local_9 + 1)
    jump label_34
label_47:
    return 0
label_48:
    push local_2
    push local_1
    push 1baf0
    call 10b49 (pop, pop, pop)
    return 0
";
            #endregion

            using (var stream = Resources.LoadResource(Resources.Glulx_Advent))
            {
                var machine = await GlulxMachine.CreateAsync(stream);
                var function = machine.GetFunction(0x104b0);
                var body = AstDumper.Dump(function.Body);

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
            #region expected
            const string expected = @"
label_0:
    call 48 ()
    return 0";
            #endregion

            using (var stream = Resources.LoadResource(Resources.Glulx_Glulxercise))
            {
                var machine = await GlulxMachine.CreateAsync(stream);
                var body = AstDumper.Dump(machine.StartFunction.Body);

                Assert.Equal(expected.Trim(), body.Trim());
            }
        }

        [Fact]
        public async Task glulxercise_glulx_function_48_has_correct_body()
        {
            #region expected
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
            #endregion

            using (var stream = Resources.LoadResource(Resources.Glulx_Glulxercise))
            {
                var machine = await GlulxMachine.CreateAsync(stream);
                var function = machine.GetFunction(0x48);
                var body = AstDumper.Dump(function.Body);

                Assert.Equal(expected.Trim(), body.Trim());
            }
        }
    }
}
