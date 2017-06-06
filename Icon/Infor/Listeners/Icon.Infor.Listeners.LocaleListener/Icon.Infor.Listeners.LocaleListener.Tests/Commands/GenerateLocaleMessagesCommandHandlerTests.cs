using Dapper;
using Icon.Esb.Schemas.Wfm.Contracts;
using Icon.Framework;
using Icon.Infor.Listeners.LocaleListener.Commands;
using Icon.Infor.Listeners.LocaleListener.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Icon.Infor.Listeners.LocaleListener.Tests.Commands
{
    [TestClass]
    public class GenerateLocaleMessagesCommandHandlerTests
    {
        private GenerateLocaleMessagesCommandHandler commandHandler;
        private GenerateLocaleMessagesCommand command;
        private TransactionScope transaction;
        private SqlConnection connection;

        [TestInitialize]
        public void Initialize()
        {
            transaction = new TransactionScope();

            var connectionString = ConfigurationManager.ConnectionStrings["Icon"].ConnectionString;
            connection = new SqlConnection(connectionString);

            commandHandler = new GenerateLocaleMessagesCommandHandler(connectionString);
            command = new GenerateLocaleMessagesCommand();

            connection.Execute("delete app.MessageQueueLocale");
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose();
            this.connection.Close();
            this.connection.Dispose();
        }

        [TestMethod]
        public void GenerateLocaleMessages_AddOrUpdateOneStore_ShouldGenerateMessageForStore()
        {
            //Given
            CreateTestLocalesInDatabase();

            var company = CreateLocaleModel(0, ActionEnum.Inherit);
            var chain = CreateLocaleModel(9000, ActionEnum.Inherit);
            var region = CreateLocaleModel(9001, ActionEnum.Inherit);
            var metro = CreateLocaleModel(9002, ActionEnum.Inherit);
            var store = CreateLocaleModel(12345, ActionEnum.AddOrUpdate, true);
            company.Locales = new List<LocaleModel> { chain };
            chain.Locales = new List<LocaleModel> { region };
            region.Locales = new List<LocaleModel> { metro };
            metro.Locales = new List<LocaleModel> { store };

            command.Locale = company;

            //When
            commandHandler.Execute(command);

            //Then
            var messageQueueLocales = connection.Query<MessageQueueLocale>("select * from app.MessageQueueLocale").Single();
            var address = connection.Query<PhysicalAddress>(@"
                select phys.* 
                from PhysicalAddress phys 
                join LocaleAddress la on phys.addressID = la.addressID 
                where la.localeID = 9003")
                .Single();

            Assert.AreEqual(9003, messageQueueLocales.LocaleId);
            Assert.AreEqual("Test Store", messageQueueLocales.LocaleName);
            Assert.AreEqual(address.addressID, messageQueueLocales.AddressId);
        }

        private LocaleModel CreateLocaleModel(int id, ActionEnum action, bool isStore = false)
        {
            return new LocaleModel
            {
                LocaleId = isStore ? 0 : id,
                BusinessUnitId = isStore ? id : 0,
                Action = action
            };
        }

        private void CreateTestLocalesInDatabase()
        {
            var sql = @"
            declare @now datetime2(7) = GETDATE()

            declare @chainLocaleTypeId int = (select localeTypeID from LocaleType where localeTypeDesc = 'Chain'),
                @regionLocaleTypeId int = (select localeTypeID from LocaleType where localeTypeDesc = 'Region'),
                @metroLocaleTypeId int = (select localeTypeID from LocaleType where localeTypeDesc = 'Metro'),
                @storeLocaleTypeId int = (select localeTypeID from LocaleType where localeTypeDesc = 'store'),
	            @businessUnitTraitId int = (select traitID from Trait where traitCode = 'BU'),
	            @storeAbbreviationTraitId int = (select traitID from Trait where traitCode = 'SAB'),
	            @phoneNumberTraitId int = (select traitID from Trait where traitCode = 'PHN'),
                @shippingAddressTypeId INT = (SELECT addressUsageID FROM AddressUsage WHERE addressUsageCode = 'SHP')

            set identity_insert Locale ON

            INSERT INTO Locale (localeID, localeName, localeOpenDate, localeCloseDate, localeTypeID, parentLocaleID, ownerOrgPartyID)
            values (9000, 'Test Chain', @now, @now, @chainLocaleTypeId, null, 1),
                (9001, 'Test Region', @now, @now, @chainLocaleTypeId, 9000, 1),
                (9002, 'Test Metro', @now, @now, @chainLocaleTypeId, 9001, 1),
                (9003, 'Test Store', @now, @now, @chainLocaleTypeId, 9002, 1)

            set identity_insert Locale off

            insert into LocaleTrait(localeId, traitId, traitValue)
            values (9003, @businessUnitTraitId, '12345'),
                (9003, @storeAbbreviationTraitId, 'TSA'),
                (9003, @phoneNumberTraitId, 'Test Phone Number')

            insert into Address(addressTypeId)
            values (@shippingAddressTypeId)

            declare @addressId int = SCOPE_IDENTITY()

            insert into LocaleAddress(localeID, addressID, addressUsageID)
            values (9003, @addressId, @shippingAddressTypeId)

            declare @countryId int = (select top 1 countryID from Country),
                @territoryId int = (select top 1 territoryID from Territory),
                @cityId int = (select top 1 cityID from City),
                @postalCodeId int = (select top 1 postalCodeID from PostalCode),
                @latitude decimal(9, 6) = 1,
                @longitude decimal(9, 6) = 2,
                @addressLine1 nvarchar(255) = 'Test Address Line 1',
                @addressLine2 nvarchar(255) = 'Test Address Line 2',
                @addressLine3 nvarchar(255) = 'Test Address Line 3',
                @timezoneId int = (select top 1 timezoneID from Timezone)

            insert into PhysicalAddress(addressID, countryID, territoryID, cityID, postalCodeID, latitude, longitude, addressLine1, addressLine2, addressLine3, timezoneID)
            values (@addressId, @countryId, @territoryId, @cityId, @postalCodeId, @latitude, @longitude, @addressLine1, @addressLine2, @addressLine3, @timezoneId)";

            connection.Execute(sql);
        }
    }
}
