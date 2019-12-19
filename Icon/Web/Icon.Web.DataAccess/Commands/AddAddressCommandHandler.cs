using Icon.Common.DataAccess;
using Icon.Framework;
using System.Linq;

namespace Icon.Web.DataAccess.Commands
{
    public class AddAddressCommandHandler : ICommandHandler<AddAddressCommand>
    {
        private readonly IconContext context;

        public AddAddressCommandHandler(IconContext context)
        {
            this.context = context;
        }

        public void Execute(AddAddressCommand data)
        {
            // Setup objects for adds or updates
            City city = null;
            County county = null;
            PostalCode postalCode = null;


            // Add an entry to the Address table
            Address address = new Address { addressTypeID = AddressTypes.PhysicalAddress };
            this.context.Address.Add(address);

            // Add LocaleAddress entry
            LocaleAddress localeAddress = new LocaleAddress { addressID = address.addressID, localeID = data.LocaleId, addressUsageID = AddressUsages.Shipping };
            this.context.LocaleAddress.Add(localeAddress);

            // Add County entry if it doesn't exist yet
            county = this.context.County.FirstOrDefault(c => c.territoryID == data.TerritoryId && c.countyName == data.County);
            if (county == null)
            {
                county = new County { countyName = data.County, territoryID = data.TerritoryId };
                this.context.County.Add(county);
            }

            // Add City entry if it doesn't exist
            city = this.context.City.FirstOrDefault(c => c.cityName == data.City && c.countyID == county.countyID && c.territoryID == data.TerritoryId);
            if (city == null)
            {
                city = new City { cityName = data.City, countyID = county.countyID, territoryID = data.TerritoryId };
                this.context.City.Add(city);
            }

            // Add PostalCode if it doesn't exist
            postalCode = this.context.PostalCode.FirstOrDefault(pc => pc.countryID == data.CountryId && pc.postalCode == data.PostalCode.ToString());
            if (postalCode == null)
            {
                postalCode = new PostalCode { postalCode = data.PostalCode, countryID = data.CountryId, countyID = county.countyID };
                this.context.PostalCode.Add(postalCode);
            }

            // Add Physical Address
            PhysicalAddress physicalAddress = new PhysicalAddress();
            physicalAddress.addressID = address.addressID;
            physicalAddress.addressLine1 = data.AddressLine1;
            physicalAddress.addressLine2 = data.AddressLine2;
            physicalAddress.addressLine3 = data.AddressLine3;
            physicalAddress.cityID = city.cityID;
            physicalAddress.territoryID = data.TerritoryId;
            physicalAddress.postalCodeID = postalCode.postalCodeID;
            physicalAddress.countryID = data.CountryId;
            physicalAddress.timezoneID = data.TimeZoneId;
            physicalAddress.latitude = data.Latitude;
            physicalAddress.longitude = data.Longitude;
            
            this.context.PhysicalAddress.Add(physicalAddress);
            this.context.SaveChanges();

            data.AddressId = address.addressID;
        }
    }
}
