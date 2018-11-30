using Icon.Framework;
using Icon.Logging;
using Icon.Testing.Builders;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Models;
using Icon.Web.Tests.Common.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;

namespace Icon.Web.Tests.Integration.Commands
{
    [TestClass] [Ignore]
    public class BulkImportNewItemCommandHandlerTests
    {
        private BulkImportNewItemCommandHandler commandHandler;
        private IconContext context;
        private DbContextTransaction transaction;
        private Mock<ILogger> mockLogger;
        private string userName;
        private string[] newScanCodes;
        private string newBrandName;
        private string testBrandName;
        private string testTaxName;
        private string testMerchandiseName;
        private string testBrowsingName;
        private string testFinancialName;
        private string testNationalClassName;
        private string testGlutenFreeAgencyName;
        private string testKosherAgencyName;
        private string testNonGmoAgencyName;
        private string testOrganicAgencyName;
        private string testVeganAgencyName;
        private HierarchyClass testBrand;
        private HierarchyClass testMerchandise;
        private HierarchyClass testTax;
        private HierarchyClass testBrowsing;
        private HierarchyClass testFinancial;
        private HierarchyClass testNational;
        private HierarchyClass testGlutenFree;
        private HierarchyClass testKosher;
        private HierarchyClass testNonGmo;
        private HierarchyClass testOrganic;
        private HierarchyClass testVegan;
        private HierarchyClassTrait merchFinMapping;
        private HierarchyClassTrait glutenFreeAgencyMapping;
        private HierarchyClassTrait kosherAgencyMapping;
        private HierarchyClassTrait nonGmoAgencyMapping;
        private HierarchyClassTrait organicAgencyMapping;
        private HierarchyClassTrait veganAgencyMapping;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            mockLogger = new Mock<ILogger>();

            commandHandler = new BulkImportNewItemCommandHandler(mockLogger.Object, context);

            newScanCodes = new string[3] { "8844667733", "5544886677", "2211334477" };
            testBrandName = "Update Item Integration Test Brand";
            testTaxName = "Update Item Edit Integration Test Tax";
            testMerchandiseName = "update item integration test merch";
            testFinancialName = "test (8000)";
            testBrowsingName = "TestBrowsing";
            newBrandName = "Brand New Brand";
            testNationalClassName = "NewItemImport Integration Test National ";

            testGlutenFreeAgencyName = "GF Agency";
            testKosherAgencyName = "KS Agency";
            testNonGmoAgencyName = "GMO Agency";
            testOrganicAgencyName = "OG Agency";
            testVeganAgencyName = "VG Agency";

            userName = "TestUser";

            testBrand = new TestHierarchyClassBuilder()
                .WithHierarchyId(Hierarchies.Brands)
                .WithHierarchyClassName(testBrandName)
                .WithHierarchyLevel(HierarchyLevels.Brand);

            testTax = new TestHierarchyClassBuilder()
                .WithHierarchyId(Hierarchies.Tax)
                .WithHierarchyClassName(testTaxName)
                .WithHierarchyLevel(HierarchyLevels.Tax);

            testMerchandise = new TestHierarchyClassBuilder()
                .WithHierarchyId(Hierarchies.Merchandise)
                .WithHierarchyClassName(testMerchandiseName)
                .WithHierarchyLevel(HierarchyLevels.SubBrick);

            testBrowsing = new TestHierarchyClassBuilder()
                .WithHierarchyId(Hierarchies.Browsing)
                .WithHierarchyClassName(testBrowsingName)
                .WithHierarchyLevel(HierarchyLevels.Parent);

            testFinancial = new TestHierarchyClassBuilder()
                .WithHierarchyId(Hierarchies.Financial)
                .WithHierarchyClassName(testFinancialName)
                .WithHierarchyLevel(HierarchyLevels.Financial);

            testNational = new TestHierarchyClassBuilder()
                .WithHierarchyId(Hierarchies.National)
                .WithHierarchyClassName(testNationalClassName)
                .WithHierarchyLevel(HierarchyLevels.NationalClass);

            testGlutenFree = new TestHierarchyClassBuilder()
                .WithHierarchyId(Hierarchies.CertificationAgencyManagement)
                .WithHierarchyClassName(testGlutenFreeAgencyName)
                .WithHierarchyLevel(HierarchyLevels.CertificationAgencyManagement);

            testKosher = new TestHierarchyClassBuilder()
                .WithHierarchyId(Hierarchies.CertificationAgencyManagement)
                .WithHierarchyClassName(testKosherAgencyName)
                .WithHierarchyLevel(HierarchyLevels.CertificationAgencyManagement);

            testNonGmo = new TestHierarchyClassBuilder()
                .WithHierarchyId(Hierarchies.CertificationAgencyManagement)
                .WithHierarchyClassName(testNonGmoAgencyName)
                .WithHierarchyLevel(HierarchyLevels.CertificationAgencyManagement);

            testOrganic = new TestHierarchyClassBuilder()
                .WithHierarchyId(Hierarchies.CertificationAgencyManagement)
                .WithHierarchyClassName(testOrganicAgencyName)
                .WithHierarchyLevel(HierarchyLevels.CertificationAgencyManagement);

            testVegan = new TestHierarchyClassBuilder()
                .WithHierarchyId(Hierarchies.CertificationAgencyManagement)
                .WithHierarchyClassName(testVeganAgencyName)
                .WithHierarchyLevel(HierarchyLevels.CertificationAgencyManagement);

            transaction = context.Database.BeginTransaction();

            context.HierarchyClass.AddRange(new List<HierarchyClass> 
                { 
                    testBrand, testMerchandise, testTax, testFinancial, testBrowsing, testNational, testGlutenFree, testKosher, testNonGmo, testOrganic, testVegan
                });

            context.SaveChanges();

            merchFinMapping = new TestHierarchyClassTraitBuilder()
                .WithHierarchyClassId(testMerchandise.hierarchyClassID)
                .WithTraitId(Traits.MerchFinMapping)
                .WithTraitValue(testFinancialName);

            glutenFreeAgencyMapping = new TestHierarchyClassTraitBuilder()
                .WithHierarchyClassId(testGlutenFree.hierarchyClassID)
                .WithTraitId(Traits.GlutenFree)
                .WithTraitValue("1");

            kosherAgencyMapping = new TestHierarchyClassTraitBuilder()
                .WithHierarchyClassId(testKosher.hierarchyClassID)
                .WithTraitId(Traits.Kosher)
                .WithTraitValue("1");

            nonGmoAgencyMapping = new TestHierarchyClassTraitBuilder()
                .WithHierarchyClassId(testNonGmo.hierarchyClassID)
                .WithTraitId(Traits.NonGmo)
                .WithTraitValue("1");

            organicAgencyMapping = new TestHierarchyClassTraitBuilder()
                .WithHierarchyClassId(testOrganic.hierarchyClassID)
                .WithTraitId(Traits.Organic)
                .WithTraitValue("1");

            veganAgencyMapping = new TestHierarchyClassTraitBuilder()
                .WithHierarchyClassId(testVegan.hierarchyClassID)
                .WithTraitId(Traits.Vegan)
                .WithTraitValue("1");

            context.HierarchyClassTrait.AddRange(new List<HierarchyClassTrait>
                { 
                    merchFinMapping, glutenFreeAgencyMapping, kosherAgencyMapping, nonGmoAgencyMapping, organicAgencyMapping, veganAgencyMapping
                });

            context.SaveChanges();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
        }

        [TestMethod]
        public void BulkImportNewItem_ItemWithAllData_ItemShouldBeCreatedWithAllNewInformation()
        {
            // Given.
            BulkImportNewItemModel newItemData = new TestBulkImportNewItemModelBuilder()
                .WithScanCode(newScanCodes[0])
                .WithBrandId(testBrand.hierarchyClassID.ToString())
                .WithMerchandiseId(testMerchandise.hierarchyClassID.ToString())
                .WithTaxId(testTax.hierarchyClassID.ToString())
                .WithBrowsingId(testBrowsing.hierarchyClassID.ToString())
                .WithNationalId(testNational.hierarchyClassID.ToString())
                .WithMsc("1");

            newItemData.AnimalWelfareRating = newItemData.AnimalWelfareRating;
            newItemData.CheeseAttributeMilkType = newItemData.CheeseAttributeMilkType;
            newItemData.EcoScaleRating = newItemData.EcoScaleRating;
            newItemData.SeafoodFreshOrFrozen =newItemData.SeafoodFreshOrFrozen;
            newItemData.SeafoodWildOrFarmRaised = newItemData.SeafoodWildOrFarmRaised;
            newItemData.GlutenFreeAgency = testGlutenFree.hierarchyClassID.ToString();
            newItemData.KosherAgency = testKosher.hierarchyClassID.ToString();
            newItemData.NonGmoAgency = testNonGmo.hierarchyClassID.ToString();
            newItemData.OrganicAgency = testOrganic.hierarchyClassID.ToString();
            newItemData.VeganAgency = testVegan.hierarchyClassID.ToString();

            var command = new BulkImportCommand<BulkImportNewItemModel>
            {
                BulkImportData = new List<BulkImportNewItemModel> { newItemData },
                UserName = userName
            };

            // When.
            commandHandler.Execute(command);

            // Then.

            // Core.
            int newItemId = context.ScanCode.Single(sc => sc.scanCode == newItemData.ScanCode).itemID;
            string scanCode = context.ScanCode.Single(sc => sc.itemID == newItemId).scanCode;

            // Item traits.
            string productDescription = context.ItemTrait.Single(it => it.Trait.traitCode == TraitCodes.ProductDescription && it.itemID == newItemId).traitValue;
            string posDescription = context.ItemTrait.Single(it => it.Trait.traitCode == TraitCodes.PosDescription && it.itemID == newItemId).traitValue;
            string packageUnit = context.ItemTrait.Single(it => it.Trait.traitCode == TraitCodes.PackageUnit && it.itemID == newItemId).traitValue;
            string foodStampEligible = context.ItemTrait.Single(it => it.Trait.traitCode == TraitCodes.FoodStampEligible && it.itemID == newItemId).traitValue;
            string posScaleTare = context.ItemTrait.Single(it => it.Trait.traitCode == TraitCodes.PosScaleTare && it.itemID == newItemId).traitValue;
            string retailSize = context.ItemTrait.Single(it => it.traitID == Traits.RetailSize && it.itemID == newItemId).traitValue;
            string retailUom = context.ItemTrait.Single(it => it.traitID == Traits.RetailUom && it.itemID == newItemId).traitValue;
            string deliverySystem = context.ItemTrait.Single(it => it.traitID == Traits.DeliverySystem && it.itemID == newItemId).traitValue;

            // Hierarchy associations.
            int brandId = context.ItemHierarchyClass.Single(ihc => ihc.itemID == newItemId && ihc.HierarchyClass.Hierarchy.hierarchyID == Hierarchies.Brands).hierarchyClassID;
            int merchandiseId = context.ItemHierarchyClass.Single(ihc => ihc.itemID == newItemId && ihc.HierarchyClass.Hierarchy.hierarchyID == Hierarchies.Merchandise).hierarchyClassID;
            int taxId = context.ItemHierarchyClass.Single(ihc => ihc.itemID == newItemId && ihc.HierarchyClass.Hierarchy.hierarchyID == Hierarchies.Tax).hierarchyClassID;
            int nationalId = context.ItemHierarchyClass.Single(ihc => ihc.itemID == newItemId && ihc.HierarchyClass.Hierarchy.hierarchyID == Hierarchies.National).hierarchyClassID;
            int browsingId = context.ItemHierarchyClass.Single(ihc => ihc.itemID == newItemId && ihc.HierarchyClass.Hierarchy.hierarchyID == Hierarchies.Browsing).hierarchyClassID;

            // Sign Attributes.
            var signAttributes = context.ItemSignAttribute.Single(isa => isa.ItemID == newItemId);
            string animalWelfareRating = signAttributes.AnimalWelfareRating;
            bool biodynamic = signAttributes.Biodynamic;
            string cheeseMilkType = signAttributes.MilkType;
            bool cheeseRaw = signAttributes.CheeseRaw;
            string ecoScaleRating = signAttributes.EcoScaleRating;
            string glutenFree= signAttributes.GlutenFreeAgencyName;
            string kosher = signAttributes.KosherAgencyName;
            string nonGmo = signAttributes.NonGmoAgencyName;
            string organic = signAttributes.OrganicAgencyName;
            bool premiumBodyCare = signAttributes.PremiumBodyCare;
            string seafoodFreshOrFrozen = signAttributes.FreshOrFrozen;
            string seafoodCatchType = signAttributes.SeafoodCatchType;
            string vegan = signAttributes.VeganAgencyName;
            bool vegetarian = signAttributes.Vegetarian;
            bool wholeTrade = signAttributes.WholeTrade;
            bool msc = signAttributes.Msc;

            // Auditing.
            string insertedDate = context.ItemTrait.Single(it => it.Trait.traitCode == TraitCodes.InsertDate && it.itemID == newItemId).traitValue;
            string modifiedDate = context.ItemTrait.Single(it => it.Trait.traitCode == TraitCodes.ModifiedDate && it.itemID == newItemId).traitValue;
            string modifiedUser = context.ItemTrait.Single(it => it.Trait.traitCode == TraitCodes.ModifiedUser && it.itemID == newItemId).traitValue;

            Assert.AreEqual(newItemData.ScanCode, scanCode);
            Assert.AreEqual(newItemData.ProductDescription, productDescription);
            Assert.AreEqual(newItemData.PosDescription, posDescription);
            Assert.AreEqual(newItemData.PackageUnit, packageUnit);
            Assert.AreEqual(newItemData.FoodStampEligible, foodStampEligible);
            Assert.AreEqual(newItemData.PosScaleTare, posScaleTare);
            Assert.AreEqual(newItemData.RetailSize, retailSize);
            Assert.AreEqual(newItemData.RetailUom, retailUom);
            Assert.AreEqual(newItemData.DeliverySystem, deliverySystem);
            Assert.AreEqual(newItemData.BrandId, brandId.ToString());
            Assert.AreEqual(newItemData.MerchandiseId, merchandiseId.ToString());
            Assert.AreEqual(newItemData.TaxId, taxId.ToString());
            Assert.AreEqual(newItemData.NationalId, nationalId.ToString());
            Assert.AreEqual(newItemData.BrowsingId, browsingId.ToString());
            Assert.AreEqual(newItemData.AnimalWelfareRating, animalWelfareRating);
            Assert.IsTrue(biodynamic);
            Assert.AreEqual(newItemData.CheeseAttributeMilkType, cheeseMilkType);
            Assert.IsFalse(cheeseRaw);
            Assert.AreEqual(newItemData.EcoScaleRating, ecoScaleRating);
            Assert.AreEqual(newItemData.GlutenFreeAgency, glutenFree.ToString());
            Assert.AreEqual(newItemData.KosherAgency, kosher.ToString());
            Assert.AreEqual(newItemData.NonGmoAgency, nonGmo.ToString());
            Assert.AreEqual(newItemData.OrganicAgency, organic.ToString());
            Assert.IsTrue(premiumBodyCare);
            Assert.AreEqual(newItemData.SeafoodFreshOrFrozen, seafoodFreshOrFrozen);
            Assert.AreEqual(newItemData.SeafoodWildOrFarmRaised, seafoodCatchType);
            Assert.AreEqual(newItemData.VeganAgency, vegan);
            Assert.IsTrue(vegetarian);
            Assert.IsFalse(wholeTrade);
            Assert.AreEqual(DateTime.Today.Date, DateTime.Parse(insertedDate).Date);
            Assert.AreEqual(null, modifiedDate);
            Assert.AreEqual(userName, modifiedUser);
            Assert.IsTrue(msc);
            //The following true/false fields are imported as empty strings, and null value should be inserted into these colunns.  
            Assert.IsFalse(signAttributes.GrassFed);
            Assert.IsFalse(signAttributes.PastureRaised);
            Assert.IsFalse(signAttributes.FreeRange);
            Assert.IsFalse(signAttributes.DryAged);
            Assert.IsFalse(signAttributes.AirChilled);
            Assert.IsFalse(signAttributes.MadeInHouse);
        }

        [TestMethod]
        public void BulkImportNewItem_ItemBelongsToCrvSubBrick_ItemShouldBeCreatedWithDepositItemType()
        {
            // Given.
            var nonMerchandisePattern = context.Trait.Single(t => t.traitCode == TraitCodes.NonMerchandise).traitPattern;
            string nonMerchandiseTraitValue = NonMerchandiseTraits.Crv;

            if (!Regex.IsMatch(nonMerchandiseTraitValue, nonMerchandisePattern))
            {
                Assert.Fail("Not a valid non-merchandise trait.");
            }

            HierarchyClassTrait nonMerchandiseTrait = new TestHierarchyClassTraitBuilder()
                .WithTraitId(Traits.NonMerchandise)
                .WithHierarchyClassId(testMerchandise.hierarchyClassID)
                .WithTraitValue(nonMerchandiseTraitValue);

            context.HierarchyClassTrait.Add(nonMerchandiseTrait);
            context.SaveChanges();

            BulkImportNewItemModel newItemData = new TestBulkImportNewItemModelBuilder()
                .WithScanCode(newScanCodes[0])
                .WithBrandId(testBrand.hierarchyClassID.ToString())
                .WithMerchandiseId(testMerchandise.hierarchyClassID.ToString())
                .WithTaxId(testTax.hierarchyClassID.ToString())
                .WithBrowsingId(testBrowsing.hierarchyClassID.ToString())
                .WithNationalId(testNational.hierarchyClassID.ToString());

            var command = new BulkImportCommand<BulkImportNewItemModel>
            {
                BulkImportData = new List<BulkImportNewItemModel> { newItemData },
                UserName = userName
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            int newItemId = context.ScanCode.Single(sc => sc.scanCode == newItemData.ScanCode).itemID;
            string newItemTypeCode = context.Item.Single(i => i.itemID == newItemId).ItemType.itemTypeCode;

            Assert.AreEqual(ItemTypeCodes.Deposit, newItemTypeCode);
        }

        [TestMethod]
        public void BulkImportNewItem_ItemBelongsToBottleDepositSubBrick_ItemShouldBeCreatedWithDepositItemType()
        {
            // Given.
            var nonMerchandisePattern = context.Trait.Single(t => t.traitCode == TraitCodes.NonMerchandise).traitPattern;
            string nonMerchandiseTraitValue = NonMerchandiseTraits.BottleDeposit;

            if (!Regex.IsMatch(nonMerchandiseTraitValue, nonMerchandisePattern))
            {
                Assert.Fail("Not a valid non-merchandise trait.");
            }

            HierarchyClassTrait nonMerchandiseTrait = new TestHierarchyClassTraitBuilder()
                .WithTraitId(Traits.NonMerchandise)
                .WithHierarchyClassId(testMerchandise.hierarchyClassID)
                .WithTraitValue(nonMerchandiseTraitValue);

            context.HierarchyClassTrait.Add(nonMerchandiseTrait);
            context.SaveChanges();

            BulkImportNewItemModel newItemData = new TestBulkImportNewItemModelBuilder()
                .WithScanCode(newScanCodes[0])
                .WithBrandId(testBrand.hierarchyClassID.ToString())
                .WithMerchandiseId(testMerchandise.hierarchyClassID.ToString())
                .WithTaxId(testTax.hierarchyClassID.ToString())
                .WithBrowsingId(testBrowsing.hierarchyClassID.ToString())
                .WithNationalId(testNational.hierarchyClassID.ToString());

            var command = new BulkImportCommand<BulkImportNewItemModel>
            {
                BulkImportData = new List<BulkImportNewItemModel> { newItemData },
                UserName = userName
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            int newItemId = context.ScanCode.Single(sc => sc.scanCode == newItemData.ScanCode).itemID;
            string newItemTypeCode = context.Item.Single(i => i.itemID == newItemId).ItemType.itemTypeCode;

            Assert.AreEqual(ItemTypeCodes.Deposit, newItemTypeCode);
        }

        [TestMethod]
        public void BulkImportNewItem_ItemBelongsToBottleReturnSubBrick_ItemShouldBeCreatedWithReturnItemType()
        {
            // Given.
            var nonMerchandisePattern = context.Trait.Single(t => t.traitCode == TraitCodes.NonMerchandise).traitPattern;
            string nonMerchandiseTraitValue = NonMerchandiseTraits.BottleReturn;

            if (!Regex.IsMatch(nonMerchandiseTraitValue, nonMerchandisePattern))
            {
                Assert.Fail("Not a valid non-merchandise trait.");
            }

            HierarchyClassTrait nonMerchandiseTrait = new TestHierarchyClassTraitBuilder()
                .WithTraitId(Traits.NonMerchandise)
                .WithHierarchyClassId(testMerchandise.hierarchyClassID)
                .WithTraitValue(nonMerchandiseTraitValue);

            context.HierarchyClassTrait.Add(nonMerchandiseTrait);
            context.SaveChanges();

            BulkImportNewItemModel newItemData = new TestBulkImportNewItemModelBuilder()
                .WithScanCode(newScanCodes[0])
                .WithBrandId(testBrand.hierarchyClassID.ToString())
                .WithMerchandiseId(testMerchandise.hierarchyClassID.ToString())
                .WithTaxId(testTax.hierarchyClassID.ToString())
                .WithBrowsingId(testBrowsing.hierarchyClassID.ToString())
                .WithNationalId(testNational.hierarchyClassID.ToString());

            var command = new BulkImportCommand<BulkImportNewItemModel>
            {
                BulkImportData = new List<BulkImportNewItemModel> { newItemData },
                UserName = userName
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            int newItemId = context.ScanCode.Single(sc => sc.scanCode == newItemData.ScanCode).itemID;
            string newItemTypeCode = context.Item.Single(i => i.itemID == newItemId).ItemType.itemTypeCode;

            Assert.AreEqual(ItemTypeCodes.Return, newItemTypeCode);
        }

        [TestMethod]
        public void BulkImportNewItem_ItemBelongsToCrvCreditSubBrick_ItemShouldBeCreatedWithReturnItemType()
        {
            // Given.
            var nonMerchandisePattern = context.Trait.Single(t => t.traitCode == TraitCodes.NonMerchandise).traitPattern;
            string nonMerchandiseTraitValue = NonMerchandiseTraits.CrvCredit;

            if (!Regex.IsMatch(nonMerchandiseTraitValue, nonMerchandisePattern))
            {
                Assert.Fail("Not a valid non-merchandise trait.");
            }

            HierarchyClassTrait nonMerchandiseTrait = new TestHierarchyClassTraitBuilder()
                .WithTraitId(Traits.NonMerchandise)
                .WithHierarchyClassId(testMerchandise.hierarchyClassID)
                .WithTraitValue(nonMerchandiseTraitValue);

            context.HierarchyClassTrait.Add(nonMerchandiseTrait);
            context.SaveChanges();

            BulkImportNewItemModel newItemData = new TestBulkImportNewItemModelBuilder()
                .WithScanCode(newScanCodes[0])
                .WithBrandId(testBrand.hierarchyClassID.ToString())
                .WithMerchandiseId(testMerchandise.hierarchyClassID.ToString())
                .WithTaxId(testTax.hierarchyClassID.ToString())
                .WithBrowsingId(testBrowsing.hierarchyClassID.ToString())
                .WithNationalId(testNational.hierarchyClassID.ToString());

            var command = new BulkImportCommand<BulkImportNewItemModel>
            {
                BulkImportData = new List<BulkImportNewItemModel> { newItemData },
                UserName = userName
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            int newItemId = context.ScanCode.Single(sc => sc.scanCode == newItemData.ScanCode).itemID;
            string newItemTypeCode = context.Item.Single(i => i.itemID == newItemId).ItemType.itemTypeCode;

            Assert.AreEqual(ItemTypeCodes.Return, newItemTypeCode);
        }

        [TestMethod]
        public void BulkImportNewItem_ItemBelongsToCouponSubBrick_ItemShouldBeCreatedWithCouponItemType()
        {
            // Given.
            var nonMerchandisePattern = context.Trait.Single(t => t.traitCode == TraitCodes.NonMerchandise).traitPattern;
            string nonMerchandiseTraitValue = NonMerchandiseTraits.Coupon;

            if (!Regex.IsMatch(nonMerchandiseTraitValue, nonMerchandisePattern))
            {
                Assert.Fail("Not a valid non-merchandise trait.");
            }

            HierarchyClassTrait nonMerchandiseTrait = new TestHierarchyClassTraitBuilder()
                .WithTraitId(Traits.NonMerchandise)
                .WithHierarchyClassId(testMerchandise.hierarchyClassID)
                .WithTraitValue(nonMerchandiseTraitValue);

            context.HierarchyClassTrait.Add(nonMerchandiseTrait);
            context.SaveChanges();

            BulkImportNewItemModel newItemData = new TestBulkImportNewItemModelBuilder()
                .WithScanCode(newScanCodes[0])
                .WithBrandId(testBrand.hierarchyClassID.ToString())
                .WithMerchandiseId(testMerchandise.hierarchyClassID.ToString())
                .WithTaxId(testTax.hierarchyClassID.ToString())
                .WithBrowsingId(testBrowsing.hierarchyClassID.ToString())
                .WithNationalId(testNational.hierarchyClassID.ToString());

            var command = new BulkImportCommand<BulkImportNewItemModel>
            {
                BulkImportData = new List<BulkImportNewItemModel> { newItemData },
                UserName = userName
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            int newItemId = context.ScanCode.Single(sc => sc.scanCode == newItemData.ScanCode).itemID;
            string newItemTypeCode = context.Item.Single(i => i.itemID == newItemId).ItemType.itemTypeCode;

            Assert.AreEqual(ItemTypeCodes.Coupon, newItemTypeCode);
        }

        [TestMethod]
        public void BulkImportNewItem_ItemBelongsToNonRetailSubBrick_ItemShouldBeCreatedWithNonRetailItemType()
        {
            // Given.
            var nonMerchandisePattern = context.Trait.Single(t => t.traitCode == TraitCodes.NonMerchandise).traitPattern;
            string nonMerchandiseTraitValue = NonMerchandiseTraits.LegacyPosOnly;

            if (!Regex.IsMatch(nonMerchandiseTraitValue, nonMerchandisePattern))
            {
                Assert.Fail("Not a valid non-merchandise trait.");
            }

            HierarchyClassTrait nonMerchandiseTrait = new TestHierarchyClassTraitBuilder()
                .WithTraitId(Traits.NonMerchandise)
                .WithHierarchyClassId(testMerchandise.hierarchyClassID)
                .WithTraitValue(nonMerchandiseTraitValue);

            context.HierarchyClassTrait.Add(nonMerchandiseTrait);
            context.SaveChanges();

            BulkImportNewItemModel newItemData = new TestBulkImportNewItemModelBuilder()
                .WithScanCode(newScanCodes[0])
                .WithBrandId(testBrand.hierarchyClassID.ToString())
                .WithMerchandiseId(testMerchandise.hierarchyClassID.ToString())
                .WithTaxId(testTax.hierarchyClassID.ToString())
                .WithBrowsingId(testBrowsing.hierarchyClassID.ToString())
                .WithNationalId(testNational.hierarchyClassID.ToString());

            var command = new BulkImportCommand<BulkImportNewItemModel>
            {
                BulkImportData = new List<BulkImportNewItemModel> { newItemData },
                UserName = userName
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            int newItemId = context.ScanCode.Single(sc => sc.scanCode == newItemData.ScanCode).itemID;
            string newItemTypeCode = context.Item.Single(i => i.itemID == newItemId).ItemType.itemTypeCode;

            Assert.AreEqual(ItemTypeCodes.NonRetail, newItemTypeCode);
        }

        [TestMethod]
        public void BulkImportNewItem_ItemIsCreatedWithNoMerchandiseHierarchyAssociation_ItemShouldBeCreatedWithRetailSaleItemType()
        {
            // Given.
            BulkImportNewItemModel newItemData = new TestBulkImportNewItemModelBuilder()
                .WithScanCode(newScanCodes[0])
                .WithBrandId(testBrand.hierarchyClassID.ToString())
                .WithMerchandiseId(String.Empty)
                .WithTaxId(testTax.hierarchyClassID.ToString())
                .WithBrowsingId(testBrowsing.hierarchyClassID.ToString())
                .WithNationalId(testNational.hierarchyClassID.ToString());

            var command = new BulkImportCommand<BulkImportNewItemModel>
            {
                BulkImportData = new List<BulkImportNewItemModel> { newItemData },
                UserName = userName
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            int newItemId = context.ScanCode.Single(sc => sc.scanCode == newItemData.ScanCode).itemID;
            string newItemTypeCode = context.Item.Single(i => i.itemID == newItemId).ItemType.itemTypeCode;

            Assert.AreEqual(ItemTypeCodes.RetailSale, newItemTypeCode);
        }

        [TestMethod]
        public void BulkImportNewItem_ItemBelongsToBlackhawkSubBrick_ItemShouldBeCreatedWithFeeItemType()
        {
            // Given.
            var nonMerchandisePattern = context.Trait.Single(t => t.traitCode == TraitCodes.NonMerchandise).traitPattern;
            string nonMerchandiseTraitValue = NonMerchandiseTraits.BlackhawkFee;

            if (!Regex.IsMatch(nonMerchandiseTraitValue, nonMerchandisePattern))
            {
                Assert.Fail("Not a valid non-merchandise trait.");
            }

            HierarchyClassTrait nonMerchandiseTrait = new TestHierarchyClassTraitBuilder()
                .WithTraitId(Traits.NonMerchandise)
                .WithHierarchyClassId(testMerchandise.hierarchyClassID)
                .WithTraitValue(nonMerchandiseTraitValue);

            context.HierarchyClassTrait.Add(nonMerchandiseTrait);
            context.SaveChanges();

            BulkImportNewItemModel newItemData = new TestBulkImportNewItemModelBuilder()
                .WithScanCode(newScanCodes[0])
                .WithBrandId(testBrand.hierarchyClassID.ToString())
                .WithMerchandiseId(testMerchandise.hierarchyClassID.ToString())
                .WithTaxId(testTax.hierarchyClassID.ToString())
                .WithBrowsingId(testBrowsing.hierarchyClassID.ToString())
                .WithNationalId(testNational.hierarchyClassID.ToString());

            var command = new BulkImportCommand<BulkImportNewItemModel>
            {
                BulkImportData = new List<BulkImportNewItemModel> { newItemData },
                UserName = userName
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            int newItemId = context.ScanCode.Single(sc => sc.scanCode == newItemData.ScanCode).itemID;
            string newItemTypeCode = context.Item.Single(i => i.itemID == newItemId).ItemType.itemTypeCode;

            Assert.AreEqual(ItemTypeCodes.Fee, newItemTypeCode);
        }

        [TestMethod]
        public void BulkImportNewItem_ItemBelongsToNonRetailSubBrick_NoProductMessageShouldBeGenerated()
        {
            // Given.
            var nonMerchandisePattern = context.Trait.Single(t => t.traitCode == TraitCodes.NonMerchandise).traitPattern;
            string nonMerchandiseTraitValue = NonMerchandiseTraits.LegacyPosOnly;

            if (!Regex.IsMatch(nonMerchandiseTraitValue, nonMerchandisePattern))
            {
                Assert.Fail("Not a valid non-merchandise trait.");
            }

            HierarchyClassTrait nonMerchandiseTrait = new TestHierarchyClassTraitBuilder()
                .WithTraitId(Traits.NonMerchandise)
                .WithHierarchyClassId(testMerchandise.hierarchyClassID)
                .WithTraitValue(nonMerchandiseTraitValue);

            context.HierarchyClassTrait.Add(nonMerchandiseTrait);
            context.SaveChanges();

            BulkImportNewItemModel newItemData = new TestBulkImportNewItemModelBuilder()
                .WithScanCode(newScanCodes[0])
                .WithBrandId(testBrand.hierarchyClassID.ToString())
                .WithMerchandiseId(testMerchandise.hierarchyClassID.ToString())
                .WithTaxId(testTax.hierarchyClassID.ToString())
                .WithBrowsingId(testBrowsing.hierarchyClassID.ToString())
                .WithNationalId(testNational.hierarchyClassID.ToString());

            var command = new BulkImportCommand<BulkImportNewItemModel>
            {
                BulkImportData = new List<BulkImportNewItemModel> { newItemData },
                UserName = userName
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            int newItemId = context.ScanCode.Single(sc => sc.scanCode == newItemData.ScanCode).itemID;
            var message = context.MessageQueueProduct.SingleOrDefault(q => q.ItemId == newItemId);

            Assert.IsTrue(message == null, "A message was created when it should not have been.");
        }

        [TestMethod]
        public void BulkImportNewItem_ItemBelongsToCouponSubBrick_NoProductMessageShouldBeGenerated()
        {
            // Given.
            var nonMerchandisePattern = context.Trait.Single(t => t.traitCode == TraitCodes.NonMerchandise).traitPattern;
            string nonMerchandiseTraitValue = NonMerchandiseTraits.Coupon;

            if (!Regex.IsMatch(nonMerchandiseTraitValue, nonMerchandisePattern))
            {
                Assert.Fail("Not a valid non-merchandise trait.");
            }

            HierarchyClassTrait nonMerchandiseTrait = new TestHierarchyClassTraitBuilder()
                .WithTraitId(Traits.NonMerchandise)
                .WithHierarchyClassId(testMerchandise.hierarchyClassID)
                .WithTraitValue(nonMerchandiseTraitValue);

            context.HierarchyClassTrait.Add(nonMerchandiseTrait);
            context.SaveChanges();

            BulkImportNewItemModel newItemData = new TestBulkImportNewItemModelBuilder()
                .WithScanCode(newScanCodes[0])
                .WithBrandId(testBrand.hierarchyClassID.ToString())
                .WithMerchandiseId(testMerchandise.hierarchyClassID.ToString())
                .WithTaxId(testTax.hierarchyClassID.ToString())
                .WithBrowsingId(testBrowsing.hierarchyClassID.ToString())
                .WithNationalId(testNational.hierarchyClassID.ToString());

            var command = new BulkImportCommand<BulkImportNewItemModel>
            {
                BulkImportData = new List<BulkImportNewItemModel> { newItemData },
                UserName = userName
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            int newItemId = context.ScanCode.Single(sc => sc.scanCode == newItemData.ScanCode).itemID;
            var message = context.MessageQueueProduct.SingleOrDefault(q => q.ItemId == newItemId);

            Assert.IsTrue(message == null, "A message was created when it should not have been.");
        }

        [TestMethod]
        public void BulkImportNewItem_ItemWithAllData_BlankBrandShouldNotBeCreatedAfterSuccessfulUpload()
        {
            //Precondition
            var blankBrands = context.HierarchyClass.Where(hc => hc.hierarchyID == Hierarchies.Brands && (hc.hierarchyClassName == String.Empty || hc.hierarchyClassName == String.Empty)).ToList();
            Assert.AreEqual(0, blankBrands.Count, "Hierarchy Class with blank name already exists.");

            // Given.
            BulkImportNewItemModel newItemData = new TestBulkImportNewItemModelBuilder()
                .WithScanCode(newScanCodes[0])
                .WithBrandId(testBrand.hierarchyClassID.ToString())
                .WithMerchandiseId(testMerchandise.hierarchyClassID.ToString())
                .WithTaxId(testTax.hierarchyClassID.ToString())
                .WithBrowsingId(testBrowsing.hierarchyClassID.ToString())
                .WithNationalId(testNational.hierarchyClassID.ToString());

            var command = new BulkImportCommand<BulkImportNewItemModel>
            {
                BulkImportData = new List<BulkImportNewItemModel> { newItemData },
                UserName = userName
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            blankBrands = context.HierarchyClass.Where(hc => hc.hierarchyID == Hierarchies.Brands && hc.hierarchyClassName == String.Empty).ToList();

            Assert.AreEqual(0, blankBrands.Count);
        }

        [TestMethod]
        public void BulkImportNewItem_ItemWithMissingPosScaleTare_PosScaleTareShouldDefaultToZero()
        {
            // Given.
            BulkImportNewItemModel newItemData = new TestBulkImportNewItemModelBuilder()
                .WithScanCode(newScanCodes[0])
                .WithBrandId(testBrand.hierarchyClassID.ToString())
                .WithMerchandiseId(testMerchandise.hierarchyClassID.ToString())
                .WithTaxId(testTax.hierarchyClassID.ToString())
                .WithBrowsingId(testBrowsing.hierarchyClassID.ToString())
                .WithNationalId(testNational.hierarchyClassID.ToString())
                .WithPosScaleTare(String.Empty);

            var command = new BulkImportCommand<BulkImportNewItemModel>
            {
                BulkImportData = new List<BulkImportNewItemModel> { newItemData },
                UserName = userName
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            int newItemId = context.ScanCode.Single(sc => sc.scanCode == newItemData.ScanCode).itemID;
            string posScaleTare = context.ItemTrait.Single(it => it.Trait.traitCode == TraitCodes.PosScaleTare && it.itemID == newItemId).traitValue;

            Assert.AreEqual("0", posScaleTare);
        }

        [TestMethod]
        public void BulkImportNewItem_ItemWithMissingMerchandiseOrTaxOrBrowsing_MissingItemHierarchyAssociationsShouldNotBeCreated()
        {
            // Given.
            BulkImportNewItemModel newItemData = new TestBulkImportNewItemModelBuilder()
                .WithScanCode(newScanCodes[0])
                .WithBrandId(testBrand.hierarchyClassID.ToString())
                .WithMerchandiseId(String.Empty)
                .WithTaxId(String.Empty)
                .WithBrowsingId(String.Empty)
                .WithNationalId(string.Empty);

            var command = new BulkImportCommand<BulkImportNewItemModel>
            {
                BulkImportData = new List<BulkImportNewItemModel> { newItemData },
                UserName = userName
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            int newItemId = context.ScanCode.Single(sc => sc.scanCode == newItemData.ScanCode).itemID;
            var itemMerchandiseAssociation = context.ItemHierarchyClass.Where(ihc => ihc.itemID == newItemId && ihc.HierarchyClass.Hierarchy.hierarchyID == Hierarchies.Merchandise);
            var itemTaxAssociation = context.ItemHierarchyClass.Where(ihc => ihc.itemID == newItemId && ihc.HierarchyClass.Hierarchy.hierarchyID == Hierarchies.Tax);
            var itemBrowsingAssociation = context.ItemHierarchyClass.Where(ihc => ihc.itemID == newItemId && ihc.HierarchyClass.Hierarchy.hierarchyID == Hierarchies.Browsing);

            Assert.IsFalse(itemMerchandiseAssociation.Any());
            Assert.IsFalse(itemTaxAssociation.Any());
            Assert.IsFalse(itemBrowsingAssociation.Any());
        }

        [TestMethod]
        public void BulkImportNewItem_ItemIsUploadedWithNewBrand_NewBrandShouldBeCreatedAndAssociatedToTheNewItem()
        {
            // Given.
            BulkImportNewItemModel newItemData = new TestBulkImportNewItemModelBuilder()
                .WithScanCode(newScanCodes[0])
                .WithBrandId("0")
                .WithMerchandiseId(testMerchandise.hierarchyClassID.ToString())
                .WithTaxId(testTax.hierarchyClassID.ToString())
                .WithBrowsingId(testBrowsing.hierarchyClassID.ToString())
                .WithNationalId(testNational.hierarchyClassID.ToString())
                .WithBrandName(newBrandName);

            var command = new BulkImportCommand<BulkImportNewItemModel>
            {
                BulkImportData = new List<BulkImportNewItemModel> { newItemData },
                UserName = userName
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            var newBrand = context.HierarchyClass.Single(hc => hc.hierarchyClassName == newBrandName);
            int newItemId = context.ScanCode.Single(sc => sc.scanCode == newItemData.ScanCode).itemID;
            var itemBrandAssociation = context.ItemHierarchyClass.Single(ihc => ihc.itemID == newItemId && ihc.HierarchyClass.Hierarchy.hierarchyID == Hierarchies.Brands);

            Assert.AreEqual(newBrand.hierarchyClassID, itemBrandAssociation.hierarchyClassID);
        }

        [TestMethod]
        public void BulkImportNewItem_TwoItemsUploadedWithNewBrand_NewBrandShouldBeCreatedAndAssociatedToTheNewItems()
        {
            // Given.
            BulkImportNewItemModel firstItemData = new TestBulkImportNewItemModelBuilder()
                .WithScanCode(newScanCodes[0])
                .WithBrandId("0")
                .WithMerchandiseId(testMerchandise.hierarchyClassID.ToString())
                .WithTaxId(testTax.hierarchyClassID.ToString())
                .WithBrowsingId(testBrowsing.hierarchyClassID.ToString())
                .WithBrandName(newBrandName)
                .WithNationalId(testNational.hierarchyClassID.ToString());

            BulkImportNewItemModel secondItemData = new TestBulkImportNewItemModelBuilder()
                .WithScanCode(newScanCodes[1])
                .WithBrandId("0")
                .WithMerchandiseId(testMerchandise.hierarchyClassID.ToString())
                .WithTaxId(testTax.hierarchyClassID.ToString())
                .WithBrowsingId(testBrowsing.hierarchyClassID.ToString())
                .WithBrandName(newBrandName)
                .WithNationalId(testNational.hierarchyClassID.ToString());

            var command = new BulkImportCommand<BulkImportNewItemModel>
            {
                BulkImportData = new List<BulkImportNewItemModel> { firstItemData, secondItemData },
                UserName = userName
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            var newBrand = context.HierarchyClass.Single(hc => hc.hierarchyClassName == newBrandName);

            int firstItemId = context.ScanCode.Single(sc => sc.scanCode == firstItemData.ScanCode).itemID;
            var firstItemBrandAssociation = context.ItemHierarchyClass.Single(ihc => ihc.itemID == firstItemId && ihc.HierarchyClass.Hierarchy.hierarchyID == Hierarchies.Brands);

            int secondItemId = context.ScanCode.Single(sc => sc.scanCode == secondItemData.ScanCode).itemID;
            var secondItemBrandAssociation = context.ItemHierarchyClass.Single(ihc => ihc.itemID == secondItemId && ihc.HierarchyClass.Hierarchy.hierarchyID == Hierarchies.Brands);

            Assert.AreEqual(newBrand.hierarchyClassID, firstItemBrandAssociation.hierarchyClassID);
            Assert.AreEqual(newBrand.hierarchyClassID, secondItemBrandAssociation.hierarchyClassID);
        }

        [TestMethod]
        public void BulkImportNewItem_ItemIsUploadedWithNewBrand_NewBrandShouldHaveSentToEsbTraitWithNullValue()
        {
            // Given.
            BulkImportNewItemModel newItemData = new TestBulkImportNewItemModelBuilder()
                .WithScanCode(newScanCodes[0])
                .WithBrandId("0")
                .WithMerchandiseId(testMerchandise.hierarchyClassID.ToString())
                .WithTaxId(testTax.hierarchyClassID.ToString())
                .WithBrowsingId(testBrowsing.hierarchyClassID.ToString())
                .WithBrandName(newBrandName)
                .WithNationalId(testNational.hierarchyClassID.ToString());

            var command = new BulkImportCommand<BulkImportNewItemModel>
            {
                BulkImportData = new List<BulkImportNewItemModel> { newItemData },
                UserName = userName
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            var newBrand = context.HierarchyClass.Single(hc => hc.hierarchyClassName == newBrandName);

            var sentToEsbTrait = context.HierarchyClassTrait.Single(hct => hct.Trait.traitCode == TraitCodes.SentToEsb && hct.hierarchyClassID == newBrand.hierarchyClassID);

            Assert.IsNull(sentToEsbTrait.traitValue);
            Assert.AreEqual(Traits.SentToEsb, sentToEsbTrait.traitID);
        }

        [TestMethod]
        public void BulkImportNewItem_ItemIsUploadedWithNewBrand_HierarchyMessageShouldBeGeneratedForTheNewBrand()
        {
            // Given.
            BulkImportNewItemModel newItemData = new TestBulkImportNewItemModelBuilder()
                .WithScanCode(newScanCodes[0])
                .WithBrandId("0")
                .WithMerchandiseId(testMerchandise.hierarchyClassID.ToString())
                .WithTaxId(testTax.hierarchyClassID.ToString())
                .WithBrowsingId(testBrowsing.hierarchyClassID.ToString())
                .WithBrandName(newBrandName)
                .WithNationalId(testNational.hierarchyClassID.ToString());

            var command = new BulkImportCommand<BulkImportNewItemModel>
            {
                BulkImportData = new List<BulkImportNewItemModel> { newItemData },
                UserName = userName
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            var newBrand = context.HierarchyClass.Single(hc => hc.hierarchyClassName == newBrandName);
            var message = context.MessageQueueHierarchy.Single(mq => mq.HierarchyClassName == newBrandName);

            Assert.AreEqual(newBrandName, message.HierarchyClassName);
            Assert.AreEqual(MessageTypes.Hierarchy, message.MessageTypeId);
            Assert.AreEqual(HierarchyNames.Brands, message.HierarchyName);
        }

        [TestMethod]
        public void BulkImportNewItem_ItemIsUploadedWithExistingBrandNameButBrandIdOfZero_ItemShouldBeAssignedToExistingBrand()
        {
            // Given.
            BulkImportNewItemModel newItemData = new TestBulkImportNewItemModelBuilder()
                .WithScanCode(newScanCodes[0])
                .WithBrandId("0")
                .WithMerchandiseId(testMerchandise.hierarchyClassID.ToString())
                .WithTaxId(testTax.hierarchyClassID.ToString())
                .WithBrowsingId(testBrowsing.hierarchyClassID.ToString())
                .WithBrandName(testBrand.hierarchyClassName)
                .WithNationalId(testNational.hierarchyClassID.ToString());

            var command = new BulkImportCommand<BulkImportNewItemModel>
            {
                BulkImportData = new List<BulkImportNewItemModel> { newItemData },
                UserName = userName
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            var newItemId = context.ScanCode.Single(sc => sc.scanCode == newItemData.ScanCode).itemID;
            var itemBrandAssociation = context.ItemHierarchyClass.Single(ihc => ihc.itemID == newItemId && ihc.HierarchyClass.Hierarchy.hierarchyID == Hierarchies.Brands);

            Assert.AreEqual(testBrand.hierarchyClassName, itemBrandAssociation.HierarchyClass.hierarchyClassName);
        }

        [TestMethod]
        public void BulkImportNewItem_TwoItemsUploadedWithExistingBrandNameButBrandIdOfZero_ItemsShouldBeAssignedToExistingBrand()
        {
            // Given.
            BulkImportNewItemModel firstItemData = new TestBulkImportNewItemModelBuilder()
                .WithScanCode(newScanCodes[0])
                .WithBrandId("0")
                .WithMerchandiseId(testMerchandise.hierarchyClassID.ToString())
                .WithTaxId(testTax.hierarchyClassID.ToString())
                .WithBrowsingId(testBrowsing.hierarchyClassID.ToString())
                .WithBrandName(testBrand.hierarchyClassName)
                .WithNationalId(testNational.hierarchyClassID.ToString());

            BulkImportNewItemModel secondItemData = new TestBulkImportNewItemModelBuilder()
                .WithScanCode(newScanCodes[1])
                .WithBrandId("0")
                .WithMerchandiseId(testMerchandise.hierarchyClassID.ToString())
                .WithTaxId(testTax.hierarchyClassID.ToString())
                .WithBrowsingId(testBrowsing.hierarchyClassID.ToString())
                .WithBrandName(testBrand.hierarchyClassName)
                .WithNationalId(testNational.hierarchyClassID.ToString());

            var command = new BulkImportCommand<BulkImportNewItemModel>
            {
                BulkImportData = new List<BulkImportNewItemModel> { firstItemData, secondItemData },
                UserName = userName
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            var firstItemId = context.ScanCode.Single(sc => sc.scanCode == firstItemData.ScanCode).itemID;
            var firstItemBrandAssociation = context.ItemHierarchyClass.Single(ihc => ihc.itemID == firstItemId && ihc.HierarchyClass.Hierarchy.hierarchyID == Hierarchies.Brands);

            var secondItemId = context.ScanCode.Single(sc => sc.scanCode == secondItemData.ScanCode).itemID;
            var secondItemBrandAssociation = context.ItemHierarchyClass.Single(ihc => ihc.itemID == secondItemId && ihc.HierarchyClass.Hierarchy.hierarchyID == Hierarchies.Brands);

            Assert.AreEqual(testBrand.hierarchyClassName, firstItemBrandAssociation.HierarchyClass.hierarchyClassName);
            Assert.AreEqual(testBrand.hierarchyClassName, secondItemBrandAssociation.HierarchyClass.hierarchyClassName);
        }

        [TestMethod]
        public void BulkImportNewItem_ThreeDifferentScenariosInTheSameImport_EverythingShouldWork()
        {
            // Given.
            var newItemList = new List<BulkImportNewItemModel>();

            // An item with all data.
            BulkImportNewItemModel firstItem = new TestBulkImportNewItemModelBuilder()
                .WithScanCode(newScanCodes[0])
                .WithBrandId(testBrand.hierarchyClassID.ToString())
                .WithMerchandiseId(testMerchandise.hierarchyClassID.ToString())
                .WithTaxId(testTax.hierarchyClassID.ToString())
                .WithBrowsingId(testBrowsing.hierarchyClassID.ToString())
                .WithNationalId(testNational.hierarchyClassID.ToString());

            newItemList.Add(firstItem);

            // An item with only required data.
            BulkImportNewItemModel secondItem = new TestBulkImportNewItemModelBuilder()
                .WithScanCode(newScanCodes[1])
                .WithBrandId(testBrand.hierarchyClassID.ToString())
                .WithMerchandiseId(String.Empty)
                .WithTaxId(String.Empty)
                .WithBrowsingId(String.Empty)
                .WithPosScaleTare(String.Empty)
                .WithNationalId(String.Empty);

            newItemList.Add(secondItem);

            // An item with a new brand.
            BulkImportNewItemModel thirdItem = new TestBulkImportNewItemModelBuilder()
                .WithScanCode(newScanCodes[2])
                .WithBrandId("0")
                .WithMerchandiseId(testMerchandise.hierarchyClassID.ToString())
                .WithTaxId(testTax.hierarchyClassID.ToString())
                .WithBrowsingId(testBrowsing.hierarchyClassID.ToString())
                .WithBrandName(newBrandName)
                .WithNationalId(testNational.hierarchyClassID.ToString());

            newItemList.Add(thirdItem);

            var command = new BulkImportCommand<BulkImportNewItemModel>
            {
                BulkImportData = newItemList,
                UserName = userName
            };

            // When.
            commandHandler.Execute(command);

            // Then.

            // Assert all of the things about the first item.
            int firstNewItemId = context.ScanCode.Single(sc => sc.scanCode == firstItem.ScanCode).itemID;
            string scanCode = context.ScanCode.Single(sc => sc.itemID == firstNewItemId).scanCode;
            string productDescription = context.ItemTrait.Single(it => it.Trait.traitCode == TraitCodes.ProductDescription && it.itemID == firstNewItemId).traitValue;
            string posDescription = context.ItemTrait.Single(it => it.Trait.traitCode == TraitCodes.PosDescription && it.itemID == firstNewItemId).traitValue;
            string packageUnit = context.ItemTrait.Single(it => it.Trait.traitCode == TraitCodes.PackageUnit && it.itemID == firstNewItemId).traitValue;
            string foodStampEligible = context.ItemTrait.Single(it => it.Trait.traitCode == TraitCodes.FoodStampEligible && it.itemID == firstNewItemId).traitValue;
            string posScaleTare = context.ItemTrait.Single(it => it.Trait.traitCode == TraitCodes.PosScaleTare && it.itemID == firstNewItemId).traitValue;
            int brandId = context.ItemHierarchyClass.Single(ihc => ihc.itemID == firstNewItemId && ihc.HierarchyClass.Hierarchy.hierarchyID == Hierarchies.Brands).hierarchyClassID;
            int merchandiseId = context.ItemHierarchyClass.Single(ihc => ihc.itemID == firstNewItemId && ihc.HierarchyClass.Hierarchy.hierarchyID == Hierarchies.Merchandise).hierarchyClassID;
            int taxId = context.ItemHierarchyClass.Single(ihc => ihc.itemID == firstNewItemId && ihc.HierarchyClass.Hierarchy.hierarchyID == Hierarchies.Tax).hierarchyClassID;
            int browsingId = context.ItemHierarchyClass.Single(ihc => ihc.itemID == firstNewItemId && ihc.HierarchyClass.Hierarchy.hierarchyID == Hierarchies.Browsing).hierarchyClassID;
            string insertedDate = context.ItemTrait.Single(it => it.Trait.traitCode == TraitCodes.InsertDate && it.itemID == firstNewItemId).traitValue;
            int nationalClassID = context.ItemHierarchyClass.Single(ihc => ihc.itemID == firstNewItemId && ihc.HierarchyClass.Hierarchy.hierarchyID == Hierarchies.National).hierarchyClassID;

            Assert.AreEqual(firstItem.ScanCode, scanCode);
            Assert.AreEqual(firstItem.ProductDescription, productDescription);
            Assert.AreEqual(firstItem.PosDescription, posDescription);
            Assert.AreEqual(firstItem.PackageUnit, packageUnit);
            Assert.AreEqual(firstItem.FoodStampEligible, foodStampEligible);
            Assert.AreEqual(firstItem.PosScaleTare, posScaleTare);
            Assert.AreEqual(firstItem.BrandId, brandId.ToString());
            Assert.AreEqual(firstItem.MerchandiseId, merchandiseId.ToString());
            Assert.AreEqual(firstItem.TaxId, taxId.ToString());
            Assert.AreEqual(firstItem.BrowsingId, browsingId.ToString());
            Assert.AreEqual(DateTime.Today.Date, DateTime.Parse(insertedDate).Date);
            Assert.AreEqual(firstItem.NationalId, nationalClassID.ToString());

            // Assert all of the things about the second item.
            int secondNewItemId = context.ScanCode.Single(sc => sc.scanCode == secondItem.ScanCode).itemID;
            scanCode = context.ScanCode.Single(sc => sc.itemID == secondNewItemId).scanCode;
            productDescription = context.ItemTrait.Single(it => it.Trait.traitCode == TraitCodes.ProductDescription && it.itemID == secondNewItemId).traitValue;
            posDescription = context.ItemTrait.Single(it => it.Trait.traitCode == TraitCodes.PosDescription && it.itemID == secondNewItemId).traitValue;
            packageUnit = context.ItemTrait.Single(it => it.Trait.traitCode == TraitCodes.PackageUnit && it.itemID == secondNewItemId).traitValue;
            foodStampEligible = context.ItemTrait.Single(it => it.Trait.traitCode == TraitCodes.FoodStampEligible && it.itemID == secondNewItemId).traitValue;
            posScaleTare = context.ItemTrait.Single(it => it.Trait.traitCode == TraitCodes.PosScaleTare && it.itemID == secondNewItemId).traitValue;
            brandId = context.ItemHierarchyClass.Single(ihc => ihc.itemID == secondNewItemId && ihc.HierarchyClass.Hierarchy.hierarchyID == Hierarchies.Brands).hierarchyClassID;
            var itemMerchandiseAssociation = context.ItemHierarchyClass.Where(ihc => ihc.itemID == secondNewItemId && ihc.HierarchyClass.Hierarchy.hierarchyID == Hierarchies.Merchandise);
            var itemTaxAssociation = context.ItemHierarchyClass.Where(ihc => ihc.itemID == secondNewItemId && ihc.HierarchyClass.Hierarchy.hierarchyID == Hierarchies.Tax);
            var itemBrowsingAssociation = context.ItemHierarchyClass.Where(ihc => ihc.itemID == secondNewItemId && ihc.HierarchyClass.Hierarchy.hierarchyID == Hierarchies.Browsing);
            insertedDate = context.ItemTrait.Single(it => it.Trait.traitCode == TraitCodes.InsertDate && it.itemID == secondNewItemId).traitValue;

            Assert.AreEqual(secondItem.ScanCode, scanCode);
            Assert.AreEqual(secondItem.ProductDescription, productDescription);
            Assert.AreEqual(secondItem.PosDescription, posDescription);
            Assert.AreEqual(secondItem.PackageUnit, packageUnit);
            Assert.AreEqual(secondItem.FoodStampEligible, foodStampEligible);
            Assert.AreEqual("0", posScaleTare);
            Assert.AreEqual(secondItem.BrandId, brandId.ToString());
            Assert.IsFalse(itemMerchandiseAssociation.Any());
            Assert.IsFalse(itemTaxAssociation.Any());
            Assert.IsFalse(itemBrowsingAssociation.Any());
            Assert.AreEqual(DateTime.Today.Date, DateTime.Parse(insertedDate).Date);

            // Assert all of the things about the third item.
            var newBrand = context.HierarchyClass.Single(hc => hc.hierarchyClassName == newBrandName);
            var message = context.MessageQueueHierarchy.Single(mq => mq.HierarchyClassName == newBrandName);

            int thirdNewItemId = context.ScanCode.Single(sc => sc.scanCode == thirdItem.ScanCode).itemID;
            scanCode = context.ScanCode.Single(sc => sc.itemID == thirdNewItemId).scanCode;
            productDescription = context.ItemTrait.Single(it => it.Trait.traitCode == TraitCodes.ProductDescription && it.itemID == thirdNewItemId).traitValue;
            posDescription = context.ItemTrait.Single(it => it.Trait.traitCode == TraitCodes.PosDescription && it.itemID == thirdNewItemId).traitValue;
            packageUnit = context.ItemTrait.Single(it => it.Trait.traitCode == TraitCodes.PackageUnit && it.itemID == thirdNewItemId).traitValue;
            foodStampEligible = context.ItemTrait.Single(it => it.Trait.traitCode == TraitCodes.FoodStampEligible && it.itemID == thirdNewItemId).traitValue;
            posScaleTare = context.ItemTrait.Single(it => it.Trait.traitCode == TraitCodes.PosScaleTare && it.itemID == thirdNewItemId).traitValue;
            var itemBrandAssociation = context.ItemHierarchyClass.Where(ihc => ihc.itemID == thirdNewItemId && ihc.HierarchyClass.Hierarchy.hierarchyID == Hierarchies.Brands);
            merchandiseId = context.ItemHierarchyClass.Single(ihc => ihc.itemID == thirdNewItemId && ihc.HierarchyClass.Hierarchy.hierarchyID == Hierarchies.Merchandise).hierarchyClassID;
            taxId = context.ItemHierarchyClass.Single(ihc => ihc.itemID == thirdNewItemId && ihc.HierarchyClass.Hierarchy.hierarchyID == Hierarchies.Tax).hierarchyClassID;
            browsingId = context.ItemHierarchyClass.Single(ihc => ihc.itemID == thirdNewItemId && ihc.HierarchyClass.Hierarchy.hierarchyID == Hierarchies.Browsing).hierarchyClassID;
            insertedDate = context.ItemTrait.Single(it => it.Trait.traitCode == TraitCodes.InsertDate && it.itemID == thirdNewItemId).traitValue;

            Assert.AreEqual(thirdItem.ScanCode, scanCode);
            Assert.AreEqual(thirdItem.ProductDescription, productDescription);
            Assert.AreEqual(thirdItem.PosDescription, posDescription);
            Assert.AreEqual(thirdItem.PackageUnit, packageUnit);
            Assert.AreEqual(thirdItem.FoodStampEligible, foodStampEligible);
            Assert.AreEqual(thirdItem.PosScaleTare, posScaleTare);
            Assert.IsTrue(itemBrandAssociation.Any());
            Assert.AreEqual(newBrand.hierarchyClassID, itemBrandAssociation.Single().hierarchyClassID);
            Assert.AreEqual(thirdItem.MerchandiseId, merchandiseId.ToString());
            Assert.AreEqual(thirdItem.TaxId, taxId.ToString());
            Assert.AreEqual(thirdItem.BrowsingId, browsingId.ToString());
            Assert.AreEqual(DateTime.Today.Date, DateTime.Parse(insertedDate).Date);
            Assert.AreEqual(newBrandName, message.HierarchyClassName);
            Assert.AreEqual(HierarchyNames.Brands, message.HierarchyName);
        }

        [TestMethod]
        public void BulkImportNewItem_SuccessfulImport_ItemShouldBeRemovedFromNewItem()
        {
            // Given.
            var testIrmaItems = new List<IRMAItem>
            {
                new TestIrmaItemBuilder().WithIdentifier(newScanCodes[0]),
                new TestIrmaItemBuilder().WithIdentifier("999999")
            };

            context.IRMAItem.AddRange(testIrmaItems);
            context.SaveChanges();

            var newItemList = new List<BulkImportNewItemModel>();

            BulkImportNewItemModel firstNewItem = new TestBulkImportNewItemModelBuilder()
                .WithScanCode(newScanCodes[0])
                .WithBrandId(testBrand.hierarchyClassID.ToString())
                .WithMerchandiseId(testMerchandise.hierarchyClassID.ToString())
                .WithTaxId(testTax.hierarchyClassID.ToString())
                .WithBrowsingId(testBrowsing.hierarchyClassID.ToString())
                .WithNationalId(testNational.hierarchyClassID.ToString());

            newItemList.Add(firstNewItem);

            BulkImportNewItemModel secondNewItem = new TestBulkImportNewItemModelBuilder()
                .WithScanCode(newScanCodes[1])
                .WithBrandId(testBrand.hierarchyClassID.ToString())
                .WithMerchandiseId(testMerchandise.hierarchyClassID.ToString())
                .WithTaxId(testTax.hierarchyClassID.ToString())
                .WithBrowsingId(testBrowsing.hierarchyClassID.ToString())
                .WithNationalId(testNational.hierarchyClassID.ToString());

            newItemList.Add(secondNewItem);

            var command = new BulkImportCommand<BulkImportNewItemModel>
            {
                BulkImportData = newItemList,
                UserName = userName
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            var removedIrmaItem = context.IRMAItem.SingleOrDefault(ii => ii.identifier == firstNewItem.ScanCode);
            var unaffectedIrmaItem = context.IRMAItem.SingleOrDefault(ii => ii.identifier == "999999");

            Assert.IsNull(removedIrmaItem);
            Assert.IsNotNull(unaffectedIrmaItem);
        }

        [TestMethod]
        public void BulkImportNewItem_ValidatedIsSetToTrueAndItemHasAllRequiredFields_ItemShouldBeValidated()
        {
            // Given.
            var scanCode = newScanCodes[0];

            BulkImportNewItemModel newItemData = new TestBulkImportNewItemModelBuilder()
                .WithScanCode(newScanCodes[0])
                .WithBrandId(testBrand.hierarchyClassID.ToString())
                .WithMerchandiseId(testMerchandise.hierarchyClassID.ToString())
                .WithTaxId(testTax.hierarchyClassID.ToString())
                .WithBrowsingId(testBrowsing.hierarchyClassID.ToString())
                .WithIsValidated(true)
                .WithNationalId(testNational.hierarchyClassID.ToString());

            var command = new BulkImportCommand<BulkImportNewItemModel>
            {
                BulkImportData = new List<BulkImportNewItemModel> { newItemData },
                UserName = userName
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            var newItemId = context.ScanCode.Single(sc => sc.scanCode == newItemData.ScanCode).itemID;
            var validationTrait = context.ItemTrait.Single(it => it.itemID == newItemId && it.Trait.traitCode == TraitCodes.ValidationDate);

            Assert.IsNotNull(validationTrait.traitValue);
        }

        [TestMethod]
        public void BulkImportNewItem_ValidatedIsSetDifferentlyForMultipleRows_ItemsShouldBeValidatedWhenIsValidatedIsSetToTrue()
        {
            // Given.
            var firstValidatedItemScanCode = newScanCodes[0];
            var nonValidatedItemScanCode = newScanCodes[1];
            var secondValidatedItemScanCode = newScanCodes[2];

            // Validated item.
            BulkImportNewItemModel firstValidatedItem = new TestBulkImportNewItemModelBuilder()
                .WithScanCode(firstValidatedItemScanCode)
                .WithBrandId(testBrand.hierarchyClassID.ToString())
                .WithMerchandiseId(testMerchandise.hierarchyClassID.ToString())
                .WithTaxId(testTax.hierarchyClassID.ToString())
                .WithBrowsingId(testBrowsing.hierarchyClassID.ToString())
                .WithNationalId(testNational.hierarchyClassID.ToString())
                .WithIsValidated(true);

            // Non-validated item.
            BulkImportNewItemModel nonValidatedItem = new TestBulkImportNewItemModelBuilder()
                .WithScanCode(nonValidatedItemScanCode)
                .WithBrandId(testBrand.hierarchyClassID.ToString())
                .WithMerchandiseId(testMerchandise.hierarchyClassID.ToString())
                .WithTaxId(testTax.hierarchyClassID.ToString())
                .WithBrowsingId(testBrowsing.hierarchyClassID.ToString())
                .WithNationalId(testNational.hierarchyClassID.ToString())
                .WithIsValidated(false);

            // Validated item.
            BulkImportNewItemModel secondValidatedItem = new TestBulkImportNewItemModelBuilder()
                .WithScanCode(secondValidatedItemScanCode)
                .WithBrandId(testBrand.hierarchyClassID.ToString())
                .WithMerchandiseId(testMerchandise.hierarchyClassID.ToString())
                .WithTaxId(testTax.hierarchyClassID.ToString())
                .WithBrowsingId(testBrowsing.hierarchyClassID.ToString())
                .WithNationalId(testNational.hierarchyClassID.ToString())
                .WithIsValidated(true);

            var command = new BulkImportCommand<BulkImportNewItemModel>
            {
                BulkImportData = new List<BulkImportNewItemModel> { firstValidatedItem, nonValidatedItem, secondValidatedItem },
                UserName = userName
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            int newItemId;
            ItemTrait validationTrait;

            // Assert first validated item.
            newItemId = context.ScanCode.Single(sc => sc.scanCode == firstValidatedItem.ScanCode).itemID;
            validationTrait = context.ItemTrait.SingleOrDefault(it => it.itemID == newItemId && it.Trait.traitCode == TraitCodes.ValidationDate);
            Assert.IsNotNull(validationTrait.traitValue);

            // Assert non-validated item.
            newItemId = context.ScanCode.Single(sc => sc.scanCode == nonValidatedItem.ScanCode).itemID;
            validationTrait = context.ItemTrait.SingleOrDefault(it => it.itemID == newItemId && it.Trait.traitCode == TraitCodes.ValidationDate);
            Assert.IsNull(validationTrait);

            // Assert second validated item.
            newItemId = context.ScanCode.Single(sc => sc.scanCode == secondValidatedItem.ScanCode).itemID;
            validationTrait = context.ItemTrait.SingleOrDefault(it => it.itemID == newItemId && it.Trait.traitCode == TraitCodes.ValidationDate);
            Assert.IsNotNull(validationTrait.traitValue);
        }

        [TestMethod]
        public void BulkImportNewItem_ScanCodesStartWithZeroes_ScanCodesShouldHaveStartingZeroesRemoved()
        {
            // Given.
            var firstScanCode = newScanCodes[0];
            var secondScanCode = newScanCodes[1];
            var thirdScanCode = newScanCodes[2];

            var firstScanCodeWithZeroes = "000" + firstScanCode;
            var secondScanCodeWithZeroes = "000" + secondScanCode;
            var thirdScanCodeWithZeroes = "000" + thirdScanCode;

            BulkImportNewItemModel firstItem = new TestBulkImportNewItemModelBuilder()
                .WithScanCode(firstScanCodeWithZeroes)
                .WithBrandId(testBrand.hierarchyClassID.ToString())
                .WithMerchandiseId(testMerchandise.hierarchyClassID.ToString())
                .WithTaxId(testTax.hierarchyClassID.ToString())
                .WithBrowsingId(testBrowsing.hierarchyClassID.ToString())
                .WithNationalId(testNational.hierarchyClassID.ToString());

            BulkImportNewItemModel secondItem = new TestBulkImportNewItemModelBuilder()
                .WithScanCode(secondScanCodeWithZeroes)
                .WithBrandId(testBrand.hierarchyClassID.ToString())
                .WithMerchandiseId(testMerchandise.hierarchyClassID.ToString())
                .WithTaxId(testTax.hierarchyClassID.ToString())
                .WithBrowsingId(testBrowsing.hierarchyClassID.ToString())
                .WithNationalId(testNational.hierarchyClassID.ToString());

            BulkImportNewItemModel thirdItem = new TestBulkImportNewItemModelBuilder()
                .WithScanCode(thirdScanCodeWithZeroes)
                .WithBrandId(testBrand.hierarchyClassID.ToString())
                .WithMerchandiseId(testMerchandise.hierarchyClassID.ToString())
                .WithTaxId(testTax.hierarchyClassID.ToString())
                .WithBrowsingId(testBrowsing.hierarchyClassID.ToString())
                .WithNationalId(testNational.hierarchyClassID.ToString());

            var command = new BulkImportCommand<BulkImportNewItemModel>
            {
                BulkImportData = new List<BulkImportNewItemModel> { firstItem, secondItem, thirdItem },
                UserName = userName
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            ScanCode scanCode;

            scanCode = context.ScanCode.SingleOrDefault(sc => sc.scanCode == firstScanCodeWithZeroes);
            Assert.IsNull(scanCode);

            scanCode = context.ScanCode.SingleOrDefault(sc => sc.scanCode == secondScanCodeWithZeroes);
            Assert.IsNull(scanCode);

            scanCode = context.ScanCode.SingleOrDefault(sc => sc.scanCode == thirdScanCodeWithZeroes);
            Assert.IsNull(scanCode);

            scanCode = context.ScanCode.SingleOrDefault(sc => sc.scanCode == firstScanCode);
            Assert.IsNotNull(scanCode);

            scanCode = context.ScanCode.SingleOrDefault(sc => sc.scanCode == secondScanCode);
            Assert.IsNotNull(scanCode);

            scanCode = context.ScanCode.SingleOrDefault(sc => sc.scanCode == thirdScanCode);
            Assert.IsNotNull(scanCode);
        }

        [TestMethod]
        public void BulkImportNewItem_ItemIsValidatedDuringUpload_ProductMessageShouldBeGenerated()
        {
            // Given.
            string scanCode = newScanCodes[0];

            BulkImportNewItemModel newItemData = new TestBulkImportNewItemModelBuilder()
                .WithScanCode(scanCode)
                .WithBrandId(testBrand.hierarchyClassID.ToString())
                .WithMerchandiseId(testMerchandise.hierarchyClassID.ToString())
                .WithTaxId(testTax.hierarchyClassID.ToString())
                .WithBrowsingId(testBrowsing.hierarchyClassID.ToString())
                .WithNationalId(testNational.hierarchyClassID.ToString())
                .WithIsValidated(true);

            var command = new BulkImportCommand<BulkImportNewItemModel>
            {
                BulkImportData = new List<BulkImportNewItemModel> { newItemData },
                UserName = userName
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            int newItemId = context.ScanCode.Single(sc => sc.scanCode == scanCode).Item.itemID;
            var message = context.MessageQueueProduct.Single(mq => mq.ItemId == newItemId);

            Assert.AreEqual(scanCode, message.ScanCode);
            Assert.AreEqual(MessageTypes.Product, message.MessageTypeId);
        }

        [TestMethod]
        public void BulkImportNewItem_ItemIsValidatedDuringUpload_ItemValidationEventShouldBeGenerated()
        {
            // Given.
            string scanCode = newScanCodes[0];

            IRMAItemSubscription testSubscription = new TestIrmaItemSubscriptionBuilder().WithIdentifier(scanCode);
            context.IRMAItemSubscription.Add(testSubscription);
            context.SaveChanges();

            BulkImportNewItemModel newItemData = new TestBulkImportNewItemModelBuilder()
                .WithScanCode(scanCode)
                .WithBrandId(testBrand.hierarchyClassID.ToString())
                .WithMerchandiseId(testMerchandise.hierarchyClassID.ToString())
                .WithTaxId(testTax.hierarchyClassID.ToString())
                .WithBrowsingId(testBrowsing.hierarchyClassID.ToString())
                .WithNationalId(testNational.hierarchyClassID.ToString())
                .WithIsValidated(true);

            var command = new BulkImportCommand<BulkImportNewItemModel>
            {
                BulkImportData = new List<BulkImportNewItemModel> { newItemData },
                UserName = userName
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            var newEvent = context.EventQueue.Single(eq => eq.EventMessage == scanCode && eq.EventId == EventTypes.ItemValidation);

            Assert.AreEqual(scanCode, newEvent.EventMessage);
            Assert.AreEqual(EventTypes.ItemValidation, newEvent.EventId);
        }

        [TestMethod]
        public void BulkImportNewItem_IsValidatedOptionIsFalse_ProductMessageShouldNotBeGenerated()
        {
            // Given.
            var scanCode = newScanCodes[0];

            BulkImportNewItemModel newItemData = new TestBulkImportNewItemModelBuilder()
                .WithScanCode(scanCode)
                .WithBrandId(testBrand.hierarchyClassID.ToString())
                .WithMerchandiseId(testMerchandise.hierarchyClassID.ToString())
                .WithTaxId(testTax.hierarchyClassID.ToString())
                .WithBrowsingId(testBrowsing.hierarchyClassID.ToString())
                .WithNationalId(testNational.hierarchyClassID.ToString())
                .WithIsValidated(false);

            var command = new BulkImportCommand<BulkImportNewItemModel>
            {
                BulkImportData = new List<BulkImportNewItemModel> { newItemData },
                UserName = userName
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            var item = context.ScanCode.Single(sc => sc.scanCode == scanCode).Item;
            Assert.IsNotNull(item);

            var message = context.MessageQueueProduct.SingleOrDefault(mq => mq.ItemId == item.itemID && mq.MessageTypeId == MessageTypes.Product);
            Assert.IsNull(message);
        }

        [TestMethod]
        public void BulkImportNewItem_IsValidatedOptionIsFalse_ItemUpdateEventShouldNotBeGenerated()
        {
            // Given.
            string scanCode = newScanCodes[0];

            IRMAItemSubscription testSubscription = new TestIrmaItemSubscriptionBuilder().WithIdentifier(scanCode);
            context.IRMAItemSubscription.Add(testSubscription);
            context.SaveChanges();

            BulkImportNewItemModel newItemData = new TestBulkImportNewItemModelBuilder()
                .WithScanCode(scanCode)
                .WithBrandId(testBrand.hierarchyClassID.ToString())
                .WithMerchandiseId(testMerchandise.hierarchyClassID.ToString())
                .WithTaxId(testTax.hierarchyClassID.ToString())
                .WithBrowsingId(testBrowsing.hierarchyClassID.ToString())
                .WithNationalId(testNational.hierarchyClassID.ToString())
                .WithIsValidated(false);

            var command = new BulkImportCommand<BulkImportNewItemModel>
            {
                BulkImportData = new List<BulkImportNewItemModel> { newItemData },
                UserName = userName
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            var newEvent = context.EventQueue.SingleOrDefault(eq => eq.EventMessage == scanCode && eq.EventId == EventTypes.ItemUpdate);

            Assert.IsNull(newEvent);
        }
    }
}
