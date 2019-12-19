﻿using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Managers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace Icon.Web.Tests.Unit.ManagerHandlers
{
    [TestClass]
    public class AddItemManagerHandlerTests
    {
        private AddItemManagerHandler managerHandler;
        private AddItemManager manager;
        private Mock<ICommandHandler<AddItemCommand>> mockAddItemCommandHandler;
        private Mock<ICommandHandler<PublishItemUpdatesCommand>> mockPublishItemUpdatesCommandHandler;

        [TestInitialize]
        public void Initialize()
        {
            mockAddItemCommandHandler = new Mock<ICommandHandler<AddItemCommand>>();
            mockPublishItemUpdatesCommandHandler = new Mock<ICommandHandler<PublishItemUpdatesCommand>>();

            managerHandler = new AddItemManagerHandler(
                mockAddItemCommandHandler.Object,
                mockPublishItemUpdatesCommandHandler.Object);

            manager = new AddItemManager();
        }

        [TestMethod]
        public void AddItemManager_NewItem_ShouldCallCommandHandlers()
        {
            //Given
            manager.BrandsHierarchyClassId = 1;
            manager.FinancialHierarchyClassId = 2;
            manager.ItemAttributes = new Dictionary<string, string>();
            manager.MerchandiseHierarchyClassId = 4;
            manager.NationalHierarchyClassId = 5;
            manager.ScanCode = "1234";
            manager.TaxHierarchyClassId = 6;
            manager.ManufacturerHierarchyClassId = 7;
            manager.BarCodeTypeId = 1;
            manager.ItemTypeCode = "Rtl";

            //When
            managerHandler.Execute(manager);

            //Then
            mockAddItemCommandHandler.Verify(m => m.Execute(It.Is<AddItemCommand>(
                c => c.BrandsHierarchyClassId == manager.BrandsHierarchyClassId
                    && c.FinancialHierarchyClassId == manager.FinancialHierarchyClassId
                    && c.ItemAttributes == manager.ItemAttributes
                    && c.MerchandiseHierarchyClassId == manager.MerchandiseHierarchyClassId
                    && c.NationalHierarchyClassId == manager.NationalHierarchyClassId
                    && c.SelectedBarCodeTypeId == manager.BarCodeTypeId
                    && c.ItemTypeCode == manager.ItemTypeCode
                    && c.ScanCode == manager.ScanCode
                    && c.TaxHierarchyClassId == manager.TaxHierarchyClassId
                    && c.ManufacturerHierarchyClassId == manager.ManufacturerHierarchyClassId)));
            mockPublishItemUpdatesCommandHandler.Verify(m => m.Execute(It.Is<PublishItemUpdatesCommand>(
                c => c.ScanCodes.Count == 1
                    && c.ScanCodes.Contains(manager.ScanCode))));
        }
    }
}