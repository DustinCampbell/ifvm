using System;
using IFVM.Ast;
using IFVM.Collections;
using IFVM.Core;
using IFVM.FlowAnalysis;

namespace IFVM.Execution
{
    public partial class Interpreter
    {
        public static uint Execute(Function function, Machine machine)
        {
            var cfg = ControlFlowGraph.Compute(function.Body);
            var block = cfg.GetBlock(cfg.EntryBlock.Successors[0]);
            var result = 0u;

            while (!block.IsExit)
            {
                var nextBlockId = block.ID.GetNext();

                foreach (var statement in block.Statements)
                {
                    var jump = false;

                    void HandleReturnStatement(AstReturnStatement returnStatement)
                    {
                        result = Execute(returnStatement.Expression, machine);
                        nextBlockId = BlockId.Exit;
                        jump = true;
                    }

                    void HandleJumpStatement(AstJumpStatement jumpStatement)
                    {
                        nextBlockId = new BlockId(jumpStatement.Label.Index);
                        jump = true;
                    }

                    // Handle control flow
                    switch (statement.Kind)
                    {
                        case AstNodeKind.ReturnStatement:
                            HandleReturnStatement((AstReturnStatement)statement);
                            break;

                        case AstNodeKind.JumpStatement:
                            HandleJumpStatement((AstJumpStatement)statement);
                            break;

                        case AstNodeKind.BranchStatement:
                            {
                                var branchStatement = (AstBranchStatement)statement;
                                var condition = Execute(branchStatement.Condition, machine);
                                if (condition == 1)
                                {
                                    switch (branchStatement.Statement.Kind)
                                    {
                                        case AstNodeKind.ReturnStatement:
                                            HandleReturnStatement((AstReturnStatement)branchStatement.Statement);
                                            break;

                                        case AstNodeKind.JumpStatement:
                                            HandleJumpStatement((AstJumpStatement)branchStatement.Statement);
                                            break;

                                        default:
                                            throw new InvalidOperationException($"Invalid statement kind for branch: {branchStatement.Statement.Kind}");
                                    }
                                }

                                continue;
                            }
                    }

                    if (jump)
                    {
                        break;
                    }

                    Execute(statement, machine);
                }

                block = cfg.GetBlock(nextBlockId);
            }

            return result;
        }

        private static uint Execute(AstExpression expression, Machine machine)
        {
            switch (expression.Kind)
            {
                default:
                    throw new InvalidOperationException($"Invalid expression kind: {expression.Kind}");
            }
        }

        private static void Execute(AstStatement statement, Machine machine)
        {
            switch (statement.Kind)
            {
                case AstNodeKind.LabelStatement:
                    break;

                default:
                    throw new InvalidOperationException($"Invalid statement kind: {statement.Kind}");
            }
        }
    }
}
