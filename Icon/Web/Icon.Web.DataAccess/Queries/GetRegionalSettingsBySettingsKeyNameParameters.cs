using System.Collections.Generic;
using Icon.Web.DataAccess.Models;
using Icon.Common.DataAccess;

namespace Icon.Web.DataAccess.Queries
{
    public class GetRegionalSettingsBySettingsKeyNameParameters : IQuery<List<RegionalSettingsModel>>
    {
        public string SettingsKeyName { get; set; }
    }
}
