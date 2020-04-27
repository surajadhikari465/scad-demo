using System;
using System.Collections.Generic;
using System.Data;
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
    public class UpdateBrandsCommandHandlerTests
    {

        private IDbConnection sqlConnection;
        private TransactionScope transaction;

        private UpdateBrandsCommandHandler commandHandler;
        private UpdateBrandsCommand command;
        private RowObjectToUpdateBrandModelMapper mapper;

        [TestInitialize]
        public void Init()
        {
            sqlConnection = TestHelpers.Icon;
            transaction = new TransactionScope();
            mapper = new RowObjectToUpdateBrandModelMapper();
            commandHandler = new UpdateBrandsCommandHandler(sqlConnection);
            command = new UpdateBrandsCommand();
        }

        [TestMethod]
        public void UpdateBrands_Update1Brand_BrandName_NewNameSavedToDb()
        {
            var originalBrandId = 97536; // 1911 SPIRITS
            var originalBrand = TestHelpers.GetBrandAndTraitsByHierarchyClassId<UpdateBrandModel>(sqlConnection,originalBrandId);
            var expectedUpdatedBrandCount = 1;
            var expectedInvalidBrandCount = 0;

            Assert.IsNotNull(originalBrand, $"Could not find a brand with id {originalBrandId}");

            var originalBrandName = originalBrand.BrandName;
            var newBrandName = $"{originalBrandName}.updated";
            
            var updatedBrand = TestHelpers.CreateRowObject(1, new List<ParsedCell>
            {
                TestHelpers.CreateParsedCell(Constants.BrandIdColumnHeader, originalBrand.BrandId.ToString()),
                TestHelpers.CreateParsedCell(Constants.BrandNameColumnHeader, newBrandName),
                TestHelpers.CreateParsedCell(Constants.BrandAbbreviationColumnHeader,  originalBrand.BrandAbbreviation),
                TestHelpers.CreateParsedCell(Constants.ZipCodeColumnHeader, originalBrand.ZipCode),
                TestHelpers.CreateParsedCell(Constants.LocalityColumnHeader, originalBrand.Locality),
                TestHelpers.CreateParsedCell(Constants.DesignationColumnHeader, originalBrand.Designation),
                TestHelpers.CreateParsedCell(Constants.ParentCompanyColumnHeader, originalBrand.ParentCompany)
            });

            var brands = new List<RowObject> { updatedBrand };
            var mapperResponse = mapper.Map(brands, TestHelpers.GetHeaders(), TestHelpers.GetBrandAttributeModels(), "Tester");

            command.Brands.AddRange(mapperResponse.Brands);
            commandHandler.Execute(command);

            var brandAfterUpdate = TestHelpers.GetBrandAndTraitsByHierarchyClassId<UpdateBrandModel>(sqlConnection, originalBrandId);

            Assert.AreEqual(expectedUpdatedBrandCount, command.UpdatedBrandIds.Count);
            Assert.AreEqual(expectedUpdatedBrandCount, command.UpdatedBrandIds.Count);
            Assert.AreEqual(expectedInvalidBrandCount, command.InvalidBrands.Count);

            Assert.AreNotEqual(originalBrand.BrandName, brandAfterUpdate.BrandName);
        }


        [TestMethod]
        public void UpdateBrands_Update1Brand_Traits_TraitsSavedToDb()
        {
            var originalBrandId = 134114; // 1944 SKIN CARE
            var originalBrand = TestHelpers.GetBrandAndTraitsByHierarchyClassId<UpdateBrandModel>(sqlConnection, originalBrandId);
            var expectedUpdatedBrandCount = 1;
            var expectedInvalidBrandCount = 0;

            var expectedZip = "78613";
            var expectedParentCompany = "testPC";
            var expectedDesignation = "testDes";
            var expectedLocality = "testLocality";

            Assert.IsNotNull(originalBrand, $"Could not find a brand with id {originalBrandId}");

            var updatedBrand = TestHelpers.CreateRowObject(1, new List<ParsedCell>
            {
                TestHelpers.CreateParsedCell(Constants.BrandIdColumnHeader, originalBrand.BrandId.ToString()),
                TestHelpers.CreateParsedCell(Constants.BrandNameColumnHeader, originalBrand.BrandName),
                TestHelpers.CreateParsedCell(Constants.BrandAbbreviationColumnHeader,  originalBrand.BrandAbbreviation),
                TestHelpers.CreateParsedCell(Constants.ZipCodeColumnHeader, expectedZip),
                TestHelpers.CreateParsedCell(Constants.LocalityColumnHeader, expectedLocality),
                TestHelpers.CreateParsedCell(Constants.DesignationColumnHeader, expectedDesignation),
                TestHelpers.CreateParsedCell(Constants.ParentCompanyColumnHeader, expectedParentCompany)
            });

            var brands = new List<RowObject> { updatedBrand };
            var mapperResponse = mapper.Map(brands, TestHelpers.GetHeaders(), TestHelpers.GetBrandAttributeModels(), "Tester");

            command.Brands.AddRange(mapperResponse.Brands);
            commandHandler.Execute(command);

            var brandAfterUpdate = TestHelpers.GetBrandAndTraitsByHierarchyClassId<UpdateBrandModel>(sqlConnection, originalBrandId);

            Assert.AreEqual(expectedUpdatedBrandCount, command.UpdatedBrandIds.Count);
            Assert.AreEqual(expectedUpdatedBrandCount, command.UpdatedBrandIds.Count);
            Assert.AreEqual(expectedInvalidBrandCount, command.InvalidBrands.Count);

            Assert.AreNotEqual(expectedZip, originalBrand.ZipCode);
            Assert.AreNotEqual(expectedLocality, originalBrand.Locality);
            Assert.AreNotEqual(expectedDesignation, originalBrand.Designation);
            Assert.AreNotEqual(expectedParentCompany, originalBrand.ParentCompany);

            Assert.AreEqual(originalBrand.BrandName, brandAfterUpdate.BrandName);
            Assert.AreEqual(originalBrand.BrandAbbreviation, brandAfterUpdate.BrandAbbreviation);
            Assert.AreEqual(expectedZip, brandAfterUpdate.ZipCode);
            Assert.AreEqual(expectedLocality, brandAfterUpdate.Locality);
            Assert.AreEqual(expectedDesignation, brandAfterUpdate.Designation);
            Assert.AreEqual(expectedParentCompany, brandAfterUpdate.ParentCompany);
        }


        [TestMethod]
        public void UpdateBrands_Update1Brand_RemoveTraits_TaitsRemovedFromBrand()
        {
            var originalBrandId = 134114; // 1944 SKIN CARE
            var originalBrand = TestHelpers.GetBrandAndTraitsByHierarchyClassId<UpdateBrandModel>(sqlConnection, originalBrandId);
            var expectedUpdatedBrandCount = 1;
            var expectedInvalidBrandCount = 0;

            var expectedZip = "78613";
            var expectedParentCompany = "testPC";
            var expectedDesignation = "testDes";
            var expectedLocality = "testLocality";

            var removeValue = "REMOVE";

            Assert.IsNotNull(originalBrand, $"Could not find a brand with id {originalBrandId}");

            // set new values.
            var updatedBrand = TestHelpers.CreateRowObject(1, new List<ParsedCell>
            {
                TestHelpers.CreateParsedCell(Constants.BrandIdColumnHeader, originalBrand.BrandId.ToString()),
                TestHelpers.CreateParsedCell(Constants.BrandNameColumnHeader, originalBrand.BrandName),
                TestHelpers.CreateParsedCell(Constants.BrandAbbreviationColumnHeader,  originalBrand.BrandAbbreviation),
                TestHelpers.CreateParsedCell(Constants.ZipCodeColumnHeader, expectedZip),
                TestHelpers.CreateParsedCell(Constants.LocalityColumnHeader, expectedLocality),
                TestHelpers.CreateParsedCell(Constants.DesignationColumnHeader, expectedDesignation),
                TestHelpers.CreateParsedCell(Constants.ParentCompanyColumnHeader, expectedParentCompany)
            });

            var brands = new List<RowObject> { updatedBrand };
            var mapperResponse = mapper.Map(brands, TestHelpers.GetHeaders(), TestHelpers.GetBrandAttributeModels(), "Tester");

            command.Brands.AddRange(mapperResponse.Brands);
            commandHandler.Execute(command);

            // get updated brand.
            var brandAfterUpdate = TestHelpers.GetBrandAndTraitsByHierarchyClassId<UpdateBrandModel>(sqlConnection, originalBrandId);

            Assert.AreEqual(expectedUpdatedBrandCount, command.UpdatedBrandIds.Count);
            Assert.AreEqual(expectedUpdatedBrandCount, command.UpdatedBrandIds.Count);
            Assert.AreEqual(expectedInvalidBrandCount, command.InvalidBrands.Count);

            Assert.AreNotEqual(expectedZip, originalBrand.ZipCode);
            Assert.AreNotEqual(expectedLocality, originalBrand.Locality);
            Assert.AreNotEqual(expectedDesignation, originalBrand.Designation);
            Assert.AreNotEqual(expectedParentCompany, originalBrand.ParentCompany);


            // make sure traits were updated.
            Assert.AreEqual(originalBrand.BrandName, brandAfterUpdate.BrandName);
            Assert.AreEqual(originalBrand.BrandAbbreviation, brandAfterUpdate.BrandAbbreviation);
            Assert.AreEqual(expectedZip, brandAfterUpdate.ZipCode);
            Assert.AreEqual(expectedLocality, brandAfterUpdate.Locality);
            Assert.AreEqual(expectedDesignation, brandAfterUpdate.Designation);
            Assert.AreEqual(expectedParentCompany, brandAfterUpdate.ParentCompany);

            // remove traits.
            updatedBrand = TestHelpers.CreateRowObject(1, new List<ParsedCell>
            {
                TestHelpers.CreateParsedCell(Constants.BrandIdColumnHeader, originalBrand.BrandId.ToString()),
                TestHelpers.CreateParsedCell(Constants.BrandNameColumnHeader, originalBrand.BrandName),
                TestHelpers.CreateParsedCell(Constants.BrandAbbreviationColumnHeader,  originalBrand.BrandAbbreviation),
                TestHelpers.CreateParsedCell(Constants.ZipCodeColumnHeader, removeValue),
                TestHelpers.CreateParsedCell(Constants.LocalityColumnHeader, removeValue),
                TestHelpers.CreateParsedCell(Constants.DesignationColumnHeader, removeValue),
                TestHelpers.CreateParsedCell(Constants.ParentCompanyColumnHeader, removeValue)
            });

            brands = new List<RowObject> { updatedBrand };
            mapperResponse = mapper.Map(brands, TestHelpers.GetHeaders(), TestHelpers.GetBrandAttributeModels(), "Tester");

            command = new UpdateBrandsCommand();
            command.Brands.AddRange(mapperResponse.Brands);
            commandHandler.Execute(command);

            // get brand after remove
            var brandAfterRemove = TestHelpers.GetBrandAndTraitsByHierarchyClassId<UpdateBrandModel>(sqlConnection, originalBrandId);
            Assert.AreEqual(expectedUpdatedBrandCount, command.UpdatedBrandIds.Count);
            Assert.AreEqual(expectedUpdatedBrandCount, command.UpdatedBrandIds.Count);
            Assert.AreEqual(expectedInvalidBrandCount, command.InvalidBrands.Count);

            // make sure traits were removed.
            Assert.AreEqual(originalBrandId, brandAfterUpdate.BrandId);
            Assert.IsNull(brandAfterRemove.ZipCode);
            Assert.IsNull(brandAfterRemove.Locality);
            Assert.IsNull(brandAfterRemove.Designation);
            Assert.IsNull(brandAfterRemove.ParentCompany);
        }


        [TestMethod]
        public void UpdateBrands_Update1Brand_NoTraitsBadData_AddFails()
        {
            var originalBrandId = 2000320; // 2ND STREET DISTILLING
            var originalBrand = TestHelpers.GetBrandAndTraitsByHierarchyClassId<UpdateBrandModel>(sqlConnection, originalBrandId);
            
            var expectedAddedBrandCount = 0;
            var expectedInvalidBrandCount = 1;

            //given
            var brand = TestHelpers.CreateRowObject(1, new List<ParsedCell>
            {
                TestHelpers.CreateParsedCell(Constants.BrandIdColumnHeader, originalBrand.BrandId.ToString()),
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

            var brandAfterUpdate = TestHelpers.GetBrandAndTraitsByHierarchyClassId<UpdateBrandModel>(sqlConnection, originalBrandId);

            Assert.AreEqual(originalBrand.BrandName, brandAfterUpdate.BrandName);  // no changes. these should match.
            Assert.AreEqual(expectedAddedBrandCount, command.UpdatedBrandIds.Count);
            Assert.AreEqual(expectedAddedBrandCount, command.UpdatedBrandIds.Count);
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