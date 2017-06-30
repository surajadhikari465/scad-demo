using GlobalEventController.Common;
using GlobalEventController.DataAccess.BulkCommands;
using Icon.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Transactions;

namespace GlobalEventController.Tests.DataAccess.BulkCommandTests
{
    [TestClass]
    public class BulkGetValidatedItemsQueryHandlerTests
    {
        private BulkGetValidatedItemsQueryHandler handler;
        private BulkGetValidatedItemsQuery query;
        private IconDbContextFactory contextFactory;
        private IconContext context;
        private TransactionScope transaction;
        private GlobalControllerSettings settings;

        [TestInitialize]
        public void InitializeData()
        {
            this.transaction = new TransactionScope();
            this.context = new IconContext();
            this.query = new BulkGetValidatedItemsQuery();
            this.settings = new GlobalControllerSettings { EnableInforUpdates = false };
            this.contextFactory = new IconDbContextFactory();
            this.handler = new BulkGetValidatedItemsQueryHandler(contextFactory, settings);
        }

        [TestCleanup]
        public void CleanupData()
        {
            transaction.Dispose();
        }

        [TestMethod]
        public void BulkGetValidatedItems_ValidatedScanCodes_ReturnsValidatedItemModelList()
        {
            // Given
            IQueryable<ItemHierarchyClass> brands = this.context.ItemHierarchyClass.Where(ihc => ihc.HierarchyClass.hierarchyID == Hierarchies.Brands);
            IQueryable<ItemHierarchyClass> taxes = this.context.ItemHierarchyClass.Where(ihc => ihc.HierarchyClass.hierarchyID == Hierarchies.Tax);
            IQueryable<Item> items = this.context.Item.Where(i => i.ItemTrait.Any(it => it.traitID == Traits.ValidationDate && it.traitValue != null && it.localeID == Locales.WholeFoods));

            var expectedItems = (from ihc in this.context.ItemHierarchyClass
                                                      join b in brands on ihc.itemID equals b.itemID
                                                      join t in taxes on ihc.itemID equals t.itemID
                                                      join vi in items on ihc.itemID equals vi.itemID
                                                      select new 
                                                      {
                                                          ItemId = vi.itemID,
                                                          ScanCode = vi.ScanCode.FirstOrDefault().scanCode,
                                                          ScanCodeType = vi.ScanCode.FirstOrDefault().ScanCodeType.scanCodeTypeDesc,
                                                          ProductDescription = vi.ItemTrait.FirstOrDefault(it => it.traitID == Traits.ProductDescription).traitValue,
                                                          PosDescription = vi.ItemTrait.FirstOrDefault(it => it.traitID == Traits.PosDescription).traitValue,
                                                          PackageUnit = vi.ItemTrait.FirstOrDefault(it => it.traitID == Traits.PackageUnit).traitValue,
                                                          FoodStampEligible = vi.ItemTrait.FirstOrDefault(it => it.traitID == Traits.FoodStampEligible).traitValue,
                                                          Tare = vi.ItemTrait.FirstOrDefault(it => it.traitID == Traits.PosScaleTare).traitValue,
                                                          BrandId = b.hierarchyClassID,
                                                          BrandName = b.HierarchyClass.hierarchyClassName,
                                                          TaxClassName = t.HierarchyClass.HierarchyClassTrait.FirstOrDefault(hct => hct.traitID == Traits.TaxAbbreviation).traitValue,
                                                          ValidationDate = vi.ItemTrait.FirstOrDefault(it => it.traitID == Traits.ValidationDate).traitValue,
                                                          RetailSize = vi.ItemTrait.FirstOrDefault(it => it.traitID == Traits.RetailSize).traitValue,
                                                          RetailUom = vi.ItemTrait.FirstOrDefault(it => it.traitID == Traits.RetailUom).traitValue,
                                                      }).Take(3).ToList();

            this.query.Events = expectedItems.Select(i => new EventQueue { EventMessage = i.ScanCode }).ToList();
            this.query.Events[0].EventId = EventTypes.ItemUpdate;
            this.query.Events[1].EventId = EventTypes.ItemValidation;
            this.query.Events[2].EventId = EventTypes.NewIrmaItem;

            // When
            List<ValidatedItemModel> actualItems = this.handler.Handle(query);

            // Then
            Assert.AreEqual(3, actualItems.Count);
            for (int i = 0; i < actualItems.Count; i++)
            {
                var actual = actualItems.First(ai => ai.ScanCode == expectedItems[i].ScanCode);
                Assert.AreEqual(expectedItems[i].ItemId, actual.ItemId, "The ItemId does not match the expected value.");
                Assert.AreEqual(expectedItems[i].ScanCode, actual.ScanCode, "The ScanCode does not match the expected value.");
                Assert.AreEqual(expectedItems[i].ScanCodeType, actual.ScanCodeType, "The ScanCodeType does not match the expected value.");
                Assert.AreEqual(expectedItems[i].ProductDescription, actual.ProductDescription, "The ProductDescription does not match the expected value.");
                Assert.AreEqual(expectedItems[i].PosDescription, actual.PosDescription, "The PosDescription does not match the expected value.");
                Assert.AreEqual(expectedItems[i].PackageUnit, actual.PackageUnit, "The PackageUnit does not match the expected value.");
                Assert.AreEqual(expectedItems[i].FoodStampEligible, actual.FoodStampEligible, "The FoodStampEligible does not match the expected value.");
                Assert.AreEqual(expectedItems[i].Tare, actual.Tare, "The Tare does not match the expected value.");
                Assert.AreEqual(expectedItems[i].BrandId, actual.BrandId, "The BrandId does not match the expected value.");
                Assert.AreEqual(expectedItems[i].BrandName, actual.BrandName, "The BrandName does not match the expected value.");
                Assert.AreEqual(expectedItems[i].TaxClassName, actual.TaxClassName, "The TaxClassName does not match the expected value.");
                Assert.AreEqual(expectedItems[i].ValidationDate, actual.ValidationDate, "The ValidationDate does not match the expected value.");
                Assert.AreEqual(Convert.ToDecimal(expectedItems[i].RetailSize), actual.RetailSize, "The RetailSize does not match the expected value.");
                Assert.AreEqual(expectedItems[i].RetailUom, actual.RetailUom, "The RetailUom does not match the expected value.");

                var eventTypeId = this.query.Events.First(e => e.EventMessage == expectedItems[i].ScanCode).EventId;
                Assert.AreEqual(eventTypeId, actual.EventTypeId);
            }
        }

        [TestMethod]
        public void BulkGetValidatedItems_ItemsHaveItemSignAttributes_ReturnsItemsWithItemSignAttributesPopulated()
        {
            //Given
            var expectedItems = new List<ScanCode>
            {
                SetupValidatedItemInDb("123498763", true),
                SetupValidatedItemInDb("123498764", true),
                SetupValidatedItemInDb("123498765", true)

            };

            query.Events = expectedItems
                .Select(i => new EventQueue { EventId = EventTypes.ItemUpdate, EventMessage = i.scanCode })
                .ToList();

            //When
            List<ValidatedItemModel> actualItems = this.handler.Handle(query).OrderBy(i => i.ScanCode).ToList();

            //Then
            Assert.AreEqual(3, actualItems.Count);
            for(int i = 0; i < actualItems.Count; i++)
            {
                var signAttributes = expectedItems[i].Item.ItemSignAttribute.First();
                Assert.IsTrue(actualItems[i].HasItemSignAttributes);
                Assert.AreEqual(expectedItems[i].itemID, actualItems[i].ItemId);
                Assert.AreEqual(expectedItems[i].scanCode, actualItems[i].ScanCode);
                Assert.AreEqual(AnimalWelfareRatings.Descriptions.Step1, actualItems[i].AnimalWelfareRating);
                Assert.AreEqual(HealthyEatingRatings.Descriptions.Good, actualItems[i].HealthyEatingRating);
                Assert.IsTrue(actualItems[i].Biodynamic.Value);
                Assert.AreEqual(MilkTypes.Descriptions.CowGoatSheepMilk, actualItems[i].CheeseMilkType);
                Assert.IsTrue(actualItems[i].CheeseRaw.Value);
                Assert.AreEqual(EcoScaleRatings.Descriptions.PremiumYellow, actualItems[i].EcoScaleRating);
                Assert.IsTrue(actualItems[i].GlutenFree.Value);
                Assert.IsTrue(actualItems[i].Kosher.Value);
                Assert.IsTrue(actualItems[i].Msc.Value);
                Assert.IsTrue(actualItems[i].NonGmo.Value);
                Assert.IsTrue(actualItems[i].Organic.Value);
                Assert.IsTrue(actualItems[i].PremiumBodyCare.Value);
                Assert.AreEqual(SeafoodFreshOrFrozenTypes.Descriptions.Frozen, actualItems[i].FreshOrFrozen);
                Assert.AreEqual(SeafoodCatchTypes.Descriptions.Wild, actualItems[i].SeafoodCatchType);
                Assert.IsTrue(actualItems[i].Vegan.Value);
                Assert.IsTrue(actualItems[i].Vegetarian.Value);
                Assert.IsTrue(actualItems[i].WholeTrade.Value);
                Assert.IsTrue(actualItems[i].GrassFed.Value);
                Assert.IsTrue(actualItems[i].PastureRaised.Value);
                Assert.IsTrue(actualItems[i].FreeRange.Value);
                Assert.IsTrue(actualItems[i].DryAged.Value);
                Assert.IsTrue(actualItems[i].AirChilled.Value);
                Assert.IsTrue(actualItems[i].MadeInHouse.Value);
            }
        }

        [TestMethod]
        public void BulkGetValidatedItems_ItemsDontHaveItemSignAttributes_ReturnsItemsWithNullItemSignAttributes()
        {
            //Given
            var expectedItems = new List<ScanCode>
            {
                SetupValidatedItemInDb("123498763", false),
                SetupValidatedItemInDb("123498764", false),
                SetupValidatedItemInDb("123498765", false)
            };

            query.Events = expectedItems
                .Select(i => new EventQueue { EventId = EventTypes.ItemUpdate, EventMessage = i.scanCode })
                .ToList();

            //When
            List<ValidatedItemModel> actualItems = this.handler.Handle(query).OrderBy(i => i.ScanCode).ToList();

            //Then
            Assert.AreEqual(3, actualItems.Count);
            for (int i = 0; i < actualItems.Count; i++)
            {
                Assert.AreEqual(expectedItems[i].itemID, actualItems[i].ItemId);
                Assert.AreEqual(expectedItems[i].scanCode, actualItems[i].ScanCode);
                Assert.IsFalse(actualItems[i].HasItemSignAttributes);
                Assert.IsNull(actualItems[i].AnimalWelfareRating);
                Assert.IsNull(actualItems[i].Biodynamic);
                Assert.IsNull(actualItems[i].CheeseMilkType);
                Assert.IsNull(actualItems[i].CheeseRaw);
                Assert.IsNull(actualItems[i].EcoScaleRating);
                Assert.IsNull(actualItems[i].GlutenFree);
                Assert.IsNull(actualItems[i].Kosher);
                Assert.IsNull(actualItems[i].Msc);
                Assert.IsNull(actualItems[i].NonGmo);
                Assert.IsNull(actualItems[i].Organic);
                Assert.IsNull(actualItems[i].PremiumBodyCare);
                Assert.IsNull(actualItems[i].FreshOrFrozen);
                Assert.IsNull(actualItems[i].SeafoodCatchType);
                Assert.IsNull(actualItems[i].Vegan);
                Assert.IsNull(actualItems[i].Vegetarian);
                Assert.IsNull(actualItems[i].WholeTrade);
                Assert.IsNull(actualItems[i].GrassFed);
                Assert.IsNull(actualItems[i].PastureRaised);
                Assert.IsNull(actualItems[i].FreeRange);
                Assert.IsNull(actualItems[i].DryAged);
                Assert.IsNull(actualItems[i].AirChilled);
                Assert.IsNull(actualItems[i].MadeInHouse);
            }
        }

        [TestMethod]
        public void BulkGetValidatedItems_ItemsHaveFalseItemSignAttributes_ReturnsItemsWithFalseSignAttributes()
        {
            //Given
            var expectedItems = new List<ScanCode>
            {
                SetupValidatedItemInDb("123498763", false),
                SetupValidatedItemInDb("123498764", false),
                SetupValidatedItemInDb("123498765", false)
            };

            foreach (var item in expectedItems)
            {
                item.Item.ItemSignAttribute.Add(CreateFalseSignAttributes());
            }
            context.SaveChanges();

            query.Events = expectedItems
                .Select(i => new EventQueue { EventId = EventTypes.ItemUpdate, EventMessage = i.scanCode })
                .ToList();

            //When
            List<ValidatedItemModel> actualItems = this.handler.Handle(query).OrderBy(i => i.ScanCode).ToList();

            //Then
            Assert.AreEqual(3, actualItems.Count);
            for (int i = 0; i < actualItems.Count; i++)
            {
                var signAttributes = expectedItems[i].Item.ItemSignAttribute.First();
                Assert.AreEqual(expectedItems[i].itemID, actualItems[i].ItemId);
                Assert.AreEqual(expectedItems[i].scanCode, actualItems[i].ScanCode);
                Assert.IsTrue(actualItems[i].HasItemSignAttributes);
                Assert.IsNull(actualItems[i].AnimalWelfareRating);
                Assert.IsFalse(actualItems[i].Biodynamic.Value);
                Assert.IsNull(actualItems[i].CheeseMilkType);
                Assert.IsFalse(actualItems[i].CheeseRaw.Value);
                Assert.IsNull(actualItems[i].EcoScaleRating);
                Assert.IsNull(actualItems[i].GlutenFree);
                Assert.IsNull(actualItems[i].Kosher);
                Assert.IsFalse(actualItems[i].Msc.Value);
                Assert.IsNull(actualItems[i].NonGmo);
                Assert.IsNull(actualItems[i].Organic);
                Assert.IsFalse(actualItems[i].PremiumBodyCare.Value);
                Assert.IsNull(actualItems[i].FreshOrFrozen);
                Assert.IsNull(actualItems[i].SeafoodCatchType);
                Assert.IsNull(actualItems[i].Vegan);
                Assert.IsFalse(actualItems[i].Vegetarian.Value);
                Assert.IsFalse(actualItems[i].WholeTrade.Value);
                Assert.IsFalse(actualItems[i].GrassFed.Value);
                Assert.IsFalse(actualItems[i].PastureRaised.Value);
                Assert.IsFalse(actualItems[i].FreeRange.Value);
                Assert.IsFalse(actualItems[i].DryAged.Value);
                Assert.IsFalse(actualItems[i].AirChilled.Value);
                Assert.IsFalse(actualItems[i].MadeInHouse.Value);
            }
        }

        private ScanCode SetupValidatedItemInDb(string testScanCode, bool addItemSignAttributes)
        {
            string validationDate = DateTime.Now.ToString() + testScanCode;
            string productDescription = "Test Product Description" + testScanCode;
            string posDescription = "Test POS Description" + testScanCode;
            string packageUnit = "Test Package Unit" + testScanCode;
            string foodStampEligible = "Test FSE" + testScanCode;
            string posScaleTare = "Test Tare";
            string taxAbbreviation = "Test Tax Abbr" + testScanCode;
            string subTeamName = "Test SubTeam" + testScanCode;
            string retailSize = "123";
            string retailUom = "Test Retail Uom" + testScanCode;

            HierarchyClass brand = new HierarchyClass { hierarchyID = Hierarchies.Brands, hierarchyClassName = "Test Brand" + testScanCode };
            HierarchyClass tax = new HierarchyClass
            {
                hierarchyID = Hierarchies.Tax,
                hierarchyClassName = "Test Tax" + testScanCode,
                HierarchyClassTrait = new List<HierarchyClassTrait>
                {
                    new HierarchyClassTrait { traitID = Traits.TaxAbbreviation, traitValue = taxAbbreviation }
                }
            };
            HierarchyClass merchandise = new HierarchyClass
            {
                hierarchyID = Hierarchies.Merchandise,
                hierarchyClassName = "Test Merchandise" + testScanCode,
                HierarchyClassTrait = new List<HierarchyClassTrait>
                {
                    new HierarchyClassTrait { traitID = Traits.MerchFinMapping, traitValue = subTeamName }
                }
            };

            HierarchyClass glutenAgency = new HierarchyClass { hierarchyID = Hierarchies.CertificationAgencyManagement, hierarchyClassName = "Test Gluten" + testScanCode };
            HierarchyClass kosherAgency = new HierarchyClass { hierarchyID = Hierarchies.CertificationAgencyManagement, hierarchyClassName = "Kosher Agency" + testScanCode };
            HierarchyClass nonGmoAgency = new HierarchyClass { hierarchyID = Hierarchies.CertificationAgencyManagement, hierarchyClassName = "NonGmo Agency" + testScanCode };
            HierarchyClass organicAgency = new HierarchyClass { hierarchyID = Hierarchies.CertificationAgencyManagement, hierarchyClassName = "Organic Agency" + testScanCode };
            HierarchyClass veganAgency = new HierarchyClass { hierarchyID = Hierarchies.CertificationAgencyManagement, hierarchyClassName = "Vegan Agency" + testScanCode };

            context.HierarchyClass.AddRange(new List<HierarchyClass> { brand, tax, merchandise, glutenAgency, kosherAgency, nonGmoAgency, organicAgency, veganAgency });
            context.SaveChanges();

            ScanCode scanCode = new ScanCode
            {
                scanCode = testScanCode,
                ScanCodeType = new ScanCodeType { scanCodeTypeDesc = "Test ScanCode Type Description" + testScanCode },
                Item = new Item
                {
                    itemTypeID = ItemTypes.RetailSale,
                    ItemTrait = new List<ItemTrait>
                    {
                        new ItemTrait { localeID = Locales.WholeFoods, traitID = Traits.ValidationDate, traitValue = validationDate },
                        new ItemTrait { localeID = Locales.WholeFoods, traitID = Traits.ProductDescription, traitValue = productDescription },
                        new ItemTrait { localeID = Locales.WholeFoods, traitID = Traits.PosDescription, traitValue = posDescription },
                        new ItemTrait { localeID = Locales.WholeFoods, traitID = Traits.PackageUnit, traitValue = packageUnit },
                        new ItemTrait { localeID = Locales.WholeFoods, traitID = Traits.FoodStampEligible, traitValue = foodStampEligible },
                        new ItemTrait { localeID = Locales.WholeFoods, traitID = Traits.PosScaleTare, traitValue = posScaleTare },
                        new ItemTrait { localeID = Locales.WholeFoods, traitID = Traits.RetailSize, traitValue = retailSize },
                        new ItemTrait { localeID = Locales.WholeFoods, traitID = Traits.RetailUom, traitValue = retailUom },
                    },
                    ItemHierarchyClass = new List<ItemHierarchyClass>
                    {
                        new ItemHierarchyClass { hierarchyClassID = brand.hierarchyClassID },
                        new ItemHierarchyClass { hierarchyClassID = tax.hierarchyClassID },
                        new ItemHierarchyClass { hierarchyClassID = merchandise.hierarchyClassID }
                    }
                }
            };

            if (addItemSignAttributes)
            {
                ItemSignAttribute signAttributes = new ItemSignAttribute
                {
                    AnimalWelfareRatingId = AnimalWelfareRatings.Step1,
                    HealthyEatingRatingId = HealthyEatingRatings.Good,
                    Biodynamic = true,
                    CheeseMilkTypeId = MilkTypes.CowGoatSheepMilk,
                    CheeseRaw = true,
                    EcoScaleRatingId = EcoScaleRatings.PremiumYellow,
                    GlutenFreeAgencyId = glutenAgency.hierarchyClassID,
                    KosherAgencyId = kosherAgency.hierarchyClassID,
                    Msc = true,
                    NonGmoAgencyId = nonGmoAgency.hierarchyClassID,
                    OrganicAgencyId = organicAgency.hierarchyClassID,
                    PremiumBodyCare = true,
                    SeafoodFreshOrFrozenId = SeafoodFreshOrFrozenTypes.Frozen,
                    SeafoodCatchTypeId = SeafoodCatchTypes.Wild,
                    VeganAgencyId = veganAgency.hierarchyClassID,
                    Vegetarian = true,
                    WholeTrade = true,
                    GrassFed = true,
                    PastureRaised = true,
                    FreeRange = true,
                    DryAged = true,
                    AirChilled = true,
                    MadeInHouse = true
                };
                scanCode.Item.ItemSignAttribute.Add(signAttributes);
            }
            context.ScanCode.Add(scanCode);
            context.SaveChanges();

            return scanCode;
        }

        private ItemSignAttribute CreateFalseSignAttributes()
        {
            return new ItemSignAttribute
            {
                AnimalWelfareRatingId = null,
                HealthyEatingRatingId = null,
                Biodynamic = false,
                CheeseMilkTypeId = null,
                CheeseRaw = false,
                EcoScaleRatingId = null,
                GlutenFreeAgencyId = null,
                KosherAgencyId = null,
                Msc = false,
                NonGmoAgencyId = null,
                OrganicAgencyId = null,
                PremiumBodyCare = false,
                SeafoodFreshOrFrozenId = null,
                SeafoodCatchTypeId = null,
                VeganAgencyId = null,
                Vegetarian = false,
                WholeTrade = false,
                GrassFed = false,
                PastureRaised = false,
                FreeRange = false,
                DryAged = false,
                AirChilled = false,
                MadeInHouse = false
            };
        }
    }
}
