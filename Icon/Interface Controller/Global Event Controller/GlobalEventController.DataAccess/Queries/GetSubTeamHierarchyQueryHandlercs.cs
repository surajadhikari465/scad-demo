using GlobalEventController.DataAccess.Infrastructure;
using Icon.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace GlobalEventController.DataAccess.Queries
{
    public class GetSubTeamHierarchyQueryHandlercs : IQueryHandler<GetSubTeamHierarchyQuery, HierarchyClass>
    {
        private readonly IconContext context;

        public GetSubTeamHierarchyQueryHandlercs(IconContext context)
        {
            this.context = context;
        }

        public HierarchyClass Handle(GetSubTeamHierarchyQuery parameters)
        {
            if (string.IsNullOrEmpty(parameters.HierarchyName))
            {
                throw new ArgumentOutOfRangeException("The value of HierarchyName must not be empty.");
            }

            HierarchyClass hierarchyClass = this.context.HierarchyClass
                .Include(hc => hc.HierarchyClassTrait)
                .Include(hc => hc.HierarchyClassTrait.Select(hct => hct.Trait))
                .Include(hc => hc.Hierarchy)
                .First(hc => hc.hierarchyClassName == parameters.HierarchyName);

            return hierarchyClass;
        }
    }
}
