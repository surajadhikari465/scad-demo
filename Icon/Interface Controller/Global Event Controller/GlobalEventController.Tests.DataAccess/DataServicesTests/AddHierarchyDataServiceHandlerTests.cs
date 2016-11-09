using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Irma.Framework;
using GlobalEventController.DataAccess.DataServices;
using Moq;
using GlobalEventController.DataAccess.Infrastructure;
using GlobalEventController.DataAccess.BulkCommands;
using GlobalEventController.Common;
using System.Collections.Generic;

namespace GlobalEventController.Tests.DataAccess.DataServicesTests
{
    [TestClass]
    public class AddHierarchyDataServiceHandlerTests
    {
        private AddHierarchyDataService service;
        private AddHierarchyDataServiceHandler handler;
        private Mock<ICommandHandler<BulkAddBrandCommand>> mockAddBrand;

        [TestInitialize]
        public void InitializeData()
        {
            this.mockAddBrand = new Mock<ICommandHandler<BulkAddBrandCommand>>();
            this.service = new AddHierarchyDataService();
            this.handler = new AddHierarchyDataServiceHandler(
                this.mockAddBrand.Object);

            this.service.BulkBrandCommand = new BulkAddBrandCommand();
        }

        [TestMethod]
        public void AddHierarchyDataService_AddHierarchyDataService_BulkAddBrandAndAddTaxCommandCalledOnce()
        {
            // Given
            // Nothing to setup

            // When
            this.handler.Process(this.service);

            // Then
            this.mockAddBrand.Verify(b => b.Handle(It.IsAny<BulkAddBrandCommand>()), Times.Once,
                "BulkAddBrandCommand was not called exactly one time.");
        }

        [TestMethod]
        public void AddHierarchyDataService_AddHierarchyDataService_ValidatedItemWithBrandIdBrandNameSetByAddBrandCommand()
        {
            // Given
            this.service.BulkBrandCommand.ValidatedItems = new List<ValidatedItemModel> {new ValidatedItemModel { BrandId = 20, BrandName = "Test Brand" }};
            ValidatedItemModel expected = new ValidatedItemModel { BrandId = 20, BrandName = "Test Brand" };

            // When
            this.handler.Process(this.service);

            // Then
            Assert.AreEqual(expected.BrandId, this.service.BulkBrandCommand.ValidatedItems[0].BrandId, "The BrandId was not set properly.");
            Assert.AreEqual(expected.BrandName, this.service.BulkBrandCommand.ValidatedItems[0].BrandName, "The BrandName was not set properly.");
        }
    }
}
