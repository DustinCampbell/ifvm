namespace IFVM.Core
{
    public abstract class Machine
    {
        public Memory Memory { get; }

        protected Machine(Memory memory)
        {
            this.Memory = memory;
        }
    }
}
