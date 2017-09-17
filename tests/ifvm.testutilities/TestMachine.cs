using System;
using IFVM.Core;

namespace IFVM.TestUtilities
{
    public class TestMachine : Machine
    {
        public TestMachine(Memory memory = null, Stack stack = null)
            : base(
                  memory: memory ?? Memory.Create(new byte[0]),
                  stack: stack ?? new Stack(1024))
        {
        }

        protected override Function ReadFunction(int address)
        {
            throw new NotSupportedException();
        }
    }
}
