using System;
using IFVM.Ast;
using IFVM.Collections;
using IFVM.Core;
using IFVM.FlowAnalysis;

namespace IFVM.Execution
{
    public partial class Interpreter
    {
        private readonly Machine _machine;

        public Interpreter(Machine machine)
        {
            _machine = machine ?? throw new ArgumentNullException(nameof(machine));
        }

        public uint Execute(Function function)
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

                    // First, handle any control flow statements

                    void HandleReturnStatement(AstReturnStatement returnStatement)
                    {
                        result = Execute(returnStatement.Expression);
                        nextBlockId = BlockId.Exit;
                        jump = true;
                    }

                    void HandleJumpStatement(AstJumpStatement jumpStatement)
                    {
                        nextBlockId = new BlockId(jumpStatement.Label.Index);
                        jump = true;
                    }

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
                                var condition = Execute(branchStatement.Condition);
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

                    Execute(statement);
                }

                block = cfg.GetBlock(nextBlockId);
            }

            return result;
        }
    }
}
