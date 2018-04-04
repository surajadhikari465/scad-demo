using Icon.Framework;
using Icon.Logging;
using Icon.Web.Common;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Models;
using Icon.Web.Tests.Common.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;

namespace Icon.Web.Tests.Integration.Commands
{
    [TestClass] [Ignore]
    public class BulkImportItemCommandHandlerTests
    {
        private BulkImportItemCommandHandler commandHandler;
        private BulkImportCommand<BulkImportItemModel> bulkImportCommand;
        private Mock<ILogger> mockLogger;
        private IconContext context;
        private DbContextTransaction transaction;
        private Item item1;
        private Item item2;
        private Item item3;
        private HierarchyClass initialTestBrand;
        private HierarchyClass updatedTestBrand;
        private HierarchyClass initialTestMerchandise;
        private HierarchyClass updatedTestMerchandise;
        private HierarchyClass initialTestTax;
        private HierarchyClass updatedTestTax;
        private HierarchyClass initialTestNational;
        private HierarchyClass updatedTestNational;
        private HierarchyClass initialTestBrowsing;
        private HierarchyClass updatedTestBrowsing;
        private HierarchyClass initialTestFinancial;
        private HierarchyClass updatedTestFinancial;
        private int initialBrandClassId;
        private int updatedBrandClassId;
        private int initialMerchandiseClassId;
        private int updatedMerchandiseClassId;
        private int initialTaxClassId;
        private int updatedTaxClassId;
        private int initialBrowsingClassId;
        private int updatedBrowsingClassId;
        private int initialNationalClassId;
        private int updatedNationalClassId;
        private List<Item> items;
        private List<ScanCode> scanCodes;
        private List<ItemTrait> itemTraits;
        private List<ItemHierarchyClass> itemHierarchy;
        private AnimalWelfareRating initialAnimalWelfareRating;
        private MilkType initialMilkType;
        private EcoScaleRating initialEcoScaleRating;
        private SeafoodFreshOrFrozen initialSeafoodFreshOrFrozen;
        private SeafoodCatchType initialSeafoodCatchType;
        private HierarchyClass initialGlutenFreeAgency;
        private HierarchyClass initialKosherAgency;
        private HierarchyClass initialNonGmoAgency;
        private HierarchyClass initialOrganicAgency;
        private HierarchyClass initialVeganAgency;
        private AnimalWelfareRating updatedAnimalWelfareRating;
        private MilkType updatedMilkType;
        private EcoScaleRating updatedEcoScaleRating;
        private SeafoodFreshOrFrozen updatedSeafoodFreshOrFrozen;
        private SeafoodCatchType updatedSeafoodCatchType;
        private HierarchyClass updatedGlutenFreeAgency;
        private HierarchyClass updatedKosherAgency;
        private HierarchyClass updatedNonGmoAgency;
        private HierarchyClass updatedOrganicAgency;
        private HierarchyClass updatedVeganAgency;
        private string initialAnimalWelfareRatingDescription;
        private string initialMilkTypeDescription;
        private string initialEcoScaleRatingDescription;
        private string initialSeafoodFreshOrFrozenDescription;
        private string initialSeafoodCatchTypeDescription;
        private string initialGlutenFreeAgencyName;
        private string initialKosherAgencyName;
        private string initialNonGmoAgencyName;
        private string initialOrganicAgencyName;
        private string initialVeganAgencyName;
        private string updatedAnimalWelfareRatingDescription;
        private string updatedMilkTypeDescription;
        private string updatedEcoScaleRatingDescription;
        private string updatedSeafoodFreshOrFrozenDescription;
        private string updatedSeafoodCatchTypeDescription;
        private string updatedGlutenFreeAgencyName;
        private string updatedKosherAgencyName;
        private string updatedNonGmoAgencyName;
        private string updatedOrganicAgencyName;
        private string updatedVeganAgencyName;

        private string scanCode1 = "11111155544";
        private string scanCode2 = "11111155545";
        private string scanCode3 = "11111155536";
        private string scanCode4 = "9948218188";
        private string scanCode5 = "9948218604";
        private string scanCode6 = "9948219001";

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            mockLogger = new Mock<ILogger>();
            commandHandler = new BulkImportItemCommandHandler(mockLogger.Object, context);

            bulkImportCommand = new BulkImportCommand<BulkImportItemModel>();

            transaction = context.Database.BeginTransaction();

            item1 = new Item { itemTypeID = 1 };
            item2 = new Item { itemTypeID = 6 };
            item3 = new Item { itemTypeID = 1 };

            initialTestBrand = new HierarchyClass { hierarchyID = Hierarchies.Brands, hierarchyClassName = "Initial Test Brand", hierarchyLevel = 1 };
            updatedTestBrand = new HierarchyClass { hierarchyID = Hierarchies.Brands, hierarchyClassName = "Updated Test Brand", hierarchyLevel = 1 };
            initialTestMerchandise = new HierarchyClass { hierarchyID = Hierarchies.Merchandise, hierarchyClassName = "Initial Test Merchandise", hierarchyLevel = 2 };
            updatedTestMerchandise = new HierarchyClass { hierarchyID = Hierarchies.Merchandise, hierarchyClassName = "Updated Test Merchandise", hierarchyLevel = 1 };
            initialTestTax = new HierarchyClass { hierarchyID = Hierarchies.Tax, hierarchyClassName = "Initial Test Tax", hierarchyLevel = 1 };
            updatedTestTax = new HierarchyClass { hierarchyID = Hierarchies.Tax, hierarchyClassName = "Updated Test Tax", hierarchyLevel = 1 };
            initialTestNational = new HierarchyClass { hierarchyID = Hierarchies.National, hierarchyClassName = "Initial Test National", hierarchyLevel = 1 };
            updatedTestNational = new HierarchyClass { hierarchyID = Hierarchies.National, hierarchyClassName = "Updated Test National", hierarchyLevel = 1 };
            initialTestBrowsing = new HierarchyClass { hierarchyID = Hierarchies.Browsing, hierarchyClassName = "Initial Test Browsing", hierarchyLevel = 1 };
            updatedTestBrowsing = new HierarchyClass { hierarchyID = Hierarchies.Browsing, hierarchyClassName = "Updated Test Browsing", hierarchyLevel = 1 };
            initialTestFinancial = new HierarchyClass { hierarchyID = Hierarchies.Financial, hierarchyClassName = "Initial Test Financial (1999)", hierarchyLevel = 1 };
            updatedTestFinancial = new HierarchyClass { hierarchyID = Hierarchies.Financial, hierarchyClassName = "Updated Test Financial (2999)", hierarchyLevel = 1 };

            items = new List<Item>
            {
                item1,
                item2,
                item3
            };

            // Add the test items to the context.
            context.Item.AddRange(items);
            context.SaveChanges();

            // Add the test HierarchyClass objects and capture the IDs.
            context.HierarchyClass.Add(initialTestBrand);
            context.HierarchyClass.Add(updatedTestBrand);
            context.HierarchyClass.Add(initialTestMerchandise);
            context.HierarchyClass.Add(updatedTestMerchandise);
            context.HierarchyClass.Add(initialTestTax);
            context.HierarchyClass.Add(updatedTestTax);
            context.HierarchyClass.Add(initialTestBrowsing);
            context.HierarchyClass.Add(updatedTestBrowsing);
            context.HierarchyClass.Add(initialTestFinancial);
            context.HierarchyClass.Add(updatedTestFinancial);
            context.HierarchyClass.Add(initialTestNational);
            context.HierarchyClass.Add(updatedTestNational);
            context.SaveChanges();

            initialBrandClassId = initialTestBrand.hierarchyClassID;
            updatedBrandClassId = updatedTestBrand.hierarchyClassID;
            initialMerchandiseClassId = initialTestMerchandise.hierarchyClassID;
            updatedMerchandiseClassId = updatedTestMerchandise.hierarchyClassID;
            initialTaxClassId = initialTestTax.hierarchyClassID;
            updatedTaxClassId = updatedTestTax.hierarchyClassID;
            initialBrowsingClassId = initialTestBrowsing.hierarchyClassID;
            updatedBrowsingClassId = updatedTestBrowsing.hierarchyClassID;
            initialNationalClassId = initialTestNational.hierarchyClassID;
            updatedNationalClassId = updatedTestNational.hierarchyClassID;

            // Create test scan codes, remove any currently existing instances of those test scan codes from the database, and then add them to the context.
            scanCodes = new List<ScanCode>
            {
                new ScanCode { itemID = item1.itemID, scanCode = scanCode1, scanCodeTypeID = 1, localeID = 1 },
                new ScanCode { itemID = item2.itemID, scanCode = scanCode2, scanCodeTypeID = 1, localeID = 1 },
                new ScanCode { itemID = item3.itemID, scanCode = scanCode3, scanCodeTypeID = 1, localeID = 1 },
            };

            context.ScanCode.AddRange(scanCodes);
            context.SaveChanges();

            // Create test item traits.
            itemTraits = new List<ItemTrait>
            {
                new ItemTrait { itemID = item1.itemID, traitID = Traits.ProductDescription, traitValue = "Initial Test Product Description 1", localeID = 1},
                new ItemTrait { itemID = item2.itemID, traitID = Traits.ProductDescription, traitValue = "Initial Test Product Description 2", localeID = 1 },
                new ItemTrait { itemID = item3.itemID, traitID = Traits.ProductDescription, traitValue = "Initial Test Product Description 3", localeID = 1},
            
                new ItemTrait { itemID = item1.itemID, traitID = Traits.PosDescription, traitValue = "Initial Test POS Description 1", localeID = 1 },
                new ItemTrait { itemID = item2.itemID, traitID = Traits.PosDescription, traitValue = "Initial Test POS Description 1", localeID = 1 },
                new ItemTrait { itemID = item3.itemID, traitID = Traits.PosDescription, traitValue = "Initial Test POS Description 1", localeID = 1 },
            
                new ItemTrait { itemID = item1.itemID, traitID = Traits.PackageUnit, traitValue = "1", localeID = 1 },
                new ItemTrait { itemID = item2.itemID, traitID = Traits.PackageUnit, traitValue = "2", localeID = 1 },
                new ItemTrait { itemID = item3.itemID, traitID = Traits.PackageUnit, traitValue = "6", localeID = 1 },
            
                new ItemTrait { itemID = item1.itemID, traitID = Traits.FoodStampEligible, traitValue = "0", localeID = 1 },
                new ItemTrait { itemID = item2.itemID, traitID = Traits.FoodStampEligible, traitValue = "1", localeID = 1 },
                new ItemTrait { itemID = item3.itemID, traitID = Traits.FoodStampEligible, traitValue = "1", localeID = 1 },

                new ItemTrait { itemID = item1.itemID, traitID = Traits.RetailSize, traitValue = "11", localeID = 1 },
                new ItemTrait { itemID = item2.itemID, traitID = Traits.RetailSize, traitValue = "22", localeID = 1 },
                new ItemTrait { itemID = item3.itemID, traitID = Traits.RetailSize, traitValue = "33", localeID = 1 },
            
                new ItemTrait { itemID = item1.itemID, traitID = Traits.RetailUom, traitValue = "POUND", localeID = 1 },
                new ItemTrait { itemID = item2.itemID, traitID = Traits.RetailUom, traitValue = "CASE", localeID = 1 },
                new ItemTrait { itemID = item3.itemID, traitID = Traits.RetailUom, traitValue = "COUNT", localeID = 1 },

                new ItemTrait { itemID = item1.itemID, traitID = Traits.DeliverySystem, traitValue = "CAP", localeID = 1 },
                new ItemTrait { itemID = item2.itemID, traitID = Traits.DeliverySystem, traitValue = "CHW", localeID = 1 },
                new ItemTrait { itemID = item3.itemID, traitID = Traits.DeliverySystem, traitValue = "LZ", localeID = 1 },

                new ItemTrait { itemID = item1.itemID, traitID = Traits.PosScaleTare, traitValue = "0", localeID = 1 },
                new ItemTrait { itemID = item2.itemID, traitID = Traits.PosScaleTare, traitValue = "1", localeID = 1 },
                new ItemTrait { itemID = item3.itemID, traitID = Traits.PosScaleTare, traitValue = "2", localeID = 1 },

                new ItemTrait { itemID = item1.itemID, traitID = Traits.ModifiedDate, traitValue = null, localeID = 1 },
                new ItemTrait { itemID = item2.itemID, traitID = Traits.ModifiedDate, traitValue = null, localeID = 1 },
                new ItemTrait { itemID = item3.itemID, traitID = Traits.ModifiedDate, traitValue = null, localeID = 1 },

                new ItemTrait { itemID = item1.itemID, traitID = Traits.ModifiedUser, traitValue = null, localeID = 1 },
                new ItemTrait { itemID = item2.itemID, traitID = Traits.ModifiedUser, traitValue = null, localeID = 1 },
                new ItemTrait { itemID = item3.itemID, traitID = Traits.ModifiedUser, traitValue = null, localeID = 1 },
                
                new ItemTrait { itemID = item1.itemID, traitID = Traits.ValidationDate, traitValue = DateTime.Now.ToString(), localeID = 1 }
            };

            context.ItemTrait.AddRange(itemTraits);
            context.SaveChanges();

            // Create test item hierarchy classes add them to the context.
            itemHierarchy = new List<ItemHierarchyClass>
            {
                new ItemHierarchyClass { itemID = item1.itemID, hierarchyClassID = initialBrandClassId },
                new ItemHierarchyClass { itemID = item2.itemID, hierarchyClassID = initialBrandClassId },
                new ItemHierarchyClass { itemID = item3.itemID, hierarchyClassID = initialBrandClassId },
                new ItemHierarchyClass { itemID = item1.itemID, hierarchyClassID = initialMerchandiseClassId },
                new ItemHierarchyClass { itemID = item2.itemID, hierarchyClassID = initialMerchandiseClassId },
                new ItemHierarchyClass { itemID = item3.itemID, hierarchyClassID = initialMerchandiseClassId },
                new ItemHierarchyClass { itemID = item1.itemID, hierarchyClassID = initialTaxClassId },
                new ItemHierarchyClass { itemID = item2.itemID, hierarchyClassID = initialTaxClassId },
                new ItemHierarchyClass { itemID = item3.itemID, hierarchyClassID = initialTaxClassId },
                new ItemHierarchyClass { itemID = item1.itemID, hierarchyClassID = initialBrowsingClassId },
                new ItemHierarchyClass { itemID = item2.itemID, hierarchyClassID = initialBrowsingClassId },
                new ItemHierarchyClass { itemID = item3.itemID, hierarchyClassID = initialBrowsingClassId },
                new ItemHierarchyClass { itemID = item1.itemID, hierarchyClassID = initialNationalClassId },
                new ItemHierarchyClass { itemID = item2.itemID, hierarchyClassID = initialNationalClassId },
                new ItemHierarchyClass { itemID = item3.itemID, hierarchyClassID = initialNationalClassId },
            };

            context.ItemHierarchyClass.AddRange(itemHierarchy);
            context.SaveChanges();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
        }

        private void StageInitialSignAttributes()
        {
            initialAnimalWelfareRatingDescription = "test animal welfare";
            initialMilkTypeDescription = "test milk type";
            initialEcoScaleRatingDescription = "test eco scale";
            initialSeafoodFreshOrFrozenDescription = "test seafood fresh or frozen";
            initialSeafoodCatchTypeDescription = "test seafood catch type";

            initialAnimalWelfareRating = new AnimalWelfareRating { Description = initialAnimalWelfareRatingDescription };
            initialMilkType = new MilkType { Description = initialMilkTypeDescription };
            initialEcoScaleRating = new EcoScaleRating { Description = initialEcoScaleRatingDescription };
            initialSeafoodFreshOrFrozen = new SeafoodFreshOrFrozen { Description = initialSeafoodFreshOrFrozenDescription };
            initialSeafoodCatchType = new SeafoodCatchType { Description = initialSeafoodCatchTypeDescription };

            context.AnimalWelfareRating.Add(initialAnimalWelfareRating);
            context.MilkType.Add(initialMilkType);
            context.EcoScaleRating.Add(initialEcoScaleRating);
            context.SeafoodFreshOrFrozen.Add(initialSeafoodFreshOrFrozen);
            context.SeafoodCatchType.Add(initialSeafoodCatchType);
            
            initialGlutenFreeAgencyName = "test GF agency";
            initialKosherAgencyName = "test kosher agency";
            initialNonGmoAgencyName = "test GMO agency";
            initialOrganicAgencyName = "test OG agency";
            initialVeganAgencyName = "test vegan agency";

            initialGlutenFreeAgency = new HierarchyClass { hierarchyID = Hierarchies.CertificationAgencyManagement, hierarchyLevel = HierarchyLevels.CertificationAgencyManagement, hierarchyClassName = initialGlutenFreeAgencyName };
            initialKosherAgency = new HierarchyClass { hierarchyID = Hierarchies.CertificationAgencyManagement, hierarchyLevel = HierarchyLevels.CertificationAgencyManagement, hierarchyClassName = initialKosherAgencyName };
            initialNonGmoAgency = new HierarchyClass { hierarchyID = Hierarchies.CertificationAgencyManagement, hierarchyLevel = HierarchyLevels.CertificationAgencyManagement, hierarchyClassName = initialNonGmoAgencyName };
            initialOrganicAgency = new HierarchyClass { hierarchyID = Hierarchies.CertificationAgencyManagement, hierarchyLevel = HierarchyLevels.CertificationAgencyManagement, hierarchyClassName = initialOrganicAgencyName };
            initialVeganAgency = new HierarchyClass { hierarchyID = Hierarchies.CertificationAgencyManagement, hierarchyLevel = HierarchyLevels.CertificationAgencyManagement, hierarchyClassName = initialVeganAgencyName };

            context.HierarchyClass.AddRange(new List<HierarchyClass>
                {
                    initialGlutenFreeAgency, initialKosherAgency, initialNonGmoAgency, initialOrganicAgency, initialVeganAgency
                });

            context.SaveChanges();
        }

        private void StageUpdatedSignAttributes()
        {
            updatedAnimalWelfareRatingDescription = "test animal welfare";
            updatedMilkTypeDescription = "test milk type";
            updatedEcoScaleRatingDescription = "test eco scale";
            updatedSeafoodFreshOrFrozenDescription = "test seafood fresh or frozen";
            updatedSeafoodCatchTypeDescription = "test seafood catch type";

            updatedAnimalWelfareRating = new AnimalWelfareRating { Description = updatedAnimalWelfareRatingDescription };
            updatedMilkType = new MilkType { Description = updatedMilkTypeDescription };
            updatedEcoScaleRating = new EcoScaleRating { Description = updatedEcoScaleRatingDescription };
            updatedSeafoodFreshOrFrozen = new SeafoodFreshOrFrozen { Description = updatedSeafoodFreshOrFrozenDescription };
            updatedSeafoodCatchType = new SeafoodCatchType { Description = updatedSeafoodCatchTypeDescription };

            context.AnimalWelfareRating.Add(updatedAnimalWelfareRating);
            context.MilkType.Add(updatedMilkType);
            context.EcoScaleRating.Add(updatedEcoScaleRating);
            context.SeafoodFreshOrFrozen.Add(updatedSeafoodFreshOrFrozen);
            context.SeafoodCatchType.Add(updatedSeafoodCatchType);

            updatedGlutenFreeAgencyName = "test GF agency";
            updatedKosherAgencyName = "test kosher agency";
            updatedNonGmoAgencyName = "test GMO agency";
            updatedOrganicAgencyName = "test OG agency";
            updatedVeganAgencyName = "test vegan agency";

            updatedGlutenFreeAgency = new HierarchyClass { hierarchyID = Hierarchies.CertificationAgencyManagement, hierarchyLevel = HierarchyLevels.CertificationAgencyManagement, hierarchyClassName = updatedGlutenFreeAgencyName };
            updatedKosherAgency = new HierarchyClass { hierarchyID = Hierarchies.CertificationAgencyManagement, hierarchyLevel = HierarchyLevels.CertificationAgencyManagement, hierarchyClassName = updatedKosherAgencyName };
            updatedNonGmoAgency = new HierarchyClass { hierarchyID = Hierarchies.CertificationAgencyManagement, hierarchyLevel = HierarchyLevels.CertificationAgencyManagement, hierarchyClassName = updatedNonGmoAgencyName };
            updatedOrganicAgency = new HierarchyClass { hierarchyID = Hierarchies.CertificationAgencyManagement, hierarchyLevel = HierarchyLevels.CertificationAgencyManagement, hierarchyClassName = updatedOrganicAgencyName };
            updatedVeganAgency = new HierarchyClass { hierarchyID = Hierarchies.CertificationAgencyManagement, hierarchyLevel = HierarchyLevels.CertificationAgencyManagement, hierarchyClassName = updatedVeganAgencyName };

            context.HierarchyClass.AddRange(new List<HierarchyClass>
                {
                    updatedGlutenFreeAgency, updatedKosherAgency, updatedNonGmoAgency, updatedOrganicAgency, updatedVeganAgency
                });

            context.SaveChanges();
        }

        [TestMethod]
        public void BulkImportItem_BulkImportThreeItems_ThreeItemsShouldBeUpdated()
        {
            // Given.
            bulkImportCommand = CreateCommandWithThreeItems();

            // When.
            commandHandler.Execute(bulkImportCommand);

            // Then.
            int actualCount = context.Item
                .Where(i => i.ItemTrait
                    .Any(it => it.traitID == Traits.ProductDescription
                        && it.traitValue.Contains("Bulk Import Test Description"))).ToList().Count;

            int expectedCount = bulkImportCommand.BulkImportData.Count;

            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void BulkImportItem_BulkImportOneItem_EachTraitShouldBeUpdated()
        {
            // Given.
            string testProductDescription = "Updated Product Description 1";
            string testPosDescription = "Updated POS Description 1";
            string testPackageUnit = "1";
            string testFoodStampEligible = "0";
            string testPosScaleTare = "10";
            string testRetailSize = "42";
            string testRetailUom = "EA";
            string testDeliverySystem = "LZ";
            string testIsValidated = "1";
            string testDepartmentSale = "1";
            string testNote = "test";
            string testHidden = "1";
            string testAlcoholByVolume = "1.23";
            string testCaseineFree = "1";
            string testDrainedWeight = "3.21";
            string testDrainedWeightUom = DrainedWeightUoms.Ml;
            string testFairTradeCertified = FairTradeCertifiedValues.FairTradeInternational;
            string testHemp = "1";
            string testLocaleLoanProducer = "1";
            string testMainProductName = "Test Main";
            string testNutritionRequried = "1";
            string testOrganicPersonalCare = "1";
            string testPaleo = "1";
            string testProductFlavorType = "Test Product Flavor Type";

            TestBulkImportItemModelBuilder bulkImportItem = new TestBulkImportItemModelBuilder()
                .WithScanCode(scanCode1)
                .WithBrandId(updatedTestBrand.hierarchyClassID.ToString())
                .WithProductDescription(testProductDescription)
                .WithPosDescription(testPosDescription)
                .WithPackageUnit(testPackageUnit)
                .WithFoodStampEligible(testFoodStampEligible)
                .WithPosScaleTare(testPosScaleTare)
                .WithRetailSize(testRetailSize)
                .WithRetailUom(testRetailUom)
                .WithDeliverySystem(testDeliverySystem)
                .WithMerchandiseId(updatedTestMerchandise.hierarchyClassID.ToString())
                .WithTaxId(updatedTestTax.hierarchyClassID.ToString())
                .WithNationalId(updatedTestNational.hierarchyClassID.ToString())
                .WithBrowsingId(updatedTestBrowsing.hierarchyClassID.ToString())
                .WithIsValidated(testIsValidated)
                .WithDepartmentSale(testDepartmentSale)
                .WithNote(testNote)
                .WithHiddenItem(testHidden)
                .WithAlcoholByVolume(testAlcoholByVolume)
                .WithCaseineFree(testCaseineFree)
                .WithDrainedWeight(testDrainedWeight)
                .WithDrainedWeightUom(testDrainedWeightUom)
                .WithFairTradeCertified(testFairTradeCertified)
                .WithHemp(testHemp)
                .WithLocalLoanProducer(testLocaleLoanProducer)
                .WithMainProductName(testMainProductName)
                .WithNutritionRequired(testNutritionRequried)
                .WithOrganicPersonalCare(testOrganicPersonalCare)
                .WithPaleo(testPaleo)
                .WithProductFlavorType(testProductFlavorType);

            bulkImportCommand = new BulkImportCommand<BulkImportItemModel>
            {
                UserName = "IntegrationTestUsername",
                BulkImportData = new List<BulkImportItemModel> { bulkImportItem }
            };

            // When.
            commandHandler.Execute(bulkImportCommand);

            // Then.
            var updatedItem = context.Item.AsNoTracking().First(i => i.itemID == item1.itemID);

            string actualProductDescription = updatedItem.ItemTrait.Single(it => it.traitID == Traits.ProductDescription).traitValue;
            string actualPosDescription = updatedItem.ItemTrait.Single(it => it.traitID == Traits.PosDescription).traitValue;
            string actualFoodStampEligible = updatedItem.ItemTrait.Single(it => it.traitID == Traits.FoodStampEligible).traitValue;
            string actualPackageUnit = updatedItem.ItemTrait.Single(it => it.traitID == Traits.PackageUnit).traitValue;
            string actualPosScaleTare = updatedItem.ItemTrait.Single(it => it.traitID == Traits.PosScaleTare).traitValue;
            string actualModifiedDate = updatedItem.ItemTrait.Single(it => it.traitID == Traits.ModifiedDate).traitValue;
            string actualModifiedUser = updatedItem.ItemTrait.Single(it => it.traitID == Traits.ModifiedUser).traitValue;
            string actualRetailSize = updatedItem.ItemTrait.Single(it => it.traitID == Traits.RetailSize).traitValue;
            string actualRetailUom = updatedItem.ItemTrait.Single(it => it.traitID == Traits.RetailUom).traitValue;
            string actualDeliverySystem = updatedItem.ItemTrait.Single(it => it.traitID == Traits.DeliverySystem).traitValue;
            string actualValidated = updatedItem.ItemTrait.Single(it => it.traitID == Traits.ValidationDate).traitValue;
            string actualDepartmentSale = updatedItem.ItemTrait.Single(it => it.traitID == Traits.DepartmentSale).traitValue;
            string actualHidden = updatedItem.ItemTrait.Single(it => it.traitID == Traits.HiddenItem).traitValue;
            string actualNote = updatedItem.ItemTrait.Single(it => it.traitID == Traits.Notes).traitValue;
            string actualAlcoholByVolume = updatedItem.ItemTrait.Single(it => it.traitID == Traits.AlcoholByVolume).traitValue;
            string actualCaseinFree = updatedItem.ItemTrait.Single(it => it.traitID == Traits.CaseinFree).traitValue;
            string actualDrainedWeight = updatedItem.ItemTrait.Single(it => it.traitID == Traits.DrainedWeight).traitValue;
            string actualDrainedWeightUom = updatedItem.ItemTrait.Single(it => it.traitID == Traits.DrainedWeightUom).traitValue;
            string actualFairTradeCertified = updatedItem.ItemTrait.Single(it => it.traitID == Traits.FairTradeCertified).traitValue;
            string actualHemp = updatedItem.ItemTrait.Single(it => it.traitID == Traits.Hemp).traitValue;
            string actualLocalLoanProducer = updatedItem.ItemTrait.Single(it => it.traitID == Traits.LocalLoanProducer).traitValue;
            string actualMainProductName = updatedItem.ItemTrait.Single(it => it.traitID == Traits.MainProductName).traitValue;
            string actualNutritionRequired = updatedItem.ItemTrait.Single(it => it.traitID == Traits.NutritionRequired).traitValue;
            string actualOrganicPersonalCare = updatedItem.ItemTrait.Single(it => it.traitID == Traits.OrganicPersonalCare).traitValue;
            string actualPaleo = updatedItem.ItemTrait.Single(it => it.traitID == Traits.Paleo).traitValue;
            string actualProductFlavorType = updatedItem.ItemTrait.Single(it => it.traitID == Traits.ProductFlavorType).traitValue;
            int actualBrand = updatedItem.ItemHierarchyClass.Single(ihc => ihc.hierarchyClassID == updatedTestBrand.hierarchyClassID).hierarchyClassID;
            int actualMerch = updatedItem.ItemHierarchyClass.Single(ihc => ihc.hierarchyClassID == updatedTestMerchandise.hierarchyClassID).hierarchyClassID;
            int actualTax = updatedItem.ItemHierarchyClass.Single(ihc => ihc.hierarchyClassID == updatedTestTax.hierarchyClassID).hierarchyClassID;
            int actualBrowsing = updatedItem.ItemHierarchyClass.Single(ihc => ihc.hierarchyClassID == updatedTestBrowsing.hierarchyClassID).hierarchyClassID;
            int actualNational = updatedItem.ItemHierarchyClass.Single(ihc => ihc.hierarchyClassID == updatedTestNational.hierarchyClassID).hierarchyClassID;

            Assert.AreEqual(testProductDescription, actualProductDescription);
            Assert.AreEqual(testPosDescription, actualPosDescription);
            Assert.AreEqual(testPackageUnit, actualPackageUnit);
            Assert.AreEqual(testFoodStampEligible, actualFoodStampEligible);
            Assert.AreEqual(testPosScaleTare, actualPosScaleTare);
            Assert.AreEqual(DateTime.Today.Date, DateTime.Parse(actualModifiedDate).Date);
            Assert.AreEqual(bulkImportCommand.UserName, actualModifiedUser);
            Assert.AreEqual(testRetailSize, actualRetailSize);
            Assert.AreEqual(testRetailUom, actualRetailUom);
            Assert.AreEqual(testDeliverySystem, actualDeliverySystem);
            Assert.AreEqual(testDepartmentSale, actualDepartmentSale);
            Assert.AreEqual(testHidden, actualHidden);
            Assert.AreEqual(testNote, actualNote);
            Assert.AreEqual(testAlcoholByVolume, actualAlcoholByVolume);
            Assert.AreEqual(testCaseineFree, actualCaseinFree);
            Assert.AreEqual(testDrainedWeight, actualDrainedWeight);
            Assert.AreEqual(testDrainedWeightUom, actualDrainedWeightUom);
            Assert.AreEqual(testFairTradeCertified, actualFairTradeCertified);
            Assert.AreEqual(testHemp, actualHemp);
            Assert.AreEqual(testLocaleLoanProducer, actualLocalLoanProducer);
            Assert.AreEqual(testMainProductName, actualMainProductName);
            Assert.AreEqual(testNutritionRequried, actualNutritionRequired);
            Assert.AreEqual(testOrganicPersonalCare, actualOrganicPersonalCare);
            Assert.AreEqual(testPaleo, actualPaleo);
            Assert.AreEqual(testProductFlavorType, actualProductFlavorType);
            Assert.AreEqual(DateTime.Now.Date, DateTime.Parse(actualValidated).Date);
            Assert.AreEqual(updatedTestBrand.hierarchyClassID, actualBrand);
            Assert.AreEqual(updatedTestMerchandise.hierarchyClassID, actualMerch);
            Assert.AreEqual(updatedTestTax.hierarchyClassID, actualTax);
            Assert.AreEqual(updatedTestBrowsing.hierarchyClassID, actualBrowsing);
            Assert.AreEqual(updatedTestNational.hierarchyClassID, actualNational);
        }

        [TestMethod]
        public void BulkImportItem_AddingSignAttributes_AllValuesShouldBeAddedToDatabase()
        {
            // Given.
            StageInitialSignAttributes();

            string testBiodynamic = String.Empty;
            string testCheeseRaw = String.Empty;
            string testPremiumBodyCare = String.Empty;
            string testVegetarian = "1";
            string testWholeTrade = "1";
            string testGrassFed = "0";
            string testPastureRaised = "0";
            string testFreeRange = "0";
            string testDryAged = "0";
            string testAirChilled = "0";
            string testMadeInHouse = "0";
            string testMsc = "1";

            TestBulkImportItemModelBuilder bulkImportItem = new TestBulkImportItemModelBuilder()
                .Empty()
                .WithScanCode(scanCode1)
                .WithAnimalWelfareRatingId(initialAnimalWelfareRating.AnimalWelfareRatingId.ToString())
                .WithBiodynamic(testBiodynamic)
                .WithMilkTypeId(initialMilkType.MilkTypeId.ToString())
                .WithCheeseRaw(testCheeseRaw)
                .WithEcoScaleRatingId(initialEcoScaleRating.EcoScaleRatingId.ToString())
                .WithGlutenFreeAgency(initialGlutenFreeAgency.hierarchyClassID.ToString())
                .WithKosherAgency(initialKosherAgency.hierarchyClassID.ToString())
                .WithNonGmoAgency(initialNonGmoAgency.hierarchyClassID.ToString())
                .WithOrganicAgency(initialOrganicAgency.hierarchyClassID.ToString())
                .WithPremiumBodyCare(testPremiumBodyCare)
                .WithSeafoodFreshOrFrozenId(initialSeafoodFreshOrFrozen.SeafoodFreshOrFrozenId.ToString())
                .WithSeafoodCatchTypeId(initialSeafoodCatchType.SeafoodCatchTypeId.ToString())
                .WithVeganAgency(initialVeganAgency.hierarchyClassID.ToString())
                .WithVegetarian(testVegetarian)
                .WithWholeTrade(testWholeTrade)
                .WithGrassFed(testGrassFed)
                .WithPastureRaised(testPastureRaised)
                .WithFreeRange(testFreeRange)
                .WithDryAged(testDryAged)
                .WithAirChilled(testAirChilled)
                .WithMadeInHouse(testMadeInHouse)
                .WithMsc(testMsc);

            bulkImportCommand = new BulkImportCommand<BulkImportItemModel>
            {
                UserName = "IntegrationTestUsername",
                BulkImportData = new List<BulkImportItemModel> { bulkImportItem }
            };

            // When.
            commandHandler.Execute(bulkImportCommand);

            // Then.
            RefreshItem(item1);

            var actualSignAttributes = item1.ItemSignAttribute.Single(isa => isa.ItemID == item1.itemID);

            string actualAnimalWelfareRatingId = actualSignAttributes.AnimalWelfareRatingId.ToString();
            bool actualBiodynamic = actualSignAttributes.Biodynamic;
            string actualMilkTypeId = actualSignAttributes.CheeseMilkTypeId.ToString();
            bool actualCheeseRaw = actualSignAttributes.CheeseRaw;
            string actualEcoScaleRatingId = actualSignAttributes.EcoScaleRatingId.ToString();
            string actualGlutenFreeAgencyId = actualSignAttributes.GlutenFreeAgencyId.ToString();
            string actualKosherAgencyId = actualSignAttributes.KosherAgencyId.ToString();
            string actualNonGmoAgencyId = actualSignAttributes.NonGmoAgencyId.ToString();
            string actualOrganicAgencyId = actualSignAttributes.OrganicAgencyId.ToString();
            bool actualPremiumBodyCare = actualSignAttributes.PremiumBodyCare;
            string actualSeafoodFreshOrFrozenId = actualSignAttributes.SeafoodFreshOrFrozenId.ToString();
            string actualSeafoodCatchTypeId = actualSignAttributes.SeafoodCatchTypeId.ToString();
            string actualVeganAgencyId = actualSignAttributes.VeganAgencyId.ToString();
            bool actualVegetarian = actualSignAttributes.Vegetarian;
            bool actualWholeTrade = actualSignAttributes.WholeTrade;
            bool actualMsc = actualSignAttributes.Msc;

            Assert.AreEqual(initialAnimalWelfareRating.AnimalWelfareRatingId.ToString(), actualAnimalWelfareRatingId);
            //Assert.AreEqual(testBiodynamic == "1" ? true : false, actualBiodynamic);
            Assert.IsFalse(actualBiodynamic);
            Assert.AreEqual(initialMilkType.MilkTypeId.ToString(), actualMilkTypeId);
            //Assert.AreEqual(testCheeseRaw == "1" ? true : false, actualCheeseRaw);
            Assert.IsFalse(actualCheeseRaw);
            Assert.AreEqual(initialEcoScaleRating.EcoScaleRatingId.ToString(), actualEcoScaleRatingId);
            Assert.AreEqual(initialGlutenFreeAgency.hierarchyClassID.ToString(), actualGlutenFreeAgencyId);
            Assert.AreEqual(initialKosherAgency.hierarchyClassID.ToString(), actualKosherAgencyId);
            Assert.AreEqual(initialNonGmoAgency.hierarchyClassID.ToString(), actualNonGmoAgencyId);
            Assert.AreEqual(initialOrganicAgency.hierarchyClassID.ToString(), actualOrganicAgencyId);
            //Assert.AreEqual(testPremiumBodyCare == "1" ? true : false, actualPremiumBodyCare);
            Assert.IsFalse(actualPremiumBodyCare);
            Assert.AreEqual(initialSeafoodFreshOrFrozen.SeafoodFreshOrFrozenId.ToString(), actualSeafoodFreshOrFrozenId);
            Assert.AreEqual(initialSeafoodCatchType.SeafoodCatchTypeId.ToString(), actualSeafoodCatchTypeId);
            Assert.AreEqual(initialVeganAgency.hierarchyClassID.ToString(), actualVeganAgencyId);
            Assert.AreEqual(testVegetarian == "1" ? true : false, actualVegetarian);
            Assert.AreEqual(testWholeTrade == "1" ? true : false, actualWholeTrade);
            Assert.AreEqual(testGrassFed == "1" ? true : false, actualSignAttributes.GrassFed);
            Assert.AreEqual(testPastureRaised == "1" ? true : false, actualSignAttributes.PastureRaised);
            Assert.AreEqual(testFreeRange == "1" ? true : false, actualSignAttributes.FreeRange);
            Assert.AreEqual(testDryAged == "1" ? true : false, actualSignAttributes.DryAged);
            Assert.AreEqual(testAirChilled == "1" ? true : false, actualSignAttributes.AirChilled);
            Assert.AreEqual(testMadeInHouse == "1" ? true : false, actualSignAttributes.MadeInHouse);
            Assert.IsTrue(actualMsc);
        }

        [TestMethod]
        public void BulkImportItem_AddingSignAttributes_NoEventOrMessageShouldBeGenerated()
        {
            // Given.
            StageInitialSignAttributes();

            string testBiodynamic = "1";
            string testCheeseRaw = "0";
            string testPremiumBodyCare = "0";
            string testVegetarian = "1";
            string testWholeTrade = "1";
            string testGrassFed = "0";
            string testPastureRaised = "0";
            string testFreeRange = "0";
            string testDryAged = "0";
            string testAirChilled = "0";
            string testMadeInHouse = "0";

            TestBulkImportItemModelBuilder bulkImportItem = new TestBulkImportItemModelBuilder()
                .Empty()
                .WithScanCode(scanCode1)
                .WithAnimalWelfareRatingId(initialAnimalWelfareRating.AnimalWelfareRatingId.ToString())
                .WithBiodynamic(testBiodynamic)
                .WithMilkTypeId(initialMilkType.MilkTypeId.ToString())
                .WithCheeseRaw(testCheeseRaw)
                .WithEcoScaleRatingId(initialEcoScaleRating.EcoScaleRatingId.ToString())
                .WithGlutenFreeAgency(initialGlutenFreeAgency.hierarchyClassID.ToString())
                .WithKosherAgency(initialKosherAgency.hierarchyClassID.ToString())
                .WithNonGmoAgency(initialNonGmoAgency.hierarchyClassID.ToString())
                .WithOrganicAgency(initialOrganicAgency.hierarchyClassID.ToString())
                .WithPremiumBodyCare(testPremiumBodyCare)
                .WithSeafoodFreshOrFrozenId(initialSeafoodFreshOrFrozen.SeafoodFreshOrFrozenId.ToString())
                .WithSeafoodCatchTypeId(initialSeafoodCatchType.SeafoodCatchTypeId.ToString())
                .WithVeganAgency(initialVeganAgency.hierarchyClassID.ToString())
                .WithVegetarian(testVegetarian)
                .WithWholeTrade(testWholeTrade)
                .WithGrassFed(testGrassFed)
                .WithPastureRaised(testPastureRaised)
                .WithFreeRange(testFreeRange)
                .WithDryAged(testDryAged)
                .WithAirChilled(testAirChilled)
                .WithMadeInHouse(testMadeInHouse);

            bulkImportCommand = new BulkImportCommand<BulkImportItemModel>
            {
                UserName = "IntegrationTestUsername",
                BulkImportData = new List<BulkImportItemModel> { bulkImportItem }
            };

            // When.
            commandHandler.Execute(bulkImportCommand);

            // Then.
            var newEvents = context.EventQueue.Where(q => q.EventReferenceId == item1.itemID).ToList();
            var newMessage = context.MessageQueueProduct.SingleOrDefault(q => q.ItemId == item1.itemID);

            Assert.AreEqual(0, newEvents.Count);
            Assert.IsNull(newMessage);
        }

        [TestMethod]
        public void BulkImportItem_UpdatingSignAttributes_AllFieldsValueShouldBeUpdatedExceptBooleanFieldsWithEmptyStringAsInputValue()
        {
            // Given.
            StageInitialSignAttributes();
            StageUpdatedSignAttributes();

            string initialBiodynamic = "1";
            string initialCheeseRaw = "0";
            string initialPremiumBodyCare = "0";
            string initialVegetarian = "1";
            string initialWholeTrade = "1";
            string initialGrassFed = "0";
            string initialPastureRaised = "0";
            string initialFreeRange = "0";
            string initialDryAged = "0";
            string initialAirChilled = "0";
            string initialMadeInHouse = "0";

            TestBulkImportItemModelBuilder bulkImportItem = new TestBulkImportItemModelBuilder()
                .Empty()
                .WithScanCode(scanCode1)
                .WithAnimalWelfareRatingId(initialAnimalWelfareRating.AnimalWelfareRatingId.ToString())
                .WithBiodynamic(initialBiodynamic)
                .WithMilkTypeId(initialMilkType.MilkTypeId.ToString())
                .WithCheeseRaw(initialCheeseRaw)
                .WithEcoScaleRatingId(initialEcoScaleRating.EcoScaleRatingId.ToString())
                .WithGlutenFreeAgency(initialGlutenFreeAgency.hierarchyClassID.ToString())
                .WithKosherAgency(initialKosherAgency.hierarchyClassID.ToString())
                .WithNonGmoAgency(initialNonGmoAgency.hierarchyClassID.ToString())
                .WithOrganicAgency(initialOrganicAgency.hierarchyClassID.ToString())
                .WithPremiumBodyCare(initialPremiumBodyCare)
                .WithSeafoodFreshOrFrozenId(initialSeafoodFreshOrFrozen.SeafoodFreshOrFrozenId.ToString())
                .WithSeafoodCatchTypeId(initialSeafoodCatchType.SeafoodCatchTypeId.ToString())
                .WithVeganAgency(initialVeganAgency.hierarchyClassID.ToString())
                .WithVegetarian(initialVegetarian)
                .WithWholeTrade(initialWholeTrade)
                .WithGrassFed(initialGrassFed)
                .WithPastureRaised(initialPastureRaised)
                .WithFreeRange(initialFreeRange)
                .WithDryAged(initialDryAged)
                .WithAirChilled(initialAirChilled)
                .WithMadeInHouse(initialMadeInHouse);

            bulkImportCommand = new BulkImportCommand<BulkImportItemModel>
            {
                UserName = "IntegrationTestUsername",
                BulkImportData = new List<BulkImportItemModel> { bulkImportItem }
            };

            commandHandler.Execute(bulkImportCommand);

            string updatedBiodynamic = String.Empty;
            string updatedCheeseRaw = String.Empty;
            string updatedPremiumBodyCare = String.Empty;
            string updatedVegetarian = String.Empty;
            string updatedWholeTrade = "0";
            string updatedGrassFed = "1";
            string updatedPastureRaised = "1";
            string updatedFreeRange = "1";
            string updatedDryAged = "1";
            string updatedAirChilled = "1";
            string updatedMadeInHouse = "1";

            bulkImportItem = new TestBulkImportItemModelBuilder()
                .Empty()
                .WithScanCode(scanCode1)
                .WithAnimalWelfareRatingId(updatedAnimalWelfareRating.AnimalWelfareRatingId.ToString())
                .WithBiodynamic(updatedBiodynamic)
                .WithMilkTypeId(updatedMilkType.MilkTypeId.ToString())
                .WithCheeseRaw(updatedCheeseRaw)
                .WithEcoScaleRatingId(updatedEcoScaleRating.EcoScaleRatingId.ToString())
                .WithGlutenFreeAgency(updatedGlutenFreeAgency.hierarchyClassID.ToString())
                .WithKosherAgency(updatedKosherAgency.hierarchyClassID.ToString())
                .WithNonGmoAgency(updatedNonGmoAgency.hierarchyClassID.ToString())
                .WithOrganicAgency(updatedOrganicAgency.hierarchyClassID.ToString())
                .WithPremiumBodyCare(updatedPremiumBodyCare)
                .WithSeafoodFreshOrFrozenId(updatedSeafoodFreshOrFrozen.SeafoodFreshOrFrozenId.ToString())
                .WithSeafoodCatchTypeId(updatedSeafoodCatchType.SeafoodCatchTypeId.ToString())
                .WithVeganAgency(updatedVeganAgency.hierarchyClassID.ToString())
                .WithVegetarian(updatedVegetarian)
                .WithWholeTrade(updatedWholeTrade)
                .WithWholeTrade(updatedWholeTrade)
                .WithGrassFed(updatedGrassFed)
                .WithPastureRaised(updatedPastureRaised)
                .WithFreeRange(updatedFreeRange)
                .WithDryAged(updatedDryAged)
                .WithAirChilled(updatedAirChilled)
                .WithMadeInHouse(updatedMadeInHouse);

            // When.
            bulkImportCommand.BulkImportData = new List<BulkImportItemModel> { bulkImportItem };
            commandHandler.Execute(bulkImportCommand);

            // Then.
            RefreshItem(item1);

            var actualSignAttributes = item1.ItemSignAttribute.Single(isa => isa.ItemID == item1.itemID);

            string actualAnimalWelfareRatingId = actualSignAttributes.AnimalWelfareRatingId.ToString();
            bool actualBiodynamic = actualSignAttributes.Biodynamic;
            string actualMilkTypeId = actualSignAttributes.CheeseMilkTypeId.ToString();
            bool actualCheeseRaw = actualSignAttributes.CheeseRaw;
            string actualEcoScaleRatingId = actualSignAttributes.EcoScaleRatingId.ToString();
            string actualGlutenFreeAgencyId = actualSignAttributes.GlutenFreeAgencyId.ToString();
            string actualKosherAgencyId = actualSignAttributes.KosherAgencyId.ToString();
            string actualNonGmoAgencyId = actualSignAttributes.NonGmoAgencyId.ToString();
            string actualOrganicAgencyId = actualSignAttributes.OrganicAgencyId.ToString();
            bool actualPremiumBodyCare = actualSignAttributes.PremiumBodyCare;
            string actualSeafoodFreshOrFrozenId = actualSignAttributes.SeafoodFreshOrFrozenId.ToString();
            string actualSeafoodCatchTypeId = actualSignAttributes.SeafoodCatchTypeId.ToString();
            string actualVeganAgencyId = actualSignAttributes.VeganAgencyId.ToString();
            bool actualVegetarian = actualSignAttributes.Vegetarian;
            bool actualWholeTrade = actualSignAttributes.WholeTrade;

            Assert.AreEqual(updatedAnimalWelfareRating.AnimalWelfareRatingId.ToString(), actualAnimalWelfareRatingId);
            Assert.IsTrue(actualBiodynamic);
            Assert.AreEqual(updatedMilkType.MilkTypeId.ToString(), actualMilkTypeId);
            Assert.IsFalse(actualCheeseRaw);
            Assert.AreEqual(updatedEcoScaleRating.EcoScaleRatingId.ToString(), actualEcoScaleRatingId);
            Assert.AreEqual(updatedGlutenFreeAgency.hierarchyClassID.ToString(), actualGlutenFreeAgencyId);
            Assert.AreEqual(updatedKosherAgency.hierarchyClassID.ToString(), actualKosherAgencyId);
            Assert.AreEqual(updatedNonGmoAgency.hierarchyClassID.ToString(), actualNonGmoAgencyId);
            Assert.AreEqual(updatedOrganicAgency.hierarchyClassID.ToString(), actualOrganicAgencyId);
            Assert.IsFalse(actualPremiumBodyCare);
            Assert.AreEqual(updatedSeafoodFreshOrFrozen.SeafoodFreshOrFrozenId.ToString(), actualSeafoodFreshOrFrozenId);
            Assert.AreEqual(updatedSeafoodCatchType.SeafoodCatchTypeId.ToString(), actualSeafoodCatchTypeId);
            Assert.AreEqual(updatedVeganAgency.hierarchyClassID.ToString(), actualVeganAgencyId);
            Assert.IsTrue(actualVegetarian);
            Assert.AreEqual(updatedWholeTrade == "1" ? true : false, actualWholeTrade);
            Assert.AreEqual(updatedGrassFed == "1" ? true : false, actualSignAttributes.GrassFed);
            Assert.AreEqual(updatedPastureRaised == "1" ? true : false, actualSignAttributes.PastureRaised);
            Assert.AreEqual(updatedFreeRange == "1" ? true : false, actualSignAttributes.FreeRange);
            Assert.AreEqual(updatedDryAged == "1" ? true : false, actualSignAttributes.DryAged);
            Assert.AreEqual(updatedAirChilled == "1" ? true : false, actualSignAttributes.AirChilled);
            Assert.AreEqual(updatedMadeInHouse == "1" ? true : false, actualSignAttributes.MadeInHouse);
        }

        [TestMethod]
        public void BulkImportItem_UpdatingSignAttributes_NoEventOrMessageShouldBeGenerated()
        {
            // Given.
            StageInitialSignAttributes();
            StageUpdatedSignAttributes();

            string initialBiodynamic = "1";
            string initialCheeseRaw = "0";
            string initialPremiumBodyCare = "0";
            string initialVegetarian = "1";
            string initialWholeTrade = "1";
            string initialGrassFed = "0";
            string initialPastureRaised = "0";
            string initialFreeRange = "0";
            string initialDryAged = "0";
            string initialAirChilled = "0";
            string initialMadeInHouse = "0";

            TestBulkImportItemModelBuilder bulkImportItem = new TestBulkImportItemModelBuilder()
                .Empty()
                .WithScanCode(scanCode1)
                .WithAnimalWelfareRatingId(initialAnimalWelfareRating.AnimalWelfareRatingId.ToString())
                .WithBiodynamic(initialBiodynamic)
                .WithMilkTypeId(initialMilkType.MilkTypeId.ToString())
                .WithCheeseRaw(initialCheeseRaw)
                .WithEcoScaleRatingId(initialEcoScaleRating.EcoScaleRatingId.ToString())
                .WithGlutenFreeAgency(initialGlutenFreeAgency.hierarchyClassID.ToString())
                .WithKosherAgency(initialKosherAgency.hierarchyClassID.ToString())
                .WithNonGmoAgency(initialNonGmoAgency.hierarchyClassID.ToString())
                .WithOrganicAgency(initialOrganicAgency.hierarchyClassID.ToString())
                .WithPremiumBodyCare(initialPremiumBodyCare)
                .WithSeafoodFreshOrFrozenId(initialSeafoodFreshOrFrozen.SeafoodFreshOrFrozenId.ToString())
                .WithSeafoodCatchTypeId(initialSeafoodCatchType.SeafoodCatchTypeId.ToString())
                .WithVeganAgency(initialVeganAgency.hierarchyClassID.ToString())
                .WithVegetarian(initialVegetarian)
                .WithWholeTrade(initialWholeTrade)
                .WithGrassFed(initialGrassFed)
                .WithPastureRaised(initialPastureRaised)
                .WithFreeRange(initialFreeRange)
                .WithDryAged(initialDryAged)
                .WithAirChilled(initialAirChilled)
                .WithMadeInHouse(initialMadeInHouse);

            bulkImportCommand = new BulkImportCommand<BulkImportItemModel>
            {
                UserName = "IntegrationTestUsername",
                BulkImportData = new List<BulkImportItemModel> { bulkImportItem }
            };

            commandHandler.Execute(bulkImportCommand);

            string updatedBiodynamic = "0";
            string updatedCheeseRaw = "1";
            string updatedPremiumBodyCare = "1";
            string updatedVegetarian = "0";
            string updatedWholeTrade = "0";
            string updatedGrassFed = "1";
            string updatedPastureRaised = "1";
            string updatedFreeRange = "1";
            string updatedDryAged = "1";
            string updatedAirChilled = "1";
            string updatedMadeInHouse = "1";

            bulkImportItem = new TestBulkImportItemModelBuilder()
                .Empty()
                .WithScanCode(scanCode1)
                .WithAnimalWelfareRatingId(updatedAnimalWelfareRating.AnimalWelfareRatingId.ToString())
                .WithBiodynamic(updatedBiodynamic)
                .WithMilkTypeId(updatedMilkType.MilkTypeId.ToString())
                .WithCheeseRaw(updatedCheeseRaw)
                .WithEcoScaleRatingId(updatedEcoScaleRating.EcoScaleRatingId.ToString())
                .WithGlutenFreeAgency(updatedGlutenFreeAgency.hierarchyClassID.ToString())
                .WithKosherAgency(updatedKosherAgency.hierarchyClassID.ToString())
                .WithNonGmoAgency(updatedNonGmoAgency.hierarchyClassID.ToString())
                .WithOrganicAgency(updatedOrganicAgency.hierarchyClassID.ToString())
                .WithPremiumBodyCare(updatedPremiumBodyCare)
                .WithSeafoodFreshOrFrozenId(updatedSeafoodFreshOrFrozen.SeafoodFreshOrFrozenId.ToString())
                .WithSeafoodCatchTypeId(updatedSeafoodCatchType.SeafoodCatchTypeId.ToString())
                .WithVeganAgency(updatedVeganAgency.hierarchyClassID.ToString())
                .WithVegetarian(updatedVegetarian)
                .WithWholeTrade(updatedWholeTrade)
                .WithGrassFed(updatedGrassFed)
                .WithPastureRaised(updatedPastureRaised)
                .WithFreeRange(updatedFreeRange)
                .WithDryAged(updatedDryAged)
                .WithAirChilled(updatedAirChilled)
                .WithMadeInHouse(updatedMadeInHouse);

            // When.
            bulkImportCommand.BulkImportData = new List<BulkImportItemModel> { bulkImportItem };
            commandHandler.Execute(bulkImportCommand);

            // Then.
            var newEvents = context.EventQueue.Where(q => q.EventReferenceId == item1.itemID).ToList();
            var newMessage = context.MessageQueueProduct.SingleOrDefault(q => q.ItemId == item1.itemID);

            Assert.AreEqual(0, newEvents.Count);
            Assert.IsNull(newMessage);
        }

        [TestMethod]
        public void BulkImportItem_RemovingSignAttributes_RemovableFieldsShouldHaveNullValues()
        {
            // Given.
            StageInitialSignAttributes();
            StageUpdatedSignAttributes();

            string initialBiodynamic = "1";
            string initialCheeseRaw = "0";
            string initialPremiumBodyCare = "0";
            string initialVegetarian = "1";
            string initialWholeTrade = "1";
            string initialGrassFed = "0";
            string initialPastureRaised = "0";
            string initialFreeRange = "0";
            string initialDryAged = "0";
            string initialAirChilled = "0";
            string initialMadeInHouse = "0";

            TestBulkImportItemModelBuilder bulkImportItem = new TestBulkImportItemModelBuilder()
                .Empty()
                .WithScanCode(scanCode1)
                .WithAnimalWelfareRatingId(initialAnimalWelfareRating.AnimalWelfareRatingId.ToString())
                .WithBiodynamic(initialBiodynamic)
                .WithMilkTypeId(initialMilkType.MilkTypeId.ToString())
                .WithCheeseRaw(initialCheeseRaw)
                .WithEcoScaleRatingId(initialEcoScaleRating.EcoScaleRatingId.ToString())
                .WithGlutenFreeAgency(initialGlutenFreeAgency.hierarchyClassID.ToString())
                .WithKosherAgency(initialKosherAgency.hierarchyClassID.ToString())
                .WithNonGmoAgency(initialNonGmoAgency.hierarchyClassID.ToString())
                .WithOrganicAgency(initialOrganicAgency.hierarchyClassID.ToString())
                .WithPremiumBodyCare(initialPremiumBodyCare)
                .WithSeafoodFreshOrFrozenId(initialSeafoodFreshOrFrozen.SeafoodFreshOrFrozenId.ToString())
                .WithSeafoodCatchTypeId(initialSeafoodCatchType.SeafoodCatchTypeId.ToString())
                .WithVeganAgency(initialVeganAgency.hierarchyClassID.ToString())
                .WithVegetarian(initialVegetarian)
                .WithWholeTrade(initialWholeTrade)
                .WithGrassFed(initialGrassFed)
                .WithPastureRaised(initialPastureRaised)
                .WithFreeRange(initialFreeRange)
                .WithDryAged(initialDryAged)
                .WithAirChilled(initialAirChilled)
                .WithMadeInHouse(initialMadeInHouse);

            bulkImportCommand = new BulkImportCommand<BulkImportItemModel>
            {
                UserName = "IntegrationTestUsername",
                BulkImportData = new List<BulkImportItemModel> { bulkImportItem }
            };

            commandHandler.Execute(bulkImportCommand);

            string updatedBiodynamic = "0";
            string updatedCheeseRaw = "1";
            string updatedPremiumBodyCare = "1";
            string updatedVegetarian = "0";
            string updatedWholeTrade = "0";
            string updatedGrassFed = "1";
            string updatedPastureRaised = "1";
            string updatedFreeRange = "1";
            string updatedDryAged = "1";
            string updatedAirChilled = "1";
            string updatedMadeInHouse = "1";

            bulkImportItem = new TestBulkImportItemModelBuilder()
                .Empty()
                .WithScanCode(scanCode1)
                .WithAnimalWelfareRatingId(null)
                .WithBiodynamic(updatedBiodynamic)
                .WithMilkTypeId(null)
                .WithCheeseRaw(updatedCheeseRaw)
                .WithEcoScaleRatingId(null)
                .WithGlutenFreeAgency(null)
                .WithKosherAgency(null)
                .WithNonGmoAgency(null)
                .WithOrganicAgency(null)
                .WithPremiumBodyCare(updatedPremiumBodyCare)
                .WithSeafoodFreshOrFrozenId(null)
                .WithSeafoodCatchTypeId(null)
                .WithVeganAgency(null)
                .WithVegetarian(updatedVegetarian)
                .WithWholeTrade(updatedWholeTrade)
                .WithGrassFed(updatedGrassFed)
                .WithPastureRaised(updatedPastureRaised)
                .WithFreeRange(updatedFreeRange)
                .WithDryAged(updatedDryAged)
                .WithAirChilled(updatedAirChilled)
                .WithMadeInHouse(updatedMadeInHouse);

            // When.
            bulkImportCommand.BulkImportData = new List<BulkImportItemModel> { bulkImportItem };
            commandHandler.Execute(bulkImportCommand);

            // Then.
            RefreshItem(item1);

            var actualSignAttributes = item1.ItemSignAttribute.Single(isa => isa.ItemID == item1.itemID);

            int? actualAnimalWelfareRatingId = actualSignAttributes.AnimalWelfareRatingId;
            bool actualBiodynamic = actualSignAttributes.Biodynamic;
            int? actualMilkTypeId = actualSignAttributes.CheeseMilkTypeId;
            bool actualCheeseRaw = actualSignAttributes.CheeseRaw;
            int? actualEcoScaleRatingId = actualSignAttributes.EcoScaleRatingId;
            int? actualGlutenFreeAgencyId = actualSignAttributes.GlutenFreeAgencyId;
            int? actualKosherAgencyId = actualSignAttributes.KosherAgencyId;
            int? actualNonGmoAgencyId = actualSignAttributes.NonGmoAgencyId;
            int? actualOrganicAgencyId = actualSignAttributes.OrganicAgencyId;
            bool actualPremiumBodyCare = actualSignAttributes.PremiumBodyCare;
            int? actualSeafoodFreshOrFrozenId = actualSignAttributes.SeafoodFreshOrFrozenId;
            int? actualSeafoodCatchTypeId = actualSignAttributes.SeafoodCatchTypeId;
            int? actualVeganAgencyId = actualSignAttributes.VeganAgencyId;
            bool actualVegetarian = actualSignAttributes.Vegetarian;
            bool actualWholeTrade = actualSignAttributes.WholeTrade;

            Assert.IsNull(actualAnimalWelfareRatingId);
            Assert.AreEqual(updatedBiodynamic == "1" ? true : false, actualBiodynamic);
            Assert.IsNull(actualMilkTypeId);
            Assert.AreEqual(updatedCheeseRaw == "1" ? true : false, actualCheeseRaw);
            Assert.IsNull(actualEcoScaleRatingId);
            Assert.IsNull(actualGlutenFreeAgencyId);
            Assert.IsNull(actualKosherAgencyId);
            Assert.IsNull(actualNonGmoAgencyId);
            Assert.IsNull(actualOrganicAgencyId);
            Assert.AreEqual(updatedPremiumBodyCare == "1" ? true : false, actualPremiumBodyCare);
            Assert.IsNull(actualSeafoodFreshOrFrozenId);
            Assert.IsNull(actualSeafoodCatchTypeId);
            Assert.IsNull(actualVeganAgencyId);
            Assert.AreEqual(updatedVegetarian == "1" ? true : false, actualVegetarian);
            Assert.AreEqual(updatedWholeTrade == "1" ? true : false, actualWholeTrade);
            Assert.AreEqual(updatedGrassFed == "1" ? true : false, actualSignAttributes.GrassFed);
            Assert.AreEqual(updatedPastureRaised == "1" ? true : false, actualSignAttributes.PastureRaised);
            Assert.AreEqual(updatedFreeRange == "1" ? true : false, actualSignAttributes.FreeRange);
            Assert.AreEqual(updatedDryAged == "1" ? true : false, actualSignAttributes.DryAged);
            Assert.AreEqual(updatedAirChilled == "1" ? true : false, actualSignAttributes.AirChilled);
            Assert.AreEqual(updatedMadeInHouse == "1" ? true : false, actualSignAttributes.MadeInHouse);
        }

        [TestMethod]
        public void BulkImportItem_RemovingSignAttributes_NoEventOrMessageShouldBeGenerated()
        {
            // Given.
            StageInitialSignAttributes();
            StageUpdatedSignAttributes();

            string initialBiodynamic = "1";
            string initialCheeseRaw = "0";
            string initialPremiumBodyCare = "0";
            string initialVegetarian = "1";
            string initialWholeTrade = "1";
            string initialGrassFed = "0";
            string initialPastureRaised = "0";
            string initialFreeRange = "0";
            string initialDryAged = "0";
            string initialAirChilled = "0";
            string initialMadeInHouse = "0";

            TestBulkImportItemModelBuilder bulkImportItem = new TestBulkImportItemModelBuilder()
                .Empty()
                .WithScanCode(scanCode1)
                .WithAnimalWelfareRatingId(initialAnimalWelfareRating.AnimalWelfareRatingId.ToString())
                .WithBiodynamic(initialBiodynamic)
                .WithMilkTypeId(initialMilkType.MilkTypeId.ToString())
                .WithCheeseRaw(initialCheeseRaw)
                .WithEcoScaleRatingId(initialEcoScaleRating.EcoScaleRatingId.ToString())
                .WithGlutenFreeAgency(initialGlutenFreeAgency.hierarchyClassID.ToString())
                .WithKosherAgency(initialKosherAgency.hierarchyClassID.ToString())
                .WithNonGmoAgency(initialNonGmoAgency.hierarchyClassID.ToString())
                .WithOrganicAgency(initialOrganicAgency.hierarchyClassID.ToString())
                .WithPremiumBodyCare(initialPremiumBodyCare)
                .WithSeafoodFreshOrFrozenId(initialSeafoodFreshOrFrozen.SeafoodFreshOrFrozenId.ToString())
                .WithSeafoodCatchTypeId(initialSeafoodCatchType.SeafoodCatchTypeId.ToString())
                .WithVeganAgency(initialVeganAgency.hierarchyClassID.ToString())
                .WithVegetarian(initialVegetarian)
                .WithWholeTrade(initialWholeTrade)
                .WithGrassFed(initialGrassFed)
                .WithPastureRaised(initialPastureRaised)
                .WithFreeRange(initialFreeRange)
                .WithDryAged(initialDryAged)
                .WithAirChilled(initialAirChilled)
                .WithMadeInHouse(initialMadeInHouse);

            bulkImportCommand = new BulkImportCommand<BulkImportItemModel>
            {
                UserName = "IntegrationTestUsername",
                BulkImportData = new List<BulkImportItemModel> { bulkImportItem }
            };

            commandHandler.Execute(bulkImportCommand);

            string updatedBiodynamic = "0";
            string updatedCheeseRaw = "1";
            string updatedPremiumBodyCare = "1";
            string updatedVegetarian = "0";
            string updatedWholeTrade = "0";
            string updatedGrassFed = "1";
            string updatedPastureRaised = "1";
            string updatedFreeRange = "1";
            string updatedDryAged = "1";
            string updatedAirChilled = "1";
            string updatedMadeInHouse = "1";

            bulkImportItem = new TestBulkImportItemModelBuilder()
                .Empty()
                .WithScanCode(scanCode1)
                .WithAnimalWelfareRatingId(null)
                .WithBiodynamic(updatedBiodynamic)
                .WithMilkTypeId(null)
                .WithCheeseRaw(updatedCheeseRaw)
                .WithEcoScaleRatingId(null)
                .WithGlutenFreeAgency(null)
                .WithKosherAgency(null)
                .WithNonGmoAgency(null)
                .WithOrganicAgency(null)
                .WithPremiumBodyCare(updatedPremiumBodyCare)
                .WithSeafoodFreshOrFrozenId(null)
                .WithSeafoodCatchTypeId(null)
                .WithVeganAgency(null)
                .WithVegetarian(updatedVegetarian)
                .WithWholeTrade(updatedWholeTrade)
                .WithGrassFed(updatedGrassFed)
                .WithPastureRaised(updatedPastureRaised)
                .WithFreeRange(updatedFreeRange)
                .WithDryAged(updatedDryAged)
                .WithAirChilled(updatedAirChilled)
                .WithMadeInHouse(updatedMadeInHouse);

            // When.
            bulkImportCommand.BulkImportData = new List<BulkImportItemModel> { bulkImportItem };
            commandHandler.Execute(bulkImportCommand);

            // Then.
            var newEvents = context.EventQueue.Where(q => q.EventReferenceId == item1.itemID).ToList();
            var newMessage = context.MessageQueueProduct.SingleOrDefault(q => q.ItemId == item1.itemID);

            Assert.AreEqual(0, newEvents.Count);
            Assert.IsNull(newMessage);
        }

        [TestMethod]
        public void BulkImportItem_DepartmentSaleValueIsUpdatedToTrue_DepartmentSaleTraitShouldBeAdded()
        {
            // Given.
            string testDepartmentSale = "1";

            TestBulkImportItemModelBuilder bulkImportItem = new TestBulkImportItemModelBuilder()
                .WithScanCode(scanCode1)
                .WithBrandId(updatedTestBrand.hierarchyClassID.ToString())
                .WithMerchandiseId(updatedTestMerchandise.hierarchyClassID.ToString())
                .WithTaxId(updatedTestTax.hierarchyClassID.ToString())
                .WithNationalId(updatedTestNational.hierarchyClassID.ToString())
                .WithBrowsingId(updatedTestBrowsing.hierarchyClassID.ToString())
                .WithDepartmentSale(testDepartmentSale);

            bulkImportCommand = new BulkImportCommand<BulkImportItemModel>
            {
                UserName = "IntegrationTestUsername",
                BulkImportData = new List<BulkImportItemModel> { bulkImportItem }
            };

            // When.
            commandHandler.Execute(bulkImportCommand);

            // Then.
            RefreshItem(item1);

            string actualDepartmentSale = item1.ItemTrait.Single(it => it.traitID == Traits.DepartmentSale).traitValue;

            Assert.AreEqual(testDepartmentSale, actualDepartmentSale);
        }

        [TestMethod]
        public void BulkImportItem_DepartmentSaleValueIsUpdatedToFalse_DepartmentSaleTraitShouldBeRemoved()
        {
            // Given.
            var departmentSaleItemTrait = new ItemTrait { itemID = item1.itemID, traitID = Traits.DepartmentSale, traitValue = "1", localeID = 1 };

            item1.ItemTrait.Add(departmentSaleItemTrait);
            context.SaveChanges();

            string testDepartmentSale = "0";

            TestBulkImportItemModelBuilder bulkImportItem = new TestBulkImportItemModelBuilder()
                .WithScanCode(scanCode1)
                .WithBrandId(updatedTestBrand.hierarchyClassID.ToString())
                .WithMerchandiseId(updatedTestMerchandise.hierarchyClassID.ToString())
                .WithTaxId(updatedTestTax.hierarchyClassID.ToString())
                .WithNationalId(updatedTestNational.hierarchyClassID.ToString())
                .WithBrowsingId(updatedTestBrowsing.hierarchyClassID.ToString())
                .WithDepartmentSale(testDepartmentSale);

            bulkImportCommand = new BulkImportCommand<BulkImportItemModel>
            {
                UserName = "IntegrationTestUsername",
                BulkImportData = new List<BulkImportItemModel> { bulkImportItem }
            };

            // When.
            commandHandler.Execute(bulkImportCommand);

            // Then.
            RefreshItem(item1);

            var actualDepartmentSale = item1.ItemTrait.SingleOrDefault(it => it.traitID == Traits.DepartmentSale);

            Assert.IsNull(actualDepartmentSale);
        }

        [TestMethod]
        public void BulkImportItem_HiddenItemIsTrue_HiddenItemTraitShouldBeAdded()
        {
            // Given.
            string testHiddenItem = "1";

            TestBulkImportItemModelBuilder bulkImportItem = new TestBulkImportItemModelBuilder()
                .WithScanCode(scanCode1)
                .WithBrandId(updatedTestBrand.hierarchyClassID.ToString())
                .WithMerchandiseId(updatedTestMerchandise.hierarchyClassID.ToString())
                .WithTaxId(updatedTestTax.hierarchyClassID.ToString())
                .WithNationalId(updatedTestNational.hierarchyClassID.ToString())
                .WithBrowsingId(updatedTestBrowsing.hierarchyClassID.ToString())
                .WithHiddenItem(testHiddenItem);

            bulkImportCommand = new BulkImportCommand<BulkImportItemModel>
            {
                UserName = "IntegrationTestUsername",
                BulkImportData = new List<BulkImportItemModel> { bulkImportItem }
            };

            // When.
            commandHandler.Execute(bulkImportCommand);

            // Then.
            RefreshItem(item1);

            string actualHiddenItem = item1.ItemTrait.Single(it => it.traitID == Traits.HiddenItem).traitValue;

            Assert.AreEqual(testHiddenItem, actualHiddenItem);
        }

        [TestMethod]
        public void BulkImportItem_HiddenItemIsFalse_HiddenItemTraitShouldBeRemoved()
        {
            // Given.
            var hiddenItemTrait = new ItemTrait { itemID = item1.itemID, traitID = Traits.HiddenItem, traitValue = "1", localeID = 1 };

            item1.ItemTrait.Add(hiddenItemTrait);
            context.SaveChanges();

            string testHiddenItem = "0";

            TestBulkImportItemModelBuilder bulkImportItem = new TestBulkImportItemModelBuilder()
                .WithScanCode(scanCode1)
                .WithBrandId(updatedTestBrand.hierarchyClassID.ToString())
                .WithMerchandiseId(updatedTestMerchandise.hierarchyClassID.ToString())
                .WithTaxId(updatedTestTax.hierarchyClassID.ToString())
                .WithNationalId(updatedTestNational.hierarchyClassID.ToString())
                .WithBrowsingId(updatedTestBrowsing.hierarchyClassID.ToString())
                .WithHiddenItem(testHiddenItem);

            bulkImportCommand = new BulkImportCommand<BulkImportItemModel>
            {
                UserName = "IntegrationTestUsername",
                BulkImportData = new List<BulkImportItemModel> { bulkImportItem }
            };

            // When.
            commandHandler.Execute(bulkImportCommand);

            // Then.
            RefreshItem(item1);

            var actualHiddenItem = item1.ItemTrait.SingleOrDefault(it => it.traitID == Traits.HiddenItem);

            Assert.IsNull(actualHiddenItem);
        }

        [TestMethod]
        public void BulkImportItem_HiddenItemIsTrue_NoEventOrMessageShouldBeGenerated()
        {
            // Given.
            string testHiddenItem = "1";

            TestBulkImportItemModelBuilder bulkImportItem = new TestBulkImportItemModelBuilder()
                .Empty()
                .WithScanCode(scanCode1)
                .WithHiddenItem(testHiddenItem);

            bulkImportCommand = new BulkImportCommand<BulkImportItemModel>
            {
                UserName = "IntegrationTestUsername",
                BulkImportData = new List<BulkImportItemModel> { bulkImportItem }
            };

            // When.
            commandHandler.Execute(bulkImportCommand);

            // Then.
            var newEvent = context.EventQueue.SingleOrDefault(q => q.EventReferenceId == item1.itemID);
            var newMessage = context.MessageQueueProduct.SingleOrDefault(q => q.ItemId == item1.itemID);

            Assert.IsNull(newEvent);
            Assert.IsNull(newMessage);
        }

        [TestMethod]
        public void BulkImportItem_HiddenItemIsFalse_NoEventOrMessageShouldBeGenerated()
        {
            // Given.
            var hiddenItemTrait = new ItemTrait { itemID = item1.itemID, traitID = Traits.HiddenItem, traitValue = "1", localeID = 1 };

            item1.ItemTrait.Add(hiddenItemTrait);
            context.SaveChanges();

            string testHiddenItem = "0";

            TestBulkImportItemModelBuilder bulkImportItem = new TestBulkImportItemModelBuilder()
                .Empty()
                .WithScanCode(scanCode1)
                .WithHiddenItem(testHiddenItem);

            bulkImportCommand = new BulkImportCommand<BulkImportItemModel>
            {
                UserName = "IntegrationTestUsername",
                BulkImportData = new List<BulkImportItemModel> { bulkImportItem }
            };

            // When.
            commandHandler.Execute(bulkImportCommand);

            // Then.
            var newEvent = context.EventQueue.SingleOrDefault(q => q.EventReferenceId == item1.itemID);
            var newMessage = context.MessageQueueProduct.SingleOrDefault(q => q.ItemId == item1.itemID);

            Assert.IsNull(newEvent);
            Assert.IsNull(newMessage);
        }

        [TestMethod]
        public void BulkImportItem_NoteIsBlank_NoteItemTraitShouldNotBeRemoved()
        {
            // Given.
            string testNote = "Test note.";
            string blankNote = "";

            TestBulkImportItemModelBuilder bulkImportItem = new TestBulkImportItemModelBuilder()
                .WithScanCode(scanCode1)
                .WithBrandId(updatedTestBrand.hierarchyClassID.ToString())
                .WithMerchandiseId(updatedTestMerchandise.hierarchyClassID.ToString())
                .WithTaxId(updatedTestTax.hierarchyClassID.ToString())
                .WithNationalId(updatedTestNational.hierarchyClassID.ToString())
                .WithBrowsingId(updatedTestBrowsing.hierarchyClassID.ToString())
                .WithNote(testNote);

            bulkImportCommand = new BulkImportCommand<BulkImportItemModel>
            {
                UserName = "IntegrationTestUsername",
                BulkImportData = new List<BulkImportItemModel> { bulkImportItem }
            };

            commandHandler.Execute(bulkImportCommand);

            // When.
            bulkImportItem.WithNote(blankNote);

            bulkImportCommand = new BulkImportCommand<BulkImportItemModel>
            {
                UserName = "IntegrationTestUsername",
                BulkImportData = new List<BulkImportItemModel> { bulkImportItem },
                UpdateAllItemFields = false
            };

            // When.
            commandHandler.Execute(bulkImportCommand);

            // Then.
            RefreshItem(item1);

            string actualNote = item1.ItemTrait.Single(it => it.traitID == Traits.Notes).traitValue;

            Assert.AreEqual(testNote, actualNote);
        }

        [TestMethod]
        public void BulkImportItem_NoteIsAdded_NoteItemTraitShouldBeAdded()
        {
            // Given.
            string testNote = "Test note.";

            TestBulkImportItemModelBuilder bulkImportItem = new TestBulkImportItemModelBuilder()
                .WithScanCode(scanCode1)
                .WithBrandId(updatedTestBrand.hierarchyClassID.ToString())
                .WithMerchandiseId(updatedTestMerchandise.hierarchyClassID.ToString())
                .WithTaxId(updatedTestTax.hierarchyClassID.ToString())
                .WithNationalId(updatedTestNational.hierarchyClassID.ToString())
                .WithBrowsingId(updatedTestBrowsing.hierarchyClassID.ToString())
                .WithNote(testNote);

            bulkImportCommand = new BulkImportCommand<BulkImportItemModel>
            {
                UserName = "IntegrationTestUsername",
                BulkImportData = new List<BulkImportItemModel> { bulkImportItem }
            };

            // When.
            commandHandler.Execute(bulkImportCommand);

            // Then.
            RefreshItem(item1);

            string actualNote = item1.ItemTrait.Single(it => it.traitID == Traits.Notes).traitValue;

            Assert.AreEqual(testNote, actualNote);
        }

        [TestMethod]
        public void BulkImportItem_NoteIsRemoved_NoteItemTraitShouldNotBeRemoved()
        {
            // Given.
            var noteItemTrait = new ItemTrait { itemID = item1.itemID, traitID = Traits.Notes, traitValue = "Test note.", localeID = 1 };

            item1.ItemTrait.Add(noteItemTrait);
            context.SaveChanges();

            string testNote = String.Empty;

            TestBulkImportItemModelBuilder bulkImportItem = new TestBulkImportItemModelBuilder()
                .WithScanCode(scanCode1)
                .WithBrandId(updatedTestBrand.hierarchyClassID.ToString())
                .WithMerchandiseId(updatedTestMerchandise.hierarchyClassID.ToString())
                .WithTaxId(updatedTestTax.hierarchyClassID.ToString())
                .WithNationalId(updatedTestNational.hierarchyClassID.ToString())
                .WithBrowsingId(updatedTestBrowsing.hierarchyClassID.ToString())
                .WithNote(testNote);

            bulkImportCommand = new BulkImportCommand<BulkImportItemModel>
            {
                UserName = "IntegrationTestUsername",
                BulkImportData = new List<BulkImportItemModel> { bulkImportItem }
            };

            // When.
            commandHandler.Execute(bulkImportCommand);

            // Then.
            RefreshItem(item1);

            var actualNote = item1.ItemTrait.SingleOrDefault(it => it.traitID == Traits.Notes);

            Assert.IsNotNull(actualNote);
        }

        [TestMethod]
        public void BulkImportItem_NoteIsAdded_NoEventOrMessageShouldBeGenerated()
        {
            // Given.
            string testNote = "Test note.";

            TestBulkImportItemModelBuilder bulkImportItem = new TestBulkImportItemModelBuilder()
                .Empty()
                .WithScanCode(scanCode1)
                .WithNote(testNote);

            bulkImportCommand = new BulkImportCommand<BulkImportItemModel>
            {
                UserName = "IntegrationTestUsername",
                BulkImportData = new List<BulkImportItemModel> { bulkImportItem }
            };

            // When.
            commandHandler.Execute(bulkImportCommand);

            // Then.
            var newEvent = context.EventQueue.SingleOrDefault(q => q.EventReferenceId == item1.itemID);
            var newMessage = context.MessageQueueProduct.SingleOrDefault(q => q.ItemId == item1.itemID);

            Assert.IsNull(newEvent);
            Assert.IsNull(newMessage);
        }

        [TestMethod]
        public void BulkImportItem_NoteIsRemoved_NoEventOrMessageShouldBeGenerated()
        {
            // Given.
            var noteItemTrait = new ItemTrait { itemID = item1.itemID, traitID = Traits.Notes, traitValue = "Test note.", localeID = 1 };

            item1.ItemTrait.Add(noteItemTrait);
            context.SaveChanges();

            string testNote = String.Empty;

            TestBulkImportItemModelBuilder bulkImportItem = new TestBulkImportItemModelBuilder()
                .Empty()
                .WithScanCode(scanCode1)
                .WithNote(testNote);

            bulkImportCommand = new BulkImportCommand<BulkImportItemModel>
            {
                UserName = "IntegrationTestUsername",
                BulkImportData = new List<BulkImportItemModel> { bulkImportItem }
            };

            // When.
            commandHandler.Execute(bulkImportCommand);

            // Then.
            var newEvent = context.EventQueue.SingleOrDefault(q => q.EventReferenceId == item1.itemID);
            var newMessage = context.MessageQueueProduct.SingleOrDefault(q => q.ItemId == item1.itemID);

            Assert.IsNull(newEvent);
            Assert.IsNull(newMessage);
        }

        [TestMethod]
        [ExpectedException(typeof(SqlException))]
        public void BulkImportItem_ErrorWithUpload_ThrowsException()
        {
            // Form string that is over 13 characters in length so that it doesn't match Db Table Type
            string upc = "56474567811452756";

            // Given - Setup error with PosScaleTare to have more than 255 characters.
            bulkImportCommand = new BulkImportCommand<BulkImportItemModel>
            {
                UserName = "IntegrationTestUsername",
                BulkImportData = new List<BulkImportItemModel>
                {
                    new BulkImportItemModel
                    {
                        ScanCode = upc, 
                        BrandLineage = "Bulk Import Test Brand",
                        ProductDescription = String.Format("Bulk Import Test Description {0}", DateTime.Now),
                        PosDescription = String.Format("Bulk Import Pos Test Description {0}", DateTime.Now),
                        PackageUnit = "EACH",
                        FoodStampEligible = "Y",
                        PosScaleTare = "0"
                    }
                }
            };

            // When & Then - SqlException expected.
            commandHandler.Execute(bulkImportCommand);
        }

        [TestMethod]
        public void BulkImportItem_Logging_VerifyLoggingIsCalled()
        {
            // Given.
            bulkImportCommand = new BulkImportCommand<BulkImportItemModel>
            {
                UserName = "IntegrationTestUsername",
                BulkImportData = new List<BulkImportItemModel>
                {
                    new BulkImportItemModel 
                    { 
                        ScanCode = scanCode4, 
                        BrandLineage = "Bulk Import Test Brand",
                        BrandId = String.Empty,
                        ProductDescription = "Bulk Import Test Description 1",
                        PosDescription = "Bulk Import Pos Test Description 1",
                        PackageUnit = "EACH",
                        FoodStampEligible = "0",
                        PosScaleTare = "10",
                        RetailSize = "12",
                        RetailUom = "Case",
                        DeliverySystem = "CAP",
                        MerchandiseLineage = String.Empty,
                        MerchandiseId = String.Empty,
                        TaxLineage = String.Empty,
                        TaxId = String.Empty,
                        BrowsingLineage = String.Empty,
                        BrowsingId = String.Empty,
                        IsValidated = "0"
                    },
                    new BulkImportItemModel
                    {
                        ScanCode = scanCode5,
                        BrandLineage = "Bulk Import Test Brand",
                        BrandId = String.Empty,
                        ProductDescription = "Bulk Import Test Description 2",
                        PosDescription = "Bulk Import Pos Test Description 2",
                        PackageUnit = "EACH",
                        FoodStampEligible = "0",
                        PosScaleTare = "10",
                        RetailSize = "12",
                        RetailUom = "Case",
                        DeliverySystem = "CAP",
                        MerchandiseLineage = String.Empty,
                        MerchandiseId = String.Empty,
                        TaxLineage = String.Empty,
                        TaxId = String.Empty,
                        BrowsingLineage = String.Empty,
                        BrowsingId = String.Empty,
                        IsValidated = "0"
                    },
                    new BulkImportItemModel
                    {
                        ScanCode = scanCode6,
                        BrandLineage = "Bulk Import Test Brand",
                        BrandId = String.Empty,
                        ProductDescription = "Bulk Import Test Description 3",
                        PosDescription = "Bulk Import Pos Test Description 3",
                        PackageUnit = "EACH",
                        FoodStampEligible = "0",
                        PosScaleTare = "10",
                        RetailSize = "12",
                        RetailUom = "Case",
                        DeliverySystem = "CAP",
                        MerchandiseLineage = String.Empty,
                        MerchandiseId = String.Empty,
                        TaxLineage = String.Empty,
                        TaxId = String.Empty,
                        BrowsingLineage = String.Empty,
                        BrowsingId = String.Empty,
                        IsValidated = "0"
                    }
                }
            };

            // When.
            commandHandler.Execute(bulkImportCommand);

            // Then.
            mockLogger.Verify(log => log.Info(It.IsAny<string>()), Times.AtLeastOnce);
        }

        [TestMethod]
        public void BulkImportItem_IrmaItemSubscriptionExists_CreatesItemUpdatedEvent()
        {
            // Given.
            var irmaItemSubscription = new IRMAItemSubscription
            {
                identifier = scanCode1,
                regioncode = "SW"
            };

            context.IRMAItemSubscription.Add(irmaItemSubscription);
            context.SaveChanges();

            var importData = new BulkImportCommand<BulkImportItemModel>
            {
                UserName = "IntegrationTestUsername",
                BulkImportData = new List<BulkImportItemModel>
                {
                    new BulkImportItemModel 
                    { 
                        ScanCode = scanCode1, 
                        BrandLineage = updatedTestBrand.hierarchyClassName,
                        BrandId = updatedTestBrand.hierarchyClassID.ToString(),
                        ProductDescription = "Bulk Import Test Description 1",
                        PosDescription = "Bulk Import Pos Test Description 1",
                        PackageUnit = "1",
                        FoodStampEligible = "0",
                        PosScaleTare = "10",
                        RetailSize = "",
                        RetailUom = "",
                        DeliverySystem = "",
                        MerchandiseLineage = updatedTestMerchandise.hierarchyClassName,
                        MerchandiseId = updatedTestMerchandise.hierarchyClassID.ToString(),
                        TaxLineage = updatedTestTax.hierarchyClassName,
                        TaxId = updatedTestTax.hierarchyClassID.ToString(),
                        BrowsingLineage = updatedTestBrowsing.hierarchyClassName,
                        BrowsingId = updatedTestBrowsing.hierarchyClassID.ToString(),
                        IsValidated = "0"
                    }
                }
            };

            // When.
            commandHandler.Execute(importData);

            // Then.
            var result = context.EventQueue.SingleOrDefault(e => e.EventMessage == scanCode1 && e.EventId == EventTypes.ItemUpdate);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.EventId, EventTypes.ItemUpdate);
            Assert.AreEqual(result.EventReferenceId, item1.itemID);
            Assert.AreEqual(result.RegionCode, "SW");
        }

        [TestMethod]
        public void BulkImportItem_IrmaItemSubscriptionExistsAndValidationOccurs_CreatesItemValidationEvent()
        {
            // Given.
            var irmaItemSubscription = new IRMAItemSubscription
            {
                identifier = scanCode2,
                regioncode = "SW"
            };

            context.IRMAItemSubscription.Add(irmaItemSubscription);
            context.SaveChanges();

            var importData = new BulkImportCommand<BulkImportItemModel>
            {
                UserName = "IntegrationTestUsername",
                BulkImportData = new List<BulkImportItemModel>
                {
                    new BulkImportItemModel 
                    { 
                        ScanCode = scanCode2, 
                        BrandLineage = updatedTestBrand.hierarchyClassName,
                        BrandId = updatedTestBrand.hierarchyClassID.ToString(),
                        ProductDescription = "Bulk Import Test Description 1",
                        PosDescription = "Bulk Import Pos Test Description 1",
                        PackageUnit = "1",
                        FoodStampEligible = "0",
                        PosScaleTare = "10",
                        RetailSize = "",
                        RetailUom = "",
                        DeliverySystem = "",
                        MerchandiseLineage = updatedTestMerchandise.hierarchyClassName,
                        MerchandiseId = updatedTestMerchandise.hierarchyClassID.ToString(),
                        TaxLineage = updatedTestTax.hierarchyClassName,
                        TaxId = updatedTestTax.hierarchyClassID.ToString(),
                        BrowsingLineage = updatedTestBrowsing.hierarchyClassName,
                        BrowsingId = updatedTestBrowsing.hierarchyClassID.ToString(),
                        IsValidated = "1"
                    }
                }
            };

            // When.
            commandHandler.Execute(importData);

            // Then.
            var result = context.EventQueue.SingleOrDefault(e => e.EventMessage == scanCode2 && e.EventId == EventTypes.ItemValidation);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.EventId, EventTypes.ItemValidation);
            Assert.AreEqual(result.EventReferenceId, item2.itemID);
            Assert.AreEqual(result.RegionCode, "SW");
        }

        [TestMethod]
        public void BulkImportItem_ValidationOccursForAlreadyValidatedItem_CreatesItemUpdateEvent()
        {
            // Given.
            var irmaItemSubscription = new IRMAItemSubscription
            {
                identifier = scanCode1,
                regioncode = "SW"
            };

            context.IRMAItemSubscription.Add(irmaItemSubscription);
            context.SaveChanges();

            var importData = new BulkImportCommand<BulkImportItemModel>
            {
                UserName = "IntegrationTestUsername",
                BulkImportData = new List<BulkImportItemModel>
                {
                    new BulkImportItemModel 
                    { 
                        ScanCode = scanCode1, 
                        BrandLineage = updatedTestBrand.hierarchyClassName,
                        BrandId = updatedTestBrand.hierarchyClassID.ToString(),
                        ProductDescription = "Bulk Import Test Description 1",
                        PosDescription = "Bulk Import Pos Test Description 1",
                        PackageUnit = "1",
                        FoodStampEligible = "0",
                        PosScaleTare = "10",
                        RetailSize = "",
                        RetailUom = "",
                        DeliverySystem = "",
                        MerchandiseLineage = updatedTestMerchandise.hierarchyClassName,
                        MerchandiseId = updatedTestMerchandise.hierarchyClassID.ToString(),
                        TaxLineage = updatedTestTax.hierarchyClassName,
                        TaxId = updatedTestTax.hierarchyClassID.ToString(),
                        BrowsingLineage = updatedTestBrowsing.hierarchyClassName,
                        BrowsingId = updatedTestBrowsing.hierarchyClassID.ToString(),
                        IsValidated = "1"
                    }
                }
            };

            // When.
            commandHandler.Execute(importData);

            // Then.
            var result = context.EventQueue.SingleOrDefault(e => e.EventMessage == scanCode1 && e.EventId == EventTypes.ItemUpdate);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.EventId, EventTypes.ItemUpdate);
            Assert.AreEqual(result.EventReferenceId, item1.itemID);
            Assert.AreEqual(result.RegionCode, "SW");
        }

        [TestMethod]
        public void BulkImportItem_ItemIsValidatedAndRequiredHierarchiesAreSentToEsb_ShouldGenerateReadyMessageForEsb()
        {
            // Given.
            var item = context.Item.Single(i => i.ScanCode.Any(sc => sc.scanCode == scanCode2));

            context.HierarchyClassTrait.AddRange(new List<HierarchyClassTrait> 
            {
                new HierarchyClassTrait
                {
                    hierarchyClassID = updatedTestMerchandise.hierarchyClassID,
                    traitID = Traits.MerchFinMapping,
                    traitValue = updatedTestFinancial.hierarchyClassName
                },
                new HierarchyClassTrait
                {
                    hierarchyClassID = updatedTestMerchandise.hierarchyClassID,
                    traitID = Traits.SentToEsb,
                    traitValue = DateTime.Now.ToString()
                },
                new HierarchyClassTrait
                {
                    hierarchyClassID = updatedTestBrand.hierarchyClassID,
                    traitID = Traits.SentToEsb,
                    traitValue = DateTime.Now.ToString()
                },
                new HierarchyClassTrait
                {
                    hierarchyClassID = updatedTestFinancial.hierarchyClassID,
                    traitID = Traits.SentToEsb,
                    traitValue = DateTime.Now.ToString()
                }
            });

            context.ItemTrait.Add(new ItemTrait
            {
                itemID = item.itemID,
                localeID = 1,
                traitID = Traits.ValidationDate,
                traitValue = DateTime.Now.ToString()
            });

            context.SaveChanges();

            bulkImportCommand = new BulkImportCommand<BulkImportItemModel>
            {
                UserName = "IntegrationTestUsername",
                BulkImportData = new List<BulkImportItemModel>
                {
                    new BulkImportItemModel 
                    { 
                        ScanCode = scanCode1, 
                        BrandLineage = updatedTestBrand.hierarchyClassName,
                        BrandId = updatedTestBrand.hierarchyClassID.ToString(),
                        ProductDescription = "Bulk Import Test Description 1",
                        PosDescription = "Bulk Import Pos Test Description 1",
                        PackageUnit = "1",
                        FoodStampEligible = "0",
                        PosScaleTare = "10",
                        RetailSize = String.Empty,
                        RetailUom = String.Empty,
                        DeliverySystem = String.Empty,
                        MerchandiseLineage = updatedTestMerchandise.hierarchyClassName,
                        MerchandiseId = updatedTestMerchandise.hierarchyClassID.ToString(),
                        TaxLineage = updatedTestTax.hierarchyClassName,
                        TaxId = updatedTestTax.hierarchyClassID.ToString(),
                        BrowsingLineage = updatedTestBrowsing.hierarchyClassName,
                        BrowsingId = updatedTestBrowsing.hierarchyClassID.ToString(),
                        IsValidated = "1"
                    }
                }
            };

            // When.
            commandHandler.Execute(bulkImportCommand);

            // Then.
            var message = context.MessageQueueProduct.Single(p => p.ScanCode == scanCode1);

            Assert.AreEqual(MessageStatusTypes.Ready, message.MessageStatusId);
        }

        [TestMethod]
        public void BulkImportItem_NonRetailItemIsValidatedAndRequiredHierarchiesAreSentToEsb_ShouldGenerateReadyMessageForEsb()
        {
            // Given.
            //Non retailt item type (6)
            var item = context.Item.Single(i => i.ScanCode.Any(sc => sc.scanCode == scanCode2));

            context.HierarchyClassTrait.AddRange(new List<HierarchyClassTrait> 
            {
                new HierarchyClassTrait
                {
                    hierarchyClassID = updatedTestMerchandise.hierarchyClassID,
                    traitID = Traits.MerchFinMapping,
                    traitValue = updatedTestFinancial.hierarchyClassName
                },
                new HierarchyClassTrait
                {
                    hierarchyClassID = updatedTestMerchandise.hierarchyClassID,
                    traitID = Traits.SentToEsb,
                    traitValue = DateTime.Now.ToString()
                },
                new HierarchyClassTrait
                {
                    hierarchyClassID = updatedTestBrand.hierarchyClassID,
                    traitID = Traits.SentToEsb,
                    traitValue = DateTime.Now.ToString()
                },
                new HierarchyClassTrait
                {
                    hierarchyClassID = updatedTestFinancial.hierarchyClassID,
                    traitID = Traits.SentToEsb,
                    traitValue = DateTime.Now.ToString()
                }
            });

            context.ItemTrait.Add(new ItemTrait
            {
                itemID = item.itemID,
                localeID = 1,
                traitID = Traits.ValidationDate,
                traitValue = DateTime.Now.ToString()
            });

            context.SaveChanges();

            bulkImportCommand = new BulkImportCommand<BulkImportItemModel>
            {
                UserName = "IntegrationTestUsername",
                BulkImportData = new List<BulkImportItemModel>
                {
                    new BulkImportItemModel 
                    { 
                        ScanCode = scanCode1, 
                        BrandLineage = updatedTestBrand.hierarchyClassName,
                        BrandId = updatedTestBrand.hierarchyClassID.ToString(),
                        ProductDescription = "Bulk Import Test Description 1",
                        PosDescription = "Bulk Import Pos Test Description 1",
                        PackageUnit = "1",
                        FoodStampEligible = "0",
                        PosScaleTare = "10",
                        RetailSize = String.Empty,
                        RetailUom = String.Empty,
                        DeliverySystem = String.Empty,
                        MerchandiseLineage = updatedTestMerchandise.hierarchyClassName,
                        MerchandiseId = updatedTestMerchandise.hierarchyClassID.ToString(),
                        TaxLineage = updatedTestTax.hierarchyClassName,
                        TaxId = updatedTestTax.hierarchyClassID.ToString(),
                        BrowsingLineage = updatedTestBrowsing.hierarchyClassName,
                        BrowsingId = updatedTestBrowsing.hierarchyClassID.ToString(),
                        IsValidated = "1"
                    }
                }
            };

            // When.
            commandHandler.Execute(bulkImportCommand);

            // Then.
            var message = context.MessageQueueProduct.Single(p => p.ScanCode == scanCode1);

            Assert.AreEqual(MessageStatusTypes.Ready, message.MessageStatusId);
        }

        [TestMethod]
        public void BulkImportItem_ItemIsValidatedAndRequiredHierarchiesAreSentToEsb_ShouldGenerateReadyMessageForEsbWithSignAttributes()
        {
            // Given.
            //Non retailt item type (6)
            // Given.
            StageInitialSignAttributes();

            string initialBiodynamic = "1";
            string initialCheeseRaw = "0";
            string initialPremiumBodyCare = "0";
            string initialVegetarian = "1";
            string initialWholeTrade = "1";
            string initialGrassFed = "0";
            string initialPastureRaised = "0";
            string initialFreeRange = "0";
            string initialDryAged = "0";
            string initialAirChilled = "0";
            string initialMadeInHouse = "0";

            TestBulkImportItemModelBuilder bulkImportItem = new TestBulkImportItemModelBuilder()
                .Empty()
                .WithScanCode(scanCode1)
                .WithAnimalWelfareRatingId(initialAnimalWelfareRating.AnimalWelfareRatingId.ToString())
                .WithBiodynamic(initialBiodynamic)
                .WithMilkTypeId(initialMilkType.MilkTypeId.ToString())
                .WithCheeseRaw(initialCheeseRaw)
                .WithEcoScaleRatingId(initialEcoScaleRating.EcoScaleRatingId.ToString())
                .WithGlutenFreeAgency(initialGlutenFreeAgency.hierarchyClassID.ToString())
                .WithKosherAgency(initialKosherAgency.hierarchyClassID.ToString())
                .WithNonGmoAgency(initialNonGmoAgency.hierarchyClassID.ToString())
                .WithOrganicAgency(initialOrganicAgency.hierarchyClassID.ToString())
                .WithPremiumBodyCare(initialPremiumBodyCare)
                .WithSeafoodFreshOrFrozenId(initialSeafoodFreshOrFrozen.SeafoodFreshOrFrozenId.ToString())
                .WithSeafoodCatchTypeId(initialSeafoodCatchType.SeafoodCatchTypeId.ToString())
                .WithVeganAgency(initialVeganAgency.hierarchyClassID.ToString())
                .WithVegetarian(initialVegetarian)
                .WithWholeTrade(initialWholeTrade)
                .WithGrassFed(initialGrassFed)
                .WithPastureRaised(initialPastureRaised)
                .WithFreeRange(initialFreeRange)
                .WithDryAged(initialDryAged)
                .WithAirChilled(initialAirChilled)
                .WithMadeInHouse(initialMadeInHouse)
                .WithBrandLineage(updatedTestBrand.hierarchyClassName)
                .WithBrandId(updatedTestBrand.hierarchyClassID.ToString())
                .WithProductDescription("Bulk Import Test Description 1")
                .WithPosDescription("Bulk Import Pos Test Description 1")
                .WithPackageUnit("1")
                .WithFoodStampEligible("0")
                .WithPosScaleTare("10")
                .WithRetailSize(String.Empty)
                .WithRetailUom(String.Empty)
                .WithMerchandiseLineage(updatedTestMerchandise.hierarchyClassName)
                .WithMerchandiseId(updatedTestMerchandise.hierarchyClassID.ToString())
                .WithTaxLineage(updatedTestTax.hierarchyClassName)
                .WithTaxId(updatedTestTax.hierarchyClassID.ToString())
                .WithBrowsingLineage(updatedTestBrowsing.hierarchyClassName)
                .WithBrowsingId(updatedTestBrowsing.hierarchyClassID.ToString())
                .WithIsValidated("1");

            bulkImportCommand = new BulkImportCommand<BulkImportItemModel>
            {
                UserName = "IntegrationTestUsername",
                BulkImportData = new List<BulkImportItemModel> { bulkImportItem }
            };

            
            var item = context.Item.Single(i => i.ScanCode.Any(sc => sc.scanCode == scanCode2));

            context.HierarchyClassTrait.AddRange(new List<HierarchyClassTrait> 
            {
                new HierarchyClassTrait
                {
                    hierarchyClassID = updatedTestMerchandise.hierarchyClassID,
                    traitID = Traits.MerchFinMapping,
                    traitValue = updatedTestFinancial.hierarchyClassName
                },
                new HierarchyClassTrait
                {
                    hierarchyClassID = updatedTestMerchandise.hierarchyClassID,
                    traitID = Traits.SentToEsb,
                    traitValue = DateTime.Now.ToString()
                },
                new HierarchyClassTrait
                {
                    hierarchyClassID = updatedTestBrand.hierarchyClassID,
                    traitID = Traits.SentToEsb,
                    traitValue = DateTime.Now.ToString()
                },
                new HierarchyClassTrait
                {
                    hierarchyClassID = updatedTestFinancial.hierarchyClassID,
                    traitID = Traits.SentToEsb,
                    traitValue = DateTime.Now.ToString()
                }
            });

            context.ItemTrait.Add(new ItemTrait
            {
                itemID = item.itemID,
                localeID = 1,
                traitID = Traits.ValidationDate,
                traitValue = DateTime.Now.ToString()
            });

            context.SaveChanges();

            // When.
            commandHandler.Execute(bulkImportCommand);

            // Then.
            var message = context.MessageQueueProduct.Single(p => p.ScanCode == scanCode1);

            Assert.AreEqual(MessageStatusTypes.Ready, message.MessageStatusId);
            Assert.AreEqual(initialBiodynamic, message.Biodynamic);
            Assert.AreEqual(initialCheeseRaw, message.CheeseRaw);
            Assert.AreEqual(initialPremiumBodyCare, message.PremiumBodyCare);
            Assert.AreEqual(initialVegetarian, message.Vegetarian);
            Assert.AreEqual(initialWholeTrade, message.WholeTrade);
            Assert.AreEqual(initialGrassFed, message.GrassFed);
            Assert.AreEqual(initialPastureRaised, message.PastureRaised);
            Assert.AreEqual(initialFreeRange, message.FreeRange);
            Assert.AreEqual(initialDryAged, message.DryAged);
            Assert.AreEqual(initialAirChilled, message.AirChilled);
            Assert.AreEqual(initialMadeInHouse, message.MadeInHouse);
            Assert.AreEqual(initialAnimalWelfareRating.Description, message.AnimalWelfareRating);
            Assert.AreEqual(initialEcoScaleRating.Description, message.EcoScaleRating);
            Assert.AreEqual(initialGlutenFreeAgency.hierarchyClassName, message.GlutenFreeAgency);
            Assert.AreEqual(initialMilkType.Description, message.CheeseMilkType);
            Assert.AreEqual(initialKosherAgency.hierarchyClassName, message.KosherAgency);
            Assert.AreEqual(initialNonGmoAgency.hierarchyClassName, message.NonGmoAgency);
            Assert.AreEqual(initialOrganicAgency.hierarchyClassName, message.OrganicAgency);
            Assert.AreEqual(initialSeafoodFreshOrFrozen.Description, message.SeafoodFreshOrFrozen);
            Assert.AreEqual(initialSeafoodCatchType.Description, message.SeafoodCatchType);
            Assert.AreEqual(initialVeganAgency.hierarchyClassName, message.VeganAgency);
        }

        

        [TestMethod]
        public void BulkImportItem_ItemIsValidatedButBrandHierarchyClassIsNotSentToEsb_ShouldGenerateStagedMessageForEsb()
        {
            // Given.
            var item = context.Item.Single(i => i.ScanCode.Any(sc => sc.scanCode == scanCode2));

            context.HierarchyClassTrait.AddRange(new List<HierarchyClassTrait> 
            {
                new HierarchyClassTrait
                {
                    hierarchyClassID = updatedTestMerchandise.hierarchyClassID,
                    traitID = Traits.MerchFinMapping,
                    traitValue = updatedTestFinancial.hierarchyClassName
                },
                new HierarchyClassTrait
                {
                    hierarchyClassID = updatedTestMerchandise.hierarchyClassID,
                    traitID = Traits.SentToEsb,
                    traitValue = DateTime.Now.ToString()
                },
                new HierarchyClassTrait
                {
                    hierarchyClassID = updatedTestBrand.hierarchyClassID,
                    traitID = Traits.SentToEsb,
                    traitValue = null
                },
                new HierarchyClassTrait
                {
                    hierarchyClassID = updatedTestFinancial.hierarchyClassID,
                    traitID = Traits.SentToEsb,
                    traitValue = DateTime.Now.ToString()
                }
            });

            context.ItemTrait.Add(new ItemTrait
            {
                itemID = item.itemID,
                localeID = 1,
                traitID = Traits.ValidationDate,
                traitValue = DateTime.Now.ToString()
            });

            context.SaveChanges();

            bulkImportCommand = new BulkImportCommand<BulkImportItemModel>
            {
                UserName = "IntegrationTestUsername",
                BulkImportData = new List<BulkImportItemModel>
                {
                    new BulkImportItemModel 
                    { 
                        ScanCode = scanCode1, 
                        BrandLineage = updatedTestBrand.hierarchyClassName,
                        BrandId = updatedTestBrand.hierarchyClassID.ToString(),
                        ProductDescription = "Bulk Import Test Description 1",
                        PosDescription = "Bulk Import Pos Test Description 1",
                        PackageUnit = "1",
                        FoodStampEligible = "0",
                        PosScaleTare = "10",
                        RetailSize = String.Empty,
                        RetailUom = String.Empty,
                        DeliverySystem = String.Empty,
                        MerchandiseLineage = updatedTestMerchandise.hierarchyClassName,
                        MerchandiseId = updatedTestMerchandise.hierarchyClassID.ToString(),
                        TaxLineage = updatedTestTax.hierarchyClassName,
                        TaxId = updatedTestTax.hierarchyClassID.ToString(),
                        BrowsingLineage = updatedTestBrowsing.hierarchyClassName,
                        BrowsingId = updatedTestBrowsing.hierarchyClassID.ToString(),
                        IsValidated = "1"
                    }
                }
            };

            // When.
            commandHandler.Execute(bulkImportCommand);

            // Then.
            var actualMessage = context.MessageQueueProduct.Single(p => p.ItemId == item1.itemID);

            Assert.AreEqual(MessageStatusTypes.Staged, actualMessage.MessageStatusId);
        }

        [TestMethod]
        public void BulkImportItem_ItemIsValidatedButMerchandiseHierarchyClassIsNotSentToEsb_ShouldGenerateStagedMessageForEsb()
        {
            // Given.
            var item = context.Item.Single(i => i.ScanCode.Any(sc => sc.scanCode == scanCode2));

            context.HierarchyClassTrait.AddRange(new List<HierarchyClassTrait> 
            {
                new HierarchyClassTrait
                {
                    hierarchyClassID = updatedTestMerchandise.hierarchyClassID,
                    traitID = Traits.MerchFinMapping,
                    traitValue = updatedTestFinancial.hierarchyClassName
                },
                new HierarchyClassTrait
                {
                    hierarchyClassID = updatedTestMerchandise.hierarchyClassID,
                    traitID = Traits.SentToEsb,
                    traitValue = null
                },
                new HierarchyClassTrait
                {
                    hierarchyClassID = updatedTestBrand.hierarchyClassID,
                    traitID = Traits.SentToEsb,
                    traitValue = DateTime.Now.ToString()
                },
                new HierarchyClassTrait
                {
                    hierarchyClassID = updatedTestFinancial.hierarchyClassID,
                    traitID = Traits.SentToEsb,
                    traitValue = DateTime.Now.ToString()
                }
            });

            context.ItemTrait.Add(new ItemTrait
            {
                itemID = item.itemID,
                localeID = 1,
                traitID = Traits.ValidationDate,
                traitValue = DateTime.Now.ToString()
            });

            context.SaveChanges();

            bulkImportCommand = new BulkImportCommand<BulkImportItemModel>
            {
                UserName = "IntegrationTestUsername",
                BulkImportData = new List<BulkImportItemModel>
                {
                    new BulkImportItemModel 
                    { 
                        ScanCode = scanCode1, 
                        BrandLineage = updatedTestBrand.hierarchyClassName,
                        BrandId = updatedTestBrand.hierarchyClassID.ToString(),
                        ProductDescription = "Bulk Import Test Description 1",
                        PosDescription = "Bulk Import Pos Test Description 1",
                        PackageUnit = "1",
                        FoodStampEligible = "0",
                        PosScaleTare = "10",
                        RetailSize = String.Empty,
                        RetailUom = String.Empty,
                        DeliverySystem = String.Empty,
                        MerchandiseLineage = updatedTestMerchandise.hierarchyClassName,
                        MerchandiseId = updatedTestMerchandise.hierarchyClassID.ToString(),
                        TaxLineage = updatedTestTax.hierarchyClassName,
                        TaxId = updatedTestTax.hierarchyClassID.ToString(),
                        BrowsingLineage = updatedTestBrowsing.hierarchyClassName,
                        BrowsingId = updatedTestBrowsing.hierarchyClassID.ToString(),
                        IsValidated = "1"
                    }
                }
            };

            // When.
            commandHandler.Execute(bulkImportCommand);

            // Then.
            var actualMessage = context.MessageQueueProduct.Single(p => p.ItemId == item1.itemID);

            Assert.AreEqual(MessageStatusTypes.Staged, actualMessage.MessageStatusId);
        }

        [TestMethod]
        public void BulkImportItem_ItemIsValidatedButFinancialHierarchyClassIsNotSentToEsb_ShouldGenerateStagedMessageForEsb()
        {
            // Given.
            var item = context.Item.Single(i => i.ScanCode.Any(sc => sc.scanCode == scanCode2));

            context.HierarchyClassTrait.AddRange(new List<HierarchyClassTrait> 
            {
                new HierarchyClassTrait
                {
                    hierarchyClassID = updatedTestMerchandise.hierarchyClassID,
                    traitID = Traits.MerchFinMapping,
                    traitValue = updatedTestFinancial.hierarchyClassName
                },
                new HierarchyClassTrait
                {
                    hierarchyClassID = updatedTestMerchandise.hierarchyClassID,
                    traitID = Traits.SentToEsb,
                    traitValue = DateTime.Now.ToString()
                },
                new HierarchyClassTrait
                {
                    hierarchyClassID = updatedTestBrand.hierarchyClassID,
                    traitID = Traits.SentToEsb,
                    traitValue = DateTime.Now.ToString()
                },
                new HierarchyClassTrait
                {
                    hierarchyClassID = updatedTestFinancial.hierarchyClassID,
                    traitID = Traits.SentToEsb,
                    traitValue = null
                }
            });

            context.ItemTrait.Add(new ItemTrait
            {
                itemID = item.itemID,
                localeID = 1,
                traitID = Traits.ValidationDate,
                traitValue = DateTime.Now.ToString()
            });

            context.SaveChanges();

            bulkImportCommand = new BulkImportCommand<BulkImportItemModel>
            {
                UserName = "IntegrationTestUsername",
                BulkImportData = new List<BulkImportItemModel>
                {
                    new BulkImportItemModel 
                    { 
                        ScanCode = scanCode1, 
                        BrandLineage = updatedTestBrand.hierarchyClassName,
                        BrandId = updatedTestBrand.hierarchyClassID.ToString(),
                        ProductDescription = "Bulk Import Test Description 1",
                        PosDescription = "Bulk Import Pos Test Description 1",
                        PackageUnit = "1",
                        FoodStampEligible = "0",
                        PosScaleTare = "10",
                        RetailSize = String.Empty,
                        RetailUom = String.Empty,
                        DeliverySystem = String.Empty,
                        MerchandiseLineage = updatedTestMerchandise.hierarchyClassName,
                        MerchandiseId = updatedTestMerchandise.hierarchyClassID.ToString(),
                        TaxLineage = updatedTestTax.hierarchyClassName,
                        TaxId = updatedTestTax.hierarchyClassID.ToString(),
                        BrowsingLineage = updatedTestBrowsing.hierarchyClassName,
                        BrowsingId = updatedTestBrowsing.hierarchyClassID.ToString(),
                        IsValidated = "1"
                    }
                }
            };

            // When.
            commandHandler.Execute(bulkImportCommand);

            // Then.
            var actualMessage = context.MessageQueueProduct.Single(p => p.ItemId == item1.itemID);

            Assert.AreEqual(MessageStatusTypes.Staged, actualMessage.MessageStatusId);
        }

        [TestMethod]
        public void BulkImportItem_ItemIsAssociatedToMerchandiseClassWithBottleDepositNonMerchandiseTrait_ShouldUpdateItemTypeToDeposit()
        {
            // Given.
            HierarchyClassTrait nonMerchandiseTrait = new HierarchyClassTrait
            {
                hierarchyClassID = updatedTestMerchandise.hierarchyClassID,
                traitID = Traits.NonMerchandise,
                traitValue = NonMerchandiseTraits.BottleDeposit,
            };

            this.context.HierarchyClassTrait.Add(nonMerchandiseTrait);
            this.context.SaveChanges();

            bulkImportCommand = new BulkImportCommand<BulkImportItemModel>
            {
                UserName = "IntegrationTestUsername",
                BulkImportData = new List<BulkImportItemModel>
                {
                    new BulkImportItemModel 
                    { 
                        ScanCode = scanCode1, 
                        BrandLineage = updatedTestBrand.hierarchyClassName,
                        BrandId = updatedTestBrand.hierarchyClassID.ToString(),
                        ProductDescription = "Bulk Import Test Description 1",
                        PosDescription = "Bulk Import Pos Test Description 1",
                        PackageUnit = "1",
                        FoodStampEligible = "0",
                        PosScaleTare = "10",
                        RetailSize = "12",
                        RetailUom = "CASE",
                        DeliverySystem = "CAP",
                        MerchandiseLineage = updatedTestMerchandise.hierarchyClassName,
                        MerchandiseId = updatedTestMerchandise.hierarchyClassID.ToString(),
                        TaxLineage = updatedTestTax.hierarchyClassName,
                        TaxId = updatedTestTax.hierarchyClassID.ToString(),
                        BrowsingLineage = updatedTestBrowsing.hierarchyClassName,
                        BrowsingId = updatedTestBrowsing.hierarchyClassID.ToString(),
                        IsValidated = "0"
                    }
                }
            };

            // When.
            commandHandler.Execute(bulkImportCommand);

            // Then.
            Item actualItem = context.Item.Single(i => i.itemID == item1.itemID);
            int expectedItemTypeId = this.context.ItemType.Single(it => it.itemTypeCode == ItemTypeCodes.Deposit).itemTypeID;
            RefreshItem(actualItem);

            Assert.AreEqual(expectedItemTypeId, actualItem.itemTypeID,
                String.Format("Expected itemTypeID {0} is not equal to actual itemTypeID {1}", expectedItemTypeId, actualItem.itemTypeID));
        }

        [TestMethod]
        public void BulkImportItem_ItemIsAssociatedToMerchandiseClassWithCrvNonMerchandiseTrait_ShouldUpdateItemTypeToDeposit()
        {
            // Given.
            HierarchyClassTrait nonMerchandiseTrait = new HierarchyClassTrait
            {
                hierarchyClassID = updatedTestMerchandise.hierarchyClassID,
                traitID = Traits.NonMerchandise,
                traitValue = NonMerchandiseTraits.Crv,
            };

            this.context.HierarchyClassTrait.Add(nonMerchandiseTrait);
            this.context.SaveChanges();

            bulkImportCommand = new BulkImportCommand<BulkImportItemModel>
            {
                UserName = "IntegrationTestUsername",
                BulkImportData = new List<BulkImportItemModel>
                {
                    new BulkImportItemModel 
                    { 
                        ScanCode = scanCode1, 
                        BrandLineage = updatedTestBrand.hierarchyClassName,
                        BrandId = updatedTestBrand.hierarchyClassID.ToString(),
                        ProductDescription = "Bulk Import Test Description 1",
                        PosDescription = "Bulk Import Pos Test Description 1",
                        PackageUnit = "1",
                        FoodStampEligible = "0",
                        PosScaleTare = "10",
                        RetailSize = "12",
                        RetailUom = "CASE",
                        DeliverySystem = "CAP",
                        MerchandiseLineage = updatedTestMerchandise.hierarchyClassName,
                        MerchandiseId = updatedTestMerchandise.hierarchyClassID.ToString(),
                        TaxLineage = updatedTestTax.hierarchyClassName,
                        TaxId = updatedTestTax.hierarchyClassID.ToString(),
                        BrowsingLineage = updatedTestBrowsing.hierarchyClassName,
                        BrowsingId = updatedTestBrowsing.hierarchyClassID.ToString(),
                        IsValidated = "0"
                    }
                }
            };

            // When.
            commandHandler.Execute(bulkImportCommand);

            // Then.
            Item actualItem = context.Item.Single(i => i.itemID == item1.itemID);
            int expectedItemTypeId = this.context.ItemType.Single(it => it.itemTypeCode == ItemTypeCodes.Deposit).itemTypeID;
            RefreshItem(actualItem);

            Assert.AreEqual(expectedItemTypeId, actualItem.itemTypeID,
                String.Format("Expected itemTypeID {0} is not equal to actual itemTypeID {1}", expectedItemTypeId, actualItem.itemTypeID));
        }

        [TestMethod]
        public void BulkImportItem_ItemIsAssociatedToMerchandiseClassWithBottleReturnNonMerchandiseTrait_ShouldUpdateItemTypeToReturn()
        {
            // Given.
            HierarchyClassTrait nonMerchandiseTrait = new HierarchyClassTrait
            {
                hierarchyClassID = updatedTestMerchandise.hierarchyClassID,
                traitID = Traits.NonMerchandise,
                traitValue = NonMerchandiseTraits.BottleReturn,
            };

            this.context.HierarchyClassTrait.Add(nonMerchandiseTrait);
            this.context.SaveChanges();

            bulkImportCommand = new BulkImportCommand<BulkImportItemModel>
            {
                UserName = "IntegrationTestUsername",
                BulkImportData = new List<BulkImportItemModel>
                {
                    new BulkImportItemModel 
                    { 
                        ScanCode = scanCode1, 
                        BrandLineage = updatedTestBrand.hierarchyClassName,
                        BrandId = updatedTestBrand.hierarchyClassID.ToString(),
                        ProductDescription = "Bulk Import Test Description 1",
                        PosDescription = "Bulk Import Pos Test Description 1",
                        PackageUnit = "1",
                        FoodStampEligible = "0",
                        PosScaleTare = "10",
                        RetailSize = "12",
                        RetailUom = "CASE",
                        DeliverySystem = "CAP",
                        MerchandiseLineage = updatedTestMerchandise.hierarchyClassName,
                        MerchandiseId = updatedTestMerchandise.hierarchyClassID.ToString(),
                        TaxLineage = updatedTestTax.hierarchyClassName,
                        TaxId = updatedTestTax.hierarchyClassID.ToString(),
                        BrowsingLineage = updatedTestBrowsing.hierarchyClassName,
                        BrowsingId = updatedTestBrowsing.hierarchyClassID.ToString(),
                        IsValidated = "0"
                    }
                }
            };

            // When.
            commandHandler.Execute(bulkImportCommand);

            // Then.
            Item actualItem = context.Item.Single(i => i.itemID == item1.itemID);
            int expectedItemTypeId = this.context.ItemType.Single(it => it.itemTypeCode == ItemTypeCodes.Return).itemTypeID;
            RefreshItem(actualItem);

            Assert.AreEqual(expectedItemTypeId, actualItem.itemTypeID,
                String.Format("Expected itemTypeID {0} is not equal to actual itemTypeID {1}", expectedItemTypeId, actualItem.itemTypeID));
        }

        [TestMethod]
        public void BulkImportItem_ItemIsAssociatedToMerchandiseClassWithCrvCreditNonMerchandiseTrait_ShouldUpdateItemTypeToReturn()
        {
            // Given.
            HierarchyClassTrait nonMerchandiseTrait = new HierarchyClassTrait
            {
                hierarchyClassID = updatedTestMerchandise.hierarchyClassID,
                traitID = Traits.NonMerchandise,
                traitValue = NonMerchandiseTraits.CrvCredit,
            };

            this.context.HierarchyClassTrait.Add(nonMerchandiseTrait);
            this.context.SaveChanges();

            bulkImportCommand = new BulkImportCommand<BulkImportItemModel>
            {
                UserName = "IntegrationTestUsername",
                BulkImportData = new List<BulkImportItemModel>
                {
                    new BulkImportItemModel 
                    { 
                        ScanCode = scanCode1, 
                        BrandLineage = updatedTestBrand.hierarchyClassName,
                        BrandId = updatedTestBrand.hierarchyClassID.ToString(),
                        ProductDescription = "Bulk Import Test Description 1",
                        PosDescription = "Bulk Import Pos Test Description 1",
                        PackageUnit = "1",
                        FoodStampEligible = "0",
                        PosScaleTare = "10",
                        RetailSize = "12",
                        RetailUom = "CASE",
                        DeliverySystem = "CAP",
                        MerchandiseLineage = updatedTestMerchandise.hierarchyClassName,
                        MerchandiseId = updatedTestMerchandise.hierarchyClassID.ToString(),
                        TaxLineage = updatedTestTax.hierarchyClassName,
                        TaxId = updatedTestTax.hierarchyClassID.ToString(),
                        BrowsingLineage = updatedTestBrowsing.hierarchyClassName,
                        BrowsingId = updatedTestBrowsing.hierarchyClassID.ToString(),
                        IsValidated = "0"
                    }
                }
            };

            // When.
            commandHandler.Execute(bulkImportCommand);

            // Then.
            Item actualItem = context.Item.Single(i => i.itemID == item1.itemID);
            int expectedItemTypeId = this.context.ItemType.Single(it => it.itemTypeCode == ItemTypeCodes.Return).itemTypeID;
            RefreshItem(actualItem);

            Assert.AreEqual(expectedItemTypeId, actualItem.itemTypeID,
                String.Format("Expected itemTypeID {0} is not equal to actual itemTypeID {1}", expectedItemTypeId, actualItem.itemTypeID));
        }

        [TestMethod]
        public void BulkImportItem_ItemIsAssociatedToMerchandiseClassWithBlackhawkNonMerchandiseTrait_ShouldUpdateItemTypeToFee()
        {
            // Given.
            HierarchyClassTrait nonMerchandiseTrait = new HierarchyClassTrait
            {
                hierarchyClassID = updatedTestMerchandise.hierarchyClassID,
                traitID = Traits.NonMerchandise,
                traitValue = NonMerchandiseTraits.BlackhawkFee,
            };

            this.context.HierarchyClassTrait.Add(nonMerchandiseTrait);
            this.context.SaveChanges();

            bulkImportCommand = new BulkImportCommand<BulkImportItemModel>
            {
                UserName = "IntegrationTestUsername",
                BulkImportData = new List<BulkImportItemModel>
                {
                    new BulkImportItemModel 
                    { 
                        ScanCode = scanCode1, 
                        BrandLineage = updatedTestBrand.hierarchyClassName,
                        BrandId = updatedTestBrand.hierarchyClassID.ToString(),
                        ProductDescription = "Bulk Import Test Description 1",
                        PosDescription = "Bulk Import Pos Test Description 1",
                        PackageUnit = "1",
                        FoodStampEligible = "0",
                        PosScaleTare = "10",
                        RetailSize = "12",
                        RetailUom = "CASE",
                        DeliverySystem = "CAP",
                        MerchandiseLineage = updatedTestMerchandise.hierarchyClassName,
                        MerchandiseId = updatedTestMerchandise.hierarchyClassID.ToString(),
                        TaxLineage = updatedTestTax.hierarchyClassName,
                        TaxId = updatedTestTax.hierarchyClassID.ToString(),
                        BrowsingLineage = updatedTestBrowsing.hierarchyClassName,
                        BrowsingId = updatedTestBrowsing.hierarchyClassID.ToString(),
                        IsValidated = "0",
                    }
                }
            };

            // When.
            commandHandler.Execute(bulkImportCommand);

            // Then.
            Item actualItem = context.Item.Single(i => i.itemID == item1.itemID);
            int expectedItemTypeId = this.context.ItemType.Single(it => it.itemTypeCode == ItemTypeCodes.Fee).itemTypeID;
            RefreshItem(actualItem);

            Assert.AreEqual(expectedItemTypeId, actualItem.itemTypeID,
                String.Format("Expected itemTypeID {0} is not equal to actual itemTypeID {1}", expectedItemTypeId, actualItem.itemTypeID));
        }

        [TestMethod]
        public void BulkImportItem_ItemIsAssociatedToMerchandiseClassWithNonRetailNonMerchandiseTrait_ShouldUpdateItemTypeToNonRetail()
        {
            // Given.
            HierarchyClassTrait nonMerchandiseTrait = new HierarchyClassTrait
            {
                hierarchyClassID = updatedTestMerchandise.hierarchyClassID,
                traitID = Traits.NonMerchandise,
                traitValue = NonMerchandiseTraits.LegacyPosOnly,
            };

            this.context.HierarchyClassTrait.Add(nonMerchandiseTrait);
            this.context.SaveChanges();

            bulkImportCommand = new BulkImportCommand<BulkImportItemModel>
            {
                UserName = "IntegrationTestUsername",
                BulkImportData = new List<BulkImportItemModel>
                {
                    new BulkImportItemModel 
                    { 
                        ScanCode = scanCode1, 
                        BrandLineage = updatedTestBrand.hierarchyClassName,
                        BrandId = updatedTestBrand.hierarchyClassID.ToString(),
                        ProductDescription = "Bulk Import Test Description 1",
                        PosDescription = "Bulk Import Pos Test Description 1",
                        PackageUnit = "1",
                        FoodStampEligible = "0",
                        PosScaleTare = "10",
                        RetailSize = "12",
                        RetailUom = "CASE",
                        DeliverySystem = "CAP",
                        MerchandiseLineage = updatedTestMerchandise.hierarchyClassName,
                        MerchandiseId = updatedTestMerchandise.hierarchyClassID.ToString(),
                        TaxLineage = updatedTestTax.hierarchyClassName,
                        TaxId = updatedTestTax.hierarchyClassID.ToString(),
                        BrowsingLineage = updatedTestBrowsing.hierarchyClassName,
                        BrowsingId = updatedTestBrowsing.hierarchyClassID.ToString(),
                        IsValidated = "0"
                    }
                }
            };

            // When.
            commandHandler.Execute(bulkImportCommand);

            // Then.
            Item actualItem = context.Item.Single(i => i.itemID == item1.itemID);
            int expectedItemTypeId = this.context.ItemType.Single(it => it.itemTypeCode == ItemTypeCodes.NonRetail).itemTypeID;
            RefreshItem(actualItem);

            Assert.AreEqual(expectedItemTypeId, actualItem.itemTypeID,
                String.Format("Expected itemTypeID {0} is not equal to actual itemTypeID {1}", expectedItemTypeId, actualItem.itemTypeID));
        }

        [TestMethod]
        public void BulkImportItem_RetailSaleItemIsBeingValidated_ItemShouldHaveTypeRetailSaleAfterUpdate()
        {
            // Given.
            var nonMerchandiseTrait = new HierarchyClassTrait
            {
                hierarchyClassID = updatedTestMerchandise.hierarchyClassID,
                traitID = Traits.NonMerchandise,
                traitValue = NonMerchandiseTraits.LegacyPosOnly,
            };

            this.context.HierarchyClassTrait.Add(nonMerchandiseTrait);
            this.context.SaveChanges();

            bulkImportCommand = new BulkImportCommand<BulkImportItemModel>
            {
                UserName = "IntegrationTestUsername",
                BulkImportData = new List<BulkImportItemModel>
                {
                    new BulkImportItemModel 
                    { 
                        ScanCode = scanCode1, 
                        BrandLineage = String.Empty,
                        BrandId = String.Empty,
                        ProductDescription = String.Empty,
                        PosDescription = String.Empty,
                        PackageUnit = String.Empty,
                        FoodStampEligible = String.Empty,
                        PosScaleTare = String.Empty,
                        RetailSize = String.Empty,
                        RetailUom = String.Empty,
                        DeliverySystem = String.Empty,
                        MerchandiseLineage = String.Empty,
                        MerchandiseId = String.Empty,
                        TaxLineage = String.Empty,
                        TaxId = String.Empty,
                        BrowsingLineage = String.Empty,
                        BrowsingId = String.Empty,
                        IsValidated = "1"
                    }
                }
            };

            // When.
            commandHandler.Execute(bulkImportCommand);

            // Then.
            Item actualItem = context.Item.Single(i => i.itemID == item1.itemID);
            int expectedItemTypeId = this.context.ItemType.Single(it => it.itemTypeCode == ItemTypeCodes.RetailSale).itemTypeID;
            RefreshItem(actualItem);

            Assert.AreEqual(expectedItemTypeId, actualItem.itemTypeID,
                String.Format("Expected itemTypeID {0} is not equal to actual itemTypeID {1}.", expectedItemTypeId, actualItem.itemTypeID));
        }

        [TestMethod]
        public void BulkImportItem_NonRetailItemIsBeingValidated_ItemShouldHaveTypeNonRetailAfterUpdate()
        {
            // Given.
            item1.itemTypeID = ItemTypes.NonRetail;
            context.SaveChanges();

            var nonMerchandiseTrait = new HierarchyClassTrait
            {
                hierarchyClassID = updatedTestMerchandise.hierarchyClassID,
                traitID = Traits.NonMerchandise,
                traitValue = NonMerchandiseTraits.LegacyPosOnly,
            };

            this.context.HierarchyClassTrait.Add(nonMerchandiseTrait);
            this.context.SaveChanges();

            bulkImportCommand = new BulkImportCommand<BulkImportItemModel>
            {
                UserName = "IntegrationTestUsername",
                BulkImportData = new List<BulkImportItemModel>
                {
                    new BulkImportItemModel 
                    { 
                        ScanCode = scanCode1, 
                        BrandLineage = String.Empty,
                        BrandId = String.Empty,
                        ProductDescription = String.Empty,
                        PosDescription = String.Empty,
                        PackageUnit = String.Empty,
                        FoodStampEligible = String.Empty,
                        PosScaleTare = String.Empty,
                        RetailSize = String.Empty,
                        RetailUom = String.Empty,
                        DeliverySystem = String.Empty,
                        MerchandiseLineage = String.Empty,
                        MerchandiseId = String.Empty,
                        TaxLineage = String.Empty,
                        TaxId = String.Empty,
                        BrowsingLineage = String.Empty,
                        BrowsingId = String.Empty,
                        IsValidated = "1"
                    }
                }
            };

            // When.
            commandHandler.Execute(bulkImportCommand);

            // Then.
            Item actualItem = context.Item.Single(i => i.itemID == item1.itemID);
            int expectedItemTypeId = this.context.ItemType.Single(it => it.itemTypeCode == ItemTypeCodes.NonRetail).itemTypeID;
            RefreshItem(actualItem);

            Assert.AreEqual(expectedItemTypeId, actualItem.itemTypeID,
                String.Format("Expected itemTypeID {0} is not equal to actual itemTypeID {1}.", expectedItemTypeId, actualItem.itemTypeID));
        }

        [TestMethod]
        public void BulkImportItem_ItemIsAssociatedToMerchandiseClassWithNonRetailNonMerchandiseTrait_ShouldNotGenerateProductMessage()
        {
            // Given.
            HierarchyClassTrait nonMerchandiseTrait = new HierarchyClassTrait
            {
                hierarchyClassID = updatedTestMerchandise.hierarchyClassID,
                traitID = Traits.NonMerchandise,
                traitValue = NonMerchandiseTraits.LegacyPosOnly,
            };

            this.context.HierarchyClassTrait.Add(nonMerchandiseTrait);
            this.context.SaveChanges();

            bulkImportCommand = new BulkImportCommand<BulkImportItemModel>
            {
                UserName = "IntegrationTestUsername",
                BulkImportData = new List<BulkImportItemModel>
                {
                    new BulkImportItemModel 
                    { 
                        ScanCode = scanCode1, 
                        BrandLineage = updatedTestBrand.hierarchyClassName,
                        BrandId = updatedTestBrand.hierarchyClassID.ToString(),
                        ProductDescription = "Bulk Import Test Description 1",
                        PosDescription = "Bulk Import Pos Test Description 1",
                        PackageUnit = "1",
                        FoodStampEligible = "0",
                        PosScaleTare = "10",
                        RetailSize = "12",
                        RetailUom = "CASE",
                        DeliverySystem = "CAP",
                        MerchandiseLineage = updatedTestMerchandise.hierarchyClassName,
                        MerchandiseId = updatedTestMerchandise.hierarchyClassID.ToString(),
                        TaxLineage = updatedTestTax.hierarchyClassName,
                        TaxId = updatedTestTax.hierarchyClassID.ToString(),
                        BrowsingLineage = updatedTestBrowsing.hierarchyClassName,
                        BrowsingId = updatedTestBrowsing.hierarchyClassID.ToString(),
                        IsValidated = "0"
                    }
                }
            };

            // When.
            commandHandler.Execute(bulkImportCommand);

            // Then.
            Item actualItem = context.Item.Single(i => i.itemID == item1.itemID);
            var actualMessage = context.MessageQueueProduct.Where(p => p.ItemId == actualItem.itemID);

            RefreshItem(actualItem);

            Assert.IsTrue(actualMessage.Count() == 0, "A Message was generated for a Legacy POS Only item when no message should have been created.");
        }

        [TestMethod]
        public void BulkImportItem_ItemIsAssociatedToMerchandiseClassWithCouponNonMerchandiseTrait_ShouldNotGenerateProductMessage()
        {
            // Given.
            HierarchyClassTrait nonMerchandiseTrait = new HierarchyClassTrait
            {
                hierarchyClassID = updatedTestMerchandise.hierarchyClassID,
                traitID = Traits.NonMerchandise,
                traitValue = NonMerchandiseTraits.Coupon,
            };

            this.context.HierarchyClassTrait.Add(nonMerchandiseTrait);
            this.context.SaveChanges();

            bulkImportCommand = new BulkImportCommand<BulkImportItemModel>
            {
                UserName = "IntegrationTestUsername",
                BulkImportData = new List<BulkImportItemModel>
                {
                    new BulkImportItemModel 
                    { 
                        ScanCode = scanCode1, 
                        BrandLineage = updatedTestBrand.hierarchyClassName,
                        BrandId = updatedTestBrand.hierarchyClassID.ToString(),
                        ProductDescription = "Bulk Import Test Description 1",
                        PosDescription = "Bulk Import Pos Test Description 1",
                        PackageUnit = "1",
                        FoodStampEligible = "0",
                        PosScaleTare = "10",
                        RetailSize = "12",
                        RetailUom = "CASE",
                        DeliverySystem = "CAP",
                        MerchandiseLineage = updatedTestMerchandise.hierarchyClassName,
                        MerchandiseId = updatedTestMerchandise.hierarchyClassID.ToString(),
                        TaxLineage = updatedTestTax.hierarchyClassName,
                        TaxId = updatedTestTax.hierarchyClassID.ToString(),
                        BrowsingLineage = updatedTestBrowsing.hierarchyClassName,
                        BrowsingId = updatedTestBrowsing.hierarchyClassID.ToString(),
                        IsValidated = "0"
                    }
                }
            };

            // When.
            commandHandler.Execute(bulkImportCommand);

            // Then.
            Item actualItem = context.Item.Single(i => i.itemID == item1.itemID);
            var actualMessage = context.MessageQueueProduct.Where(p => p.ItemId == actualItem.itemID);

            RefreshItem(actualItem);

            Assert.IsTrue(actualMessage.Count() == 0, "A Message was generated for a Coupon item when no message should have been created.");
        }

        [TestMethod]
        public void BulkImportItem_IsValidateSetToTrue_ItemIsValidated()
        {
            // Given.
            bulkImportCommand = CreateCommandWithThreeItems();

            foreach (var itemModel in bulkImportCommand.BulkImportData)
            {
                itemModel.IsValidated = "1";
            }

            // Remove any Validation Date trait currently associated to the item.
            foreach (var itemModel in bulkImportCommand.BulkImportData)
            {
                var item = context.ScanCode.SingleOrDefault(sc => sc.scanCode == itemModel.ScanCode).Item;

                if (item != null)
                {
                    var validationDateTrait = item.ItemTrait.SingleOrDefault(it => it.traitID == Traits.ValidationDate);
                    if (validationDateTrait != null)
                    {
                        context.ItemTrait.Remove(validationDateTrait);
                    }
                }

                context.SaveChanges();
            }

            // When.
            commandHandler.Execute(bulkImportCommand);

            // Then.
            foreach (var itemModel in bulkImportCommand.BulkImportData)
            {
                var item = context.ScanCode.SingleOrDefault(sc => sc.scanCode == itemModel.ScanCode).Item;

                RefreshItem(item);

                Assert.IsNotNull(item);
                Assert.IsNotNull(item.ItemTrait.SingleOrDefault(it => it.traitID == Traits.ValidationDate));
            }
        }

        [TestMethod]
        public void BulkImportItem_IsValidatedSetToTrueButItemIsAlreadyValidated_ItemsValidationDateKeepsOriginalValue()
        {
            // Given.
            string validationDateTraitValue = "Test Validation Date";
            bulkImportCommand = CreateCommandWithThreeItems();

            foreach (var itemModel in bulkImportCommand.BulkImportData)
            {
                itemModel.IsValidated = "1";
            }

            // Set Validation Date trait and trait values to test values.
            foreach (var itemModel in bulkImportCommand.BulkImportData)
            {
                var item = context.Item.SingleOrDefault(i => i.ScanCode.FirstOrDefault().scanCode == itemModel.ScanCode);
                if (item != null)
                {
                    var validationDateTrait = item.ItemTrait.SingleOrDefault(it => it.traitID == Traits.ValidationDate);
                    if (validationDateTrait != null)
                    {
                        validationDateTrait.traitValue = validationDateTraitValue;
                    }
                    else
                    {
                        item.ItemTrait.Add(new ItemTrait { itemID = item.itemID, localeID = Locales.WholeFoods, traitID = Traits.ValidationDate, traitValue = validationDateTraitValue });
                    }
                }
                context.SaveChanges();
            }

            // When.
            commandHandler.Execute(bulkImportCommand);

            // Then.
            foreach (var itemModel in bulkImportCommand.BulkImportData)
            {
                var scanCode = context.ScanCode.SingleOrDefault(sc => sc.scanCode == itemModel.ScanCode);
                Assert.IsNotNull(scanCode);

                var item = scanCode.Item;
                RefreshItem(item);

                var validationDateTrait = item.ItemTrait.SingleOrDefault(it => it.traitID == Traits.ValidationDate);
                Assert.IsNotNull(validationDateTrait);
                Assert.AreEqual(validationDateTraitValue, validationDateTrait.traitValue);
            }
        }

        [TestMethod]
        public void BulkImportItem_IsValidatedSetToFalse_ItemIsNotValidated()
        {
            // Given.
            bulkImportCommand = CreateCommandWithThreeItems();

            foreach (var itemModel in bulkImportCommand.BulkImportData)
            {
                itemModel.IsValidated = "0";
            }

            // Remove Validation Date trait.
            foreach (var itemModel in bulkImportCommand.BulkImportData)
            {
                var item = context.Item.SingleOrDefault(i => i.ScanCode.FirstOrDefault().scanCode == itemModel.ScanCode);

                if (item != null)
                {
                    var validationDateTrait = item.ItemTrait.SingleOrDefault(it => it.traitID == Traits.ValidationDate);
                    if (validationDateTrait != null)
                    {
                        context.ItemTrait.Remove(validationDateTrait);
                    }
                }

                context.SaveChanges();
            }

            // When.
            commandHandler.Execute(bulkImportCommand);

            // Then.
            foreach (var itemModel in bulkImportCommand.BulkImportData)
            {
                var scanCode = context.ScanCode.SingleOrDefault(sc => sc.scanCode == itemModel.ScanCode);
                Assert.IsNotNull(scanCode);

                var item = scanCode.Item;
                RefreshItem(item);

                var validationDateTrait = item.ItemTrait.SingleOrDefault(it => it.traitID == Traits.ValidationDate);
                Assert.IsNull(validationDateTrait);
            }
        }

        [TestMethod]
        public void BulkImportItem_IsValidatedSetToFalseButItemIsAlreadyValidated_ItemRemainsValidated()
        {
            // Given.
            var validationDateTraitValue = "Test Trait Value";
            bulkImportCommand = CreateCommandWithThreeItems();

            foreach (var itemModel in bulkImportCommand.BulkImportData)
            {
                itemModel.IsValidated = "0";
            }

            // Validate all items.
            foreach (var itemModel in bulkImportCommand.BulkImportData)
            {
                var item = context.Item.SingleOrDefault(i => i.ScanCode.FirstOrDefault().scanCode == itemModel.ScanCode);

                if (item != null)
                {
                    var validationDateTrait = item.ItemTrait.SingleOrDefault(it => it.traitID == Traits.ValidationDate);

                    if (validationDateTrait != null)
                    {
                        validationDateTrait.traitValue = validationDateTraitValue;
                    }
                    else
                    {
                        item.ItemTrait.Add(new ItemTrait { itemID = item.itemID, localeID = Locales.WholeFoods, traitID = Traits.ValidationDate, traitValue = validationDateTraitValue });
                    }
                }

                context.SaveChanges();
            }

            // When.
            commandHandler.Execute(bulkImportCommand);

            // Then.
            foreach (var itemModel in bulkImportCommand.BulkImportData)
            {
                var scanCode = context.ScanCode.SingleOrDefault(sc => sc.scanCode == itemModel.ScanCode);
                Assert.IsNotNull(scanCode);

                var item = scanCode.Item;
                RefreshItem(item);

                var validationDateTrait = item.ItemTrait.SingleOrDefault(it => it.traitID == Traits.ValidationDate);
                Assert.IsNotNull(validationDateTrait);
                Assert.AreEqual(validationDateTraitValue, validationDateTrait.traitValue);
            }
        }

        [TestMethod]
        public void BulkImportItem_ItemHasNullTraitValue_TraitValueShouldBeUpdatedByTheImport()
        {
            // Given.
            item1.ItemTrait.Single(it => it.traitID == Traits.PackageUnit).traitValue = null;
            context.SaveChanges();

            bulkImportCommand = new BulkImportCommand<BulkImportItemModel>
            {
                UserName = "IntegrationTestUsername",
                BulkImportData = new List<BulkImportItemModel>
                {
                    new BulkImportItemModel 
                    { 
                        ScanCode = scanCode1, 
                        BrandLineage = updatedTestBrand.hierarchyClassName,
                        BrandId = updatedTestBrand.hierarchyClassID.ToString(),
                        ProductDescription = "Bulk Import Test Description 1",
                        PosDescription = "Bulk Import Pos Test Description 1",
                        PackageUnit = "1",
                        FoodStampEligible = "0",
                        PosScaleTare = "10",
                        RetailSize = "12",
                        RetailUom = "CASE",
                        DeliverySystem = "CAP",
                        MerchandiseLineage = updatedTestMerchandise.hierarchyClassName,
                        MerchandiseId = updatedTestMerchandise.hierarchyClassID.ToString(),
                        TaxLineage = updatedTestTax.hierarchyClassName,
                        TaxId = updatedTestTax.hierarchyClassID.ToString(),
                        BrowsingLineage = updatedTestBrowsing.hierarchyClassName,
                        BrowsingId = updatedTestBrowsing.hierarchyClassID.ToString(),
                        IsValidated = "0"
                    }
                }
            };

            // When.
            commandHandler.Execute(bulkImportCommand);

            // Then.
            Item updatedItem = context.Item.Single(i => i.itemID == item1.itemID);
            RefreshItem(updatedItem);
            string packageUnit = updatedItem.ItemTrait.Single(it => it.traitID == Traits.PackageUnit).traitValue;

            Assert.IsNotNull(packageUnit);
            Assert.AreEqual(packageUnit, "1");
        }

        [TestMethod]
        public void BulkImportItem_WhenNotesColumnIsAlreadyPopulatedAndAddingDeliverySystem_ShouldNotSetDeliverySystemToNotesValue()
        {
            // Given.
            var deliverySystemTrait = item1.ItemTrait.First(it => it.localeID == Locales.WholeFoods && it.traitID == Traits.DeliverySystem);
            context.ItemTrait.Remove(deliverySystemTrait);

            string testNote = "Test note.";
            item1.ItemTrait.Add(new ItemTrait { localeID = Locales.WholeFoods, traitID = Traits.Notes, traitValue = testNote });
            context.SaveChanges();

            TestBulkImportItemModelBuilder bulkImportItem = new TestBulkImportItemModelBuilder()
                .Empty()
                .WithDeliverySystem(DeliverySystems.Descriptions.Cap)
                .WithScanCode(scanCode1)
                .WithNote(testNote);

            bulkImportCommand = new BulkImportCommand<BulkImportItemModel>
            {
                UserName = "IntegrationTestUsername",
                BulkImportData = new List<BulkImportItemModel> { bulkImportItem }
            };

            // When.
            commandHandler.Execute(bulkImportCommand);

            // Then.
            var updatedItem = context.Item
                .AsNoTracking()
                .Include(i => i.ItemTrait)
                .First(i => i.itemID == item1.itemID);
            var deliverySystem = updatedItem.ItemTrait.First(it => it.localeID == Locales.WholeFoods && it.traitID == Traits.DeliverySystem).traitValue;
            var notes = updatedItem.ItemTrait.First(it => it.localeID == Locales.WholeFoods && it.traitID == Traits.Notes).traitValue;
          
            Assert.AreEqual(DeliverySystems.Descriptions.Cap, deliverySystem);
            Assert.AreEqual(testNote, notes);
        }
        
        private BulkImportCommand<BulkImportItemModel> CreateCommandWithThreeItems()
        {
            return new BulkImportCommand<BulkImportItemModel>
            {
                UserName = "IntegrationTestUsername",
                BulkImportData = new List<BulkImportItemModel>
                {
                    new BulkImportItemModel 
                    { 
                        ScanCode = scanCode1, 
                        BrandLineage = "Bulk Import Test Brand",
                        BrandId = String.Empty,
                        ProductDescription = "Bulk Import Test Description 1",
                        PosDescription = "Bulk Import POS Test Description 1",
                        PackageUnit = "1",
                        FoodStampEligible = "0",
                        PosScaleTare = "10",
                        RetailSize = "",
                        RetailUom = "",
                        DeliverySystem = "",
                        MerchandiseLineage = String.Empty,
                        MerchandiseId = String.Empty,
                        TaxLineage = String.Empty,
                        TaxId = String.Empty,
                        BrowsingLineage = String.Empty,
                        BrowsingId = String.Empty,
                        IsValidated = "0"
                    },
                    new BulkImportItemModel
                    {
                        ScanCode = scanCode2, 
                        BrandLineage = "Bulk Import Test Brand",
                        BrandId = String.Empty,
                        ProductDescription = "Bulk Import Test Description 2",
                        PosDescription = "Bulk Import POS Test Description 2",
                        PackageUnit = "1",
                        FoodStampEligible = "0",
                        PosScaleTare = "10",
                        RetailSize = "",
                        RetailUom = "",
                        DeliverySystem = "",
                        MerchandiseLineage = String.Empty,
                        MerchandiseId = String.Empty,
                        TaxLineage = String.Empty,
                        TaxId = String.Empty,
                        BrowsingLineage = String.Empty,
                        BrowsingId = String.Empty,
                        IsValidated = "0"
                    },
                    new BulkImportItemModel
                    {
                        ScanCode = scanCode3,
                        BrandLineage = "Bulk Import Test Brand",
                        BrandId = String.Empty,
                        ProductDescription = "Bulk Import Test Description 3",
                        PosDescription = "Bulk Import POS Test Description 3",
                        PackageUnit = "1",
                        FoodStampEligible = "0",
                        PosScaleTare = "10",
                        RetailSize = "",
                        RetailUom = "",
                        DeliverySystem = "",
                        MerchandiseLineage = String.Empty,
                        MerchandiseId = String.Empty,
                        TaxLineage = String.Empty,
                        TaxId = String.Empty,
                        BrowsingLineage = String.Empty,
                        BrowsingId = String.Empty,
                        IsValidated = "0"
                    }
                }
            };
        }

        private ObjectContext GetObjectContext(DbContext context)
        {
            var objectContext = ((IObjectContextAdapter)context).ObjectContext;
            return objectContext;
        }

        private void RefreshItem(Item item)
        {
            var objContext = ((IObjectContextAdapter)context).ObjectContext;
            objContext.Refresh(RefreshMode.StoreWins, item);
            objContext.LoadProperty(item, i => i.ItemTrait, MergeOption.OverwriteChanges);
            objContext.LoadProperty(item, i => i.ItemHierarchyClass, MergeOption.OverwriteChanges);
            objContext.LoadProperty(item, i => i.ItemSignAttribute, MergeOption.OverwriteChanges);

            item.ItemTrait = context.Entry(item).Collection(i => i.ItemTrait).Query().ToList();
            item.ItemHierarchyClass = context.Entry(item).Collection(i => i.ItemHierarchyClass).Query().ToList();
            item.ItemSignAttribute = context.Entry(item).Collection(i => i.ItemSignAttribute).Query().ToList();
        }
    }
}
