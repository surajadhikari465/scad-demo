using Icon.Common.Context;
using Icon.Framework;
using Icon.Infor.Listeners.Item.Commands;
using Icon.Infor.Listeners.Item.Models;
using Icon.Infor.Listeners.Item.Tests.TestInfrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace Icon.Infor.Listeners.Item.Tests.Commands
{
    [TestClass]
    public class GenerateItemMessagesCommandHandlerTest
    {
        private const string ExpectedFinancialHierarchyClassId = "1234";

        private GenerateItemMessagesCommandHandler commandHandler;
        private IconContext context;
        private IconDbContextFactory contextFactory;
        private TransactionScope transaction;
        private HierarchyClass brand;
        private HierarchyClass merch;
        private HierarchyClass tax;
        private HierarchyClass financial;
        private HierarchyClass national;

        [TestInitialize]
        public void Initialize()
        {
            transaction = new TransactionScope();
            contextFactory = new IconDbContextFactory();
            context = new IconContext();

            commandHandler = new GenerateItemMessagesCommandHandler(contextFactory);
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose();
        }

        [TestMethod]
        public void GenerateItemMessages_WhenInforHasOneItem_ThenGenerateItemMessages()
        {
            //Given
            var scanCode = "123456789999";
            Assert.IsFalse(context.ScanCode.Any(sc => sc.scanCode == scanCode), "Scan code already exists. Unable to run test.");

            var item = BuildAndSaveItem(scanCode);
            var expectedItemId = item.itemID;
            var subscriptions = AddIrmaItemSubscription(scanCode, new List<string> { "MW", "FL" });

            var expectedItem = new ItemModel
            {
                ItemId = expectedItemId
            };

            //When
            commandHandler.Execute(new GenerateItemMessagesCommand
            {
                Items = new List<ItemModel> { expectedItem }
            });

            //Then
            var eventQueue = this.context.EventQueue.Where(mqp => mqp.EventReferenceId == item.itemID).ToList();
            var messageQueueProducts = this.context.MessageQueueProduct.Where(mqp => mqp.ItemId == item.itemID).ToList();

            Assert.AreEqual(subscriptions.Count, eventQueue.Count());
            Assert.AreEqual(1, messageQueueProducts.Count());

            var messageQueueProduct = messageQueueProducts.First();
            var itemSignAttributes = item.ItemSignAttribute.First();
            Assert.AreEqual(itemSignAttributes.AirChilled.ToBoolString(), messageQueueProduct.AirChilled);
            Assert.AreEqual(itemSignAttributes.AnimalWelfareRating, messageQueueProduct.AnimalWelfareRating);
            Assert.AreEqual(itemSignAttributes.Biodynamic.ToBoolString(), messageQueueProduct.Biodynamic);
            Assert.AreEqual(itemSignAttributes.MilkType, messageQueueProduct.CheeseMilkType);
            Assert.AreEqual(itemSignAttributes.CheeseRaw.ToBoolString(), messageQueueProduct.CheeseRaw);
            Assert.AreEqual(itemSignAttributes.CustomerFriendlyDescription, messageQueueProduct.CustomerFriendlyDescription);
            Assert.AreEqual(itemSignAttributes.DryAged.ToBoolString(), messageQueueProduct.DryAged);
            Assert.AreEqual(itemSignAttributes.EcoScaleRating, messageQueueProduct.EcoScaleRating);
            Assert.AreEqual(itemSignAttributes.FreeRange.ToBoolString(), messageQueueProduct.FreeRange);
            Assert.AreEqual(itemSignAttributes.GlutenFreeAgencyName, messageQueueProduct.GlutenFreeAgency);
            Assert.AreEqual(itemSignAttributes.GrassFed.ToBoolString(), messageQueueProduct.GrassFed);
            Assert.AreEqual(HealthyEatingRatings.AsDictionary[itemSignAttributes.HealthyEatingRatingId.Value], messageQueueProduct.HealthyEatingRating);
            Assert.AreEqual(itemSignAttributes.KosherAgencyName, messageQueueProduct.KosherAgency);
            Assert.AreEqual(itemSignAttributes.MadeInHouse.ToBoolString(), messageQueueProduct.MadeInHouse);
            Assert.AreEqual(itemSignAttributes.Msc.ToBoolString(), messageQueueProduct.Msc);
            Assert.AreEqual(itemSignAttributes.NonGmoAgencyName, messageQueueProduct.NonGmoAgency);
            Assert.AreEqual(itemSignAttributes.OrganicAgencyName, messageQueueProduct.OrganicAgency);
            Assert.AreEqual(itemSignAttributes.PastureRaised.ToBoolString(), messageQueueProduct.PastureRaised);
            Assert.AreEqual(itemSignAttributes.PremiumBodyCare.ToBoolString(), messageQueueProduct.PremiumBodyCare);
            Assert.AreEqual(itemSignAttributes.SeafoodCatchType, messageQueueProduct.SeafoodCatchType);
            Assert.AreEqual(itemSignAttributes.FreshOrFrozen, messageQueueProduct.SeafoodFreshOrFrozen);
            Assert.AreEqual(itemSignAttributes.VeganAgencyName, messageQueueProduct.VeganAgency);
            Assert.AreEqual(itemSignAttributes.Vegetarian.ToBoolString(), messageQueueProduct.Vegetarian);
            Assert.AreEqual(itemSignAttributes.WholeTrade.ToBoolString(), messageQueueProduct.WholeTrade);

            var traits = item.ItemTrait;
            Assert.AreEqual(traits.First(it => it.traitID == Traits.ProductDescription).traitValue, messageQueueProduct.ProductDescription);
            Assert.AreEqual(traits.First(it => it.traitID == Traits.PosDescription).traitValue, messageQueueProduct.PosDescription);
            Assert.AreEqual(traits.First(it => it.traitID == Traits.FoodStampEligible).traitValue, messageQueueProduct.FoodStampEligible);
            Assert.AreEqual(traits.First(it => it.traitID == Traits.ProhibitDiscount).traitValue.ParseBoolString(), messageQueueProduct.ProhibitDiscount);
            Assert.AreEqual(traits.First(it => it.traitID == Traits.PackageUnit).traitValue, messageQueueProduct.PackageUnit);
            Assert.AreEqual(traits.First(it => it.traitID == Traits.RetailSize).traitValue, messageQueueProduct.RetailSize);
            Assert.AreEqual(traits.First(it => it.traitID == Traits.RetailUom).traitValue, messageQueueProduct.RetailUom);
            Assert.AreEqual(traits.First(it => it.traitID == Traits.FairTradeCertified).traitValue, messageQueueProduct.FairTradeCertified);
            Assert.AreEqual(traits.First(it => it.traitID == Traits.NutritionRequired).traitValue, messageQueueProduct.NutritionRequired);
            Assert.AreEqual(traits.First(it => it.traitID == Traits.FlexibleText).traitValue, messageQueueProduct.FlexibleText);
            Assert.AreEqual(traits.First(it => it.traitID == Traits.MadeWithOrganicGrapes).traitValue, messageQueueProduct.MadeWithOrganicGrapes);
            Assert.AreEqual(traits.First(it => it.traitID == Traits.PrimeBeef).traitValue.ParseBoolStringAdvanced(), messageQueueProduct.PrimeBeef);
            Assert.AreEqual(traits.First(it => it.traitID == Traits.RainforestAlliance).traitValue.ParseBoolStringAdvanced(), messageQueueProduct.RainforestAlliance);
            Assert.AreEqual(traits.First(it => it.traitID == Traits.Refrigerated).traitValue, messageQueueProduct.Refrigerated);
            Assert.AreEqual(traits.First(it => it.traitID == Traits.SmithsonianBirdFriendly).traitValue.ParseBoolStringAdvanced(), messageQueueProduct.SmithsonianBirdFriendly);
            Assert.AreEqual(traits.First(it => it.traitID == Traits.WicEligible).traitValue.ParseBoolStringAdvanced(), messageQueueProduct.WicEligible);
            Assert.AreEqual(int.Parse(traits.First(it => it.traitID == Traits.ShelfLife).traitValue), messageQueueProduct.ShelfLife);
            Assert.AreEqual(traits.First(it => it.traitID == Traits.SelfCheckoutItemTareGroup).traitValue, messageQueueProduct.SelfCheckoutItemTareGroup);
            Assert.AreEqual(traits.First(it => it.traitID == Traits.GlobalPricingProgram).traitValue, messageQueueProduct.GlobalPricingProgram);
            Assert.AreEqual(traits.First(it => it.traitID == Traits.ProductDescription).traitValue, messageQueueProduct.ProductDescription);
            Assert.AreEqual(traits.First(it => it.traitID == Traits.HiddenItem).traitValue.ParseBoolString(), messageQueueProduct.Hidden);
            Assert.AreEqual(traits.First(it => it.traitID == Traits.DataSource).traitValue, messageQueueProduct.DataSource);
            Assert.AreEqual(traits.First(it => it.traitID == Traits.GmoTransparency).traitValue, messageQueueProduct.GMOTransparency);
            Assert.AreEqual(decimal.Parse(traits.First(it => it.traitID == Traits.ItemDepth).traitValue), messageQueueProduct.ItemDepth);
            Assert.AreEqual(decimal.Parse(traits.First(it => it.traitID == Traits.ItemHeight).traitValue), messageQueueProduct.ItemHeight);
            Assert.AreEqual(decimal.Parse(traits.First(it => it.traitID == Traits.ItemWidth).traitValue), messageQueueProduct.ItemWidth);
            Assert.AreEqual(decimal.Parse(traits.First(it => it.traitID == Traits.Cube).traitValue), messageQueueProduct.Cube);
            Assert.AreEqual(decimal.Parse(traits.First(it => it.traitID == Traits.Weight).traitValue), messageQueueProduct.Weight);
            Assert.AreEqual(decimal.Parse(traits.First(it => it.traitID == Traits.TrayDepth).traitValue), messageQueueProduct.TrayDepth);
            Assert.AreEqual(decimal.Parse(traits.First(it => it.traitID == Traits.TrayHeight).traitValue), messageQueueProduct.TrayHeight);
            Assert.AreEqual(decimal.Parse(traits.First(it => it.traitID == Traits.TrayWidth).traitValue), messageQueueProduct.TrayWidth);
            Assert.AreEqual(traits.First(it => it.traitID == Traits.Labeling).traitValue, messageQueueProduct.Labeling);
            Assert.AreEqual(traits.First(it => it.traitID == Traits.CountryOfOrigin).traitValue, messageQueueProduct.CountryOfOrigin);
            Assert.AreEqual(int.Parse(traits.First(it => it.traitID == Traits.PackageGroup).traitValue), messageQueueProduct.PackageGroup);
            Assert.AreEqual(traits.First(it => it.traitID == Traits.PackageGroupType).traitValue, messageQueueProduct.PackageGroupType);
            Assert.AreEqual(traits.First(it => it.traitID == Traits.PrivateLabel).traitValue, messageQueueProduct.PrivateLabel);
            Assert.AreEqual(traits.First(it => it.traitID == Traits.Appellation).traitValue, messageQueueProduct.Appellation);
            Assert.AreEqual(traits.First(it => it.traitID == Traits.FairTradeClaim).traitValue.ParseBoolStringAdvancedNullable(), messageQueueProduct.FairTradeClaim);
            Assert.AreEqual(traits.First(it => it.traitID == Traits.GlutenFreeClaim).traitValue.ParseBoolStringAdvancedNullable(), messageQueueProduct.GlutenFreeClaim);
            Assert.AreEqual(traits.First(it => it.traitID == Traits.NonGmoClaim).traitValue.ParseBoolStringAdvancedNullable(), messageQueueProduct.NonGMOClaim);
            Assert.AreEqual(traits.First(it => it.traitID == Traits.OrganicClaim).traitValue.ParseBoolStringAdvancedNullable(), messageQueueProduct.OrganicClaim);
            Assert.AreEqual(traits.First(it => it.traitID == Traits.Varietal).traitValue, messageQueueProduct.Varietal);
            Assert.AreEqual(traits.First(it => it.traitID == Traits.BeerStyle).traitValue, messageQueueProduct.BeerStyle);
            Assert.AreEqual(traits.First(it => it.traitID == Traits.LineExtension).traitValue, messageQueueProduct.LineExtension);


            Assert.AreEqual(brand.hierarchyClassID, messageQueueProduct.BrandId);
            Assert.AreEqual(brand.hierarchyLevel, messageQueueProduct.BrandLevel);
            Assert.AreEqual(brand.hierarchyClassName, messageQueueProduct.BrandName);
            Assert.AreEqual(brand.hierarchyParentClassID, messageQueueProduct.BrandParentId);

            Assert.AreEqual(merch.hierarchyClassID, messageQueueProduct.MerchandiseClassId);
            Assert.AreEqual(merch.hierarchyLevel, messageQueueProduct.MerchandiseLevel);
            Assert.AreEqual(merch.hierarchyClassName, messageQueueProduct.MerchandiseClassName);
            Assert.AreEqual(merch.hierarchyParentClassID, messageQueueProduct.MerchandiseParentId);

            Assert.AreEqual(tax.hierarchyClassID, messageQueueProduct.TaxClassId);
            Assert.AreEqual(tax.hierarchyLevel, messageQueueProduct.TaxLevel);
            Assert.AreEqual(tax.hierarchyClassName, messageQueueProduct.TaxClassName);
            Assert.AreEqual(tax.hierarchyParentClassID, messageQueueProduct.TaxParentId);

            Assert.AreEqual(ExpectedFinancialHierarchyClassId, messageQueueProduct.FinancialClassId);
            Assert.AreEqual(financial.hierarchyLevel, messageQueueProduct.FinancialLevel);
            Assert.AreEqual(financial.hierarchyClassName, messageQueueProduct.FinancialClassName);
            Assert.AreEqual(financial.hierarchyParentClassID, messageQueueProduct.FinancialParentId);

            Assert.AreEqual(national.hierarchyClassID, messageQueueProduct.NationalClassId);
            Assert.AreEqual(national.hierarchyLevel, messageQueueProduct.NationalLevel);
            Assert.AreEqual(national.hierarchyClassName, messageQueueProduct.NationalClassName);
            Assert.AreEqual(national.hierarchyParentClassID, messageQueueProduct.NationalParentId);
        }

        [TestMethod]
        public void GenerateItemMessages_WhenInforHasOneItemWithHospitalityAndEstoreTraits_ThenGenerateItemMessages()
        {
            //Given
            var scanCode = "123456789999";
            Assert.IsFalse(context.ScanCode.Any(sc => sc.scanCode == scanCode), "Scan code already exists. Unable to run test.");

            var item = BuildAndSaveItem(scanCode);
            var expectedItemId = item.itemID;
            var subscriptions = AddIrmaItemSubscription(scanCode, new List<string> { "MW", "FL" });

            var expectedItem = new ItemModel
            {
                ItemId = expectedItemId
            };

            //When
            commandHandler.Execute(new GenerateItemMessagesCommand
            {
                Items = new List<ItemModel> { expectedItem }
            });

            //Then
            var eventQueue = this.context.EventQueue.Where(mqp => mqp.EventReferenceId == item.itemID).ToList();
            var messageQueueProducts = this.context.MessageQueueProduct.Where(mqp => mqp.ItemId == item.itemID).ToList();

            Assert.AreEqual(subscriptions.Count, eventQueue.Count());
            Assert.AreEqual(1, messageQueueProducts.Count());

            var messageQueueProduct = messageQueueProducts.First();
            var itemSignAttributes = item.ItemSignAttribute.First();
            Assert.AreEqual(itemSignAttributes.AirChilled.ToBoolString(), messageQueueProduct.AirChilled);
            Assert.AreEqual(itemSignAttributes.AnimalWelfareRating, messageQueueProduct.AnimalWelfareRating);
            Assert.AreEqual(itemSignAttributes.Biodynamic.ToBoolString(), messageQueueProduct.Biodynamic);
            Assert.AreEqual(itemSignAttributes.MilkType, messageQueueProduct.CheeseMilkType);
            Assert.AreEqual(itemSignAttributes.CheeseRaw.ToBoolString(), messageQueueProduct.CheeseRaw);
            Assert.AreEqual(itemSignAttributes.CustomerFriendlyDescription, messageQueueProduct.CustomerFriendlyDescription);
            Assert.AreEqual(itemSignAttributes.DryAged.ToBoolString(), messageQueueProduct.DryAged);
            Assert.AreEqual(itemSignAttributes.EcoScaleRating, messageQueueProduct.EcoScaleRating);
            Assert.AreEqual(itemSignAttributes.FreeRange.ToBoolString(), messageQueueProduct.FreeRange);
            Assert.AreEqual(itemSignAttributes.GlutenFreeAgencyName, messageQueueProduct.GlutenFreeAgency);
            Assert.AreEqual(itemSignAttributes.GrassFed.ToBoolString(), messageQueueProduct.GrassFed);
            Assert.AreEqual(HealthyEatingRatings.AsDictionary[itemSignAttributes.HealthyEatingRatingId.Value], messageQueueProduct.HealthyEatingRating);
            Assert.AreEqual(itemSignAttributes.KosherAgencyName, messageQueueProduct.KosherAgency);
            Assert.AreEqual(itemSignAttributes.MadeInHouse.ToBoolString(), messageQueueProduct.MadeInHouse);
            Assert.AreEqual(itemSignAttributes.Msc.ToBoolString(), messageQueueProduct.Msc);
            Assert.AreEqual(itemSignAttributes.NonGmoAgencyName, messageQueueProduct.NonGmoAgency);
            Assert.AreEqual(itemSignAttributes.OrganicAgencyName, messageQueueProduct.OrganicAgency);
            Assert.AreEqual(itemSignAttributes.PastureRaised.ToBoolString(), messageQueueProduct.PastureRaised);
            Assert.AreEqual(itemSignAttributes.PremiumBodyCare.ToBoolString(), messageQueueProduct.PremiumBodyCare);
            Assert.AreEqual(itemSignAttributes.SeafoodCatchType, messageQueueProduct.SeafoodCatchType);
            Assert.AreEqual(itemSignAttributes.FreshOrFrozen, messageQueueProduct.SeafoodFreshOrFrozen);
            Assert.AreEqual(itemSignAttributes.VeganAgencyName, messageQueueProduct.VeganAgency);
            Assert.AreEqual(itemSignAttributes.Vegetarian.ToBoolString(), messageQueueProduct.Vegetarian);
            Assert.AreEqual(itemSignAttributes.WholeTrade.ToBoolString(), messageQueueProduct.WholeTrade);

            var traits = item.ItemTrait;
            Assert.AreEqual(traits.First(it => it.traitID == Traits.ProductDescription).traitValue, messageQueueProduct.ProductDescription);
            Assert.AreEqual(traits.First(it => it.traitID == Traits.PosDescription).traitValue, messageQueueProduct.PosDescription);
            Assert.AreEqual(traits.First(it => it.traitID == Traits.FoodStampEligible).traitValue, messageQueueProduct.FoodStampEligible);
            Assert.AreEqual(traits.First(it => it.traitID == Traits.ProhibitDiscount).traitValue.ParseBoolString(), messageQueueProduct.ProhibitDiscount);
            Assert.AreEqual(traits.First(it => it.traitID == Traits.PackageUnit).traitValue, messageQueueProduct.PackageUnit);
            Assert.AreEqual(traits.First(it => it.traitID == Traits.RetailSize).traitValue, messageQueueProduct.RetailSize);
            Assert.AreEqual(traits.First(it => it.traitID == Traits.RetailUom).traitValue, messageQueueProduct.RetailUom);
            Assert.AreEqual(traits.First(it => it.traitID == Traits.FairTradeCertified).traitValue, messageQueueProduct.FairTradeCertified);
            Assert.AreEqual(traits.First(it => it.traitID == Traits.NutritionRequired).traitValue, messageQueueProduct.NutritionRequired);
            Assert.AreEqual(traits.First(it => it.traitID == Traits.FlexibleText).traitValue, messageQueueProduct.FlexibleText);
            Assert.AreEqual(traits.First(it => it.traitID == Traits.MadeWithOrganicGrapes).traitValue, messageQueueProduct.MadeWithOrganicGrapes);
            Assert.AreEqual(traits.First(it => it.traitID == Traits.PrimeBeef).traitValue.ParseBoolStringAdvanced(), messageQueueProduct.PrimeBeef);
            Assert.AreEqual(traits.First(it => it.traitID == Traits.RainforestAlliance).traitValue.ParseBoolStringAdvanced(), messageQueueProduct.RainforestAlliance);
            Assert.AreEqual(traits.First(it => it.traitID == Traits.Refrigerated).traitValue, messageQueueProduct.Refrigerated);
            Assert.AreEqual(traits.First(it => it.traitID == Traits.SmithsonianBirdFriendly).traitValue.ParseBoolStringAdvanced(), messageQueueProduct.SmithsonianBirdFriendly);
            Assert.AreEqual(traits.First(it => it.traitID == Traits.WicEligible).traitValue.ParseBoolStringAdvanced(), messageQueueProduct.WicEligible);
            Assert.AreEqual(int.Parse(traits.First(it => it.traitID == Traits.ShelfLife).traitValue), messageQueueProduct.ShelfLife);
            Assert.AreEqual(traits.First(it => it.traitID == Traits.SelfCheckoutItemTareGroup).traitValue, messageQueueProduct.SelfCheckoutItemTareGroup);
            Assert.AreEqual(traits.First(it => it.traitID == Traits.GlobalPricingProgram).traitValue, messageQueueProduct.GlobalPricingProgram);
            Assert.AreEqual(traits.First(it => it.traitID == Traits.ProductDescription).traitValue, messageQueueProduct.ProductDescription);
            Assert.AreEqual(traits.First(it => it.traitID == Traits.HiddenItem).traitValue.ParseBoolString(), messageQueueProduct.Hidden);

            Assert.AreEqual(traits.First(it => it.traitID == Traits.Line).traitValue, messageQueueProduct.Line, nameof(messageQueueProduct.Line));
            Assert.AreEqual(traits.First(it => it.traitID == Traits.Sku).traitValue, messageQueueProduct.SKU, nameof(messageQueueProduct.SKU));
            Assert.AreEqual(traits.First(it => it.traitID == Traits.PriceLine).traitValue, messageQueueProduct.PriceLine, nameof(messageQueueProduct.PriceLine));
            Assert.AreEqual(traits.First(it => it.traitID == Traits.VariantSize).traitValue, messageQueueProduct.VariantSize, nameof(messageQueueProduct.VariantSize));
            Assert.AreEqual(traits.First(it => it.traitID == Traits.EstoreNutritionRequired).traitValue.ParseBoolStringAdvanced(), messageQueueProduct.EStoreNutritionRequired, nameof(messageQueueProduct.EStoreNutritionRequired));
            Assert.AreEqual(traits.First(it => it.traitID == Traits.PrimeNowEligible).traitValue.ParseBoolStringAdvancedNullable(), messageQueueProduct.PrimeNowEligible, nameof(messageQueueProduct.PrimeNowEligible));
            Assert.AreEqual(traits.First(it => it.traitID == Traits.EstoreEligible).traitValue.ParseBoolStringAdvanced(), messageQueueProduct.EstoreEligible, nameof(messageQueueProduct.EstoreEligible));
            Assert.AreEqual(traits.First(it => it.traitID == Traits.Tsf365Eligible).traitValue.ParseBoolStringAdvancedNullable(), messageQueueProduct.TSFEligible, nameof(messageQueueProduct.TSFEligible));
            Assert.AreEqual(traits.First(it => it.traitID == Traits.WfmEligilble).traitValue.ParseBoolStringAdvancedNullable(), messageQueueProduct.WFMEligilble, nameof(messageQueueProduct.WFMEligilble));
            Assert.AreEqual(traits.First(it => it.traitID == Traits.Other3pEligible).traitValue.ParseBoolStringAdvancedNullable(), messageQueueProduct.Other3PEligible, nameof(messageQueueProduct.Other3PEligible));

            Assert.AreEqual(traits.First(it => it.traitID == Traits.DataSource).traitValue, messageQueueProduct.DataSource);
            Assert.AreEqual(traits.First(it => it.traitID == Traits.GmoTransparency).traitValue, messageQueueProduct.GMOTransparency);
            Assert.AreEqual(decimal.Parse(traits.First(it => it.traitID == Traits.ItemDepth).traitValue), messageQueueProduct.ItemDepth);
            Assert.AreEqual(decimal.Parse(traits.First(it => it.traitID == Traits.ItemHeight).traitValue), messageQueueProduct.ItemHeight);
            Assert.AreEqual(decimal.Parse(traits.First(it => it.traitID == Traits.ItemWidth).traitValue), messageQueueProduct.ItemWidth);
            Assert.AreEqual(decimal.Parse(traits.First(it => it.traitID == Traits.Cube).traitValue), messageQueueProduct.Cube);
            Assert.AreEqual(decimal.Parse(traits.First(it => it.traitID == Traits.Weight).traitValue), messageQueueProduct.Weight);
            Assert.AreEqual(decimal.Parse(traits.First(it => it.traitID == Traits.TrayDepth).traitValue), messageQueueProduct.TrayDepth);
            Assert.AreEqual(decimal.Parse(traits.First(it => it.traitID == Traits.TrayHeight).traitValue), messageQueueProduct.TrayHeight);
            Assert.AreEqual(decimal.Parse(traits.First(it => it.traitID == Traits.TrayWidth).traitValue), messageQueueProduct.TrayWidth);
            Assert.AreEqual(traits.First(it => it.traitID == Traits.Labeling).traitValue, messageQueueProduct.Labeling);
            Assert.AreEqual(traits.First(it => it.traitID == Traits.CountryOfOrigin).traitValue, messageQueueProduct.CountryOfOrigin);
            Assert.AreEqual(int.Parse(traits.First(it => it.traitID == Traits.PackageGroup).traitValue), messageQueueProduct.PackageGroup);
            Assert.AreEqual(traits.First(it => it.traitID == Traits.PackageGroupType).traitValue, messageQueueProduct.PackageGroupType);
            Assert.AreEqual(traits.First(it => it.traitID == Traits.PrivateLabel).traitValue, messageQueueProduct.PrivateLabel);
            Assert.AreEqual(traits.First(it => it.traitID == Traits.Appellation).traitValue, messageQueueProduct.Appellation);
            Assert.AreEqual(traits.First(it => it.traitID == Traits.FairTradeClaim).traitValue.ParseBoolStringAdvancedNullable(), messageQueueProduct.FairTradeClaim);
            Assert.AreEqual(traits.First(it => it.traitID == Traits.GlutenFreeClaim).traitValue.ParseBoolStringAdvancedNullable(), messageQueueProduct.GlutenFreeClaim);
            Assert.AreEqual(traits.First(it => it.traitID == Traits.NonGmoClaim).traitValue.ParseBoolStringAdvancedNullable(), messageQueueProduct.NonGMOClaim);
            Assert.AreEqual(traits.First(it => it.traitID == Traits.OrganicClaim).traitValue.ParseBoolStringAdvancedNullable(), messageQueueProduct.OrganicClaim);
            Assert.AreEqual(traits.First(it => it.traitID == Traits.Varietal).traitValue, messageQueueProduct.Varietal);
            Assert.AreEqual(traits.First(it => it.traitID == Traits.BeerStyle).traitValue, messageQueueProduct.BeerStyle);
            Assert.AreEqual(traits.First(it => it.traitID == Traits.LineExtension).traitValue, messageQueueProduct.LineExtension);

            Assert.AreEqual(brand.hierarchyClassID, messageQueueProduct.BrandId);
            Assert.AreEqual(brand.hierarchyLevel, messageQueueProduct.BrandLevel);
            Assert.AreEqual(brand.hierarchyClassName, messageQueueProduct.BrandName);
            Assert.AreEqual(brand.hierarchyParentClassID, messageQueueProduct.BrandParentId);

            Assert.AreEqual(merch.hierarchyClassID, messageQueueProduct.MerchandiseClassId);
            Assert.AreEqual(merch.hierarchyLevel, messageQueueProduct.MerchandiseLevel);
            Assert.AreEqual(merch.hierarchyClassName, messageQueueProduct.MerchandiseClassName);
            Assert.AreEqual(merch.hierarchyParentClassID, messageQueueProduct.MerchandiseParentId);

            Assert.AreEqual(tax.hierarchyClassID, messageQueueProduct.TaxClassId);
            Assert.AreEqual(tax.hierarchyLevel, messageQueueProduct.TaxLevel);
            Assert.AreEqual(tax.hierarchyClassName, messageQueueProduct.TaxClassName);
            Assert.AreEqual(tax.hierarchyParentClassID, messageQueueProduct.TaxParentId);

            Assert.AreEqual(ExpectedFinancialHierarchyClassId, messageQueueProduct.FinancialClassId);
            Assert.AreEqual(financial.hierarchyLevel, messageQueueProduct.FinancialLevel);
            Assert.AreEqual(financial.hierarchyClassName, messageQueueProduct.FinancialClassName);
            Assert.AreEqual(financial.hierarchyParentClassID, messageQueueProduct.FinancialParentId);

            Assert.AreEqual(national.hierarchyClassID, messageQueueProduct.NationalClassId);
            Assert.AreEqual(national.hierarchyLevel, messageQueueProduct.NationalLevel);
            Assert.AreEqual(national.hierarchyClassName, messageQueueProduct.NationalClassName);
            Assert.AreEqual(national.hierarchyParentClassID, messageQueueProduct.NationalParentId);
        }

        private List<IRMAItemSubscription> AddIrmaItemSubscription(string scanCode, List<string> regions)
        {
            var subscriptions = regions
                .Select(r => new IRMAItemSubscription
                {
                    identifier = scanCode,
                    regioncode = r
                }).ToList();

            context.IRMAItemSubscription.AddRange(subscriptions);
            context.SaveChanges();

            return subscriptions;
        }

        private Framework.Item BuildAndSaveItem(string scanCode)
        {
            brand = BuildAndSaveHierarchyClass(Hierarchies.Brands, HierarchyLevels.Brand, "Infor Test Brand");
            merch = BuildAndSaveHierarchyClass(Hierarchies.Merchandise, HierarchyLevels.SubBrick, "Infor Test Merch");
            tax = BuildAndSaveHierarchyClass(Hierarchies.Tax, HierarchyLevels.Tax, "Infor Test Tax");
            financial = BuildAndSaveHierarchyClass(Hierarchies.Financial, HierarchyLevels.Financial, $"Infor Test Fin ({ExpectedFinancialHierarchyClassId})");
            national = BuildAndSaveHierarchyClass(Hierarchies.National, HierarchyLevels.NationalClass, "Infor Test National");

            Framework.Item item = new Framework.Item
            {
                itemTypeID = ItemTypes.RetailSale,
                ScanCode = new List<ScanCode> { new ScanCode { scanCode = scanCode, scanCodeTypeID = ScanCodeTypes.Upc } },
                ItemHierarchyClass = new List<ItemHierarchyClass>
                {
                    new ItemHierarchyClass { hierarchyClassID = brand.hierarchyClassID, localeID = Locales.WholeFoods },
                    new ItemHierarchyClass { hierarchyClassID = merch.hierarchyClassID, localeID = Locales.WholeFoods },
                    new ItemHierarchyClass { hierarchyClassID = tax.hierarchyClassID, localeID = Locales.WholeFoods },
                    new ItemHierarchyClass { hierarchyClassID = financial.hierarchyClassID, localeID = Locales.WholeFoods },
                    new ItemHierarchyClass { hierarchyClassID = national.hierarchyClassID, localeID = Locales.WholeFoods }
                },
                ItemTrait = new List<ItemTrait>
                {
                    new ItemTrait { traitID = Traits.ValidationDate, traitValue = DateTime.Now.ToString(), localeID = Locales.WholeFoods },
                    new ItemTrait { traitID = Traits.ProductDescription, traitValue = "Test Product Description", localeID = Locales.WholeFoods },
                    new ItemTrait { traitID = Traits.PosDescription, traitValue = "Test PosDescription", localeID = Locales.WholeFoods },
                    new ItemTrait { traitID = Traits.FoodStampEligible, traitValue = "Test FoodStampEligible", localeID = Locales.WholeFoods },
                    new ItemTrait { traitID = Traits.ProhibitDiscount, traitValue = "1", localeID = Locales.WholeFoods },
                    new ItemTrait { traitID = Traits.PackageUnit, traitValue = "Test PackageUnit", localeID = Locales.WholeFoods },
                    new ItemTrait { traitID = Traits.RetailSize, traitValue = "Test RetailSize", localeID = Locales.WholeFoods },
                    new ItemTrait { traitID = Traits.RetailUom, traitValue = "Test RetailUom", localeID = Locales.WholeFoods },
                    new ItemTrait { traitID = Traits.AlcoholByVolume, traitValue = "Test AlcoholByVolume", localeID = Locales.WholeFoods },
                    new ItemTrait { traitID = Traits.CaseinFree, traitValue = "Test CaseinFree", localeID = Locales.WholeFoods },
                    new ItemTrait { traitID = Traits.DrainedWeight, traitValue = "Test DrainedWeight", localeID = Locales.WholeFoods },
                    new ItemTrait { traitID = Traits.DrainedWeightUom, traitValue = "Test DrainedWeightUom", localeID = Locales.WholeFoods },
                    new ItemTrait { traitID = Traits.FairTradeCertified, traitValue = "Test FairTradeCertified", localeID = Locales.WholeFoods },
                    new ItemTrait { traitID = Traits.Hemp, traitValue = "Test Hemp", localeID = Locales.WholeFoods },
                    new ItemTrait { traitID = Traits.LocalLoanProducer, traitValue = "Test LocalLoanProducer", localeID = Locales.WholeFoods },
                    new ItemTrait { traitID = Traits.MainProductName, traitValue = "Test MainProductName", localeID = Locales.WholeFoods },
                    new ItemTrait { traitID = Traits.NutritionRequired, traitValue = "Test NutritionRequired", localeID = Locales.WholeFoods },
                    new ItemTrait { traitID = Traits.OrganicPersonalCare, traitValue = "Test OrganicPersonalCare", localeID = Locales.WholeFoods },
                    new ItemTrait { traitID = Traits.Paleo, traitValue = "Test Paleo", localeID = Locales.WholeFoods },
                    new ItemTrait { traitID = Traits.ProductFlavorType, traitValue = "Test ProductFlavorType", localeID = Locales.WholeFoods },
                    new ItemTrait { traitID = Traits.InsertDate, traitValue = "Test InsertDate", localeID = Locales.WholeFoods },
                    new ItemTrait { traitID = Traits.ModifiedDate, traitValue = "Test ModifiedDate", localeID = Locales.WholeFoods },
                    new ItemTrait { traitID = Traits.ModifiedUser, traitValue = "Test ModifiedUser", localeID = Locales.WholeFoods },
                    new ItemTrait { traitID = Traits.HiddenItem, traitValue = "1", localeID = Locales.WholeFoods },
                    new ItemTrait { traitID = Traits.Notes, traitValue = "Test Notes", localeID = Locales.WholeFoods },
                    new ItemTrait { traitID = Traits.DeliverySystem, traitValue = "Test DeliverySystem", localeID = Locales.WholeFoods },
                    new ItemTrait { traitID = Traits.FlexibleText, traitValue = "Test FlexibleText", localeID = Locales.WholeFoods },
                    new ItemTrait { traitID = Traits.MadeWithOrganicGrapes, traitValue = "Test MadeWithOrganicGrapes", localeID = Locales.WholeFoods },
                    new ItemTrait { traitID = Traits.PrimeBeef, traitValue = "true", localeID = Locales.WholeFoods },
                    new ItemTrait { traitID = Traits.RainforestAlliance, traitValue = "True", localeID = Locales.WholeFoods },
                    new ItemTrait { traitID = Traits.Refrigerated, traitValue = "Test Refrigerated", localeID = Locales.WholeFoods },
                    new ItemTrait { traitID = Traits.SmithsonianBirdFriendly, traitValue = "1", localeID = Locales.WholeFoods },
                    new ItemTrait { traitID = Traits.WicEligible, traitValue = "Yes", localeID = Locales.WholeFoods },
                    new ItemTrait { traitID = Traits.ShelfLife, traitValue = "44", localeID = Locales.WholeFoods },
                    new ItemTrait { traitID = Traits.SelfCheckoutItemTareGroup, traitValue = "Test SelfCheckoutItemTareGroup", localeID = Locales.WholeFoods },
                    new ItemTrait { traitID = Traits.GlobalPricingProgram, traitValue = "Test GlobalPricingProgram", localeID = Locales.WholeFoods },
                    new ItemTrait { traitID = Traits.Line, traitValue = "Test Line", localeID = Locales.WholeFoods },
                    new ItemTrait { traitID = Traits.Sku, traitValue = "Test Sku", localeID = Locales.WholeFoods },
                    new ItemTrait { traitID = Traits.PriceLine, traitValue = "Test PriceLine", localeID = Locales.WholeFoods },
                    new ItemTrait { traitID = Traits.VariantSize, traitValue = "Test VariantSize", localeID = Locales.WholeFoods },
                    new ItemTrait { traitID = Traits.EstoreNutritionRequired, traitValue = "N", localeID = Locales.WholeFoods },
                    new ItemTrait { traitID = Traits.PrimeNowEligible, traitValue = "Y", localeID = Locales.WholeFoods },
                    new ItemTrait { traitID = Traits.EstoreEligible, traitValue = "Y", localeID = Locales.WholeFoods },
                    new ItemTrait { traitID = Traits.Tsf365Eligible, traitValue = "N", localeID = Locales.WholeFoods },
                    new ItemTrait { traitID = Traits.WfmEligilble, traitValue = "Y", localeID = Locales.WholeFoods },
                    new ItemTrait { traitID = Traits.Other3pEligible, traitValue = null, localeID = Locales.WholeFoods },
                    new ItemTrait { traitID = Traits.DataSource, traitValue = "Test Data Source", localeID = Locales.WholeFoods },
                    new ItemTrait { traitID = Traits.GmoTransparency, traitValue = "Test Gmo Transparency", localeID = Locales.WholeFoods },
                    new ItemTrait { traitID = Traits.ItemDepth, traitValue = "1.23", localeID = Locales.WholeFoods },
                    new ItemTrait { traitID = Traits.ItemHeight, traitValue = "1.24", localeID = Locales.WholeFoods },
                    new ItemTrait { traitID = Traits.ItemWidth, traitValue = "1.13", localeID = Locales.WholeFoods },
                    new ItemTrait { traitID = Traits.Cube, traitValue = "1.1", localeID = Locales.WholeFoods },
                    new ItemTrait { traitID = Traits.Weight, traitValue = "2.1", localeID = Locales.WholeFoods },
                    new ItemTrait { traitID = Traits.TrayDepth, traitValue = "2.2", localeID = Locales.WholeFoods },
                    new ItemTrait { traitID = Traits.TrayHeight, traitValue = "2.3", localeID = Locales.WholeFoods },
                    new ItemTrait { traitID = Traits.TrayWidth, traitValue = "2.4", localeID = Locales.WholeFoods },
                    new ItemTrait { traitID = Traits.Labeling, traitValue = "Test Labeling", localeID = Locales.WholeFoods },
                    new ItemTrait { traitID = Traits.CountryOfOrigin, traitValue = "Test Country of Origin", localeID = Locales.WholeFoods },
                    new ItemTrait { traitID = Traits.PackageGroup, traitValue = "45", localeID = Locales.WholeFoods },
                    new ItemTrait { traitID = Traits.PackageGroupType, traitValue = "Test Package Group Type", localeID = Locales.WholeFoods },
                    new ItemTrait { traitID = Traits.PrivateLabel, traitValue = "Test Private Label", localeID = Locales.WholeFoods },
                    new ItemTrait { traitID = Traits.Appellation, traitValue = "Test Appellation", localeID = Locales.WholeFoods },
                    new ItemTrait { traitID = Traits.FairTradeClaim, traitValue = null, localeID = Locales.WholeFoods },
                    new ItemTrait { traitID = Traits.GlutenFreeClaim, traitValue = null, localeID = Locales.WholeFoods },
                    new ItemTrait { traitID = Traits.NonGmoClaim, traitValue = null, localeID = Locales.WholeFoods },
                    new ItemTrait { traitID = Traits.OrganicClaim, traitValue = null, localeID = Locales.WholeFoods },
                    new ItemTrait { traitID = Traits.Varietal, traitValue = "Test Varietal", localeID = Locales.WholeFoods },
                    new ItemTrait { traitID = Traits.BeerStyle, traitValue = "Test BeerStyle", localeID = Locales.WholeFoods },
                    new ItemTrait { traitID = Traits.LineExtension, traitValue = "Test LineExtension", localeID = Locales.WholeFoods },
                },
                ItemSignAttribute = new List<ItemSignAttribute>
                {
                    new ItemSignAttribute
                    {
                        AirChilled = true,
                        AnimalWelfareRating = "Step5Plus",
                        Biodynamic = true,
                        MilkType = "CowGoatSheepMilk",
                        CheeseRaw = true,
                        CustomerFriendlyDescription = "Test CustomerFriendlyDescription",
                        DryAged = true,
                        EcoScaleRating = "UltraPremiumGreen",
                        FreeRange = true,
                        GlutenFreeAgencyName = "Test GlutenFreeAgencyName",
                        GrassFed = true,
                        HealthyEatingRatingId = HealthyEatingRatings.Best,
                        KosherAgencyName = "Test KosherAgencyName",
                        MadeInHouse = true,
                        Msc = true,
                        NonGmoAgencyName = "Test NonGmoAgencyName",
                        OrganicAgencyName = "Test OrganicAgencyName",
                        PastureRaised = true,
                        PremiumBodyCare = true,
                        SeafoodCatchType = "FarmRaised",
                        FreshOrFrozen = "Frozen",
                        VeganAgencyName = "Test VeganAgencyName",
                        Vegetarian = true,
                        WholeTrade = true
                    }
                }
            };

            context.Item.Add(item);
            context.SaveChanges();

            return item;
        }

        private HierarchyClass BuildAndSaveHierarchyClass(int hierarchyId, int hierarchyLevel, string name)
        {
            HierarchyClass hierarchyClass = new HierarchyClass
            {
                hierarchyClassName = name,
                hierarchyID = hierarchyId,
                hierarchyLevel = hierarchyLevel
            };

            context.HierarchyClass.Add(hierarchyClass);
            context.SaveChanges();

            return hierarchyClass;
        }
    }
}
