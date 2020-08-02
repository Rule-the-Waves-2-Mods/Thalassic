using Thalassic.Mods;
using Semver;
using System;

namespace Thalassic
{
    public class Rtw2Version : IComparable<Rtw2Version>
    {
        public SemVersion Version { get; }
        public string Checksum { get; }

        public Rtw2Version(SemVersion version, string checksum)
        {
            if (version == null || checksum == null)
            {
                throw new ArgumentNullException();
            }

            Version = version;
            Checksum = checksum;
        }

        public int CompareTo(Rtw2Version other)
        {
            return Version.CompareByPrecedence(other.Version);
        }

        // TODO this is dead code I'm keeping around in case we end up wanting to tackle version compatibility
        public bool CompatibleWith(ModCompabitilityVersion modCompatibility)
        {
            if (modCompatibility.Major != -1 && Version.Major != modCompatibility.Major)
            {
                return false;
            }
            else if (modCompatibility.Minor != -1 && Version.Minor != modCompatibility.Minor)
            {
                return false;
            }
            else if (modCompatibility.Patch != -1 && Version.Patch != modCompatibility.Patch)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
