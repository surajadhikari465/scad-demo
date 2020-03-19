using BulkItemUploadProcessor.Common;
using BulkItemUploadProcessor.Common.Models;
using BulkItemUploadProcessor.DataAccess.Commands;
using BulkItemUploadProcessor.Service.BulkUpload;
using Icon.Common.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace BulkItemUploadProcessor.Service.Tests
{
    [TestClass]
    public class AddUpdateItemManagerTests
    {
        private Mock<ICommandHandler<UpdateItemsCommand>> mockUpdateItemsCommandHandler;
        private Mock<ICommandHandler<AddItemsCommand>> mockAddItemsCommandHandler;
        private AddUpdateItemManager addUpdateItemManager;
        private List<UpdateItemModel> updateItemModels;
        private List<ErrorItem<UpdateItemModel>> invalidUpdateItems;
        private List<ItemIdAndScanCode> updatedItems;
        private List<AddItemModel> addItemModels;
        private List<ErrorItem<AddItemModel>> invalidAddItems;
        private List<ItemIdAndScanCode> addedItems;

        [TestInitialize]
        public void Init()
        {
            mockUpdateItemsCommandHandler = new Mock<ICommandHandler<UpdateItemsCommand>>();
            mockAddItemsCommandHandler = new Mock<ICommandHandler<AddItemsCommand>>();

            addUpdateItemManager = new AddUpdateItemManager(mockUpdateItemsCommandHandler.Object,
                mockAddItemsCommandHandler.Object
               );
        }

        [TestMethod]
        public void Validate_UpdateItemsPassFourItemsWithAllFourHavingError_ShouldCallCommandHandlerSevenTimes()
        {
            invalidUpdateItems = new List<ErrorItem<UpdateItemModel>>();
            updatedItems = new List<ItemIdAndScanCode>();

            mockUpdateItemsCommandHandler.Setup(m => m.Execute(It.IsAny<UpdateItemsCommand>())).Throws(new System.Exception());
            updateItemModels = new List<UpdateItemModel>()
            {
               new UpdateItemModel(){ ItemId = -1,ScanCode = "4011",BrandsHierarchyClassId = 1,FinancialHierarchyClassId = 1,
                   MerchandiseHierarchyClassId =1, TaxHierarchyClassId=1,ManufacturerHierarchyClassId =1,ItemAttributesJson = "", ItemTypeId = 0},
               new UpdateItemModel(){ ItemId = -2,ScanCode = "4012",BrandsHierarchyClassId = 1,FinancialHierarchyClassId = 1,
                   MerchandiseHierarchyClassId =1, TaxHierarchyClassId=1,ManufacturerHierarchyClassId =1,ItemAttributesJson = "", ItemTypeId = 0},
               new UpdateItemModel(){ ItemId = -3,ScanCode = "4013",BrandsHierarchyClassId = 1,FinancialHierarchyClassId = 1,
                   MerchandiseHierarchyClassId =1, TaxHierarchyClassId=1,ManufacturerHierarchyClassId =1,ItemAttributesJson = "", ItemTypeId = 0},
               new UpdateItemModel(){ ItemId = -4,ScanCode = "4014",BrandsHierarchyClassId = 1,FinancialHierarchyClassId = 1,
                   MerchandiseHierarchyClassId =1, TaxHierarchyClassId=1,ManufacturerHierarchyClassId =1,ItemAttributesJson = "", ItemTypeId = 0}

            };

            //When
            addUpdateItemManager.UpdateItems(updateItemModels, invalidUpdateItems, updatedItems);

            //then
            mockUpdateItemsCommandHandler.Verify(m => m.Execute(It.IsAny<UpdateItemsCommand>()), Times.Exactly(7));
        }

        [TestMethod]
        public void Validate_AddItemsPassFourItemsWithAllFourHavingError_ShouldCallCommandHandlerSevenTimes()
        {
            invalidAddItems = new List<ErrorItem<AddItemModel>>();
            addedItems = new List<ItemIdAndScanCode>();

            mockAddItemsCommandHandler.Setup(m => m.Execute(It.IsAny<AddItemsCommand>())).Throws(new System.Exception());
            addItemModels = new List<AddItemModel>()
            {
               new AddItemModel(){ ScanCode = "4011",BarCodeTypeId = -1, BrandsHierarchyClassId = 1,FinancialHierarchyClassId = 1,
                   MerchandiseHierarchyClassId =1, TaxHierarchyClassId=1,ManufacturerHierarchyClassId =1,ItemAttributesJson = "", ItemTypeId = 0},
               new AddItemModel(){ ScanCode = "4012",BarCodeTypeId = -1,  BrandsHierarchyClassId = 1,FinancialHierarchyClassId = 1,
                   MerchandiseHierarchyClassId =1,TaxHierarchyClassId=1,ManufacturerHierarchyClassId =1,ItemAttributesJson = "", ItemTypeId = 0},
               new AddItemModel(){ ScanCode = "4013", BarCodeTypeId = -1, BrandsHierarchyClassId = 1,FinancialHierarchyClassId = 1,
                   MerchandiseHierarchyClassId =1,TaxHierarchyClassId=1,ManufacturerHierarchyClassId =1,ItemAttributesJson = "", ItemTypeId = 0},
               new AddItemModel(){ ScanCode = "4014",BarCodeTypeId = -1,  BrandsHierarchyClassId = 1,FinancialHierarchyClassId = 1,
                   MerchandiseHierarchyClassId =1, TaxHierarchyClassId=1,ManufacturerHierarchyClassId =1,ItemAttributesJson = "", ItemTypeId = 0}

            };

            //When
            addUpdateItemManager.CreateItems(addItemModels, invalidAddItems, addedItems);

            //then
            mockAddItemsCommandHandler.Verify(m => m.Execute(It.IsAny<AddItemsCommand>()), Times.Exactly(7));
        }

        [TestMethod]
        public void Validate_AddItemsPassFourItemsNoErrors_ShouldCallCommandHandlerOneTime()
        {
            invalidAddItems = new List<ErrorItem<AddItemModel>>();
            addedItems = new List<ItemIdAndScanCode>();

             addItemModels = new List<AddItemModel>()
            {
               new AddItemModel(){ ScanCode = "4011",BarCodeTypeId = -1, BrandsHierarchyClassId = 1,FinancialHierarchyClassId = 1,
                   MerchandiseHierarchyClassId =1, TaxHierarchyClassId=1,ManufacturerHierarchyClassId =1,ItemAttributesJson = "", ItemTypeId = 0},
               new AddItemModel(){ ScanCode = "4012",BarCodeTypeId = -1,  BrandsHierarchyClassId = 1,FinancialHierarchyClassId = 1,
                   MerchandiseHierarchyClassId =1,TaxHierarchyClassId=1,ManufacturerHierarchyClassId =1,ItemAttributesJson = "", ItemTypeId = 0},
               new AddItemModel(){ ScanCode = "4013", BarCodeTypeId = -1, BrandsHierarchyClassId = 1,FinancialHierarchyClassId = 1,
                   MerchandiseHierarchyClassId =1,TaxHierarchyClassId=1,ManufacturerHierarchyClassId =1,ItemAttributesJson = "", ItemTypeId = 0},
               new AddItemModel(){ ScanCode = "4014",BarCodeTypeId = -1,  BrandsHierarchyClassId = 1,FinancialHierarchyClassId = 1,
                   MerchandiseHierarchyClassId =1, TaxHierarchyClassId=1,ManufacturerHierarchyClassId =1,ItemAttributesJson = "", ItemTypeId = 0}

            };

            //When
            addUpdateItemManager.CreateItems(addItemModels, invalidAddItems, addedItems);

            //then
            mockAddItemsCommandHandler.Verify(m => m.Execute(It.IsAny<AddItemsCommand>()), Times.Exactly(1));
        }
    }
}