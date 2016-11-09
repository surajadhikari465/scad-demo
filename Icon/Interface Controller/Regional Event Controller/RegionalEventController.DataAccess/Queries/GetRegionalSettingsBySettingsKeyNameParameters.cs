
using RegionalEventController.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RegionalEventController.Common.Models;

namespace RegionalEventController.DataAccess.Queries
{
    public class GetRegionalSettingsBySettingsKeyNameParameters : IQuery<List<RegionalSettingsModel>>
    {
        public string SettingsKeyName { get; set; }
    }
}
