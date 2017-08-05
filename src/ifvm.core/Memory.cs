using System;

namespace IFVM.Core
{
    public class Memory
    {
        private readonly byte[] _bytes;

        public int Length => _bytes.Length;

        private Memory(byte[] bytes)
        {
            _bytes = bytes ?? throw new ArgumentNullException(nameof(bytes));
        }

        public static Memory Create(byte[] bytes) => new Memory(bytes);

        private void ValidateIndex(int index, int size)
        {
            if (index < 0 || index > _bytes.Length - size)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

        public byte ReadByte(int index)
        {
            ValidateIndex(index, sizeof(byte));

            return _bytes[index];
        }

        public ushort ReadWord(int index)
        {
            ValidateIndex(index, sizeof(ushort));

            var b1 = _bytes[index];
            var b2 = _bytes[index + 1];

            return (ushort)((b1 << 8) | b2);
        }

        public uint ReadDWord(int index)
        {
            ValidateIndex(index, sizeof(uint));

            var b1 = _bytes[index];
            var b2 = _bytes[index + 1];
            var b3 = _bytes[index + 2];
            var b4 = _bytes[index + 3];

            return (uint)((b1 << 24) | (b2 << 16) | (b3 << 8) | b4);
        }

        public void WriteByte(int index, byte value)
        {
            ValidateIndex(index, sizeof(byte));

            _bytes[index] = value;
        }

        public void WriteWord(int index, ushort value)
        {
            ValidateIndex(index, sizeof(ushort));

            _bytes[index] = (byte)(value >> 8);
            _bytes[index + 1] = (byte)(value & 0x00ff);
        }

        public void WriteDWord(int index, uint value)
        {
            ValidateIndex(index, sizeof(uint));

            _bytes[index] = (byte)(value >> 24);
            _bytes[index + 1] = (byte)(value >> 16);
            _bytes[index + 2] = (byte)(value >> 8);
            _bytes[index + 3] = (byte)(value & 0x000000ff);
        }
    }
}
