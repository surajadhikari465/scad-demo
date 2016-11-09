using SubteamEventController.DataAccess.Commands;
using SubteamEventController.DataAccess.Queries;
using Irma.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubteamEventController.DataAccess.DataServices
{
    public class UpdateItemSubTeamDataService
    {
        public AddUpdateItemSubTeamLastChangeCommand LastChangeCommand { get; set; }
        public UpdateItemSubTeamCommand ItemCommand { get; set; }
        public List<ItemIdentifier> ItemIdentifiers { get; set; }
        public string Region { get; set; }
    }
}
