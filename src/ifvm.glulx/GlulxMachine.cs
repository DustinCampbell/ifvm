using System;
using IFVM.Core;

namespace IFVM.Glulx
{
    public class GlulxMachine : Machine
    {
        public GlulxHeader Header { get; }

        public GlulxMachine(Memory memory) : base(memory)
        {
            this.Header = new GlulxHeader(memory);

            VerifyChecksum(memory, this.Header.Checksum);

            // Initial the memory should have a size equal to ExtStart.
            // We must expand it to EndMem.
            if (this.Header.ExtStart != memory.Size)
            {
                throw new InvalidOperationException($"Size expected to be {this.Header.ExtStart}");
            }

            memory.Expand((int)this.Header.EndMem);
            memory.AddReadOnlyRegion(0, (int)this.Header.RamStart);
        }

        private static void VerifyChecksum(Memory memory, uint expectedValue)
        {
            var scanner = memory.CreateScanner(offset: 0);

            var checksum = 0u;
            while (scanner.CanReadNextDWord)
            {
                if (scanner.Offset == 0x20)
                {
                    // Note: We don't include the checksum value from the header.
                    scanner.SkipDWord();
                }
                else
                {
                    checksum += scanner.NextDWord();
                }
            }

            if (checksum != expectedValue)
            {
                throw new InvalidOperationException("Checksum does not match.");
            }
        }
    }
}
