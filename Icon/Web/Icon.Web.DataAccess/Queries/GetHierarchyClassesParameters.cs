using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Models;
using System.Collections.Generic;

namespace Icon.Web.DataAccess.Queries
{
    public class GetHierarchyClassesParameters : IQuery<IEnumerable<HierarchyClassModel>>
    {
        public int? HierarchyId { get; set; }
        public int? HierarchyClassId { get; set; }
        public string HierarchyLineageFilter { get; set; }
    }
}
