namespace IFVM.Glulx.Functions
{
    public struct Operand
    {
        public OperandType Type { get; }
        public uint Value { get; }

        public Operand(OperandType type, uint value)
        {
            Type = type;
            Value = value;
        }

        public override string ToString()
            => $"{{ {Type}: Value = 0x{Value:x} }}";
    }
}
