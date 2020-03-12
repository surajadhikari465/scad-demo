using Icon.Common.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Icon.Common.Validators.ItemAttributes;
using BulkItemUploadProcessor.Service.Validation.Interfaces;
using Icon.Common.Validators;
using BulkItemUploadProcessor.Service.Validation;
using BulkItemUploadProcessor.Common;
using BulkItemUploadProcessor.DataAccess.Queries;
using BulkItemUploadProcessor.Common.Models;
using Icon.Common.Models;
using BulkItemUploadProcessor.Service.ExcelParsing;
using OfficeOpenXml;
using System.IO;

namespace BulkItemUploadProcessor.Service.Tests.Validators
{
    [TestClass]
    public class RowObjectsValidatorTests
    {
        private RowObjectsValidator validator;
        private Mock<IItemAttributesValidatorFactory> mockItemAttributesValidatorFactory;
        private Mock<IHierarchyValidator> mockHierarchyValidator;
        private Mock<ScanCodeValidator> mockScanCodeValidator;
        private Mock<IQueryHandler<EmptyQueryParameters<List<BarcodeTypeModel>>, List<BarcodeTypeModel>>> mockGetBarcodeTypesQueryHandler;
        private Mock<IQueryHandler<GetScanCodesThatExistParameters, HashSet<string>>> mockGetScanCodesThatExistQueryHandler;
        private List<RowObject> rowObjects;
        private ExcelRowParser excelRowParser;
        private ExcelPackage excelPackage;
        private List<ColumnHeader> columnHeaders;
        private List<AttributeModel> attributeModels;
        private List<string> columnNames = new List<string>
        {
            "Barcode Type",
            "Scan Code",
            "Brands",
            "Merchandise",
            "Tax",
            "National",
            "Manufacturer",
            "Product Description",
            "POS Description",
            "Item Pack",
            "Retail Size",
            "UOM",
            "Food Stamp Eligible",
            "POS Scale Tare",
            "Delivery System",
            "Alcohol By Volume",
            "Notes",
            "Casein Free",
            "Drained Weight",
            "Fair Trade Certified",
            "Drained Weight UOM",
            "Hemp",
            "Local Loan Producer",
            "Nutrition Required",
            "Organic Personal Care",
            "Air Chilled",
            "Animal Welfare Rating",
            "Biodynamic",
            "Cheese Attribute: Milk Type",
            "Raw",
            "Dry Aged",
            "Eco-Scale Rating",
            "Free Range",
            "Fresh or Frozen",
            "Gluten Free",
            "Grass Fed",
            "Kosher",
            "Made In House",
            "MSC",
            "Non-GMO",
            "Organic",
            "Paleo",
            "Pasture Raised",
            "Premium Body Care"
        };

        [TestInitialize]
        public void Initialize()
        {
            mockScanCodeValidator = new Mock<ScanCodeValidator>();
            mockHierarchyValidator = new Mock<IHierarchyValidator>();
            mockGetBarcodeTypesQueryHandler = new Mock<IQueryHandler<EmptyQueryParameters<List<BarcodeTypeModel>>, List<BarcodeTypeModel>>>();
            mockGetScanCodesThatExistQueryHandler = new Mock<IQueryHandler<GetScanCodesThatExistParameters, HashSet<string>>>();
            mockItemAttributesValidatorFactory = new Mock<IItemAttributesValidatorFactory>();
            validator = new RowObjectsValidator
                                (mockItemAttributesValidatorFactory.Object,
                                 mockHierarchyValidator.Object,
                                 mockScanCodeValidator.Object,
                                 mockGetBarcodeTypesQueryHandler.Object,
                                 mockGetScanCodesThatExistQueryHandler.Object
                                 );

            excelPackage = new ExcelPackage(new FileInfo(@".\TestData\ExcelRowParserTest_SingleRow - ScalePlu.xlsx"));
            excelRowParser = new ExcelRowParser();
            columnHeaders = columnNames
                    .Select((c, i) => new ColumnHeader
                    {
                        Address = null,
                        ColumnIndex = i + 1,
                        Name = c
                    }).ToList();

            //When
            rowObjects = excelRowParser.Parse(
                excelPackage.Workbook.Worksheets["items"],
                columnHeaders);

            attributeModels = new List<AttributeModel>();
        }

        [TestMethod]
        public void Validate_BarcodeIsScalePluScanCodeISNotValid_ShouldReturnFalse()
        {
            mockGetBarcodeTypesQueryHandler.Setup(s => s.Search(It.IsAny<EmptyQueryParameters<List<BarcodeTypeModel>>>())).Returns(
                new List<BarcodeTypeModel>()
                {
                    new BarcodeTypeModel{ BarcodeTypeId = 19,
                                          BarcodeType = "Scale PLU (20000000000-20999900000)",
                                          BeginRange = "20000000000",
                                          EndRange = "20999900000",
                                          ScalePlu =true},
                     new BarcodeTypeModel{ BarcodeTypeId = 12,
                                          BarcodeType = "UPC",
                                          BeginRange = null,
                                          EndRange = null,
                                          ScalePlu =false},

                }
              );

            mockGetScanCodesThatExistQueryHandler.Setup(s => s.Search(It.IsAny<GetScanCodesThatExistParameters>())).Returns(new HashSet<string>());
            mockHierarchyValidator.Setup(s => s.Validate(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(new ValidationResponse() { IsValid = true, Error = string.Empty });

            var rowObjectValidatorResponse = validator.Validate(Enums.FileModeTypeEnum.CreateNew, rowObjects, columnHeaders, attributeModels);

            Assert.AreEqual(1, rowObjectValidatorResponse.InvalidRows.Count());
            Assert.AreEqual("Scan Code '21000000001' does not fall under 'Scale PLU (20000000000-20999900000)' range.", rowObjectValidatorResponse.InvalidRows[0].Error);
        }
    }
}
