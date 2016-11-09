using Icon.Common.DataAccess;
using Icon.Framework;
using System.Data.Entity;
using System.Linq;

namespace SubteamEventController.DataAccess.Queries
{
    public class GetSubTeamQuery : IQueryHandler<GetSubTeamParameters, HierarchyClass>
    {
        private IconContext context;

        public GetSubTeamQuery(IconContext context)
        {
            this.context = context;
        }

        public HierarchyClass Search(GetSubTeamParameters parameters)
        {
            return context.HierarchyClass
                .Include(hc => hc.HierarchyClassTrait)
                .FirstOrDefault(hc => hc.hierarchyClassID == parameters.SubTeamId);
        }
    }
}
