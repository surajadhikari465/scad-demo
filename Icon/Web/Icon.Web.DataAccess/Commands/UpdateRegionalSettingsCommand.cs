using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Web.DataAccess.Commands
{
    public class UpdateRegionalSettingsCommand
    {
        public int RegionalSettingId { get; set; }
        public bool SettingValue { get; set; }
    }
}
