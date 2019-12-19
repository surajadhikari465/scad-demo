using Icon.Common.DataAccess;
using Icon.Framework;
using System.Linq;

namespace Icon.Web.DataAccess.Queries
{
    public class GetCurrencyForCountryQuery : IQueryHandler<GetCurrencyForCountryParameters, Currency>
    {
        private readonly IconContext context;

        // these are the ISO-4217 country numeric codes
        public const int IsoCodeForUSA = 840;
        public const int IsoCodeForCAN = 124;
        public const int IsoCodeForGBR = 826;

        public GetCurrencyForCountryQuery(IconContext context)
        {
            this.context = context;
        }

        public Currency Search(GetCurrencyForCountryParameters parameters)
        {
            // look up the country from the country id
            var country = context.Country.FirstOrDefault(c => c.countryID == parameters.CountryId);

            if (country != default(Country))
            {
                // based on the country, look up the matching currency
                switch (country.countryCode)
                {
                    case "USA":
                        return context.Currencies.FirstOrDefault(c => c.numericCode == IsoCodeForUSA);
                    case "CAN":
                        return context.Currencies.FirstOrDefault(c => c.numericCode == IsoCodeForCAN);
                    case "GBR":
                        return context.Currencies.FirstOrDefault(c => c.numericCode == IsoCodeForGBR);
                    default:
                        break;
                }
            }
            return (Currency) null;
        }
    }
}
