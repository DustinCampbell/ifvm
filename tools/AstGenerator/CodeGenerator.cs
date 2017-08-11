using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AstGenerator
{
    internal class CodeGenerator
    {
        private readonly TextWriter writer;
        private readonly Tree tree;

        private int indentLevel;
        private bool needsIndent = true;
        private const int IndentSize = 4;

        private CodeGenerator(TextWriter writer, Tree tree)
        {
            this.writer = writer;
            this.tree = tree;
        }

        private string NormalizeName(string name)
        {
            return name.StartsWith("Ast")
                ? name.Substring(3)
                : name;
        }

        private string CamelCaseName(string name)
        {
            return char.ToLower(name[0]) + name.Substring(1);
        }

        private string GetTypeName(Field field)
        {
            return field.IsList
                ? string.Format("ImmutableList<{0}>", field.Type)
                : field.Type;
        }

        private bool IsNodeType(Field field)
        {
            return tree.Nodes.Any(n => n.Name == field.Type);
        }

        private IEnumerable<Node> GetConcreteNodes()
        {
            return tree.Nodes.Where(n => !n.IsAbstract);
        }

        private Node GetBaseNode(Node node)
        {
            return tree.Nodes.FirstOrDefault(n => n.Name == node.Base);
        }

        private IEnumerable<Node> GetBaseNodes(Node node)
        {
            var bases = new Stack<Node>();

            var parent = node;
            while ((parent = GetBaseNode(parent)) != null)
            {
                bases.Push(parent);
            }

            while (bases.Count > 0)
            {
                yield return bases.Pop();
            }
        }

        private IEnumerable<Node> GetBaseNodesAndSelf(Node node)
        {
            var bases = new Stack<Node>();
            bases.Push(node);

            var parent = node;
            while ((parent = GetBaseNode(parent)) != null)
            {
                bases.Push(parent);
            }

            while (bases.Count > 0)
            {
                yield return bases.Pop();
            }
        }

        private IEnumerable<Field> GetInheritedFields(Node node)
        {
            foreach (var baseNode in GetBaseNodes(node))
            {
                foreach (var field in baseNode.Fields)
                {
                    yield return field;
                }
            }
        }

        private IEnumerable<Field> GetDeclaredFields(Node node)
        {
            foreach (var field in node.Fields)
            {
                yield return field;
            }
        }

        private IEnumerable<Field> GetAllFields(Node node)
        {
            foreach (var baseNode in GetBaseNodesAndSelf(node))
            {
                foreach (var field in baseNode.Fields)
                {
                    yield return field;
                }
            }
        }

        private IEnumerable<Field> GetAllVisitableFields(Node node)
        {
            return GetAllFields(node).Where(f => !f.DoNotVisit && IsNodeType(f));
        }

        private void Indent()
        {
            indentLevel++;
        }

        private void Unindent()
        {
            indentLevel--;
        }

        private void Write(string format, params object[] args)
        {
            if (needsIndent)
            {
                writer.Write(new string(' ', indentLevel * IndentSize));
                needsIndent = false;
            }

            writer.Write(format, args);
        }

        private void WriteLine(string format, params object[] args)
        {
            Write(format, args);
            writer.WriteLine();
            needsIndent = true;
        }

        private void BlankLine()
        {
            writer.WriteLine();
            needsIndent = true;
        }

        private void OpenBrace()
        {
            WriteLine("{{");
            Indent();
        }

        private void CloseBrace()
        {
            Unindent();
            WriteLine("}}");
        }

        private void WriteUsingDirectives()
        {
            foreach (var usingDirective in tree.UsingDirectives)
            {
                WriteLine("using {0};", usingDirective.Name);
            }
        }

        private void WriteNamespaceStart()
        {
            WriteLine("namespace {0}", tree.Namespace);
            OpenBrace();
        }

        private void WriteNamespaceEnd()
        {
            CloseBrace();
        }

        private void WriteNodeKinds()
        {
            WriteLine("public enum AstNodeKind");
            OpenBrace();

            foreach (var node in GetConcreteNodes())
            {
                WriteLine("{0},", NormalizeName(node.Name));
            }
            CloseBrace();
        }
        private void WriteNodeStart(Node node)
        {
            Write("public ");
            if (node.IsAbstract)
            {
                Write("abstract ");
            }
            WriteLine("partial class {0} : {1}", node.Name, node.Base);
            OpenBrace();
        }
        private void WriteNodeFields(Node node)
        {
            foreach (var field in GetDeclaredFields(node))
            {
                WriteLine("private readonly {0} {1};", GetTypeName(field), CamelCaseName(field.Name));
            }
        }
        private void WriteNodeConstructor(Node node)
        {
            if (node.Fields.Any())
            {
                BlankLine();
            }
            var inheritedFields = GetInheritedFields(node);
            var declaredFields = GetDeclaredFields(node);
            var allFields = GetAllFields(node);
            var allParameters = allFields.Select(f => string.Format("{0} {1}", GetTypeName(f), CamelCaseName(f.Name)));
            if (node.IsAbstract)
            {
                var parameterList = allFields.Any()
                    ? "AstNodeKind kind, " + string.Join(", ", allParameters)
                    : "AstNodeKind kind";
                var baseList = inheritedFields.Any()
                    ? "kind, " + string.Join(", ", inheritedFields.Select(f => CamelCaseName(f.Name)))
                    : "kind";
                WriteLine("internal {0}({1}) : base({2})", node.Name, parameterList, baseList);
                OpenBrace();
                foreach (var field in declaredFields)
                {
                    WriteLine("this.{0} = {0};", CamelCaseName(field.Name));
                }
                CloseBrace();
            }
            else
            {
                var parameterList = string.Join(", ", allParameters);
                var enumName = "AstNodeKind." + NormalizeName(node.Name);
                var baseList = inheritedFields.Any()
                    ? enumName + ", " + string.Join(", ", inheritedFields.Select(f => CamelCaseName(f.Name)))
                    : enumName;
                WriteLine("internal {0}({1}) : base({2})", node.Name, parameterList, baseList);
                OpenBrace();
                foreach (var field in declaredFields)
                {
                    WriteLine("this.{0} = {0};", CamelCaseName(field.Name));
                }
                CloseBrace();
            }
        }
        private void WriteNodeProperties(Node node)
        {
            foreach (var field in node.Fields)
            {
                BlankLine();
                WriteLine("public {0} {1}", GetTypeName(field), field.Name);
                OpenBrace();
                WriteLine("get {{ return this.{0}; }}", CamelCaseName(field.Name));
                CloseBrace();
            }
        }
        private void WriteNodeAcceptMethods(Node node)
        {
            BlankLine();
            WriteLine("public override AstNode Accept(AstRewriter rewriter)");
            OpenBrace();
            WriteLine("return rewriter.Visit{0}(this);", NormalizeName(node.Name));
            CloseBrace();
            BlankLine();
            WriteLine("public override void Accept(AstVisitor visitor)");
            OpenBrace();
            WriteLine("visitor.Visit{0}(this);", NormalizeName(node.Name));
            CloseBrace();
        }
        private void WriteNodeAccessMethods(Node node)
        {
            BlankLine();
            WriteLine("public override ImmutableList<AstNode> ChildNodes()");
            OpenBrace();
            var visitableFields = GetAllVisitableFields(node);
            if (visitableFields.Any())
            {
                WriteLine("var builder = ImmutableList.CreateBuilder<AstNode>();");
                BlankLine();
                var needsBlankLine = false;
                foreach (var field in visitableFields)
                {
                    if (needsBlankLine)
                    {
                        BlankLine();
                        needsBlankLine = false;
                    }
                    if (field.IsList)
                    {
                        WriteLine("foreach (var child in {0})", NormalizeName(field.Name));
                        OpenBrace();
                        WriteLine("builder.Add(child);");
                        CloseBrace();
                        needsBlankLine = true;
                    }
                    else
                    {
                        WriteLine("builder.Add({0});", NormalizeName(field.Name));
                    }
                }

                BlankLine();
                WriteLine("return builder.ToImmutable();");
            }
            else
            {
                WriteLine("return ImmutableList<AstNode>.Empty;");
            }

            CloseBrace();
        }

        private void WriteNodeEnd()
        {
            CloseBrace();
        }

        private void WriteNodes()
        {
            foreach (var node in tree.Nodes)
            {
                BlankLine();
                WriteNodeStart(node);
                WriteNodeFields(node);
                WriteNodeConstructor(node);
                WriteNodeProperties(node);

                if (!node.IsAbstract)
                {
                    WriteNodeAcceptMethods(node);
                    WriteNodeAccessMethods(node);
                }

                WriteNodeEnd();
            }
        }

        private void WriteVisitor()
        {
            BlankLine();

            WriteLine("public abstract partial class AstVisitor");
            OpenBrace();

            WriteLine("public virtual void Visit(AstNode node)");
            OpenBrace();
            WriteLine("node.Accept(this);");
            CloseBrace();

            BlankLine();
            WriteLine("public void VisitList<TNode>(ImmutableList<TNode> list) where TNode : AstNode");
            OpenBrace();
            WriteLine("foreach (var item in list)");
            OpenBrace();
            WriteLine("Visit(item);");
            CloseBrace();
            CloseBrace();

            foreach (var node in GetConcreteNodes())
            {
                BlankLine();

                WriteLine("public virtual void Visit{0}({1} node)", NormalizeName(node.Name), node.Name);
                OpenBrace();

                foreach (var field in GetAllVisitableFields(node))
                {
                    WriteLine("{0}(node.{1});", field.IsList ? "VisitList" : "Visit", field.Name);
                }

                CloseBrace();
            }

            CloseBrace();
        }

        private void WriteRewriter()
        {
            BlankLine();

            WriteLine("public abstract partial class AstRewriter");
            OpenBrace();

            WriteLine("public virtual AstNode Visit(AstNode node)");
            OpenBrace();
            WriteLine("return node.Accept(this);");
            CloseBrace();

            foreach (var node in GetConcreteNodes())
            {
                BlankLine();

                WriteLine("public virtual AstNode Visit{0}({1} node)", NormalizeName(node.Name), node.Name);
                OpenBrace();

                var vistableFields = GetAllVisitableFields(node);

                foreach (var field in vistableFields)
                {
                    if (field.IsList)
                    {
                        WriteLine("var {0} = VisitList(node.{1});", CamelCaseName(field.Name), field.Name);
                    }
                    else
                    {
                        WriteLine("var {0} = ({1})Visit(node.{2});", CamelCaseName(field.Name), GetTypeName(field), field.Name);
                    }
                }

                if (vistableFields.Any())
                {
                    var comparisons = vistableFields.Select(f => string.Format("{0} != node.{1}", CamelCaseName(f.Name), f.Name));
                    var parameters = GetAllFields(node).Select(f => f.DoNotVisit ? string.Format("node.{0}", f.Name) : CamelCaseName(f.Name));

                    BlankLine();

                    if (vistableFields.Any())
                    {
                        WriteLine("return {0}", string.Join(" || ", comparisons));
                        Indent();
                        WriteLine("? new {0}({1})", node.Name, string.Join(", ", parameters));
                        WriteLine(": node;");
                        Unindent();
                    }
                    else
                    {
                        WriteLine("return new {0}({1});", node.Name, string.Join(", ", parameters));
                    }
                }
                else
                {
                    WriteLine("return node;");
                }

                CloseBrace();
            }

            CloseBrace();
        }

        private void WriteFactory()
        {
            BlankLine();

            WriteLine("public static partial class AstFactory");
            OpenBrace();

            var isFirst = true;

            foreach (var node in GetConcreteNodes())
            {
                if (node.DontCreateFactoryMethod)
                {
                    continue;
                }

                if (!isFirst)
                {
                    BlankLine();
                }

                var parameterNames = GetAllFields(node).Select(f => CamelCaseName(f.Name));
                var parameters = GetAllFields(node).Select(f => string.Format("{0} {1}", GetTypeName(f), CamelCaseName(f.Name)));

                WriteLine("public static {0} {1}({2})", node.Name, NormalizeName(node.Name), string.Join(", ", parameters));
                OpenBrace();

                WriteLine("return new {0}({1});", node.Name, string.Join(", ", parameterNames));

                CloseBrace();

                isFirst = false;
            }

            CloseBrace();
        }

        private void WriteFile()
        {
            WriteLine("// <auto-generated />");
            BlankLine();
            WriteUsingDirectives();
            BlankLine();
            WriteNamespaceStart();
            WriteNodeKinds();
            WriteNodes();
            WriteVisitor();
            WriteRewriter();
            WriteFactory();
            WriteNamespaceEnd();
        }

        public static void Write(TextWriter writer, Tree tree)
        {
            var generator = new CodeGenerator(writer, tree);
            generator.WriteFile();
        }
    }
}
