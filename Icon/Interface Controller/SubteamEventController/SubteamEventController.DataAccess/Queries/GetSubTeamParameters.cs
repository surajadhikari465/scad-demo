using Icon.Common.DataAccess;
using Icon.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubteamEventController.DataAccess.Queries
{
    public class GetSubTeamParameters : IQuery<HierarchyClass>
    {
        public int SubTeamId { get; set; }
    }
}
