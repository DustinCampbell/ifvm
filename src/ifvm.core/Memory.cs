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

        public byte ReadByte(int index)
        {
            if (index < 0 || index >= _bytes.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            return _bytes[index];
        }
    }
}
