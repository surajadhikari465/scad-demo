using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OOSCommon.DataContext;

namespace OOS.Model.Repository
{
    public class SQLRegionFactory
    {
        private SQLRegionFactory()
        {}

        public static Region Reconstitute(REGION region)
        {
            var r = new Region(region.ID) {Abbrev = region.REGION_ABBR};
            return r;
        }
    }
}
