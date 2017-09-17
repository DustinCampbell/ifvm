using System;
using System.Threading.Tasks;
using IFVM.Collections;
using IFVM.FlowAnalysis;
using IFVM.TestUtilities;
using Xunit;

namespace IFVM.Glulx.Tests
{
    public class ControlFlowGraphTests
    {
        [Fact]
        public async Task advent_glulx_start_function()
        {
            #region code
            //label_0:
            //    call 48 ()
            //    return 0";
            #endregion

            using (var stream = Resources.LoadResource(Resources.Glulx_Advent))
            {
                var machine = await GlulxMachine.CreateAsync(stream);
                var graph = ControlFlowGraph.Compute(machine.StartFunction.Body);

                Assert.Equal(3, graph.Blocks.Count);

                Assert.Single(graph.EntryBlock.Successors);
                Assert.Empty(graph.EntryBlock.Predecessors);

                Assert.Empty(graph.ExitBlock.Successors);
                Assert.Single(graph.ExitBlock.Predecessors);

                VerifyLabel(0, graph,
                    Predecessors(Entry),
                    Successors(Exit),
                    StatementCount(3));
            }
        }

        [Fact]
        public async Task glulxercise_glulx_function_48_has_correct_body()
        {
            #region code
            //label_0:
            //    call fb ()
            //    local_0 <- 74a
            //    push call 1c8bd (28c95, 0)
            //    if (pop != 4d2) then 
            //        jump label_4
            //label_1:
            //    push call 1c8bd (28c95, 1)
            //    if (pop != 2) then 
            //        jump label_4
            //label_2:
            //    push call 1c8bd (28c95, 2)
            //    if (pop != 0) then 
            //        jump label_4
            //label_3:
            //    output-char a
            //    output-string 1d122
            //    call 1c93c (28c95, 1, 3)
            //    call 1c93c (28c95, 2, 3)
            //    output-string 1d147
            //    restore-undo
            //    output-string 1d156
            //    output-num local_0
            //    output-string 1d161
            //    quit
            //label_4:
            //    output-char a
            //    call 4e9 ()
            //    output-char a
            //    call e61 ()
            //    call 672 ()
            //    output-string 1d16d
            //    output-string 1d1a7
            //    return 1
            #endregion

            using (var stream = Resources.LoadResource(Resources.Glulx_Glulxercise))
            {
                var machine = await GlulxMachine.CreateAsync(stream);
                var function = machine.GetFunction(0x48);
                var graph = ControlFlowGraph.Compute(function.Body);

                Assert.Equal(7, graph.Blocks.Count);

                Assert.Single(graph.EntryBlock.Successors);
                Assert.Empty(graph.EntryBlock.Predecessors);

                Assert.Empty(graph.ExitBlock.Successors);
                Assert.Equal(2, graph.ExitBlock.Predecessors.Count);

                VerifyLabel(0, graph,
                    Predecessors(Entry),
                    Successors(1, 4),
                    StatementCount(5));

                VerifyLabel(1, graph,
                    Predecessors(0),
                    Successors(2, 4),
                    StatementCount(3));

                VerifyLabel(2, graph,
                    Predecessors(1),
                    Successors(3, 4),
                    StatementCount(3));

                VerifyLabel(3, graph,
                    Predecessors(2),
                    Successors(Exit),
                    StatementCount(11));

                VerifyLabel(4, graph,
                    Predecessors(0, 1, 2),
                    Successors(Exit),
                    StatementCount(9));
            }
        }

        private static void VerifyLabel(int id, ControlFlowGraph graph, params Action<ControlFlowGraph.Block>[] verifiers)
        {
            var label = graph.GetBlock(new BlockId(id));

            Assert.False(label.IsEntry);
            Assert.False(label.IsExit);

            foreach (var verifier in verifiers)
            {
                verifier(label);
            }
        }

        private struct ExpectedBlockId
        {
            public BlockId BlockId { get; }

            public ExpectedBlockId(BlockId blockId)
            {
                this.BlockId = blockId;
            }

            public static implicit operator ExpectedBlockId(int number)
                => new ExpectedBlockId(new BlockId(number));

            public static implicit operator ExpectedBlockId(BlockId blockId)
                => new ExpectedBlockId(blockId);
        }

        private static ExpectedBlockId Entry => BlockId.Entry;
        private static ExpectedBlockId Exit => BlockId.Exit;

        private static Action<BlockId> Block(int expectedId)
            => id => Assert.Equal(new BlockId(expectedId), id);

        private static Action<ControlFlowGraph.Block> Predecessors(params ExpectedBlockId[] expectedBlockIds)
        {
            return b =>
            {
                Assert.Equal(expectedBlockIds.Length, b.Predecessors.Count);

                for (int i = 0; i < expectedBlockIds.Length; i++)
                {
                    Assert.Equal(expectedBlockIds[i].BlockId, b.Predecessors[i]);
                }
            };
        }

        private static Action<ControlFlowGraph.Block> Successors(params ExpectedBlockId[] expectedBlockIds)
        {
            return b =>
            {
                Assert.Equal(expectedBlockIds.Length, b.Successors.Count);

                for (int i = 0; i < expectedBlockIds.Length; i++)
                {
                    Assert.Equal(expectedBlockIds[i].BlockId, b.Successors[i]);
                }
            };
        }

        private static Action<ControlFlowGraph.Block> StatementCount(int count)
        {
            return b =>
            {
                Assert.Equal(count, b.Statements.Count);
            };
        }
    }
}
