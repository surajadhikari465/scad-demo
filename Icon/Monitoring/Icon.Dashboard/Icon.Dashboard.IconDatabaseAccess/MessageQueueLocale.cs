//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Icon.Dashboard.IconDatabaseAccess
{
    using System;
    using System.Collections.Generic;
    using Icon.Dashboard.CommonDatabaseAccess;
    
    public partial class MessageQueueLocale : IMessageQueueLocale
    {
        public int MessageQueueId { get; set; }
        public int MessageTypeId { get; set; }
        public int MessageStatusId { get; set; }
        public Nullable<int> MessageHistoryId { get; set; }
        public System.DateTime InsertDate { get; set; }
        public int LocaleId { get; set; }
        public int OwnerOrgPartyId { get; set; }
        public string StoreAbbreviation { get; set; }
        public string LocaleName { get; set; }
        public Nullable<System.DateTime> LocaleOpenDate { get; set; }
        public Nullable<System.DateTime> LocaleCloseDate { get; set; }
        public int LocaleTypeId { get; set; }
        public Nullable<int> ParentLocaleId { get; set; }
        public string BusinessUnitId { get; set; }
        public Nullable<int> AddressId { get; set; }
        public string AddressUsageCode { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public string TerritoryName { get; set; }
        public string TerritoryCode { get; set; }
        public string CityName { get; set; }
        public string PostalCode { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string TimezoneCode { get; set; }
        public string TimezoneName { get; set; }
        public string PhoneNumber { get; set; }
        public Nullable<int> InProcessBy { get; set; }
        public Nullable<System.DateTime> ProcessedDate { get; set; }
    
        public virtual MessageType MessageType { get; set; }
    }
}
