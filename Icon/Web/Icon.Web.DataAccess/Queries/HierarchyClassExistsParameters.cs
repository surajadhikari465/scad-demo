using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Infrastructure;

namespace Icon.Web.DataAccess.Queries
{
    public class HierarchyClassExistsParameters : IQuery<bool>
    {
        public string HierarchyClassName { get; set; }
        public string HierarchyName { get; set; }
    }
}
