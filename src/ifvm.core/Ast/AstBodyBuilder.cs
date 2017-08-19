using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace IFVM.Ast
{
    public partial class AstBodyBuilder
    {
        private readonly ImmutableList<AstLabel>.Builder labels;
        private readonly ImmutableList<AstLocal>.Builder locals;
        private readonly ImmutableList<AstStatement>.Builder statements;

        public AstBodyBuilder()
        {
            this.labels = ImmutableList.CreateBuilder<AstLabel>();
            this.locals = ImmutableList.CreateBuilder<AstLocal>();
            this.statements = ImmutableList.CreateBuilder<AstStatement>();
            this.markedLabels = new List<bool>();
        }

        public void AddStatement(AstStatement statement)
        {
            this.statements.Add(statement);
        }

        public AstLocal DeclareLocal(ValueSize size, AstExpression value = null)
        {
            var index = AstFactory.ConstantExpression(this.locals.Count);
            var local = AstFactory.Local(index, size);
            this.locals.Add(local);

            if (value != null)
            {
                WriteLocal(local, value);
            }

            return local;
        }

        public void WriteLocal(AstLocal local, AstExpression value)
        {
            AddStatement(
                AstFactory.WriteLocalStatement(local, value));
        }

        public void Quit()
        {
            AddStatement(
                AstFactory.QuitStatement());
        }

        public void Return(AstExpression expression)
        {
            AddStatement(
                AstFactory.ReturnStatement(expression));
        }

        public AstBody ToBody()
        {
            if (this.markedLabels.Contains(false))
            {
                throw new InvalidOperationException("There are labels that have not been marked.");
            }

            PruneAndNormalizeLabels();

            return new AstBody(
                this.labels.ToImmutable(),
                this.locals.ToImmutable(),
                this.statements.ToImmutable());
        }
    }
}
