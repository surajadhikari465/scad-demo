using Icon.Common.DataAccess;
using Icon.Framework;

namespace Icon.Web.DataAccess.Queries
{
    public class GetCurrencyForCountryParameters : IQuery<Currency> 
    {
        public int CountryId { get; set; }
    }
}
