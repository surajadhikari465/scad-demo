using GlobalEventController.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubteamEventController.DataAccess.Commands
{
    public class UpdateItemSubTeamCommand
    {
        public ItemSubTeamModel UpdatedItemSubTeam { get; set; }
        public int UserId { get; set; }
        public int Category_ID { get; set; }
    }
}
