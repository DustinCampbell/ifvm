using System;
using IFVM.Utilities;

namespace IFVM.Core
{
    public class Stack
    {
        private static readonly ObjectPool<byte[]> s_twoByteArrays = new ObjectPool<byte[]>(() => new byte[2], 512);
        private static readonly ObjectPool<byte[]> s_fourByteArrays = new ObjectPool<byte[]>(() => new byte[4], 512);

        private byte[] _bytes;
        private int _pointer;

        public int Size => _bytes.Length;
        public int Pointer => _pointer;

        public Stack(int size)
        {
            if (size < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(size), "Stack size cannot be less than zero.");
            }

            _bytes = new byte[size];
            _pointer = 0;
        }

        private void ValidatePush(int size)
        {
            if (_pointer + size > _bytes.Length)
            {
                throw new StackOverflowException("Stack overflow.");
            }
        }

        private void ValidatePop(int size)
        {
            if (_pointer - size < 0)
            {
                throw new StackUnderflowException("Stack underflow.");
            }
        }

        public void PushByte(byte value)
        {
            ValidatePush(sizeof(byte));

            _bytes[_pointer] = value;
            _pointer++;
        }

        public void PushWord(ushort value)
        {
            ValidatePush(sizeof(ushort));

            var tempArray = s_twoByteArrays.Allocate();

            tempArray[0] = (byte)(value >> 8);
            tempArray[1] = (byte)(value & 0x00ff);

            Array.Copy(tempArray, 0, _bytes, _pointer, sizeof(ushort));

            _pointer += sizeof(ushort);

            s_twoByteArrays.Free(tempArray);
        }

        public void PushDWord(uint value)
        {
            ValidatePush(sizeof(uint));

            var tempArray = s_fourByteArrays.Allocate();

            tempArray[0] = (byte)(value >> 24);
            tempArray[1] = (byte)(value >> 16);
            tempArray[2] = (byte)(value >> 8);
            tempArray[3] = (byte)(value & 0x000000ff);

            Array.Copy(tempArray, 0, _bytes, _pointer, sizeof(uint));

            _pointer += sizeof(uint);

            s_fourByteArrays.Free(tempArray);
        }

        public byte PopByte()
        {
            ValidatePop(sizeof(byte));

            _pointer--;
            return _bytes[_pointer];
        }

        public ushort PopWord()
        {
            ValidatePop(sizeof(ushort));

            _pointer -= sizeof(ushort);

            var tempArray = s_twoByteArrays.Allocate();
            Array.Copy(_bytes, _pointer, tempArray, 0, sizeof(ushort));

            var b1 = tempArray[0];
            var b2 = tempArray[1];

            s_twoByteArrays.Free(tempArray);

            return (ushort)((b1 << 8) | b2);
        }

        public uint PopDWord()
        {
            ValidatePop(sizeof(uint));

            _pointer -= sizeof(uint);

            var tempArray = s_fourByteArrays.Allocate();
            Array.Copy(_bytes, _pointer, tempArray, 0, sizeof(uint));

            var b1 = tempArray[0];
            var b2 = tempArray[1];
            var b3 = tempArray[2];
            var b4 = tempArray[3];

            s_fourByteArrays.Free(tempArray);

            return (uint)((b1 << 24) | (b2 << 16) | (b3 << 8) | b4);
        }
    }
}
