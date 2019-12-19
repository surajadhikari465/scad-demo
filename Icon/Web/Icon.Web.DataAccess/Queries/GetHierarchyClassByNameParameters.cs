using Icon.Common.DataAccess;
using Icon.Framework;

namespace Icon.Web.DataAccess.Queries
{
    public class GetHierarchyClassByNameParameters : IQuery<HierarchyClass>
    {
        public string HierarchyClassName { get; set; }
        public string HierarchyName { get; set; }
        public int HierarchyLevel { get; set; }
    }
}
