using Icon.Common.DataAccess;
using System.Collections.Generic;

namespace Icon.Web.DataAccess.Queries
{
    public class GetBrandAbbreviationsThatExistParameters : IQuery<List<string>>
    {
        public List<string> BrandAbbreviations { get; set; }
    }
}
