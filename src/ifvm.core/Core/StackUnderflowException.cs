using System;

namespace IFVM.Core
{
    public class StackUnderflowException : Exception
    {
        public StackUnderflowException()
        {
        }

        public StackUnderflowException(string message) : base(message)
        {
        }

        public StackUnderflowException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
