using System.Collections.Generic;

namespace IFVM.Core
{
    public abstract class Machine
    {
        public Memory Memory { get; }
        public Stack Stack { get; }

        private readonly Dictionary<int, Function> _functionCache;

        protected Machine(Memory memory, Stack stack)
        {
            this.Memory = memory;
            this.Stack = stack;

            _functionCache = new Dictionary<int, Function>();
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
    }
}
