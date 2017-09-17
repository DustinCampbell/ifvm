using System.Collections.Generic;
using System.Collections.Immutable;

namespace IFVM.Collections
{
    public partial class Graph
    {
        public abstract class Builder<TBlock, TBlockBuilder>
            where TBlock : Block
            where TBlockBuilder : Block.Builder
        {
            private readonly Dictionary<BlockId, TBlockBuilder> _builders;

            public Builder()
            {
                _builders = new Dictionary<BlockId, TBlockBuilder>();

                AddNode(BlockId.Entry);
                AddNode(BlockId.Exit);
            }

            protected abstract TBlockBuilder CreateBlockBuilder(BlockId id);

            public TBlockBuilder GetBlockBuilder(BlockId id)
            {
                return _builders[id];
            }

            public void AddNode(BlockId id)
            {
                var blockBuilder = CreateBlockBuilder(id);
                _builders.Add(id, blockBuilder);
            }

            public void AddEdge(BlockId id1, BlockId id2)
            {
                var blockBuilder1 = _builders[id1];
                var blockBuilder2 = _builders[id2];

                blockBuilder1.AddSuccessor(id2);
                blockBuilder2.AddPredecessor(id1);
            }

            public ImmutableList<TBlock> ToBlocks()
            {
                var listBuilder = ImmutableList.CreateBuilder<TBlock>();

                foreach (var pair in _builders)
                {
                    listBuilder.Add((TBlock)pair.Value.ToBlock());
                }

                return listBuilder.ToImmutable();
            }
        }
    }
}