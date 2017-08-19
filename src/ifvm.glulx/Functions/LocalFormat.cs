using IFVM.Ast;

namespace IFVM.Glulx.Functions
{
    public struct LocalFormat
    {
        public ValueSize Size { get; }
        public byte Count { get; }

        public LocalFormat(ValueSize size, byte count)
        {
            this.Size = size;
            this.Count = count;
        }
    }
}
