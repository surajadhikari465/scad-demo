using GlobalEventController.Common;
using GlobalEventController.DataAccess.BulkCommands;
using Irma.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;

namespace GlobalEventController.Tests.DataAccess.BulkCommandTests
{
    [TestClass]
    public class BulkAddUpdateLastChangeCommandHandlerTests
    {
        private IrmaContext context;
        private BulkAddUpdateLastChangeCommand command;
        private BulkAddUpdateLastChangeCommandHandler handler;
        private List<IconItemLastChangeModel> lastChangedItems;
        private DbContextTransaction transaction;

        [TestInitialize]
        public void InitializeData()
        {
            this.context = new IrmaContext(); // this is the FL ItemCatalog_Test database
            this.command = new BulkAddUpdateLastChangeCommand();
            this.handler = new BulkAddUpdateLastChangeCommandHandler(this.context);
            this.lastChangedItems = new List<IconItemLastChangeModel>();

            this.transaction = this.context.Database.BeginTransaction();
        }

        [TestCleanup]
        public void CleanupData()
        {
            if (transaction != null)
            {
                this.transaction.Rollback();
            }
        }

        [TestMethod]
        public void BulkAddUpdateLastChange_ScanCodeDoesNotAlreadyHaveARow_LastChangeRecordAddedWithExpectedValues()
        {
            // Given
            IEnumerable<string> existingLastChangeIdentifier = this.context.IconItemLastChange.Select(lc => lc.Identifier);
            string identifier = this.context.ItemIdentifier.Where(ii => ii.Deleted_Identifier == 0 && ii.Default_Identifier == 1 && ii.Item.Retail_Sale == true)
                .Select(ii => ii.Identifier)
                .Except(existingLastChangeIdentifier)
                .First();

            var itemKey = this.context.ItemIdentifier.First(ii => ii.Identifier == identifier).Item_Key;
            var itemChangeHistory = this.context.Database.ExecuteSqlCommand(
                "insert into itemchangehistory(item_key, insert_date) values(@item_key, @insert_date)",
                new SqlParameter("@item_key", itemKey),
                new SqlParameter("@insert_date", DateTime.Now));

            TaxClass tax = this.context.TaxClass.First(tc => !tc.TaxClassDesc.ToLower().Contains("do not use"));
            ValidatedBrand brand = this.context.ValidatedBrand.First();

            IconItemLastChangeModel expectedItem = new TestIconItemLastChangeModelBuilder()
                .WithScanCode(identifier).WithTaxClass(tax.TaxClassDesc).WithBrandId(brand.IconBrandId).WithBrandName(brand.ItemBrand.Brand_Name).WithAreNutriFactChanged(true).Build();
            
            this.lastChangedItems.Add(expectedItem);
            this.command.IconLastChangedItems = this.lastChangedItems;

            if (this.context.IconItemLastChange.Any(lc => lc.Identifier == identifier))
            {
                Assert.Fail(String.Format("There is already a Last Change row for Identifier {0}", identifier));
            }

            // When
            this.handler.Handle(this.command);

            // Then
            IconItemLastChange lastChange = this.context.IconItemLastChange.Single(lc => lc.Identifier == identifier);

            Assert.AreEqual(expectedItem.ScanCode, lastChange.Identifier, "The Last Change Identifier does not match expected");
            Assert.AreEqual(null, lastChange.Subteam_No, "The Last Change SubTeam does not match expected");
            Assert.AreEqual(brand.IrmaBrandId, lastChange.Brand_ID, "The Last Change Brand_ID does not match expected");
            Assert.AreEqual(expectedItem.ProductDescription, lastChange.Item_Description, "The Last Change Item Description does not match expected");
            Assert.AreEqual(expectedItem.PosDescription, lastChange.POS_Description, "The Last Change POS Description does not match expected");
            Assert.AreEqual(Convert.ToDecimal(expectedItem.PackageUnit), lastChange.Package_Desc1, "The Last Change Package_Desc1 does not match expected");
            Assert.AreEqual(expectedItem.FoodStampEligible == "1", lastChange.Food_Stamps, "The Last Change FoodStamps does not match expected");
            Assert.AreEqual(Convert.ToDecimal(expectedItem.Tare), lastChange.ScaleTare, "The Last Change ScaleTare does not match expected");
            Assert.AreEqual(tax.TaxClassID, lastChange.TaxClassID, "The Last Change TaxClassID does not match expected");
            Assert.IsNotNull(lastChange.InsertDate, "The Last Change InsertDate is null");
            Assert.IsTrue(lastChange.AreNutriFactsChanged.Value);
        }

        [TestMethod]
        public void BulkAddUpdateLastChange_ScanCodeAlreadyHasARowInLastChangeTable_LastChangeRecordIsUpdated()
        {
            // Given
            IEnumerable<string> existingLastChangeIdentifier = this.context.IconItemLastChange.Select(lc => lc.Identifier);
            if (existingLastChangeIdentifier.Count() == 0)
            {
                // TODO: Add Last Change if there isn't one
                Assert.Fail("There is no existing IconItemLastChange rows to test with.");
            }

            string identifier = this.context.ItemIdentifier.Where(ii => ii.Deleted_Identifier == 0 && ii.Default_Identifier == 1 && ii.Item.Retail_Sale == true)
                .Select(ii => ii.Identifier)
                .Intersect(existingLastChangeIdentifier)
                .First();

            TaxClass tax = this.context.TaxClass.First(tc => !tc.TaxClassDesc.ToLower().Contains("do not use"));
            ValidatedBrand brand = this.context.ValidatedBrand.First();

            IconItemLastChangeModel expectedItem = new TestIconItemLastChangeModelBuilder()
                .WithScanCode(identifier).WithTaxClass(tax.TaxClassDesc).WithBrandId(brand.IconBrandId).WithBrandName(brand.ItemBrand.Brand_Name).WithAreNutriFactChanged(true).Build();

            this.lastChangedItems.Add(expectedItem);
            this.command.IconLastChangedItems = this.lastChangedItems;

            if (this.context.IconItemLastChange.Count(lc => lc.Identifier == identifier) == 0)
            {
                Assert.Fail(String.Format("There is not a Last Change row for Identifier {0}", identifier));
            }

            IconItemLastChange actualLastChange = this.context.IconItemLastChange.Single(lc => lc.Identifier == identifier);
            DbEntityEntry entry = this.context.Entry(actualLastChange);

            // When
            this.handler.Handle(this.command);

            // Then
            entry.Reload(); // reload to get values from DB
            Assert.AreEqual(expectedItem.ProductDescription, actualLastChange.Item_Description, "The LastChange Item_Description does not match expected value");
            Assert.AreEqual(expectedItem.PosDescription, actualLastChange.POS_Description, "The LastChange POS_Description does not match expected value");
            Assert.AreEqual(Convert.ToDecimal(expectedItem.PackageUnit), actualLastChange.Package_Desc1, "The LastChange Package_Desc1 does not match expected value");
            Assert.AreEqual(expectedItem.FoodStampEligible == "1", actualLastChange.Food_Stamps, "The LastChange Food_Stamps does not match expected value");
            Assert.AreEqual(Convert.ToDecimal(expectedItem.Tare), actualLastChange.ScaleTare, "The LastChange ScaleTare does not match expected value");
            Assert.AreEqual(tax.TaxClassID, actualLastChange.TaxClassID, "The LastChange TaxClassID does not match expected value");
            Assert.AreEqual(brand.IrmaBrandId, actualLastChange.Brand_ID, "The LastChange Brand_ID does not match expected value");
            Assert.IsTrue(actualLastChange.AreNutriFactsChanged.Value);
        }

        [TestMethod]
        public void BulkAddUpdateLastChange_ValidatedItemIsNotDefaultIdentifier_NoRowAddedToIconItemLastChange()
        {
            // Given
            IEnumerable<string> existingLastChangeIdentifier = this.context.IconItemLastChange.Select(lc => lc.Identifier);
            string identifier = this.context.ItemIdentifier.Where(ii => ii.Deleted_Identifier == 0 && ii.Default_Identifier == 0 && ii.Item.Retail_Sale == true)
                .Select(ii => ii.Identifier)
                .Except(existingLastChangeIdentifier)
                .First();

            TaxClass tax = this.context.TaxClass.First(tc => !tc.TaxClassDesc.ToLower().Contains("do not use"));
            ValidatedBrand brand = this.context.ValidatedBrand.First();
            IconItemLastChangeModel expectedItem = new TestIconItemLastChangeModelBuilder()
                .WithScanCode(identifier).WithTaxClass(tax.TaxClassDesc).WithBrandId(brand.IconBrandId).WithBrandName(brand.ItemBrand.Brand_Name).Build();

            this.lastChangedItems.Add(expectedItem);
            this.command.IconLastChangedItems = this.lastChangedItems;

            // When
            this.handler.Handle(this.command);

            // Then
            Assert.IsNull(this.context.IconItemLastChange.FirstOrDefault(lc => lc.Identifier == identifier));
        }

        [TestMethod]
        public void BulkAddUpdateLastChange_ValidatedItemIsDefaultIdentifierButBrandIsNotValidated_NoRowAddedToIconItemLastChange()
        {
            // Given
            IEnumerable<string> existingLastChangeIdentifier = this.context.IconItemLastChange.Select(lc => lc.Identifier);
            string identifier = this.context.ItemIdentifier.Where(ii => ii.Deleted_Identifier == 0 && ii.Default_Identifier == 1 && ii.Item.Retail_Sale == true)
                .Select(ii => ii.Identifier)
                .Except(existingLastChangeIdentifier)
                .First();

            TaxClass tax = this.context.TaxClass.First(tc => !tc.TaxClassDesc.ToLower().Contains("do not use"));
            IconItemLastChangeModel expectedItem = new TestIconItemLastChangeModelBuilder()
                .WithScanCode(identifier).WithTaxClass(tax.TaxClassDesc).WithBrandId(10).WithBrandName("qwertyytrewq").Build();

            this.lastChangedItems.Add(expectedItem);
            this.command.IconLastChangedItems = this.lastChangedItems;

            // When
            this.handler.Handle(this.command);

            // Then
            Assert.IsNull(this.context.IconItemLastChange.FirstOrDefault(lc => lc.Identifier == identifier));
        }


        [TestMethod]
        public void BulkAddUpdateLastChange_ValidatedItemIsDefaultIdentifierButTaxClassIsNotInIrma_NoRowAddedToIconItemLastChange()
        {
            // Given
            IEnumerable<string> existingLastChangeIdentifier = this.context.IconItemLastChange.Select(lc => lc.Identifier);
            string identifier = this.context.ItemIdentifier.Where(ii => ii.Deleted_Identifier == 0 && ii.Default_Identifier == 1 && ii.Item.Retail_Sale == true)
                .Select(ii => ii.Identifier)
                .Except(existingLastChangeIdentifier)
                .First();

            ValidatedBrand brand = this.context.ValidatedBrand.First();
            IconItemLastChangeModel expectedItem = new TestIconItemLastChangeModelBuilder()
                .WithScanCode(identifier).WithTaxClass("9876976 Tax Not In Irma").WithBrandId(brand.IconBrandId).WithBrandName(brand.ItemBrand.Brand_Name).Build();

            this.lastChangedItems.Add(expectedItem);
            this.command.IconLastChangedItems = this.lastChangedItems;

            // When
            this.handler.Handle(this.command);

            // Then
            Assert.IsNull(this.context.IconItemLastChange.FirstOrDefault(lc => lc.Identifier == identifier));
        }

        [TestMethod]
        public void BulkAddUpdateLastChange_ScanCodeAlreadyHasARowInLastChangeAndBrandIsNotValidated_LastChangeRowsIsNotUpdated()
        {
            // Given
            IEnumerable<string> existingLastChangeIdentifier = this.context.IconItemLastChange.Select(lc => lc.Identifier);
            string identifier = this.context.ItemIdentifier.Where(ii => ii.Deleted_Identifier == 0 && ii.Default_Identifier == 1 && ii.Item.Retail_Sale == true)
                .Select(ii => ii.Identifier)
                .Intersect(existingLastChangeIdentifier)
                .First();

            TaxClass tax = this.context.TaxClass.First(tc => !tc.TaxClassDesc.ToLower().Contains("do not use"));
            IconItemLastChangeModel expectedItem = new TestIconItemLastChangeModelBuilder()
                .WithScanCode(identifier).WithTaxClass(tax.TaxClassDesc).WithBrandId(10).WithBrandName("qwertyytrewq").Build();

            this.lastChangedItems.Add(expectedItem);
            this.command.IconLastChangedItems = this.lastChangedItems;

            IconItemLastChange actualLastChange = this.context.IconItemLastChange.Single(lc => lc.Identifier == identifier);
            DbEntityEntry<IconItemLastChange> entry = this.context.Entry(actualLastChange);

            // When
            this.handler.Handle(this.command);

            // Then
            entry.Reload();
            Assert.AreEqual(actualLastChange.Brand_ID, entry.Property(e => e.Brand_ID).CurrentValue, "Brand_ID value should not have changed.");
            Assert.AreEqual(actualLastChange.Food_Stamps, entry.Property(e => e.Food_Stamps).CurrentValue, "Food_Stamps value should not have changed.");
            Assert.AreEqual(actualLastChange.Item_Description, entry.Property(e => e.Item_Description).CurrentValue, "Item_Description value should not have changed.");
            Assert.AreEqual(actualLastChange.POS_Description, entry.Property(e => e.POS_Description).CurrentValue, "POS_Description value should not have changed.");
            Assert.AreEqual(actualLastChange.Package_Desc1, entry.Property(e => e.Package_Desc1).CurrentValue, "Package_Desc1 value should not have changed.");
            Assert.AreEqual(actualLastChange.ScaleTare, entry.Property(e => e.ScaleTare).CurrentValue, "ScaleTare value should not have changed.");
            Assert.AreEqual(actualLastChange.Subteam_No, entry.Property(e => e.Subteam_No).CurrentValue, "Subteam_No value should not have changed.");
            Assert.AreEqual(actualLastChange.TaxClassID, entry.Property(e => e.TaxClassID).CurrentValue, "TaxClass_ID value should not have changed.");
        }

        [TestMethod]
        public void BulkAddUpdateLastChange_ScanCodeAlreadyHasARowInLastChangeAndTaxIsNotInIRMA_LastChangeRowsIsNotUpdated()
        {
            // Given
            IEnumerable<string> existingLastChangeIdentifier = this.context.IconItemLastChange.Select(lc => lc.Identifier);
            string identifier = this.context.ItemIdentifier.Where(ii => ii.Deleted_Identifier == 0 && ii.Default_Identifier == 1 && ii.Item.Retail_Sale == true)
                .Select(ii => ii.Identifier)
                .Intersect(existingLastChangeIdentifier)
                .First();

            ValidatedBrand brand = this.context.ValidatedBrand.First();
            IconItemLastChangeModel expectedItem = new TestIconItemLastChangeModelBuilder()
                .WithScanCode(identifier).WithTaxClass("9876976 Tax Not In Irma").WithBrandId(brand.IconBrandId).WithBrandName(brand.ItemBrand.Brand_Name).Build();

            this.lastChangedItems.Add(expectedItem);
            this.command.IconLastChangedItems = this.lastChangedItems;

            IconItemLastChange actualLastChange = this.context.IconItemLastChange.Single(lc => lc.Identifier == identifier);
            DbEntityEntry<IconItemLastChange> entry = this.context.Entry(actualLastChange);

            // When
            this.handler.Handle(this.command);

            // Then
            entry.Reload();
            Assert.AreEqual(actualLastChange.Brand_ID, entry.Property(e => e.Brand_ID).CurrentValue, "Brand_ID value should not have changed.");
            Assert.AreEqual(actualLastChange.Food_Stamps, entry.Property(e => e.Food_Stamps).CurrentValue, "Food_Stamps value should not have changed.");
            Assert.AreEqual(actualLastChange.Item_Description, entry.Property(e => e.Item_Description).CurrentValue, "Item_Description value should not have changed.");
            Assert.AreEqual(actualLastChange.POS_Description, entry.Property(e => e.POS_Description).CurrentValue, "POS_Description value should not have changed.");
            Assert.AreEqual(actualLastChange.Package_Desc1, entry.Property(e => e.Package_Desc1).CurrentValue, "Package_Desc1 value should not have changed.");
            Assert.AreEqual(actualLastChange.ScaleTare, entry.Property(e => e.ScaleTare).CurrentValue, "ScaleTare value should not have changed.");
            Assert.AreEqual(actualLastChange.Subteam_No, entry.Property(e => e.Subteam_No).CurrentValue, "Subteam_No value should not have changed.");
            Assert.AreEqual(actualLastChange.TaxClassID, entry.Property(e => e.TaxClassID).CurrentValue, "TaxClass_ID value should not have changed.");
        }

          [TestMethod]
        public void BulkAddUpdateLastChange_ValidatedItemIsDefaultIdentifierWithDifferentTaxClassDescription_NewRowAddedToIconItemLastChange()
        {
            // Given
            IEnumerable<string> existingLastChangeIdentifier = this.context.IconItemLastChange.Select(lc => lc.Identifier);
            string identifier = this.context.ItemIdentifier.Where(ii => ii.Deleted_Identifier == 0 && ii.Default_Identifier == 1 && ii.Item.Retail_Sale == true)
                .Select(ii => ii.Identifier)
                .Except(existingLastChangeIdentifier)
                .First();

            var itemKey = this.context.ItemIdentifier.First(ii => ii.Identifier == identifier).Item_Key;
            var itemChangeHistory = this.context.Database.ExecuteSqlCommand(
                "insert into itemchangehistory(item_key, insert_date) values(@item_key, @insert_date)",
                new SqlParameter("@item_key", itemKey),
                new SqlParameter("@insert_date", DateTime.Now));

            TaxClass tax = this.context.TaxClass.First(tc => !tc.TaxClassDesc.ToLower().Contains("do not use"));
            ValidatedBrand brand = this.context.ValidatedBrand.First();

            IconItemLastChangeModel expectedItem = new TestIconItemLastChangeModelBuilder()
                .WithScanCode(identifier).WithTaxClass(tax.TaxClassDesc + "TEST").WithBrandId(brand.IconBrandId).WithBrandName(brand.ItemBrand.Brand_Name).Build();

            this.lastChangedItems.Add(expectedItem);
            this.command.IconLastChangedItems = this.lastChangedItems;

            if (this.context.IconItemLastChange.Any(lc => lc.Identifier == identifier))
            {
                Assert.Fail(String.Format("There is already a Last Change row for Identifier {0}", identifier));
            }

            // When
            this.handler.Handle(this.command);

            // Then
            IconItemLastChange lastChange = this.context.IconItemLastChange.Single(lc => lc.Identifier == identifier);

            Assert.AreEqual(expectedItem.ScanCode, lastChange.Identifier, "The Last Change Identifier does not match expected");
            Assert.AreEqual(null, lastChange.Subteam_No, "The Last Change SubTeam does not match expected");
            Assert.AreEqual(brand.IrmaBrandId, lastChange.Brand_ID, "The Last Change Brand_ID does not match expected");
            Assert.AreEqual(expectedItem.ProductDescription, lastChange.Item_Description, "The Last Change Item Description does not match expected");
            Assert.AreEqual(expectedItem.PosDescription, lastChange.POS_Description, "The Last Change POS Description does not match expected");
            Assert.AreEqual(Convert.ToDecimal(expectedItem.PackageUnit), lastChange.Package_Desc1, "The Last Change Package_Desc1 does not match expected");
            Assert.AreEqual(expectedItem.FoodStampEligible == "1", lastChange.Food_Stamps, "The Last Change FoodStamps does not match expected");
            Assert.AreEqual(Convert.ToDecimal(expectedItem.Tare), lastChange.ScaleTare, "The Last Change ScaleTare does not match expected");
            Assert.AreEqual(tax.TaxClassID, lastChange.TaxClassID, "The Last Change TaxClassID does not match expected");
            Assert.IsNotNull(lastChange.InsertDate, "The Last Change InsertDate is null");
        }

        [TestMethod]
        public void BulkAddUpdateLastChange_ValidatedItemIsDefaultIdentifierWithDifferentRetailUom_NewRowAddedToIconItemLastChange()
        {
            // Given
            IEnumerable<string> existingLastChangeIdentifier = this.context.IconItemLastChange.Select(lc => lc.Identifier);
            string identifier = this.context.ItemIdentifier.Where(ii => ii.Deleted_Identifier == 0 && ii.Default_Identifier == 1 && ii.Item.Retail_Sale == true)
                .Select(ii => ii.Identifier)
                .Except(existingLastChangeIdentifier)
                .First();

            var itemKey = this.context.ItemIdentifier.First(ii => ii.Identifier == identifier).Item_Key;
            var itemChangeHistory = this.context.Database.ExecuteSqlCommand(
                "insert into itemchangehistory(item_key, insert_date) values(@item_key, @insert_date)",
                new SqlParameter("@item_key", itemKey),
                new SqlParameter("@insert_date", DateTime.Now));

            ItemUnit iu = this.context.ItemUnit.First();
            ValidatedBrand brand = this.context.ValidatedBrand.First();

            IconItemLastChangeModel expectedItem = new TestIconItemLastChangeModelBuilder()
                .WithScanCode(identifier).WithRetailUom(iu.Unit_Abbreviation).WithBrandId(brand.IconBrandId).WithBrandName(brand.ItemBrand.Brand_Name).Build();

            this.lastChangedItems.Add(expectedItem);
            this.command.IconLastChangedItems = this.lastChangedItems;

            if (this.context.IconItemLastChange.Any(lc => lc.Identifier == identifier))
            {
                Assert.Fail(String.Format("There is already a Last Change row for Identifier {0}", identifier));
            }

            // When
            this.handler.Handle(this.command);

            // Then
            IconItemLastChange lastChange = this.context.IconItemLastChange.Single(lc => lc.Identifier == identifier);

            Assert.AreEqual(iu.Unit_ID, lastChange.Package_Unit_ID, "The Last Change Package Unit ID does not match expected");
        }

        [TestMethod]
        public void BulkAddUpdateLastChange_ValidatedItemIsDefaultIdentifierWithDifferentRetailUom_LastChangeRecordIsUpdated()
        {
            // Given
            IEnumerable<string> existingLastChangeIdentifier = this.context.IconItemLastChange.Select(lc => lc.Identifier);
            if (existingLastChangeIdentifier.Count() == 0)
            {
                // TODO: Add Last Change if there isn't one
                Assert.Fail("There is no existing IconItemLastChange rows to test with.");
            }

            string identifier = this.context.ItemIdentifier.Where(ii => ii.Deleted_Identifier == 0 && ii.Default_Identifier == 1 && ii.Item.Retail_Sale == true)
                .Select(ii => ii.Identifier)
                .Intersect(existingLastChangeIdentifier)
                .First();

            ItemUnit iu = this.context.ItemUnit.First();
            ValidatedBrand brand = this.context.ValidatedBrand.First();

            IconItemLastChangeModel expectedItem = new TestIconItemLastChangeModelBuilder()
                .WithScanCode(identifier).WithRetailUom(iu.Unit_Abbreviation).WithBrandId(brand.IconBrandId).WithBrandName(brand.ItemBrand.Brand_Name).WithAreNutriFactChanged(true).Build();

            this.lastChangedItems.Add(expectedItem);
            this.command.IconLastChangedItems = this.lastChangedItems;

            if (this.context.IconItemLastChange.Count(lc => lc.Identifier == identifier) == 0)
            {
                Assert.Fail(String.Format("There is not a Last Change row for Identifier {0}", identifier));
            }

            IconItemLastChange actualLastChange = this.context.IconItemLastChange.Single(lc => lc.Identifier == identifier);
            DbEntityEntry entry = this.context.Entry(actualLastChange);

            // When
            this.handler.Handle(this.command);

            // Then
            entry.Reload(); // reload to get values from DB

            IconItemLastChange lastChange = this.context.IconItemLastChange.Single(lc => lc.Identifier == identifier);

            Assert.AreEqual(iu.Unit_ID, lastChange.Package_Unit_ID, "The Last Change Package Unit ID does not match expected");
        }

        [TestMethod]
        public void BulkAddUpdateLastChange_ValidatedItemIsDefaultIdentifierWithDifferentRetailSize_NewRowAddedToIconItemLastChange()
        {
            // Given
            IEnumerable<string> existingLastChangeIdentifier = this.context.IconItemLastChange.Select(lc => lc.Identifier);
            string identifier = this.context.ItemIdentifier.Where(ii => ii.Deleted_Identifier == 0 && ii.Default_Identifier == 1 && ii.Item.Retail_Sale == true)
                .Select(ii => ii.Identifier)
                .Except(existingLastChangeIdentifier)
                .First();

            var itemKey = this.context.ItemIdentifier.First(ii => ii.Identifier == identifier).Item_Key;
            var itemChangeHistory = this.context.Database.ExecuteSqlCommand(
                "insert into itemchangehistory(item_key, insert_date) values(@item_key, @insert_date)",
                new SqlParameter("@item_key", itemKey),
                new SqlParameter("@insert_date", DateTime.Now));

            ValidatedBrand brand = this.context.ValidatedBrand.First();

            IconItemLastChangeModel expectedItem = new TestIconItemLastChangeModelBuilder()
                .WithScanCode(identifier).WithBrandId(brand.IconBrandId).WithBrandName(brand.ItemBrand.Brand_Name).Build();

            this.lastChangedItems.Add(expectedItem);
            this.command.IconLastChangedItems = this.lastChangedItems;

            if (this.context.IconItemLastChange.Any(lc => lc.Identifier == identifier))
            {
                Assert.Fail(String.Format("There is already a Last Change row for Identifier {0}", identifier));
            }

            // When
            this.handler.Handle(this.command);

            // Then
            IconItemLastChange lastChange = this.context.IconItemLastChange.Single(lc => lc.Identifier == identifier);

            Assert.AreEqual(expectedItem.RetailSize, lastChange.Package_Desc2, "The Last Change Package Desc 2 does not match expected");
        }

        [TestMethod]
        public void BulkAddUpdateLastChange_ValidatedItemIsDefaultIdentifierWithDifferentRetailSize_LastChangeRecordIsUpdated()
        {
            // Given
            IEnumerable<string> existingLastChangeIdentifier = this.context.IconItemLastChange.Select(lc => lc.Identifier);
            if (existingLastChangeIdentifier.Count() == 0)
            {
                // TODO: Add Last Change if there isn't one
                Assert.Fail("There is no existing IconItemLastChange rows to test with.");
            }

            string identifier = this.context.ItemIdentifier.Where(ii => ii.Deleted_Identifier == 0 && ii.Default_Identifier == 1 && ii.Item.Retail_Sale == true)
                .Select(ii => ii.Identifier)
                .Intersect(existingLastChangeIdentifier)
                .First();

            ValidatedBrand brand = this.context.ValidatedBrand.First();

            IconItemLastChangeModel expectedItem = new TestIconItemLastChangeModelBuilder()
                .WithScanCode(identifier).WithBrandId(brand.IconBrandId).WithBrandName(brand.ItemBrand.Brand_Name).WithAreNutriFactChanged(true).Build();

            this.lastChangedItems.Add(expectedItem);
            this.command.IconLastChangedItems = this.lastChangedItems;

            if (this.context.IconItemLastChange.Count(lc => lc.Identifier == identifier) == 0)
            {
                Assert.Fail(String.Format("There is not a Last Change row for Identifier {0}", identifier));
            }

            IconItemLastChange actualLastChange = this.context.IconItemLastChange.Single(lc => lc.Identifier == identifier);
            DbEntityEntry entry = this.context.Entry(actualLastChange);

            // When
            this.handler.Handle(this.command);

            // Then
            entry.Reload(); // reload to get values from DB

            IconItemLastChange lastChange = this.context.IconItemLastChange.Single(lc => lc.Identifier == identifier);

            Assert.AreEqual(expectedItem.RetailSize, lastChange.Package_Desc2, "The Last Change Package Desc 2 does not match expected");
        }
    }
}
