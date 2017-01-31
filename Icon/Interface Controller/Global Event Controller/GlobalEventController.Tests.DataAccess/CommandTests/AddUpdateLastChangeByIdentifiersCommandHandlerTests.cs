using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Irma.Framework;
using GlobalEventController.DataAccess.Commands;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace GlobalEventController.Tests.DataAccess.CommandTests
{
    [TestClass]
    public class AddUpdateLastChangeByIdentifiersCommandHandlerTests
    {
        private IrmaContext context;
        private AddUpdateLastChangeByIdentifiersCommand command;
        private AddUpdateLastChangeByIdentifiersCommandHandler handler;
        private DbContextTransaction transaction;
        private List<IconItemLastChange> lastChangeList;
        private List<ItemIdentifier> itemIdentifierList;
        private TaxClass existingTaxClass;
        private ItemBrand existingBrand;

        [TestInitialize]
        public void InitializeData()
        {
            this.context = new IrmaContext(); // no constructor uses the FL region: idd-fl\fld
            this.command = new AddUpdateLastChangeByIdentifiersCommand();
            this.handler = new AddUpdateLastChangeByIdentifiersCommandHandler(this.context);
            this.lastChangeList = new List<IconItemLastChange>();
            this.itemIdentifierList = new List<ItemIdentifier>();

            
            this.existingTaxClass = this.context.TaxClass.First(tc => !tc.TaxClassDesc.ToLower().Contains("do not use"));
            this.existingBrand = this.context.ItemBrand.First(ib => ib.ValidatedBrand.Any());

            this.transaction = this.context.Database.BeginTransaction();
        }

        [TestCleanup]
        public void CleanupData()
        {
            if (this.transaction != null)
            {
                this.transaction.Rollback();
            }
        }

        [TestMethod]
        public void AddUpdateLastChangeByIdentifiers_LastChangeRowsDoNotExistsForListOfIdentifiers_LastChangeRowsAreAdded()
        {
            // Given
            // Get list of ItemIdentifiers that do not have an IconItemLastchange row
            this.itemIdentifierList = (from ii in this.context.ItemIdentifier
                                       join lc in this.context.IconItemLastChange on ii.Identifier equals lc.Identifier into templc
                                       from tlc in templc.DefaultIfEmpty() // similates a LEFT JOIN
                                       where
                                            tlc == null
                                            && ii.Deleted_Identifier == 0
                                            && ii.Default_Identifier == 1
                                            && ii.Item.Retail_Sale == true
                                       select ii)
                                       .Take(3).ToList();

            this.command.Identifiers = this.itemIdentifierList.Take(3).ToList();
            DateTime preTestDateTime = DateTime.Now;

            // When
            this.handler.Handle(this.command);
            this.context.SaveChanges();

            // Then
            for (int i = 0; i < this.command.Identifiers.Count; i++)
            {
                ItemIdentifier expected = this.itemIdentifierList[0];
                IconItemLastChange actual = this.context.IconItemLastChange.First(lc => lc.Identifier == expected.Identifier);
                Assert.IsNotNull(actual.IconItemLastChangeId);
                Assert.AreEqual(expected.Identifier, actual.Identifier, "The identifier of the Last Change row did not match the expected value.");
                Assert.AreEqual(expected.Item.Item_Description, actual.Item_Description, "The Item_Description of the Last Change row did not match the expected value.");
                Assert.AreEqual(expected.Item.POS_Description, actual.POS_Description, "The POS_Description of the Last Change row did not match the expected value.");
                Assert.AreEqual(expected.Item.Package_Desc1, actual.Package_Desc1, "The Package_Desc1 of the Last Change row did not match the expected value.");
                Assert.AreEqual(expected.Item.Food_Stamps, actual.Food_Stamps, "The Food_Stamps of the Last Change row did not match the expected value.");
                Assert.AreEqual(expected.Item.ScaleTare, actual.ScaleTare, "The ScaleTare of the Last Change row did not match the expected value.");
                Assert.AreEqual(expected.Item.SubTeam_No, actual.Subteam_No, "The Subteam_No of the Last Change row was not the expected null value.");
                Assert.AreEqual(expected.Item.TaxClassID, actual.TaxClassID, "The TaxClassID of the Last Change row did not match the expected value.");
                Assert.AreEqual(expected.Item.Brand_ID, actual.Brand_ID, "The Brand_ID of the Last Change row did not match the expected value.");
                Assert.IsTrue(actual.InsertDate > preTestDateTime);
            }
        }

        [TestMethod]
        public void AddUpdateLastChangeByIdentifiers_LastChangeRowsExistsForListOfIdentifiers_InsertDateOfLastChangeRowsIsUpdated()
        {
            // Given
            int maxNumberOfChanges = 5;
            DateTime preTestDateTime = DateTime.Now; System.Threading.Thread.Sleep(500);
            this.lastChangeList = GetExistingLastChangeRows(maxNumberOfChanges);

            if (this.lastChangeList.Count == 0)
            {
                this.itemIdentifierList = GetListOfIdentifiers(maxNumberOfChanges);
                this.lastChangeList = BuildLastChangeList(maxNumberOfChanges);
                this.context.IconItemLastChange.AddRange(this.lastChangeList);
                this.context.SaveChanges();
            }

            if (this.itemIdentifierList.Count == 0)
	        {
                IEnumerable<string> identifiers = this.lastChangeList.Select(lc => lc.Identifier);
		        this.itemIdentifierList = this.context.ItemIdentifier.Where(ii => identifiers.Contains(ii.Identifier)).Take(maxNumberOfChanges).ToList();
	        }

            this.command.Identifiers = this.itemIdentifierList;

            // When
            this.handler.Handle(this.command);
            this.context.SaveChanges();

            // Then
            // Nothing should be updated except the insert date.
            for (int i = 0; i < this.command.Identifiers.Count; i++)
            {
                IconItemLastChange expected = this.lastChangeList[i];
                IconItemLastChange actual = this.context.IconItemLastChange.First(lc => lc.Identifier == expected.Identifier);
                Assert.IsNotNull(actual.IconItemLastChangeId);
                Assert.AreEqual(this.lastChangeList[i].Identifier, actual.Identifier, "The identifier of the existing Last Change row updated unexpectedly.");
                Assert.AreEqual(this.lastChangeList[i].Item_Description, actual.Item_Description, "The Item_Description of the existing Last Change row updated unexpectedly.");
                Assert.AreEqual(this.lastChangeList[i].POS_Description, actual.POS_Description, "The POS_Description of the existing Last Change row updated unexpectedly.");
                Assert.AreEqual(this.lastChangeList[i].Package_Desc1, actual.Package_Desc1, "The Package_Desc1 of the existing Last Change row updated unexpectedly.");
                Assert.AreEqual(this.lastChangeList[i].Food_Stamps, actual.Food_Stamps, "The Food_Stamps of the existing Last Change row updated unexpectedly.");
                Assert.AreEqual(this.lastChangeList[i].ScaleTare, actual.ScaleTare, "The ScaleTare of the existing Last Change row updated unexpectedly.");
                Assert.AreEqual(this.lastChangeList[i].Subteam_No, actual.Subteam_No, "The Subteam_No of the existing Last Change row updated unexpectedly.");
                Assert.AreEqual(this.lastChangeList[i].TaxClassID, actual.TaxClassID, "The TaxClassID of the existing Last Change row updated unexpectedly.");
                Assert.AreEqual(this.lastChangeList[i].Brand_ID, actual.Brand_ID, "The Brand_ID of the existing Last Change row updated unexpectedly.");
                Assert.IsTrue(actual.InsertDate > preTestDateTime, "The LastChange insertdate was not updated for the existing rows.");
            }
        }

        private List<IconItemLastChange> GetExistingLastChangeRows(int maxRows)
        {
            List<IconItemLastChange> lastChanges = new List<IconItemLastChange>();
            lastChanges = this.context.IconItemLastChange.Take(maxRows).ToList();
            return lastChanges;
        }

        private List<ItemIdentifier> GetListOfIdentifiers(int maxRows)
        {
            List<ItemIdentifier> itemIdentifiers = new List<ItemIdentifier>();
            itemIdentifierList = this.context.ItemIdentifier
                .Include(ii => ii.Item)
                .Where(ii => ii.Deleted_Identifier == 0 && ii.Default_Identifier == 1 && ii.Item.Retail_Sale == true)
                .Take(maxRows).ToList();

            return itemIdentifierList;
        }

        private List<IconItemLastChange> BuildLastChangeList(int maxRows)
        {
            List<IconItemLastChange> lastChanges = new List<IconItemLastChange>();
            for (int i = 0; i < maxRows; i++)
            {
                IconItemLastChange lastChange = new TestIconItemLastChangeBuilder()
                    .WithIdentifier(this.itemIdentifierList[i].Identifier)
                    .WithItemDescription("AddUpdate Last Change Test" + i.ToString())
                    .WithPosDescription(("AddUpdateLsTest" + i.ToString()).ToUpper())
                    .WithPackageUnit(Convert.ToDecimal(i))
                    .WithTaxClassId(this.existingTaxClass.TaxClassID)
                    .WithBrandId(this.existingBrand.Brand_ID)
                    .Build();
                lastChanges.Add(lastChange);
            }

            return lastChanges;
        }
    }
}
