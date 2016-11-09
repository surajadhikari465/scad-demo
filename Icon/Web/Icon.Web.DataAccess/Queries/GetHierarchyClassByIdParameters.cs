using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Infrastructure;

namespace Icon.Web.DataAccess.Queries
{
    public class GetHierarchyClassByIdParameters : IQuery<HierarchyClass>
    {
        public int HierarchyClassId { get; set; }
    }
}
