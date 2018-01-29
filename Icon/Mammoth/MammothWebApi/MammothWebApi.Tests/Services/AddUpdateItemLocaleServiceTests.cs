using Mammoth.Common.DataAccess;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Logging;
using MammothWebApi.DataAccess.Commands;
using MammothWebApi.DataAccess.Models;
using MammothWebApi.Service.Models;
using MammothWebApi.Service.Services;
using MammothWebApi.Tests.Helpers;
using MammothWebApi.Tests.ModelBuilders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace MammothWebApi.Tests.Services
{
    [TestClass]
    public class AddUpdateItemLocaleServiceTests
    {
        private Mock<ILogger> mockLogger;
        private Mock<ICommandHandler<StagingItemLocaleCommand>> mockItemLocaleStagingHandler;
        private Mock<ICommandHandler<StagingItemLocaleExtendedCommand>> mockItemLocaleExtendedStagingHandler;
        private Mock<ICommandHandler<StagingItemLocaleSupplierCommand>> mockItemLocaleSupplierStagingHandler;
        private Mock<ICommandHandler<StagingItemLocaleSupplierDeleteCommand>> mockItemLocaleSupplierDeleteStagingHandler;
        private Mock<ICommandHandler<AddOrUpdateItemLocaleCommand>> mockAddUpdateItemLocaleHandler;
        private Mock<ICommandHandler<AddOrUpdateItemLocaleExtendedCommand>> mockAddUpdateItemLocaleExtendedHandler;
        private Mock<ICommandHandler<AddOrUpdateItemLocaleSupplierCommand>> mockAddUpdateItemLocaleSupplierHandler;
        private Mock<ICommandHandler<DeleteItemLocaleSupplierCommand>> mockDeleteItemLocaleSupplierHandler;
        private Mock<ICommandHandler<AddEsbMessageQueueItemLocaleCommand>> mockAddToMessageQueueItemLocaleHandler;
        private Mock<ICommandHandler<DeleteStagingCommand>> mockDeleteStagingHandler;

        private AddUpdateItemLocaleService itemLocaleService;
        private AddUpdateItemLocale serviceData;
        private List<ItemLocaleServiceModel> itemLocales;

        [TestInitialize]
        public void Initialize()
        {
            this.mockLogger = new Mock<ILogger>();
            this.mockItemLocaleStagingHandler = new Mock<ICommandHandler<StagingItemLocaleCommand>>();
            this.mockItemLocaleExtendedStagingHandler = new Mock<ICommandHandler<StagingItemLocaleExtendedCommand>>();
            this.mockItemLocaleSupplierStagingHandler = new Mock<ICommandHandler<StagingItemLocaleSupplierCommand>>();
            this.mockItemLocaleSupplierDeleteStagingHandler = new Mock<ICommandHandler<StagingItemLocaleSupplierDeleteCommand>>();
            this.mockAddUpdateItemLocaleHandler = new Mock<ICommandHandler<AddOrUpdateItemLocaleCommand>>();
            this.mockAddUpdateItemLocaleExtendedHandler = new Mock<ICommandHandler<AddOrUpdateItemLocaleExtendedCommand>>();
            this.mockAddUpdateItemLocaleSupplierHandler = new Mock<ICommandHandler<AddOrUpdateItemLocaleSupplierCommand>>();
            this.mockDeleteItemLocaleSupplierHandler = new Mock<ICommandHandler<DeleteItemLocaleSupplierCommand>>();
            this.mockAddToMessageQueueItemLocaleHandler = new Mock<ICommandHandler<AddEsbMessageQueueItemLocaleCommand>>();
            this.mockDeleteStagingHandler = new Mock<ICommandHandler<DeleteStagingCommand>>();

            this.itemLocaleService = new AddUpdateItemLocaleService(
                this.mockLogger.Object,
                this.mockItemLocaleStagingHandler.Object,
                this.mockItemLocaleExtendedStagingHandler.Object,
                this.mockItemLocaleSupplierStagingHandler.Object,
                this.mockItemLocaleSupplierDeleteStagingHandler.Object,
                this.mockAddUpdateItemLocaleHandler.Object,
                this.mockAddUpdateItemLocaleExtendedHandler.Object,
                this.mockAddUpdateItemLocaleSupplierHandler.Object,
                this.mockDeleteItemLocaleSupplierHandler.Object,
                this.mockAddToMessageQueueItemLocaleHandler.Object,
                this.mockDeleteStagingHandler.Object);

            this.itemLocales = new List<ItemLocaleServiceModel>
            {
                new TestItemLocaleServiceModelBuilder().WithRegion("SW").WithScanCode("543210").WithBusinessUnit(1).Build(),
                new TestItemLocaleServiceModelBuilder().WithRegion("MW").WithScanCode("543211").WithBusinessUnit(2).Build(),
                new TestItemLocaleServiceModelBuilder().WithRegion("PN").WithScanCode("543212").WithBusinessUnit(3).Build()
            };

            this.serviceData = new AddUpdateItemLocale { ItemLocales = this.itemLocales };
        }

        [TestMethod]
        public void AddUpdateItemLocaleService_ValidItemLocaleModelWithNonNullExtendedProperties_ShouldCallBothStagingItemLocaleAndItemLocaleExtendedHandlersOneTime()
        {
            // Given.
            foreach (var item in this.itemLocales.OrderBy(il => il.ScanCode))
            {
                item.ColorAdded = true;
                item.CountryOfProcessing = "USA";
            };

            itemLocales = itemLocales.OrderBy(il => il.ScanCode).ThenBy(i => i.Region).ToList();

            // When.
            this.itemLocaleService.Handle(this.serviceData);

            // Then.
            this.mockItemLocaleStagingHandler.Verify(s => s.Execute(It.Is<StagingItemLocaleCommand>(c => c.ItemLocales.Count == 3)), Times.Once);

            for (int i = 0; i < 3; i++)
            {
                this.mockItemLocaleStagingHandler.Verify(s => s.Execute(It.Is<StagingItemLocaleCommand>(c =>
                    c.ItemLocales[i].ScanCode == this.itemLocales[i].ScanCode
                    && c.ItemLocales[i].Region == this.itemLocales[i].Region
                    && c.ItemLocales[i].BusinessUnitID == this.itemLocales[i].BusinessUnitId)), Times.Once);
            }

            int expectedCount = this.itemLocales.Count * ItemLocaleTestData.NumberOfExtendedAttributesPerItemLocale;

            this.mockItemLocaleExtendedStagingHandler
                .Verify(s => s.Execute(It.IsAny<StagingItemLocaleExtendedCommand>()), Times.Once);
            this.mockItemLocaleExtendedStagingHandler
                .Verify(s => s.Execute(It.Is<StagingItemLocaleExtendedCommand>(c =>
                    c.ItemLocalesExtended.Count == expectedCount)), Times.Once);
            this.mockItemLocaleExtendedStagingHandler
                .Verify(s => s.Execute(It.Is<StagingItemLocaleExtendedCommand>(c =>
                    c.ItemLocalesExtended.Count == expectedCount)), Times.Once);
            this.mockItemLocaleExtendedStagingHandler
                .Verify(s => s.Execute(It.Is<StagingItemLocaleExtendedCommand>(c =>
                    c.ItemLocalesExtended.Where(il => il.AttributeId == Attributes.ColorAdded).Count() == 3
                    && c.ItemLocalesExtended.Where(ils => ils.AttributeValue == "1").Count() == 3)), Times.Once);
            this.mockItemLocaleExtendedStagingHandler
                .Verify(s => s.Execute(It.Is<StagingItemLocaleExtendedCommand>(c =>
                    c.ItemLocalesExtended.Where(il => il.AttributeId == Attributes.CountryOfProcessing).Count() == 3
                    && c.ItemLocalesExtended.Where(ils => ils.AttributeValue == "USA").Count() == 3)), Times.Once);
            this.mockItemLocaleExtendedStagingHandler
                .Verify(s => s.Execute(It.Is<StagingItemLocaleExtendedCommand>(c =>
                    c.ItemLocalesExtended.Where(il => il.AttributeId == Attributes.ChicagoBaby).Count() == 3)), Times.Once);
            this.mockItemLocaleExtendedStagingHandler
                .Verify(s => s.Execute(It.Is<StagingItemLocaleExtendedCommand>(c =>
                    c.ItemLocalesExtended.Where(il => il.AttributeId == Attributes.ElectronicShelfTag).Count() == 3)), Times.Once);
            this.mockItemLocaleExtendedStagingHandler
                .Verify(s => s.Execute(It.Is<StagingItemLocaleExtendedCommand>(c =>
                    c.ItemLocalesExtended.Where(il => il.AttributeId == Attributes.Exclusive).Count() == 3)), Times.Once);
            this.mockItemLocaleExtendedStagingHandler
                .Verify(s => s.Execute(It.Is<StagingItemLocaleExtendedCommand>(c =>
                    c.ItemLocalesExtended.Where(il => il.AttributeId == Attributes.LinkedScanCode).Count() == 3)), Times.Once);
            this.mockItemLocaleExtendedStagingHandler
                .Verify(s => s.Execute(It.Is<StagingItemLocaleExtendedCommand>(c =>
                    c.ItemLocalesExtended.Where(il => il.AttributeId == Attributes.NumberOfDigitsSentToScale).Count() == 3)), Times.Once);
            this.mockItemLocaleExtendedStagingHandler
                .Verify(s => s.Execute(It.Is<StagingItemLocaleExtendedCommand>(c =>
                    c.ItemLocalesExtended.Where(il => il.AttributeId == Attributes.Origin).Count() == 3)), Times.Once);
            this.mockItemLocaleExtendedStagingHandler
                .Verify(s => s.Execute(It.Is<StagingItemLocaleExtendedCommand>(c =>
                    c.ItemLocalesExtended.Where(il => il.AttributeId == Attributes.ScaleExtraText).Count() == 3)), Times.Once);
            this.mockItemLocaleExtendedStagingHandler
                .Verify(s => s.Execute(It.Is<StagingItemLocaleExtendedCommand>(c =>
                    c.ItemLocalesExtended.Where(il => il.AttributeId == Attributes.TagUom).Count() == 3)), Times.Once);
            this.mockItemLocaleExtendedStagingHandler
                .Verify(s => s.Execute(It.Is<StagingItemLocaleExtendedCommand>(c =>
                    c.ItemLocalesExtended.Where(il => il.AttributeId == Attributes.ForceTare).Count() == 3)), Times.Once);
            this.mockItemLocaleExtendedStagingHandler
                .Verify(s => s.Execute(It.Is<StagingItemLocaleExtendedCommand>(c =>
                    c.ItemLocalesExtended.Where(il => il.AttributeId == Attributes.CfsSendToScale).Count() == 3)), Times.Once);
            this.mockItemLocaleExtendedStagingHandler
                .Verify(s => s.Execute(It.Is<StagingItemLocaleExtendedCommand>(c =>
                    c.ItemLocalesExtended.Where(il => il.AttributeId == Attributes.WrappedTareWeight).Count() == 3)), Times.Once);
            this.mockItemLocaleExtendedStagingHandler
                .Verify(s => s.Execute(It.Is<StagingItemLocaleExtendedCommand>(c =>
                    c.ItemLocalesExtended.Where(il => il.AttributeId == Attributes.UnwrappedTareWeight).Count() == 3)), Times.Once);
            this.mockItemLocaleExtendedStagingHandler
                .Verify(s => s.Execute(It.Is<StagingItemLocaleExtendedCommand>(c =>
                    c.ItemLocalesExtended.Where(il => il.AttributeId == Attributes.UseByEab).Count() == 3)), Times.Once);
            this.mockItemLocaleExtendedStagingHandler
                .Verify(s => s.Execute(It.Is<StagingItemLocaleExtendedCommand>(c =>
                    c.ItemLocalesExtended.Where(il => il.AttributeId == Attributes.ShelfLife).Count() == 3)), Times.Once);

            this.mockItemLocaleSupplierStagingHandler
                .Verify(s => s.Execute(It.IsAny<StagingItemLocaleSupplierCommand>()), Times.Once);
        }

        [TestMethod]
        public void AddUpdateItemLocaleService_ValidItemLocaleModelList_ShouldCallDeleteStagingCommandHandlerForBothItemLocaleAndItemLocaleExtended()
        {
            // Given.

            // When.
            this.itemLocaleService.Handle(this.serviceData);

            // Then.
            this.mockDeleteStagingHandler
                .Verify(s => s.Execute(It.Is<DeleteStagingCommand>(d => d.StagingTableName == StagingTableNames.ItemLocale)), Times.Once);
            this.mockDeleteStagingHandler
                .Verify(s => s.Execute(It.Is<DeleteStagingCommand>(d => d.StagingTableName == StagingTableNames.ItemLocaleExtended)), Times.Once);
            this.mockDeleteStagingHandler
                .Verify(s => s.Execute(It.Is<DeleteStagingCommand>(d => d.StagingTableName == StagingTableNames.ItemLocaleSupplier)), Times.Once);
        }

        [TestMethod]
        public void AddUpdateItemLocaleService_ItemLocaleModelHasOneRegionWithoutExtendedProperties_ShouldCallBothAddUpdateItemLocaleAndItemLocaleExtendedCommandHandlerOneTime()
        {
            // Given.
            DateTime now = DateTime.Now;

            foreach (var item in this.itemLocales)
            {
                item.Region = "SW";
            }

            // When.                                                                     
            this.itemLocaleService.Handle(this.serviceData);

            // Then.
            this.mockAddUpdateItemLocaleHandler
                .Verify(s => s.Execute(It.Is<AddOrUpdateItemLocaleCommand>(c => c.Region == "SW" && c.Timestamp >= now)), Times.Once);
            this.mockAddUpdateItemLocaleExtendedHandler
                .Verify(s => s.Execute(It.Is<AddOrUpdateItemLocaleExtendedCommand>(c => c.Region == "SW" && c.Timestamp >= now)), Times.Once);
            this.mockAddUpdateItemLocaleSupplierHandler
                .Verify(s => s.Execute(It.Is<AddOrUpdateItemLocaleSupplierCommand>(c => c.Region == "SW" && c.Timestamp >= now)), Times.Once);
        }

        [TestMethod]
        public void AddUpdateItemLocaleService_ItemLocaleModelHasMoreThanOneRegion_ShouldCallAddUpdateItemLocaleAndItemLocaleExtendedCommandHandlerOneTimeForEachRegion()
        {
            // Given.
            // ItemLocale list is setup with three different regions.
            // Expected count is 3.
            DateTime now = DateTime.Now;

            // When.                                                                   
            this.itemLocaleService.Handle(this.serviceData);

            // Then.
            this.mockAddUpdateItemLocaleHandler.Verify(s => s.Execute(It.IsAny<AddOrUpdateItemLocaleCommand>()), Times.Exactly(3));

            foreach (var item in this.serviceData.ItemLocales)
            {
                this.mockAddUpdateItemLocaleHandler
                    .Verify(s => s.Execute(It.Is<AddOrUpdateItemLocaleCommand>(c => c.Region == item.Region && c.Timestamp >= now)), Times.Once);
            }

            this.mockAddUpdateItemLocaleExtendedHandler.Verify(s => s.Execute(It.IsAny<AddOrUpdateItemLocaleExtendedCommand>()), Times.Exactly(3));

            foreach (var item in this.serviceData.ItemLocales)
            {
                this.mockAddUpdateItemLocaleExtendedHandler
                    .Verify(s => s.Execute(It.Is<AddOrUpdateItemLocaleExtendedCommand>(c => c.Region == item.Region && c.Timestamp >= now)), Times.Once);
            }

            this.mockAddUpdateItemLocaleSupplierHandler.Verify(s => s.Execute(It.IsAny<AddOrUpdateItemLocaleSupplierCommand>()), Times.Exactly(3));

            foreach (var item in this.serviceData.ItemLocales)
            {
                this.mockAddUpdateItemLocaleSupplierHandler
                    .Verify(s => s.Execute(It.Is<AddOrUpdateItemLocaleSupplierCommand>(c => c.Region == item.Region && c.Timestamp >= now)), Times.Once);
            }
        }

        [TestMethod]
        public void AddUpdateItemLocaleService_AttributesUpdatedForSingleRegionModel_MessagesShouldBeQueuedForOneRegion()
        {
            // Given.
            DateTime now = DateTime.Now;

            foreach (var item in this.itemLocales)
            {
                item.Region = "SW";
            }

            // When.                                                                   
            this.itemLocaleService.Handle(this.serviceData);

            // Then.
            this.mockAddToMessageQueueItemLocaleHandler
                .Verify(s => s.Execute(It.Is<AddEsbMessageQueueItemLocaleCommand>(c => c.Region == "SW" && c.Timestamp >= now)), Times.Once);
        }

        [TestMethod]
        public void AddUpdateItemLocaleService_AttributesUpdatedForMultipleRegionModel_MessagesShouldBeQueuedForMultipleRegions()
        {
            // Given.
            DateTime now = DateTime.Now;

            // When.                                                                   
            this.itemLocaleService.Handle(this.serviceData);

            // Then.
            foreach (var item in this.serviceData.ItemLocales)
            {
                this.mockAddToMessageQueueItemLocaleHandler
                    .Verify(s => s.Execute(It.Is<AddEsbMessageQueueItemLocaleCommand>(c => c.Region == item.Region && c.Timestamp >= now)), Times.Once);
            }
        }
        [TestMethod]
        public void AddUpdateItemLocaleService_ItemLocaleHandlerThrowsException_StagingDataDeleted()
        {
            // Given.
            Guid transactionId = Guid.NewGuid();
            this.mockAddUpdateItemLocaleHandler.Setup(s => s.Execute(It.IsAny<AddOrUpdateItemLocaleCommand>())).Throws<NullReferenceException>();

            // When.                                                                   
            try
            {
                this.itemLocaleService.Handle(this.serviceData);
            }
            // Then.
            catch
            {
                foreach (var item in this.serviceData.ItemLocales)
                {
                    this.mockDeleteStagingHandler.Verify(s => s.Execute(It.Is<DeleteStagingCommand>(c => c.StagingTableName == StagingTableNames.ItemLocale)), Times.Once);
                    this.mockDeleteStagingHandler.Verify(s => s.Execute(It.Is<DeleteStagingCommand>(c => c.StagingTableName == StagingTableNames.ItemLocaleExtended)), Times.Once);
                    this.mockDeleteStagingHandler.Verify(s => s.Execute(It.Is<DeleteStagingCommand>(c => c.StagingTableName == StagingTableNames.ItemLocaleSupplier)), Times.Once);
                }
            }
        }

        [TestMethod]
        public void AddUpdateItemLocaleService_AddUpdateItemLocaleExtendedHandlerThrowsException_StagingDataDeleted()
        {
            // Given.
            Guid transactionId = Guid.NewGuid();
            this.mockAddUpdateItemLocaleExtendedHandler.Setup(s => s.Execute(It.IsAny<AddOrUpdateItemLocaleExtendedCommand>())).Throws<NullReferenceException>();

            // When.                                                                   
            try
            {
                this.itemLocaleService.Handle(this.serviceData);
            }
            // Then.
            catch
            {
                foreach (var item in this.serviceData.ItemLocales)
                {
                    this.mockDeleteStagingHandler.Verify(s => s.Execute(It.Is<DeleteStagingCommand>(c => c.StagingTableName == StagingTableNames.ItemLocale)), Times.Once);
                    this.mockDeleteStagingHandler.Verify(s => s.Execute(It.Is<DeleteStagingCommand>(c => c.StagingTableName == StagingTableNames.ItemLocaleExtended)), Times.Once);
                }
            }
        }

        [TestMethod]
        public void AddUpdateItemLocaleService_AddToMessageQueueItemLocaleHandlerThrowsException_StagingDataDeleted()
        {
            // Given.
            Guid transactionId = Guid.NewGuid();
            this.mockAddToMessageQueueItemLocaleHandler.Setup(s => s.Execute(It.IsAny<AddEsbMessageQueueItemLocaleCommand>())).Throws<NullReferenceException>();

            // When.                                                                   
            try
            {
                this.itemLocaleService.Handle(this.serviceData);
            }
            // Then.
            catch
            {
                foreach (var item in this.serviceData.ItemLocales)
                {
                    this.mockDeleteStagingHandler.Verify(s => s.Execute(It.Is<DeleteStagingCommand>(c => c.StagingTableName == StagingTableNames.ItemLocale)), Times.Once);
                    this.mockDeleteStagingHandler.Verify(s => s.Execute(It.Is<DeleteStagingCommand>(c => c.StagingTableName == StagingTableNames.ItemLocaleExtended)), Times.Once);
                }
            }
        }

        [TestMethod]
        public void AddUpdateItemLocaleService_AddUpdateItemLocaleCommandThrowsException_ThrowsOriginalException()
        {
            // Given.
            string expectedExceptionMessage = "Test Exception";
            Exception expectedInnerException = new Exception("Test Inner Exception");
            Exception expectedException = new InvalidOperationException(expectedExceptionMessage, expectedInnerException);
            this.mockAddToMessageQueueItemLocaleHandler.Setup(s => s.Execute(It.IsAny<AddEsbMessageQueueItemLocaleCommand>()))
                .Throws(expectedException);

            // When.                                                                   
            try
            {
                this.itemLocaleService.Handle(this.serviceData);
            }
            // Then.
            catch (Exception actualException)
            {
                Assert.AreEqual(expectedExceptionMessage, actualException.Message);
                Assert.AreSame(expectedException, actualException);
                Assert.AreEqual(expectedInnerException.Message, actualException.InnerException.Message);
                Assert.AreSame(expectedInnerException, actualException.InnerException);
            }
        }
    }
}