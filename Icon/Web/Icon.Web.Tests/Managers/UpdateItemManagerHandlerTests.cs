using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Managers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace Icon.Web.Tests.Unit.ManagerHandlers
{
    [TestClass]
    public class UpdateItemManagerHandlerTests
    {
        private UpdateItemManagerHandler managerHandler;
        private UpdateItemManager manager;
        private Mock<ICommandHandler<UpdateItemCommand>> mockUpdateItemCommandHandler;

        [TestInitialize]
        public void Initialize()
        {
            mockUpdateItemCommandHandler = new Mock<ICommandHandler<UpdateItemCommand>>();

            managerHandler = new UpdateItemManagerHandler(mockUpdateItemCommandHandler.Object);

            manager = new UpdateItemManager();
        }

        [TestMethod]
        public void UpdateItemManager_GivenAnItem_ShouldCallCommandHandlers()
        {
            //Given
            manager.BrandsHierarchyClassId = 1;
            manager.FinancialHierarchyClassId = 2;
            manager.ItemAttributes = new Dictionary<string, string>();
            manager.ItemAttributes.Add("ProductDescription","TestProductDescription");
            manager.ItemId = 3;
            manager.MerchandiseHierarchyClassId = 4;
            manager.NationalHierarchyClassId = 5;
            manager.ScanCode = "1234";
            manager.TaxHierarchyClassId = 6;

            //When
            managerHandler.Execute(manager);

            //Then
            mockUpdateItemCommandHandler.Verify(m => m.Execute(It.Is<UpdateItemCommand>(
                c => c.BrandsHierarchyClassId == manager.BrandsHierarchyClassId
                    && c.FinancialHierarchyClassId == manager.FinancialHierarchyClassId
                    && c.ItemAttributes == manager.ItemAttributes
                    && c.ItemId == manager.ItemId
                    && c.MerchandiseHierarchyClassId == manager.MerchandiseHierarchyClassId
                    && c.NationalHierarchyClassId == manager.NationalHierarchyClassId
                    && c.TaxHierarchyClassId == manager.TaxHierarchyClassId)));
        }
    }
}
