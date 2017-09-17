using System;
using System.Collections.Immutable;
using IFVM.Execution;

namespace IFVM.Core
{
    public class CallFrame
    {
        private readonly Machine _machine;
        private readonly Function _function;
        private readonly ImmutableArray<uint> _arguments;
        private readonly uint[] _locals;

        public int StackPointer { get; }

        public CallFrame(Machine machine, Function function, ImmutableArray<uint> arguments)
        {
            _machine = machine ?? throw new ArgumentNullException(nameof(machine));
            _function = function ?? throw new ArgumentNullException(nameof(function));
            _arguments = arguments;
            _locals = new uint[function.Body.Locals.Count];
            StackPointer = machine.Stack.Pointer;

            SetUpArguments();
        }

        private void SetUpArguments()
        {
            var machine = _machine;
            var function = _function;
            var arguments = _arguments;
            var locals = _locals;

            switch (function.Kind)
            {
                case FunctionKind.StackArgument:
                    // Push arguments on stack in reverse order
                    for (int i = arguments.Length - 1; i >= 0; i--)
                    {
                        machine.Stack.PushDWord(arguments[i]);
                    }

                    // Push argument count onto stack
                    machine.Stack.PushDWord((uint)arguments.Length);

                    break;

                case FunctionKind.LocalArgument:
                    // Pass arguments into locals
                    var count = Math.Min(locals.Length, arguments.Length);
                    for (int i = 0; i < count; i++)
                    {
                        locals[i] = arguments[i];
                    }

                    break;
            }
        }

        public uint ReadLocal(int index)
        {
            ValidateLocalIndex(index);
            return _locals[index];
        }

        public void WriteLocal(int index, uint value)
        {
            ValidateLocalIndex(index);
            _locals[index] = value;
        }

        private void ValidateLocalIndex(int index)
        {
            if (index < 0 || index >= _locals.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(index), index,
                    $"Attempted to write to local {index}, but there are only 0 to {_locals.Length - 1} locals.");
            }
        }

        public uint Run()
        {
            var interpreter = new Interpreter(_machine);
            return interpreter.Evaluate(_function);
        }
    }
}
