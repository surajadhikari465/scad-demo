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
    public class GetHierarchyClassQueryHandler : IQueryHandler<GetHierarchyClassQuery, HierarchyClass>
    {
        private readonly IconContext context;

        public GetHierarchyClassQueryHandler(IconContext context)
        {
            this.context = context;
        }

        public HierarchyClass Handle(GetHierarchyClassQuery parameters)
        {
            if (parameters.HierarchyClassId < 1)
            {
                throw new ArgumentOutOfRangeException("The value of hierarchyClassId must be greater than 0.");
            }

            HierarchyClass hierarchyClass = this.context.HierarchyClass
                .Include(hc => hc.HierarchyClassTrait)
                .Include(hc => hc.HierarchyClassTrait.Select(hct => hct.Trait))
                .Include(hc => hc.Hierarchy)
                .First(hc => hc.hierarchyClassID == parameters.HierarchyClassId);

            return hierarchyClass;
        }
    }
}
