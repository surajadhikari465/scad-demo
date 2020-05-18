using System.Collections.Generic;
using BrandUploadProcessor.Common;
using BrandUploadProcessor.Common.Models;
using BrandUploadProcessor.Service.Mappers;
using BrandUploadProcessor.Service.Mappers.Interfaces;
using Icon.Common.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BrandUploadProcessor.Service.Tests
{
    [TestClass]
    public class RowObjectToUpdateBrandModelMapperTests
    {
        private IRowObjectToUpdateBrandModelMapper mapper;
        private List<ColumnHeader> columnHeaders;
        private List<RowObject> rowObjects;
        private List<BrandAttributeModel> brandAttributeModels;

        [TestInitialize]
        public void Init()
        {
            columnHeaders = TestHelpers.GetHeaders();
            brandAttributeModels = new List<BrandAttributeModel>();

            rowObjects = new List<RowObject>
            {
                TestHelpers.CreateRowObject(1, new List<ParsedCell>
                {
                    TestHelpers.CreateParsedCell(Constants.BrandIdColumnHeader, "12345"),
                    TestHelpers.CreateParsedCell(Constants.ParentCompanyColumnHeader, "test"),

                })
            };
            mapper = new RowObjectToUpdateBrandModelMapper();
        }

        [TestMethod]
        public void Map_ParentCompany_ReturnBrandName()
        {
            var result = mapper.Map(rowObjects, columnHeaders, brandAttributeModels, "Tester");
            var expected = "test";
            Assert.AreEqual(expected, result.Brands[0].ParentCompany);
        }

        [TestMethod]
        public void Map_BrandId_ReturnBrandId()
        {
            var result = mapper.Map(rowObjects, columnHeaders, brandAttributeModels, "Tester");
            var expected = 12345;
            Assert.AreEqual(expected, result.Brands[0].BrandId);
        }


        [TestMethod]
        public void Map_BrandName_EmptyStringBrandAbbreviation_ReturnNull()
        {

            var rows = new List<RowObject>
            {
                TestHelpers.CreateRowObject(1, new List<ParsedCell>
                {
                    TestHelpers.CreateParsedCell(Constants.BrandIdColumnHeader, "12345"),
                    TestHelpers.CreateParsedCell(Constants.BrandAbbreviationColumnHeader, ""),
                    TestHelpers.CreateParsedCell(Constants.ParentCompanyColumnHeader, "test"),
                })
            };

            var result = mapper.Map(rows, columnHeaders, brandAttributeModels, "Tester");
            Assert.IsNull(result.Brands[0].BrandAbbreviation);
        }

        [TestMethod]
        public void Map_BrandName_ValidValueBrandAbbreviation_ReturnBrandAbbreviation()
        {

            var rows = new List<RowObject>
            {
                TestHelpers.CreateRowObject(1, new List<ParsedCell>
                {
                    TestHelpers.CreateParsedCell(Constants.BrandIdColumnHeader, "12345"),
                    TestHelpers.CreateParsedCell(Constants.BrandAbbreviationColumnHeader, "tst"),
                    TestHelpers.CreateParsedCell(Constants.ParentCompanyColumnHeader, "test"),
                })
            };

            var result = mapper.Map(rows, columnHeaders, brandAttributeModels, "Tester");
            Assert.IsNotNull(result.Brands[0].BrandAbbreviation);
            Assert.AreEqual("tst", result.Brands[0].BrandAbbreviation);
        }


        [TestMethod]
        public void Map_BrandName_RemoveBrandAbbreviation_ReturnsNull()
        {

            var rows = new List<RowObject>
            {
                TestHelpers.CreateRowObject(1, new List<ParsedCell>
                {
                    TestHelpers.CreateParsedCell(Constants.BrandIdColumnHeader, "12345"),
                    TestHelpers.CreateParsedCell(Constants.BrandAbbreviationColumnHeader, "REMOVE"),
                    TestHelpers.CreateParsedCell(Constants.ParentCompanyColumnHeader, "test"),
                })
            };

            var result = mapper.Map(rows, columnHeaders, brandAttributeModels, "Tester");
            Assert.IsNull(result.Brands[0].BrandName);
        }




        [TestMethod]
        public void Map_BrandName_EmptyStringBrandName_ReturnNull()
        {

            var rows = new List<RowObject>
            {
                TestHelpers.CreateRowObject(1, new List<ParsedCell>
                {
                    TestHelpers.CreateParsedCell(Constants.BrandIdColumnHeader, "12345"),
                    TestHelpers.CreateParsedCell(Constants.BrandNameColumnHeader, ""),
                    TestHelpers.CreateParsedCell(Constants.ParentCompanyColumnHeader, "test"),
                })
            };

            var result = mapper.Map(rows, columnHeaders, brandAttributeModels, "Tester");
            Assert.IsNull(result.Brands[0].BrandName);
        }

        [TestMethod]
        public void Map_BrandName_ValidValueBrandName_ReturnBrandName()
        {

            var rows = new List<RowObject>
            {
                TestHelpers.CreateRowObject(1, new List<ParsedCell>
                {
                    TestHelpers.CreateParsedCell(Constants.BrandIdColumnHeader, "12345"),
                    TestHelpers.CreateParsedCell(Constants.BrandNameColumnHeader, "test brand"),
                    TestHelpers.CreateParsedCell(Constants.ParentCompanyColumnHeader, "test"),
                })
            };

            var result = mapper.Map(rows, columnHeaders, brandAttributeModels, "Tester");
            Assert.IsNotNull(result.Brands[0].BrandName);
            Assert.AreEqual("test brand", result.Brands[0].BrandName);
        }


        [TestMethod]
        public void Map_BrandName_RemoveBrandName_ReturnsNull()
        {

            var rows = new List<RowObject>
            {
                TestHelpers.CreateRowObject(1, new List<ParsedCell>
                {
                    TestHelpers.CreateParsedCell(Constants.BrandIdColumnHeader, "12345"),
                    TestHelpers.CreateParsedCell(Constants.BrandNameColumnHeader, "REMOVE"),
                    TestHelpers.CreateParsedCell(Constants.ParentCompanyColumnHeader, "test"),
                })
            };

            var result = mapper.Map(rows, columnHeaders, brandAttributeModels, "Tester");
            Assert.IsNull(result.Brands[0].BrandName);
        }



        [TestMethod]
        public void RowObjectToUpdateBrandModelMappter_ReturnsUpdateMapperResponse()
        {
            var result = mapper.Map(rowObjects, columnHeaders, brandAttributeModels, "Tester");
            Assert.IsInstanceOfType(result, typeof(RowObjectToBrandMapperResponse<UpdateBrandModel>));
        }
    }
}