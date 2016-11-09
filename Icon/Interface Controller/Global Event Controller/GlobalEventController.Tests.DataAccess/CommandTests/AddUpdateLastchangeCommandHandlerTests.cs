using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Irma.Framework;
using GlobalEventController.DataAccess.Commands;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using GlobalEventController.Common;
using System.Threading;

namespace GlobalEventController.Tests.DataAccess.CommandTests
{
    [TestClass]
    public class AddUpdateLastchangeCommandHandlerTests
    {
        private IrmaContext context;
        private AddUpdateLastChangeCommand command;
        private AddUpdateLastChangeCommandHandler handler;
        private DbContextTransaction transaction;
        private string testIdentifier;
        private IconItemLastChange existingLastChange;
        private TaxClass existingTaxClass;
        private ItemBrand existingBrand;
        private ItemUnit itemUnit;

        [TestInitialize]
        public void InitializeData()
        {
            this.context = new IrmaContext(); // no constructor uses the FL region: idd-fl\fld
            this.command = new AddUpdateLastChangeCommand();
            this.handler = new AddUpdateLastChangeCommandHandler(this.context);

            this.existingTaxClass = this.context.TaxClass.First(tc => !tc.TaxClassDesc.ToLower().Contains("do not use"));
            this.existingBrand = this.context.ItemBrand.First(ib => ib.ValidatedBrand.Any());
            this.testIdentifier = "445566332211";
            this.itemUnit = this.context.ItemUnit.First();

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
        public void AddUpdateLastChange_VastChangeItemDoesNotHaveLastChangeRow_LastChangeRowIsAdded()
        {
            // Given
            IEnumerable<string> existingLastChangeIdentifier = this.context.IconItemLastChange.Select(lc => lc.Identifier);
            string identifier = this.context.ItemIdentifier.Where(ii => ii.Deleted_Identifier == 0 && ii.Default_Identifier == 1 && ii.Item.Retail_Sale == true)
                .Select(ii => ii.Identifier)
                .Except(existingLastChangeIdentifier)
                .First();

            if (this.context.IconItemLastChange.Any(lc => lc.Identifier == identifier))
            {
                Assert.Fail("The test identifier is already in the last change table. Check the code that gets the test identifier.");
            }

            IconItemLastChangeModel lastChangeItem = new TestIconItemLastChangeModelBuilder()
                .WithScanCode(identifier)
                .WithProductDesccription("AddUpd Last Change Unit Test")
                .WithPosDescription("Last Chg Unit Test")
                .WithPackageUnit("1")
                .WithItemId(1)
                .WithAreNutriFactChanged(true)
                .WithSubTeamNo(10)
                .WithSubTeamNotAligned(false)
                .Build();

            this.command.BrandId = existingBrand.Brand_ID;
            this.command.TaxClassId = existingTaxClass.TaxClassID;
            this.command.UpdatedItem = lastChangeItem;
            this.command.PackageUnitId = itemUnit.Unit_ID;

            // When
            this.handler.Handle(this.command);
            this.context.SaveChanges(); // .SaveChanges() is not called in the command handler.

            // Then
            IconItemLastChange actual = this.context.IconItemLastChange.First(lc => lc.Identifier == identifier);

            Assert.IsNotNull(actual, String.Format("There was no last change row found for Identifier {0}", identifier));
            Assert.AreEqual(lastChangeItem.ScanCode, actual.Identifier, "The identifier of the new Last Change record did not match the expected value.");
            Assert.AreEqual(command.BrandId, actual.Brand_ID, "The Brand_ID of the new Last Change record did not match the expected value.");
            Assert.AreEqual(lastChangeItem.ProductDescription, actual.Item_Description, "The Item_Description of the new Last Change record did not match the expected value.");
            Assert.AreEqual(lastChangeItem.PosDescription.ToUpper(), actual.POS_Description, "The POS_Description of the new Last Change record did not match the expected value.");
            Assert.AreEqual(lastChangeItem.PackageUnit, actual.Package_Desc1.ToString(), "The Package_Desc1 of the new Last Change record did not match the expected value.");
            Assert.AreEqual(lastChangeItem.FoodStampEligible == "1", actual.Food_Stamps, "The Food_Stamps of the new Last Change record did not match the expected value.");
            Assert.AreEqual(lastChangeItem.Tare, actual.ScaleTare.ToString(), "The ScaleTare of the new Last Change record did not match the expected value.");
            Assert.AreEqual(this.command.TaxClassId, actual.TaxClassID, "The TaxClassID of the new Last Change record did not match the expected value.");
            Assert.AreEqual(this.command.PackageUnitId, actual.Package_Unit_ID, "The PackageUnitID of the new Last Change record did not match the expected value");
            Assert.AreEqual(lastChangeItem.RetailSize, actual.Package_Desc2.Value, "The Package_Desc2 of the existing Last Change record did not match the expected value");
            Assert.IsNotNull(actual.InsertDate, "The InsertDate is null.");
            Assert.IsTrue(actual.AreNutriFactsChanged.Value);
            Assert.AreEqual(lastChangeItem.SubTeamNo, actual.Subteam_No);
        }

        [TestMethod]
        public void AddUpdateLastChange_LastChangeItemRowAlreadyHasLastChangeRow_LastChangeRowIsUpdatedWithLastChangeItemData()
        {
            // Given
            this.existingLastChange = this.context.IconItemLastChange.First();
            this.testIdentifier = this.existingLastChange.Identifier;

            if (this.existingLastChange == null)
            {
                this.existingLastChange = BuildLastChange(this.testIdentifier);
            }

            IconItemLastChangeModel lastChangeItem = new TestIconItemLastChangeModelBuilder()
                .WithScanCode(this.testIdentifier)
                .WithProductDesccription("AddUpd Last Change Unit Test")
                .WithPosDescription("Last Chg Unit Test")
                .WithPackageUnit("1")
                .WithItemId(1)
                .WithSubTeamNo(10)
                .WithSubTeamNotAligned(false)
                .Build();

            this.command.TaxClassId = this.existingTaxClass.TaxClassID;
            this.command.BrandId = this.existingBrand.Brand_ID;
            this.command.UpdatedItem = lastChangeItem;
            this.command.PackageUnitId = itemUnit.Unit_ID;
            DateTime preTestDateTime = DateTime.Now;

            Thread.Sleep(100);

            // When
            this.handler.Handle(this.command);
            this.context.SaveChanges(); // command handler does not call .SaveChanges();

            // Then
            IconItemLastChange actual = this.context.IconItemLastChange.First(lc => lc.Identifier == this.testIdentifier);

            Assert.IsNotNull(actual, String.Format("There was no last change row found for Identifier {0}", this.testIdentifier));
            Assert.AreEqual(lastChangeItem.ScanCode, actual.Identifier, "The identifier of the existing Last Change record did not match the expected value.");
            Assert.AreEqual(command.BrandId, actual.Brand_ID, "The Brand_ID of the existing Last Change record did not match the expected value.");
            Assert.AreEqual(lastChangeItem.ProductDescription, actual.Item_Description, "The Item_Description of the existing Last Change record did not match the expected value.");
            Assert.AreEqual(lastChangeItem.PosDescription.ToUpper(), actual.POS_Description, "The POS_Description of the existing Last Change record did not match the expected value.");
            Assert.AreEqual(lastChangeItem.PackageUnit, actual.Package_Desc1.ToString(), "The Package_Desc1 of the existing Last Change record did not match the expected value.");
            Assert.AreEqual(lastChangeItem.FoodStampEligible == "1", actual.Food_Stamps, "The Food_Stamps of the existing Last Change record did not match the expected value.");
            Assert.AreEqual(lastChangeItem.Tare, actual.ScaleTare.ToString(), "The ScaleTare of the existing Last Change record did not match the expected value.");
            Assert.AreEqual(this.command.TaxClassId, actual.TaxClassID, "The TaxClassID of the new Last Change record did not match the expected value.");
            Assert.AreEqual(this.command.PackageUnitId, actual.Package_Unit_ID, "The PackageUnitID of the existing Last Change record did not match the expected value");
            Assert.AreEqual(lastChangeItem.RetailSize, actual.Package_Desc2.Value, "The Package_Desc2 of the existing Last Change record did not match the expected value");
            Assert.IsTrue(actual.InsertDate > preTestDateTime, "The InsertDate did not change.");
            Assert.AreEqual(lastChangeItem.SubTeamNo, actual.Subteam_No);
        }

        private IconItemLastChange BuildLastChange(string identifier)
        {
            IconItemLastChange lastChange = new TestIconItemLastChangeBuilder()
                .WithIdentifier(identifier)
                .WithItemDescription("AddUpdate Last Change Test")
                .WithPosDescription("AddUpdateLsTest")
                .WithPackageUnit(1)
                .WithTaxClassId(this.existingTaxClass.TaxClassID)
                .WithBrandId(this.existingBrand.Brand_ID)
                .Build();

            return lastChange;
        }
    }
}
