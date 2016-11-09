using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Models;
using System.Collections.Generic;

namespace Icon.Web.DataAccess.Queries
{
    public class GetNationalClassByClassCodeParameters : IQuery<List<HierarchyClass>>
    {
        public string ClassCode { get; set; }
    }
}
