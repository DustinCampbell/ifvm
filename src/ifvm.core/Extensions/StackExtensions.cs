using System.Collections.Generic;

namespace IFVM.Extensions
{
    public static class StackExtensions
    {
        public static bool TryPop<T>(this Stack<T> stack, out T value)
        {
            if (stack.Count == 0)
            {
                value = default;
                return false;
            }

            value = stack.Pop();
            return true;
        }
    }
}
