using System;

namespace IFVM.Utilities
{
    public static class Bits
    {
        private static void ValidateBit(int bit, int sizeInBytes)
        {
            const int ByteSize = 8;

            if (bit < 0 || bit >= (sizeInBytes * ByteSize))
            {
                throw new ArgumentOutOfRangeException(nameof(bit), bit, $"Should be in range 0 to {(sizeInBytes * ByteSize) - 1}");
            }
        }

        public static bool IsSet(byte value, int bit)
        {
            ValidateBit(bit, sizeof(byte));
            return (value & (1 << bit)) != 0;
        }

        public static bool IsSet(ushort value, int bit)
        {
            ValidateBit(bit, sizeof(ushort));
            return (value & (1 << bit)) != 0;
        }

        public static bool IsSet(uint value, int bit)
        {
            ValidateBit(bit, sizeof(uint));
            return (value & (1u << bit)) != 0;
        }

        public static bool IsSet(ulong value, int bit)
        {
            ValidateBit(bit, sizeof(ulong));
            return (value & (1ul << bit)) != 0;
        }

        public static bool IsClear(byte value, int bit)
        {
            ValidateBit(bit, sizeof(byte));
            return (value & (1 << bit)) == 0;
        }

        public static bool IsClear(ushort value, int bit)
        {
            ValidateBit(bit, sizeof(ushort));
            return (value & (1 << bit)) == 0;
        }

        public static bool IsClear(uint value, int bit)
        {
            ValidateBit(bit, sizeof(uint));
            return (value & 1u << bit) == 0;
        }

        public static bool IsClear(ulong value, int bit)
        {
            ValidateBit(bit, sizeof(ulong));
            return (value & 1ul << bit) == 0;
        }

        public static byte Set(byte value, int bit)
        {
            ValidateBit(bit, sizeof(byte));
            return (byte)(value | (1 << bit));
        }

        public static ushort Set(ushort value, int bit)
        {
            ValidateBit(bit, sizeof(ushort));
            return (ushort)(value | (1 << bit));
        }

        public static uint Set(uint value, int bit)
        {
            ValidateBit(bit, sizeof(uint));
            return value | (1u << bit);
        }

        public static ulong Set(ulong value, int bit)
        {
            ValidateBit(bit, sizeof(ulong));
            return value | (1ul << bit);
        }

        public static byte Clear(byte value, int bit)
        {
            ValidateBit(bit, sizeof(byte));
            return (byte)(value & ~(1 << bit));
        }

        public static ushort Clear(ushort value, int bit)
        {
            ValidateBit(bit, sizeof(ushort));
            return (ushort)(value & ~(1 << bit));
        }

        public static uint Clear(uint value, int bit)
        {
            ValidateBit(bit, sizeof(uint));
            return value & ~(1u << bit);
        }

        public static ulong Clear(ulong value, int bit)
        {
            ValidateBit(bit, sizeof(ulong));
            return value & ~(1ul << bit);
        }
    }
}
