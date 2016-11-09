using Icon.Framework;
using System;

namespace Icon.Testing.Builders
{
    public class TestLocaleMessageBuilder
    {
        private int messageTypeId;
        private int messageStatusId;
        private int? messageHistoryId;
        private DateTime insertDate;
        private int localeId;
        private int ownerOrgPartyId;
        private string storeAbbreviation;
        private string localeName;
        private DateTime? localeOpenDate;
        private DateTime? localeCloseDate;
        private int localeTypeId;
        private int? parentLocaleId;
        private string businessUnitId;
        private int? addressId;
        private string addressUsageCode;
        private string countryName;
        private string countryCode;
        private string territoryName;
        private string territoryCode;
        private string cityName;
        private string postalCode;
        private string latitude;
        private string longitude;
        private string addressLine1;
        private string addressLine2;
        private string addressLine3;
        private string timezoneCode;
        private string timezoneName;
        private string phoneNumber;
        private int? inProcessBy;
        private DateTime? processedDate;

        public TestLocaleMessageBuilder()
        {
            this.messageTypeId = MessageTypes.ItemLocale;
            this.messageStatusId = MessageStatusTypes.Ready;
            this.messageHistoryId = null;
            this.insertDate = DateTime.Now;
            this.localeId = 0;
            this.ownerOrgPartyId = 0;
            this.storeAbbreviation = "TST";
            this.localeName = "Test Locale";
            this.localeOpenDate = null;
            this.localeCloseDate = null;
            this.localeTypeId = LocaleTypes.Store;
            this.parentLocaleId = null;
            this.businessUnitId = "99999";
            this.addressId = 1;
            this.addressUsageCode = "PHY";
            this.countryName = "United States";
            this.countryCode = "US";
            this.territoryName = "Texas";
            this.territoryCode = "TX";
            this.cityName = "Austin";
            this.postalCode = "99999";
            this.latitude = null;
            this.longitude = null;
            this.addressLine1 = "Address1";
            this.addressLine2 = "Address2";
            this.addressLine3 = "Address3";
            this.timezoneCode = "CST";
            this.timezoneName = "(UTC-06:00) Central Time (US & Canada)";
            this.phoneNumber = "999-999-9999";
            this.inProcessBy = null;
            this.processedDate = null;
        }

        public TestLocaleMessageBuilder WithLocaleTypeId(int localeTypeId)
        {
            this.localeTypeId = localeTypeId;
            return this;
        }

        public TestLocaleMessageBuilder WithParentLocaleId(int parentLocaleId)
        {
            this.parentLocaleId = parentLocaleId;
            return this;
        }

        public TestLocaleMessageBuilder WithBusinessUnitId(int businessUnitId)
        {
            this.businessUnitId = businessUnitId.ToString();
            return this;
        }

        public MessageQueueLocale Build()
        {
            return new MessageQueueLocale
            {
                MessageTypeId = this.messageTypeId,
                MessageStatusId = this.messageStatusId,
                MessageHistoryId = this.messageHistoryId,
                InsertDate = this.insertDate,
                LocaleId = this.localeId,
                OwnerOrgPartyId = this.ownerOrgPartyId,
                StoreAbbreviation = this.storeAbbreviation,
                LocaleName = this.localeName,
                LocaleOpenDate = this.localeOpenDate,
                LocaleCloseDate = this.localeCloseDate,
                LocaleTypeId = this.localeTypeId,
                ParentLocaleId = this.parentLocaleId,
                BusinessUnitId = this.businessUnitId,
                AddressId = this.addressId,
                AddressUsageCode = this.addressUsageCode,
                CountryName = this.countryName,
                CountryCode = this.countryCode,
                TerritoryName = this.territoryName,
                TerritoryCode = this.territoryCode,
                CityName = this.cityName,
                PostalCode = this.postalCode,
                Latitude = this.latitude,
                Longitude = this.longitude,
                AddressLine1 = this.addressLine1,
                AddressLine2 = this.addressLine2,
                AddressLine3 = this.addressLine3,
                TimezoneCode = this.timezoneCode,
                TimezoneName = this.timezoneName,
                PhoneNumber = this.phoneNumber,
                InProcessBy = this.inProcessBy,
                ProcessedDate = this.processedDate
            };
        }

        public static implicit operator MessageQueueLocale(TestLocaleMessageBuilder builder)
        {
            return builder.Build();
        }
    }
}
