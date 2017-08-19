using System.Collections.Immutable;

namespace IFVM.Glulx.Functions
{
    public class OpcodeDescriptor
    {
        public uint Number { get; }
        public string Name { get; }
        public uint ArgumentSize { get; }
        public ImmutableList<OperandKind> OperandKinds { get; }
        public int OperandCount => OperandKinds.Count;

        public OpcodeDescriptor(uint number, string name, uint argumentSize, ImmutableList<OperandKind> operandKinds)
        {
            Number = number;
            Name = name;
            ArgumentSize = argumentSize;
            OperandKinds = operandKinds;
        }

        public override string ToString()
            => $"{Name} (0x{Number:x})";
    }
}
