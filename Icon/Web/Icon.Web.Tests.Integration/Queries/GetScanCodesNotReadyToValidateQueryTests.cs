using Icon.Framework;
using Icon.Testing.Builders;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Tests.Common.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Icon.Web.Tests.Integration.Queries
{
    [TestClass] [Ignore]
    public class GetScanCodesNotReadyToValidateQueryTests
    {
        private GetScanCodesNotReadyToValidateQuery query;
        private GetScanCodesNotReadyToValidateParameters parameters;
        private IconContext context;
        private DbContextTransaction transaction;
        private HierarchyClass testBrand;
        private HierarchyClass testMerch;
        private HierarchyClass testTax;
        private HierarchyClass testNational;
        private string scanCode1 = "1234561";
        private string scanCode2 = "1234562";
        private string scanCode3 = "1234563";
        private string scanCode4 = "1234564";

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            transaction = context.Database.BeginTransaction();
            query = new GetScanCodesNotReadyToValidateQuery(context);
            parameters = new GetScanCodesNotReadyToValidateParameters();

            testBrand = new TestHierarchyClassBuilder().WithHierarchyId(Hierarchies.Brands);
            testMerch = new TestHierarchyClassBuilder().WithHierarchyId(Hierarchies.Merchandise);
            testTax = new TestHierarchyClassBuilder().WithHierarchyId(Hierarchies.Tax);
            testNational = new TestHierarchyClassBuilder().WithHierarchyId(Hierarchies.National);

            context.HierarchyClass.Add(testBrand);
            context.HierarchyClass.Add(testMerch);
            context.HierarchyClass.Add(testTax);
            context.HierarchyClass.Add(testNational);

            context.SaveChanges();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
            transaction.Dispose();
            context.Dispose();
        }

        [TestMethod]
        public void GetScanCodesNotReadyToValidate_ItemsHaveMissingHierarchyCanonicalInformation_ShouldAllReturnScanCodes()
        {
            //Given
            Item itemWithNoBrand = new TestItemBuilder()
                .WithScanCode(scanCode1)
                .WithSubBrickAssociation(testMerch.hierarchyClassID)
                .WithTaxClassAssociation(testTax.hierarchyClassID);
            Item itemWithNoTax = new TestItemBuilder()
                .WithScanCode(scanCode2)
                .WithSubBrickAssociation(testMerch.hierarchyClassID)
                .WithBrandAssociation(testBrand.hierarchyClassID);
            Item itemWithNoMerch = new TestItemBuilder()
                .WithScanCode(scanCode3)
                .WithBrandAssociation(testBrand.hierarchyClassID)
                .WithTaxClassAssociation(testMerch.hierarchyClassID);
            Item itemWithNoTaxOrMerch = new TestItemBuilder()
                .WithScanCode(scanCode4)
                .WithBrandAssociation(testBrand.hierarchyClassID);

            context.Item.AddRange(new List<Item>
                {
                    itemWithNoBrand,
                    itemWithNoTax,
                    itemWithNoMerch,
                    itemWithNoTaxOrMerch
                });
            context.SaveChanges();

            parameters.Items = new List<BulkImportItemModel>
                {
                    new TestBulkImportItemModelBuilder().FromItem(itemWithNoBrand),
                    new TestBulkImportItemModelBuilder().FromItem(itemWithNoTax),
                    new TestBulkImportItemModelBuilder().FromItem(itemWithNoMerch),
                    new TestBulkImportItemModelBuilder().FromItem(itemWithNoTaxOrMerch)
                };

            //When
            var result = query.Search(parameters);

            //Then
            Assert.AreEqual(4, result.Count);
            Assert.IsTrue(result.Contains(scanCode1));
            Assert.IsTrue(result.Contains(scanCode2));
            Assert.IsTrue(result.Contains(scanCode3));
            Assert.IsTrue(result.Contains(scanCode4));
        }

        [TestMethod]
        public void GetScanCodesNotReadyToValidate_ItemsHaveMissingTraitCanonicalInformation_ShouldAllReturnScanCodes()
        {
            //Given
            Item itemWithNoProductDescription = new TestItemBuilder()
                .WithScanCode(scanCode1)
                .WithBrandAssociation(testBrand.hierarchyClassID)
                .WithSubBrickAssociation(testMerch.hierarchyClassID)
                .WithTaxClassAssociation(testTax.hierarchyClassID)
                .WithProductDescription(null);
            Item itemWithNoPosDescription = new TestItemBuilder()
                .WithScanCode(scanCode2)
                .WithBrandAssociation(testBrand.hierarchyClassID)
                .WithSubBrickAssociation(testMerch.hierarchyClassID)
                .WithTaxClassAssociation(testTax.hierarchyClassID)
                .WithPosDescription(null);
            Item itemWithNoPackageUnit = new TestItemBuilder()
                .WithScanCode(scanCode3)
                .WithBrandAssociation(testBrand.hierarchyClassID)
                .WithSubBrickAssociation(testMerch.hierarchyClassID)
                .WithTaxClassAssociation(testTax.hierarchyClassID)
                .WithPackageUnit(null);
            Item itemWithNoPosScaleTare = new TestItemBuilder()
                .WithScanCode(scanCode4)
                .WithBrandAssociation(testBrand.hierarchyClassID)
                .WithSubBrickAssociation(testMerch.hierarchyClassID)
                .WithTaxClassAssociation(testTax.hierarchyClassID)
                .WithPosScaleTare(null);

            context.Item.AddRange(new List<Item>
                {
                    itemWithNoProductDescription,
                    itemWithNoPosDescription,
                    itemWithNoPackageUnit,
                    itemWithNoPosScaleTare
                });
            context.SaveChanges();

            parameters.Items = new List<BulkImportItemModel>
                {
                    new TestBulkImportItemModelBuilder().FromItem(itemWithNoProductDescription).WithProductDescription(String.Empty),
                    new TestBulkImportItemModelBuilder().FromItem(itemWithNoPosDescription).WithPosDescription(String.Empty),
                    new TestBulkImportItemModelBuilder().FromItem(itemWithNoPackageUnit).WithPackageUnit(String.Empty),
                    new TestBulkImportItemModelBuilder().FromItem(itemWithNoPosScaleTare).WithPosScaleTare(String.Empty)
                };

            //When
            var result = query.Search(parameters);

            //Then
            Assert.AreEqual(4, result.Count);
            Assert.IsTrue(result.Contains(scanCode1));
            Assert.IsTrue(result.Contains(scanCode2));
            Assert.IsTrue(result.Contains(scanCode3));
            Assert.IsTrue(result.Contains(scanCode4));
        }

        [TestMethod]
        public void GetScanCodesNotReadyToValidate_ItemsHaveAllRequiredCanonicalInformationAndArePassedAllRequiredCanonicalInformation_ShouldReturnAnEmptyList()
        {
            //Given
            Item item1 = new TestItemBuilder()
                .WithScanCode(scanCode1)
                .WithBrandAssociation(testBrand.hierarchyClassID)
                .WithSubBrickAssociation(testMerch.hierarchyClassID)
                .WithTaxClassAssociation(testTax.hierarchyClassID)
                .WithNationalClassAssociation(testNational.hierarchyClassID);
            Item item2 = new TestItemBuilder()
                .WithScanCode(scanCode2)
                .WithBrandAssociation(testBrand.hierarchyClassID)
                .WithSubBrickAssociation(testMerch.hierarchyClassID)
                .WithTaxClassAssociation(testTax.hierarchyClassID)
                  .WithNationalClassAssociation(testNational.hierarchyClassID);
            Item item3 = new TestItemBuilder()
                .WithScanCode(scanCode3)
                .WithBrandAssociation(testBrand.hierarchyClassID)
                .WithSubBrickAssociation(testMerch.hierarchyClassID)
                .WithTaxClassAssociation(testTax.hierarchyClassID)
                  .WithNationalClassAssociation(testNational.hierarchyClassID);
            Item item4 = new TestItemBuilder()
                .WithScanCode(scanCode4)
                .WithBrandAssociation(testBrand.hierarchyClassID)
                .WithSubBrickAssociation(testMerch.hierarchyClassID)
                .WithTaxClassAssociation(testTax.hierarchyClassID)
                  .WithNationalClassAssociation(testNational.hierarchyClassID);

            context.Item.AddRange(new List<Item>
                {
                    item1,
                    item2,
                    item3,
                    item4
                });
            context.SaveChanges();

            parameters.Items = new List<BulkImportItemModel>
                {
                    new TestBulkImportItemModelBuilder().FromItem(item1),
                    new TestBulkImportItemModelBuilder().FromItem(item2),
                    new TestBulkImportItemModelBuilder().FromItem(item3),
                    new TestBulkImportItemModelBuilder().FromItem(item4)
                };

            //When
            var result = query.Search(parameters);

            //Then
            Assert.IsFalse(result.Any());
        }

        [TestMethod]
        public void GetScanCodesNotReadyToValidate_ItemsArePassedAllRequiredCanonicalInformation_ShouldReturnEmptyList()
        {
            //Given
            Item item1 = new TestItemBuilder()
                .WithScanCode(scanCode1);
            Item item2 = new TestItemBuilder()
                .WithScanCode(scanCode2);
            Item item3 = new TestItemBuilder()
                .WithScanCode(scanCode3);
            Item item4 = new TestItemBuilder()
                .WithScanCode(scanCode4);

            parameters.Items = new List<BulkImportItemModel>
                {
                    new TestBulkImportItemModelBuilder().FromItem(item1)
                        .WithHierarchyClassIds(testBrand.hierarchyClassID.ToString(), testMerch.hierarchyClassID.ToString(), testTax.hierarchyClassID.ToString(), null, testNational.hierarchyClassID.ToString()),
                    new TestBulkImportItemModelBuilder().FromItem(item2)
                        .WithHierarchyClassIds(testBrand.hierarchyClassID.ToString(), testMerch.hierarchyClassID.ToString(), testTax.hierarchyClassID.ToString(), null, testNational.hierarchyClassID.ToString()),
                    new TestBulkImportItemModelBuilder().FromItem(item3)
                        .WithHierarchyClassIds(testBrand.hierarchyClassID.ToString(), testMerch.hierarchyClassID.ToString(), testTax.hierarchyClassID.ToString(), null, testNational.hierarchyClassID.ToString()),
                    new TestBulkImportItemModelBuilder().FromItem(item4)
                        .WithHierarchyClassIds(testBrand.hierarchyClassID.ToString(), testMerch.hierarchyClassID.ToString(), testTax.hierarchyClassID.ToString(), null, testNational.hierarchyClassID.ToString())
                };

            context.Item.AddRange(new List<Item>
                {
                    new Item { itemTypeID = 1, ScanCode = new List<ScanCode> { new ScanCode { scanCode = scanCode1, scanCodeTypeID = ScanCodeTypes.Upc } } },
                    new Item { itemTypeID = 1, ScanCode = new List<ScanCode> { new ScanCode { scanCode = scanCode2, scanCodeTypeID = ScanCodeTypes.Upc } } },
                    new Item { itemTypeID = 1, ScanCode = new List<ScanCode> { new ScanCode { scanCode = scanCode3, scanCodeTypeID = ScanCodeTypes.Upc } } },
                    new Item { itemTypeID = 1, ScanCode = new List<ScanCode> { new ScanCode { scanCode = scanCode4, scanCodeTypeID = ScanCodeTypes.Upc } } },
                });
            context.SaveChanges();

            //When
            var result = query.Search(parameters);

            //Then
            Assert.IsFalse(result.Any());
        }

        [TestMethod]
        public void GetScanCodesNotReadyToValidate_ItemsHaveAllRequiredCanonicalInformationAndParametersContainOnlyScanCodes_ShouldReturnEmptyList()
        {
            //Given
            Item item1 = new TestItemBuilder()
                .WithScanCode(scanCode1)
                .WithBrandAssociation(testBrand.hierarchyClassID)
                .WithSubBrickAssociation(testMerch.hierarchyClassID)
                .WithTaxClassAssociation(testTax.hierarchyClassID)
                .WithNationalClassAssociation(testNational.hierarchyClassID); 
            Item item2 = new TestItemBuilder()
                .WithScanCode(scanCode2)
                .WithBrandAssociation(testBrand.hierarchyClassID)
                .WithSubBrickAssociation(testMerch.hierarchyClassID)
                .WithTaxClassAssociation(testTax.hierarchyClassID)
                .WithNationalClassAssociation(testNational.hierarchyClassID);
            Item item3 = new TestItemBuilder()
                .WithScanCode(scanCode3)
                .WithBrandAssociation(testBrand.hierarchyClassID)
                .WithSubBrickAssociation(testMerch.hierarchyClassID)
                .WithTaxClassAssociation(testTax.hierarchyClassID)
                .WithNationalClassAssociation(testNational.hierarchyClassID);
            Item item4 = new TestItemBuilder()
                .WithScanCode(scanCode4)
                .WithBrandAssociation(testBrand.hierarchyClassID)
                .WithSubBrickAssociation(testMerch.hierarchyClassID)
                .WithTaxClassAssociation(testTax.hierarchyClassID)
                .WithNationalClassAssociation(testNational.hierarchyClassID);

            context.Item.AddRange(new List<Item>
                {
                    item1,
                    item2,
                    item3,
                    item4
                });
            context.SaveChanges();

            parameters.Items = new List<BulkImportItemModel>
                {
                    new TestBulkImportItemModelBuilder().Empty().WithScanCode(scanCode1),
                    new TestBulkImportItemModelBuilder().Empty().WithScanCode(scanCode2),
                    new TestBulkImportItemModelBuilder().Empty().WithScanCode(scanCode3),
                    new TestBulkImportItemModelBuilder().Empty().WithScanCode(scanCode4)
                };

            //When
            var result = query.Search(parameters);

            //Then
            Assert.IsFalse(result.Any());
        }

        [TestMethod]
        public void GetScanCodesNotReadyToValidate_TwoItemsDoNotHaveAllRequiredCanonicalInformation_ShouldReturnTwoScanCodes()
        {
            //Given
            Item item1 = new TestItemBuilder()
                .WithScanCode(scanCode1)
                .WithBrandAssociation(testBrand.hierarchyClassID)
                .WithSubBrickAssociation(testMerch.hierarchyClassID)
                .WithTaxClassAssociation(testTax.hierarchyClassID)
                .WithNationalClassAssociation(testNational.hierarchyClassID);
            Item item2 = new TestItemBuilder()
                .WithScanCode(scanCode2);
            Item item3 = new TestItemBuilder()
                .WithScanCode(scanCode3);
            Item item4 = new TestItemBuilder()
                .WithScanCode(scanCode4);

            context.Item.AddRange(new List<Item>
                {
                    item1,
                    item2,
                    item3,
                    item4
                });
            context.SaveChanges();

            parameters.Items = new List<BulkImportItemModel>
                {
                    new TestBulkImportItemModelBuilder().Empty().WithScanCode(scanCode1),
                    new TestBulkImportItemModelBuilder().Empty().WithScanCode(scanCode2)
                        .WithHierarchyClassIds(testBrand.hierarchyClassID.ToString(), testMerch.hierarchyClassID.ToString(), testTax.hierarchyClassID.ToString(), null, testNational.hierarchyClassID.ToString()),
                    new TestBulkImportItemModelBuilder().Empty().WithScanCode(scanCode3),
                    new TestBulkImportItemModelBuilder().Empty().WithScanCode(scanCode4)
                };

            //When
            var result = query.Search(parameters);

            //Then
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Contains(scanCode3));
            Assert.IsTrue(result.Contains(scanCode4));
        }
    }
}
