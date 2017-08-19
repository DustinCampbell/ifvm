using System;
using System.Collections.Generic;

namespace IFVM.Ast
{
    public partial class AstBodyBuilder
    {
        private readonly List<bool> markedLabels;

        private void ValidateLabel(AstLabel label)
        {
            if (label.Index < 0 ||
                label.Index >= this.labels.Count ||
                this.labels[label.Index] != label)
            {
                throw new ArgumentException("AstLabel not created by this AstBodyBuilder.", nameof(label));
            }
        }

        public AstLabel NewLabel()
        {
            var index = this.labels.Count;
            var label = AstFactory.Label(index);
            this.labels.Add(label);
            this.markedLabels.Add(false);

            return label;
        }

        public void MarkLabel(AstLabel label)
        {
            ValidateLabel(label);

            if (this.markedLabels[label.Index])
            {
                throw new ArgumentException($"Label {label.Index} is already marked.", nameof(label));
            }

            this.markedLabels[label.Index] = true;

            AddStatement(
                AstFactory.LabelStatement(label));
        }

        private void PruneAndNormalizeLabels()
        {
            var usedLabels = new HashSet<AstLabel>();

            AstStatement previousStatement = null;
            foreach (var statement in this.statements)
            {
                switch (statement.Kind)
                {
                    case AstNodeKind.JumpStatement:
                        {
                            var jump = (AstJumpStatement)statement;
                            usedLabels.Add(jump.Label);
                            break;
                        }

                    case AstNodeKind.BranchStatement:
                        {
                            var branch = (AstBranchStatement)statement;
                            if (branch.Statement.Kind == AstNodeKind.JumpStatement)
                            {
                                var jump = (AstJumpStatement)branch.Statement;
                                usedLabels.Add(jump.Label);
                            }

                            break;
                        }

                    case AstNodeKind.LabelStatement:
                        {
                            if (previousStatement != null &&
                                previousStatement.Kind == AstNodeKind.BranchStatement)
                            {
                                var label = (AstLabelStatement)statement;
                                usedLabels.Add(label.Label);
                            }

                            break;
                        }
                }

                previousStatement = statement;
            }

            // Remove any unused labels.
            for (int i = this.statements.Count - 1; i >= 0; i--)
            {
                var statement = this.statements[i];

                if (statement.Kind == AstNodeKind.LabelStatement)
                {
                    var labelStatement = (AstLabelStatement)statement;
                    if (!usedLabels.Contains(labelStatement.Label))
                    {
                        this.statements.RemoveAt(i);
                    }
                }
            }

            // Ensure that there is a label at the start of the body.
            if (this.statements[0].Kind != AstNodeKind.LabelStatement)
            {
                var label = NewLabel();

                this.statements.Insert(0,
                    AstFactory.LabelStatement(label));
            }

            // Clear out the existing labels and recreate each used label.
            this.labels.Clear();
            this.markedLabels.Clear();

            var oldLabelToNewLabelMap = new Dictionary<int, AstLabel>();

            foreach (var statement in this.statements)
            {
                if (statement.Kind == AstNodeKind.LabelStatement)
                {
                    var labelStatement = (AstLabelStatement)statement;
                    oldLabelToNewLabelMap.Add(labelStatement.Label.Index, NewLabel());
                }
            }

            // Finally, rewrite each label to point to the new label.
            var rewriter = new LabelRewriter(oldLabelToNewLabelMap);
            for (int i = 0; i < this.statements.Count; i++)
            {
                this.statements[i] = (AstStatement)rewriter.Visit(this.statements[i]);
            }
        }

        private class LabelRewriter : AstRewriter
        {
            private readonly Dictionary<int, AstLabel> oldLabelToNewLabelMap;

            public LabelRewriter(Dictionary<int, AstLabel> oldLabelToNewLabelMap)
            {
                this.oldLabelToNewLabelMap = oldLabelToNewLabelMap;
            }

            public override AstNode VisitLabel(AstLabel node)
            {
                return this.oldLabelToNewLabelMap[node.Index];
            }
        }
    }
}
