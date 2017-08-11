using System;
using System.IO;
using System.Threading.Tasks;

namespace IFVM.Core
{
    public partial class Memory
    {
        private static readonly ObjectPool<byte[]> s_twoByteArrays = new ObjectPool<byte[]>(() => new byte[2], 512);
        private static readonly ObjectPool<byte[]> s_fourByteArrays = new ObjectPool<byte[]>(() => new byte[4], 512);

        private readonly byte[] _bytes;

        public int Length => _bytes.Length;

        private Memory(byte[] bytes)
        {
            _bytes = bytes ?? throw new ArgumentNullException(nameof(bytes));
        }

        public static Memory Create(byte[] bytes) => new Memory(bytes);

        public static async Task<Memory> CreateAsync(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            var buffer = new byte[1024];
            var read = 0;

            int chunk;
            while ((chunk = await stream.ReadAsync(buffer, read, buffer.Length - read)) > 0)
            {
                read += chunk;

                if (read == buffer.Length)
                {
                    int nextByte = stream.ReadByte();
                    if (nextByte == -1)
                    {
                        return new Memory(buffer);
                    }

                    Array.Resize(ref buffer, buffer.Length * 2);
                    buffer[read] = (byte)nextByte;
                    read++;
                }
            }

            Array.Resize(ref buffer, read);

            return new Memory(buffer);
        }

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

            var tempArray = s_twoByteArrays.Allocate();
            Array.Copy(_bytes, index, tempArray, 0, sizeof(ushort));

            var b1 = tempArray[0];
            var b2 = tempArray[1];

            s_twoByteArrays.Free(tempArray);

            return (ushort)((b1 << 8) | b2);
        }

        public uint ReadDWord(int index)
        {
            ValidateIndex(index, sizeof(uint));

            var tempArray = s_fourByteArrays.Allocate();
            Array.Copy(_bytes, index, tempArray, 0, sizeof(uint));

            var b1 = tempArray[0];
            var b2 = tempArray[1];
            var b3 = tempArray[2];
            var b4 = tempArray[3];

            s_fourByteArrays.Free(tempArray);

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

            var tempArray = s_twoByteArrays.Allocate();

            tempArray[0] = (byte)(value >> 8);
            tempArray[1] = (byte)(value & 0x00ff);

            Array.Copy(tempArray, 0, _bytes, index, sizeof(ushort));

            s_twoByteArrays.Free(tempArray);
        }

        public void WriteDWord(int index, uint value)
        {
            ValidateIndex(index, sizeof(uint));

            var tempArray = s_fourByteArrays.Allocate();

            tempArray[0] = (byte)(value >> 24);
            tempArray[1] = (byte)(value >> 16);
            tempArray[2] = (byte)(value >> 8);
            tempArray[3] = (byte)(value & 0x000000ff);

            Array.Copy(tempArray, 0, _bytes, index, sizeof(uint));

            s_fourByteArrays.Free(tempArray);
        }

        public Scanner CreateScanner(int offset)
        {
            return new Scanner(this, offset);
        }
    }
}
