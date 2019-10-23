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
        public static class National
        {
            public static class HierarchyLevels
            {
                public const string Family = "National Family";
                public const string Category = "National Category";
                public const string SubCategory = "National Sub Category";
				public const string NationalClass = "National Class";
            }
        }
    }
}
