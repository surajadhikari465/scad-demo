using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubteamEventController.DataAccess.Commands
{
    public class UpdateSubTeamCommand
    {
        public int PosDepartmentNumber { get; set; }
        public string SubTeamName { get; set; }
        public int SubTeamNumber { get; set; }
        public string TeamName { get; set; }
        public int TeamNumber { get; set; }
        public object Region { get; set; }
    }
}
