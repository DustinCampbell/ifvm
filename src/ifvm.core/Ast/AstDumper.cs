using System;
using System.Collections.Generic;
using System.Text;

namespace IFVM.Ast
{
    public class AstDumper : AstVisitor
    {
        private readonly StringBuilder builder;

        private int indentSize = 4;
        private int indentLevel = 0;
        private bool needsIndent = true;
        private bool allowParentheses = true;

        private AstDumper(StringBuilder builder)
        {
            this.builder = builder;
        }

        public static string Dump(AstBody body)
        {
            var builder = new StringBuilder();
            var dumper = new AstDumper(builder);

            foreach (var statement in body.Statements)
            {
                dumper.Visit(statement);
                dumper.AppendLine();
            }

            return builder.ToString();
        }

        public static string Dump(AstNode node)
        {
            var builder = new StringBuilder();
            var dumper = new AstDumper(builder);

            dumper.Visit(node);

            return builder.ToString();
        }

        private void IncreaseIndent()
        {
            indentLevel++;
        }

        private void DecreaseIndent()
        {
            indentLevel--;
        }

        private void Append(string text)
        {
            if (needsIndent)
            {
                builder.Append(' ', indentSize * indentLevel);
                needsIndent = false;
            }

            builder.Append(text);
        }

        private void AppendLine()
        {
            builder.AppendLine();
            needsIndent = true;
        }

        private void Parenthesize(Action action)
        {
            if (allowParentheses)
            {
                Append("(");
            }

            var oldAllowParentheses = allowParentheses;
            allowParentheses = true;

            action();

            allowParentheses = oldAllowParentheses;

            if (allowParentheses)
            {
                Append(")");
            }
        }

        private void ParenthesizedList(IReadOnlyList<AstNode> nodes)
        {
            Append("(");

            if (nodes.Count == 1)
            {
                allowParentheses = false;
            }

            var first = true;

            foreach (var node in nodes)
            {
                if (!first)
                {
                    Append(", ");
                }

                Visit(node);
                first = false;
            }

            allowParentheses = true;

            Append(")");
        }

        private void ParenthesizedList(AstNode node)
        {
            Append("(");
            allowParentheses = false;
            Visit(node);
            allowParentheses = true;
            Append(")");
        }

        public override void VisitAddExpression(AstAddExpression node)
        {
            Parenthesize(() =>
            {
                Visit(node.Left);
                Append(" + ");
                Visit(node.Right);
            });
        }

        public override void VisitCallExpression(AstCallExpression node)
        {
            Append("call ");
            Visit(node.Address);
            Append(" ");

            ParenthesizedList(node.Arguments);
        }

        public override void VisitCallWithArgCountExpression(AstCallWithArgCountExpression node)
        {
            Append("call ");
            Visit(node.Address);
            Append(" arg-count: ");
            Visit(node.ArgumentCount);
        }

        public override void VisitConstantExpression(AstConstantExpression node)
        {
            if (node.Value < 0)
            {
                Append("-");
                Append(Math.Abs(node.Value).ToString("x"));
            }
            else
            {
                Append(node.Value.ToString("x"));
            }
        }

        public override void VisitDivideExpression(AstDivideExpression node)
        {
            Parenthesize(() =>
            {
                Visit(node.Left);
                Append(" / ");
                Visit(node.Right);
            });
        }

        public override void VisitExpressionStatement(AstExpressionStatement node)
        {
            Visit(node.Expression);
        }

        public override void VisitLabel(AstLabel node)
        {
            Append($"label_{node.Index}");
        }

        public override void VisitLocal(AstLocal node)
        {
            Append("local_");

            if (node.Index.Kind == AstNodeKind.ConstantExpression)
            {
                var index = (AstConstantExpression)node.Index;
                Append(index.Value.ToString("X"));
            }
            else
            {
                Visit(node.Index);
            }
        }

        public override void VisitModuloExpression(AstModuloExpression node)
        {
            Parenthesize(() =>
            {
                Visit(node.Left);
                Append(" % ");
                Visit(node.Right);
            });
        }

        public override void VisitMultiplyExpression(AstMultiplyExpression node)
        {
            Parenthesize(() =>
            {
                Visit(node.Left);
                Append(" * ");
                Visit(node.Right);
            });
        }

        public override void VisitReadLocalExpression(AstReadLocalExpression node)
        {
            Visit(node.Local);
        }

        public override void VisitReadMemoryExpression(AstReadMemoryExpression node)
        {
            switch (node.Size)
            {
                case ValueSize.Byte:
                    Append("read.1");
                    break;
                case ValueSize.Word:
                    Append("read.2");
                    break;
                case ValueSize.DWord:
                    Append("read.4");
                    break;
            }

            ParenthesizedList(node.Address);
        }

        public override void VisitReturnStatement(AstReturnStatement node)
        {
            Append("return ");
            Visit(node.Expression);
        }

        public override void VisitStackPopExpression(AstStackPopExpression node)
        {
            Append("pop");
        }

        public override void VisitStackPushStatement(AstStackPushStatement node)
        {
            Append("push ");
            Visit(node.Value);
        }

        public override void VisitSubtractExpression(AstSubtractExpression node)
        {
            Parenthesize(() =>
            {
                Visit(node.Left);
                Append(" - ");
                Visit(node.Right);
            });
        }

        public override void VisitWriteLocalStatement(AstWriteLocalStatement node)
        {
            Visit(node.Local);
            Append(" <- ");
            Visit(node.Value);
        }

        public override void VisitWriteMemoryStatement(AstWriteMemoryStatement node)
        {
            switch (node.Size)
            {
                case ValueSize.Byte:
                    Append("write.1");
                    break;
                case ValueSize.Word:
                    Append("write.2");
                    break;
                case ValueSize.DWord:
                    Append("write.4");
                    break;
            }

            ParenthesizedList(node.Address);

            Append(" <- ");
            Visit(node.Value);
        }
    }
}
