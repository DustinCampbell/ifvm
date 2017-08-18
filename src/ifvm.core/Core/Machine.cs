namespace IFVM.Core
{
    public abstract class Machine
    {
        public Memory Memory { get; }
        public Stack Stack { get; }

        protected Machine(Memory memory, Stack stack)
        {
            this.Memory = memory;
            this.Stack = stack;
        }
    }
}
