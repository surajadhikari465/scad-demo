using AutoMapper;
using Icon.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Icon.Web.Mvc.AutoMapperConverters
{
    public class LocaleToMessageQueueLocaleConverter : TypeConverter<Locale, MessageQueueLocale>
    {
        protected override MessageQueueLocale ConvertCore(Locale source)
        {
            MessageQueueLocale destination = new MessageQueueLocale();

            destination.LocaleId = source.localeID;
            destination.LocaleName = source.localeName;
            destination.LocaleOpenDate = source.localeOpenDate;
            destination.LocaleCloseDate = source.localeCloseDate;
            destination.LocaleTypeId = source.localeTypeID;
            destination.OwnerOrgPartyId = source.ownerOrgPartyID;
            destination.ParentLocaleId = source.parentLocaleID;

			// store related traits
            LocaleTrait businessUnitIdTrait = source.LocaleTrait.SingleOrDefault(lt => lt.traitID == Traits.PsBusinessUnitId);
            destination.BusinessUnitId = businessUnitIdTrait == null ? null : businessUnitIdTrait.traitValue;
            
            LocaleTrait storeAbbreviationTrait = source.LocaleTrait.SingleOrDefault(lt => lt.traitID == Traits.StoreAbbreviation);
            destination.StoreAbbreviation = storeAbbreviationTrait == null ? null : storeAbbreviationTrait.traitValue;

            LocaleTrait phoneNumberTrait = source.LocaleTrait.SingleOrDefault(lt => lt.traitID == Traits.PhoneNumber);
            destination.PhoneNumber = phoneNumberTrait == null ? null : phoneNumberTrait.traitValue;

			LocaleTrait sodiumWarning = source.LocaleTrait.SingleOrDefault(lt => lt.traitID == Traits.SodiumWarningRequired);
            destination.SodiumWarningRequired = sodiumWarning == null ? (bool?)null : sodiumWarning.traitValue == "1";

			LocaleAddress localeAddress = source.LocaleAddress.FirstOrDefault();
            if (localeAddress != null)
            {
                destination.AddressId = localeAddress.addressID;
                destination.AddressUsageCode = localeAddress.AddressUsage == null ? null : localeAddress.AddressUsage.addressUsageCode;

                Address address = localeAddress.Address;
                if (address != null)
                {
                    PhysicalAddress physicalAddress = address.PhysicalAddress;
                    if(physicalAddress != null)
                    {
                        destination.AddressLine1 = physicalAddress.addressLine1;
                        destination.AddressLine2 = physicalAddress.addressLine2;
                        destination.AddressLine3 = physicalAddress.addressLine3;
                        destination.CityName = physicalAddress.City == null ? null : physicalAddress.City.cityName;
                        destination.CountryCode = physicalAddress.Country == null ? null : physicalAddress.Country.countryCode;
                        destination.CountryName = physicalAddress.Country == null ? null : physicalAddress.Country.countryName;
                        destination.Latitude = physicalAddress.latitude.HasValue ? physicalAddress.latitude.ToString() : null;
                        destination.Longitude = physicalAddress.longitude.HasValue ? physicalAddress.longitude.ToString() : null;
                        destination.PostalCode = physicalAddress.PostalCode == null ? null : physicalAddress.PostalCode.postalCode;
                        destination.TerritoryCode = physicalAddress.Territory == null ? null : physicalAddress.Territory.territoryCode;
                        destination.TerritoryName = physicalAddress.Territory == null ? null : physicalAddress.Territory.territoryName;
                        destination.TimezoneCode = physicalAddress.Timezone == null ? null : physicalAddress.Timezone.timezoneCode;
                        destination.TimezoneName = physicalAddress.Timezone == null ? null : physicalAddress.Timezone.posTimeZoneName;
                    }
                }
            }
			//end of Store related traits

			//Venue Related Traits
			LocaleTrait venueCode = source.LocaleTrait.SingleOrDefault(lt => lt.traitID == Traits.VenueCode);
			destination.VenueCode = venueCode == null ? null : venueCode.traitValue;

			LocaleTrait venueOccupant = source.LocaleTrait.SingleOrDefault(lt => lt.traitID == Traits.VenueOccupant);
			destination.VenueOccupant = venueOccupant == null ? null : venueOccupant.traitValue;

			LocaleTrait venueSubType = source.LocaleTrait.SingleOrDefault(lt => lt.traitID == Traits.LocaleSubtype);
			destination.VenueSubType = venueSubType == null ? null : venueSubType.traitValue;

            LocaleTrait TouchGroupGroupId = source.LocaleTrait.SingleOrDefault(lt => lt.traitID == Traits.TouchpointGroupId);
            destination.TouchPointGroupId = TouchGroupGroupId == null ? null : TouchGroupGroupId.traitValue;
			// end of Venue related Traits


			return destination;
        }
    }
}