using Dapper;
using Icon.Esb.Subscriber;
using Icon.Framework;
using Icon.Infor.Listeners.LocaleListener.Commands;
using Icon.Infor.Listeners.LocaleListener.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Contracts = Icon.Esb.Schemas.Wfm.Contracts;

namespace Icon.Infor.Listeners.LocaleListener.Tests.Commands
{
    [TestClass]
    public class ArchiveLocaleMessageCommandHandlerTests
    {
        private ArchiveLocaleMessageCommandHandler commandHandler;
        private ArchiveLocaleMessageCommand command;
        private Mock<IEsbMessage> mockMessage;
        private TransactionScope transaction;
        private SqlConnection connection;

        [TestInitialize]
        public void Initialize()
        {
            transaction = new TransactionScope();

            var connectionString = ConfigurationManager.ConnectionStrings["Icon"].ConnectionString;

            commandHandler = new ArchiveLocaleMessageCommandHandler(connectionString);
            command = new ArchiveLocaleMessageCommand();
            mockMessage = new Mock<IEsbMessage>();

            connection = new SqlConnection(connectionString);
            connection.Execute("delete infor.MessageArchiveLocale");
            //connection.Execute("delete infor.MessageHistory");
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose();
        }

        [TestMethod]
        public void ArchiveLocaleMessage_OneMessage_ShouldArchiveLocaleMessage()
        {
            //Given
            Guid testMessageId = Guid.NewGuid();

            var company = CreateLocale(1234, "Test Company", "CMP");
            var chain = CreateLocale(12345, "Test Chain", "CHN");
            var region = CreateLocale(123456, "Test Region", "REG");
            var metro = CreateLocale(1234567, "Test Metro", "MTR");
            var store = CreateLocale(12345678, "Test Store", "STR");
            company.Locales = new List<LocaleModel> { chain };
            chain.Locales = new List<LocaleModel> { region };
            region.Locales = new List<LocaleModel> { metro };
            metro.Locales = new List<LocaleModel> { store };

            command.Locale = company;

            command.Message = mockMessage.Object;
            mockMessage.SetupGet(m => m.MessageText)
                .Returns("<test>Test</test>");
            mockMessage.Setup(m => m.GetProperty("MessageID"))
                .Returns(testMessageId.ToString());

            //When
            commandHandler.Execute(command);

            //Then
            var archivedLocales = connection.Query<MessageArchiveLocaleModel>("select * from infor.MessageArchiveLocale");

            Assert.AreEqual(5, archivedLocales.Count());
            var companyArchivedLocale = archivedLocales.Single(l => l.LocaleId == company.LocaleId);
            var chainArchivedLocale = archivedLocales.Single(l => l.LocaleId == chain.LocaleId);
            var regionArchivedLocale = archivedLocales.Single(l => l.LocaleId == region.LocaleId);
            var metroArchivedLocale = archivedLocales.Single(l => l.LocaleId == metro.LocaleId);
            var storeArchivedLocale = archivedLocales.Single(l => l.BusinessUnitId == store.BusinessUnitId);

            AssertLocaleIsEqualToArchivedLocale(company, companyArchivedLocale, testMessageId);
            AssertLocaleIsEqualToArchivedLocale(chain, chainArchivedLocale, testMessageId);
            AssertLocaleIsEqualToArchivedLocale(region, regionArchivedLocale, testMessageId);
            AssertLocaleIsEqualToArchivedLocale(metro, metroArchivedLocale, testMessageId);
            AssertLocaleIsEqualToArchivedLocale(store, storeArchivedLocale, testMessageId);

            var messageHistory = connection.Query<InforMessageHistoryModel>("select * from infor.MessageHistory where inforMessageId = " + "'"+ testMessageId + "'").Single();
            Assert.AreEqual(MessageTypes.Locale, messageHistory.MessageTypeId);
            Assert.AreEqual(MessageStatusTypes.Consumed, messageHistory.MessageStatusId);
            Assert.AreEqual("<test>Test</test>", messageHistory.Message);
        }

        private void AssertLocaleIsEqualToArchivedLocale(LocaleModel localeModel, MessageArchiveLocaleModel archiveModel, Guid messageId)
        {
            if (localeModel.TypeCode == "STR")
            {
                Assert.IsNull(archiveModel.LocaleId);
            }
            else
            {
                Assert.AreEqual(localeModel.LocaleId, archiveModel.LocaleId);
            }
            if(localeModel.TypeCode == "STR")
            {
                Assert.AreEqual(localeModel.BusinessUnitId, archiveModel.BusinessUnitId);
            }
            else
            {
                Assert.IsNull(archiveModel.BusinessUnitId);
            }
            Assert.AreEqual(localeModel.Name, archiveModel.LocaleName);
            Assert.AreEqual(localeModel.TypeCode, archiveModel.LocaleTypeCode);
            Assert.AreEqual(messageId, archiveModel.InforMessageId);
            Assert.AreEqual(localeModel.Action.ToString(), archiveModel.Action);
            Assert.AreEqual(JsonConvert.SerializeObject(localeModel), archiveModel.Context);
            Assert.AreEqual(localeModel.ErrorCode, archiveModel.ErrorCode);
            Assert.AreEqual(localeModel.ErrorDetails, archiveModel.ErrorDetails);
        }

        private LocaleModel CreateLocale(int id, string localeName, string typeCode)
        {
            return new LocaleModel
            {
                LocaleId = typeCode == "STR" ? 0 : id,
                BusinessUnitId = typeCode == "STR" ? id : 0,
                Action = Contracts.ActionEnum.AddOrUpdate,
                Name = localeName,
                TypeCode = typeCode
            };
        }
    }
}
