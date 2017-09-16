using IFVM.Ast;

namespace IFVM.Core
{
    public abstract class Function
    {
        public FunctionKind Kind { get; }
        public int Address { get; }
        public AstBody Body { get; }

        protected Function(FunctionKind kind, int address, AstBody body)
        {
            Kind = kind;
            Address = address;
            Body = body;
        }
    }
}
