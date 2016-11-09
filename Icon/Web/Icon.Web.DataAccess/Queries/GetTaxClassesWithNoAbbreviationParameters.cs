using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Infrastructure;
using System.Collections.Generic;

namespace Icon.Web.DataAccess.Queries
{
    public class GetTaxClassesWithNoAbbreviationParameters : IQuery<List<string>>
    {
        public List<string> TaxClasses { get; set; }
    }
}
