using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Queries;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Icon.Web.DataAccess.Commands
{
    public class AddLocaleCommandHandler : ICommandHandler<AddLocaleCommand>
    {
        private IconContext context;
        private IQueryHandler<GetCurrencyForCountryParameters, Currency> currencyQuery;

        public AddLocaleCommandHandler(IconContext context, IQueryHandler<GetCurrencyForCountryParameters, Currency> currencyQuery)
        {
            this.context = context;
            this.currencyQuery = currencyQuery;
        }

        public void Execute(AddLocaleCommand data)
        {
            // Verify store name is unique.
            if (context.Locale.Any(l => l.localeName.ToLower() == data.LocaleName.ToLower()))
            {
                throw new ArgumentException(String.Format("{0}: This store name is already in use.", data.LocaleName));
            }

            // Verify store abbreviation is unique.
            if (context.LocaleTrait.Any(lt => lt.Trait.traitCode == TraitCodes.StoreAbbreviation && lt.traitValue == data.StoreAbbreviation))
            {
                throw new ArgumentException(String.Format("{0}: This store abbreviation is already in use.", data.StoreAbbreviation));
            }

            // Verify that Business Unit is unique.
            var duplicateBusinessUnit = context.LocaleTrait
                .Where(lt => lt.Trait.traitCode == TraitCodes.PsBusinessUnitId && lt.traitValue == data.BusinessUnit);
            if (duplicateBusinessUnit.Any())
            {
                throw new ArgumentException(String.Format("{0}: This Business Unit ID is already in use.", data.BusinessUnit));
            }

            // look up the currency code trait based on the country of the Locale
            var currencyCode = GetCurrencyCodeForCountry(data.CountryId);

            var locale = new Locale
            {
                localeName = data.LocaleName,
                localeTypeID = data.LocaleTypeId,
                parentLocaleID = data.LocaleParentId,
                localeOpenDate = data.OpenDate,
                ownerOrgPartyID = data.OwnerOrgPartyId,
                LocaleTrait = new List<LocaleTrait>
                {
                    new LocaleTrait { traitID = Traits.PsBusinessUnitId, traitValue = data.BusinessUnit },
                    new LocaleTrait { traitID = Traits.PhoneNumber, traitValue = data.PhoneNumber },
                    new LocaleTrait { traitID = Traits.ContactPerson, traitValue = data.ContactPerson },
                    new LocaleTrait { traitID = Traits.StoreAbbreviation, traitValue = data.StoreAbbreviation },
                    new LocaleTrait { traitID = Traits.IrmaStoreId, traitValue = data.IrmaStoreId },
                    new LocaleTrait { traitID = Traits.StorePosType, traitValue = data.StorePosType },
                    new LocaleTrait { traitID = Traits.Fax, traitValue = data.Fax },
                    new LocaleTrait { traitID = Traits.InsertDate, traitValue = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffff", CultureInfo.InvariantCulture) },
                    new LocaleTrait { traitID = Traits.ModifiedUser, traitValue = data.UserName },
                    new LocaleTrait { traitID = Traits.Currency, traitValue = currencyCode }
                }
            };

            context.Locale.Add(locale);
            context.SaveChanges();

            // Add the eWIC Agency association, if present.
            if (!String.IsNullOrEmpty(data.EwicAgencyId))
            {
                var agency = context.Agency.Single(a => a.AgencyId == data.EwicAgencyId);
                agency.Locale.Add(locale);
                context.SaveChanges();
            }

            // Set 'output' parameter for Manager to consume.
            data.LocaleId = locale.localeID;
        }
        private string GetCurrencyCodeForCountry(int countryId)
        {
            var currencyForCountry = currencyQuery.Search(new GetCurrencyForCountryParameters { CountryId = countryId });
            if (currencyForCountry == default(Currency)) return "USD";
            return currencyForCountry.currencyTypeCode;
        }
    }
}
