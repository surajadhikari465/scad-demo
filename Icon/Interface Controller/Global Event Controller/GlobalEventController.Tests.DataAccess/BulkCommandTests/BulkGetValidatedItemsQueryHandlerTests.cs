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
                SetupValidatedItemInDb("123498763", SignAttributeTestDataFlag.On),
                SetupValidatedItemInDb("123498764", SignAttributeTestDataFlag.On),
                SetupValidatedItemInDb("123498765", SignAttributeTestDataFlag.On)
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
                AssertSignAttributesMatchExpected(expectedItems[i], actualItems[i], SignAttributeTestDataFlag.On);
            }
        }

        [TestMethod]
        public void BulkGetValidatedItems_ItemsDontHaveItemSignAttributes_ReturnsItemsWithNullItemSignAttributes()
        {
            //Given
            var expectedItems = new List<ScanCode>
            {
                SetupValidatedItemInDb("123498763", SignAttributeTestDataFlag.Null),
                SetupValidatedItemInDb("123498764", SignAttributeTestDataFlag.Null),
                SetupValidatedItemInDb("123498765", SignAttributeTestDataFlag.Null)
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
                AssertSignAttributesMatchExpected(expectedItems[i], actualItems[i], SignAttributeTestDataFlag.Null);
            }
        }

        [TestMethod]
        public void BulkGetValidatedItems_ItemsHaveFalseItemSignAttributes_ReturnsItemsWithFalseSignAttributes()
        {
            //Given
            var expectedItems = new List<ScanCode>
            {
                SetupValidatedItemInDb("123498763", SignAttributeTestDataFlag.Off),
                SetupValidatedItemInDb("123498764", SignAttributeTestDataFlag.Off),
                SetupValidatedItemInDb("123498765", SignAttributeTestDataFlag.Off)
            };
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
                AssertSignAttributesMatchExpected(expectedItems[i], actualItems[i], SignAttributeTestDataFlag.Off);
            }
        }

        [TestMethod]
        public void BulkGetValidatedItems_ItemsHaveDefaultItemSignAttributes_ExceptAgencyNamesBlank_ShouldReturnNull()
        {
            //Given
            var customSignAttrValues = new List<SignAttributeValidatedItemPair> {
                new SignAttributeValidatedItemPair(nameof(ItemSignAttribute.GlutenFreeAgencyName), "", nameof(ValidatedItemModel.GlutenFree), (bool?)null),
                new SignAttributeValidatedItemPair(nameof(ItemSignAttribute.KosherAgencyName), "", nameof(ValidatedItemModel.Kosher), (bool?)null),
                new SignAttributeValidatedItemPair(nameof(ItemSignAttribute.NonGmoAgencyName), "", nameof(ValidatedItemModel.NonGmo), (bool?)null),
                new SignAttributeValidatedItemPair(nameof(ItemSignAttribute.OrganicAgencyName), "", nameof(ValidatedItemModel.Organic), (bool?)null),
                new SignAttributeValidatedItemPair(nameof(ItemSignAttribute.VeganAgencyName), "", nameof(ValidatedItemModel.Vegan), (bool?)null)
            };
            var expectedItem = SetupValidatedItemInDb("123498763", SignAttributeTestDataFlag.On, null, customSignAttrValues);
            context.SaveChanges();

            query.Events = new List<EventQueue> {
                new EventQueue { EventId = EventTypes.ItemUpdate, EventMessage = expectedItem.scanCode }
            };

            //When
            var actualItem = this.handler.Handle(query).OrderBy(i => i.ScanCode).FirstOrDefault();

            //Then
            AssertSignAttributesMatchExpected(expectedItem, actualItem, SignAttributeTestDataFlag.On, customSignAttrValues);
        }

        [TestMethod]
        public void BulkGetValidatedItems_ItemsHaveDefaultItemSignAttributes_ExceptAgencyNamesNo_ShouldReturnFalse()
        {
            //Given
            var customSignAttrValues = new List<SignAttributeValidatedItemPair> {
                new SignAttributeValidatedItemPair(nameof(ItemSignAttribute.GlutenFreeAgencyName), "No", nameof(ValidatedItemModel.GlutenFree), false),
                new SignAttributeValidatedItemPair(nameof(ItemSignAttribute.KosherAgencyName), "No", nameof(ValidatedItemModel.Kosher), false),
                new SignAttributeValidatedItemPair(nameof(ItemSignAttribute.NonGmoAgencyName), "No", nameof(ValidatedItemModel.NonGmo), false),
                new SignAttributeValidatedItemPair(nameof(ItemSignAttribute.OrganicAgencyName), "No", nameof(ValidatedItemModel.Organic), false),
                new SignAttributeValidatedItemPair(nameof(ItemSignAttribute.VeganAgencyName), "No", nameof(ValidatedItemModel.Vegan), false)
            };
            var expectedItem = SetupValidatedItemInDb("123498763", SignAttributeTestDataFlag.On, null, customSignAttrValues);
            context.SaveChanges();

            query.Events = new List<EventQueue> {
                new EventQueue { EventId = EventTypes.ItemUpdate, EventMessage = expectedItem.scanCode }
            };

            //When
            var actualItem = this.handler.Handle(query).OrderBy(i => i.ScanCode).FirstOrDefault();

            //Then
            AssertSignAttributesMatchExpected(expectedItem, actualItem, SignAttributeTestDataFlag.On, customSignAttrValues);
        }

        [TestMethod]
        public void BulkGetValidatedItems_ItemsHaveFalseItemSignAttributes_ExceptAgencyNamesHaveValues_ShouldReturnTrue()
        {
            //Given
            var customSignAttrValues = new List<SignAttributeValidatedItemPair> {
                new SignAttributeValidatedItemPair(nameof(ItemSignAttribute.GlutenFreeAgencyName), "Acme Gluten Free Agency", nameof(ValidatedItemModel.GlutenFree), true),
                new SignAttributeValidatedItemPair(nameof(ItemSignAttribute.KosherAgencyName), "Acme KosherAgency", nameof(ValidatedItemModel.Kosher), true),
                new SignAttributeValidatedItemPair(nameof(ItemSignAttribute.NonGmoAgencyName), "Acme GMO Agency", nameof(ValidatedItemModel.NonGmo), true),
                new SignAttributeValidatedItemPair(nameof(ItemSignAttribute.OrganicAgencyName), "Acme Organic Agency", nameof(ValidatedItemModel.Organic), true),
                new SignAttributeValidatedItemPair(nameof(ItemSignAttribute.VeganAgencyName), "Acme Vegan Agency", nameof(ValidatedItemModel.Vegan), true)
            };
            var expectedItem = SetupValidatedItemInDb("123498763", SignAttributeTestDataFlag.Off, null, customSignAttrValues);
            context.SaveChanges();

            query.Events = new List<EventQueue> {
                new EventQueue { EventId = EventTypes.ItemUpdate, EventMessage = expectedItem.scanCode }
            };

            //When
            var actualItem = this.handler.Handle(query).OrderBy(i => i.ScanCode).FirstOrDefault();

            //Then
            AssertSignAttributesMatchExpected(expectedItem, actualItem, SignAttributeTestDataFlag.Off, customSignAttrValues);
        }

        [TestMethod]
        public void BulkGetValidatedItems_ItemsHaveFalseItemSignAttributes_ExceptAgencyNamesYes_ShouldReturnTrue()
        {
            //Given
            var customSignAttrValues = new List<SignAttributeValidatedItemPair> {
                new SignAttributeValidatedItemPair(nameof(ItemSignAttribute.GlutenFreeAgencyName), "Yes", nameof(ValidatedItemModel.GlutenFree), true),
                new SignAttributeValidatedItemPair(nameof(ItemSignAttribute.KosherAgencyName), "Yes", nameof(ValidatedItemModel.Kosher), true),
                new SignAttributeValidatedItemPair(nameof(ItemSignAttribute.NonGmoAgencyName), "Yes", nameof(ValidatedItemModel.NonGmo), true),
                new SignAttributeValidatedItemPair(nameof(ItemSignAttribute.OrganicAgencyName), "Yes", nameof(ValidatedItemModel.Organic), true),
                new SignAttributeValidatedItemPair(nameof(ItemSignAttribute.VeganAgencyName), "Yes", nameof(ValidatedItemModel.Vegan), true)
            };
            var expectedItem = SetupValidatedItemInDb("123498763", SignAttributeTestDataFlag.Off, null, customSignAttrValues);
            context.SaveChanges();

            query.Events = new List<EventQueue> {
                new EventQueue { EventId = EventTypes.ItemUpdate, EventMessage = expectedItem.scanCode }
            };

            //When
            var actualItem = this.handler.Handle(query).OrderBy(i => i.ScanCode).FirstOrDefault();

            //Then
            AssertSignAttributesMatchExpected(expectedItem, actualItem, SignAttributeTestDataFlag.Off, customSignAttrValues);
        }

        [TestMethod]
        public void BulkGetValidatedItems_ItemHasCustomerFriendlyDescriptionTrait_ReturnsValidatedItemWithExpectedSignAttributeValue()
        {
            //Given
            const string sampleIdentifier = "123498763";
            var expectedCfdValue = "Test CFD for " + sampleIdentifier;
            var customSignAttrValues = new List<SignAttributeValidatedItemPair> {
                new SignAttributeValidatedItemPair(nameof(ItemSignAttribute.CustomerFriendlyDescription), expectedCfdValue, nameof(ValidatedItemModel.CustomerFriendlyDescription), expectedCfdValue),
                };
            var expectedItems = new List<ScanCode>
            {
                SetupValidatedItemInDb(sampleIdentifier, SignAttributeTestDataFlag.On, null, customSignAttrValues)
            };

            query.Events = expectedItems
                .Select(i => new EventQueue { EventId = EventTypes.ItemUpdate, EventMessage = i.scanCode })
                .ToList();

            //When
            List<ValidatedItemModel> actualItems = this.handler.Handle(query).OrderBy(i => i.ScanCode).ToList();

            //Then 
            Assert.AreEqual(expectedCfdValue, actualItems[0].CustomerFriendlyDescription, nameof(ValidatedItemModel.CustomerFriendlyDescription));
        }

        [TestMethod]
        public void BulkGetValidatedItems_ItemLacksCustomerFriendlyDescriptionTrait_ReturnsValidatedItemWithNullTraitValue()
        {
            //Given
            const string sampleIdentifier = "123498763";
            var customSignAttrValues = new List<SignAttributeValidatedItemPair> {
                new SignAttributeValidatedItemPair(nameof(ItemSignAttribute.CustomerFriendlyDescription), (string)null, nameof(ValidatedItemModel.CustomerFriendlyDescription), (string)null),
                };
            var expectedItems = new List<ScanCode>
            {
                SetupValidatedItemInDb(sampleIdentifier, SignAttributeTestDataFlag.On, null, customSignAttrValues)
            };

            query.Events = expectedItems
                .Select(i => new EventQueue { EventId = EventTypes.ItemUpdate, EventMessage = i.scanCode })
                .ToList();

            //When
            List<ValidatedItemModel> actualItems = this.handler.Handle(query).OrderBy(i => i.ScanCode).ToList();

            //Then 
            Assert.IsNull(actualItems[0].CustomerFriendlyDescription, nameof(ValidatedItemModel.CustomerFriendlyDescription));
        }

        /// <summary>
        /// Creates a ScanCode object and saves it to the Icon database context for use in subsequent testing.
        ///   The ScanCode will have values populated for standard ItemTraits,
        ///   required HierarchyClasses (brand/tax/merch), and ItemSignAttribute data as specified
        /// </summary>
        /// <param name="identifier">scanCode/identifier for the item</param> 
        /// <param name="signAttrDataFlag">flag indicating the values to use for ScanCode.Item.ItemSignAttribute
        ///   members: On/Populated, Off/False, Null/No Sign Attribute data</param>
        /// <param name="additionalTraitsToInclude">Any additional trait ids/values to include in 
        ///    the created validated item, other than the default traits of ValidationDate, ProductDescription,
        ///    PosDescription, PackageUnit, FoodStampEligible, PosScaleTare, RetailSize, RetailUom which 
        ///    will be added by default with sample values</param>
        /// <param name="customSignAttributeValues">Optional list of property name/value pairs to use for 
        ///    ScanCode.Item.ItemSignAttribute members. When left null, the standard values appropriate to the 
        ///    data flag parameter will be used for all properties. Any properties not listed in the custom values
        ///    will default to the standard value for the sign attribute data flag.</param>
        /// <returns>ScanCode object which has been saved to the test context</returns>
        private ScanCode SetupValidatedItemInDb(string identifier,
            SignAttributeTestDataFlag signAttrDataFlag,
            List<Tuple<int, string>> additionalTraitsToInclude = null,
            List<SignAttributeValidatedItemPair> customSignAttributeValues = null)
        {
            var itemTraits = CreateSampleItemTraits(identifier, additionalTraitsToInclude);

            var hierarchiesForBrandTaxMerch = CreateBrandTaxMerchHierarchyClassesForTestItem(identifier);
            context.HierarchyClass.AddRange(hierarchiesForBrandTaxMerch);

            context.SaveChanges();

            var scanCode = CreateScanCodeModelForValidatedItem(identifier, itemTraits, hierarchiesForBrandTaxMerch);
            if (signAttrDataFlag != SignAttributeTestDataFlag.Null)
            {
                var signAttributes = CreateItemSignAttributes(signAttrDataFlag, customSignAttributeValues);
                scanCode.Item.ItemSignAttribute.Add(signAttributes);
            }

            context.ScanCode.Add(scanCode);
            context.SaveChanges();

            return scanCode;
        }

        /// <summary>
        /// Creates a list of 3 HierarchyClass objects with sample data for the brand, tax, and merchandise
        ///   HierarchyClasses associated with a test data item
        /// </summary>
        /// <param name="scanCode">identifier/scanCode for the test item</param>
        /// <param name="customTaxAbbr">custom value to use for the tax hierarchy class name
        ///   (when left null, defaults to "Test Tax Abbr" + scanCode</param>
        /// <param name="customSubTeamName">custom value to use for the merch hierarchy class 
        ///    HierarchyTrait traitValue (when left null, defaults to "Test SubTeam" + scanCode</param>
        /// <returns>List of 3 HierarchyClass objects with populated values for 
        ///   hierarchyID, hierarchyClassName, and HierarchyClassTrait (for tax & merch only):
        ///   one each for brand, tax, merch</returns>
        private List<HierarchyClass> CreateBrandTaxMerchHierarchyClassesForTestItem(string scanCode,
            string customTaxAbbr = null, string customSubTeamName = null)
        {
            string brandClassName = "Test Brand" + scanCode;
            string taxClassName = "Test Tax" + scanCode;
            string merchClassName = "Test Merchandise" + scanCode;
            string taxAbbrVal = customTaxAbbr ?? "Test Tax Abbr" + scanCode;
            string merchSubTeamNameVal = customSubTeamName ?? "Test SubTeam" + scanCode;

            HierarchyClass brand = new HierarchyClass
            {
                hierarchyID = Hierarchies.Brands,
                hierarchyClassName = brandClassName
            };
            HierarchyClass tax = new HierarchyClass
            {
                hierarchyID = Hierarchies.Tax,
                hierarchyClassName = taxClassName,
                HierarchyClassTrait = new List<HierarchyClassTrait>
                {
                    new HierarchyClassTrait { traitID = Traits.TaxAbbreviation, traitValue = taxAbbrVal }
                }
            };
            HierarchyClass merchandise = new HierarchyClass
            {
                hierarchyID = Hierarchies.Merchandise,
                hierarchyClassName = merchClassName,
                HierarchyClassTrait = new List<HierarchyClassTrait>
                {
                    new HierarchyClassTrait { traitID = Traits.MerchFinMapping, traitValue = merchSubTeamNameVal}
                }
            };

            return new List<HierarchyClass> { brand, tax, merchandise };
        }

        /// <summary>
        /// Creates a ScanCode object containing populated values for scanCode, ScanCodeType, and Item
        ///   including Item.ItemTrait and Item.ItemHierarchyClass
        /// </summary>
        /// <param name="identifier">identifier/scan code for the test data item</param>
        /// <param name="itemTraits">list of ItemTraits to use for ScanCode.Item.ItemTrait property</param>
        /// <param name="brandTaxAndMerchHierarchies">list of brand, tax, merch HierarchyClass objects
        ///    to use for the ScanCode.Item.ItemHierarchyClass property</param>
        /// <returns>ScanCode object</returns>
        private ScanCode CreateScanCodeModelForValidatedItem(string identifier,
            List<ItemTrait> itemTraits, List<HierarchyClass> brandTaxAndMerchHierarchies)
        {
            HierarchyClass brand = brandTaxAndMerchHierarchies.Single(hc => hc.hierarchyID == Hierarchies.Brands);
            HierarchyClass tax = brandTaxAndMerchHierarchies.Single(hc => hc.hierarchyID == Hierarchies.Tax);
            HierarchyClass merchandise = brandTaxAndMerchHierarchies.Single(hc => hc.hierarchyID == Hierarchies.Merchandise);

            ScanCode scanCode = new ScanCode
            {
                scanCode = identifier,
                ScanCodeType = new ScanCodeType { scanCodeTypeDesc = "Test ScanCode Type Description" + identifier },
                Item = new Item
                {
                    itemTypeID = ItemTypes.RetailSale,
                    //ItemType = new ItemType
                    //{
                    //    itemTypeID = ItemTypes.RetailSale,
                    //    itemTypeCode = ItemTypes.Codes.RetailSale,
                    //    itemTypeDesc = ItemTypes.Descriptions.RetailSale
                    //},
                    ItemTrait = itemTraits,
                    ItemHierarchyClass = new List<ItemHierarchyClass>
                    {
                        new ItemHierarchyClass { hierarchyClassID = brand.hierarchyClassID },
                        new ItemHierarchyClass { hierarchyClassID = tax.hierarchyClassID },
                        new ItemHierarchyClass { hierarchyClassID = merchandise.hierarchyClassID }
                    }
                }
            };

            return scanCode;
        }

        /// <summary>
        /// Creates a list of ItemTrait objects with sample data for the following traits:
        ///   ValidationDate, ProductDescription, PosDescription, PackageUnit, FoodStampEligible, PosScaleTare, RetailSize, RetailUom
        /// </summary>
        /// <param name="scanCode">ScanCode/Identifer for the test data item (will be prepended to test trait values)</param>
        /// <param name="additionalTraitsToInclude">Any additional trait ids/values which the tester wished to include</param>
        /// <param name="validationDate">Value to use for the ValidationDate trait (defaults to Now if not specified)</param>
        /// <returns>List of ItemTrait objects with sample values</returns>
        private List<ItemTrait> CreateSampleItemTraits(string scanCode,
            List<Tuple<int,string>> additionalTraitsToInclude = null,
            DateTime? validationDate = null)
        {
            if (!validationDate.HasValue) validationDate = DateTime.Now;

            var itemTraits = new List<ItemTrait>
            {
                new ItemTrait { localeID = Locales.WholeFoods, traitID = Traits.ValidationDate, traitValue = validationDate + scanCode },
                new ItemTrait { localeID = Locales.WholeFoods, traitID = Traits.ProductDescription, traitValue = "Test Product Description" + scanCode },
                new ItemTrait { localeID = Locales.WholeFoods, traitID = Traits.PosDescription, traitValue = "Test POS Description" + scanCode},
                new ItemTrait { localeID = Locales.WholeFoods, traitID = Traits.PackageUnit, traitValue = "Test Package Unit" + scanCode },
                new ItemTrait { localeID = Locales.WholeFoods, traitID = Traits.FoodStampEligible, traitValue = "Test FSE" + scanCode },
                new ItemTrait { localeID = Locales.WholeFoods, traitID = Traits.PosScaleTare, traitValue = "Test Tare" },
                new ItemTrait { localeID = Locales.WholeFoods, traitID = Traits.RetailSize, traitValue = "123" },
                new ItemTrait { localeID = Locales.WholeFoods, traitID = Traits.RetailUom, traitValue = "Test Retail Uom" + scanCode},
            };
            if (additionalTraitsToInclude != null)
            {
                foreach (var additionalTrait in additionalTraitsToInclude)
                {
                    itemTraits.Add(
                        new ItemTrait { localeID = Locales.WholeFoods, traitID = additionalTrait.Item1, traitValue = additionalTrait.Item2 });
                }
            }
            return itemTraits;
        }

        /// <summary>
        /// Returns an ItemSignAttribute object with property values as specified by the test data value flag,
        ///   or with the values provided in the custom property name/value list
        /// </summary>
        /// <param name="signAttrDataFlag">TestDataValueFlag: set the value for the properies 
        /// in the ItemSignAttribute object to either :
        ///    On (true/string value/"Yes"), 
        ///    Off (false/empty string/null),
        ///    Null (null for all nullables or false/0) </param>
        /// <param name="customValidatedItemSignAttrData">List of property/name pairs indicating that the
        ///    specified property names should have the matching value, overriding the test data flag</param>
        /// <returns>ItemSignAttribute object with property values specified as indicated by the parameters</returns>
        private ItemSignAttribute CreateItemSignAttributes(SignAttributeTestDataFlag signAttrDataFlag,
            List<SignAttributeValidatedItemPair> customValidatedItemSignAttrData = null)
        {
            ItemSignAttribute signAttributes = null;

            switch (signAttrDataFlag)
            {
                case SignAttributeTestDataFlag.On:
                    // assign default (on/true/yes/string value) values
                    signAttributes = new ItemSignAttribute
                    {
                        HealthyEatingRatingId = HealthyEatingRatings.Good,
                        AnimalWelfareRating = "Step1",
                        MilkType = "CowGoatSheepMilk",
                        EcoScaleRating = "PremiumYellow",
                        FreshOrFrozen = "Frozen",
                        SeafoodCatchType ="Wild",
                        Biodynamic = true,
                        CheeseRaw = true,
                        Msc = true,
                        PremiumBodyCare = true,
                        Vegetarian = true,
                        WholeTrade = true,
                        GrassFed = true,
                        PastureRaised = true,
                        FreeRange = true,
                        DryAged = true,
                        AirChilled = true,
                        MadeInHouse = true,
                        GlutenFreeAgencyName = "Yes",
                        KosherAgencyName = "Yes",
                        OrganicAgencyName = "Yes",
                        VeganAgencyName = "Yes",
                        NonGmoAgencyName = "Yes",
                        CustomerFriendlyDescription = "Test Friendly Customer Description!"
                    };
                    break;
                case SignAttributeTestDataFlag.Off: 
                    // assign false/off/no values to all appropriate properties
                    signAttributes = new ItemSignAttribute
                    {
                        AnimalWelfareRating = null,
                        HealthyEatingRatingId = null,
                        MilkType = null,
                        EcoScaleRating = null,
                        FreshOrFrozen = null,
                        SeafoodCatchType = null,
                        Biodynamic = false,
                        CheeseRaw = false,
                        Msc = false,
                        PremiumBodyCare = false,
                        Vegetarian = false,
                        WholeTrade = false,
                        GrassFed = false,
                        PastureRaised = false,
                        FreeRange = false,
                        DryAged = false,
                        AirChilled = false,
                        MadeInHouse = false,
                        GlutenFreeAgencyName = "No",
                        KosherAgencyName = "No",
                        OrganicAgencyName = "No",
                        VeganAgencyName = "No",
                        NonGmoAgencyName = "No",
                        CustomerFriendlyDescription = null
                    };
                    break;
                case SignAttributeTestDataFlag.Null:
                default:
                    // leave all nullable properties null, all others to false
                    signAttributes = new ItemSignAttribute
                    {
                        AnimalWelfareRating = null,
                        HealthyEatingRatingId = null,
                        MilkType = null,
                        EcoScaleRating = null,
                        FreshOrFrozen = null,
                        SeafoodCatchType = null,
                        Biodynamic = false,
                        CheeseRaw = false,
                        Msc = false,
                        PremiumBodyCare = false,
                        Vegetarian = false,
                        WholeTrade = false,
                        GrassFed = false,
                        PastureRaised = false,
                        FreeRange = false,
                        DryAged = false,
                        AirChilled = false,
                        MadeInHouse = false,
                        GlutenFreeAgencyName = null,
                        KosherAgencyName = null,
                        OrganicAgencyName = null,
                        VeganAgencyName = null,
                        NonGmoAgencyName = null,
                        CustomerFriendlyDescription = null
                    };
                    break;
            }

            // were any custom properties specified?
            if (customValidatedItemSignAttrData!=null && customValidatedItemSignAttrData.Any())
            {
                // use reflection to get a collection of all available properties
                var props = typeof(ItemSignAttribute).GetProperties();

                // iterate the provided custom property-values
                foreach (var customSignAttribute in customValidatedItemSignAttrData)
                {
                    // find the property matching the name in the custom list
                    var prop = props.FirstOrDefault(p => p.Name == customSignAttribute.ItemSignAttributePropertyName);
                    if (prop != null)
                    {
                        // set the value of the property on the object to the specified custom value
                        prop.SetValue(signAttributes, customSignAttribute.ItemSignAttributePropertyValue);
                    }
                }
            }

            return signAttributes;
        }

        /// <summary>
        /// Returns a value of the provided type which the automated test expects in the ValidatedItemModel
        /// </summary>
        /// <typeparam name="T">data type (string, bool?, int)</typeparam>
        /// <param name="defaultExpectedValue">the value expected (unless something is listed for this property in the custom data)</param>
        /// <param name="customExpectedData">a list of ValidatedItemModel property names and values. If the selected property
        ///   is included, the specified value will be set as the expected value instead of the defaultExpectedValue</param>
        /// <param name="validatedItemPropertyName">the property name in the ValidatedItemModel</param>
        /// <returns></returns>
        private T SetExpectedPropertyValue<T>(T defaultExpectedValue,
            IList<SignAttributeValidatedItemPair> customExpectedData, string validatedItemPropertyName)
        {
            if (customExpectedData != null)
            {
                // check the custom data to see if there is a value for the specified property
                var customDataEntry = customExpectedData
                    .FirstOrDefault(d => d.ValidatedItemPropertyName == validatedItemPropertyName);
                // if a custom entry was found, use that value, otherwise leave the value as provided
                if (customDataEntry != default(SignAttributeValidatedItemPair))
                {
                    return (T)(customDataEntry.ValidatedItemPropertyValue ?? default(T));
                }
            }
            return defaultExpectedValue;
        }

        public void AssertSignAttributesMatchExpected(ScanCode expected,
            ValidatedItemModel actual,
            SignAttributeTestDataFlag defaultPropertyValue,
            List<SignAttributeValidatedItemPair> customExpectedData = null)
        {
            Assert.AreEqual(expected.itemID, actual.ItemId);
            Assert.AreEqual(expected.scanCode, actual.ScanCode);

            bool expectedHasItemSignAttributes;
            string expectedAWR;
            string expectedHER;
            bool? expectedBIO;
            string expectedCMT;
            bool? expectedCR;
            string expectedECO;
            bool? expectedGF;
            bool? expectedKSH;
            bool? expectedMSC;
            bool? expectedNGM;
            bool? expectedOG;
            bool? expectedPBC;
            string expectedSFF;
            string expectedSFT;
            bool? expectedVEG;
            bool? expectedVGN;
            bool? expectedWT;
            bool? expectedGRF;
            bool? expectedPAS;
            bool? expectedFRR;
            bool? expectedDAG;
            bool? expectedACH;
            bool? expectedMIH;
            string expectedCFD;

            switch (defaultPropertyValue)
            {
                case SignAttributeTestDataFlag.On:
                    // set expected values for when sign attributes data is is expected to exist and have populated values
                    expectedHasItemSignAttributes = true;
                    expectedAWR = SetExpectedPropertyValue("Step1", customExpectedData, nameof(ValidatedItemModel.AnimalWelfareRating));
                    expectedHER = SetExpectedPropertyValue(HealthyEatingRatings.Descriptions.Good, customExpectedData, nameof(ValidatedItemModel.HealthyEatingRating));
                    expectedBIO = SetExpectedPropertyValue((bool?)true, customExpectedData, nameof(ValidatedItemModel.Biodynamic));
                    expectedCMT = SetExpectedPropertyValue("CowGoatSheepMilk", customExpectedData, nameof(ValidatedItemModel.MilkType));
                    expectedCR = SetExpectedPropertyValue((bool?)true, customExpectedData, nameof(ValidatedItemModel.CheeseRaw));
                    expectedECO = SetExpectedPropertyValue("PremiumYellow", customExpectedData, nameof(ValidatedItemModel.EcoScaleRating));
                    expectedGF = SetExpectedPropertyValue((bool?)true, customExpectedData, nameof(ValidatedItemModel.GlutenFree));
                    expectedKSH = SetExpectedPropertyValue((bool?)true, customExpectedData, nameof(ValidatedItemModel.Kosher));
                    expectedMSC = SetExpectedPropertyValue((bool?)true, customExpectedData, nameof(ValidatedItemModel.Msc));
                    expectedNGM = SetExpectedPropertyValue((bool?)true, customExpectedData, nameof(ValidatedItemModel.NonGmo));
                    expectedOG = SetExpectedPropertyValue((bool?)true, customExpectedData, nameof(ValidatedItemModel.Organic));
                    expectedPBC = SetExpectedPropertyValue((bool?)true, customExpectedData, nameof(ValidatedItemModel.PremiumBodyCare));
                    expectedSFF = SetExpectedPropertyValue("Frozen", customExpectedData, nameof(ValidatedItemModel.FreshOrFrozen));
                    expectedSFT = SetExpectedPropertyValue("Wild", customExpectedData, nameof(ValidatedItemModel.SeafoodCatchType));
                    expectedVEG = SetExpectedPropertyValue((bool?)true, customExpectedData, nameof(ValidatedItemModel.Vegetarian));
                    expectedVGN = SetExpectedPropertyValue((bool?)true, customExpectedData, nameof(ValidatedItemModel.Vegan));
                    expectedWT = SetExpectedPropertyValue((bool?)true, customExpectedData, nameof(ValidatedItemModel.WholeTrade));
                    expectedGRF = SetExpectedPropertyValue((bool?)true, customExpectedData, nameof(ValidatedItemModel.GrassFed));
                    expectedPAS = SetExpectedPropertyValue((bool?)true, customExpectedData, nameof(ValidatedItemModel.PastureRaised));
                    expectedFRR = SetExpectedPropertyValue((bool?)true, customExpectedData, nameof(ValidatedItemModel.FreeRange));
                    expectedDAG = SetExpectedPropertyValue((bool?)true, customExpectedData, nameof(ValidatedItemModel.DryAged));
                    expectedACH = SetExpectedPropertyValue((bool?)true, customExpectedData, nameof(ValidatedItemModel.AirChilled));
                    expectedMIH = SetExpectedPropertyValue((bool?)true, customExpectedData, nameof(ValidatedItemModel.MadeInHouse));
                    expectedCFD = SetExpectedPropertyValue("Test Friendly Customer Description!", customExpectedData, nameof(ValidatedItemModel.CustomerFriendlyDescription));
                    break;
                case SignAttributeTestDataFlag.Off:
                    // set expected  values for when sign attribute data is expected to exist but be false/empty
                    expectedHasItemSignAttributes = true;
                    expectedAWR = SetExpectedPropertyValue((string)null, customExpectedData, nameof(ValidatedItemModel.AnimalWelfareRating));
                    expectedHER = SetExpectedPropertyValue((string)null, customExpectedData, nameof(ValidatedItemModel.HealthyEatingRating));
                    expectedBIO = SetExpectedPropertyValue((bool?)false, customExpectedData, nameof(ValidatedItemModel.Biodynamic));
                    expectedCMT = SetExpectedPropertyValue((string)null, customExpectedData, nameof(ValidatedItemModel.MilkType));
                    expectedCR  = SetExpectedPropertyValue((bool?)false, customExpectedData, nameof(ValidatedItemModel.CheeseRaw));
                    expectedECO = SetExpectedPropertyValue((string)null, customExpectedData, nameof(ValidatedItemModel.EcoScaleRating));
                    expectedGF  = SetExpectedPropertyValue((bool?)false, customExpectedData, nameof(ValidatedItemModel.GlutenFree));
                    expectedKSH = SetExpectedPropertyValue((bool?)false, customExpectedData, nameof(ValidatedItemModel.Kosher));
                    expectedMSC = SetExpectedPropertyValue((bool?)false, customExpectedData, nameof(ValidatedItemModel.Msc));
                    expectedNGM = SetExpectedPropertyValue((bool?)false, customExpectedData, nameof(ValidatedItemModel.NonGmo));
                    expectedOG  = SetExpectedPropertyValue((bool?)false, customExpectedData, nameof(ValidatedItemModel.Organic));
                    expectedPBC = SetExpectedPropertyValue((bool?)false, customExpectedData, nameof(ValidatedItemModel.PremiumBodyCare));
                    expectedSFF = SetExpectedPropertyValue((string)null, customExpectedData, nameof(ValidatedItemModel.FreshOrFrozen));
                    expectedSFT = SetExpectedPropertyValue((string)null, customExpectedData, nameof(ValidatedItemModel.SeafoodCatchType));
                    expectedVEG = SetExpectedPropertyValue((bool?)false, customExpectedData, nameof(ValidatedItemModel.Vegetarian));
                    expectedVGN = SetExpectedPropertyValue((bool?)false, customExpectedData, nameof(ValidatedItemModel.Vegan));
                    expectedWT  = SetExpectedPropertyValue((bool?)false, customExpectedData, nameof(ValidatedItemModel.WholeTrade));
                    expectedGRF = SetExpectedPropertyValue((bool?)false, customExpectedData, nameof(ValidatedItemModel.GrassFed));
                    expectedPAS = SetExpectedPropertyValue((bool?)false, customExpectedData, nameof(ValidatedItemModel.PastureRaised));
                    expectedFRR = SetExpectedPropertyValue((bool?)false, customExpectedData, nameof(ValidatedItemModel.FreeRange));
                    expectedDAG = SetExpectedPropertyValue((bool?)false, customExpectedData, nameof(ValidatedItemModel.DryAged));
                    expectedACH = SetExpectedPropertyValue((bool?)false, customExpectedData, nameof(ValidatedItemModel.AirChilled));
                    expectedMIH = SetExpectedPropertyValue((bool?)false, customExpectedData, nameof(ValidatedItemModel.MadeInHouse));
                    expectedCFD = SetExpectedPropertyValue((string)null, customExpectedData, nameof(ValidatedItemModel.CustomerFriendlyDescription));
                    break;
                case SignAttributeTestDataFlag.Null:
                default:
                    //set expected values for when sign attribute data is expected to be null
                    expectedHasItemSignAttributes = false;
                    expectedAWR = SetExpectedPropertyValue((string)null, customExpectedData, nameof(ValidatedItemModel.AnimalWelfareRating));
                    expectedHER = SetExpectedPropertyValue((string)null, customExpectedData, nameof(ValidatedItemModel.HealthyEatingRating));
                    expectedBIO = SetExpectedPropertyValue((bool?)null, customExpectedData, nameof(ValidatedItemModel.Biodynamic));
                    expectedCMT = SetExpectedPropertyValue((string)null, customExpectedData, nameof(ValidatedItemModel.MilkType));
                    expectedCR = SetExpectedPropertyValue((bool?)null, customExpectedData, nameof(ValidatedItemModel.CheeseRaw));
                    expectedECO = SetExpectedPropertyValue((string)null, customExpectedData, nameof(ValidatedItemModel.EcoScaleRating));
                    expectedGF = SetExpectedPropertyValue((bool?)null, customExpectedData, nameof(ValidatedItemModel.GlutenFree));
                    expectedKSH = SetExpectedPropertyValue((bool?)null, customExpectedData, nameof(ValidatedItemModel.Kosher));
                    expectedMSC = SetExpectedPropertyValue((bool?)null, customExpectedData, nameof(ValidatedItemModel.Msc));
                    expectedNGM = SetExpectedPropertyValue((bool?)null, customExpectedData, nameof(ValidatedItemModel.NonGmo));
                    expectedOG = SetExpectedPropertyValue((bool?)null, customExpectedData, nameof(ValidatedItemModel.Organic));
                    expectedPBC = SetExpectedPropertyValue((bool?)null, customExpectedData, nameof(ValidatedItemModel.PremiumBodyCare));
                    expectedSFF = SetExpectedPropertyValue((string)null, customExpectedData, nameof(ValidatedItemModel.FreshOrFrozen));
                    expectedSFT = SetExpectedPropertyValue((string)null, customExpectedData, nameof(ValidatedItemModel.SeafoodCatchType));
                    expectedVEG = SetExpectedPropertyValue((bool?)null, customExpectedData, nameof(ValidatedItemModel.Vegetarian));
                    expectedVGN = SetExpectedPropertyValue((bool?)null, customExpectedData, nameof(ValidatedItemModel.Vegan));
                    expectedWT = SetExpectedPropertyValue((bool?)null, customExpectedData, nameof(ValidatedItemModel.WholeTrade));
                    expectedGRF = SetExpectedPropertyValue((bool?)null, customExpectedData, nameof(ValidatedItemModel.GrassFed));
                    expectedPAS = SetExpectedPropertyValue((bool?)null, customExpectedData, nameof(ValidatedItemModel.PastureRaised));
                    expectedFRR = SetExpectedPropertyValue((bool?)null, customExpectedData, nameof(ValidatedItemModel.FreeRange));
                    expectedDAG = SetExpectedPropertyValue((bool?)null, customExpectedData, nameof(ValidatedItemModel.DryAged));
                    expectedACH = SetExpectedPropertyValue((bool?)null, customExpectedData, nameof(ValidatedItemModel.AirChilled));
                    expectedMIH = SetExpectedPropertyValue((bool?)null, customExpectedData, nameof(ValidatedItemModel.MadeInHouse));
                    expectedCFD = SetExpectedPropertyValue((string)null, customExpectedData, nameof(ValidatedItemModel.CustomerFriendlyDescription));
                    break;
            }

            // assert that the expected values match the actual values in the ValidatedItemModel
            Assert.AreEqual(expectedHasItemSignAttributes, actual.HasItemSignAttributes, "Property: " + nameof(actual.HasItemSignAttributes));
            Assert.AreEqual(expectedAWR, actual.AnimalWelfareRating, "Property: " + nameof(actual.AnimalWelfareRating));
            Assert.AreEqual(expectedHER, actual.HealthyEatingRating, "Property: " + nameof(actual.HealthyEatingRating));
            Assert.AreEqual(expectedBIO, actual.Biodynamic, "Property: " + nameof(actual.Biodynamic));
            Assert.AreEqual(expectedCMT, actual.MilkType, "Property: " + nameof(actual.MilkType));
            Assert.AreEqual(expectedCR, actual.CheeseRaw, "Property: " + nameof(actual.CheeseRaw));
            Assert.AreEqual(expectedECO, actual.EcoScaleRating, "Property: " + nameof(actual.EcoScaleRating));
            Assert.AreEqual(expectedGF, actual.GlutenFree, "Property: " + nameof(actual.GlutenFree));
            Assert.AreEqual(expectedKSH, actual.Kosher, "Property: " + nameof(actual.Kosher));
            Assert.AreEqual(expectedMSC, actual.Msc, "Property: " + nameof(actual.Msc));
            Assert.AreEqual(expectedNGM, actual.NonGmo, "Property: " + nameof(actual.NonGmo));
            Assert.AreEqual(expectedOG, actual.Organic, "Property: " + nameof(actual.Organic));
            Assert.AreEqual(expectedPBC, actual.PremiumBodyCare, "Property: " + nameof(actual.PremiumBodyCare));
            Assert.AreEqual(expectedSFF, actual.FreshOrFrozen, "Property: " + nameof(actual.FreshOrFrozen));
            Assert.AreEqual(expectedSFT, actual.SeafoodCatchType, "Property: " + nameof(actual.SeafoodCatchType));
            Assert.AreEqual(expectedVGN, actual.Vegan, "Property: " + nameof(actual.Vegan));
            Assert.AreEqual(expectedVEG, actual.Vegetarian, "Property: " + nameof(actual.Vegetarian));
            Assert.AreEqual(expectedWT, actual.WholeTrade, "Property: " + nameof(actual.WholeTrade));
            Assert.AreEqual(expectedGRF, actual.GrassFed, "Property: " + nameof(actual.GrassFed));
            Assert.AreEqual(expectedPAS, actual.PastureRaised, "Property: " + nameof(actual.PastureRaised));
            Assert.AreEqual(expectedFRR, actual.FreeRange, "Property: " + nameof(actual.FreeRange));
            Assert.AreEqual(expectedDAG, actual.DryAged, "Property: " + nameof(actual.DryAged));
            Assert.AreEqual(expectedACH, actual.AirChilled, "Property: " + nameof(actual.AirChilled));
            Assert.AreEqual(expectedMIH, actual.MadeInHouse, "Property: " + nameof(actual.MadeInHouse));
            Assert.AreEqual(expectedCFD, actual.CustomerFriendlyDescription, "Property: " + nameof(actual.CustomerFriendlyDescription));
        }

        /// <summary>
        /// Flag for what values to set/expect for the properties in a test data object
        /// </summary>
        public enum SignAttributeTestDataFlag
        {
            Off = 0,
            On = 1,
            Null = 2
        }

        /// <summary>
        /// Represents a pair of string/object values to set/expect
        ///  for a ItemSignAttribute property and a ValidatedItem property
        /// </summary>
        public class SignAttributeValidatedItemPair
        {
            public SignAttributeValidatedItemPair(string itemSignAttributePropertyName, object itemSignAttributePropertyValue,
                string validatedItemPropertyName, object validatedItemPropertyValue)
            {
                ItemSignAttributePropertyName = itemSignAttributePropertyName;
                ItemSignAttributePropertyValue = itemSignAttributePropertyValue;
                ValidatedItemPropertyName = validatedItemPropertyName;
                ValidatedItemPropertyValue = validatedItemPropertyValue;
            }

            public string ItemSignAttributePropertyName { get; set; }
            public object ItemSignAttributePropertyValue { get; set; }
            public string ValidatedItemPropertyName { get; set; }
            public object ValidatedItemPropertyValue { get; set; }
        }
    }
}
