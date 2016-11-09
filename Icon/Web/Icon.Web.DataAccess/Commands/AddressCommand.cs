
namespace Icon.Web.DataAccess.Commands
{
    public class AddressCommand
    {
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string City { get; set; }
        public int TerritoryId { get; set; }
        public string PostalCode { get; set; }
        public string County { get; set; }
        public int CountryId { get; set; }
        public int TimeZoneId { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }

        // 'output' properties
        public int AddressId { get; set; }
    }
}
