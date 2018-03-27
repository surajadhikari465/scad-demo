using Mammoth.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSupport.DataAccess.Test.TestData
{
    public class RegionGpmStatusTestData
    {
        private const string testRegionWithGpm = "XX";
        private const bool testRegionWithGpmStatus = true;
        private const string testRegionNoGpm = "YY";
        private const bool testRegionNoGpmStatus = false;
        private const string testBadRegion = "ZZ";

        public static string ExistingRegionOnGpm => testRegionWithGpm;

        public static string ExistingRegionNotOnGpm => testRegionNoGpm;

        public static string NonExistentRegion => testBadRegion;

        public static bool GpmOn => testRegionWithGpmStatus;

        public static bool GpmOff => testRegionNoGpmStatus;

        public static string SqlForInsertingTestRegions
        {
            get
            {
                var sql = $@" 
                    INSERT INTO dbo.RegionGpmStatus(
                        Region,
                        IsGpmEnabled)
                    VALUES
                         ('{ExistingRegionOnGpm}', { (GpmOn ? "1" : "0")})
                        ,('{ExistingRegionNotOnGpm}', { (GpmOff ? "1" : "0")})
                ";

                return sql;
            }
        }

        public static string SqlForCurrentRegionCount
        {
            get
            {
                var sql = $@" 
                    SELECT Count(*) FROM dbo.RegionGpmStatus
                ";

                return sql;
            }
        }

        public static List<RegionGpmStatus> TestRegions
        {
            get
            {
                var regions = new List<RegionGpmStatus>
                {
                    new RegionGpmStatus {Region = ExistingRegionOnGpm,    IsGpmEnabled = GpmOn },
                    new RegionGpmStatus {Region = ExistingRegionNotOnGpm, IsGpmEnabled = GpmOff }
                };
                return regions;
            }
        }
    }
}
