using Icon.Common.DataAccess;

namespace Icon.Web.DataAccess.Queries
{
    public class HierarchyClassExistsParameters : IQuery<bool>
    {
        public string HierarchyClassName { get; set; }
        public string HierarchyName { get; set; }
    }
}
