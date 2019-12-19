using System.Collections.Generic;
using Icon.Web.DataAccess.Models;
using Icon.Common.DataAccess;

namespace Icon.Web.DataAccess.Queries
{
    public class GetRegionalSettingsByRegionParameters : IQuery<List<RegionalSettingsModel>>
    {
        public string RegionCode { get; set; }
    }
}
