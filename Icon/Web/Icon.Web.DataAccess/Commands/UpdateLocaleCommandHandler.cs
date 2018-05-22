using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Queries;
using System;
using System.Data.Entity;
using System.Globalization;
using System.Linq;

namespace Icon.Web.DataAccess.Commands
{
    public class UpdateLocaleCommandHandler : ICommandHandler<UpdateLocaleCommand>
    {
        private IconContext context;
        private IQueryHandler<GetCurrencyForCountryParameters, Currency> currencyQuery;

        public UpdateLocaleCommandHandler(IconContext context, IQueryHandler<GetCurrencyForCountryParameters, Currency> currencyQuery)
        {
            this.context = context;
            this.currencyQuery = currencyQuery;
        }

        public void Execute(UpdateLocaleCommand data)
        {
            Locale existingLocale = context.Locale
                .Include(l => l.LocaleTrait.Select(lt => lt.Trait))
                .Include(l => l.LocaleAddress.Select(la => la.Address.PhysicalAddress.City))
                .Include(l => l.LocaleAddress.Select(la => la.Address.PhysicalAddress.City.County))
                .Include(l => l.LocaleAddress.Select(la => la.Address.PhysicalAddress.PostalCode))
                .Single(l => l.localeID == data.LocaleId);

            LocaleTrait existingBusinessUnit = context.LocaleTrait.Single(lt => lt.Trait.traitCode == TraitCodes.PsBusinessUnitId && lt.localeID == data.LocaleId);

            ThrowExceptionIfNewValuesAlreadyExist(data, existingLocale, existingBusinessUnit);

            existingLocale.localeName = data.LocaleName;
            existingLocale.localeOpenDate = data.OpenDate;
            existingLocale.localeCloseDate = data.CloseDate;
            existingBusinessUnit.traitValue = data.BusinessUnitId;

            AddOrUpdateTraitValue(TraitCodes.StoreAbbreviation, existingLocale, data.StoreAbbreviation);
            AddOrUpdateTraitValue(TraitCodes.ContactPerson, existingLocale, data.ContactPerson);
            AddOrUpdateTraitValue(TraitCodes.PhoneNumber, existingLocale, data.PhoneNumber);
            AddOrUpdateTraitValue(TraitCodes.ModifiedDate, existingLocale, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffff", CultureInfo.InvariantCulture));
            AddOrUpdateTraitValue(TraitCodes.Fax, existingLocale, data.Fax);
            AddOrUpdateTraitValue(TraitCodes.IrmaStoreId, existingLocale, data.IrmaStoreId);
            AddOrUpdateTraitValue(TraitCodes.StorePosType, existingLocale, data.StorePosType);
            AddOrUpdateTraitValue(TraitCodes.ModifiedUser, existingLocale, data.UserName);
            // set the currency code trait based on the country set for the Locale
            var currencyCode = GetCurrencyCodeForCountry(data.CountryId);
            if (!String.IsNullOrWhiteSpace(currencyCode))
            {
                AddOrUpdateTraitValue(TraitCodes.Currency, existingLocale, currencyCode);
            }

            UpdateAddress(existingLocale, data);

            UpdateEwicAgencyId(existingLocale, data);

            context.SaveChanges();
        }

        private void ThrowExceptionIfNewValuesAlreadyExist(UpdateLocaleCommand data, Locale existingLocale, LocaleTrait existingBusinessUnit)
        {
            if (context.Locale
                .Where(l => l.localeID != existingLocale.localeID)
                .Any(l => l.localeName.ToLower() == data.LocaleName.ToLower()))
            {
                throw new ArgumentException(String.Format("{0}: This store name is already in use.", data.LocaleName));
            }

            if (context.LocaleTrait
                .Where(lt => lt.localeID != existingLocale.localeID)
                .Any(lt => lt.Trait.traitCode == TraitCodes.StoreAbbreviation && lt.traitValue.ToLower() == data.StoreAbbreviation.ToLower()))
            {
                throw new ArgumentException(String.Format("{0}: This store abbreviation is already in use.", data.StoreAbbreviation));
            }

            if (context.LocaleTrait
                .Where(lt => lt.localeID != existingBusinessUnit.localeID)
                .Any(lt => lt.Trait.traitCode == TraitCodes.PsBusinessUnitId && lt.traitValue == data.BusinessUnitId))
            {
                throw new ArgumentException(String.Format("{0}: This Business Unit ID is already in use.", data.BusinessUnitId));
            }
        }

        private void AddOrUpdateTraitValue(string traitCode, Locale existingLocale, string traitValue)
        {
            LocaleTrait trait = existingLocale.LocaleTrait.SingleOrDefault(lt => lt.Trait.traitCode == traitCode);

            if (trait == null)
            {
                existingLocale.LocaleTrait.Add(new LocaleTrait
                {
                    localeID = existingLocale.localeID,
                    traitID = context.Trait.First(t => t.traitCode == traitCode).traitID,
                    traitValue = traitValue,
                    Trait = context.Trait.First(t => t.traitCode == traitCode)
                });
            }
            else
            {
                trait.traitValue = traitValue;
            }
        }

        private void UpdateEwicAgencyId(Locale existingLocale, UpdateLocaleCommand data)
        {
            var existingAgencyId = existingLocale.Agency.SingleOrDefault();

            if (String.IsNullOrEmpty(data.EwicAgencyId))
            {
                if (existingAgencyId != null)
                {
                    context.Agency.Single(a => a.AgencyId == existingAgencyId.AgencyId).Locale.Remove(existingLocale);
                }
            }
            else
            {
                if (existingAgencyId != null)
                {
                    existingAgencyId.AgencyId = data.EwicAgencyId;
                }
                else
                {
                    context.Agency.Single(a => a.AgencyId == data.EwicAgencyId).Locale.Add(existingLocale);
                }
            }
        }

        private void UpdateAddress(Locale existingLocale, UpdateLocaleCommand command)
        {
            bool countyAdded = false;

            County county = context.County.SingleOrDefault(c => c.countyName.ToLower() == command.CountyName.ToLower() && c.territoryID == command.TerritoryId);
            if (county == null)
            {
                county = new County
                {
                    countyName = command.CountyName,
                    territoryID = command.TerritoryId
                };

                context.County.Add(county);
                countyAdded = true;
            }

            City city = context.City.SingleOrDefault(c => c.cityName.ToLower() == command.CityName.ToLower() && c.territoryID == command.TerritoryId && c.County.countyName == command.CountyName && c.County.territoryID == command.TerritoryId);
            if (city == null || countyAdded)
            {
                city = new City
                {
                    cityName = command.CityName,
                    countyID = county.countyID,
                    territoryID = command.TerritoryId,
                    County = county,
                    Territory = context.Territory.First(t => t.territoryID == command.TerritoryId)
                };

                context.City.Add(city);
            }

            PostalCode postalCode = context.PostalCode.SingleOrDefault(pc => pc.postalCode.ToLower() == command.PostalCode.ToLower());
            if (postalCode == null)
            {
                postalCode = new PostalCode
                {
                    postalCode = command.PostalCode,
                    countryID = command.CountryId,
                    County = county,
                    countyID = county.countyID
                };

                context.PostalCode.Add(postalCode);
            }
            else if (countyAdded)
            {
                postalCode.countyID = county.countyID;
                postalCode.County = county;
            }

            PhysicalAddress address = existingLocale.LocaleAddress
                .Single(la => la.addressID == command.AddressId)
                .Address
                .PhysicalAddress;

            address.addressLine1 = command.AddressLine1;
            address.addressLine2 = String.IsNullOrEmpty(command.AddressLine2) ? null : command.AddressLine2;
            address.addressLine3 = String.IsNullOrEmpty(command.AddressLine2) ? null : command.AddressLine3;
            address.latitude = command.Latitude;
            address.longitude = command.Longitude;
            address.territoryID = command.TerritoryId;
            address.countryID = command.CountryId;
            address.timezoneID = command.TimezoneId;
            address.cityID = city.cityID;
            address.postalCodeID = postalCode.postalCodeID;
        }

        private string GetCurrencyCodeForCountry(int countryId)
        {
            var currencyForCountry = currencyQuery.Search(new GetCurrencyForCountryParameters { CountryId = countryId });
            if (currencyForCountry == default(Currency)) return "USD";
            return currencyForCountry.currencyTypeCode;
        }
    }
}
