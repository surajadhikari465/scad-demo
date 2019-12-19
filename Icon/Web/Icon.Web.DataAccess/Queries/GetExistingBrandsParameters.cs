using Icon.Common.DataAccess;
using System.Collections.Generic;

namespace Icon.Web.DataAccess.Queries
{
    public class GetExistingBrandsParameters : IQuery<List<string>>
    {
        public List<string> BrandNames { get; set; }
    }
}
