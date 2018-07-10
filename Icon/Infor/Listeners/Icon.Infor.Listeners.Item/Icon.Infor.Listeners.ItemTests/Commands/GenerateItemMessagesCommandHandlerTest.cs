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
                    Items = new List<ItemModel>{ expectedItem }
                });

            //Then
            var eventQueue = this.context.EventQueue.Where(mqp => mqp.EventReferenceId == item.itemID).ToList();
            var messageQueueProducts = this.context.MessageQueueProduct.Where(mqp => mqp.ItemId == item.itemID).ToList();

            Assert.AreEqual(subscriptions.Count, eventQueue.Count());
            Assert.AreEqual(1, messageQueueProducts.Count());

            var messageQueueProduct = messageQueueProducts.First();
            var itemSignAttributes = item.ItemSignAttribute.First();
            Assert.AreEqual(itemSignAttributes.AirChilled.ToBoolString(), messageQueueProduct.AirChilled);
            Assert.AreEqual(AnimalWelfareRatings.AsDictionary[itemSignAttributes.AnimalWelfareRatingId.Value], messageQueueProduct.AnimalWelfareRating);
            Assert.AreEqual(itemSignAttributes.Biodynamic.ToBoolString(), messageQueueProduct.Biodynamic);
            Assert.AreEqual(MilkTypes.AsDictionary[itemSignAttributes.CheeseMilkTypeId.Value], messageQueueProduct.CheeseMilkType);
            Assert.AreEqual(itemSignAttributes.CheeseRaw.ToBoolString(), messageQueueProduct.CheeseRaw);
            Assert.AreEqual(itemSignAttributes.CustomerFriendlyDescription, messageQueueProduct.CustomerFriendlyDescription);
            Assert.AreEqual(itemSignAttributes.DryAged.ToBoolString(), messageQueueProduct.DryAged);
            Assert.AreEqual(EcoScaleRatings.AsDictionary[itemSignAttributes.EcoScaleRatingId.Value], messageQueueProduct.EcoScaleRating);
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
            Assert.AreEqual(SeafoodCatchTypes.AsDictionary[itemSignAttributes.SeafoodCatchTypeId.Value], messageQueueProduct.SeafoodCatchType);
            Assert.AreEqual(SeafoodFreshOrFrozenTypes.AsDictionary[itemSignAttributes.SeafoodFreshOrFrozenId.Value], messageQueueProduct.SeafoodFreshOrFrozen);
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
                },
                ItemSignAttribute = new List<ItemSignAttribute>
                {
                    new ItemSignAttribute
                    {
                        AirChilled = true,
                        AnimalWelfareRatingId = AnimalWelfareRatings.Step5Plus,
                        Biodynamic = true,
                        CheeseMilkTypeId = MilkTypes.CowGoatSheepMilk,
                        CheeseRaw = true,
                        CustomerFriendlyDescription = "Test CustomerFriendlyDescription",
                        DryAged = true,
                        EcoScaleRatingId = EcoScaleRatings.UltraPremiumGreen,
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
                        SeafoodCatchTypeId = SeafoodCatchTypes.FarmRaised,
                        SeafoodFreshOrFrozenId = SeafoodFreshOrFrozenTypes.Frozen,
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
