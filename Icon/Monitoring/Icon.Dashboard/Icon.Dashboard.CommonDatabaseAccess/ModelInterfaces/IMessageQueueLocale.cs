using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Dashboard.CommonDatabaseAccess
{
    public interface IMessageQueueLocale
    {
        int MessageQueueId { get; set; }
        int MessageTypeId { get; set; }
        int MessageStatusId { get; set; }
        int? MessageHistoryId { get; set; }
        DateTime InsertDate { get; set; }
        int LocaleId { get; set; }
        int OwnerOrgPartyId { get; set; }
        string StoreAbbreviation { get; set; }
        string LocaleName { get; set; }
        DateTime? LocaleOpenDate { get; set; }
        DateTime? LocaleCloseDate { get; set; }
        int LocaleTypeId { get; set; }
        int? ParentLocaleId { get; set; }
        string BusinessUnitId { get; set; }
        int? AddressId { get; set; }
        string AddressUsageCode { get; set; }
        string CountryName { get; set; }
        string CountryCode { get; set; }
        string TerritoryName { get; set; }
        string TerritoryCode { get; set; }
        string CityName { get; set; }
        string PostalCode { get; set; }
        string Latitude { get; set; }
        string Longitude { get; set; }
        string AddressLine1 { get; set; }
        string AddressLine2 { get; set; }
        string AddressLine3 { get; set; }
        string TimezoneCode { get; set; }
        string TimezoneName { get; set; }
        string PhoneNumber { get; set; }
        int? InProcessBy { get; set; }
        DateTime? ProcessedDate { get; set; }
    }
}
