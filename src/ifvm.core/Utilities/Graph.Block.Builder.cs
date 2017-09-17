using System;
using System.Collections.Immutable;

namespace IFVM.Utilities
{
    public abstract partial class Graph
    {
        public abstract partial class Block
        {
            public abstract class Builder
            {
                private readonly BlockId _id;
                private readonly ImmutableSortedSet<BlockId>.Builder _predecessors;
                private readonly ImmutableSortedSet<BlockId>.Builder _successors;

                protected Builder(BlockId id)
                {
                    _id = id;
                    _predecessors = ImmutableSortedSet.CreateBuilder<BlockId>();
                    _successors = ImmutableSortedSet.CreateBuilder<BlockId>();
                }

                protected abstract Block ToBlock(
                    BlockId id, ImmutableSortedSet<BlockId> predecssors, ImmutableSortedSet<BlockId> successors);

                public Block ToBlock()
                    => ToBlock(_id, _predecessors.ToImmutable(), _successors.ToImmutable());

                public void AddPredecessor(BlockId id)
                {
                    if (_successors.Contains(id))
                    {
                        throw new ArgumentException($"Can't add block as both successor and predecessor");
                    }

                    _predecessors.Add(id);
                }

                public void AddSuccessor(BlockId id)
                {
                    if (_predecessors.Contains(id))
                    {
                        throw new ArgumentException($"Can't add block as both successor and predecessor");
                    }

                    _successors.Add(id);
                }
            }
        }
    }
}