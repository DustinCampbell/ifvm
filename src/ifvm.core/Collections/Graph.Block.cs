using System.Collections.Immutable;

namespace IFVM.Collections
{
    public partial class Graph
    {
        public abstract partial class Block
        {
            public BlockId ID { get; }
            public ImmutableSortedSet<BlockId> Predecessors { get; }
            public ImmutableSortedSet<BlockId> Successors { get; }

            public bool IsEntry => ID == BlockId.Entry;
            public bool IsExit => ID == BlockId.Exit;

            protected Block(BlockId id, ImmutableSortedSet<BlockId> predecessors, ImmutableSortedSet<BlockId> successors)
            {
                this.ID = id;
                this.Predecessors = predecessors;
                this.Successors = successors;
            }
        }
    }
}
