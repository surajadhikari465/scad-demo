//using System;
//using System.Collections.Generic;
//using System.Linq;
//using BulkItemUploadProcessor.Common;
//using BulkItemUploadProcessor.Common.Models;
//using BulkItemUploadProcessor.DataAccess.Queries;
//using Icon.Common.DataAccess;
//using Icon.Common.Models;
//using Icon.Common.Validators.ItemAttributes;
//using Microsoft.SqlServer.Server;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Moq;
//using OfficeOpenXml;

//namespace BulkItemUploadProcessor.Service.Tests
//{
//    [TestClass]
//    public class ValidationManagerTests
//    {

//        private Mock<ItemAttributesValidatorFactory> MockIItemAttributesValidatorFactory;
//        private Mock<IQueryHandler<DoesScanCodeExistParameters, bool>> MockDoesScanCodeExistQueryHandler;

//        private Mock<IQueryHandler<EmptyQueryParameters<IEnumerable<AttributeModel>>, IEnumerable<AttributeModel>>>
//            MockGetAttributesQueryHandler;

//        private Mock<IQueryHandler<EmptyQueryParameters<List<BarcodeTypeModel>>, List<BarcodeTypeModel>>>
//            MockGetBarcodeTypeQueryHandler;

//        private Mock<IQueryHandler<GetItemParameters, ItemDbModel>> MockGetItemQueryHandler;


//        private Mock<IHierarchyCache> MockHierarchyCache;
//        private Mock<IMerchItemPropertiesCache> MockMerchItemPropertiesCache;

//        private IValidationManager ValidationManager;
//        private Mock<IValidationManager> MockValidationManager;

//        [TestInitialize]
//        public void TestInitialize()
//        {
//           MockGetAttributesQueryHandler = new Mock<IQueryHandler<EmptyQueryParameters<IEnumerable<AttributeModel>>, IEnumerable<AttributeModel>>>();
//            MockIItemAttributesValidatorFactory = new Mock<ItemAttributesValidatorFactory>(MockGetAttributesQueryHandler.Object);

//            MockDoesScanCodeExistQueryHandler = new Mock<IQueryHandler<DoesScanCodeExistParameters, bool>>();
//            MockGetBarcodeTypeQueryHandler = new Mock<IQueryHandler<EmptyQueryParameters<List<BarcodeTypeModel>>, List<BarcodeTypeModel>>>();
//            MockGetItemQueryHandler = new Mock<IQueryHandler<GetItemParameters, ItemDbModel>>();

//            MockHierarchyCache = new Mock<IHierarchyCache>();
//            MockMerchItemPropertiesCache = new Mock<IMerchItemPropertiesCache>();
//        MockValidationManager = new Mock<IValidationManager>();

//            ValidationManager = new ValidationManager(MockIItemAttributesValidatorFactory.Object, 
//                MockDoesScanCodeExistQueryHandler.Object, 
//                MockGetBarcodeTypeQueryHandler.Object,
//                MockGetItemQueryHandler.Object,
//                MockHierarchyCache.Object,MockMerchItemPropertiesCache.Object);
//        }

//        [TestMethod]
//        public void ParseEditItemViewModelsFromRowData_NoRows_ReturnEmptyList()
//        {
//            ValidationManager.RowData = new List<RowObject>();
//            var resutls = ValidationManager.ParseEditItemViewModelsFromRowData(new List<AttributeModel>());

//            Assert.IsNotNull(resutls);
//            Assert.IsFalse(resutls.Any());
//        }
//        [TestMethod]
//        public void ParseEditItemViewModelsFromRowData_NoRowDataObject_ReturnEmptyList()
//        {
//            ValidationManager.RowData = null;
//            var resutls = ValidationManager.ParseEditItemViewModelsFromRowData(new List<AttributeModel>());

//            Assert.IsNotNull(resutls);
//            Assert.IsFalse(resutls.Any());
//        }

//        [TestMethod]
//        public void ParseEditItemViewModelsFromRowData_RowsExist_ExcludeReadonlyAttributes()
//        {

//            var rowdata = new List<RowObject>();
//            var row = new RowObject
//            {
//                Cells = new List<ParsedCell>
//                {
//                    new ParsedCell {Column = new ColumnHeader {Address = "a1", ColumnIndex = 1, Name = "Test"}, CellValue = "Test"},
//                    new ParsedCell {Column = new ColumnHeader {Address = "b1", ColumnIndex = 2, Name = "Test2"}, CellValue = "Test2"}
//                }
//            };
//            rowdata.Add(row);


//            var attributes = new List<AttributeModel>
//            {
//                new AttributeModel {AttributeName = "Test", IsReadOnly = true},
//                new AttributeModel {AttributeName = "Test2", IsReadOnly = false}
//            };

//            ValidationManager.RowData = rowdata;
//            var results = ValidationManager.ParseEditItemViewModelsFromRowData(attributes).ToList();
//            var firstresult = results.FirstOrDefault();

//            Assert.IsNotNull(results);
//            Assert.IsNotNull(firstresult);
//            Assert.IsTrue(firstresult.ValuesToUpdate.ContainsKey("Test2"));
//            Assert.IsFalse(firstresult.ValuesToUpdate.ContainsKey("Test"));
//        }

//        [TestMethod]
//        public void ParseEditItemViewModelsFromRowData_RowsExist_ExcludeRequiredWithRemove()
//        {

//            var rowdata = new List<RowObject>();
//            var row = new RowObject
//            {
//                Cells = new List<ParsedCell>
//                {
//                    new ParsedCell {Column = new ColumnHeader {Address = "a1", ColumnIndex = 1, Name = "Test"}, CellValue = "remove"},
//                    new ParsedCell {Column = new ColumnHeader {Address = "b1", ColumnIndex = 2, Name = "Test2"}, CellValue = "Test2"}
//                }
//            };
//            rowdata.Add(row);


//            var attributes = new List<AttributeModel>
//            {
//                new AttributeModel {AttributeName = "Test", IsRequired = true},
//                new AttributeModel {AttributeName = "Test2", IsReadOnly = false}
//            };

//            ValidationManager.RowData = rowdata;
//            var results = ValidationManager.ParseEditItemViewModelsFromRowData(attributes).ToList();
//            var firstresult = results.FirstOrDefault();

//            Assert.IsNotNull(results);
//            Assert.IsNotNull(firstresult);
//            Assert.IsTrue(firstresult.ValuesToUpdate.ContainsKey("Test2"));
//            Assert.IsFalse(firstresult.ValuesToUpdate.ContainsKey("Test"));
//        }


//        [TestMethod]
//        public void ParseEditItemViewModelsFromRowData_RowsExist_ExcludeEmptyValues()
//        {

//            var rowdata = new List<RowObject>();
//            var row = new RowObject
//            {
//                Cells = new List<ParsedCell>
//                {
//                    new ParsedCell {Column = new ColumnHeader {Address = "a1", ColumnIndex = 1, Name = "Test"}, CellValue = ""},
//                    new ParsedCell {Column = new ColumnHeader {Address = "b1", ColumnIndex = 2, Name = "Test2"}, CellValue = "Test2"}
//                }
//            };
//            rowdata.Add(row);


//            var attributes = new List<AttributeModel>
//            {
//                new AttributeModel {AttributeName = "Test", IsReadOnly = false, IsRequired = false},
//                new AttributeModel {AttributeName = "Test2", IsReadOnly = false, IsRequired = false}
//            };

//            ValidationManager.RowData = rowdata;
//            var results = ValidationManager.ParseEditItemViewModelsFromRowData(attributes).ToList();
//            var firstresult = results.FirstOrDefault();

//            Assert.IsNotNull(results);
//            Assert.IsNotNull(firstresult);
//            Assert.IsTrue(firstresult.ValuesToUpdate.ContainsKey("Test2"));
//            Assert.IsFalse(firstresult.ValuesToUpdate.ContainsKey("Test"));
//        }


//        [TestMethod]
//        public void ValidationManager_IsUpcInBarcodeRanges_UpcInRange_ReturnsTrue()
//        {

//            var barcodeRanges = new List<BarcodeTypeModel>();
//            var barcodeTypemodel = new BarcodeTypeModel {BarcodeType = "test", BarcodeTypeId = 1, BeginRange = "1234", EndRange = "1236"};
//            barcodeRanges.Add(barcodeTypemodel);


//            var result = ValidationManager.IsUpcInBarcodeTypeRanges("1235", barcodeRanges);

//            Assert.IsTrue(result, "Upc Not In Range");

//        }
//        [TestMethod]
//        public void ValidationManager_IsUpcInBarcodeRanges_UpcNoInRange_ReturnsFalse()
//        {

//            var barcodeRanges = new List<BarcodeTypeModel>();
//            var barcodeTypemodel = new BarcodeTypeModel { BarcodeType = "test", BarcodeTypeId = 1, BeginRange = "1234", EndRange = "1236" };
//            barcodeRanges.Add(barcodeTypemodel);


//            var result = ValidationManager.IsUpcInBarcodeTypeRanges("12356", barcodeRanges);

//            Assert.IsFalse(result, "Upc In Range");
//        }

//        [TestMethod]
//        public void ValidateSingleItemAdd_InvalidBarcodeId_ReturnsError()
//        {
//            var item = new NewItemViewModel();
//            item.BarcodeTypeId = "a";

//            ValidationManager.ValidateSingleItemAdd(item, new List<BarcodeTypeModel>());

//            Assert.IsTrue(item.Errors.Contains("Barcode Type Id is required. "));

//        }

//        [TestMethod]
//        public void ValidateSingleItemAdd_EmptyBarcodeId_ReturnsError()
//        {
//            var item = new NewItemViewModel();
//            item.BarcodeTypeId = "";

//            ValidationManager.ValidateSingleItemAdd(item, new List<BarcodeTypeModel>());

//            Assert.IsTrue(item.Errors.Contains("Barcode Type Id is required. "));

//        }

//        [TestMethod]
//        public void ValidateSingleItemAdd_EmptyBarcodeIdValidBarcodeType_LooksupBarcodeTypeInfo()
//        {

//            var barcodeTypes = new List<BarcodeTypeModel>
//            {
//                new BarcodeTypeModel {BarcodeType = "PLU", BarcodeTypeId = 1}
//            };
//            var item = new NewItemViewModel {BarcodeTypeId = "", BarcodeTypeName = "PLU"};

//            ValidationManager.ValidateSingleItemAdd(item, barcodeTypes);

//            Assert.AreEqual("1", item.BarcodeTypeId);
//        }
//        [TestMethod]
//        public void ValidateSingleItemAdd_EmptyBarcodeIdInvalidBarcodeType_LooksupBarcodeReturnError()
//        {

//            var barcodeTypes = new List<BarcodeTypeModel>
//            {
//                new BarcodeTypeModel {BarcodeType = "PLU", BarcodeTypeId = 1}
//            };
//            var item = new NewItemViewModel {BarcodeTypeId = "", BarcodeTypeName = "Unknown"};

//            ValidationManager.ValidateSingleItemAdd(item, barcodeTypes);

//            Assert.AreEqual("", item.BarcodeTypeId);
//            Assert.IsTrue(item.Errors.Contains($"Barcode Type Id is required. {item.BarcodeTypeName}"));

//        }

//        [TestMethod]
//        public void ValidateSingleItemAdd_InvalidHiearchyId_ReturnsError()
//        {

//            var barcodeTypes = new List<BarcodeTypeModel>
//            {
//                new BarcodeTypeModel {BarcodeType = "PLU", BarcodeTypeId = 1}
//            };
//            var item = new NewItemViewModel
//            {
//                BarcodeTypeId = "1", BarcodeTypeName = "UPC", BarcodeType = "UPC", BrandHierarchyClassId = ""
//            };

//            ValidationManager.ValidateSingleItemAdd(item, barcodeTypes);
//            Assert.IsTrue(item.Errors.Contains($"Brand Hierarchy Id is required."));

//        }

//        [TestMethod]
//        public void ValidateSingleItemAdd_HasHiearchyIdButHierarchyDoesntExist_ReturnsError()
//        {

//            MockHierarchyCache.Setup(s => s.IsValidBrandHierarchyClassId(It.IsAny<int>()))
//                .Returns(false);

//            var barcodeTypes = new List<BarcodeTypeModel>
//            {
//                new BarcodeTypeModel {BarcodeType = "PLU", BarcodeTypeId = 1}
//            };
//            var item = new NewItemViewModel
//            {
//                BarcodeTypeId = "1",
//                BarcodeTypeName = "UPC",
//                BarcodeType = "UPC",
//                BrandHierarchyClassId = "1"
//            };

//            ValidationManager.ValidateSingleItemAdd(item, barcodeTypes);
//            Assert.IsTrue(item.Errors.Contains($"[{item.BrandHierarchyClassId}] is not a valid Brand HierarchyClassId"));

//        }

//        [TestMethod]
//        public void ValidateSingleItemAdd_InvalidTaxId_ReturnsError()
//        {

//            var barcodeTypes = new List<BarcodeTypeModel>
//            {
//                new BarcodeTypeModel {BarcodeType = "PLU", BarcodeTypeId = 1}
//            };
//            var item = new NewItemViewModel
//            {
//                BarcodeTypeId = "1",
//                BarcodeTypeName = "UPC",
//                BarcodeType = "UPC",
//                TaxHierarchyClassId = ""
//            };

//            ValidationManager.ValidateSingleItemAdd(item, barcodeTypes);
//            Assert.IsTrue(item.Errors.Contains($"Tax Hierarchy Id is required."));

//        }

//        [TestMethod]
//        public void ValidateSingleItemAdd_HasTaxIdButHierarchyDoesntExist_ReturnsError()
//        {

//            MockHierarchyCache.Setup(s => s.IsValidTaxHierarchyClassId(It.IsAny<int>()))
//                .Returns(false);

//            var barcodeTypes = new List<BarcodeTypeModel>
//            {
//                new BarcodeTypeModel {BarcodeType = "PLU", BarcodeTypeId = 1}
//            };
//            var item = new NewItemViewModel
//            {
//                BarcodeTypeId = "1",
//                BarcodeTypeName = "UPC",
//                BarcodeType = "UPC",
//                TaxHierarchyClassId = "1"
//            };

//            ValidationManager.ValidateSingleItemAdd(item, barcodeTypes);
//            Assert.IsFalse(item.Errors.Contains($"Tax Hierarchy Id is required."));
//            Assert.IsTrue(item.Errors.Contains($"[{item.TaxHierarchyClassId}] is not a valid Tax HierarchyClassId"));

//        }

//        [TestMethod]
//        public void ValidateSingleItemAdd_InvalidMerchId_ReturnsError()
//        {

//            var barcodeTypes = new List<BarcodeTypeModel>
//            {
//                new BarcodeTypeModel {BarcodeType = "PLU", BarcodeTypeId = 1}
//            };
//            var item = new NewItemViewModel
//            {
//                BarcodeTypeId = "1",
//                BarcodeTypeName = "UPC",
//                BarcodeType = "UPC",
//                MerchandiseHierarchyClassId = ""
//            };

//            ValidationManager.ValidateSingleItemAdd(item, barcodeTypes);
//            Assert.IsTrue(item.Errors.Contains($"Merchandise Hierarchy Id is required."));

//        }

//        [TestMethod]
//        public void ValidateSingleItemAdd_HasMerchIdButHierarchyDoesntExist_ReturnsError()
//        {

//            MockHierarchyCache.Setup(s => s.IsValidMerchandiseHierarchyClassId(It.IsAny<int>()))
//                .Returns(false);

//            var barcodeTypes = new List<BarcodeTypeModel>
//            {
//                new BarcodeTypeModel {BarcodeType = "PLU", BarcodeTypeId = 1}
//            };
//            var item = new NewItemViewModel
//            {
//                BarcodeTypeId = "1",
//                BarcodeTypeName = "UPC",
//                BarcodeType = "UPC",
//                MerchandiseHierarchyClassId = "1"
//            };

//            ValidationManager.ValidateSingleItemAdd(item, barcodeTypes);
//            Assert.IsFalse(item.Errors.Contains($"Merchandise Hierarchy Id is required."));
//            Assert.IsTrue(item.Errors.Contains($"[{item.MerchandiseHierarchyClassId}] is not a valid Merch HierarchyClassId"));

//        }

//        [TestMethod]
//        public void ValidateSingleItemAdd_InvalidNationalClassId_ReturnsError()
//        {

//            var barcodeTypes = new List<BarcodeTypeModel>
//            {
//                new BarcodeTypeModel {BarcodeType = "PLU", BarcodeTypeId = 1}
//            };
//            var item = new NewItemViewModel
//            {
//                BarcodeTypeId = "1",
//                BarcodeTypeName = "UPC",
//                BarcodeType = "UPC",
//                NationalHierarchyClassId = ""
//            };

//            ValidationManager.ValidateSingleItemAdd(item, barcodeTypes);
//            Assert.IsTrue(item.Errors.Contains($"National Class Hierarchy Id is required."));

//        }

//        [TestMethod]
//        public void ValidateSingleItemAdd_HasNationalClassIdButHierarchyDoesntExist_ReturnsError()
//        {

//            MockHierarchyCache.Setup(s => s.IsValidTaxHierarchyClassId(It.IsAny<int>()))
//                .Returns(false);

//            var barcodeTypes = new List<BarcodeTypeModel>
//            {
//                new BarcodeTypeModel {BarcodeType = "PLU", BarcodeTypeId = 1}
//            };
//            var item = new NewItemViewModel
//            {
//                BarcodeTypeId = "1",
//                BarcodeTypeName = "UPC",
//                BarcodeType = "UPC",
//                NationalHierarchyClassId = "1"
//            };

//            ValidationManager.ValidateSingleItemAdd(item, barcodeTypes);
//            Assert.IsFalse(item.Errors.Contains($"National Class Hierarchy Id is required."));
//            Assert.IsTrue(item.Errors.Contains($"[{item.NationalHierarchyClassId}] is not a valid National HierarchyClassId"));

//        }

//        [TestMethod]
//        public void ValidateSingleItemAdd_TypeUPC_NullScanCode_ReturnsError()
//        {

//            MockHierarchyCache.Setup(s => s.IsValidTaxHierarchyClassId(It.IsAny<int>()))
//                .Returns(true);

//            MockHierarchyCache.Setup(s => s.IsValidMerchandiseHierarchyClassId(It.IsAny<int>()))
//                .Returns(true);

//            MockHierarchyCache.Setup(s => s.IsValidBrandHierarchyClassId(It.IsAny<int>()))
//                .Returns(true);

//            MockHierarchyCache.Setup(s => s.IsValidNationalHierarchyClassId(It.IsAny<int>()))
//                .Returns(true);


//            var barcodeTypes = new List<BarcodeTypeModel>
//            {
//                new BarcodeTypeModel {BarcodeType = "PLU", BarcodeTypeId = 1}
//            };
//            var item = new NewItemViewModel
//            {
//                BarcodeTypeId = "1",
//                BarcodeTypeName = "UPC",
//                BarcodeType = "UPC",
//                NationalHierarchyClassId = "1", 
//                MerchandiseHierarchyClassId = "1", 
//                TaxHierarchyClassId = "1", 
//                BrandHierarchyClassId =  "1",
//            };

//            ValidationManager.ValidateSingleItemAdd(item, barcodeTypes);
//            Assert.IsTrue(item.Errors.Contains($"Scan Code is required when choosing UPC"));

//        }

//        [TestMethod]
//        public void ValidateSingleItemAdd_TypeUPC_ValidScanCodeButAlreadyExists_ReturnsError()
//        {

//            MockHierarchyCache.Setup(s => s.IsValidTaxHierarchyClassId(It.IsAny<int>()))
//                .Returns(true);

//            MockHierarchyCache.Setup(s => s.IsValidMerchandiseHierarchyClassId(It.IsAny<int>()))
//                .Returns(true);

//            MockHierarchyCache.Setup(s => s.IsValidBrandHierarchyClassId(It.IsAny<int>()))
//                .Returns(true);

//            MockHierarchyCache.Setup(s => s.IsValidNationalHierarchyClassId(It.IsAny<int>()))
//                .Returns(true);

//            MockDoesScanCodeExistQueryHandler.Setup(s => s.Search(It.IsAny<DoesScanCodeExistParameters>())).Returns(true);

//            var barcodeTypes = new List<BarcodeTypeModel>
//            {
//                new BarcodeTypeModel {BarcodeType = "PLU", BarcodeTypeId = 1}
//            };
//            var item = new NewItemViewModel
//            {
//                BarcodeTypeId = "1",
//                BarcodeTypeName = "UPC",
//                BarcodeType = "UPC",
//                NationalHierarchyClassId = "1",
//                MerchandiseHierarchyClassId = "1",
//                TaxHierarchyClassId = "1",
//                BrandHierarchyClassId = "1",
//                ScanCode = "1234"
//            };

//            ValidationManager.ValidateSingleItemAdd(item, barcodeTypes);
//            Assert.IsTrue(item.Errors.Contains($"'{item.ScanCode}' is already associated to an item. Please use another scan code"));

//        }

//        [TestMethod]
//        public void OverlayExistingItemAttributes_ResultShouldIncludeAttributesFromExistingAndUpdatedItem()
//        {
//            var valuesToUpdate = new Dictionary<string, string> {{"Test", "TestA"}};

//            var uploadedItem = new EditItemViewModel(1, valuesToUpdate);

//            var existingItem = new ItemDbModel();
//            existingItem.ItemAttributesJson = "{\"CreatedBy\":\"ICON\",\"CreatedDateTimeUtc\":\"2014-06-06T13:25:38.22Z\",\"CustomerFriendlyDescription\":\"CRV Soda 15Pk Under 24Oz\",\"FoodStampEligible\":\"true\"}";

//            var result = ValidationManager.OverlayExistingItemAttributes(uploadedItem, existingItem);

//            Assert.IsNotNull(result);
//            Assert.IsTrue(result.ContainsKey("Test"));
//            Assert.IsTrue(result.ContainsKey("CreatedBy"));
//            Assert.IsTrue(result.ContainsKey("CreatedDateTimeUtc")); 
//            Assert.IsTrue(result.Count == 5);
//        }

//        [TestMethod]
//        public void Attributes_RemoveDeletedAttributes_ShouldRemoveAttributesWithValueOfRemove()
//        {
//            var attributes = new Dictionary<string,string>();
//            attributes.Add("Test", "keepme");
//            attributes.Add("Test1", "Remove");
//            attributes.Add("Test2", "keepthis too");

//            var result = ValidationManager.RemoveAttributesToBeDeleted(attributes);

//            Assert.IsNotNull(result);
//            Assert.IsTrue(result.Count == 2);
//            Assert.IsTrue(result.ContainsKey("Test"));
//            Assert.IsFalse(result.ContainsKey("Test1"));
//            Assert.IsTrue(result.ContainsKey("Test2"));

//        }

//    }
//}
//TODO: fix tests
