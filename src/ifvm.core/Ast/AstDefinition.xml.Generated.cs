// <auto-generated />

using System.Collections.Immutable;

namespace IFVM.Ast
{
    public enum AstNodeKind
    {
        Label,
        Local,
        ConstantExpression,
        WriteLocalStatement,
    }

    public partial class AstLabel : AstNode
    {
        private readonly int index;

        internal AstLabel(int index) : base(AstNodeKind.Label)
        {
            this.index = index;
        }

        public int Index
        {
            get { return this.index; }
        }

        public override AstNode Accept(AstRewriter rewriter)
        {
            return rewriter.VisitLabel(this);
        }

        public override void Accept(AstVisitor visitor)
        {
            visitor.VisitLabel(this);
        }

        public override ImmutableList<AstNode> ChildNodes()
        {
            return ImmutableList<AstNode>.Empty;
        }
    }

    public partial class AstLocal : AstNode
    {
        private readonly AstExpression index;
        private readonly ValueSize size;

        internal AstLocal(AstExpression index, ValueSize size) : base(AstNodeKind.Local)
        {
            this.index = index;
            this.size = size;
        }

        public AstExpression Index
        {
            get { return this.index; }
        }

        public ValueSize Size
        {
            get { return this.size; }
        }

        public override AstNode Accept(AstRewriter rewriter)
        {
            return rewriter.VisitLocal(this);
        }

        public override void Accept(AstVisitor visitor)
        {
            visitor.VisitLocal(this);
        }

        public override ImmutableList<AstNode> ChildNodes()
        {
            var builder = ImmutableList.CreateBuilder<AstNode>();

            builder.Add(Index);

            return builder.ToImmutable();
        }
    }

    public abstract partial class AstExpression : AstNode
    {
        internal AstExpression(AstNodeKind kind) : base(kind)
        {
        }
    }

    public partial class AstConstantExpression : AstExpression
    {
        private readonly int value;

        internal AstConstantExpression(int value) : base(AstNodeKind.ConstantExpression)
        {
            this.value = value;
        }

        public int Value
        {
            get { return this.value; }
        }

        public override AstNode Accept(AstRewriter rewriter)
        {
            return rewriter.VisitConstantExpression(this);
        }

        public override void Accept(AstVisitor visitor)
        {
            visitor.VisitConstantExpression(this);
        }

        public override ImmutableList<AstNode> ChildNodes()
        {
            return ImmutableList<AstNode>.Empty;
        }
    }

    public abstract partial class AstStatement : AstNode
    {
        internal AstStatement(AstNodeKind kind) : base(kind)
        {
        }
    }

    public partial class AstWriteLocalStatement : AstStatement
    {
        private readonly AstLocal local;
        private readonly AstExpression value;

        internal AstWriteLocalStatement(AstLocal local, AstExpression value) : base(AstNodeKind.WriteLocalStatement)
        {
            this.local = local;
            this.value = value;
        }

        public AstLocal Local
        {
            get { return this.local; }
        }

        public AstExpression Value
        {
            get { return this.value; }
        }

        public override AstNode Accept(AstRewriter rewriter)
        {
            return rewriter.VisitWriteLocalStatement(this);
        }

        public override void Accept(AstVisitor visitor)
        {
            visitor.VisitWriteLocalStatement(this);
        }

        public override ImmutableList<AstNode> ChildNodes()
        {
            var builder = ImmutableList.CreateBuilder<AstNode>();

            builder.Add(Local);
            builder.Add(Value);

            return builder.ToImmutable();
        }
    }

    public abstract partial class AstVisitor
    {
        public virtual void Visit(AstNode node)
        {
            node.Accept(this);
        }

        public void VisitList<TNode>(ImmutableList<TNode> list) where TNode : AstNode
        {
            foreach (var item in list)
            {
                Visit(item);
            }
        }

        public virtual void VisitLabel(AstLabel node)
        {
        }

        public virtual void VisitLocal(AstLocal node)
        {
            Visit(node.Index);
        }

        public virtual void VisitConstantExpression(AstConstantExpression node)
        {
        }

        public virtual void VisitWriteLocalStatement(AstWriteLocalStatement node)
        {
            Visit(node.Local);
            Visit(node.Value);
        }
    }

    public abstract partial class AstRewriter
    {
        public virtual AstNode Visit(AstNode node)
        {
            return node.Accept(this);
        }

        public virtual AstNode VisitLabel(AstLabel node)
        {
            return node;
        }

        public virtual AstNode VisitLocal(AstLocal node)
        {
            var index = (AstExpression)Visit(node.Index);

            return index != node.Index
                ? new AstLocal(index, node.Size)
                : node;
        }

        public virtual AstNode VisitConstantExpression(AstConstantExpression node)
        {
            return node;
        }

        public virtual AstNode VisitWriteLocalStatement(AstWriteLocalStatement node)
        {
            var local = (AstLocal)Visit(node.Local);
            var value = (AstExpression)Visit(node.Value);

            return local != node.Local || value != node.Value
                ? new AstWriteLocalStatement(local, value)
                : node;
        }
    }

    public static partial class AstFactory
    {
        public static AstLabel Label(int index)
        {
            return new AstLabel(index);
        }

        public static AstLocal Local(AstExpression index, ValueSize size)
        {
            return new AstLocal(index, size);
        }

        public static AstWriteLocalStatement WriteLocalStatement(AstLocal local, AstExpression value)
        {
            return new AstWriteLocalStatement(local, value);
        }
    }
}
