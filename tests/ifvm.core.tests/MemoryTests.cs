using System;
using Xunit;

namespace IFVM.Core.Tests
{
    public class MemoryTests
    {
        [Fact]
        public void create_with_null_fails()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var memory = Memory.Create(null);
            });
        }

        [Fact]
        public void create_with_byte_array_succeeds()
        {
            var memory = Memory.Create(new byte[0]);
            Assert.NotNull(memory);
        }

        [Fact]
        public void length_returns_zero_for_empty_byte_array()
        {
            var memory = Memory.Create(new byte[0]);
            Assert.Equal(0, memory.Length);
        }

        [Fact]
        public void length_returns_size_of_byte_array()
        {
            var memory = Memory.Create(new byte[] { 1, 2, 3, 4 });
            Assert.Equal(4, memory.Length);
        }

        [Fact]
        public void readbyte_with_zero_returns_first_byte()
        {
            var memory = Memory.Create(new byte[] { 1, 2, 3, 4 });
            var b = memory.ReadByte(0);
            Assert.Equal(1, b);
        }

        [Fact]
        public void readbyte_with_minus_one_throws()
        {
            var memory = Memory.Create(new byte[] { 1, 2, 3, 4 });

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var b = memory.ReadByte(-1);
            });
        }

        [Fact]
        public void readbyte_with_length_throws()
        {
            var memory = Memory.Create(new byte[] { 1, 2, 3, 4 });

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var b = memory.ReadByte(memory.Length);
            });
        }

        [Fact]
        public void readbyte_with_length_minus_one_returns_last_byte()
        {
            var memory = Memory.Create(new byte[] { 1, 2, 3, 4 });
            var b = memory.ReadByte(memory.Length - 1);
            Assert.Equal(4, b);
        }
    }
}
