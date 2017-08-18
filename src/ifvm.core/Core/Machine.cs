namespace IFVM.Core
{
    public abstract class Machine
    {
        protected readonly Memory Memory;

        protected Machine(Memory memory)
        {
            this.Memory = memory;
        }
    }
}
