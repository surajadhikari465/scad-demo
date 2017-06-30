using GlobalEventController.DataAccess.Infrastructure;
using Icon.DbContextFactory;
using Icon.Framework;
using System;
using System.Data.Entity;
using System.Linq;

namespace GlobalEventController.DataAccess.Queries
{
    public class GetHierarchyClassQueryHandler : IQueryHandler<GetHierarchyClassQuery, HierarchyClass>
    {
        private IDbContextFactory<IconContext> contextFactory;

        public GetHierarchyClassQueryHandler(IDbContextFactory<IconContext> contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public HierarchyClass Handle(GetHierarchyClassQuery parameters)
        {
            if (parameters.HierarchyClassId < 1)
            {
                throw new ArgumentOutOfRangeException("The value of hierarchyClassId must be greater than 0.");
            }

            using (var context = contextFactory.CreateContext())
            {
                HierarchyClass hierarchyClass = context.HierarchyClass
                    .Include(hc => hc.HierarchyClassTrait)
                    .Include(hc => hc.HierarchyClassTrait.Select(hct => hct.Trait))
                    .Include(hc => hc.Hierarchy)
                    .First(hc => hc.hierarchyClassID == parameters.HierarchyClassId);

                return hierarchyClass;
            }
        }
    }
}
