using Icon.Framework;
using Icon.Web.DataAccess.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Icon.Web.DataAccess.Models;
using Icon.Common.DataAccess;

namespace Icon.Web.DataAccess.Queries
{
    public class GetRegionalSettingsBySettingsKeyNameParameters : IQuery<List<RegionalSettingsModel>>
    {
        public string SettingsKeyName { get; set; }
    }
}
