using System;

namespace OutOfStock.Classes
{
    public class TimeZoneOffsetForRegion
    {
        public TimeZoneOffsetForRegion(int regionId, string regionAbbreviation, string regionName, Int16 timezoneOffset)
        {
            RegionId = regionId;
            RegionAbbreviation = regionAbbreviation;
            RegionName = regionName;
            TimezoneOffset = timezoneOffset;
        }

        public int RegionId { get; set; }
        public string RegionAbbreviation { get; set; }
        public string RegionName { get; set; }
        public Int16 TimezoneOffset { get; set; }

        public TimeZoneOffsetForRegion()
        {
        }
    }
}