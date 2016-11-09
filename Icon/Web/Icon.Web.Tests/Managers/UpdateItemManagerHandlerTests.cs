using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.Common;
using Icon.Web.Common.Utility;
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

namespace Icon.Web.Tests.Unit.ManagerHandlers
{
    [TestClass]
    public class UpdateItemManagerHandlerTests
    {
        private UpdateItemManagerHandler managerHandler;
        private UpdateItemManager manager;

        private Mock<IObjectValidator<UpdateItemManager>> mockUpdateItemManagerValidator;
        private Mock<ICommandHandler<BulkImportCommand<BulkImportItemModel>>> mockBulkImportItemCommandHandler;
        private Mock<IQueryHandler<GetHierarchyClassByNameParameters, HierarchyClass>> mockGetHierarchyClassByNameQuery;

        [TestInitialize]
        public void Initialize()
        {
            mockUpdateItemManagerValidator = new Mock<IObjectValidator<UpdateItemManager>>();
            mockBulkImportItemCommandHandler = new Mock<ICommandHandler<BulkImportCommand<BulkImportItemModel>>>();
            mockGetHierarchyClassByNameQuery = new Mock<IQueryHandler<GetHierarchyClassByNameParameters, HierarchyClass>>();
            managerHandler = new UpdateItemManagerHandler(mockUpdateItemManagerValidator.Object, mockBulkImportItemCommandHandler.Object, mockGetHierarchyClassByNameQuery.Object);

            manager = new UpdateItemManager
            {
                Item = new BulkImportItemModel
                {
                    ItemId = 45,
                    ScanCode = "111",
                    BrandId = 1.ToString(),
                    BrandName = "Test brand name",
                    BrowsingId = 1.ToString(),
                    DepartmentSale = ConversionUtility.ConvertToItemTraitDbValue(true),
                    FoodStampEligible = ConversionUtility.ConvertToItemTraitDbValue(true),
                    MerchandiseId = 99.ToString(),
                    PackageUnit = "Test Package Unit",
                    PosDescription = "Test PosDescription",
                    PosScaleTare = "Test PosScaleTare",
                    ProductDescription = "Test Product Description",
                    TaxId = 19.ToString()
                },
                UserName = "TestIconUser"                
            };
        }

        [TestMethod]
        public void UpdateItemManager_ValidManager_ShouldCallValidatorAndCommandHandler()
        {
            //Given
            mockUpdateItemManagerValidator.Setup(v => v.Validate(It.IsAny<UpdateItemManager>()))
                .Returns(ObjectValidationResult.ValidResult);

            //When
            managerHandler.Execute(manager);

            //Then
            mockUpdateItemManagerValidator.Verify(v => v.Validate(It.IsAny<UpdateItemManager>()), Times.Once);
            mockBulkImportItemCommandHandler.Verify(v => v.Execute(It.IsAny<BulkImportCommand<BulkImportItemModel>>()), Times.Once);
        }

        [TestMethod]
        public void UpdateItemManager_InvalidManager_ShouldThrowArgumentException()
        {
            // Given.
            mockUpdateItemManagerValidator.Setup(v => v.Validate(It.IsAny<UpdateItemManager>()))
                .Returns(ObjectValidationResult.InvalidResult("Test Exception"));

            // When.
            Exception exception = null;
            try
            {
                managerHandler.Execute(manager);
            }
            catch(Exception ex)
            {
                exception = ex;
            }

            // Then.
            Assert.IsNotNull(exception);
            Assert.AreEqual("Cannot update item.  Error details: Test Exception", exception.Message);
            mockUpdateItemManagerValidator.Verify(v => v.Validate(It.IsAny<UpdateItemManager>()), Times.Once);
            mockBulkImportItemCommandHandler.Verify(v => v.Execute(It.IsAny<BulkImportCommand<BulkImportItemModel>>()), Times.Never);
        }

        [TestMethod]
        public void UpdateItemManager_BulkImportThrowsException_ShouldThrowCommandException()
        {
            //Given
            mockUpdateItemManagerValidator.Setup(v => v.Validate(It.IsAny<UpdateItemManager>()))
                .Returns(ObjectValidationResult.ValidResult);
            mockBulkImportItemCommandHandler.Setup(c => c.Execute(It.IsAny<BulkImportCommand<BulkImportItemModel>>()))
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
            Assert.AreEqual("There was an error while updating Scan Code 111: Test Exception", exception.Message);
            mockUpdateItemManagerValidator.Verify(v => v.Validate(It.IsAny<UpdateItemManager>()), Times.Once);
            mockBulkImportItemCommandHandler.Verify(v => v.Execute(It.IsAny<BulkImportCommand<BulkImportItemModel>>()), Times.Once);
        }
    }
}
