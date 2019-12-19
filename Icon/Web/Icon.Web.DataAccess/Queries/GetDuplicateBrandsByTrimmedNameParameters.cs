using Icon.Common.DataAccess;
using System.Collections.Generic;

namespace Icon.Web.DataAccess.Queries
{
    public class GetDuplicateBrandsByTrimmedNameParameters : IQuery<List<string>>
    {
        public Dictionary<string, string> LongBrandNameList { get; set; }
    }
}
