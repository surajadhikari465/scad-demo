using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Infrastructure;
using System.Data.Entity;
using System.Linq;

namespace Icon.Web.DataAccess.Queries
{
    public class GetHierarchyClassByIdQuery : IQueryHandler<GetHierarchyClassByIdParameters, HierarchyClass>
    {
        private IconContext context;

        public GetHierarchyClassByIdQuery(IconContext context)
        {
            this.context = context;
        }
        public HierarchyClass Search(GetHierarchyClassByIdParameters parameters)
        {
            HierarchyClass hierarchyClass = context
                .HierarchyClass
                .Include(hc => hc.HierarchyPrototype)
                .Include(hc => hc.HierarchyClass2)
                .Include(hc => hc.HierarchyClassTrait.Select(hct => hct.Trait))
                .SingleOrDefault(hc => hc.hierarchyClassID == parameters.HierarchyClassId);

            return hierarchyClass;
        }
    }
}
