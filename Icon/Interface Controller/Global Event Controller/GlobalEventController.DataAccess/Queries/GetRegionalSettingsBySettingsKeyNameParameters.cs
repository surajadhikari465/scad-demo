using GlobalEventController.DataAccess.Infrastructure;
using GlobalEventController.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Icon.Framework;

namespace GlobalEventController.DataAccess.Queries
{
    public class GetRegionalSettingsBySettingsKeyNameParameters : IQuery<List<RegionalSettingsModel>>
    {
        public string SettingsKeyName { get; set; }
    }
}
