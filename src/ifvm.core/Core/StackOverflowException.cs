using System;

namespace IFVM.Core
{
    public class StackOverflowException : Exception
    {
        public StackOverflowException()
        {
        }

        public StackOverflowException(string message) : base(message)
        {
        }

        public StackOverflowException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
