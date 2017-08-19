using System;
using System.IO;
using System.Threading.Tasks;
using IFVM.Core;
using IFVM.Glulx.Functions;

namespace IFVM.Glulx
{
    public class GlulxMachine : Machine
    {
        public GlulxHeader Header { get; }
        public Function StartFunction { get; }

        private GlulxMachine(GlulxHeader header, Memory memory, Stack stack) : base(memory, stack)
        {
            this.Header = header;
            this.StartFunction = Function.Read(memory, (int)header.StartFunc);
        }

        public static async Task<GlulxMachine> CreateAsync(Stream stream)
        {
            var memory = await Memory.CreateAsync(stream);
            var header = new GlulxHeader(memory);

            VerifyChecksum(memory, header.Checksum);

            // Initial the memory should have a size equal to ExtStart.
            // We must expand it to EndMem.
            if (header.ExtStart != memory.Size)
            {
                throw new InvalidOperationException($"Size expected to be {header.ExtStart}");
            }

            memory.Expand((int)header.EndMem);
            memory.AddReadOnlyRegion(0, (int)header.RamStart);

            if (header.StackSize % 256 != 0)
            {
                throw new InvalidOperationException("Stack size must be a multiple of 256");
            }

            var stack = new Stack((int)header.StackSize);

            return new GlulxMachine(header, memory, stack);
        }

        private static void VerifyChecksum(Memory memory, uint expectedValue)
        {
            var scanner = memory.CreateScanner(offset: 0);

            var checksum = 0u;
            while (scanner.CanReadNextDWord)
            {
                if (scanner.Address == 0x20)
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
