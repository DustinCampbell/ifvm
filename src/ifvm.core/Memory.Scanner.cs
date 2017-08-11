using System;

namespace IFVM.Core
{
    public partial class Memory
    {
        public class Scanner
        {
            private int offset;

            public Memory Memory { get; }
            public int Offset => offset;

            internal Scanner(Memory memory, int offset)
            {
                this.Memory = memory ?? throw new ArgumentNullException(nameof(memory));
                this.offset = offset;
            }

            public byte NextByte()
            {
                var result = this.Memory.ReadByte(offset);
                offset++;
                return result;
            }

            public ushort NextWord()
            {
                var result = this.Memory.ReadWord(offset);
                offset += 2;
                return result;
            }

            public uint NextDWord()
            {
                var result = this.Memory.ReadDWord(offset);
                offset += 4;
                return result;
            }
        }
    }
}
