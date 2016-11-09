using Icon.Web.DataAccess.Models;
using System.Collections.Generic;

namespace Icon.Web.DataAccess.Commands
{
    public class AddSubTeamEventsCommand
    {
        public int HierarchyClassId { get; set; }
        public List<RegionalSettingsModel> PosSubTeamRegionalSettingsList { get; set; }
    }
}
