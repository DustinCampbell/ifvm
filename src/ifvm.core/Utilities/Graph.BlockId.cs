using System;

namespace IFVM.Utilities
{
    public partial class Graph
    {
        public struct BlockId : IEquatable<BlockId>, IComparable<BlockId>
        {
            private const int EntryValue = -2;
            private const int ExitValue = -1;

            public static readonly BlockId Entry = new BlockId(EntryValue);
            public static readonly BlockId Exit = new BlockId(ExitValue);
            public static readonly BlockId Initial = new BlockId(0);

            private readonly int _value;

            public BlockId(int value)
            {
                _value = value;
            }

            public BlockId GetNext()
            {
                if (_value == EntryValue || _value == ExitValue)
                {
                    throw new InvalidOperationException("Cannot get the next block ID of the entry or exit blocks");
                }

                return new BlockId(_value + 1);
            }

            public override bool Equals(object obj)
                => obj is BlockId && Equals((BlockId)obj);

            public bool Equals(BlockId other)
                => _value == other._value;

            public override int GetHashCode()
            {
                var hashCode = 1305186703;
                hashCode = hashCode * -1521134295 + base.GetHashCode();
                hashCode = hashCode * -1521134295 + _value.GetHashCode();
                return hashCode;
            }

            public int CompareTo(BlockId other)
            {
                if (_value == other._value)
                {
                    return 0;
                }

                if (_value == EntryValue || other._value == ExitValue)
                {
                    return int.MinValue;
                }

                if (_value == ExitValue || other._value == EntryValue)
                {
                    return int.MaxValue;
                }

                return _value.CompareTo(other._value);
            }

            public override string ToString()
            {
                switch (this._value)
                {
                    case EntryValue: return "{Entry Block}";
                    case ExitValue: return "{Exit Block}";
                    default: return $"{{Block: {_value}}}";
                }
            }

            public static bool operator ==(BlockId id1, BlockId id2)
                => id1.Equals(id2);

            public static bool operator !=(BlockId id1, BlockId id2)
                => !(id1 == id2);

            public static bool operator <(BlockId id1, BlockId id2)
                => id1.CompareTo(id2) < 0;
            public static bool operator <=(BlockId id1, BlockId id2)
                => id1.CompareTo(id2) <= 0;
            public static bool operator >(BlockId id1, BlockId id2)
                => id1.CompareTo(id2) > 0;
            public static bool operator >=(BlockId id1, BlockId id2)
                => id1.CompareTo(id2) >= 0;
        }
    }
}
