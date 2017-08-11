using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using IFVM.Core.Extensions;

namespace IFVM.Core
{
    public partial class Memory
    {
        private static readonly ObjectPool<byte[]> s_twoByteArrays = new ObjectPool<byte[]>(() => new byte[2], 512);
        private static readonly ObjectPool<byte[]> s_fourByteArrays = new ObjectPool<byte[]>(() => new byte[4], 512);

        private byte[] _bytes;
        private List<(int start, int length)> _readOnlyRegions;

        public int Size => _bytes.Length;

        private Memory(byte[] bytes)
        {
            _bytes = bytes ?? throw new ArgumentNullException(nameof(bytes));
        }

        public static Memory Create(byte[] bytes) => new Memory(bytes);

        public static async Task<Memory> CreateAsync(Stream stream)
        {
            var bytes = await stream.ReadAllBytesAsync();
            return new Memory(bytes);
        }

        public void Expand(int newSize)
        {
            if (newSize < this.Size)
            {
                throw new ArgumentOutOfRangeException(nameof(newSize), "New size cannot be less than the current size.");
            }

            Array.Resize(ref _bytes, newSize);
        }

        private void ValidateOffsetAndSize(int index, int size)
        {
            if (index < 0 || index > _bytes.Length - size)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

        private void ValidateOffsetAndSizeForWrite(int index, int size)
        {
            if (_readOnlyRegions != null)
            {
                foreach (var (start, length) in _readOnlyRegions)
                {
                    if ((index >= start && index < start + length) ||
                        (index + size > start && index + size <= start + length))
                    {
                        throw new InvalidOperationException("Write overlaps with a region of read-only memory");
                    }
                }
            }
        }

        public void AddReadOnlyRegion(int start, int length)
        {
            if (_readOnlyRegions == null)
            {
                _readOnlyRegions = new List<(int start, int length)>();
            }

            _readOnlyRegions.Add((start, length));
        }

        public byte ReadByte(int offset)
        {
            ValidateOffsetAndSize(offset, sizeof(byte));

            return _bytes[offset];
        }

        public ushort ReadWord(int offset)
        {
            ValidateOffsetAndSize(offset, sizeof(ushort));

            var tempArray = s_twoByteArrays.Allocate();
            Array.Copy(_bytes, offset, tempArray, 0, sizeof(ushort));

            var b1 = tempArray[0];
            var b2 = tempArray[1];

            s_twoByteArrays.Free(tempArray);

            return (ushort)((b1 << 8) | b2);
        }

        public uint ReadDWord(int offset)
        {
            ValidateOffsetAndSize(offset, sizeof(uint));

            var tempArray = s_fourByteArrays.Allocate();
            Array.Copy(_bytes, offset, tempArray, 0, sizeof(uint));

            var b1 = tempArray[0];
            var b2 = tempArray[1];
            var b3 = tempArray[2];
            var b4 = tempArray[3];

            s_fourByteArrays.Free(tempArray);

            return (uint)((b1 << 24) | (b2 << 16) | (b3 << 8) | b4);
        }

        public void WriteByte(int offset, byte value)
        {
            ValidateOffsetAndSize(offset, sizeof(byte));
            ValidateOffsetAndSizeForWrite(offset, sizeof(byte));

            _bytes[offset] = value;
        }

        public void WriteWord(int offset, ushort value)
        {
            ValidateOffsetAndSize(offset, sizeof(ushort));
            ValidateOffsetAndSizeForWrite(offset, sizeof(ushort));

            var tempArray = s_twoByteArrays.Allocate();

            tempArray[0] = (byte)(value >> 8);
            tempArray[1] = (byte)(value & 0x00ff);

            Array.Copy(tempArray, 0, _bytes, offset, sizeof(ushort));

            s_twoByteArrays.Free(tempArray);
        }

        public void WriteDWord(int offset, uint value)
        {
            ValidateOffsetAndSize(offset, sizeof(uint));
            ValidateOffsetAndSizeForWrite(offset, sizeof(uint));

            var tempArray = s_fourByteArrays.Allocate();

            tempArray[0] = (byte)(value >> 24);
            tempArray[1] = (byte)(value >> 16);
            tempArray[2] = (byte)(value >> 8);
            tempArray[3] = (byte)(value & 0x000000ff);

            Array.Copy(tempArray, 0, _bytes, offset, sizeof(uint));

            s_fourByteArrays.Free(tempArray);
        }

        public Scanner CreateScanner(int offset)
        {
            return new Scanner(this, offset);
        }
    }
}
