using System;
using IFVM.Collections;

namespace IFVM.Ast.FlowAnalysis
{
    public partial class ControlFlowGraph : Graph<ControlFlowGraph.Builder, ControlFlowGraph.Block, ControlFlowGraph.Block.Builder>
    {
        public AstBody Body { get; }

        private ControlFlowGraph(AstBody body)
        {
            this.Body = body;
        }

        public static ControlFlowGraph Compute(AstBody body)
        {
            var graph = new ControlFlowGraph(body);
            graph.Compute();

            return graph;
        }

        private static BlockId LabelToBlockId(AstLabel label)
            => new BlockId(label.Index);

        private static bool ContinuesToNextBlock(AstStatement statement)
            => statement.Kind != AstNodeKind.JumpStatement
            && statement.Kind != AstNodeKind.ReturnStatement
            && statement.Kind != AstNodeKind.QuitStatement;

        protected override Builder CreateGraphBuilder()
        {
            return new Builder();
        }

        protected override void BuildBlocks(Builder builder)
        {
            // First, add a block for each label statement
            foreach (var statement in Body.Statements)
            {
                if (statement.Kind == AstNodeKind.LabelStatement)
                {
                    var labelStatement = (AstLabelStatement)statement;
                    var blockId = LabelToBlockId(labelStatement.Label);
                    builder.AddNode(blockId);
                }
            }

            var currentBlockId = BlockId.Entry;
            AstStatement previousStatement = null;

            // Next, add statements to the blocks and all edges
            foreach (var statement in Body.Statements)
            {
                switch (statement.Kind)
                {
                    case AstNodeKind.LabelStatement:
                        {
                            var labelStatement = (AstLabelStatement)statement;
                            var labelBlockId = LabelToBlockId(labelStatement.Label);

                            // Add edge if previous statement is null to ensure that an
                            // edge is created between the entry block and the first block.
                            if (previousStatement == null ||
                                ContinuesToNextBlock(previousStatement))
                            {
                                builder.AddEdge(currentBlockId, labelBlockId);
                            }

                            currentBlockId = labelBlockId;
                        }

                        break;

                    case AstNodeKind.JumpStatement:
                        {
                            var jumpStatement = (AstJumpStatement)statement;
                            var jumpBlockId = LabelToBlockId(jumpStatement.Label);

                            builder.AddEdge(currentBlockId, jumpBlockId);
                        }

                        break;

                    case AstNodeKind.BranchStatement:
                        {
                            var branchStatement = (AstBranchStatement)statement;

                            // Branches can contain a jump or return statement.
                            switch (branchStatement.Statement.Kind)
                            {
                                case AstNodeKind.JumpStatement:
                                    {
                                        var jumpStatement = (AstJumpStatement)branchStatement.Statement;
                                        var jumpBlockId = LabelToBlockId(jumpStatement.Label);

                                        builder.AddEdge(currentBlockId, jumpBlockId);
                                    }

                                    break;

                                case AstNodeKind.ReturnStatement:
                                    {
                                        builder.AddEdge(currentBlockId, BlockId.Exit);
                                    }

                                    break;

                                default:
                                    throw new InvalidOperationException();
                            }
                        }

                        break;

                    case AstNodeKind.ReturnStatement:
                    case AstNodeKind.QuitStatement:
                        builder.AddEdge(currentBlockId, BlockId.Exit);
                        break;
                }

                var blockBuilder = builder.GetBlockBuilder(currentBlockId);
                blockBuilder.AddStatement(statement);

                previousStatement = statement;
            }
        }
    }
}
