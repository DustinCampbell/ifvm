﻿using System.Collections.Immutable;

namespace IFVM.Ast
{
    public class AstBodyBuilder
    {
        private readonly ImmutableList<AstLabel>.Builder labels;
        private readonly ImmutableList<AstLocal>.Builder locals;
        private readonly ImmutableList<AstStatement>.Builder statements;

        public AstBodyBuilder()
        {
            this.labels = ImmutableList.CreateBuilder<AstLabel>();
            this.locals = ImmutableList.CreateBuilder<AstLocal>();
            this.statements = ImmutableList.CreateBuilder<AstStatement>();
        }

        public void AddStatement(AstStatement statement)
        {
            this.statements.Add(statement);
        }

        public AstLocal DeclareLocal(AstExpression value = null, ValueSize size = ValueSize.DWord)
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

        public AstBody ToBody()
            => new AstBody(
                this.labels.ToImmutable(),
                this.locals.ToImmutable(),
                this.statements.ToImmutable());
    }
}