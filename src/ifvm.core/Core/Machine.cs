using System.Collections.Generic;
using System.Collections.Immutable;

namespace IFVM.Core
{
    public abstract class Machine
    {
        public Memory Memory { get; }
        public Stack Stack { get; }

        private readonly int _startFunctionAddress;
        private readonly Dictionary<int, Function> _functionCache;
        private readonly Stack<CallFrame> _callFrames;

        public Function StartFunction => GetFunction(_startFunctionAddress);
        public CallFrame CurrentFrame => _callFrames.Peek();

        protected Machine(Memory memory, Stack stack, int startFunctionAddress)
        {
            this.Memory = memory;
            this.Stack = stack;
            _startFunctionAddress = startFunctionAddress;

            _functionCache = new Dictionary<int, Function>();
            _callFrames = new Stack<CallFrame>(capacity: 1024);
        }

        protected abstract Function ReadFunction(int address);

        public Function GetFunction(int address)
        {
            if (!_functionCache.TryGetValue(address, out var function))
            {
                function = ReadFunction(address);
                _functionCache.Add(address, function);
            }

            return function;
        }

        internal uint CallFunction(int address, ImmutableArray<uint> arguments)
        {
            var function = GetFunction(address);
            var frame = new CallFrame(this, function, arguments);

            _callFrames.Push(frame);
            var result = frame.Run();
            _callFrames.Pop();

            this.Stack.SetPointer(frame.StackPointer);

            return result;
        }

        public void Run()
        {
            CallFunction(_startFunctionAddress, ImmutableArray<uint>.Empty);
        }
    }
}
