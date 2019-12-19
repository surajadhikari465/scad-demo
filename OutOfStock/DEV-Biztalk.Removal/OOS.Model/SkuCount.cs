using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOS.Model
{
    public class SkuCount
    {
        private string storeAbbrev;
        private string team;
        private int count;

        public SkuCount(string storeAbbrev, string team, int count)
        {
            this.storeAbbrev = storeAbbrev;
            this.team = team;
            this.count = count;
        }

        public string StoreAbbreviation { get { return storeAbbrev; } }
        public string Team { get { return team; } }
        public int Count { get { return count; } }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            
            var skuCount = obj as SkuCount;
            return skuCount != null && Equals(skuCount);
        }

        public bool Equals(SkuCount skuCount)
        {
            if (skuCount == null) return false;
            return StoreAbbreviation == skuCount.StoreAbbreviation && Team == skuCount.Team;
        }

        public static bool operator ==(SkuCount leftSide, SkuCount rightSide)
        {
            if (ReferenceEquals(leftSide, rightSide)) return true;
            if (((object)leftSide == null) || ((object)rightSide == null)) return false;

            return leftSide.Equals(rightSide);
        }

        public static bool operator !=(SkuCount leftSide, SkuCount rightSide)
        {
            return !(leftSide == rightSide);
        }

        public override int GetHashCode()
        {
            return Team.GetHashCode() ^ StoreAbbreviation.GetHashCode();
        }
    }
}
