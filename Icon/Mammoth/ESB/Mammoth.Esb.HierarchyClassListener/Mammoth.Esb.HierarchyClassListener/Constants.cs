using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mammoth.Esb.HierarchyClassListener
{
    public static class Constants 
    {
        public static class Brands
        {
            public static class HierarchyLevels
            {
                public const string Brand = "Brand";
            }
        }
        public static class Merchandise
        {
            public static class HierarchyLevels
            {
                public const string Segment = "Segment";
                public const string Family = "Family";
                public const string Class = "Class";
                public const string Gs1Brick = "GS1 Brick";
                public const string SubBrick = "Sub Brick";
            }
        }
    }
}
