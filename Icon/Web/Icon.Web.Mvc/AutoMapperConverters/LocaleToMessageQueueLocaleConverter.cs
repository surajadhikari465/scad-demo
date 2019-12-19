using AutoMapper;
using Icon.Framework;
using System.Linq;

namespace Icon.Web.Mvc.AutoMapperConverters
{
    public class LocaleToMessageQueueLocaleConverter : ITypeConverter<Locale, MessageQueueLocale>
    {
        public MessageQueueLocale Convert(Locale source, MessageQueueLocale destination, ResolutionContext context)
        {
            var result = new MessageQueueLocale
            {
                LocaleId = source.localeID,
                LocaleName = source.localeName,
                LocaleOpenDate = source.localeOpenDate,
                LocaleCloseDate = source.localeCloseDate,
                LocaleTypeId = source.localeTypeID,
                OwnerOrgPartyId = source.ownerOrgPartyID,
                ParentLocaleId = source.parentLocaleID
            };

            // store related traits
            result.BusinessUnitId = source.LocaleTrait.SingleOrDefault(lt => lt.traitID == Traits.PsBusinessUnitId)?.traitValue;
            result.StoreAbbreviation = source.LocaleTrait.SingleOrDefault(lt => lt.traitID == Traits.StoreAbbreviation)?.traitValue;
            result.PhoneNumber = source.LocaleTrait.SingleOrDefault(lt => lt.traitID == Traits.PhoneNumber)?.traitValue;
            result.SodiumWarningRequired = source.LocaleTrait.SingleOrDefault(lt => lt.traitID == Traits.SodiumWarningRequired)?.traitValue == "1";

			LocaleTrait sodiumWarning = source.LocaleTrait.SingleOrDefault(lt => lt.traitID == Traits.SodiumWarningRequired);
            destination.SodiumWarningRequired = sodiumWarning == null ? (bool?)null : sodiumWarning.traitValue == "1";

			LocaleAddress localeAddress = source.LocaleAddress.FirstOrDefault();
            if (localeAddress != null)
            {
                result.AddressId = localeAddress.addressID;
                result.AddressUsageCode = localeAddress.AddressUsage?.addressUsageCode;

                Address address = localeAddress.Address;
                if (address != null)
                {
                    PhysicalAddress physicalAddress = address.PhysicalAddress;
                    if(physicalAddress != null)
                    {
                        result.AddressLine1 = physicalAddress.addressLine1;
                        result.AddressLine2 = physicalAddress.addressLine2;
                        result.AddressLine3 = physicalAddress.addressLine3;
                        result.CityName = physicalAddress.City?.cityName;
                        result.CountryCode = physicalAddress.Country?.countryCode;
                        result.CountryName = physicalAddress.Country?.countryName;
                        result.Latitude = physicalAddress.latitude?.ToString();
                        result.Longitude = physicalAddress.longitude?.ToString();
                        result.PostalCode = physicalAddress.PostalCode?.postalCode;
                        result.TerritoryCode = physicalAddress.Territory?.territoryCode;
                        result.TerritoryName = physicalAddress.Territory?.territoryName;
                        result.TimezoneCode = physicalAddress.Timezone?.timezoneCode;
                        result.TimezoneName = physicalAddress.Timezone?.posTimeZoneName;
                    }
                }
            }
			//end of Store related traits

			//Venue Related Traits
            result.VenueCode = source.LocaleTrait.SingleOrDefault(lt => lt.traitID == Traits.VenueCode)?.traitValue;
            result.VenueOccupant = source.LocaleTrait.SingleOrDefault(lt => lt.traitID == Traits.VenueOccupant)?.traitValue;
			result.VenueSubType = source.LocaleTrait.SingleOrDefault(lt => lt.traitID == Traits.LocaleSubtype)?.traitValue;
            result.TouchPointGroupId = source.LocaleTrait.SingleOrDefault(lt => lt.traitID == Traits.TouchpointGroupId)?.traitValue;
			// end of Venue related Traits

			return result;
        }
    }
}