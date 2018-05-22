using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Infrastructure;

namespace Icon.Web.DataAccess.Queries
{
    public class GetCurrencyForCountryParameters : IQuery<Currency> 
    {
        public int CountryId { get; set; }
    }
}
