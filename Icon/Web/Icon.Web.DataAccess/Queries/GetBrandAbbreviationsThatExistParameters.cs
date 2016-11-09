using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Web.DataAccess.Queries
{
    public class GetBrandAbbreviationsThatExistParameters : IQuery<List<string>>
    {
        public List<string> BrandAbbreviations { get; set; }
    }
}
