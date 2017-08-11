using System.Collections.Immutable;

namespace IFVM.Core.Ast
{
    public abstract class AstNode
    {
        public AstNodeKind Kind { get; }

        protected AstNode(AstNodeKind kind)
        {
            this.Kind = kind;
        }

        public abstract void Accept(AstVisitor visitor);
        public abstract AstNode Accept(AstRewriter rewriter);

        public abstract ImmutableList<AstNode> ChildNodes();
    }
}
