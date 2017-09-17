using System.Collections.Generic;

namespace IFVM.Utilities
{
    public static class ObjectPoolExtensions
    {
        private const int Threshold = 512;

        public static Stack<T> AllocateAndClear<T>(this ObjectPool<Stack<T>> pool)
        {
            var stack = pool.Allocate();
            stack.Clear();

            return stack;
        }

        public static void ClearAndFree<T>(this ObjectPool<Stack<T>> pool, Stack<T> stack)
        {
            if (stack == null)
            {
                return;
            }

            var count = stack.Count;
            stack.Clear();

            if (count > Threshold)
            {
                stack.TrimExcess();
            }

            pool.Free(stack);
        }
    }
}
