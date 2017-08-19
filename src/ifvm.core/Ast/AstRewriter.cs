using System.Collections.Immutable;

namespace IFVM.Ast
{
    public abstract partial class AstRewriter
    {
        public ImmutableList<TNode> VisitList<TNode>(ImmutableList<TNode> list)
            where TNode : AstNode
        {
            var count = list.Count;
            var builder = ImmutableList.CreateBuilder<TNode>();

            var changed = false;
            for (int i = 0; i < count; i++)
            {
                var node = list[i];
                var newNode = (TNode)Visit(node);

                if (!changed && node != newNode)
                {
                    changed = true;
                }

                builder.Add(newNode);
            }

            return changed
                ? builder.ToImmutable()
                : list;
        }
    }
}
