using System;

namespace IFVM.Glulx
{
    public struct GlulxVersion : IEquatable<GlulxVersion>
    {
        public int Major { get; }
        public int Minor { get; }
        public int SubMinor { get; }

        public GlulxVersion(int major, int minor, int subMinor)
        {
            if (major < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(major));
            }

            if (minor < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(major));
            }

            if (subMinor < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(subMinor));
            }

            Major = major;
            Minor = minor;
            SubMinor = subMinor;
        }

        public override bool Equals(object obj)
            => obj is GlulxVersion
                && Equals((GlulxVersion)obj);

        public bool Equals(GlulxVersion other)
            => Major == other.Major
                && Minor == other.Minor
                && SubMinor == other.SubMinor;

        public override int GetHashCode()
        {
            var hashCode = -215608226;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + Major.GetHashCode();
            hashCode = hashCode * -1521134295 + Minor.GetHashCode();
            hashCode = hashCode * -1521134295 + SubMinor.GetHashCode();
            return hashCode;
        }

        public override string ToString()
            => $"{Major}.{Minor}.{SubMinor}";

        public static bool operator ==(GlulxVersion version1, GlulxVersion version2)
            => version1.Equals(version2);

        public static bool operator !=(GlulxVersion version1, GlulxVersion version2)
            => !(version1 == version2);
    }
}
