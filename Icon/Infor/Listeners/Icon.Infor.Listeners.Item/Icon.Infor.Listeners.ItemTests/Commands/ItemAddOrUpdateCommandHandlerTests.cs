using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Icon.Infor.Listeners.Item.Commands;
using Icon.Framework;
using System.Data.Entity;
using Moq;
using Icon.Common.Context;
using Icon.Infor.Listeners.Item.Models;
using System.Collections.Generic;
using System.Linq;
using Icon.Infor.Listeners.Item.Extensions;
using System.Transactions;

namespace Icon.Infor.Listeners.Item.Tests.Commands
{
    [TestClass]
    public class ItemAddOrUpdateCommandHandlerTests
    {
        private ItemAddOrUpdateCommandHandler commandHandler;
        private IconContext context;
        private ItemAddOrUpdateCommand command;
        private HierarchyClass testMerchandiseHierarchyClass;
        private HierarchyClass testBrandHierarchyClass;
        private HierarchyClass testFinancialHierarchyClass;
        private HierarchyClass testNationalHierarchyClass;
        private HierarchyClass testTaxHierarchyClass;
        private IconDbContextFactory contextFactory;
        private TransactionScope transaction;

        [TestInitialize]
        public void Initialize()
        {
            transaction = new TransactionScope();
            contextFactory = new IconDbContextFactory();
            context = new IconContext();

            commandHandler = new ItemAddOrUpdateCommandHandler(contextFactory);
            command = new ItemAddOrUpdateCommand();

            testMerchandiseHierarchyClass = context.HierarchyClass.Add(new HierarchyClass { hierarchyID = Hierarchies.Merchandise, hierarchyClassName = "Test Merchandise", hierarchyLevel = HierarchyLevels.SubBrick });
            testBrandHierarchyClass = context.HierarchyClass.Add(new HierarchyClass { hierarchyID = Hierarchies.Brands, hierarchyClassName = "Test Brand", hierarchyLevel = HierarchyLevels.Brand });
            testFinancialHierarchyClass = context.HierarchyClass.Add(new HierarchyClass { hierarchyID = Hierarchies.Financial, hierarchyClassName = "Test Financial (1234)", hierarchyLevel = HierarchyLevels.Financial });
            testNationalHierarchyClass = context.HierarchyClass.Add(new HierarchyClass { hierarchyID = Hierarchies.National, hierarchyClassName = "Test National Class", hierarchyLevel = HierarchyLevels.NationalClass });
            testTaxHierarchyClass = context.HierarchyClass.Add(new HierarchyClass { hierarchyID = Hierarchies.Tax, hierarchyClassName = "1234567 Test Tax", hierarchyLevel = HierarchyLevels.Tax });
            context.SaveChanges();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose();
        }

        [TestMethod]
        public void ItemAddOrUpdate_WhenItemDoesNotExist_ShouldAddItem()
        {
            //Given
            var expectedInsertTime = DateTime.Today;
            var expectedItem = new ItemModel
            {
                ItemId = context.Item.Max(i => i.itemID) + 1,
                ItemTypeCode = ItemTypes.Codes.NonRetail,
                ScanCode = "3334445552",
                ScanCodeType = ScanCodeTypes.Descriptions.ScalePlu,
                //Hierarchy
                MerchandiseHierarchyClassId = testMerchandiseHierarchyClass.hierarchyClassID.ToString(),
                BrandsHierarchyClassId = testBrandHierarchyClass.hierarchyClassID.ToString(),
                NationalHierarchyClassId = testNationalHierarchyClass.hierarchyClassID.ToString(),
                TaxHierarchyClassId = "1234567",
                FinancialHierarchyClassId = "1234",
                //Trait
                ProductDescription = "Test ProductDescription",
                PosDescription = "Test PosDescription",
                FoodStampEligible = "Test FoodStampEligible",
                PosScaleTare = "Test PosScaleTare",
                ProhibitDiscount = "Test ProhibitDiscount",
                PackageUnit = "Test PackageUnit",
                RetailSize = "Test RetailSize",
                RetailUom = "Test RetailUom",
                AlcoholByVolume = "Test AlcoholByVolume",
                CaseinFree = "Test CaseinFree",
                DrainedWeight = "Test DrainedWeight",
                DrainedWeightUom = "Test DrainedWeightUom",
                FairTradeCertified = "Test FairTradeCertified",
                Hemp = "Test Hemp",
                LocalLoanProducer = "Test LocalLoanProducer",
                MainProductName = "Test MainProductName",
                NutritionRequired = "Test NutritionRequired",
                OrganicPersonalCare = "Test OrganicPersonalCare",
                Paleo = "Test Paleo",
                ProductFlavorType = "Test ProductFlavorType",
                InsertDate = DateTime.Now.ToString(),
                ModifiedDate = DateTime.Now.ToString(),
                ModifiedUser = String.Empty,
                DeliverySystem = null,
                Notes = "Test Notes",
                HiddenItem = "Test Hidden Item",
                DataSource = "Test Data Source",
                GMOTransparency = "Test Gmo Transparency",
                ItemDepth = "Test Item Depth",
                ItemHeight = "Test Item Height",
                ItemWidth = "Test Item Width",
                Cube = "Test Cube",
                Weight = "Test Weight",
                TrayDepth = "Test Tray Depth",
                TrayHeight = "Test Tray Height",
                TrayWidth = "Test Tray Width",
                Labeling = "Test Labeling",
                CountryofOrigin = "Test Country Of Origin",
                PackageGroup = "Test Package Group",
                PackageGroupType = "Test Package Group Type",
                PrivateLabel = "Test Private Label",
                Appellation = "Test Appellation",
                FairTradeClaim = "Test Fair Trade Claim",
                GlutenFreeClaim = "Test Gluten Free Claim",
                NonGMOClaim = "Test Non-GMO Claim",
                OrganicClaim = "Test Organic Claim",
                Varietal = "Test Varietal",
                BeerStyle = "Test Beer Style",
                LineExtension = "Test Line Extension",
                //ItemSignAttribute
                AnimalWelfareRating = "Step2",
                Biodynamic = "1",
                MilkType = "BuffaloMilk",
                CheeseRaw = "0",
                EcoScaleRating = "UltraPremiumGreen",
                GlutenFree = "Test GlutenFree",
                Kosher = "Test Kosher",
                Msc = "1",
                NonGmo = "Test NonGmo",
                Organic = "Test Organic",
                PremiumBodyCare = "0",
                FreshOrFrozen = "Frozen",
                SeafoodCatchType = "FarmRaised",
                Vegan = "Test Vegan",
                Vegetarian = "1",
                WholeTrade = "1",
                GrassFed = "1",
                PastureRaised = "1",
                FreeRange = "0",
                DryAged = "0",
                AirChilled = "1",
                MadeInHouse = "1",
                SequenceId = 10,
                CustomerFriendlyDescription = "Test Customer Friendly Description"
            };

            command.Items = new List<ItemModel>
            {
                expectedItem
            };

            //When
            commandHandler.Execute(command);

            //Then
            var item = context
                .Item
                .AsNoTracking()
                .Include(i => i.ScanCode)
                .Include(i => i.ItemTrait)
                .Include(i => i.ItemHierarchyClass)
                .Include(i => i.ItemSignAttribute)
                .Single(i => i.itemID == expectedItem.ItemId);
            var traits = item.ItemTrait;
            var itemHierarchyClasses = item.ItemHierarchyClass;
            var signAttributes = item.ItemSignAttribute.Single();

            Assert.AreEqual(expectedItem.ItemId, item.itemID);
            Assert.AreEqual(ItemTypes.Ids[expectedItem.ItemTypeCode], item.itemTypeID);
            Assert.AreEqual(expectedItem.ScanCode, item.ScanCode.First().scanCode);
            Assert.AreEqual(ScanCodeTypes.Ids[expectedItem.ScanCodeType], item.ScanCode.First().scanCodeTypeID);
            //hierarchy
            Assert.AreEqual(testMerchandiseHierarchyClass.hierarchyClassID, itemHierarchyClasses.Single(ihc => ihc.HierarchyClass.hierarchyID == Hierarchies.Merchandise).hierarchyClassID);
            Assert.AreEqual(testBrandHierarchyClass.hierarchyClassID, itemHierarchyClasses.Single(ihc => ihc.HierarchyClass.hierarchyID == Hierarchies.Brands).hierarchyClassID);
            Assert.AreEqual(testTaxHierarchyClass.hierarchyClassID, itemHierarchyClasses.Single(ihc => ihc.HierarchyClass.hierarchyID == Hierarchies.Tax).hierarchyClassID);
            Assert.AreEqual(testFinancialHierarchyClass.hierarchyClassID, itemHierarchyClasses.Single(ihc => ihc.HierarchyClass.hierarchyID == Hierarchies.Financial).hierarchyClassID);
            Assert.AreEqual(testNationalHierarchyClass.hierarchyClassID, itemHierarchyClasses.Single(ihc => ihc.HierarchyClass.hierarchyID == Hierarchies.National).hierarchyClassID);
            //traits
            Assert.AreEqual(expectedItem.ProductDescription, traits.Single(it => it.traitID == Traits.ProductDescription).traitValue);
            Assert.AreEqual(expectedItem.PosDescription, traits.Single(it => it.traitID == Traits.PosDescription).traitValue);
            Assert.AreEqual(expectedItem.FoodStampEligible, traits.Single(it => it.traitID == Traits.FoodStampEligible).traitValue);
            Assert.AreEqual(expectedItem.PosScaleTare, traits.Single(it => it.traitID == Traits.PosScaleTare).traitValue);
            Assert.AreEqual(expectedItem.ProhibitDiscount, traits.Single(it => it.traitID == Traits.ProhibitDiscount).traitValue);
            Assert.AreEqual(expectedItem.PackageUnit, traits.Single(it => it.traitID == Traits.PackageUnit).traitValue);
            Assert.AreEqual(expectedItem.RetailSize, traits.Single(it => it.traitID == Traits.RetailSize).traitValue);
            Assert.AreEqual(expectedItem.RetailUom, traits.Single(it => it.traitID == Traits.RetailUom).traitValue);
            Assert.IsTrue(expectedInsertTime <= DateTime.Parse(traits.Single(it => it.traitID == Traits.ValidationDate).traitValue));
            Assert.AreEqual(expectedItem.AlcoholByVolume, traits.Single(it => it.traitID == Traits.AlcoholByVolume).traitValue);
            Assert.AreEqual(expectedItem.CaseinFree, traits.Single(it => it.traitID == Traits.CaseinFree).traitValue);
            Assert.AreEqual(expectedItem.DrainedWeight, traits.Single(it => it.traitID == Traits.DrainedWeight).traitValue);
            Assert.AreEqual(expectedItem.DrainedWeightUom, traits.Single(it => it.traitID == Traits.DrainedWeightUom).traitValue);
            Assert.AreEqual(expectedItem.FairTradeCertified, traits.Single(it => it.traitID == Traits.FairTradeCertified).traitValue);
            Assert.AreEqual(expectedItem.Hemp, traits.Single(it => it.traitID == Traits.Hemp).traitValue);
            Assert.AreEqual(expectedItem.LocalLoanProducer, traits.Single(it => it.traitID == Traits.LocalLoanProducer).traitValue);
            Assert.AreEqual(expectedItem.MainProductName, traits.Single(it => it.traitID == Traits.MainProductName).traitValue);
            Assert.AreEqual(expectedItem.NutritionRequired, traits.Single(it => it.traitID == Traits.NutritionRequired).traitValue);
            Assert.AreEqual(expectedItem.OrganicPersonalCare, traits.Single(it => it.traitID == Traits.OrganicPersonalCare).traitValue);
            Assert.AreEqual(expectedItem.Paleo, traits.Single(it => it.traitID == Traits.Paleo).traitValue);
            Assert.AreEqual(expectedItem.ProductFlavorType, traits.Single(it => it.traitID == Traits.ProductFlavorType).traitValue);
            Assert.AreEqual(expectedItem.InsertDate, traits.Single(it => it.traitID == Traits.InsertDate).traitValue);
            Assert.AreEqual(expectedItem.ModifiedDate, traits.Single(it => it.traitID == Traits.ModifiedDate).traitValue);
            Assert.IsNull(traits.SingleOrDefault(it => it.traitID == Traits.ModifiedUser));
            Assert.IsNull(traits.SingleOrDefault(it => it.traitID == Traits.DeliverySystem));
            Assert.AreEqual(expectedItem.Notes, traits.Single(it => it.traitID == Traits.Notes).traitValue);
            Assert.AreEqual(expectedItem.HiddenItem, traits.Single(it => it.traitID == Traits.HiddenItem).traitValue);
            Assert.AreEqual(expectedItem.DataSource, traits.Single(it => it.traitID == Traits.DataSource).traitValue);
            Assert.AreEqual(expectedItem.GMOTransparency, traits.Single(it => it.traitID == Traits.GmoTransparency).traitValue);
            Assert.AreEqual(expectedItem.ItemDepth, traits.Single(it => it.traitID == Traits.ItemDepth).traitValue);
            Assert.AreEqual(expectedItem.ItemHeight, traits.Single(it => it.traitID == Traits.ItemHeight).traitValue);
            Assert.AreEqual(expectedItem.ItemWidth, traits.Single(it => it.traitID == Traits.ItemWidth).traitValue);
            Assert.AreEqual(expectedItem.Cube, traits.Single(it => it.traitID == Traits.Cube).traitValue);
            Assert.AreEqual(expectedItem.Weight, traits.Single(it => it.traitID == Traits.Weight).traitValue);
            Assert.AreEqual(expectedItem.TrayDepth, traits.Single(it => it.traitID == Traits.TrayDepth).traitValue);
            Assert.AreEqual(expectedItem.TrayHeight, traits.Single(it => it.traitID == Traits.TrayHeight).traitValue);
            Assert.AreEqual(expectedItem.TrayWidth, traits.Single(it => it.traitID == Traits.TrayWidth).traitValue);
            Assert.AreEqual(expectedItem.Labeling, traits.Single(it => it.traitID == Traits.Labeling).traitValue);
            Assert.AreEqual(expectedItem.CountryofOrigin, traits.Single(it => it.traitID == Traits.CountryOfOrigin).traitValue);
            Assert.AreEqual(expectedItem.PackageGroup, traits.Single(it => it.traitID == Traits.PackageGroup).traitValue);
            Assert.AreEqual(expectedItem.PackageGroupType, traits.Single(it => it.traitID == Traits.PackageGroupType).traitValue);
            Assert.AreEqual(expectedItem.PrivateLabel, traits.Single(it => it.traitID == Traits.PrivateLabel).traitValue);
            Assert.AreEqual(expectedItem.Appellation, traits.Single(it => it.traitID == Traits.Appellation).traitValue);
            Assert.AreEqual(expectedItem.FairTradeClaim, traits.Single(it => it.traitID == Traits.FairTradeClaim).traitValue);
            Assert.AreEqual(expectedItem.GlutenFreeClaim, traits.Single(it => it.traitID == Traits.GlutenFreeClaim).traitValue);
            Assert.AreEqual(expectedItem.NonGMOClaim, traits.Single(it => it.traitID == Traits.NonGmoClaim).traitValue);
            Assert.AreEqual(expectedItem.OrganicClaim, traits.Single(it => it.traitID == Traits.OrganicClaim).traitValue);
            Assert.AreEqual(expectedItem.Varietal, traits.Single(it => it.traitID == Traits.Varietal).traitValue);
            Assert.AreEqual(expectedItem.BeerStyle, traits.Single(it => it.traitID == Traits.BeerStyle).traitValue);
            Assert.AreEqual(expectedItem.LineExtension, traits.Single(it => it.traitID == Traits.LineExtension).traitValue);
            //Sign Attributes
            Assert.AreEqual(expectedItem.AnimalWelfareRating, signAttributes.AnimalWelfareRating);
            Assert.AreEqual(expectedItem.Biodynamic.ToBool(), signAttributes.Biodynamic);
            Assert.AreEqual(expectedItem.MilkType, signAttributes.MilkType);
            Assert.AreEqual(expectedItem.CheeseRaw.ToBool(), signAttributes.CheeseRaw);
            Assert.AreEqual(expectedItem.EcoScaleRating, signAttributes.EcoScaleRating);
            Assert.AreEqual("Test GlutenFree", signAttributes.GlutenFreeAgencyName);
            Assert.AreEqual("Test Kosher", signAttributes.KosherAgencyName);
            Assert.AreEqual(expectedItem.Msc.ToBool(), signAttributes.Msc);
            Assert.AreEqual("Test NonGmo", signAttributes.NonGmoAgencyName);
            Assert.AreEqual("Test Organic", signAttributes.OrganicAgencyName);
            Assert.AreEqual(expectedItem.PremiumBodyCare.ToBool(), signAttributes.PremiumBodyCare);
            Assert.AreEqual(expectedItem.FreshOrFrozen, signAttributes.FreshOrFrozen);
            Assert.AreEqual(expectedItem.SeafoodCatchType, signAttributes.SeafoodCatchType);
            Assert.AreEqual("Test Vegan", signAttributes.VeganAgencyName);
            Assert.AreEqual(expectedItem.Vegetarian.ToBool(), signAttributes.Vegetarian);
            Assert.AreEqual(expectedItem.WholeTrade.ToBool(), signAttributes.WholeTrade);
            Assert.AreEqual(expectedItem.GrassFed.ToBool(), signAttributes.GrassFed);
            Assert.AreEqual(expectedItem.PastureRaised.ToBool(), signAttributes.PastureRaised);
            Assert.AreEqual(expectedItem.FreeRange.ToBool(), signAttributes.FreeRange);
            Assert.AreEqual(expectedItem.DryAged.ToBool(), signAttributes.DryAged);
            Assert.AreEqual(expectedItem.AirChilled.ToBool(), signAttributes.AirChilled);
            Assert.AreEqual(expectedItem.MadeInHouse.ToBool(), signAttributes.MadeInHouse);
            Assert.AreEqual(expectedItem.CustomerFriendlyDescription, signAttributes.CustomerFriendlyDescription);
            //Sequence ID
            var sequenceId = context.Database.SqlQuery<decimal?>($"SELECT SequenceID FROM infor.ItemSequence WHERE ItemID = {item.itemID}").First();
            Assert.AreEqual(expectedItem.SequenceId, sequenceId);
        }

        [TestMethod]
        public void ItemAddOrUpdate_WhenItemDoesExist_ShouldUpdateItem()
        {
            //Given
            var expectedMerchandiseHierarchyClass = context.HierarchyClass.Add(new HierarchyClass { hierarchyID = Hierarchies.Merchandise, hierarchyClassName = "Changed Merchandise", hierarchyLevel = HierarchyLevels.SubBrick });
            var expectedBrandHierarchyClass = context.HierarchyClass.Add(new HierarchyClass { hierarchyID = Hierarchies.Brands, hierarchyClassName = "Changed Brand", hierarchyLevel = HierarchyLevels.Brand });
            var expectedFinancialHierarchyClass = context.HierarchyClass.Add(new HierarchyClass { hierarchyID = Hierarchies.Financial, hierarchyClassName = "Changed Financial (5678)", hierarchyLevel = HierarchyLevels.Financial });
            var expectedNationalHierarchyClass = context.HierarchyClass.Add(new HierarchyClass { hierarchyID = Hierarchies.National, hierarchyClassName = "Changed National Class", hierarchyLevel = HierarchyLevels.NationalClass });
            var expectedTaxHierarchyClass = context.HierarchyClass.Add(new HierarchyClass { hierarchyID = Hierarchies.Tax, hierarchyClassName = "2345678 Changed Tax", hierarchyLevel = HierarchyLevels.Tax });

            context.SaveChanges();

            DateTime expectedInsertTime;
            var initialItem = new ItemModel
            {
                ItemId = context.Item.Max(i => i.itemID) + 1,
                ItemTypeCode = ItemTypes.Codes.NonRetail,
                ScanCode = "33344455552",
                ScanCodeType = ScanCodeTypes.Descriptions.ScalePlu,
                //Hierarchy
                MerchandiseHierarchyClassId = testMerchandiseHierarchyClass.hierarchyClassID.ToString(),
                BrandsHierarchyClassId = testBrandHierarchyClass.hierarchyClassID.ToString(),
                NationalHierarchyClassId = testNationalHierarchyClass.hierarchyClassID.ToString(),
                TaxHierarchyClassId = "1234567",
                FinancialHierarchyClassId = "1234",
                //Trait
                ProductDescription = "Test ProductDescription",
                PosDescription = "Test PosDescription",
                FoodStampEligible = "Test FoodStampEligible",
                PosScaleTare = "Test PosScaleTare",
                ProhibitDiscount = "Test ProhibitDiscount",
                PackageUnit = "Test PackageUnit",
                RetailSize = "Test RetailSize",
                RetailUom = "Test RetailUom",
                AlcoholByVolume = "Test AlcoholByVolume",
                CaseinFree = "Test CaseinFree",
                DrainedWeight = "Test DrainedWeight",
                DrainedWeightUom = "Test DrainedWeightUom",
                FairTradeCertified = "Test FairTradeCertified",
                Hemp = "Test Hemp",
                LocalLoanProducer = "Test LocalLoanProducer",
                MainProductName = "Test MainProductName",
                NutritionRequired = "Test NutritionRequired",
                OrganicPersonalCare = "Test OrganicPersonalCare",
                Paleo = "Test Paleo",
                ProductFlavorType = "Test ProductFlavorType",
                CustomerFriendlyDescription = "Test Customer Friendly Description",
                DataSource = " Test Data Source",
                GMOTransparency = "Test Gmo Transparency",
                ItemDepth = "Test Item Depth",
                ItemHeight = "Test Item Height",
                ItemWidth = "Test Item Width",
                Cube = "Test Cube",
                Weight = "Test Weight",
                TrayDepth = "Test Tray Depth",
                TrayHeight = "Test Tray Height",
                TrayWidth = "Test Tray Width",
                Labeling = "Test Labeling",
                CountryofOrigin = "Test Country Of Origin",
                PackageGroup = "Test Package Group",
                PackageGroupType = "Test Package Group Type",
                PrivateLabel = "Test Private Label",
                Appellation = "Test Appellation",
                FairTradeClaim = "Test Fair Trade Claim",
                GlutenFreeClaim = "Test Gluten Free Claim",
                NonGMOClaim = "Test Non-GMO Claim",
                OrganicClaim = "Test Organic Claim",
                Varietal = "Test Varietal",
                BeerStyle = "Test Beer Style",
                LineExtension = "Test Line Extension",
                //ItemSignAttribute
                AnimalWelfareRating = "Step2",
                Biodynamic = "1",
                MilkType = "BuffaloMilk",
                CheeseRaw = "0",
                EcoScaleRating = "UltraPremiumGreen",
                GlutenFree = "Test GlutenFree",
                Kosher = "Test Kosher",
                Msc = "1",
                NonGmo = "Test NonGmo",
                Organic = "Test Organic",
                PremiumBodyCare = "0",
                FreshOrFrozen = "Frozen",
                SeafoodCatchType = "FarmRaised",
                Vegan = "Test Vegan",
                Vegetarian = "1",
                WholeTrade = "1",
                GrassFed = "1",
                PastureRaised = "1",
                FreeRange = "0",
                DryAged = "0",
                AirChilled = "1",
                MadeInHouse = "1",
                DeliverySystem = "Test Delivery System",
                Notes = "Test Notes",
                HiddenItem = "Test Hidden Item",
                SequenceId = 10
            };

            command.Items = new List<ItemModel>
            {
                initialItem
            };
            commandHandler.Execute(command);

            expectedInsertTime = DateTime.Parse(context.ItemTrait.Single(it => it.traitID == Traits.ValidationDate && it.itemID == initialItem.ItemId).traitValue);
            ItemModel expectedItem = UpdateItem(
                initialItem,
                expectedMerchandiseHierarchyClass,
                expectedBrandHierarchyClass,
                expectedNationalHierarchyClass);

            command.Items = new List<ItemModel>
            {
                expectedItem
            };


            //When
            commandHandler.Execute(command);

            //Then
            var item = context
                .Item
                .AsNoTracking()
                .Include(i => i.ScanCode)
                .Include(i => i.ItemTrait)
                .Include(i => i.ItemHierarchyClass)
                .Include(i => i.ItemSignAttribute)
                .Single(i => i.itemID == expectedItem.ItemId);
            var traits = item.ItemTrait;
            var itemHierarchyClasses = item.ItemHierarchyClass;
            var signAttributes = item.ItemSignAttribute.Single();

            Assert.AreEqual(expectedItem.ItemId, item.itemID);
            Assert.AreEqual(ItemTypes.Ids[expectedItem.ItemTypeCode], item.itemTypeID);
            Assert.AreEqual(expectedItem.ScanCode, item.ScanCode.First().scanCode);
            Assert.AreEqual(ScanCodeTypes.Ids[expectedItem.ScanCodeType], item.ScanCode.First().scanCodeTypeID);
            //hierarchy
            Assert.AreEqual(expectedMerchandiseHierarchyClass.hierarchyClassID, itemHierarchyClasses.Single(ihc => ihc.HierarchyClass.hierarchyID == Hierarchies.Merchandise).hierarchyClassID);
            Assert.AreEqual(expectedBrandHierarchyClass.hierarchyClassID, itemHierarchyClasses.Single(ihc => ihc.HierarchyClass.hierarchyID == Hierarchies.Brands).hierarchyClassID);
            Assert.AreEqual(expectedTaxHierarchyClass.hierarchyClassID, itemHierarchyClasses.Single(ihc => ihc.HierarchyClass.hierarchyID == Hierarchies.Tax).hierarchyClassID);
            Assert.AreEqual(expectedFinancialHierarchyClass.hierarchyClassID, itemHierarchyClasses.Single(ihc => ihc.HierarchyClass.hierarchyID == Hierarchies.Financial).hierarchyClassID);
            Assert.AreEqual(expectedNationalHierarchyClass.hierarchyClassID, itemHierarchyClasses.Single(ihc => ihc.HierarchyClass.hierarchyID == Hierarchies.National).hierarchyClassID);

            //traits
            Assert.AreEqual(expectedItem.ProductDescription, traits.Single(it => it.traitID == Traits.ProductDescription).traitValue);
            Assert.AreEqual(expectedItem.PosDescription, traits.Single(it => it.traitID == Traits.PosDescription).traitValue);
            Assert.AreEqual(expectedItem.FoodStampEligible, traits.Single(it => it.traitID == Traits.FoodStampEligible).traitValue);
            Assert.AreEqual(expectedItem.PosScaleTare, traits.Single(it => it.traitID == Traits.PosScaleTare).traitValue);
            Assert.AreEqual(expectedItem.ProhibitDiscount, traits.Single(it => it.traitID == Traits.ProhibitDiscount).traitValue);
            Assert.AreEqual(expectedItem.PackageUnit, traits.Single(it => it.traitID == Traits.PackageUnit).traitValue);
            Assert.AreEqual(expectedItem.RetailSize, traits.Single(it => it.traitID == Traits.RetailSize).traitValue);
            Assert.AreEqual(expectedItem.RetailUom, traits.Single(it => it.traitID == Traits.RetailUom).traitValue);
            Assert.IsTrue(expectedInsertTime == DateTime.Parse(traits.Single(it => it.traitID == Traits.ValidationDate).traitValue));
            Assert.AreEqual(expectedItem.AlcoholByVolume, traits.Single(it => it.traitID == Traits.AlcoholByVolume).traitValue);
            Assert.AreEqual(expectedItem.CaseinFree, traits.Single(it => it.traitID == Traits.CaseinFree).traitValue);
            Assert.AreEqual(expectedItem.DrainedWeight, traits.Single(it => it.traitID == Traits.DrainedWeight).traitValue);
            Assert.AreEqual(expectedItem.DrainedWeightUom, traits.Single(it => it.traitID == Traits.DrainedWeightUom).traitValue);
            Assert.AreEqual(expectedItem.FairTradeCertified, traits.Single(it => it.traitID == Traits.FairTradeCertified).traitValue);
            Assert.AreEqual(expectedItem.Hemp, traits.Single(it => it.traitID == Traits.Hemp).traitValue);
            Assert.AreEqual(expectedItem.LocalLoanProducer, traits.Single(it => it.traitID == Traits.LocalLoanProducer).traitValue);
            Assert.AreEqual(expectedItem.MainProductName, traits.Single(it => it.traitID == Traits.MainProductName).traitValue);
            Assert.AreEqual(expectedItem.NutritionRequired, traits.Single(it => it.traitID == Traits.NutritionRequired).traitValue);
            Assert.AreEqual(expectedItem.OrganicPersonalCare, traits.Single(it => it.traitID == Traits.OrganicPersonalCare).traitValue);
            Assert.AreEqual(expectedItem.Paleo, traits.Single(it => it.traitID == Traits.Paleo).traitValue);
            Assert.AreEqual(expectedItem.ProductFlavorType, traits.Single(it => it.traitID == Traits.ProductFlavorType).traitValue);
            Assert.AreEqual(expectedItem.InsertDate, traits.Single(it => it.traitID == Traits.InsertDate).traitValue);
            Assert.AreEqual(expectedItem.ModifiedDate, traits.Single(it => it.traitID == Traits.ModifiedDate).traitValue);
            Assert.IsNull(traits.SingleOrDefault(it => it.traitID == Traits.ModifiedUser));
            Assert.IsNull(traits.SingleOrDefault(it => it.traitID == Traits.DeliverySystem));
            Assert.AreEqual(expectedItem.Notes, traits.Single(it => it.traitID == Traits.Notes).traitValue);
            Assert.AreEqual(expectedItem.HiddenItem, traits.Single(it => it.traitID == Traits.HiddenItem).traitValue);
            Assert.AreEqual(expectedItem.DataSource, traits.Single(it => it.traitID == Traits.DataSource).traitValue);
            Assert.AreEqual(expectedItem.GMOTransparency, traits.Single(it => it.traitID == Traits.GmoTransparency).traitValue);
            Assert.AreEqual(expectedItem.ItemDepth, traits.Single(it => it.traitID == Traits.ItemDepth).traitValue);
            Assert.AreEqual(expectedItem.ItemHeight, traits.Single(it => it.traitID == Traits.ItemHeight).traitValue);
            Assert.AreEqual(expectedItem.ItemWidth, traits.Single(it => it.traitID == Traits.ItemWidth).traitValue);
            Assert.AreEqual(expectedItem.Cube, traits.Single(it => it.traitID == Traits.Cube).traitValue);
            Assert.AreEqual(expectedItem.Weight, traits.Single(it => it.traitID == Traits.Weight).traitValue);
            Assert.AreEqual(expectedItem.TrayDepth, traits.Single(it => it.traitID == Traits.TrayDepth).traitValue);
            Assert.AreEqual(expectedItem.TrayHeight, traits.Single(it => it.traitID == Traits.TrayHeight).traitValue);
            Assert.AreEqual(expectedItem.TrayWidth, traits.Single(it => it.traitID == Traits.TrayWidth).traitValue);
            Assert.AreEqual(expectedItem.Labeling, traits.Single(it => it.traitID == Traits.Labeling).traitValue);
            Assert.AreEqual(expectedItem.CountryofOrigin, traits.Single(it => it.traitID == Traits.CountryOfOrigin).traitValue);
            Assert.AreEqual(expectedItem.PackageGroup, traits.Single(it => it.traitID == Traits.PackageGroup).traitValue);
            Assert.AreEqual(expectedItem.PackageGroupType, traits.Single(it => it.traitID == Traits.PackageGroupType).traitValue);
            Assert.AreEqual(expectedItem.PrivateLabel, traits.Single(it => it.traitID == Traits.PrivateLabel).traitValue);
            Assert.AreEqual(expectedItem.Appellation, traits.Single(it => it.traitID == Traits.Appellation).traitValue);
            Assert.AreEqual(expectedItem.FairTradeClaim, traits.Single(it => it.traitID == Traits.FairTradeClaim).traitValue);
            Assert.AreEqual(expectedItem.GlutenFreeClaim, traits.Single(it => it.traitID == Traits.GlutenFreeClaim).traitValue);
            Assert.AreEqual(expectedItem.NonGMOClaim, traits.Single(it => it.traitID == Traits.NonGmoClaim).traitValue);
            Assert.AreEqual(expectedItem.OrganicClaim, traits.Single(it => it.traitID == Traits.OrganicClaim).traitValue);
            Assert.AreEqual(expectedItem.Varietal, traits.Single(it => it.traitID == Traits.Varietal).traitValue);
            Assert.AreEqual(expectedItem.BeerStyle, traits.Single(it => it.traitID == Traits.BeerStyle).traitValue);
            Assert.AreEqual(expectedItem.LineExtension, traits.Single(it => it.traitID == Traits.LineExtension).traitValue);
            //Sign Attributes
            Assert.AreEqual(expectedItem.AnimalWelfareRating, signAttributes.AnimalWelfareRating);
            Assert.AreEqual(expectedItem.Biodynamic.ToBool(), signAttributes.Biodynamic);
            Assert.AreEqual(expectedItem.MilkType, signAttributes.MilkType);
            Assert.AreEqual(expectedItem.CheeseRaw.ToBool(), signAttributes.CheeseRaw);
            Assert.AreEqual(expectedItem.EcoScaleRating, signAttributes.EcoScaleRating);
            Assert.AreEqual("Changed GlutenFree", signAttributes.GlutenFreeAgencyName);
            Assert.AreEqual("Changed Kosher", signAttributes.KosherAgencyName);
            Assert.AreEqual(expectedItem.Msc.ToBool(), signAttributes.Msc);
            Assert.AreEqual("Changed NonGmo", signAttributes.NonGmoAgencyName);
            Assert.AreEqual("Changed Organic", signAttributes.OrganicAgencyName);
            Assert.AreEqual(expectedItem.PremiumBodyCare.ToBool(), signAttributes.PremiumBodyCare);
            Assert.AreEqual(expectedItem.FreshOrFrozen, signAttributes.FreshOrFrozen);
            Assert.AreEqual(expectedItem.SeafoodCatchType, signAttributes.SeafoodCatchType);
            Assert.AreEqual("Changed Vegan", signAttributes.VeganAgencyName);
            Assert.AreEqual(expectedItem.Vegetarian.ToBool(), signAttributes.Vegetarian);
            Assert.AreEqual(expectedItem.WholeTrade.ToBool(), signAttributes.WholeTrade);
            Assert.AreEqual(expectedItem.GrassFed.ToBool(), signAttributes.GrassFed);
            Assert.AreEqual(expectedItem.PastureRaised.ToBool(), signAttributes.PastureRaised);
            Assert.AreEqual(expectedItem.FreeRange.ToBool(), signAttributes.FreeRange);
            Assert.AreEqual(expectedItem.DryAged.ToBool(), signAttributes.DryAged);
            Assert.AreEqual(expectedItem.AirChilled.ToBool(), signAttributes.AirChilled);
            Assert.AreEqual(expectedItem.MadeInHouse.ToBool(), signAttributes.MadeInHouse);
            Assert.AreEqual(expectedItem.CustomerFriendlyDescription, signAttributes.CustomerFriendlyDescription);
            //Sequence ID
            var sequenceId = context.Database.SqlQuery<decimal?>($"SELECT SequenceID FROM infor.ItemSequence WHERE ItemID = {item.itemID}").First();
            Assert.AreEqual(expectedItem.SequenceId, sequenceId);
        }

        [TestMethod]
        public void ItemAddOrUpdate_SequenceIdIsNull_ShouldNotAddSequenceId()
        {
            //Given
            var expectedInsertTime = DateTime.Today;
            var expectedItem = new ItemModel
            {
                ItemId = context.Item.Max(i => i.itemID) + 1,
                ItemTypeCode = ItemTypes.Codes.NonRetail,
                ScanCode = "3334445552",
                ScanCodeType = ScanCodeTypes.Descriptions.ScalePlu,
                //Hierarchy
                MerchandiseHierarchyClassId = testMerchandiseHierarchyClass.hierarchyClassID.ToString(),
                BrandsHierarchyClassId = testBrandHierarchyClass.hierarchyClassID.ToString(),
                NationalHierarchyClassId = testNationalHierarchyClass.hierarchyClassID.ToString(),
                TaxHierarchyClassId = "1234567",
                FinancialHierarchyClassId = "1234",
                //Trait
                ProductDescription = "Test ProductDescription",
                PosDescription = "Test PosDescription",
                FoodStampEligible = "Test FoodStampEligible",
                PosScaleTare = "Test PosScaleTare",
                ProhibitDiscount = "Test ProhibitDiscount",
                PackageUnit = "Test PackageUnit",
                RetailSize = "Test RetailSize",
                RetailUom = "Test RetailUom",
                AlcoholByVolume = "Test AlcoholByVolume",
                CaseinFree = "Test CaseinFree",
                DrainedWeight = "Test DrainedWeight",
                DrainedWeightUom = "Test DrainedWeightUom",
                FairTradeCertified = "Test FairTradeCertified",
                Hemp = "Test Hemp",
                LocalLoanProducer = "Test LocalLoanProducer",
                MainProductName = "Test MainProductName",
                NutritionRequired = "Test NutritionRequired",
                OrganicPersonalCare = "Test OrganicPersonalCare",
                Paleo = "Test Paleo",
                ProductFlavorType = "Test ProductFlavorType",
                InsertDate = DateTime.Now.ToString(),
                ModifiedDate = DateTime.Now.ToString(),
                ModifiedUser = String.Empty,
                DeliverySystem = null,
                Notes = "Test Notes",
                HiddenItem = "Test Hidden Item",
                CustomerFriendlyDescription = "Test Customer Friendly Description",
                //ItemSignAttribute
                AnimalWelfareRating = "Step2",
                Biodynamic = "1",
                MilkType = "BuffaloMilk",
                CheeseRaw = "0",
                EcoScaleRating = "UltraPremiumGreen",
                GlutenFree = "Test GlutenFree",
                Kosher = "Test Kosher",
                Msc = "1",
                NonGmo = "Test NonGmo",
                Organic = "Test Organic",
                PremiumBodyCare = "0",
                FreshOrFrozen = "Frozen",
                SeafoodCatchType = "FarmRaised",
                Vegan = "Test Vegan",
                Vegetarian = "1",
                WholeTrade = "1",
                GrassFed = "1",
                PastureRaised = "1",
                FreeRange = "0",
                DryAged = "0",
                AirChilled = "1",
                MadeInHouse = "1",
                SequenceId = null
            };

            command.Items = new List<ItemModel>
            {
                expectedItem
            };

            //When
            commandHandler.Execute(command);

            //Then
            var item = context
                .Item
                .AsNoTracking()
                .Include(i => i.ScanCode)
                .Include(i => i.ItemTrait)
                .Include(i => i.ItemHierarchyClass)
                .Include(i => i.ItemSignAttribute)
                .Single(i => i.itemID == expectedItem.ItemId);
            var traits = item.ItemTrait;
            var itemHierarchyClasses = item.ItemHierarchyClass;
            var signAttributes = item.ItemSignAttribute.Single();

            Assert.AreEqual(expectedItem.ItemId, item.itemID);
            Assert.AreEqual(ItemTypes.Ids[expectedItem.ItemTypeCode], item.itemTypeID);
            Assert.AreEqual(expectedItem.ScanCode, item.ScanCode.First().scanCode);
            Assert.AreEqual(ScanCodeTypes.Ids[expectedItem.ScanCodeType], item.ScanCode.First().scanCodeTypeID);
            //hierarchy
            Assert.AreEqual(testMerchandiseHierarchyClass.hierarchyClassID, itemHierarchyClasses.Single(ihc => ihc.HierarchyClass.hierarchyID == Hierarchies.Merchandise).hierarchyClassID);
            Assert.AreEqual(testBrandHierarchyClass.hierarchyClassID, itemHierarchyClasses.Single(ihc => ihc.HierarchyClass.hierarchyID == Hierarchies.Brands).hierarchyClassID);
            Assert.AreEqual(testTaxHierarchyClass.hierarchyClassID, itemHierarchyClasses.Single(ihc => ihc.HierarchyClass.hierarchyID == Hierarchies.Tax).hierarchyClassID);
            Assert.AreEqual(testFinancialHierarchyClass.hierarchyClassID, itemHierarchyClasses.Single(ihc => ihc.HierarchyClass.hierarchyID == Hierarchies.Financial).hierarchyClassID);
            Assert.AreEqual(testNationalHierarchyClass.hierarchyClassID, itemHierarchyClasses.Single(ihc => ihc.HierarchyClass.hierarchyID == Hierarchies.National).hierarchyClassID);
            //traits
            Assert.AreEqual(expectedItem.ProductDescription, traits.Single(it => it.traitID == Traits.ProductDescription).traitValue);
            Assert.AreEqual(expectedItem.PosDescription, traits.Single(it => it.traitID == Traits.PosDescription).traitValue);
            Assert.AreEqual(expectedItem.FoodStampEligible, traits.Single(it => it.traitID == Traits.FoodStampEligible).traitValue);
            Assert.AreEqual(expectedItem.PosScaleTare, traits.Single(it => it.traitID == Traits.PosScaleTare).traitValue);
            Assert.AreEqual(expectedItem.ProhibitDiscount, traits.Single(it => it.traitID == Traits.ProhibitDiscount).traitValue);
            Assert.AreEqual(expectedItem.PackageUnit, traits.Single(it => it.traitID == Traits.PackageUnit).traitValue);
            Assert.AreEqual(expectedItem.RetailSize, traits.Single(it => it.traitID == Traits.RetailSize).traitValue);
            Assert.AreEqual(expectedItem.RetailUom, traits.Single(it => it.traitID == Traits.RetailUom).traitValue);
            Assert.IsTrue(expectedInsertTime <= DateTime.Parse(traits.Single(it => it.traitID == Traits.ValidationDate).traitValue));
            Assert.AreEqual(expectedItem.AlcoholByVolume, traits.Single(it => it.traitID == Traits.AlcoholByVolume).traitValue);
            Assert.AreEqual(expectedItem.CaseinFree, traits.Single(it => it.traitID == Traits.CaseinFree).traitValue);
            Assert.AreEqual(expectedItem.DrainedWeight, traits.Single(it => it.traitID == Traits.DrainedWeight).traitValue);
            Assert.AreEqual(expectedItem.DrainedWeightUom, traits.Single(it => it.traitID == Traits.DrainedWeightUom).traitValue);
            Assert.AreEqual(expectedItem.FairTradeCertified, traits.Single(it => it.traitID == Traits.FairTradeCertified).traitValue);
            Assert.AreEqual(expectedItem.Hemp, traits.Single(it => it.traitID == Traits.Hemp).traitValue);
            Assert.AreEqual(expectedItem.LocalLoanProducer, traits.Single(it => it.traitID == Traits.LocalLoanProducer).traitValue);
            Assert.AreEqual(expectedItem.MainProductName, traits.Single(it => it.traitID == Traits.MainProductName).traitValue);
            Assert.AreEqual(expectedItem.NutritionRequired, traits.Single(it => it.traitID == Traits.NutritionRequired).traitValue);
            Assert.AreEqual(expectedItem.OrganicPersonalCare, traits.Single(it => it.traitID == Traits.OrganicPersonalCare).traitValue);
            Assert.AreEqual(expectedItem.Paleo, traits.Single(it => it.traitID == Traits.Paleo).traitValue);
            Assert.AreEqual(expectedItem.ProductFlavorType, traits.Single(it => it.traitID == Traits.ProductFlavorType).traitValue);
            Assert.AreEqual(expectedItem.InsertDate, traits.Single(it => it.traitID == Traits.InsertDate).traitValue);
            Assert.AreEqual(expectedItem.ModifiedDate, traits.Single(it => it.traitID == Traits.ModifiedDate).traitValue);
            Assert.IsNull(traits.SingleOrDefault(it => it.traitID == Traits.ModifiedUser));
            Assert.IsNull(traits.SingleOrDefault(it => it.traitID == Traits.DeliverySystem));
            Assert.AreEqual(expectedItem.Notes, traits.Single(it => it.traitID == Traits.Notes).traitValue);
            Assert.AreEqual(expectedItem.HiddenItem, traits.Single(it => it.traitID == Traits.HiddenItem).traitValue);
            //Sign Attributes
            Assert.AreEqual(expectedItem.AnimalWelfareRating, signAttributes.AnimalWelfareRating);
            Assert.AreEqual(expectedItem.Biodynamic.ToBool(), signAttributes.Biodynamic);
            Assert.AreEqual(expectedItem.MilkType, signAttributes.MilkType);
            Assert.AreEqual(expectedItem.CheeseRaw.ToBool(), signAttributes.CheeseRaw);
            Assert.AreEqual(expectedItem.EcoScaleRating, signAttributes.EcoScaleRating);
            Assert.AreEqual("Test GlutenFree", signAttributes.GlutenFreeAgencyName);
            Assert.AreEqual("Test Kosher", signAttributes.KosherAgencyName);
            Assert.AreEqual(expectedItem.Msc.ToBool(), signAttributes.Msc);
            Assert.AreEqual("Test NonGmo", signAttributes.NonGmoAgencyName);
            Assert.AreEqual("Test Organic", signAttributes.OrganicAgencyName);
            Assert.AreEqual(expectedItem.PremiumBodyCare.ToBool(), signAttributes.PremiumBodyCare);
            Assert.AreEqual(expectedItem.FreshOrFrozen, signAttributes.FreshOrFrozen);
            Assert.AreEqual(expectedItem.SeafoodCatchType, signAttributes.SeafoodCatchType);
            Assert.AreEqual("Test Vegan", signAttributes.VeganAgencyName);
            Assert.AreEqual(expectedItem.Vegetarian.ToBool(), signAttributes.Vegetarian);
            Assert.AreEqual(expectedItem.WholeTrade.ToBool(), signAttributes.WholeTrade);
            Assert.AreEqual(expectedItem.GrassFed.ToBool(), signAttributes.GrassFed);
            Assert.AreEqual(expectedItem.PastureRaised.ToBool(), signAttributes.PastureRaised);
            Assert.AreEqual(expectedItem.FreeRange.ToBool(), signAttributes.FreeRange);
            Assert.AreEqual(expectedItem.DryAged.ToBool(), signAttributes.DryAged);
            Assert.AreEqual(expectedItem.AirChilled.ToBool(), signAttributes.AirChilled);
            Assert.AreEqual(expectedItem.MadeInHouse.ToBool(), signAttributes.MadeInHouse);
            Assert.AreEqual(expectedItem.CustomerFriendlyDescription, signAttributes.CustomerFriendlyDescription);
            //Sequence ID
            var sequenceId = context.Database.SqlQuery<decimal?>($"SELECT SequenceID FROM infor.ItemSequence WHERE ItemID = {item.itemID}").FirstOrDefault();
            Assert.IsNull(sequenceId);
        }

        private ItemModel UpdateItem(ItemModel item,
            HierarchyClass newMerchandiseHierarchyClass,
            HierarchyClass newBrandHierarchyClass,
            HierarchyClass newNationalHierarchyClass)
        {
            item.ScanCodeType = ScanCodeTypes.Descriptions.Upc;

            //Hierarchy
            item.MerchandiseHierarchyClassId = newMerchandiseHierarchyClass.hierarchyClassID.ToString();
            item.BrandsHierarchyClassId = newBrandHierarchyClass.hierarchyClassID.ToString();
            item.TaxHierarchyClassId = "2345678";
            item.FinancialHierarchyClassId = "5678";
            item.NationalHierarchyClassId = newNationalHierarchyClass.hierarchyClassID.ToString();

            //Trait
            item.ProductDescription = "Changed ProductDescription";
            item.PosDescription = "Changed PosDescription";
            item.FoodStampEligible = "Changed FoodStampEligible";
            item.PosScaleTare = "Changed PosScaleTare";
            item.ProhibitDiscount = "Changed ProhibitDiscount";
            item.PackageUnit = "Changed PackageUnit";
            item.RetailSize = "Changed RetailSize";
            item.RetailUom = "Changed RetailUom";
            item.AlcoholByVolume = "Test AlcoholByVolume";
            item.CaseinFree = "Test CaseinFree";
            item.DrainedWeight = "Test DrainedWeight";
            item.DrainedWeightUom = "Test DrainedWeightUom";
            item.FairTradeCertified = "Test FairTradeCertified";
            item.Hemp = "Test Hemp";
            item.LocalLoanProducer = "Test LocalLoanProducer";
            item.MainProductName = "Test MainProductName";
            item.NutritionRequired = "Test NutritionRequired";
            item.OrganicPersonalCare = "Test OrganicPersonalCare";
            item.Paleo = "Test Paleo";
            item.ProductFlavorType = "Test ProductFlavorType";
            item.InsertDate = DateTime.Now.ToString();
            item.ModifiedDate = DateTime.Now.ToString();
            item.ModifiedUser = String.Empty;
            item.DeliverySystem = null;
            item.Notes = "Changed Notes";
            item.HiddenItem = "Changed Hidden Item";
            item.CustomerFriendlyDescription = "Changed Customer Friendly Description";
            item.DataSource = "Changed DataSource";
            item.GMOTransparency = "Changed Gmo Transparency";
            item.ItemDepth = "Changed Item Depth";
            item.ItemHeight = "Changed Item Height";
            item.ItemWidth = "Changed Item Width";
            item.Cube = "Changed Cube";
            item.Weight = "Changed Weight";
            item.TrayDepth = "Changed Tray Depth";
            item.TrayHeight = "Changed Tray Height";
            item.TrayWidth = "Changed Tray Width";
            item.Labeling = "Changed Labeling";
            item.CountryofOrigin = "Changed Country Of Origin";
            item.PackageGroup = "Changed Package Group";
            item.PackageGroupType = "Changed Package Group Type";
            item.PrivateLabel = "Changed Private Label";
            item.Appellation = "Changed Appellation";
            item.FairTradeClaim = "Changed Fair Trade Claim";
            item.GlutenFreeClaim = "Changed Gluten Free Claim";
            item.NonGMOClaim = "Changed Non-GMO Claim";
            item.OrganicClaim = "Changed Organic Claim";
            item.Varietal = "Changed Varietal";
            item.BeerStyle = "Changed Beer Style";
            item.LineExtension = "Changed Line Extension";
            //ItemSignAttribute
            item.AnimalWelfareRating = "Step3";
            item.Biodynamic = "0";
            item.MilkType = "CowGoatMilk";
            item.CheeseRaw = "1";
            item.EcoScaleRating = "BaselineOrange";
            item.GlutenFree = "Changed GlutenFree";
            item.Kosher = "Changed Kosher";
            item.Msc = "0";
            item.NonGmo = "Changed NonGmo";
            item.Organic = "Changed Organic";
            item.PremiumBodyCare = "1";
            item.FreshOrFrozen = "Fresh";
            item.SeafoodCatchType = "Wild";
            item.Vegan = "Changed Vegan";
            item.Vegetarian = "0";
            item.WholeTrade = "0";
            item.GrassFed = "0";
            item.PastureRaised = "0";
            item.FreeRange = "1";
            item.DryAged = "1";
            item.AirChilled = "0";
            item.MadeInHouse = "0";
            item.SequenceId = 11;

            return item;
        }
    }
}
