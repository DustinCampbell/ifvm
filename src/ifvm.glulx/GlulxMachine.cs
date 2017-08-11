using System;
using IFVM.Core;

namespace IFVM.Glulx
{
    public class GlulxMachine : Machine
    {
        public Version GlulxVersion { get; }

        public GlulxMachine(Memory memory) : base(memory)
        {
            // The header is the first 36 bytes of memory and cannot change.

            var scanner = memory.CreateScanner(offset: 0);

            // The first 4 bytes are the magic number 47 6C 75 6C. In ASCII, that's 'Glul'
            var magicNumber = scanner.NextDWord();
            if (magicNumber != 0x476C756C)
            {
                throw new InvalidOperationException("Not a valid glulx file!");
            }

            var majorVersion = scanner.NextWord();
            var minorVersion = scanner.NextByte();
            var minorMinorVersion = scanner.NextByte();
            this.GlulxVersion = new Version(majorVersion, minorVersion, minorMinorVersion);
        }
    }
}
