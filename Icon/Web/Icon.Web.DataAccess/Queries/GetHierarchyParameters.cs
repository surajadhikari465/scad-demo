using Icon.Common.DataAccess;
using Icon.Framework;
using System.Collections.Generic;

namespace Icon.Web.DataAccess.Queries
{
    public class GetHierarchyParameters : IQuery<List<Hierarchy>>
    {
        public int HierarchyId { get; set; }
        public string HierarchyName { get; set; }
        public bool IncludeNavigation { get; set; }
    }
}
