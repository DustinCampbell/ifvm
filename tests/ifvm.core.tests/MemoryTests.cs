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
            Assert.Equal(0, memory.Size);
        }

        [Fact]
        public void length_returns_size_of_byte_array()
        {
            var memory = Memory.Create(new byte[] { 1, 2, 3, 4 });
            Assert.Equal(4, memory.Size);
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
                memory.ReadByte(-1);
            });
        }

        [Fact]
        public void readbyte_with_length_throws()
        {
            var memory = Memory.Create(new byte[] { 1, 2, 3, 4 });

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                memory.ReadByte(memory.Size);
            });
        }

        [Fact]
        public void readbyte_with_length_minus_one_returns_last_byte()
        {
            var memory = Memory.Create(new byte[] { 1, 2, 3, 4 });
            var b = memory.ReadByte(memory.Size - 1);
            Assert.Equal(4, b);
        }

        [Fact]
        public void writebyte_with_zero_sets_first_byte()
        {
            var memory = Memory.Create(new byte[] { 1, 2, 3, 4 });

            var b1 = memory.ReadByte(0);
            Assert.Equal(1, b1);

            memory.WriteByte(0, 5);
            var b2 = memory.ReadByte(0);
            Assert.Equal(5, b2);
        }

        [Fact]
        public void writebyte_with_minus_one_throws()
        {
            var memory = Memory.Create(new byte[] { 1, 2, 3, 4 });

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                memory.WriteByte(-1, 5);
            });
        }

        [Fact]
        public void writebyte_with_length_throws()
        {
            var memory = Memory.Create(new byte[] { 1, 2, 3, 4 });

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                memory.WriteByte(memory.Size, 5);
            });
        }

        [Fact]
        public void writebyte_with_length_minus_one_returns_last_byte()
        {
            var memory = Memory.Create(new byte[] { 1, 2, 3, 4 });

            var b1 = memory.ReadByte(memory.Size - 1);
            Assert.Equal(4, b1);

            memory.WriteByte(memory.Size - 1, 5);
            var b2 = memory.ReadByte(memory.Size - 1);
            Assert.Equal(5, b2);
        }

        [Fact]
        public void writebyte_before_rom_start_succeeds()
        {
            var memory = Memory.Create(new byte[] { 1, 2, 3, 4, 5 });
            memory.AddReadOnlyRegion(1, 3);

            memory.WriteByte(0, 6);
            var b = memory.ReadByte(0);
            Assert.Equal(6, b);
        }

        [Fact]
        public void writebyte_at_rom_start_fails()
        {
            var memory = Memory.Create(new byte[] { 1, 2, 3, 4, 5 });
            memory.AddReadOnlyRegion(1, 3);

            Assert.Throws<InvalidOperationException>(() =>
            {
                memory.WriteByte(1, 6);
            });
        }

        [Fact]
        public void writebyte_before_rom_end_fails()
        {
            var memory = Memory.Create(new byte[] { 1, 2, 3, 4, 5 });
            memory.AddReadOnlyRegion(1, 3);

            Assert.Throws<InvalidOperationException>(() =>
            {
                memory.WriteByte(3, 6);
            });
        }

        [Fact]
        public void writebyte_after_rom_end_succeeds()
        {
            var memory = Memory.Create(new byte[] { 1, 2, 3, 4, 5 });
            memory.AddReadOnlyRegion(1, 3);

            memory.WriteByte(4, 6);
            var b = memory.ReadByte(4);
            Assert.Equal(6, b);
        }

        [Fact]
        public void readword_with_zero_returns_first_word()
        {
            var memory = Memory.Create(new byte[] { 0x12, 0x34, 0x56, 0x78 });
            var w = memory.ReadWord(0);
            Assert.Equal(0x1234, w);
        }

        [Fact]
        public void readword_with_minus_one_throws()
        {
            var memory = Memory.Create(new byte[] { 0x12, 0x34, 0x56, 0x78 });

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                memory.ReadWord(-1);
            });
        }

        [Fact]
        public void readword_with_length_throws()
        {
            var memory = Memory.Create(new byte[] { 0x12, 0x34, 0x56, 0x78 });

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                memory.ReadWord(memory.Size);
            });
        }

        [Fact]
        public void readword_with_length_minus_one_throws()
        {
            var memory = Memory.Create(new byte[] { 0x12, 0x34, 0x56, 0x78 });

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                memory.ReadWord(memory.Size - 1);
            });
        }

        [Fact]
        public void readword_with_length_minus_two_returns_last_word()
        {
            var memory = Memory.Create(new byte[] { 0x12, 0x34, 0x56, 0x78 });
            var w = memory.ReadWord(memory.Size - 2);
            Assert.Equal(0x5678, w);
        }

        [Fact]
        public void writeword_with_zero_sets_first_word()
        {
            var memory = Memory.Create(new byte[] { 0x12, 0x34, 0x56, 0x78 });

            var w1 = memory.ReadWord(0);
            Assert.Equal(0x1234, w1);

            memory.WriteWord(0, 0x4321);
            var w2 = memory.ReadWord(0);
            Assert.Equal(0x4321, w2);
        }

        [Fact]
        public void writeword_with_minus_one_throws()
        {
            var memory = Memory.Create(new byte[] { 0x12, 0x34, 0x56, 0x78 });

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                memory.WriteWord(-1, 0x4321);
            });
        }

        [Fact]
        public void writeword_with_length_throws()
        {
            var memory = Memory.Create(new byte[] { 0x12, 0x34, 0x56, 0x78 });

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                memory.WriteWord(memory.Size, 0x8765);
            });
        }

        [Fact]
        public void writeword_with_length_minus_one_throws()
        {
            var memory = Memory.Create(new byte[] { 0x12, 0x34, 0x56, 0x78 });

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                memory.WriteWord(memory.Size - 1, 0x8765);
            });
        }

        [Fact]
        public void writeword_with_length_minus_two_returns_last_word()
        {
            var memory = Memory.Create(new byte[] { 0x12, 0x34, 0x56, 0x78 });

            var w1 = memory.ReadWord(memory.Size - 2);
            Assert.Equal(0x5678, w1);

            memory.WriteWord(memory.Size - 2, 0x7865);
            var w2 = memory.ReadWord(memory.Size - 2);
            Assert.Equal(0x7865, w2);
        }

        [Fact]
        public void writeword_before_rom_start_succeeds()
        {
            var memory = Memory.Create(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 });
            memory.AddReadOnlyRegion(2, 4);

            memory.WriteWord(0, 0x4321);
            var w = memory.ReadWord(0);
            Assert.Equal(0x4321, w);
        }

        [Fact]
        public void writeword_overlapping_rom_start_fails()
        {
            var memory = Memory.Create(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 });
            memory.AddReadOnlyRegion(2, 4);

            Assert.Throws<InvalidOperationException>(() =>
            {
                memory.WriteWord(1, 0x4321);
            });
        }

        [Fact]
        public void writeword_at_rom_start_fails()
        {
            var memory = Memory.Create(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 });
            memory.AddReadOnlyRegion(2, 4);

            Assert.Throws<InvalidOperationException>(() =>
            {
                memory.WriteWord(2, 0x4321);
            });
        }

        [Fact]
        public void writeword_before_rom_end_fails()
        {
            var memory = Memory.Create(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 });
            memory.AddReadOnlyRegion(2, 4);

            Assert.Throws<InvalidOperationException>(() =>
            {
                memory.WriteWord(4, 0x4321);
            });
        }

        [Fact]
        public void writeword_overlapping_rom_end_fails()
        {
            var memory = Memory.Create(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 });
            memory.AddReadOnlyRegion(2, 4);

            Assert.Throws<InvalidOperationException>(() =>
            {
                memory.WriteWord(5, 0x4321);
            });
        }

        [Fact]
        public void writeword_after_rom_end_succeeds()
        {
            var memory = Memory.Create(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 });
            memory.AddReadOnlyRegion(2, 4);

            memory.WriteWord(6, 0x4321);
            var w = memory.ReadWord(6);
            Assert.Equal(0x4321, w);
        }

        [Fact]
        public void readdword_with_zero_returns_first_dword()
        {
            var memory = Memory.Create(new byte[] { 0x12, 0x34, 0x56, 0x78, 0x9a, 0xbc, 0xde, 0xf0 });
            var d2 = memory.ReadDWord(0);
            Assert.Equal(0x12345678u, d2);
        }

        [Fact]
        public void readdword_with_minus_one_throws()
        {
            var memory = Memory.Create(new byte[] { 0x12, 0x34, 0x56, 0x78, 0x9a, 0xbc, 0xde, 0xf0 });

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                memory.ReadDWord(-1);
            });
        }

        [Fact]
        public void readdword_with_length_throws()
        {
            var memory = Memory.Create(new byte[] { 0x12, 0x34, 0x56, 0x78, 0x9a, 0xbc, 0xde, 0xf0 });

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                memory.ReadDWord(memory.Size);
            });
        }

        [Fact]
        public void readdword_with_length_minus_one_throws()
        {
            var memory = Memory.Create(new byte[] { 0x12, 0x34, 0x56, 0x78, 0x9a, 0xbc, 0xde, 0xf0 });

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                memory.ReadDWord(memory.Size - 1);
            });
        }

        [Fact]
        public void readdword_with_length_minus_two_throws()
        {
            var memory = Memory.Create(new byte[] { 0x12, 0x34, 0x56, 0x78, 0x9a, 0xbc, 0xde, 0xf0 });

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                memory.ReadDWord(memory.Size - 2);
            });
        }

        [Fact]
        public void readdword_with_length_minus_three_throws()
        {
            var memory = Memory.Create(new byte[] { 0x12, 0x34, 0x56, 0x78, 0x9a, 0xbc, 0xde, 0xf0 });

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                memory.ReadDWord(memory.Size - 3);
            });
        }

        [Fact]
        public void readdword_with_length_minus_four_returns_last_dword()
        {
            var memory = Memory.Create(new byte[] { 0x12, 0x34, 0x56, 0x78, 0x9a, 0xbc, 0xde, 0xf0 });
            var dw = memory.ReadDWord(memory.Size - 4);
            Assert.Equal(0x9abcdef0u, dw);
        }

        [Fact]
        public void writedword_with_zero_sets_first_dword()
        {
            var memory = Memory.Create(new byte[] { 0x12, 0x34, 0x56, 0x78, 0x9a, 0xbc, 0xde, 0xf0 });

            var dw1 = memory.ReadDWord(0);
            Assert.Equal(0x12345678u, dw1);

            memory.WriteDWord(0, 0x87654321u);
            var dw2 = memory.ReadDWord(0);
            Assert.Equal(0x87654321u, dw2);
        }

        [Fact]
        public void writedword_with_minus_one_throws()
        {
            var memory = Memory.Create(new byte[] { 0x12, 0x34, 0x56, 0x78, 0x9a, 0xbc, 0xde, 0xf0 });

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                memory.WriteDWord(-1, 0x87654321u);
            });
        }

        [Fact]
        public void writedword_with_length_throws()
        {
            var memory = Memory.Create(new byte[] { 0x12, 0x34, 0x56, 0x78, 0x9a, 0xbc, 0xde, 0xf0 });

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                memory.WriteDWord(memory.Size, 0x0fedcba9u);
            });
        }

        [Fact]
        public void writedword_with_length_minus_one_throws()
        {
            var memory = Memory.Create(new byte[] { 0x12, 0x34, 0x56, 0x78, 0x9a, 0xbc, 0xde, 0xf0 });

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                memory.WriteDWord(memory.Size - 1, 0x0fedcba9u);
            });
        }

        [Fact]
        public void writedword_with_length_minus_two_throws()
        {
            var memory = Memory.Create(new byte[] { 0x12, 0x34, 0x56, 0x78, 0x9a, 0xbc, 0xde, 0xf0 });

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                memory.WriteDWord(memory.Size - 2, 0x0fedcba9u);
            });
        }

        [Fact]
        public void writedword_with_length_minus_three_throws()
        {
            var memory = Memory.Create(new byte[] { 0x12, 0x34, 0x56, 0x78, 0x9a, 0xbc, 0xde, 0xf0 });

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                memory.WriteDWord(memory.Size - 3, 0x0fedcba9u);
            });
        }

        [Fact]
        public void writedword_with_length_minus_four_returns_last_dword()
        {
            var memory = Memory.Create(new byte[] { 0x12, 0x34, 0x56, 0x78, 0x9a, 0xbc, 0xde, 0xf0 });

            var dw1 = memory.ReadDWord(memory.Size - 4);
            Assert.Equal(0x9abcdef0u, dw1);

            memory.WriteDWord(memory.Size - 4, 0x0fedcba9u);
            var dw2 = memory.ReadDWord(memory.Size - 4);
            Assert.Equal(0x0fedcba9u, dw2);
        }

        [Fact]
        public void writedword_before_rom_start_succeeds()
        {
            var memory = Memory.Create(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 });
            memory.AddReadOnlyRegion(4, 8);

            memory.WriteDWord(0, 0x87654321);
            var dw = memory.ReadDWord(0);
            Assert.Equal(0x87654321, dw);
        }

        [Fact]
        public void writedword_overlapping_rom_start_fails_1()
        {
            var memory = Memory.Create(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 });
            memory.AddReadOnlyRegion(4, 8);

            Assert.Throws<InvalidOperationException>(() =>
            {
                memory.WriteDWord(1, 0x87654321);
            });
        }

        [Fact]
        public void writedword_overlapping_rom_start_fails_2()
        {
            var memory = Memory.Create(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 });
            memory.AddReadOnlyRegion(4, 8);

            Assert.Throws<InvalidOperationException>(() =>
            {
                memory.WriteDWord(2, 0x87654321);
            });
        }

        [Fact]
        public void writedword_overlapping_rom_start_fails_3()
        {
            var memory = Memory.Create(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 });
            memory.AddReadOnlyRegion(4, 8);

            Assert.Throws<InvalidOperationException>(() =>
            {
                memory.WriteDWord(3, 0x87654321);
            });
        }

        [Fact]
        public void writedword_at_rom_start_fails()
        {
            var memory = Memory.Create(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 });
            memory.AddReadOnlyRegion(4, 8);

            Assert.Throws<InvalidOperationException>(() =>
            {
                memory.WriteDWord(4, 0x87654321);
            });
        }

        [Fact]
        public void writedword_before_rom_end_fails()
        {
            var memory = Memory.Create(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 });
            memory.AddReadOnlyRegion(4, 8);

            Assert.Throws<InvalidOperationException>(() =>
            {
                memory.WriteDWord(8, 0x87654321);
            });
        }

        [Fact]
        public void writedword_overlapping_rom_end_fails_1()
        {
            var memory = Memory.Create(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 });
            memory.AddReadOnlyRegion(4, 8);

            Assert.Throws<InvalidOperationException>(() =>
            {
                memory.WriteDWord(9, 0x87654321);
            });
        }

        [Fact]
        public void writedword_overlapping_rom_end_fails_2()
        {
            var memory = Memory.Create(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 });
            memory.AddReadOnlyRegion(4, 8);

            Assert.Throws<InvalidOperationException>(() =>
            {
                memory.WriteDWord(10, 0x87654321);
            });
        }

        [Fact]
        public void writedword_overlapping_rom_end_fails_3()
        {
            var memory = Memory.Create(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 });
            memory.AddReadOnlyRegion(4, 8);

            Assert.Throws<InvalidOperationException>(() =>
            {
                memory.WriteDWord(11, 0x87654321);
            });
        }

        [Fact]
        public void writedword_after_rom_end_succeeds()
        {
            var memory = Memory.Create(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 });
            memory.AddReadOnlyRegion(4, 8);

            memory.WriteDWord(12, 0x87654321);
            var dw = memory.ReadDWord(12);
            Assert.Equal(0x87654321, dw);
        }
    }
}
