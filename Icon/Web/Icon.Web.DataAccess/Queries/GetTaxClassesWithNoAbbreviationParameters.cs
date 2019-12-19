using Icon.Common.DataAccess;
using System.Collections.Generic;

namespace Icon.Web.DataAccess.Queries
{
    public class GetTaxClassesWithNoAbbreviationParameters : IQuery<List<string>>
    {
        public List<string> TaxClasses { get; set; }
    }
}
