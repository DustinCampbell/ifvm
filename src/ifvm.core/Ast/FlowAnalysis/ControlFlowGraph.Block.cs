using System.Collections.Immutable;
using IFVM.Utilities;

namespace IFVM.Ast.FlowAnalysis
{
    public partial class ControlFlowGraph
    {
        new public class Block : Graph.Block
        {
            public ImmutableList<AstStatement> Statements { get; }

            private Block(
                Graph.BlockId id,
                ImmutableSortedSet<BlockId> predecessors,
                ImmutableSortedSet<BlockId> successors,
                ImmutableList<AstStatement> statements)
                : base(id, predecessors, successors)
            {
                Statements = statements;
            }

            new public class Builder : Graph.Block.Builder
            {
                private readonly ImmutableList<AstStatement>.Builder _statements;

                public Builder(BlockId id) : base(id)
                {
                    _statements = ImmutableList.CreateBuilder<AstStatement>();
                }

                protected override Graph.Block ToBlock(
                    BlockId id, ImmutableSortedSet<BlockId> predecssors, ImmutableSortedSet<BlockId> successors)
                {
                    return new Block(id, predecssors, successors, _statements.ToImmutable());
                }

                public void AddStatement(AstStatement statement)
                {
                    _statements.Add(statement);
                }
            }
        }
    }
}
