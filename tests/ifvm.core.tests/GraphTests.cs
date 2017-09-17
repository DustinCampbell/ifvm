using System;
using IFVM.Collections;
using Xunit;

namespace IFVM.Core.Tests
{
    public class GraphTests
    {
        [Fact]
        public void entry_block_id_is_less_than_exit_block_id()
        {
            Assert.True(BlockId.Entry < BlockId.Exit);
        }

        [Fact]
        public void exit_block_id_is_greater_than_entry_block_id()
        {
            Assert.True(BlockId.Exit > BlockId.Entry);
        }

        [Fact]
        public void calling_getnext_on_entry_block_id_throws()
        {
            Assert.Throws<InvalidOperationException>(() => BlockId.Entry.GetNext());
        }

        [Fact]
        public void calling_getnext_on_exit_block_id_throws()
        {
            Assert.Throws<InvalidOperationException>(() => BlockId.Exit.GetNext());
        }

        [Fact]
        public void block_id_is_equal_to_itself()
        {
            Assert.Equal(BlockId.Initial, BlockId.Initial);
        }

        [Fact]
        public void block_id_is_not_equal_to_next_block_id()
        {
            Assert.NotEqual(BlockId.Initial, BlockId.Initial.GetNext());
        }

        [Fact]
        public void block_id_is_less_than_next_block_id()
        {
            Assert.True(BlockId.Initial < BlockId.Initial.GetNext());
        }

        [Fact]
        public void block_id_is_less_than_or_equal_to_next_block_id()
        {
            Assert.True(BlockId.Initial <= BlockId.Initial.GetNext());
        }

        [Fact]
        public void next_block_id_is_greater_than_block_id()
        {
            Assert.True(BlockId.Initial.GetNext() > BlockId.Initial);
        }

        [Fact]
        public void next_block_id_is_greater_than_or_equal_to_block_id()
        {
            Assert.True(BlockId.Initial.GetNext() >= BlockId.Initial);
        }
    }
}
