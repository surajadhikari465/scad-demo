using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.Common.Validators;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Managers;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;

namespace Icon.Web.Tests.Unit.Integration
{
    [TestClass]
    public class AddItemManagerHandlerTests
    {
        private AddItemManagerHandler managerHandler;
        private AddItemManager manager;
        private Mock<IObjectValidator<AddItemManager>> mockAddItemManagerValidator;
        private Mock<ICommandHandler<BulkImportCommand<BulkImportNewItemModel>>> mockBulkImportNewItemCommandHandler;
        private Mock<IQueryHandler<GetHierarchyClassByNameParameters, HierarchyClass>> mockGetHierarchyClassByNameQuery;
        private Mock<IQueryHandler<GetTaxClassByTaxRomanceParameters, HierarchyClass>> mockGetTaxClassByTaxRomanceQuery;

        [TestInitialize]
        public void InitializeData()
        {
            mockAddItemManagerValidator = new Mock<IObjectValidator<AddItemManager>>();
            mockBulkImportNewItemCommandHandler = new Mock<ICommandHandler<BulkImportCommand<BulkImportNewItemModel>>>();
            mockGetHierarchyClassByNameQuery = new Mock<IQueryHandler<GetHierarchyClassByNameParameters, HierarchyClass>>();
            mockGetTaxClassByTaxRomanceQuery = new Mock<IQueryHandler<GetTaxClassByTaxRomanceParameters, HierarchyClass>>();
            managerHandler = new AddItemManagerHandler(mockAddItemManagerValidator.Object, mockBulkImportNewItemCommandHandler.Object, mockGetHierarchyClassByNameQuery.Object, mockGetTaxClassByTaxRomanceQuery.Object);            

            manager = new AddItemManager
            {
                Item = new BulkImportNewItemModel
                {
                    ScanCode = "1112223334445",
                    BrandName = "AddItemManagerHandler Test Brands",
                    FoodStampEligible = "1",
                    IsValidated = "1",
                    MerchandiseId = "1",
                    PackageUnit = "1",
                    PosDescription = "Test Pos Description",
                    PosScaleTare = "1",
                    ProductDescription = "Test Product Description",
                    TaxId = "1"
                },
                UserName = "IconUser"
            };
        }

        [TestMethod]
        public void AddItemManager_ValidManager_ShouldCallValidatorAndCommandHandler()
        {
            //Given
            mockAddItemManagerValidator.Setup(v => v.Validate(It.IsAny<AddItemManager>()))
                .Returns(ObjectValidationResult.ValidResult);

            //When
            managerHandler.Execute(manager);

            //Then
            mockAddItemManagerValidator.Verify(v => v.Validate(It.IsAny<AddItemManager>()), Times.Once);
            mockBulkImportNewItemCommandHandler.Verify(v => v.Execute(It.IsAny<BulkImportCommand<BulkImportNewItemModel>>()), Times.Once);
        }

        [TestMethod]
        public void AddItemManager_InvalidManager_ShouldThrowArgumentException()
        {
            //Given
            mockAddItemManagerValidator.Setup(v => v.Validate(It.IsAny<AddItemManager>()))
                .Returns(ObjectValidationResult.InvalidResult("Test Exception"));

            //When
            Exception exception = null;
            try
            {
                managerHandler.Execute(manager);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            //Then
            Assert.IsNotNull(exception);
            Assert.AreEqual("Cannot add item. Error details: Test Exception", exception.Message);
            mockAddItemManagerValidator.Verify(v => v.Validate(It.IsAny<AddItemManager>()), Times.Once);
            mockBulkImportNewItemCommandHandler.Verify(v => v.Execute(It.IsAny<BulkImportCommand<BulkImportNewItemModel>>()), Times.Never);
        }

        [TestMethod]
        public void AddItemManager_BulkImportThrowsException_ShouldThrowCommandException()
        {
            //Given
            mockAddItemManagerValidator.Setup(v => v.Validate(It.IsAny<AddItemManager>()))
                .Returns(ObjectValidationResult.ValidResult);
            mockBulkImportNewItemCommandHandler.Setup(c => c.Execute(It.IsAny<BulkImportCommand<BulkImportNewItemModel>>()))
                .Throws(new Exception("Test Exception"));

            //When
            Exception exception = null;
            try
            {
                managerHandler.Execute(manager);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            //Then
            Assert.IsNotNull(exception);
            Assert.AreEqual("There was an error adding Scan Code 1112223334445: Test Exception", exception.Message);
            mockAddItemManagerValidator.Verify(v => v.Validate(It.IsAny<AddItemManager>()), Times.Once);
            mockBulkImportNewItemCommandHandler.Verify(v => v.Execute(It.IsAny<BulkImportCommand<BulkImportNewItemModel>>()), Times.Once);
        } 
        
        [TestMethod]
        public void AddItemManager_MerchIdIsNullAndMerchNameIsSupplied_ShouldReturnMerchIDAndPassMerchIdToTheCommandHandler()
        {
            //Given
            HierarchyClass testMearch = new HierarchyClass { hierarchyClassID = 25 };
            mockAddItemManagerValidator.Setup(v => v.Validate(It.IsAny<AddItemManager>()))
                .Returns(ObjectValidationResult.ValidResult);
            mockGetHierarchyClassByNameQuery
                .Setup(m => m.Search(It.Is<GetHierarchyClassByNameParameters>(p =>
                    p.HierarchyName == HierarchyNames.Merchandise && p.HierarchyClassName == "TestMerchName")))
                .Returns(testMearch)
                .Verifiable();
           
            manager.Item.MerchandiseId = null;
            manager.Item.MerchandiseLineage = "TestMerchName";

            //When
            managerHandler.Execute(manager); 

            //Then
            mockAddItemManagerValidator.Verify(v => v.Validate(It.IsAny<AddItemManager>()), Times.Once);
            mockGetHierarchyClassByNameQuery.Verify();
            mockBulkImportNewItemCommandHandler
                .Verify(v => v.Execute(It.Is<BulkImportCommand<BulkImportNewItemModel>>(m =>
                    m.BulkImportData.First().MerchandiseId == 25.ToString())), Times.Once);
        }

        [TestMethod]
        public void AddItemManager_InValidMerchNameIsSupplied_ShouldThrowException()
        {
            //Given
            HierarchyClass testMearch = null;
            mockAddItemManagerValidator.Setup(v => v.Validate(It.IsAny<AddItemManager>()))
                .Returns(ObjectValidationResult.ValidResult);
            mockGetHierarchyClassByNameQuery
                .Setup(m => m.Search(It.Is<GetHierarchyClassByNameParameters>(p =>
                    p.HierarchyName == HierarchyNames.Merchandise && p.HierarchyClassName == "TestMerchName")))
                .Returns(testMearch)
                .Verifiable();

            manager.Item.MerchandiseId = null;
            manager.Item.MerchandiseLineage = "TestMerchName2";
            
            //When
            Exception exception = null;
            try
            {
                managerHandler.Execute(manager);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            //Then
            Assert.AreEqual("Merchandise Hierarchy Class TestMerchName2 does not exist.", exception.Message);
        }
    }
}
