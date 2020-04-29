using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using BulkItemUploadProcessor.Common;
using BulkItemUploadProcessor.Common.Models;
using Icon.Common.Models;
using BulkItemUploadProcessor.Service.ExcelParsing;
using OfficeOpenXml;
using System.IO;
using BulkItemUploadProcessor.Service.Mappers;
using BulkItemUploadProcessor.Service.Cache.Interfaces;
using BulkItemUploadProcessor.Service.Cache;

namespace BulkItemUploadProcessor.Service.Tests.Validators
{
    [TestClass]
    public class RowObjectToAddItemModelMapperTests
    {
        private List<RowObject> rowObjects;
        private List<ColumnHeader> columnHeaders;
        private ExcelRowParser excelRowParser;
        private ExcelPackage excelPackage;
        private List<AttributeModel> attributeModels;
        private RowObjectToAddItemModelMapper rowObjectToAddItemModelMapper;
        private Mock<IMerchItemPropertiesCache> merchItemPropertiesCache;

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

            excelPackage = new ExcelPackage(new FileInfo(@".\TestData\ExcelRowParserTest_SingleRow - ScalePlu.xlsx"));
            excelRowParser = new ExcelRowParser();
            merchItemPropertiesCache = new Mock<IMerchItemPropertiesCache>();
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

            attributeModels = new List<AttributeModel>()
            {
                 new AttributeModel(){AttributeId = 1, AttributeName="365Eligible", DisplayOrder =1 },
                new AttributeModel(){AttributeId = 2, AttributeName="RequestNumber", DisplayOrder =2 },
                new AttributeModel(){AttributeId = 3, AttributeName="Inactive", DisplayOrder =5 },
                new AttributeModel(){AttributeId = 4, AttributeName="POSDescription", DisplayOrder =10 },
                new AttributeModel(){AttributeId = 5, AttributeName="ItemPack", DisplayOrder =99, DefaultValue = "5" },
                new AttributeModel(){AttributeId = 6, AttributeName="VitaminK", DisplayOrder =100, DefaultValue = "Vi", IsReadOnly = true },
            };
            merchItemPropertiesCache.SetupGet(m => m.Properties)
                .Returns( new Dictionary<int, MerchPropertiesModel>()
                { {5000376,
                 new MerchPropertiesModel {FinancialHierarcyClassId=84238, ItemTypeCode="NRT",MerchandiseHierarchyClassId=5000376, NonMerchandiseTraitValue="Leagcy", ProhibitDiscount=false }}
                });

            rowObjectToAddItemModelMapper = new RowObjectToAddItemModelMapper(merchItemPropertiesCache.Object);
        }

        [TestMethod]
        public void ValidateMap_AttributeWithDefaultValueNotSpecified_ShouldAddAttributeWithDefaultValueToItem()
        {
            var response = rowObjectToAddItemModelMapper.Map(rowObjects,columnHeaders, attributeModels,"Test");

            //then        
            Assert.IsTrue(response.Items[0].ItemAttributesJson.Contains("ItemPack\":\"5\""));
            Assert.IsFalse(response.Items[0].ItemAttributesJson.Contains("POSDescription"));
            Assert.IsFalse(response.Items[0].ItemAttributesJson.Contains("RequestNumber"));
            // since its readonly it should not get into json
            Assert.IsFalse(response.Items[0].ItemAttributesJson.Contains("VitaminK"));
            Assert.IsFalse(response.Items[0].ItemAttributesJson.Contains("365Eligible"));
            // inactive is always added by the code
            Assert.IsTrue(response.Items[0].ItemAttributesJson.Contains("Inactive"));
        }
    }
}
