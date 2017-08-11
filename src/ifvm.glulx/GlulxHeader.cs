using System;
using IFVM.Core;

namespace IFVM.Glulx
{
    public class GlulxHeader
    {
        // The first 4 bytes of every Glulx file are 47 6C 75 6C. In ASCII, that's 'Glul'
        private const int MagicNumber = 0x476C756C;

        public GlulxVersion Version { get; }
        public uint RamStart { get; }
        public uint ExtStart { get; }
        public uint EndMem { get; }
        public uint StackSize { get; }
        public uint StartFunc { get; }
        public uint DecodingTbl { get; }
        public uint Checksum { get; }

        public GlulxHeader(Memory memory)
        {
            // Read header values.
            var scanner = memory.CreateScanner(offset: 0);

            if (scanner.NextDWord() != MagicNumber)
            {
                throw new InvalidOperationException("Not a valid glulx file.");
            }

            this.Version = new GlulxVersion(
                major: scanner.NextWord(),
                minor: scanner.NextByte(),
                subMinor: scanner.NextByte());

            this.RamStart = scanner.NextDWord();
            this.ExtStart = scanner.NextDWord();
            this.EndMem = scanner.NextDWord();
            this.StackSize = scanner.NextDWord();
            this.StartFunc = scanner.NextDWord();
            this.DecodingTbl = scanner.NextDWord();
            this.Checksum = scanner.NextDWord();
        }
    }
}
