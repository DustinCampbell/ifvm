using System.Collections.Immutable;

namespace IFVM.Ast
{
    public class AstBody
    {
        public ImmutableList<AstLabel> Labels { get; }
        public ImmutableList<AstLocal> Locals { get; }
        public ImmutableList<AstStatement> Statements { get; }

        public AstBody(ImmutableList<AstLabel> labels, ImmutableList<AstLocal> locals, ImmutableList<AstStatement> statements)
        {
            Labels = labels;
            Locals = locals;
            Statements = statements;
        }
    }
}
