using System;
using System.Threading;

namespace IFVM.Utilities
{
    public class ObjectPool<T> where T : class
    {
        private struct Item
        {
            public T Value;
        }

        private T _firstItem;
        private readonly Item[] _items;
        private readonly Func<T> _createItem;

        public ObjectPool(Func<T> createItem)
            : this(createItem, Environment.ProcessorCount * 2)
        {
        }

        public ObjectPool(Func<T> createItem, int size)
        {
            if (size < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(size));
            }

            _createItem = createItem ?? throw new ArgumentNullException(nameof(createItem));
            _items = new Item[size - 1];
        }

        public T Allocate()
        {
            // First, examine the first item. If that fails, call AllocateSlow to search the remaining items.
            // Note that the read below is optimistically not synchronized. The idea here is to only interlock when
            // we actually have a candidate. In the worst case, we may miss some recently returned objects,
            // which is acceptable.
            var result = _firstItem;

            if (result == null || result != Interlocked.CompareExchange(ref _firstItem, null, result))
            {
                result = AllocateSlow();
            }

            return result;
        }

        private T AllocateSlow()
        {
            var items = _items;

            for (int i = 0; i < items.Length; i++)
            {
                // Note that the read below is optimistically not synchronized. The idea here is to only interlock when
                // we actually have a candidate. In the worst case, we may miss some recently returned objects,
                // which is acceptable.
                var result = items[i].Value;

                if (result != null && result == Interlocked.CompareExchange(ref items[i].Value, null, result))
                {
                    return result;
                }
            }

            return _createItem();
        }

        public void Free(T obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            if (_firstItem == null)
            {
                // We intentionally don't use interlocked here. At worst, an object was stored in _firstItem
                // after we checked it for null and that object will be collected.
                _firstItem = obj;
            }
            else
            {
                FreeSlow(obj);
            }
        }

        private void FreeSlow(T obj)
        {
            var items = _items;

            for (int i = 0; i < items.Length; i++)
            {
                if (items[i].Value == null)
                {
                    // We intentionally don't use interlocked here. At worst, an object was stored in items at this slot
                    // after we checked it for null and that object will be collected.
                    items[i].Value = obj;
                    break;
                }
            }
        }
    }
}
