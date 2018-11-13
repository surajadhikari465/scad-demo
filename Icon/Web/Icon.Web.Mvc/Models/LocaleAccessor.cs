using Icon.Framework;
using System;
using System.Linq;

namespace Icon.Web.Mvc.Models
{
    public class LocaleAccessor
    {
        public static string GetRegionAbbreviation(Locale locale)
        {
            var regionTrait = locale.LocaleTrait.SingleOrDefault(lt => lt.traitID == Traits.RegionAbbreviation);
            return regionTrait != null ? regionTrait.traitValue : String.Empty;
        }

        public static string GetBusinessUnit(Locale locale)
        {
            var businessUnitTrait = locale.LocaleTrait.SingleOrDefault(lt => lt.traitID == Traits.PsBusinessUnitId);
            return businessUnitTrait != null ? businessUnitTrait.traitValue : String.Empty;
        }

        public static int GetAddressId(Locale locale)
        {
            LocaleAddress localeAddress = locale.LocaleAddress.FirstOrDefault();
            return localeAddress == null ? default(int) : localeAddress.addressID;
        }

        public static string GetAddressLine1(Locale locale)
        {
            LocaleAddress localeAddress = locale.LocaleAddress.FirstOrDefault();
            return localeAddress == null ? String.Empty : localeAddress.Address.PhysicalAddress.addressLine1;
        }

        public static string GetAddressLine2(Locale locale)
        {
            LocaleAddress localeAddress = locale.LocaleAddress.FirstOrDefault();
            return localeAddress == null ? String.Empty : localeAddress.Address.PhysicalAddress.addressLine2;
        }

        public static string GetAddressLine3(Locale locale)
        {
            LocaleAddress localeAddress = locale.LocaleAddress.FirstOrDefault();
            return localeAddress == null ? String.Empty : localeAddress.Address.PhysicalAddress.addressLine3;
        }

        public static string GetCity(Locale locale)
        {
            LocaleAddress localeAddress = locale.LocaleAddress.FirstOrDefault();
            return localeAddress == null ? String.Empty : localeAddress.Address.PhysicalAddress.City.cityName;
        }

        public static string GetPostalCode(Locale locale)
        {
            LocaleAddress localeAddress = locale.LocaleAddress.FirstOrDefault();
            return localeAddress == null ? String.Empty : localeAddress.Address.PhysicalAddress.PostalCode.postalCode;
        }

        public static string GetCounty(Locale locale)
        {
            LocaleAddress localeAddress = locale.LocaleAddress.FirstOrDefault();
            return localeAddress == null ? String.Empty : localeAddress.Address.PhysicalAddress.City.County.countyName;
        }

        public static int? GetCountryId(Locale locale)
        {
            LocaleAddress localeAddress = locale.LocaleAddress.FirstOrDefault();
            return localeAddress == null ? default(int) : localeAddress.Address.PhysicalAddress.countryID;
        }

        public static int? GetTerritoryId(Locale locale)
        {
            LocaleAddress localeAddress = locale.LocaleAddress.FirstOrDefault();
            return localeAddress == null ? default(int) : localeAddress.Address.PhysicalAddress.territoryID;
        }

        public static int? GetTimeZoneId(Locale locale)
        {
            LocaleAddress localeAddress = locale.LocaleAddress.FirstOrDefault();
            return localeAddress == null ? default(int) : localeAddress.Address.PhysicalAddress.timezoneID;
        }

        public static decimal? GetLatitude(Locale locale)
        {
            LocaleAddress localeAddress = locale.LocaleAddress.FirstOrDefault();
            return localeAddress == null ? default(int) : localeAddress.Address.PhysicalAddress.latitude;
        }

        public static decimal? GetLongitude(Locale locale)
        {
            LocaleAddress localeAddress = locale.LocaleAddress.FirstOrDefault();
            return localeAddress == null ? default(int) : localeAddress.Address.PhysicalAddress.longitude;
        }

        public static string GetPhoneNumber(Locale locale)
        {
            LocaleTrait phoneNumberTrait = locale.LocaleTrait.FirstOrDefault(lt => lt.Trait.traitCode == TraitCodes.PhoneNumber);
            return phoneNumberTrait == null ? String.Empty : phoneNumberTrait.traitValue;
        }

        public static string GetContactPerson(Locale locale)
        {
            LocaleTrait contactPersonTrait = locale.LocaleTrait.FirstOrDefault(lt => lt.Trait.traitCode == TraitCodes.ContactPerson);
            return contactPersonTrait == null ? String.Empty : contactPersonTrait.traitValue;
        }

        public static string GetLocaleSubType(Locale locale)
        {
            LocaleTrait localeSubtypeTrait = locale.LocaleTrait.FirstOrDefault(lt => lt.Trait.traitCode == TraitCodes.LocaleSubtype);
            return localeSubtypeTrait == null ? String.Empty : localeSubtypeTrait.traitValue;
        }

        public static string GetVenueCode(Locale locale)
        {
            LocaleTrait venueCodeTrait = locale.LocaleTrait.FirstOrDefault(lt => lt.Trait.traitCode == TraitCodes.VenueCode);
            return venueCodeTrait == null ? String.Empty : venueCodeTrait.traitValue;
        }

        public static string GetVenueOccupant(Locale locale)
        {
            LocaleTrait venueOccupantTrait = locale.LocaleTrait.FirstOrDefault(lt => lt.Trait.traitCode == TraitCodes.VenueOccupant);
            return venueOccupantTrait == null ? String.Empty : venueOccupantTrait.traitValue;
          
        }

        public static string GetTerritoryCode(Locale locale)
        {
            LocaleAddress localeAddress = locale.LocaleAddress.FirstOrDefault();
            return localeAddress == null ? String.Empty : localeAddress.Address.PhysicalAddress.Territory.territoryCode;
        }

        public static string GetCountryCode(Locale locale)
        {
            LocaleAddress localeAddress = locale.LocaleAddress.FirstOrDefault();
            return localeAddress == null ? String.Empty : localeAddress.Address.PhysicalAddress.Country.countryCode;
        }

        public static string GetTimeZoneCode(Locale locale)
        {
            LocaleAddress localeAddress = locale.LocaleAddress.FirstOrDefault();
            return localeAddress == null ? String.Empty : localeAddress.Address.PhysicalAddress.Timezone.timezoneCode;
        }

        public static string GetStoreAbbreviation(Locale locale)
        {
            LocaleTrait storeAbbreviationTrait = locale.LocaleTrait.SingleOrDefault(lt => lt.traitID == Traits.StoreAbbreviation);
            return storeAbbreviationTrait == null ? String.Empty : storeAbbreviationTrait.traitValue;
        }

        public static string GetEwicAgencyId(Locale locale)
        {
            Agency agency = locale.Agency.SingleOrDefault();
            return agency == null ? String.Empty : agency.AgencyId;
        }

        public static string GetIrmaStoreId(Locale locale)
        {
            LocaleTrait irmaId = locale.LocaleTrait.SingleOrDefault(lt => lt.traitID == Traits.IrmaStoreId);
            return irmaId == null ? String.Empty : irmaId.traitValue;
        }

        public static string GetIrmaId(Locale locale)
        {
            LocaleTrait irmaId = locale.LocaleTrait.SingleOrDefault(lt => lt.traitID == Traits.IrmaStoreId);
            return irmaId == null ? String.Empty : irmaId.traitValue;
        }

        public static string GetStorePosType(Locale locale)
        {
            LocaleTrait trait = locale.LocaleTrait.SingleOrDefault(lt => lt.traitID == Traits.StorePosType);
            return trait == null ? String.Empty : trait.traitValue;
        }

        public static string GetFax(Locale locale)
        {
            LocaleTrait trait = locale.LocaleTrait.SingleOrDefault(lt => lt.traitID == Traits.Fax);
            return trait == null ? String.Empty : trait.traitValue;
        }

        public static string GetCurrencyCode(Locale locale)
        {
            LocaleTrait trait = locale.LocaleTrait.SingleOrDefault(lt => lt.traitID == Traits.CurrencyCode);
            return trait == null ? String.Empty : trait.traitValue;
        }
    }
}