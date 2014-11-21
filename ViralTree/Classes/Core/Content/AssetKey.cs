using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViralTree
{
    public struct AssetKey : IEquatable<AssetKey>
    {
        public readonly Type assetType;
        public readonly string assetName;

        public AssetKey(Type type, string name)
        {
            this.assetType = type;
            this.assetName = name;
        }

        public bool Equals(AssetKey other)
        {
            return assetType == other.assetType && assetName.Equals(other.assetName);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            else
                return obj is AssetKey && Equals((AssetKey)obj);
        }

        //TODO: maybe add own hashCode
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return "Type: " + assetType + ", Name: " + assetName;
        }

        public static bool operator ==(AssetKey one, AssetKey other)
        {
            return one.Equals(other);
        }

        public static bool operator !=(AssetKey one, AssetKey other)
        {
            return !one.Equals(other);
        }

    }
}
