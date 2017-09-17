﻿using System;
using System.Collections.Immutable;

namespace IFVM.Utilities
{
    public abstract class Graph<TBuilder, TBlock, TBlockBuilder> : Graph
        where TBlock : Graph.Block
        where TBuilder : Graph.Builder<TBlock, TBlockBuilder>
        where TBlockBuilder : Graph.Block.Builder
    {
        private ImmutableList<TBlock> _blocks;
        private ImmutableDictionary<BlockId, TBlock> _idToBlockMap;

        protected Graph()
        {
        }

        protected void Compute()
        {
            var graphBuilder = CreateGraphBuilder();
            BuildBlocks(graphBuilder);

            _blocks = graphBuilder.ToBlocks();
            _idToBlockMap = _blocks.ToImmutableDictionary(keySelector: b => b.ID);
        }

        protected abstract TBuilder CreateGraphBuilder();
        protected abstract void BuildBlocks(TBuilder builder);
    }
}
