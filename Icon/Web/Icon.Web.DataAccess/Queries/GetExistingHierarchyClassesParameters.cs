using Icon.Common.DataAccess;
using Icon.Framework;
using System.Collections.Generic;

namespace Icon.Web.DataAccess.Queries
{
    public class GetExistingHierarchyClassesParameters : IQuery<List<HierarchyClass>>
    {
        public List<string> HierarchyClassNameList { get; set; }
    }
}
