using System;

namespace IFVM.Core
{
    public class MemoryScanner
    {
        private int address;

        public Memory Memory { get; }
        public int Address => address;

        internal MemoryScanner(Memory memory, int address)
        {
            this.Memory = memory ?? throw new ArgumentNullException(nameof(memory));
            this.address = address;
        }

        public void SetAddress(int newAddress)
        {
            this.address = newAddress;
        }

        public bool CanReadNextByte => address < this.Memory.Size;
        public bool CanReadNextWord => address < this.Memory.Size - 1;
        public bool CanReadNextDWord => address < this.Memory.Size - 3;

        public byte NextByte()
        {
            var result = this.Memory.ReadByte(address);
            address++;
            return result;
        }

        public ushort NextWord()
        {
            var result = this.Memory.ReadWord(address);
            address += 2;
            return result;
        }

        public uint NextDWord()
        {
            var result = this.Memory.ReadDWord(address);
            address += 4;
            return result;
        }

        public byte PeekByte(int offset = 0)
        {
            return this.Memory.ReadByte(address + offset);
        }

        public ushort PeekWord(int offset = 0)
        {
            return this.Memory.ReadWord(address + offset);
        }

        public uint PeekDWord(int offset = 0)
        {
            return this.Memory.ReadDWord(address + offset);
        }

        public void SkipByte()
        {
            address++;
        }

        public void SkipBytes(int count)
        {
            address += count;
        }

        public void SkipWord()
        {
            address += 2;
        }

        public void SkipWords(int count)
        {
            address += (count * 2);
        }

        public void SkipDWord()
        {
            address += 4;
        }

        public void SkipDWords(int count)
        {
            address += (count * 4);
        }
    }
}
