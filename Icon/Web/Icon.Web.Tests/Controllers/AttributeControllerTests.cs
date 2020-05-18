using Icon.Common.DataAccess;
using Icon.Logging;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Managers;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Controllers;
using Icon.Web.Mvc.Models;
using Icon.Web.Mvc.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Web.Mvc;
using Icon.Common.Models;
using Icon.Web.Mvc.Exporters;
using Newtonsoft.Json;
using Icon.Web.Tests.Unit.Models;
using System.Linq;

namespace Icon.Web.Tests.Unit.Controllers
{
    [TestClass]
    public class AttributeControllerTests
    {
        private AttributeController controller;
        private Mock<ILogger> mockLogger;
        private Mock<IQueryHandler<EmptyQueryParameters<IEnumerable<AttributeModel>>, IEnumerable<AttributeModel>>> mockGetAttributesQuery;
        private IconWebAppSettings settings;
        private Mock<ControllerContext> mockControllerContext;
        private Mock<IIdentity> mockIdentity;
        private string userName;
        private Mock<IQueryHandler<GetDataTypeParameters, List<DataTypeModel>>> mockGetDataTypeQueryHandler;
        private Mock<IQueryHandler<GetCharacterSetParameters, List<CharacterSetModel>>> mockGetCharacterSetQueryHandler;
        private Mock<IManagerHandler<AddAttributeManager>> mockAddAttributeManagerHandler;
        private Mock<IManagerHandler<UpdateAttributeManager>> mockUpdateAttributeManagerHandler;
        private Mock<IQueryHandler<GetAttributeByAttributeIdParameters, AttributeModel>> mockGetAttributeByAttributeIdQuery;
        private Mock<IQueryHandler<GetCharacterSetsByAttributeParameters, List<AttributeCharacterSetModel>>> mockgetCharacterSetsByAttributeParameters;
        private Mock<IQueryHandler<GetPickListByAttributeParameters, List<PickListModel>>> mockgetPickListByAttributeParameters;
        private Mock<IAttributesHelper> mockAttributesHelper;
        private Mock<IExcelExporterService> mockExcelExporterService;
        private Mock<IDonutCacheManager> mockCacheManager;
        private Mock<IQueryHandler<EmptyAttributesParameters, IEnumerable<AttributeModel>>> mockGetItemCountOnAttributesQueryHandler;
        private Mock<IQueryHandler<EmptyQueryParameters<List<ItemColumnOrderModel>>, List<ItemColumnOrderModel>>> mockGetItemColumnOrderQueryHandler;

        [TestInitialize]
        public void Initialize()
        {
            this.mockLogger = new Mock<ILogger>();
            this.mockGetAttributesQuery = new Mock<IQueryHandler<EmptyQueryParameters<IEnumerable<AttributeModel>>, IEnumerable<AttributeModel>>>();
            this.settings = new IconWebAppSettings();
            this.mockControllerContext = new Mock<ControllerContext>();
            this.mockIdentity = new Mock<IIdentity>();
            mockGetDataTypeQueryHandler = new Mock<IQueryHandler<GetDataTypeParameters, List<DataTypeModel>>>();
            mockGetCharacterSetQueryHandler = new Mock<IQueryHandler<GetCharacterSetParameters, List<CharacterSetModel>>>();
            mockAddAttributeManagerHandler = new Mock<IManagerHandler<AddAttributeManager>>();
            mockUpdateAttributeManagerHandler = new Mock<IManagerHandler<UpdateAttributeManager>>();
            mockGetAttributeByAttributeIdQuery = new Mock<IQueryHandler<GetAttributeByAttributeIdParameters, AttributeModel>>();
            mockgetCharacterSetsByAttributeParameters = new Mock<IQueryHandler<GetCharacterSetsByAttributeParameters, List<AttributeCharacterSetModel>>>();
            mockgetPickListByAttributeParameters = new Mock<IQueryHandler<GetPickListByAttributeParameters, List<PickListModel>>>();
            mockAttributesHelper = new Mock<IAttributesHelper>();
            mockExcelExporterService = new Mock<IExcelExporterService>();
            mockCacheManager = new Mock<IDonutCacheManager>();
            mockGetItemCountOnAttributesQueryHandler = new Mock<IQueryHandler<EmptyAttributesParameters, IEnumerable<AttributeModel>>>();
            mockGetItemColumnOrderQueryHandler = new Mock<IQueryHandler<EmptyQueryParameters<List<ItemColumnOrderModel>>, List<ItemColumnOrderModel>>>();

            mockGetItemColumnOrderQueryHandler.Setup(M => M.Search(It.IsAny<EmptyQueryParameters<List<ItemColumnOrderModel>>>())).Returns(new List<ItemColumnOrderModel>(){
                { new ItemColumnOrderModel(){ReferenceName = "ItemId", ReferenceNameWithoutSpecialCharacters = "ItemId", ColumnType = "Other" }},
                { new ItemColumnOrderModel(){ReferenceName = "RequestNumber", ReferenceNameWithoutSpecialCharacters = "RequestNumber", ColumnType = "Attribute" }},
                { new ItemColumnOrderModel(){ReferenceName ="BarcodeType", ReferenceNameWithoutSpecialCharacters = "BarcodeType", ColumnType ="Other" }},
                { new ItemColumnOrderModel(){ReferenceName ="Inactive", ReferenceNameWithoutSpecialCharacters = "Inactive", ColumnType ="Attribute" }},
                { new ItemColumnOrderModel(){ReferenceName ="ItemType", ReferenceNameWithoutSpecialCharacters = "ItemType", ColumnType ="Other" }},
                { new ItemColumnOrderModel(){ReferenceName ="ScanCode", ReferenceNameWithoutSpecialCharacters = "ScanCode", ColumnType ="Other" }},
                { new ItemColumnOrderModel(){ReferenceName ="Brand", ReferenceNameWithoutSpecialCharacters = "Brand", ColumnType ="Other" }},
                { new ItemColumnOrderModel(){ReferenceName ="ProductDescription", ReferenceNameWithoutSpecialCharacters = "ProductDescription", ColumnType = "Attribute" }},
                { new ItemColumnOrderModel(){ReferenceName ="POSDescription", ReferenceNameWithoutSpecialCharacters = "POSDescription", ColumnType ="Attribute" }},
                { new ItemColumnOrderModel(){ReferenceName ="CustomerFriendlyDescription", ReferenceNameWithoutSpecialCharacters = "CustomerFriendlyDescription", ColumnType ="Attribute" }},
                { new ItemColumnOrderModel(){ReferenceName ="ItemPack", ReferenceNameWithoutSpecialCharacters = "ItemPack", ColumnType ="Attribute" }},
                { new ItemColumnOrderModel(){ReferenceName ="RetailSize", ReferenceNameWithoutSpecialCharacters = "RetailSize", ColumnType = "Attribute" }},
                { new ItemColumnOrderModel(){ReferenceName ="UOM", ReferenceNameWithoutSpecialCharacters = "UOM", ColumnType ="Attribute" }},
                { new ItemColumnOrderModel(){ReferenceName ="Financial", ReferenceNameWithoutSpecialCharacters = "Financial", ColumnType ="Other" }},
                { new ItemColumnOrderModel(){ReferenceName ="Merchandise", ReferenceNameWithoutSpecialCharacters = "Merchandise", ColumnType ="Other" }},
                { new ItemColumnOrderModel(){ReferenceName ="National", ReferenceNameWithoutSpecialCharacters = "National", ColumnType = "Other" }},
                { new ItemColumnOrderModel(){ReferenceName ="Tax", ReferenceNameWithoutSpecialCharacters = "Tax", ColumnType ="Other" }},
                { new ItemColumnOrderModel(){ReferenceName ="FoodStampEligible", ReferenceNameWithoutSpecialCharacters = "FoodStampEligible", ColumnType ="Attribute" }},
                { new ItemColumnOrderModel(){ReferenceName = "Notes", ReferenceNameWithoutSpecialCharacters = "Notes", ColumnType ="Attribute" }},
                { new ItemColumnOrderModel() { ReferenceName = "DataSource", ReferenceNameWithoutSpecialCharacters = "DataSource", ColumnType = "Attribute" }},
                { new ItemColumnOrderModel() { ReferenceName = "Manufacturer", ReferenceNameWithoutSpecialCharacters = "Manufacturer", ColumnType = "Other" }}
                }
               );

            this.controller = new AttributeController(
                mockLogger.Object,
                settings,
                mockGetAttributesQuery.Object,
                mockGetDataTypeQueryHandler.Object,
                mockGetCharacterSetQueryHandler.Object,
                mockAddAttributeManagerHandler.Object,
                mockGetAttributeByAttributeIdQuery.Object,
                mockUpdateAttributeManagerHandler.Object,
                mockgetCharacterSetsByAttributeParameters.Object,
                mockgetPickListByAttributeParameters.Object,
                mockAttributesHelper.Object,
                mockExcelExporterService.Object,
                mockCacheManager.Object,
                mockGetItemCountOnAttributesQueryHandler.Object,
                mockGetItemColumnOrderQueryHandler.Object);

            this.userName = "Test User";
            this.mockIdentity.SetupGet(i => i.Name).Returns(userName);
            this.mockIdentity.SetupGet(i => i.IsAuthenticated).Returns(true);
            this.mockControllerContext.SetupGet(m => m.HttpContext.User).Returns(new GenericPrincipal(mockIdentity.Object, null));
            this.controller.ControllerContext = mockControllerContext.Object;
            this.mockControllerContext.Setup(c => c.HttpContext.User.IsInRole(It.Is<string>(s => s == userName))).Returns(true);
            // default settings for all tests
            this.settings.WriteAccessGroups = "";
            this.mockControllerContext.SetupGet(c => c.HttpContext.User.Identity.Name).Returns("Test User");

            // setup for get attributes list
            mockGetAttributesQuery.Setup(bq => bq.Search(It.IsAny<EmptyQueryParameters<IEnumerable<AttributeModel>>>())).Returns(new List<AttributeModel>());
        }

        [TestMethod]
        public void Index_InitialPageLoad_ShouldReturnView()
        {
            // When
            var result = this.controller.Index() as ViewResult;

            // Then
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void AttributeControllerIndex_InitialPageLoad_DefaultViewShouldBeReturned()
        {
            // Given
            this.mockGetAttributesQuery.Setup(bq => bq.Search(It.IsAny<EmptyQueryParameters<IEnumerable<AttributeModel>>>())).Returns(new List<AttributeModel>());

            // When
            var result = this.controller.Index() as ViewResult;

            // Then
            Assert.AreEqual(result.ViewName, String.Empty); // This will be empty if the view returned is not specified and returning the controller action
        }


        [TestMethod]
        public void All_Get_ShouldReturnAllAttributes()
        {
            //Given
            this.mockGetAttributesQuery.Setup(m => m.Search(It.IsAny<EmptyQueryParameters<IEnumerable<AttributeModel>>>()))
                .Returns(new List<AttributeModel>()
                {
                    new AttributeModel { AttributeName = "AttributeName", AttributeId=1, Description="Description"}
                });

            //When
            var result = controller.All() as JsonResult;

            string json = JsonConvert.SerializeObject(result.Data);
            AttributeModelWithAllFields model = JsonConvert.DeserializeObject<AttributeModelWithAllFields>(json);

            //Then
            Assert.IsNotNull(result);
            Assert.AreEqual(21, model.OrderOfFields.Count);
            Assert.AreEqual(true, model.OrderOfFields.Any(o=>o.ReferenceNameWithoutSpecialCharacters =="ItemId"));
            Assert.AreEqual(true, model.OrderOfFields.Any(o => o.ReferenceNameWithoutSpecialCharacters == "Notes"));
            Assert.AreEqual(true, model.OrderOfFields.Any(o => o.ReferenceNameWithoutSpecialCharacters == "Tax"));
            Assert.AreEqual(false, model.OrderOfFields.Any(o => o.ReferenceNameWithoutSpecialCharacters == "EStoreEligible"));
            mockGetAttributesQuery.Verify(m => m.Search(It.IsAny<EmptyQueryParameters<IEnumerable<AttributeModel>>>()), Times.Once);
        }

        [TestMethod]
        public void AttributeControllerCreatePost_AttributeIsCreated_DefaultViewShouldBeReturnedWithSuccessMessageAndAttributeCacheIsCleared()
        {
            // Given.
            var viewModel = new AttributeViewModel
            {
                DisplayName = "test",
                DataTypeId = int.MaxValue,
                TraitCode = "Test",
                IsPickList = false,
                AvailableCharacterSets = new List<CharacterSetModel>(),
                SpecialCharactersAllowed = string.Empty
            };

            mockGetDataTypeQueryHandler.Setup(m => m.Search(It.IsAny<GetDataTypeParameters>())).Returns(new List<DataTypeModel>()
                {
                    new DataTypeModel{ DataTypeId = 1, DataType = "Text" }
                });

            // When.
            var result = controller.Create(viewModel) as ViewResult;

            // Then.
            var viewData = result.ViewData;
            string expectedSuccessMessage = $"Created attribute: {viewModel.DisplayName} successfully.";

            Assert.AreEqual(result.ViewName, string.Empty);
            Assert.AreEqual(expectedSuccessMessage, viewData["SuccessMessage"]);
            mockAttributesHelper.Verify(m => m.CreateCharacterSetRegexPattern(It.IsAny<int>(), It.IsAny<List<CharacterSetModel>>(), It.IsAny<string>()));
            mockCacheManager.Verify(x => x.ClearCacheForController(It.Is<string>(a => a == "Attribute")));
        }

        [TestMethod]
        public void AttributeControllerEditPost_AttributeIsEdited_DefaultViewShouldBeReturnedWithSuccessMessageAndAttributeCacheIsCleared()
        {
            // Given.
            var viewModel = new AttributeViewModel
            {
                AttributeId = 1234,
                DisplayName = "test",
                DataTypeId = int.MaxValue,
                TraitCode = "Test",
                IsPickList = false,
                AvailableCharacterSets = new List<CharacterSetModel>(),
                SpecialCharactersAllowed = string.Empty,
            };

            mockGetAttributeByAttributeIdQuery.Setup(x => x.Search(It.IsAny<GetAttributeByAttributeIdParameters>())).Returns(
                new AttributeModel()
                {
                    AttributeId = 1234,
                    DisplayName = "test",
                    DataTypeId = int.MaxValue,
                    TraitCode = "Test",
                    IsPickList = false,
                    SpecialCharactersAllowed = string.Empty
                });

            mockGetDataTypeQueryHandler.Setup(m => m.Search(It.IsAny<GetDataTypeParameters>())).Returns(new List<DataTypeModel>()
                {
                    new DataTypeModel{ DataTypeId = 1, DataType = "Text"}
                });

            // When.
            var result = controller.Edit(viewModel) as ViewResult;

            // Then.
            var viewData = result.ViewData;
            string expectedSuccessMessage = $"Updated attribute: {viewModel.DisplayName} successfully.";

            Assert.AreEqual(result.ViewName, string.Empty);
            Assert.AreEqual(expectedSuccessMessage, viewData["SuccessMessage"]);
            mockAttributesHelper.Verify(m => m.CreateCharacterSetRegexPattern(It.IsAny<int>(), It.IsAny<List<CharacterSetModel>>(), It.IsAny<string>()));
            mockCacheManager.Verify(x => x.ClearCacheForController(It.Is<string>(a => a == "Attribute")));
        }

        [TestMethod]
        public void AttributeControllerEditPost_AttributeSpecialCharsAreEdited_AddedNewChars_Success()
        {
            // Given.
            var viewModel = new AttributeViewModel
            {
                AttributeId = 1234,
                DisplayName = "test",
                DataTypeId = int.MaxValue,
                TraitCode = "Test",
                IsPickList = false,
                AvailableCharacterSets = new List<CharacterSetModel>(),
                IsSpecialCharactersSelected = true,
                SpecialCharacterSetSelected = "Specific",
                SpecialCharactersAllowed = "!@#$",
            };

            mockGetAttributeByAttributeIdQuery.Setup(x => x.Search(It.IsAny<GetAttributeByAttributeIdParameters>())).Returns(
                new AttributeModel()
                {
                    AttributeId = 1234,
                    DisplayName = "test",
                    DataTypeId = int.MaxValue,
                    TraitCode = "Test",
                    IsPickList = false,
                    SpecialCharactersAllowed = "!@#"
                });

            mockGetDataTypeQueryHandler.Setup(m => m.Search(It.IsAny<GetDataTypeParameters>())).Returns(new List<DataTypeModel>()
                {
                    new DataTypeModel{ DataTypeId = 1, DataType = "Text"}
                });

            // When.
            var result = controller.Edit(viewModel) as ViewResult;

            // Then.
            var viewData = result.ViewData;
            string expectedSuccessMessage = $"Updated attribute: {viewModel.DisplayName} successfully.";

            Assert.AreEqual(result.ViewName, string.Empty);
            Assert.AreEqual(expectedSuccessMessage, viewData["SuccessMessage"]);
            mockAttributesHelper.Verify(m => m.CreateCharacterSetRegexPattern(It.IsAny<int>(), It.IsAny<List<CharacterSetModel>>(), It.IsAny<string>()));
        }

        [TestMethod]
        public void AttributeControllerEditGet_BooleanAttribute_ModelShouldHaveAvailableDefaultValuesForBoolean()
        {
            mockGetAttributeByAttributeIdQuery.Setup(x => x.Search(It.IsAny<GetAttributeByAttributeIdParameters>())).Returns(
               new AttributeModel()
               {
                   AttributeId = 1234,
                   DisplayName = "test",
                   DataTypeId = (int)DataType.Boolean,
                   TraitCode = "Test",
                   IsPickList = false,
                   SpecialCharactersAllowed = "!@#"
               });

            mockGetDataTypeQueryHandler.Setup(m => m.Search(It.IsAny<GetDataTypeParameters>())).Returns(new List<DataTypeModel>()
                {
                    new DataTypeModel{ DataTypeId = 1, DataType = "Boolean"}
                });

            // When.
            var result = controller.Edit(1234) as ViewResult;

            Assert.IsNotNull(((AttributeViewModel)result.Model).AvailableDefaultValuesForBoolean);
        }

        [TestMethod]
        public void AttributeControllerEditPost_AttributeSpecialCharsAreEdited_SameCharsRearranged_Success()
        {
            // Given.
            var viewModel = new AttributeViewModel
            {
                AttributeId = 1234,
                DisplayName = "test",
                DataTypeId = int.MaxValue,
                TraitCode = "Test",
                IsPickList = false,
                AvailableCharacterSets = new List<CharacterSetModel>(),
                IsSpecialCharactersSelected = true,
                SpecialCharacterSetSelected = "Specific",
                SpecialCharactersAllowed = "#@!",
            };

            mockGetAttributeByAttributeIdQuery.Setup(x => x.Search(It.IsAny<GetAttributeByAttributeIdParameters>())).Returns(
                new AttributeModel()
                {
                    AttributeId = 1234,
                    DisplayName = "test",
                    DataTypeId = int.MaxValue,
                    TraitCode = "Test",
                    IsPickList = false,
                    SpecialCharactersAllowed = "!@#"
                });

            mockGetDataTypeQueryHandler.Setup(m => m.Search(It.IsAny<GetDataTypeParameters>())).Returns(new List<DataTypeModel>()
                {
                    new DataTypeModel{ DataTypeId = 1, DataType = "Text"}
                });

            // When.
            var result = controller.Edit(viewModel) as ViewResult;

            // Then.
            var viewData = result.ViewData;
            string expectedSuccessMessage = $"Updated attribute: {viewModel.DisplayName} successfully.";

            Assert.AreEqual(result.ViewName, string.Empty);
            Assert.AreEqual(expectedSuccessMessage, viewData["SuccessMessage"]);
            mockAttributesHelper.Verify(m => m.CreateCharacterSetRegexPattern(It.IsAny<int>(), It.IsAny<List<CharacterSetModel>>(), It.IsAny<string>()));
        }

        [TestMethod]
        public void AttributeControllerEditPost_AttributeSpecialCharsAreEdited_AddedNewCharsWithOriginals_Success()
        {
            // Given.
            var viewModel = new AttributeViewModel
            {
                AttributeId = 1234,
                DisplayName = "test",
                DataTypeId = int.MaxValue,
                TraitCode = "Test",
                IsPickList = false,
                AvailableCharacterSets = new List<CharacterSetModel>(),
                IsSpecialCharactersSelected = true,
                SpecialCharacterSetSelected = "Specific",
                SpecialCharactersAllowed = "#@!$",
            };

            mockGetAttributeByAttributeIdQuery.Setup(x => x.Search(It.IsAny<GetAttributeByAttributeIdParameters>())).Returns(
                new AttributeModel()
                {
                    AttributeId = 1234,
                    DisplayName = "test",
                    DataTypeId = int.MaxValue,
                    TraitCode = "Test",
                    IsPickList = false,
                    SpecialCharactersAllowed = "!@#"
                });

            mockGetDataTypeQueryHandler.Setup(m => m.Search(It.IsAny<GetDataTypeParameters>())).Returns(new List<DataTypeModel>()
                {
                    new DataTypeModel{ DataTypeId = 1, DataType = "Text"}
                });

            // When.
            var result = controller.Edit(viewModel) as ViewResult;

            // Then.
            var viewData = result.ViewData;
            string expectedSuccessMessage = $"Updated attribute: {viewModel.DisplayName} successfully.";

            Assert.AreEqual(result.ViewName, string.Empty);
            Assert.AreEqual(expectedSuccessMessage, viewData["SuccessMessage"]);
            mockAttributesHelper.Verify(m => m.CreateCharacterSetRegexPattern(It.IsAny<int>(), It.IsAny<List<CharacterSetModel>>(), It.IsAny<string>()));
        }

        [TestMethod]
        public void AttributeControllerEditPost_AttributeSpecialCharsAreEdited_RemovedExistingChars_Failure()
        {
            // Given.
            var viewModel = new AttributeViewModel
            {
                AttributeId = 1234,
                DisplayName = "test",
                DataTypeId = int.MaxValue,
                TraitCode = "Test",
                IsPickList = false,
                AvailableCharacterSets = new List<CharacterSetModel>(),
                IsSpecialCharactersSelected = true,
                SpecialCharacterSetSelected = "Specific",
                SpecialCharactersAllowed = "!@"
            };

            mockGetAttributeByAttributeIdQuery.Setup(x => x.Search(It.IsAny<GetAttributeByAttributeIdParameters>())).Returns(
                new AttributeModel()
                {
                    AttributeId = 1234,
                    DisplayName = "test",
                    DataTypeId = int.MaxValue,
                    TraitCode = "Test",
                    IsPickList = false,
                    SpecialCharactersAllowed = "!@#"
                });

            mockGetDataTypeQueryHandler.Setup(m => m.Search(It.IsAny<GetDataTypeParameters>())).Returns(new List<DataTypeModel>()
                {
                    new DataTypeModel{ DataTypeId = 1, DataType = "Text"}
                });

            // When.
            var result = controller.Edit(viewModel) as ViewResult;

            // Then.
            var viewData = result.ViewData;
            string expectedSuccessMessage = $"Updated attribute: {viewModel.DisplayName} successfully.";

            Assert.AreEqual(result.ViewName, string.Empty);
            Assert.AreEqual("Cannot remove preexisting special characters", (viewData["ErrorMessages"] as List<string>)[0]);
            mockAttributesHelper.Verify(m => m.CreateCharacterSetRegexPattern(It.IsAny<int>(), It.IsAny<List<CharacterSetModel>>(), It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void AttributeControllerEditPost_AttributeSpecialCharsAreEdited_RemovedExistingCharsWithAddedChars_Failure()
        {
            // Given.
            var viewModel = new AttributeViewModel
            {
                AttributeId = 1234,
                DisplayName = "test",
                DataTypeId = int.MaxValue,
                TraitCode = "Test",
                IsPickList = false,
                AvailableCharacterSets = new List<CharacterSetModel>(),
                IsSpecialCharactersSelected = true,
                SpecialCharacterSetSelected = "Specific",
                SpecialCharactersAllowed = "!@$"
            };

            mockGetAttributeByAttributeIdQuery.Setup(x => x.Search(It.IsAny<GetAttributeByAttributeIdParameters>())).Returns(
                new AttributeModel()
                {
                    AttributeId = 1234,
                    DisplayName = "test",
                    DataTypeId = int.MaxValue,
                    TraitCode = "Test",
                    IsPickList = false,
                    SpecialCharactersAllowed = "!@#"
                });

            mockGetDataTypeQueryHandler.Setup(m => m.Search(It.IsAny<GetDataTypeParameters>())).Returns(new List<DataTypeModel>()
                {
                    new DataTypeModel{ DataTypeId = 1, DataType = "Text"}
                });

            // When.
            var result = controller.Edit(viewModel) as ViewResult;

            // Then.
            var viewData = result.ViewData;
            string expectedSuccessMessage = $"Updated attribute: {viewModel.DisplayName} successfully.";

            Assert.AreEqual(result.ViewName, string.Empty);
            Assert.AreEqual("Cannot remove preexisting special characters", (viewData["ErrorMessages"] as List<string>)[0]);
            mockAttributesHelper.Verify(m => m.CreateCharacterSetRegexPattern(It.IsAny<int>(), It.IsAny<List<CharacterSetModel>>(), It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void AttributeControllerEditPost_AttributeSpecialCharsAreEdited_RemovedExistingCharsWithAddedCharsRearranged_Failure()
        {
            // Given.
            var viewModel = new AttributeViewModel
            {
                AttributeId = 1234,
                DisplayName = "test",
                DataTypeId = int.MaxValue,
                TraitCode = "Test",
                IsPickList = false,
                AvailableCharacterSets = new List<CharacterSetModel>(),
                IsSpecialCharactersSelected = true,
                SpecialCharacterSetSelected = "Specific",
                SpecialCharactersAllowed = "$%^&!@"
            };

            mockGetAttributeByAttributeIdQuery.Setup(x => x.Search(It.IsAny<GetAttributeByAttributeIdParameters>())).Returns(
                new AttributeModel()
                {
                    AttributeId = 1234,
                    DisplayName = "test",
                    DataTypeId = int.MaxValue,
                    TraitCode = "Test",
                    IsPickList = false,
                    SpecialCharactersAllowed = "!@#"
                });

            mockGetDataTypeQueryHandler.Setup(m => m.Search(It.IsAny<GetDataTypeParameters>())).Returns(new List<DataTypeModel>()
                {
                    new DataTypeModel{ DataTypeId = 1, DataType = "Text"}
                });

            // When.
            var result = controller.Edit(viewModel) as ViewResult;

            // Then.
            var viewData = result.ViewData;
            string expectedSuccessMessage = $"Updated attribute: {viewModel.DisplayName} successfully.";

            Assert.AreEqual(result.ViewName, string.Empty);
            Assert.AreEqual("Cannot remove preexisting special characters", (viewData["ErrorMessages"] as List<string>)[0]);
            mockAttributesHelper.Verify(m => m.CreateCharacterSetRegexPattern(It.IsAny<int>(), It.IsAny<List<CharacterSetModel>>(), It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void AttributeControllerEditPost_AttributeStatusIsEdited_UpdateToHideAttribute_Success()
        {
            // Given.
            var viewModel = new AttributeViewModel
            {
                AttributeId = 1234,
                DisplayName = "test",
                DataTypeId = int.MaxValue,
                TraitCode = "Test",
                IsPickList = false,
                AvailableCharacterSets = new List<CharacterSetModel>(),
                IsSpecialCharactersSelected = true,
                SpecialCharacterSetSelected = "Specific",
                SpecialCharactersAllowed = "#@!$",
                ItemCount = 0,
                IsActive = true
            };

            mockGetAttributeByAttributeIdQuery.Setup(x => x.Search(It.IsAny<GetAttributeByAttributeIdParameters>())).Returns(
                new AttributeModel()
                {
                    AttributeId = 1234,
                    DisplayName = "test",
                    DataTypeId = int.MaxValue,
                    TraitCode = "Test",
                    IsPickList = false,
                    SpecialCharactersAllowed = "#@!$",
                    IsActive = false
                });

            mockGetDataTypeQueryHandler.Setup(m => m.Search(It.IsAny<GetDataTypeParameters>())).Returns(new List<DataTypeModel>()
                {
                    new DataTypeModel{ DataTypeId = 1, DataType = "Text"}
                });
            // When.
            var result = controller.Edit(viewModel) as ViewResult;

            // Then.
            var viewData = result.ViewData;
            string expectedSuccessMessage = $"Updated attribute: {viewModel.DisplayName} successfully.";

            Assert.AreEqual(result.ViewName, string.Empty);
            Assert.AreEqual(expectedSuccessMessage, viewData["SuccessMessage"]);
        }


        [TestMethod]
        public void AttributeControllerEditPost_AttributeModifiedByIsEdited_ModifiedByRetrieved_Success()
        {
            // Given.
            var viewModel = new AttributeViewModel
            {
                AttributeId = 1234,
                DisplayName = "test",
                DataTypeId = int.MaxValue,
                TraitCode = "Test",
                IsPickList = false,
                AvailableCharacterSets = new List<CharacterSetModel>(),
                IsSpecialCharactersSelected = true,
                SpecialCharacterSetSelected = "Specific",
                SpecialCharactersAllowed = "#@!$",
                ItemCount = 0,
                IsActive = true,
                LastModifiedBy = this.userName
            };

            mockGetAttributeByAttributeIdQuery.Setup(x => x.Search(It.IsAny<GetAttributeByAttributeIdParameters>())).Returns(
                new AttributeModel()
                {
                    AttributeId = 1234,
                    DisplayName = "test",
                    DataTypeId = int.MaxValue,
                    TraitCode = "Test",
                    IsPickList = false,
                    SpecialCharactersAllowed = "#@!$",
                    IsActive = true,
                    LastModifiedBy = "UserNameTest"
                });

            mockGetDataTypeQueryHandler.Setup(m => m.Search(It.IsAny<GetDataTypeParameters>())).Returns(new List<DataTypeModel>()
                {
                    new DataTypeModel{ DataTypeId = 1, DataType = "Text"}
                });
            // When.
            var result = controller.Edit(viewModel) as ViewResult;

            // Then.
            var viewData = result.ViewData;
            var resultModel = result.Model as AttributeViewModel;             

            Assert.AreEqual(result.ViewName, string.Empty);
            Assert.AreEqual("UserNameTest", resultModel.LastModifiedBy);
            
        }

    }
}