using Icon.Framework;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Icon.Web.Tests.Integration.Queries
{
    [TestClass] [Ignore]
    public class GetPluRemappingsQueryTests
    {
        private IconContext context;
        private DbContextTransaction transaction;
        private Item item1;
        private Item item2;
        private Item item3;
        private Item item4;
        private int itemId1;
        private int itemId2;
        private int itemId3;
        private int itemId4;
        private PLUMap pluMap1;
        private PLUMap pluMap2;
        private List<Item> items;
        private List<ScanCode> scanCodes;
        private GetPluRemappingsQuery remappingQuery;
        
        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            context.Database.Connection.Open();

            items = SetUpFakePlu();
            
            transaction = context.Database.BeginTransaction();
            context.Item.Add(item1);
            context.Item.Add(item2);
            context.Item.Add(item3);
            context.Item.Add(item4);
            context.SaveChanges();
            itemId1 = item1.ItemId;
            itemId2 = item2.ItemId;
            itemId3 = item3.ItemId;
            itemId4 = item4.ItemId;
            
            scanCodes = new List<ScanCode>
            {
                new ScanCode { itemID = itemId1, scanCode = "22999941000", scanCodeTypeID = 2 },
                new ScanCode { itemID = itemId2, scanCode = "22999942000", scanCodeTypeID = 2 },
                new ScanCode { itemID = itemId3, scanCode = "22999943000", scanCodeTypeID = 2 },
                new ScanCode { itemID = itemId4, scanCode = "22999944000", scanCodeTypeID = 2 }
            };

            pluMap1 = new PLUMap
            {
                itemID = itemId1,
                flPLU = "29999999100",
                maPLU = "29999999200",
                mwPLU = "29876599100",
                naPLU = "23433399100",
                ncPLU = "24544499100",
                nePLU = "27777799100",
                pnPLU = "28888899100",
                rmPLU = "24242499100",
                soPLU = "21212199100",
                spPLU = "27877799100",
                swPLU = "27777799100",
                ukPLU = "23362499100"
            };

            pluMap2 = new PLUMap
            {
                itemID = itemId2,
                flPLU = "23216599100",
                maPLU = "29562199100",
                mwPLU = "29876599100",
                naPLU = "23433399100",
                ncPLU = "25621399100",
                nePLU = "24544499100",
                pnPLU = "27777799100",
                rmPLU = "24242499100",
                soPLU = "25555699100",
                spPLU = "29999899100",
                swPLU = "27777799100",
                ukPLU = "21111199100"
            };

            context.PLUMap.Add(pluMap1);
            context.PLUMap.Add(pluMap2);
            context.ScanCode.AddRange(scanCodes);
            context.SaveChanges();

            remappingQuery = new GetPluRemappingsQuery(this.context);
        }

        [TestCleanup]
        public void DeleteTestData()
        {
            if (transaction != null)
            {
                transaction.Rollback();
                transaction.Dispose();
                transaction = null;
            }

            context.Dispose();
            remappingQuery = null;
        }

        [TestMethod]
        public void PluRemappingQuery_DuplicateRegionalPluFoundInDatabase_ShouldReturnDuplicateMapping()
        {
            // Given.
            List<BulkImportPluModel> importedItems = new List<BulkImportPluModel>();

            importedItems.Add(new BulkImportPluModel
            {
                NationalPlu = scanCodes[1].scanCode,
                flPLU = pluMap1.flPLU,
                maPLU = String.Empty,
                mwPLU = String.Empty,
                naPLU = String.Empty,
                ncPLU = String.Empty,
                nePLU = String.Empty,
                pnPLU = String.Empty,
                rmPLU = String.Empty,
                soPLU = String.Empty,
                spPLU = String.Empty,
                swPLU = String.Empty,
                ukPLU = String.Empty
            });

            GetPluRemappingsParameters parameters = new GetPluRemappingsParameters
            {
                ImportedItems = importedItems
            };

            // When.
            var remappings = remappingQuery.Search(parameters);

            // Then.
            int expectedCount = 1;
            int actualCount = remappings.Count();

            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void PluRemappingQuery_DuplicateRegionalPluFoundInDatabase_ShouldReturnPluDescriptions()
        {
            // Given.
            List<BulkImportPluModel> importedItems = new List<BulkImportPluModel>();

            importedItems.Add(new BulkImportPluModel
            {
                NationalPlu = scanCodes[1].scanCode,
                flPLU = pluMap1.flPLU,
                maPLU = String.Empty,
                mwPLU = String.Empty,
                naPLU = String.Empty,
                ncPLU = String.Empty,
                nePLU = String.Empty,
                pnPLU = String.Empty,
                rmPLU = String.Empty,
                soPLU = String.Empty,
                spPLU = String.Empty,
                swPLU = String.Empty,
                ukPLU = String.Empty
            });

            GetPluRemappingsParameters parameters = new GetPluRemappingsParameters
            {
                ImportedItems = importedItems
            };

            // When.
            var remappings = remappingQuery.Search(parameters);

            // Then.
            string currentPluDescripton = remappings[0].CurrentNationalPluDescription;
            string newPluDescription = remappings[0].NewNationalPluDescription;

            Assert.AreEqual("PLU Test1", currentPluDescripton);
            Assert.AreEqual("PLU Test2", newPluDescription);
        }

        [TestMethod]
        public void PluRemappingQuery_NewNationalPluEqualsCurrentNationalPlu_RemapShouldBeDiscarded()
        {
            // Given.
            List<BulkImportPluModel> importedItems = new List<BulkImportPluModel>();

            importedItems.Add(new BulkImportPluModel
            {
                NationalPlu = scanCodes[0].scanCode,
                flPLU = pluMap1.flPLU,
                maPLU = String.Empty,
                mwPLU = String.Empty,
                naPLU = String.Empty,
                ncPLU = String.Empty,
                nePLU = String.Empty,
                pnPLU = String.Empty,
                rmPLU = String.Empty,
                soPLU = String.Empty,
                spPLU = String.Empty,
                swPLU = String.Empty,
                ukPLU = String.Empty
            });

            GetPluRemappingsParameters parameters = new GetPluRemappingsParameters
            {
                ImportedItems = importedItems
            };

            // When.
            var remappings = remappingQuery.Search(parameters);

            // Then.
            Assert.AreEqual(0, remappings.Count);
        }

        [TestMethod]
        public void PluRemappingQuery_MultipleRemappingsForOneRegion_NewNationalPluAssignmentsShouldBeMarkedAsRemaps()
        {
            // Given.
            List<BulkImportPluModel> importedItems = new List<BulkImportPluModel>();

            importedItems.Add(new BulkImportPluModel
            {
                NationalPlu = "22999943000",
                flPLU = pluMap1.flPLU,
                maPLU = String.Empty,
                mwPLU = String.Empty,
                naPLU = String.Empty,
                ncPLU = String.Empty,
                nePLU = String.Empty,
                pnPLU = String.Empty,
                rmPLU = String.Empty,
                soPLU = String.Empty,
                spPLU = String.Empty,
                swPLU = String.Empty,
                ukPLU = String.Empty
            });

            importedItems.Add(new BulkImportPluModel
            {
                NationalPlu = "22999944000",
                flPLU = pluMap2.flPLU,
                maPLU = String.Empty,
                mwPLU = String.Empty,
                naPLU = String.Empty,
                ncPLU = String.Empty,
                nePLU = String.Empty,
                pnPLU = String.Empty,
                rmPLU = String.Empty,
                soPLU = String.Empty,
                spPLU = String.Empty,
                swPLU = String.Empty,
                ukPLU = String.Empty
            });

            GetPluRemappingsParameters parameters = new GetPluRemappingsParameters
            {
                ImportedItems = importedItems
            };

            // When.
            var remappings = remappingQuery.Search(parameters);

            // Then.
            Assert.AreEqual(2, remappings.Count);
        }

        [TestMethod]
        public void PluRemappingQuery_MultipleRemappingsForOneRegion_SameNationalPluAssignmentsShouldNotBeMarkedAsRemaps()
        {
            // Given.
            List<BulkImportPluModel> importedItems = new List<BulkImportPluModel>();

            importedItems.Add(new BulkImportPluModel
            {
                NationalPlu = "22999941000",
                flPLU = pluMap1.flPLU,
                maPLU = String.Empty,
                mwPLU = String.Empty,
                naPLU = String.Empty,
                ncPLU = String.Empty,
                nePLU = String.Empty,
                pnPLU = String.Empty,
                rmPLU = String.Empty,
                soPLU = String.Empty,
                spPLU = String.Empty,
                swPLU = String.Empty,
                ukPLU = String.Empty
            });

            importedItems.Add(new BulkImportPluModel
            {
                NationalPlu = "22999942000",
                flPLU = pluMap2.flPLU,
                maPLU = String.Empty,
                mwPLU = String.Empty,
                naPLU = String.Empty,
                ncPLU = String.Empty,
                nePLU = String.Empty,
                pnPLU = String.Empty,
                rmPLU = String.Empty,
                soPLU = String.Empty,
                spPLU = String.Empty,
                swPLU = String.Empty,
                ukPLU = String.Empty
            });

            GetPluRemappingsParameters parameters = new GetPluRemappingsParameters
            {
                ImportedItems = importedItems
            };

            // When.
            var remappings = remappingQuery.Search(parameters);

            // Then.
            Assert.AreEqual(0, remappings.Count);
        }

        private List<Item> SetUpFakePlu()
        {
            List<Item> items = new List<Item>();

            int productDescriptionTraitId = context.Trait.Single(t => t.traitCode == TraitCodes.ProductDescription).traitID;

            item1 = new Item { ItemTypeId = 1 };
            item1.ItemTrait = new List<ItemTrait>();
            item1.ItemTrait.Add(new ItemTrait { itemID = 1, traitValue = "PLU Test1", traitID = productDescriptionTraitId, localeID = 1 });
            items.Add(item1);

            item2 = new Item { ItemTypeId = 1 };
            item2.ItemTrait = new List<ItemTrait>();
            item2.ItemTrait.Add(new ItemTrait { itemID = 1, traitValue = "PLU Test2", traitID = productDescriptionTraitId, localeID = 1 });
            items.Add(item2);

            item3 = new Item { ItemTypeId = 1 };
            item3.ItemTrait = new List<ItemTrait>();
            item3.ItemTrait.Add(new ItemTrait { itemID = 1, traitValue = "PLU Test3", traitID = productDescriptionTraitId, localeID = 1 });
            items.Add(item3);

            item4 = new Item { ItemTypeId = 1 };
            item4.ItemTrait = new List<ItemTrait>();
            item4.ItemTrait.Add(new ItemTrait { itemID = 1, traitValue = "PLU Test4", traitID = productDescriptionTraitId, localeID = 1 });
            items.Add(item4);

            return items;
        }
    }
}