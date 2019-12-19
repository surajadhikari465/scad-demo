using Icon.Common.DataAccess;
using Icon.Framework;
using System.Collections.Generic;

namespace Icon.Web.DataAccess.Queries
{
    public class GetNationalClassByClassCodeParameters : IQuery<List<HierarchyClass>>
    {
        public string ClassCode { get; set; }
    }
}
