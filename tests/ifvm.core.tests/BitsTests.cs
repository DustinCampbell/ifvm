using System;
using Xunit;

namespace IFVM.Core.Tests
{
    public class BitsTests
    {
        private const int ByteSize = 8;

        [Fact]
        public void isset_throws_with_byte_and_minus_one()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                byte value = 0;
                Bits.IsSet(value, -1);
            });
        }

        [Fact]
        public void isset_throws_with_byte_and_sizeof_byte()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                byte value = 0;
                Bits.IsSet(value, sizeof(byte) * ByteSize);
            });
        }

        [Fact]
        public void isset_can_verify_each_bit_in_a_byte()
        {
            byte value = 0b1111_1111;

            for (int i = 0; i < sizeof(byte) * ByteSize; i++)
            {
                Assert.True(Bits.IsSet(value, i));
            }
        }

        [Fact]
        public void isset_can_verify_each_bit_in_a_byte2()
        {
            byte value = 0b0000_0000;

            for (int i = 0; i < sizeof(byte) * ByteSize; i++)
            {
                Assert.False(Bits.IsSet(value, i));
            }
        }

        [Fact]
        public void isset_can_verify_each_bit_in_a_byte3()
        {
            byte value = 0b1010_1010;

            // from least significant (rightmost) to most significant (leftmost)
            var expected = new[] { false, true, false, true, false, true, false, true };

            for (int i = 0; i < sizeof(byte) * ByteSize; i++)
            {
                Assert.Equal(expected[i], Bits.IsSet(value, i));
            }
        }

        [Fact]
        public void isset_can_verify_each_bit_in_a_byte4()
        {
            byte value = 0b0101_0101;

            // from least significant (rightmost) to most significant (leftmost)
            var expected = new[] { true, false, true, false, true, false, true, false };

            for (int i = 0; i < sizeof(byte) * ByteSize; i++)
            {
                Assert.Equal(expected[i], Bits.IsSet(value, i));
            }
        }

        [Fact]
        public void isset_throws_with_ushort_and_minus_one()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                ushort value = 0;
                Bits.IsSet(value, -1);
            });
        }

        [Fact]
        public void isset_throws_with_ushort_and_sizeof_ushort()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                ushort value = 0;
                Bits.IsSet(value, sizeof(ushort) * ByteSize);
            });
        }

        [Fact]
        public void isset_can_verify_each_bit_in_a_ushort()
        {
            ushort value = 0b1111_1111_1111_1111;

            for (int i = 0; i < sizeof(ushort) * ByteSize; i++)
            {
                Assert.True(Bits.IsSet(value, i));
            }
        }

        [Fact]
        public void isset_can_verify_each_bit_in_a_ushort2()
        {
            ushort value = 0b0000_0000_0000_0000;

            for (int i = 0; i < sizeof(ushort) * ByteSize; i++)
            {
                Assert.False(Bits.IsSet(value, i));
            }
        }

        [Fact]
        public void isset_can_verify_each_bit_in_a_ushort3()
        {
            ushort value = 0b1010_1010_1010_1010;

            // from least significant (rightmost) to most significant (leftmost)
            var expected = new[]
            {
                false, true, false, true, false, true, false, true,
                false, true, false, true, false, true, false, true
            };

            for (int i = 0; i < sizeof(ushort) * ByteSize; i++)
            {
                Assert.Equal(expected[i], Bits.IsSet(value, i));
            }
        }

        [Fact]
        public void isset_can_verify_each_bit_in_a_ushort4()
        {
            ushort value = 0b0101_0101_0101_0101;

            // from least significant (rightmost) to most significant (leftmost)
            var expected = new[]
            {
                true, false, true, false, true, false, true, false,
                true, false, true, false, true, false, true, false
            };

            for (int i = 0; i < sizeof(ushort) * ByteSize; i++)
            {
                Assert.Equal(expected[i], Bits.IsSet(value, i));
            }
        }

        [Fact]
        public void isset_throws_with_uint_and_minus_one()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                uint value = 0;
                Bits.IsSet(value, -1);
            });
        }

        [Fact]
        public void isset_throws_with_uint_and_sizeof_uint()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                uint value = 0;
                Bits.IsSet(value, sizeof(uint) * ByteSize);
            });
        }

        [Fact]
        public void isset_can_verify_each_bit_in_a_uint()
        {
            uint value = 0b1111_1111_1111_1111_1111_1111_1111_1111;

            for (int i = 0; i < sizeof(uint) * ByteSize; i++)
            {
                Assert.True(Bits.IsSet(value, i));
            }
        }

        [Fact]
        public void isset_can_verify_each_bit_in_a_uint2()
        {
            uint value = 0b0000_0000_0000_0000_0000_0000_0000_0000;

            for (int i = 0; i < sizeof(uint) * ByteSize; i++)
            {
                Assert.False(Bits.IsSet(value, i));
            }
        }

        [Fact]
        public void isset_can_verify_each_bit_in_a_uint3()
        {
            uint value = 0b1010_1010_1010_1010_1010_1010_1010_1010;

            // from least significant (rightmost) to most significant (leftmost)
            var expected = new[]
            {
                false, true, false, true, false, true, false, true,
                false, true, false, true, false, true, false, true,
                false, true, false, true, false, true, false, true,
                false, true, false, true, false, true, false, true
            };

            for (int i = 0; i < sizeof(uint) * ByteSize; i++)
            {
                Assert.Equal(expected[i], Bits.IsSet(value, i));
            }
        }

        [Fact]
        public void isset_can_verify_each_bit_in_a_uint4()
        {
            uint value = 0b0101_0101_0101_0101_0101_0101_0101_0101;

            // from least significant (rightmost) to most significant (leftmost)
            var expected = new[]
            {
                true, false, true, false, true, false, true, false,
                true, false, true, false, true, false, true, false,
                true, false, true, false, true, false, true, false,
                true, false, true, false, true, false, true, false
            };

            for (int i = 0; i < sizeof(uint) * ByteSize; i++)
            {
                Assert.Equal(expected[i], Bits.IsSet(value, i));
            }
        }

        [Fact]
        public void isset_throws_with_ulong_and_minus_one()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                ulong value = 0;
                Bits.IsSet(value, -1);
            });
        }

        [Fact]
        public void isset_throws_with_ulong_and_sizeof_ulong()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                ulong value = 0;
                Bits.IsSet(value, sizeof(ulong) * ByteSize);
            });
        }

        [Fact]
        public void isset_can_verify_each_bit_in_a_ulong()
        {
            ulong value = 0b1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111;

            for (int i = 0; i < sizeof(ulong) * ByteSize; i++)
            {
                Assert.True(Bits.IsSet(value, i));
            }
        }

        [Fact]
        public void isset_can_verify_each_bit_in_a_ulong2()
        {
            ulong value = 0b0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000;

            for (int i = 0; i < sizeof(ulong) * ByteSize; i++)
            {
                Assert.False(Bits.IsSet(value, i));
            }
        }

        [Fact]
        public void isset_can_verify_each_bit_in_a_ulong3()
        {
            ulong value = 0b1010_1010_1010_1010_1010_1010_1010_1010_1010_1010_1010_1010_1010_1010_1010_1010;

            // from least significant (rightmost) to most significant (leftmost)
            var expected = new[]
            {
                false, true, false, true, false, true, false, true,
                false, true, false, true, false, true, false, true,
                false, true, false, true, false, true, false, true,
                false, true, false, true, false, true, false, true,
                false, true, false, true, false, true, false, true,
                false, true, false, true, false, true, false, true,
                false, true, false, true, false, true, false, true,
                false, true, false, true, false, true, false, true
            };

            for (int i = 0; i < sizeof(ulong) * ByteSize; i++)
            {
                Assert.Equal(expected[i], Bits.IsSet(value, i));
            }
        }

        [Fact]
        public void isset_can_verify_each_bit_in_a_ulong4()
        {
            ulong value = 0b0101_0101_0101_0101_0101_0101_0101_0101_0101_0101_0101_0101_0101_0101_0101_0101;

            // from least significant (rightmost) to most significant (leftmost)
            var expected = new[]
            {
                true, false, true, false, true, false, true, false,
                true, false, true, false, true, false, true, false,
                true, false, true, false, true, false, true, false,
                true, false, true, false, true, false, true, false,
                true, false, true, false, true, false, true, false,
                true, false, true, false, true, false, true, false,
                true, false, true, false, true, false, true, false,
                true, false, true, false, true, false, true, false
            };

            for (int i = 0; i < sizeof(ulong) * ByteSize; i++)
            {
                Assert.Equal(expected[i], Bits.IsSet(value, i));
            }
        }

        [Fact]
        public void isclear_throws_with_byte_and_minus_one()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                byte value = 0;
                Bits.IsClear(value, -1);
            });
        }

        [Fact]
        public void isclear_throws_with_byte_and_sizeof_byte()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                byte value = 0;
                Bits.IsClear(value, sizeof(byte) * ByteSize);
            });
        }

        [Fact]
        public void isclear_can_verify_each_bit_in_a_byte()
        {
            byte value = 0b1111_1111;

            for (int i = 0; i < sizeof(byte) * ByteSize; i++)
            {
                Assert.False(Bits.IsClear(value, i));
            }
        }

        [Fact]
        public void isclear_can_verify_each_bit_in_a_byte2()
        {
            byte value = 0b0000_0000;

            for (int i = 0; i < sizeof(byte) * ByteSize; i++)
            {
                Assert.True(Bits.IsClear(value, i));
            }
        }

        [Fact]
        public void isclear_can_verify_each_bit_in_a_byte3()
        {
            byte value = 0b1010_1010;

            // from least significant (rightmost) to most significant (leftmost)
            var expected = new[] { true, false, true, false, true, false, true, false };

            for (int i = 0; i < sizeof(byte) * ByteSize; i++)
            {
                Assert.Equal(expected[i], Bits.IsClear(value, i));
            }
        }

        [Fact]
        public void isclear_can_verify_each_bit_in_a_byte4()
        {
            byte value = 0b0101_0101;

            // from least significant (rightmost) to most significant (leftmost)
            var expected = new[] { false, true, false, true, false, true, false, true };

            for (int i = 0; i < sizeof(byte) * ByteSize; i++)
            {
                Assert.Equal(expected[i], Bits.IsClear(value, i));
            }
        }

        [Fact]
        public void isclear_throws_with_ushort_and_minus_one()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                ushort value = 0;
                Bits.IsClear(value, -1);
            });
        }

        [Fact]
        public void isclear_throws_with_ushort_and_sizeof_ushort()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                ushort value = 0;
                Bits.IsClear(value, sizeof(ushort) * ByteSize);
            });
        }

        [Fact]
        public void isclear_can_verify_each_bit_in_a_ushort()
        {
            ushort value = 0b1111_1111_1111_1111;

            for (int i = 0; i < sizeof(ushort) * ByteSize; i++)
            {
                Assert.False(Bits.IsClear(value, i));
            }
        }

        [Fact]
        public void isclear_can_verify_each_bit_in_a_ushort2()
        {
            ushort value = 0b0000_0000_0000_0000;

            for (int i = 0; i < sizeof(ushort) * ByteSize; i++)
            {
                Assert.True(Bits.IsClear(value, i));
            }
        }

        [Fact]
        public void isclear_can_verify_each_bit_in_a_ushort3()
        {
            ushort value = 0b1010_1010_1010_1010;

            // from least significant (rightmost) to most significant (leftmost)
            var expected = new[]
            {
                true, false, true, false, true, false, true, false,
                true, false, true, false, true, false, true, false
            };

            for (int i = 0; i < sizeof(ushort) * ByteSize; i++)
            {
                Assert.Equal(expected[i], Bits.IsClear(value, i));
            }
        }

        [Fact]
        public void isclear_can_verify_each_bit_in_a_ushort4()
        {
            ushort value = 0b0101_0101_0101_0101;

            // from least significant (rightmost) to most significant (leftmost)
            var expected = new[]
            {
                false, true, false, true, false, true, false, true,
                false, true, false, true, false, true, false, true
            };

            for (int i = 0; i < sizeof(ushort) * ByteSize; i++)
            {
                Assert.Equal(expected[i], Bits.IsClear(value, i));
            }
        }

        [Fact]
        public void isclear_throws_with_uint_and_minus_one()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                uint value = 0;
                Bits.IsClear(value, -1);
            });
        }

        [Fact]
        public void isclear_throws_with_uint_and_sizeof_uint()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                uint value = 0;
                Bits.IsClear(value, sizeof(uint) * ByteSize);
            });
        }

        [Fact]
        public void isclear_can_verify_each_bit_in_a_uint()
        {
            uint value = 0b1111_1111_1111_1111_1111_1111_1111_1111;

            for (int i = 0; i < sizeof(uint) * ByteSize; i++)
            {
                Assert.False(Bits.IsClear(value, i));
            }
        }

        [Fact]
        public void isclear_can_verify_each_bit_in_a_uint2()
        {
            uint value = 0b0000_0000_0000_0000_0000_0000_0000_0000;

            for (int i = 0; i < sizeof(uint) * ByteSize; i++)
            {
                Assert.True(Bits.IsClear(value, i));
            }
        }

        [Fact]
        public void isclear_can_verify_each_bit_in_a_uint3()
        {
            uint value = 0b1010_1010_1010_1010_1010_1010_1010_1010;

            // from least significant (rightmost) to most significant (leftmost)
            var expected = new[]
            {
                true, false, true, false, true, false, true, false,
                true, false, true, false, true, false, true, false,
                true, false, true, false, true, false, true, false,
                true, false, true, false, true, false, true, false
            };

            for (int i = 0; i < sizeof(uint) * ByteSize; i++)
            {
                Assert.Equal(expected[i], Bits.IsClear(value, i));
            }
        }

        [Fact]
        public void isclear_can_verify_each_bit_in_a_uint4()
        {
            uint value = 0b0101_0101_0101_0101_0101_0101_0101_0101;

            // from least significant (rightmost) to most significant (leftmost)
            var expected = new[]
            {
                false, true, false, true, false, true, false, true,
                false, true, false, true, false, true, false, true,
                false, true, false, true, false, true, false, true,
                false, true, false, true, false, true, false, true
            };

            for (int i = 0; i < sizeof(uint) * ByteSize; i++)
            {
                Assert.Equal(expected[i], Bits.IsClear(value, i));
            }
        }

        [Fact]
        public void isclear_throws_with_ulong_and_minus_one()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                ulong value = 0;
                Bits.IsClear(value, -1);
            });
        }

        [Fact]
        public void isclear_throws_with_ulong_and_sizeof_ulong()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                ulong value = 0;
                Bits.IsClear(value, sizeof(ulong) * ByteSize);
            });
        }

        [Fact]
        public void isclear_can_verify_each_bit_in_a_ulong()
        {
            ulong value = 0b1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111;

            for (int i = 0; i < sizeof(ulong) * ByteSize; i++)
            {
                Assert.False(Bits.IsClear(value, i));
            }
        }

        [Fact]
        public void isclear_can_verify_each_bit_in_a_ulong2()
        {
            ulong value = 0b0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000;

            for (int i = 0; i < sizeof(ulong) * ByteSize; i++)
            {
                Assert.True(Bits.IsClear(value, i));
            }
        }

        [Fact]
        public void isclear_can_verify_each_bit_in_a_ulong3()
        {
            ulong value = 0b1010_1010_1010_1010_1010_1010_1010_1010_1010_1010_1010_1010_1010_1010_1010_1010;

            // from least significant (rightmost) to most significant (leftmost)
            var expected = new[]
            {
                true, false, true, false, true, false, true, false,
                true, false, true, false, true, false, true, false,
                true, false, true, false, true, false, true, false,
                true, false, true, false, true, false, true, false,
                true, false, true, false, true, false, true, false,
                true, false, true, false, true, false, true, false,
                true, false, true, false, true, false, true, false,
                true, false, true, false, true, false, true, false
            };

            for (int i = 0; i < sizeof(ulong) * ByteSize; i++)
            {
                Assert.Equal(expected[i], Bits.IsClear(value, i));
            }
        }

        [Fact]
        public void isclear_can_verify_each_bit_in_a_ulong4()
        {
            ulong value = 0b0101_0101_0101_0101_0101_0101_0101_0101_0101_0101_0101_0101_0101_0101_0101_0101;

            // from least significant (rightmost) to most significant (leftmost)
            var expected = new[]
            {
                false, true, false, true, false, true, false, true,
                false, true, false, true, false, true, false, true,
                false, true, false, true, false, true, false, true,
                false, true, false, true, false, true, false, true,
                false, true, false, true, false, true, false, true,
                false, true, false, true, false, true, false, true,
                false, true, false, true, false, true, false, true,
                false, true, false, true, false, true, false, true
            };

            for (int i = 0; i < sizeof(ulong) * ByteSize; i++)
            {
                Assert.Equal(expected[i], Bits.IsClear(value, i));
            }
        }

        [Fact]
        public void set_throws_with_byte_and_minus_one()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                byte value = 0;
                Bits.Set(value, -1);
            });
        }

        [Fact]
        public void set_throws_with_byte_and_sizeof_byte()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                byte value = 0;
                Bits.Set(value, sizeof(byte) * ByteSize);
            });
        }

        [Fact]
        public void set_byte_bits_and_verify_result()
        {
            byte value = 0b0000_0000;
            byte expected = 0b1111_1111;

            for (int i = 0; i < sizeof(byte) * ByteSize; i++)
            {
                Assert.False(Bits.IsSet(value, i));
                value = Bits.Set(value, i);
                Assert.True(Bits.IsSet(value, i));
            }

            Assert.Equal(expected, value);
        }

        [Fact]
        public void set_throws_with_ushort_and_minus_one()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                ushort value = 0;
                Bits.Set(value, -1);
            });
        }

        [Fact]
        public void set_throws_with_ushort_and_sizeof_ushort()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                ushort value = 0;
                Bits.Set(value, sizeof(ushort) * ByteSize);
            });
        }

        [Fact]
        public void set_ushort_bits_and_verify_result()
        {
            ushort value = 0b0000_0000_0000_0000;
            ushort expected = 0b1111_1111_1111_1111;

            for (int i = 0; i < sizeof(ushort) * ByteSize; i++)
            {
                Assert.False(Bits.IsSet(value, i));
                value = Bits.Set(value, i);
                Assert.True(Bits.IsSet(value, i));
            }

            Assert.Equal(expected, value);
        }

        [Fact]
        public void set_throws_with_uint_and_minus_one()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                uint value = 0;
                Bits.Set(value, -1);
            });
        }

        [Fact]
        public void set_throws_with_uint_and_sizeof_uint()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                uint value = 0;
                Bits.Set(value, sizeof(uint) * ByteSize);
            });
        }

        [Fact]
        public void set_uint_bits_and_verify_result()
        {
            uint value = 0b0000_0000_0000_0000_0000_0000_0000_0000;
            uint expected = 0b1111_1111_1111_1111_1111_1111_1111_1111;

            for (int i = 0; i < sizeof(uint) * ByteSize; i++)
            {
                Assert.False(Bits.IsSet(value, i));
                value = Bits.Set(value, i);
                Assert.True(Bits.IsSet(value, i));
            }

            Assert.Equal(expected, value);
        }

        [Fact]
        public void set_throws_with_ulong_and_minus_one()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                ulong value = 0;
                Bits.Set(value, -1);
            });
        }

        [Fact]
        public void set_throws_with_ulong_and_sizeof_ulong()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                ulong value = 0;
                Bits.Set(value, sizeof(ulong) * ByteSize);
            });
        }

        [Fact]
        public void set_ulong_bits_and_verify_result()
        {
            ulong value = 0b0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000;
            ulong expected = 0b1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111;

            for (int i = 0; i < sizeof(ulong) * ByteSize; i++)
            {
                Assert.False(Bits.IsSet(value, i));
                value = Bits.Set(value, i);
                Assert.True(Bits.IsSet(value, i));
            }

            Assert.Equal(expected, value);
        }

        [Fact]
        public void clear_throws_with_byte_and_minus_one()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                byte value = 0;
                Bits.Clear(value, -1);
            });
        }

        [Fact]
        public void clear_throws_with_byte_and_sizeof_byte()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                byte value = 0;
                Bits.Clear(value, sizeof(byte) * ByteSize);
            });
        }

        [Fact]
        public void clear_byte_bits_and_verify_result()
        {
            byte value = 0b1111_1111;
            byte expected = 0b0000_0000;

            for (int i = 0; i < sizeof(byte) * ByteSize; i++)
            {
                Assert.False(Bits.IsClear(value, i));
                value = Bits.Clear(value, i);
                Assert.True(Bits.IsClear(value, i));
            }

            Assert.Equal(expected, value);
        }

        [Fact]
        public void clear_throws_with_ushort_and_minus_one()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                ushort value = 0;
                Bits.Clear(value, -1);
            });
        }

        [Fact]
        public void clear_throws_with_ushort_and_sizeof_ushort()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                ushort value = 0;
                Bits.Clear(value, sizeof(ushort) * ByteSize);
            });
        }

        [Fact]
        public void clear_ushort_bits_and_verify_result()
        {
            ushort value = 0b1111_1111_1111_1111;
            ushort expected = 0b0000_0000_0000_0000;

            for (int i = 0; i < sizeof(ushort) * ByteSize; i++)
            {
                Assert.False(Bits.IsClear(value, i));
                value = Bits.Clear(value, i);
                Assert.True(Bits.IsClear(value, i));
            }

            Assert.Equal(expected, value);
        }

        [Fact]
        public void clear_throws_with_uint_and_minus_one()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                uint value = 0;
                Bits.Clear(value, -1);
            });
        }

        [Fact]
        public void clear_throws_with_uint_and_sizeof_uint()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                uint value = 0;
                Bits.Clear(value, sizeof(uint) * ByteSize);
            });
        }

        [Fact]
        public void clear_uint_bits_and_verify_result()
        {
            uint value = 0b1111_1111_1111_1111_1111_1111_1111_1111;
            uint expected = 0b0000_0000_0000_0000_0000_0000_0000_0000;

            for (int i = 0; i < sizeof(uint) * ByteSize; i++)
            {
                Assert.False(Bits.IsClear(value, i));
                value = Bits.Clear(value, i);
                Assert.True(Bits.IsClear(value, i));
            }

            Assert.Equal(expected, value);
        }

        [Fact]
        public void clear_throws_with_ulong_and_minus_one()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                ulong value = 0;
                Bits.Clear(value, -1);
            });
        }

        [Fact]
        public void clear_throws_with_ulong_and_sizeof_ulong()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                ulong value = 0;
                Bits.Clear(value, sizeof(ulong) * ByteSize);
            });
        }

        [Fact]
        public void clear_ulong_bits_and_verify_result()
        {
            ulong value = 0b1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111;
            ulong expected = 0b0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000;

            for (int i = 0; i < sizeof(ulong) * ByteSize; i++)
            {
                Assert.False(Bits.IsClear(value, i));
                value = Bits.Clear(value, i);
                Assert.True(Bits.IsClear(value, i));
            }

            Assert.Equal(expected, value);
        }
    }
}
