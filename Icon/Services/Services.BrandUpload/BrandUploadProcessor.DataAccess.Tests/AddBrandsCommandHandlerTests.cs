using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using BrandUploadProcessor.Common;
using BrandUploadProcessor.Common.Models;
using BrandUploadProcessor.DataAccess.Commands;
using BrandUploadProcessor.Service.Mappers;
using BrandUploadProcessor.Service.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BrandUploadProcessor.DataAccess.Tests
{
    [TestClass]
    public class AddBrandsCommandHandlerTests
    {


        private IDbConnection sqlConnection;
        private TransactionScope transaction;

        private AddBrandsCommandHandler commandHandler;
        private AddBrandsCommand command;
        private RowObjectToAddBrandModelMapper mapper;

        [TestInitialize]
        public void Init()
        {
            sqlConnection = TestHelpers.Icon;
            transaction = new TransactionScope();
            mapper = new RowObjectToAddBrandModelMapper();
            commandHandler = new AddBrandsCommandHandler(sqlConnection);
            command= new AddBrandsCommand();
        }


        [TestMethod]
        public void AddNewBrands_Add1Brand_NoTraitsBadDataRequiredFieldsTooLong_AddFails()
        {

            var timestamp = DateTime.Now.ToString("MMddyyyy.HHmmss");
            var expectedAddedBrandCount = 0;
            var expectedInvalidBrandCount = 1;

            //given
            var brand = TestHelpers.CreateRowObject(1, new List<ParsedCell>
            {
                TestHelpers.CreateParsedCell(Constants.BrandIdColumnHeader, string.Empty),
                TestHelpers.CreateParsedCell(Constants.BrandNameColumnHeader, "1".PadRight(300,'1')),
                TestHelpers.CreateParsedCell(Constants.BrandAbbreviationColumnHeader,  "1".PadRight(300,'1')),
                TestHelpers.CreateParsedCell(Constants.ZipCodeColumnHeader, string.Empty),
                TestHelpers.CreateParsedCell(Constants.LocalityColumnHeader, string.Empty),
                TestHelpers.CreateParsedCell(Constants.DesignationColumnHeader, string.Empty),
                TestHelpers.CreateParsedCell(Constants.ParentCompanyColumnHeader, string.Empty)
            });

            var brands = new List<RowObject> { brand };
            var mapperResponse = mapper.Map(brands, TestHelpers.GetHeaders(), TestHelpers.GetBrandAttributeModels(), "Tester");

            command.Brands.AddRange(mapperResponse.Brands);
            commandHandler.Execute(command);

            Assert.AreEqual(expectedAddedBrandCount, command.AddedBrandIds.Count);
            Assert.AreEqual(expectedAddedBrandCount, command.AddedBrandIds.Count);
            Assert.AreEqual(expectedInvalidBrandCount, command.InvalidBrands.Count);

        }


        [TestMethod]
        public void AddNewBrands_Add1Brand_NoTraits_AddSuccsesful()
        {

            var timestamp = DateTime.Now.ToString("MMddyyyy.HHmmss");
            var expectedAddedBrandCount = 1;
            var expectedInvalidBrandCount = 0;

            //given
            var brand = TestHelpers.CreateRowObject(1, new List<ParsedCell>
            {
                TestHelpers.CreateParsedCell(Constants.BrandIdColumnHeader, string.Empty),
                TestHelpers.CreateParsedCell(Constants.BrandNameColumnHeader, $"TestBrand-{timestamp}"),
                TestHelpers.CreateParsedCell(Constants.BrandAbbreviationColumnHeader, "tb99"),
                TestHelpers.CreateParsedCell(Constants.ZipCodeColumnHeader, string.Empty),
                TestHelpers.CreateParsedCell(Constants.LocalityColumnHeader, string.Empty),
                TestHelpers.CreateParsedCell(Constants.DesignationColumnHeader, string.Empty),
                TestHelpers.CreateParsedCell(Constants.ParentCompanyColumnHeader, string.Empty)
            });

            var brands = new List<RowObject> { brand };
            var mapperResponse = mapper.Map(brands, TestHelpers.GetHeaders(), TestHelpers.GetBrandAttributeModels(), "Tester");

            command = new AddBrandsCommand();
            command.Brands.AddRange(mapperResponse.Brands);

            commandHandler.Execute(command);

            Assert.AreEqual(expectedAddedBrandCount, command.AddedBrandIds.Count);

            var addedBrand = TestHelpers.GetBrandAndTraitsByHierarchyClassId<AddBrandModel>(sqlConnection, command.AddedBrandIds[0]);

            
            Assert.IsNotNull(addedBrand, "Did not find the expected brand in the database");
            Assert.AreEqual($"TestBrand-{timestamp}", addedBrand.BrandName);
            Assert.AreEqual(expectedAddedBrandCount, command.AddedBrandIds.Count);
            Assert.AreEqual(expectedInvalidBrandCount, command.InvalidBrands.Count);

        }


        [TestMethod]
        public void AddNewBrands_Add1Brand_AllTraits_AddSuccsesful()
        {

            var timestamp = DateTime.Now.ToString("MMddyyyy.HHmmss");
            var expectedAddedBrandCount = 1;
            var expectedInvalidBrandCount = 0;

            var expectedZip = "78613";
            var expectedPC = "testPC";
            var expectedDesignation = "testDes";
            var expectedLocality = "testLocality";

            //given
            var brand = TestHelpers.CreateRowObject(1, new List<ParsedCell>
            {
                TestHelpers.CreateParsedCell(Constants.BrandIdColumnHeader, string.Empty),
                TestHelpers.CreateParsedCell(Constants.BrandNameColumnHeader, $"TestBrand-{timestamp}"),
                TestHelpers.CreateParsedCell(Constants.BrandAbbreviationColumnHeader, "tb99"),
                TestHelpers.CreateParsedCell(Constants.ZipCodeColumnHeader, expectedZip),
                TestHelpers.CreateParsedCell(Constants.LocalityColumnHeader, expectedLocality),
                TestHelpers.CreateParsedCell(Constants.DesignationColumnHeader, expectedDesignation),
                TestHelpers.CreateParsedCell(Constants.ParentCompanyColumnHeader, expectedPC)
            });

            var brands = new List<RowObject> { brand };
            var mapperResponse = mapper.Map(brands, TestHelpers.GetHeaders(), TestHelpers.GetBrandAttributeModels(), "Tester");

            command = new AddBrandsCommand();
            command.Brands.AddRange(mapperResponse.Brands);

            commandHandler.Execute(command);

            Assert.AreEqual(expectedAddedBrandCount, command.AddedBrandIds.Count);

            var addedBrand = TestHelpers.GetBrandAndTraitsByHierarchyClassId<AddBrandModel>(sqlConnection, command.AddedBrandIds[0]);


            Assert.IsNotNull(addedBrand, "Did not find the expected brand in the database");
            Assert.AreEqual($"TestBrand-{timestamp}", addedBrand.BrandName);
            Assert.AreEqual(expectedAddedBrandCount, command.AddedBrandIds.Count);
            Assert.AreEqual(expectedInvalidBrandCount, command.InvalidBrands.Count);
            Assert.AreEqual(expectedZip, addedBrand.ZipCode);
            Assert.AreEqual(expectedLocality, addedBrand.Locality);
            Assert.AreEqual(expectedDesignation, addedBrand.Designation);
            Assert.AreEqual(expectedPC, addedBrand.ParentCompany);

        }


        [TestMethod]
        public void AddNewBrands_AddMoreThan1Brand_NoTraits_AddSuccsesful()
        {

            var timestamp = DateTime.Now.ToString("MMddyyyy.HHmmss");

            //given
            var brand1 = TestHelpers.CreateRowObject(1, new List<ParsedCell>
            {
                TestHelpers.CreateParsedCell(Constants.BrandIdColumnHeader, string.Empty),
                TestHelpers.CreateParsedCell(Constants.BrandNameColumnHeader, $"Test-AddMoreThan1Brand1-{timestamp}"),
                TestHelpers.CreateParsedCell(Constants.BrandAbbreviationColumnHeader, "tb91"),
                TestHelpers.CreateParsedCell(Constants.ZipCodeColumnHeader, string.Empty),
                TestHelpers.CreateParsedCell(Constants.LocalityColumnHeader, string.Empty),
                TestHelpers.CreateParsedCell(Constants.DesignationColumnHeader, string.Empty),
                TestHelpers.CreateParsedCell(Constants.ParentCompanyColumnHeader, string.Empty)
            });
            var brand2 = TestHelpers.CreateRowObject(2, new List<ParsedCell>
            {
                TestHelpers.CreateParsedCell(Constants.BrandIdColumnHeader, string.Empty),
                TestHelpers.CreateParsedCell(Constants.BrandNameColumnHeader, $"Test-AddMoreThan1Brand2-{timestamp}"),
                TestHelpers.CreateParsedCell(Constants.BrandAbbreviationColumnHeader, "tb92"),
                TestHelpers.CreateParsedCell(Constants.ZipCodeColumnHeader, string.Empty),
                TestHelpers.CreateParsedCell(Constants.LocalityColumnHeader, string.Empty),
                TestHelpers.CreateParsedCell(Constants.DesignationColumnHeader, string.Empty),
                TestHelpers.CreateParsedCell(Constants.ParentCompanyColumnHeader, string.Empty)
            });

            var brands = new List<RowObject> { brand1, brand2 };
            var mapperResponse = mapper.Map(brands, TestHelpers.GetHeaders(), TestHelpers.GetBrandAttributeModels(), "Tester");

            command = new AddBrandsCommand();
            command.Brands.AddRange(mapperResponse.Brands);
            commandHandler.Execute(command);

            const int expectedAddedBrandCount = 2;
            const int expectedInvalidBrandCount = 0;

            var addedBrands = command.AddedBrandIds.ToDictionary(b => b,
                b => TestHelpers.GetBrandAndTraitsByHierarchyClassId<AddBrandModel>(sqlConnection, b));

            var bothFound = addedBrands.Values.ToArray()[0].BrandAbbreviation == "tb91" &&
                            addedBrands.Values.ToArray()[1].BrandAbbreviation == "tb92";


            Assert.IsFalse(addedBrands.Values.Any(a => a == null));
            Assert.IsTrue(bothFound);
            Assert.AreEqual(expectedAddedBrandCount, addedBrands.Count);
            Assert.AreEqual(expectedAddedBrandCount, command.AddedBrandIds.Count);
            Assert.AreEqual(expectedInvalidBrandCount, command.InvalidBrands.Count);

        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose();
            sqlConnection.Dispose();
        }


    }
}
