using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using IFVM.Utilities;
using IFVM.Extensions;

namespace IFVM.Core
{
    public partial class Memory
    {
        private static readonly ObjectPool<byte[]> s_twoByteArrays = new ObjectPool<byte[]>(() => new byte[2]);
        private static readonly ObjectPool<byte[]> s_fourByteArrays = new ObjectPool<byte[]>(() => new byte[4]);

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

        private void ValidateAddressAndSize(int address, int size)
        {
            if (address < 0 || address > _bytes.Length - size)
            {
                throw new ArgumentOutOfRangeException(nameof(address));
            }
        }

        private void ValidateAddressAndSizeForWrite(int address, int size)
        {
            if (_readOnlyRegions != null)
            {
                foreach (var (start, length) in _readOnlyRegions)
                {
                    if ((address >= start && address < start + length) ||
                        (address + size > start && address + size <= start + length))
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

        public byte ReadByte(int address)
        {
            ValidateAddressAndSize(address, sizeof(byte));

            return _bytes[address];
        }

        public ushort ReadWord(int address)
        {
            ValidateAddressAndSize(address, sizeof(ushort));

            var tempArray = s_twoByteArrays.Allocate();
            Array.Copy(_bytes, address, tempArray, 0, sizeof(ushort));

            var b1 = tempArray[0];
            var b2 = tempArray[1];

            s_twoByteArrays.Free(tempArray);

            return (ushort)((b1 << 8) | b2);
        }

        public uint ReadDWord(int address)
        {
            ValidateAddressAndSize(address, sizeof(uint));

            var tempArray = s_fourByteArrays.Allocate();
            Array.Copy(_bytes, address, tempArray, 0, sizeof(uint));

            var b1 = tempArray[0];
            var b2 = tempArray[1];
            var b3 = tempArray[2];
            var b4 = tempArray[3];

            s_fourByteArrays.Free(tempArray);

            return (uint)((b1 << 24) | (b2 << 16) | (b3 << 8) | b4);
        }

        public void WriteByte(int address, byte value)
        {
            ValidateAddressAndSize(address, sizeof(byte));
            ValidateAddressAndSizeForWrite(address, sizeof(byte));

            _bytes[address] = value;
        }

        public void WriteWord(int address, ushort value)
        {
            ValidateAddressAndSize(address, sizeof(ushort));
            ValidateAddressAndSizeForWrite(address, sizeof(ushort));

            var tempArray = s_twoByteArrays.Allocate();

            tempArray[0] = (byte)(value >> 8);
            tempArray[1] = (byte)(value & 0x00ff);

            Array.Copy(tempArray, 0, _bytes, address, sizeof(ushort));

            s_twoByteArrays.Free(tempArray);
        }

        public void WriteDWord(int address, uint value)
        {
            ValidateAddressAndSize(address, sizeof(uint));
            ValidateAddressAndSizeForWrite(address, sizeof(uint));

            var tempArray = s_fourByteArrays.Allocate();

            tempArray[0] = (byte)(value >> 24);
            tempArray[1] = (byte)(value >> 16);
            tempArray[2] = (byte)(value >> 8);
            tempArray[3] = (byte)(value & 0x000000ff);

            Array.Copy(tempArray, 0, _bytes, address, sizeof(uint));

            s_fourByteArrays.Free(tempArray);
        }

        public MemoryScanner CreateScanner(int address)
        {
            return new MemoryScanner(this, address);
        }
    }
}
