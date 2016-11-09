using Icon.RenewableContext;
using Icon.Common.DataAccess;
using Icon.Esb.EwicAplListener.Common.Models;
using Icon.Esb.EwicAplListener.DataAccess.Commands;
using Icon.Esb.EwicAplListener.DataAccess.Queries;
using Icon.Esb.EwicAplListener.StorageServices;
using Icon.Framework;
using Icon.Framework.RenewableContext;
using Icon.Logging;
using Icon.Testing.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Icon.Esb.EwicAplListener.Tests.Integration.StorageServices
{
    [TestClass]
    public class IconDbStorageServiceTests
    {
        private IconDbStorageService storageService;
        private IRenewableContext<IconContext> globalContext;
        private Mock<ILogger<IconDbStorageService>> mockLogger;
        private ICommandHandler<AddMessageHistoryParameters> addMessageHistoryCommand;
        private IQueryHandler<AgencyExistsParameters, bool> agencyExistsQuery;
        private ICommandHandler<AddAgencyParameters> addAgencyCommand;
        private ICommandHandler<AddOrUpdateAuthorizedProductListParameters> updateAplCommand;
        private IconContext context;
        private DbContextTransaction transaction;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            globalContext = new GlobalIconContext(context);

            mockLogger = new Mock<ILogger<IconDbStorageService>>();
            addMessageHistoryCommand = new AddMessageHistoryCommand(globalContext);
            agencyExistsQuery = new AgencyExistsQuery(globalContext);
            addAgencyCommand = new AddAgencyCommand(globalContext);
            updateAplCommand = new AddOrUpdateAuthorizedProductListCommand(globalContext);

            storageService = new IconDbStorageService(
                mockLogger.Object,
                addMessageHistoryCommand,
                agencyExistsQuery,
                addAgencyCommand,
                updateAplCommand);

            transaction = context.Database.BeginTransaction();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
            context.Dispose();
            globalContext = null;
        }

        [TestMethod]
        public void Save_ValidMessage_MessageShouldBeSavedToDatabase()
        {
            // Given.
            var now = DateTime.Now.AddMinutes(-1);
            
            var testItems = new List<EwicItemModel>
            {
                new EwicItemModel
                {
                    AgencyId = "ZZ",
                    ScanCode = "22222222",
                    ItemDescription = "Test Description",
                    PackageSize = 1m,
                    UnitOfMeasure = "EA",
                    BenefitQuantity = 1m,
                    BenefitUnitDescription = "EA",
                    ItemPrice = 1.99m,
                    PriceType = "RG"
                }
            };

            var aplModel = new AuthorizedProductListModel
            {
                MessageXml = String.Empty,
                Items = testItems
            };

            // When.
            storageService.Save(aplModel);

            // Then.
            var newMessageHistory = context.MessageHistory.SingleOrDefault(m => 
                m.MessageTypeId == MessageTypes.Ewic && 
                m.MessageStatusId == MessageStatusTypes.Consumed &&
                m.ProcessedDate > now);

            Assert.IsNotNull(newMessageHistory);
        }

        [TestMethod]
        public void Save_NewAgencyDoesNotExist_AgencyShouldBeAddedToDatabase()
        {
            // Given.
            var testItems = new List<EwicItemModel>
            {
                new EwicItemModel
                {
                    AgencyId = "ZZ",
                    ScanCode = "22222222",
                    ItemDescription = "Test Description",
                    PackageSize = 1m,
                    UnitOfMeasure = "EA",
                    BenefitQuantity = 1m,
                    BenefitUnitDescription = "EA",
                    ItemPrice = 1.99m,
                    PriceType = "RG"
                }
            };

            var aplModel = new AuthorizedProductListModel
            {
                MessageXml = String.Empty,
                Items = testItems
            };

            // When.
            storageService.Save(aplModel);

            // Then.
            string agencyId = context.Agency.SingleOrDefault(a => a.AgencyId == "ZZ").AgencyId;

            Assert.IsNotNull(agencyId);
        }

        [TestMethod]
        public void Save_NewAgencyHasBeenAdded_ItemInformationShouldBeAddedToAplTable()
        {
            // Given.
            var testItems = new List<EwicItemModel>
            {
                new EwicItemModel
                {
                    AgencyId = "ZZ",
                    ScanCode = "22222222",
                    ItemDescription = "Test Description",
                    PackageSize = 1m,
                    UnitOfMeasure = "EA",
                    BenefitQuantity = 1m,
                    BenefitUnitDescription = "EA",
                    ItemPrice = 1.99m,
                    PriceType = "RG"
                }
            };

            var aplModel = new AuthorizedProductListModel
            {
                MessageXml = String.Empty,
                Items = testItems
            };

            // When.
            storageService.Save(aplModel);

            // Then.
            string agencyId = context.Agency.SingleOrDefault(a => a.AgencyId == "ZZ").AgencyId;
            var newAplEntry = context.AuthorizedProductList.SingleOrDefault(a => a.AgencyId == "ZZ" && a.ScanCode == "22222222");

            Assert.IsNotNull(agencyId);
            Assert.IsNotNull(newAplEntry);
            Assert.AreEqual("ZZ", newAplEntry.AgencyId);
            Assert.AreEqual("22222222", newAplEntry.ScanCode);
            Assert.AreEqual("Test Description", newAplEntry.ItemDescription);
            Assert.AreEqual(1m, newAplEntry.PackageSize);
            Assert.AreEqual("EA", newAplEntry.UnitOfMeasure);
            Assert.AreEqual(1m, newAplEntry.BenefitQuantity);
            Assert.AreEqual("EA", newAplEntry.BenefitUnitDescription);
            Assert.AreEqual(1.99m, newAplEntry.ItemPrice);
            Assert.AreEqual("RG", newAplEntry.PriceType);
            Assert.AreEqual(DateTime.Now.Date, newAplEntry.InsertDate.Date);
            Assert.IsNull(newAplEntry.ModifiedDate);
        }

        [TestMethod]
        public void Save_AgencyAlreadyExists_ItemInformationShouldBeAddedToAplTable()
        {
            // Given.
            var testAgency = new TestAgencyBuilder().WithAgencyId("ZZ");

            context.Agency.Add(testAgency);
            context.SaveChanges();

            var testItems = new List<EwicItemModel>
            {
                new EwicItemModel
                {
                    AgencyId = "ZZ",
                    ScanCode = "22222222",
                    ItemDescription = "Test Description",
                    PackageSize = 1m,
                    UnitOfMeasure = "EA",
                    BenefitQuantity = 1m,
                    BenefitUnitDescription = "EA",
                    ItemPrice = 1.99m,
                    PriceType = "RG"
                }
            };

            var aplModel = new AuthorizedProductListModel
            {
                MessageXml = String.Empty,
                Items = testItems
            };

            // When.
            storageService.Save(aplModel);

            // Then.
            string agencyId = context.Agency.SingleOrDefault(a => a.AgencyId == "ZZ").AgencyId;
            var newAplEntry = context.AuthorizedProductList.SingleOrDefault(a => a.AgencyId == "ZZ" && a.ScanCode == "22222222");

            Assert.IsNotNull(agencyId);
            Assert.IsNotNull(newAplEntry);
            Assert.AreEqual("ZZ", newAplEntry.AgencyId);
            Assert.AreEqual("22222222", newAplEntry.ScanCode);
            Assert.AreEqual("Test Description", newAplEntry.ItemDescription);
            Assert.AreEqual(1m, newAplEntry.PackageSize);
            Assert.AreEqual("EA", newAplEntry.UnitOfMeasure);
            Assert.AreEqual(1m, newAplEntry.BenefitQuantity);
            Assert.AreEqual("EA", newAplEntry.BenefitUnitDescription);
            Assert.AreEqual(1.99m, newAplEntry.ItemPrice);
            Assert.AreEqual("RG", newAplEntry.PriceType);
            Assert.AreEqual(DateTime.Now.Date, newAplEntry.InsertDate.Date);
            Assert.IsNull(newAplEntry.ModifiedDate);
        }

        [TestMethod]
        public void Save_AplEntryAlreadyExists_AplEntryShouldBeUpdated()
        {
            // Given.
            var testAgency = new TestAgencyBuilder().WithAgencyId("ZZ");

            context.Agency.Add(testAgency);
            context.SaveChanges();

            var testItems = new List<EwicItemModel>
            {
                new EwicItemModel
                {
                    AgencyId = "ZZ",
                    ScanCode = "22222222",
                    ItemDescription = "Test Description",
                    PackageSize = 1m,
                    UnitOfMeasure = "EA",
                    BenefitQuantity = 1m,
                    BenefitUnitDescription = "EA",
                    ItemPrice = 1.99m,
                    PriceType = "RG"
                },
                new EwicItemModel
                {
                    AgencyId = "ZZ",
                    ScanCode = "22222222",
                    ItemDescription = "Test Description Updated",
                    PackageSize = 1m,
                    UnitOfMeasure = "EA",
                    BenefitQuantity = 1m,
                    BenefitUnitDescription = "EA",
                    ItemPrice = 1.99m,
                    PriceType = "RG"
                }
            };

            var aplModel = new AuthorizedProductListModel
            {
                MessageXml = String.Empty,
                Items = testItems
            };

            // When.
            storageService.Save(aplModel);

            // Then.
            string agencyId = context.Agency.SingleOrDefault(a => a.AgencyId == "ZZ").AgencyId;
            var newAplEntry = context.AuthorizedProductList.SingleOrDefault(a => a.AgencyId == "ZZ" && a.ScanCode == "22222222");

            Assert.IsNotNull(agencyId);
            Assert.IsNotNull(newAplEntry);
            Assert.AreEqual("ZZ", newAplEntry.AgencyId);
            Assert.AreEqual("22222222", newAplEntry.ScanCode);
            Assert.AreEqual("Test Description Updated", newAplEntry.ItemDescription);
            Assert.AreEqual(1m, newAplEntry.PackageSize);
            Assert.AreEqual("EA", newAplEntry.UnitOfMeasure);
            Assert.AreEqual(1m, newAplEntry.BenefitQuantity);
            Assert.AreEqual("EA", newAplEntry.BenefitUnitDescription);
            Assert.AreEqual(1.99m, newAplEntry.ItemPrice);
            Assert.AreEqual("RG", newAplEntry.PriceType);
            Assert.AreEqual(DateTime.Now.Date, newAplEntry.InsertDate.Date);
            Assert.AreEqual(DateTime.Now.Date, newAplEntry.ModifiedDate.Value.Date);
        }
    }
}
