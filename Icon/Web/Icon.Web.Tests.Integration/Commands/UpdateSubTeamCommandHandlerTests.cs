using Icon.Framework;
using Icon.Logging;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;

namespace Icon.Web.Tests.Integration.Commands
{
    [TestClass] [Ignore]
    public class UpdateSubTeamCommandHandlerTests
    {
        private IconContext context;
        private Mock<ILogger> logger;
        private UpdateSubTeamCommandHandler handler;
        private HierarchyClass subTeam;
        private HierarchyClass merchLevelFive;
        private Item item;

        [TestInitialize]
        public void InitializeData()
        {
            this.context = new IconContext();
            this.logger = new Mock<ILogger>();
            this.handler = new UpdateSubTeamCommandHandler(this.context);

            Cleanup();

            // Add Test SubTeams To Work With
            this.subTeam = AddFakeSubTeam();

            // Setup more test data
            AddFakeLeafMerchandiseClass();
            AddFakeItemsAssociatedToMerch();
        }

        [TestCleanup]
        public void CleanupTestData()
        {
            // remove item data
            Cleanup();

            this.context.Dispose();
        }

        private void Cleanup()
        {
            // Remove Item entries
            Item testItem = context.Item.FirstOrDefault(i => i.ScanCode.FirstOrDefault().scanCode == "111222444822");
            if (testItem != null)
            {
                this.context.Database.CommandTimeout = 180;
                this.context.Configuration.AutoDetectChangesEnabled = false;

                this.context.ItemHierarchyClass.RemoveRange(testItem.ItemHierarchyClass);
                this.context.ItemTrait.RemoveRange(testItem.ItemTrait);
                this.context.ScanCode.RemoveRange(testItem.ScanCode);
                this.context.MessageQueueProduct.RemoveRange(this.context.MessageQueueProduct.Where(pq => pq.ItemId == testItem.itemID));
                this.context.Item.Remove(testItem);

                this.context.ChangeTracker.DetectChanges();
                this.context.SaveChanges();
            }

            // Remove Hierarchy Class entries
            this.context.HierarchyClassTrait.RemoveRange(this.context.HierarchyClassTrait.Where(hct
                => hct.HierarchyClass.hierarchyClassName == "Automated Test Fake Sub-Team (8769)"
                || hct.HierarchyClass.hierarchyClassName == "Automated Test Fake Sub-Team (8711)"
                || hct.HierarchyClass.hierarchyClassName == "Automated Edit Test Fake Sub-Team (8769)"
                || hct.HierarchyClass.hierarchyClassName == "Automated Edit Fake Sub-Team (2424)"
                || (hct.HierarchyClass.hierarchyClassName.Contains("Merch Level") && hct.HierarchyClass.hierarchyID == Hierarchies.Merchandise)));
            this.context.SaveChanges();

            this.context.HierarchyClass.RemoveRange(context.HierarchyClass.Where(hc =>
                   hc.hierarchyClassName == "Automated Test Fake Sub-Team (8769)"
                || hc.hierarchyClassName == "Automated Test Fake Sub-Team (8711)"
                || hc.hierarchyClassName == "Automated Edit Fake Sub-Team (8769)"
                || hc.hierarchyClassName == "Automated Edit Fake Sub-Team (2424)"
                || hc.hierarchyClassName == "Test Duplicate SubTeam (8711)"
                || (hc.hierarchyClassName.Contains("Merch Level") && hc.hierarchyID == Hierarchies.Merchandise)));
            this.context.SaveChanges();

            this.context.MessageQueueHierarchy.RemoveRange(this.context.MessageQueueHierarchy.Where(hq => hq.HierarchyClassId == "8711" || hq.HierarchyClassId == "2424"));
            this.context.SaveChanges();
        }

        [TestMethod]
        public void UpdateSubTeam_DuplicateSubTeamsExist_ShouldNotUpdateHierarchyClass()
        {
            // Given.
            context.HierarchyClass.Add(new HierarchyClass
            {
                hierarchyClassName = "Test Duplicate SubTeam (8711)",
                Hierarchy = context.Hierarchy.First(h => h.hierarchyID == Hierarchies.Financial)
            });
            context.SaveChanges();

            UpdateSubTeamCommand command = new UpdateSubTeamCommand();
            command.HierarchyClassId = subTeam.hierarchyClassID;
            command.HierarchyId = subTeam.hierarchyID;
            command.HierarchyLevel = subTeam.hierarchyLevel.Value;
            command.HierarchyParentClassId = subTeam.hierarchyParentClassID;
            command.PeopleSoftNumber = "8711";
            command.SubTeamName = "Test Duplicate SubTeam";

            // When.
            try
            {
                handler.Execute(command);
                Assert.Fail("Expected exception was not thrown.");
            }
            catch (DuplicateValueException dvex)
            {
                // Then.
                Assert.AreEqual(dvex.Message, "SubTeam Test Duplicate SubTeam (8711) already exists. Please pick another name.");
            }
        }

        [TestMethod]
        public void UpdateSubTeam_ChangePeopleSoftNumber_ShouldUpdatePeopleSoftNumberInSubTeamStringAndTraitValues()
        {
            // Given
            UpdateSubTeamCommand command = new UpdateSubTeamCommand();
            command.HierarchyClassId = subTeam.hierarchyClassID;
            command.HierarchyId = subTeam.hierarchyID;
            command.HierarchyLevel = subTeam.hierarchyLevel.Value;
            command.HierarchyParentClassId = subTeam.hierarchyParentClassID;
            command.PeopleSoftNumber = "8711";
            command.SubTeamName = "Automated Test Fake Sub-Team";

            string expectedSubTeam = "Automated Test Fake Sub-Team (8711)";

            // When
            handler.Execute(command);

            // Then
            var actualSubTeam = context.HierarchyClass.SingleOrDefault(hc => hc.hierarchyClassID == command.HierarchyClassId);
            var actualTrait = context.HierarchyClassTrait.SingleOrDefault(hct => hct.hierarchyClassID == merchLevelFive.hierarchyClassID && hct.Trait.traitCode == TraitCodes.MerchFinMapping);
            var subTeamEntry = context.Entry(actualSubTeam);
            var traitEntry = context.Entry(actualTrait);

            Assert.AreEqual(expectedSubTeam, actualSubTeam.hierarchyClassName);
            Assert.AreEqual(expectedSubTeam, actualTrait.traitValue);
            Assert.IsTrue(subTeamEntry.State == EntityState.Unchanged);
            Assert.IsTrue(traitEntry.State == EntityState.Unchanged);
        }

        [TestMethod]
        public void UpdateSubTeam_ChangePeopleSoftNumber_ShouldNotifyThatPeopleSoftHasChangedAndReturnUpdatedHierarchyClass()
        {
            // Given
            UpdateSubTeamCommand command = new UpdateSubTeamCommand();
            command.HierarchyClassId = subTeam.hierarchyClassID;
            command.HierarchyId = subTeam.hierarchyID;
            command.HierarchyLevel = subTeam.hierarchyLevel.Value;
            command.HierarchyParentClassId = subTeam.hierarchyParentClassID;
            command.PeopleSoftNumber = "8711";
            command.SubTeamName = "Automated Test Fake Sub-Team";

            string expectedHierarchyClassName = "Automated Test Fake Sub-Team (8711)";

            // When
            handler.Execute(command);

            // Then
            HierarchyClass updatedHierarchyClass = context.HierarchyClass.First(hc => hc.hierarchyClassID == command.HierarchyClassId);

            Assert.IsNotNull(command.UpdatedHierarchyClass);
            Assert.AreEqual(updatedHierarchyClass.hierarchyClassID, command.UpdatedHierarchyClass.hierarchyClassID);
            Assert.AreEqual(expectedHierarchyClassName, command.UpdatedHierarchyClass.hierarchyClassName);
            Assert.AreEqual(updatedHierarchyClass.hierarchyID, command.HierarchyId);
            Assert.AreEqual(updatedHierarchyClass.hierarchyLevel, command.HierarchyLevel);
            Assert.AreEqual(updatedHierarchyClass.hierarchyParentClassID, command.HierarchyParentClassId);
            Assert.IsTrue(command.PeopleSoftChanged);
        }

        [TestMethod]
        public void UpdateSubTeam_ChangeSubTeamName_ShouldUpdateSubTeamNameInSubTeamStringAndTraitValues()
        {
            // Given
            UpdateSubTeamCommand command = new UpdateSubTeamCommand();
            command.HierarchyClassId = subTeam.hierarchyClassID;
            command.HierarchyId = subTeam.hierarchyID;
            command.HierarchyLevel = subTeam.hierarchyLevel.Value;
            command.HierarchyParentClassId = subTeam.hierarchyParentClassID;
            command.PeopleSoftNumber = "8769";
            command.SubTeamName = "Automated Edit Fake Sub-Team";

            string expectedSubTeam = "Automated Edit Fake Sub-Team (8769)";

            // When
            handler.Execute(command);

            // Then
            var actualSubTeam = context.HierarchyClass.SingleOrDefault(hc => hc.hierarchyClassID == command.HierarchyClassId);
            var actualTrait = context.HierarchyClassTrait.SingleOrDefault(hct => hct.hierarchyClassID == merchLevelFive.hierarchyClassID && hct.Trait.traitCode == TraitCodes.MerchFinMapping);
            var subTeamEntry = context.Entry(actualSubTeam);
            var traitEntry = context.Entry(actualTrait);

            Assert.AreEqual(expectedSubTeam, actualSubTeam.hierarchyClassName,
                String.Format("Expected hierarchyClassName {0} does not match the actual hierarchyClassName {1}", expectedSubTeam, actualSubTeam.hierarchyClassName));
            Assert.AreEqual(expectedSubTeam, actualTrait.traitValue,
                String.Format("Expected traitValue {0} does not match actual traitValue {1}", expectedSubTeam, actualTrait.traitValue));
            Assert.IsTrue(subTeamEntry.State == EntityState.Unchanged);
            Assert.IsTrue(traitEntry.State == EntityState.Unchanged);
        }

        [TestMethod]
        public void UpdateSubTeam_ChangeSubTeamAndPeopleSoftNumber_ShouldUpdateBothSubTeamNameAndPeoplesoftInSubTeamStringAndTraitValues()
        {
            // Given
            UpdateSubTeamCommand command = new UpdateSubTeamCommand();
            command.HierarchyClassId = subTeam.hierarchyClassID;
            command.HierarchyId = subTeam.hierarchyID;
            command.HierarchyLevel = subTeam.hierarchyLevel.Value;
            command.HierarchyParentClassId = subTeam.hierarchyParentClassID;
            command.PeopleSoftNumber = "2424";
            command.SubTeamName = "Automated Edit Fake Sub-Team";

            string expectedSubTeam = "Automated Edit Fake Sub-Team (2424)";

            // When
            handler.Execute(command);

            // Then
            var actualSubTeam = context.HierarchyClass.SingleOrDefault(hc => hc.hierarchyClassID == command.HierarchyClassId);
            var actualTrait = context.HierarchyClassTrait.SingleOrDefault(hct => hct.hierarchyClassID == merchLevelFive.hierarchyClassID && hct.Trait.traitCode == TraitCodes.MerchFinMapping);
            var subTeamEntry = context.Entry(actualSubTeam);
            var traitEntry = context.Entry(actualTrait);

            Assert.AreEqual(expectedSubTeam, actualSubTeam.hierarchyClassName);
            Assert.AreEqual(expectedSubTeam, actualTrait.traitValue);
            Assert.IsTrue(subTeamEntry.State == EntityState.Unchanged);
            Assert.IsTrue(traitEntry.State == EntityState.Unchanged);
        }

        [TestMethod]
        public void UpdateSubTeam_ChangeSubTeamNameOnly_ShouldNotNotifyThatPeopleSoftChanged()
        {
            // Given
            UpdateSubTeamCommand command = new UpdateSubTeamCommand();
            command.HierarchyClassId = subTeam.hierarchyClassID;
            command.HierarchyId = subTeam.hierarchyID;
            command.HierarchyLevel = subTeam.hierarchyLevel.Value;
            command.HierarchyParentClassId = subTeam.hierarchyParentClassID;
            command.PeopleSoftNumber = "8769";
            command.SubTeamName = "Automated Edit Fake Sub-Team";

            // When
            handler.Execute(command);

            // Then
            Assert.IsNotNull(command.UpdatedHierarchyClass);
            Assert.IsFalse(command.PeopleSoftChanged);
        }

        private HierarchyClass AddFakeSubTeam()
        {
            HierarchyClass hierarchyClass = new HierarchyClass();
            hierarchyClass.hierarchyClassName = "Automated Test Fake Sub-Team (8769)";
            hierarchyClass.hierarchyLevel = 1;
            hierarchyClass.hierarchyParentClassID = null;
            hierarchyClass.hierarchyID = context.Hierarchy.SingleOrDefault(h => h.hierarchyName == HierarchyNames.Financial).hierarchyID;

            context.HierarchyClass.Add(hierarchyClass);
            context.SaveChanges();

            return context.HierarchyClass.Include(hc => hc.Hierarchy).Include(hc => hc.HierarchyClassTrait).FirstOrDefault(hc => hc.hierarchyClassID == hierarchyClass.hierarchyClassID);
        }

        private List<HierarchyClass> AddFakeLeafMerchandiseClass()
        {
            HierarchyClass merchLevelOne = new HierarchyClass();
            merchLevelOne.hierarchyID = Hierarchies.Merchandise;
            merchLevelOne.hierarchyLevel = 1;
            merchLevelOne.hierarchyParentClassID = null;
            merchLevelOne.hierarchyClassName = "Merch Level One";
            this.context.HierarchyClass.Add(merchLevelOne);
            this.context.SaveChanges();

            HierarchyClass merchLevelTwo = new HierarchyClass();
            merchLevelTwo.hierarchyID = Hierarchies.Merchandise;
            merchLevelTwo.hierarchyLevel = 2;
            merchLevelTwo.hierarchyParentClassID = merchLevelOne.hierarchyClassID;
            merchLevelTwo.hierarchyClassName = "Merch Level Two";
            this.context.HierarchyClass.Add(merchLevelTwo);
            this.context.SaveChanges();

            HierarchyClass merchLevelThree = new HierarchyClass();
            merchLevelThree.hierarchyID = Hierarchies.Merchandise;
            merchLevelThree.hierarchyLevel = 3;
            merchLevelThree.hierarchyParentClassID = merchLevelTwo.hierarchyClassID;
            merchLevelThree.hierarchyClassName = "Merch Level Three";
            this.context.HierarchyClass.Add(merchLevelThree);
            this.context.SaveChanges();

            HierarchyClass merchLevelFour = new HierarchyClass();
            merchLevelFour.hierarchyID = Hierarchies.Merchandise;
            merchLevelFour.hierarchyLevel = 4;
            merchLevelFour.hierarchyParentClassID = merchLevelThree.hierarchyClassID;
            merchLevelFour.hierarchyClassName = "Merch Level Four";
            this.context.HierarchyClass.Add(merchLevelFour);
            this.context.SaveChanges();

            this.merchLevelFive = new HierarchyClass();
            merchLevelFive.hierarchyID = Hierarchies.Merchandise;
            merchLevelFive.hierarchyLevel = 5;
            merchLevelFive.hierarchyParentClassID = merchLevelFour.hierarchyClassID;
            merchLevelFive.hierarchyClassName = "Merch Level Five";
            this.context.HierarchyClass.Add(merchLevelFive);
            this.context.SaveChanges();

            HierarchyClassTrait merchFin = new HierarchyClassTrait();
            merchFin.hierarchyClassID = merchLevelFive.hierarchyClassID;
            merchFin.traitID = Traits.MerchFinMapping;
            merchFin.traitValue = this.subTeam.hierarchyClassName;
            merchFin.Trait = this.context.Trait.First(t => t.traitCode == TraitCodes.MerchFinMapping);

            HierarchyClassTrait sentToEsb = new HierarchyClassTrait();
            sentToEsb.hierarchyClassID = merchLevelFive.hierarchyClassID;
            sentToEsb.traitID = Traits.SentToEsb;
            sentToEsb.traitValue = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffff", CultureInfo.InvariantCulture);
            sentToEsb.Trait = this.context.Trait.First(t => t.traitCode == TraitCodes.SentToEsb);

            this.context.HierarchyClassTrait.Add(merchFin);
            this.context.HierarchyClassTrait.Add(sentToEsb);
            this.context.SaveChanges();

            List<HierarchyClass> fullMerchHierarchy = new List<HierarchyClass>();
            fullMerchHierarchy.Add(merchLevelOne);
            fullMerchHierarchy.Add(merchLevelTwo);
            fullMerchHierarchy.Add(merchLevelThree);
            fullMerchHierarchy.Add(merchLevelFour);
            fullMerchHierarchy.Add(merchLevelFive);

            return fullMerchHierarchy;
        }

        private void AddFakeItemsAssociatedToMerch()
        {
            this.item = new Item { itemTypeID = 1, ItemType = this.context.ItemType.First(it => it.itemTypeID == 1) };
            this.context.Item.Add(item);
            this.context.SaveChanges();

            ScanCode scanCode = new ScanCode { itemID = item.itemID, localeID = 1, scanCode = "111222444822", scanCodeTypeID = 1, ScanCodeType = this.context.ScanCodeType.First(sct => sct.scanCodeTypeID == 1) };
            this.context.ScanCode.Add(scanCode);
            this.context.SaveChanges();

            ItemTrait validationDate = new ItemTrait { itemID = item.itemID, localeID = 1, traitID = Traits.ValidationDate, traitValue = DateTime.Now.ToString(), Trait = this.context.Trait.First(t => t.traitCode == TraitCodes.ValidationDate) };
            ItemTrait productDescription = new ItemTrait { itemID = item.itemID, localeID = 1, traitID = Traits.ProductDescription, traitValue = "update subteam product description trait", Trait = this.context.Trait.First(t => t.traitCode == TraitCodes.ProductDescription) };
            ItemTrait posDescription = new ItemTrait { itemID = item.itemID, localeID = 1, traitID = Traits.PosDescription, traitValue = "update subteam pos desc trait", Trait = this.context.Trait.First(t => t.traitCode == TraitCodes.PosDescription) };
            ItemTrait foodStamp = new ItemTrait { itemID = item.itemID, localeID = 1, traitID = Traits.FoodStampEligible, traitValue = "0", Trait = this.context.Trait.First(t => t.traitCode == TraitCodes.FoodStampEligible) };
            ItemTrait posScaleTare = new ItemTrait { itemID = item.itemID, localeID = 1, traitID = Traits.PosScaleTare, traitValue = "0", Trait = this.context.Trait.First(t => t.traitCode == TraitCodes.PosScaleTare) };
            ItemTrait packageUnit = new ItemTrait { itemID = item.itemID, localeID = 1, traitID = Traits.PackageUnit, traitValue = "1", Trait = this.context.Trait.First(t => t.traitCode == TraitCodes.PackageUnit) };

            this.context.ItemTrait.Add(validationDate);
            this.context.ItemTrait.Add(productDescription);
            this.context.ItemTrait.Add(posDescription);
            this.context.ItemTrait.Add(foodStamp);
            this.context.ItemTrait.Add(posScaleTare);
            this.context.ItemTrait.Add(packageUnit);
            this.context.SaveChanges();

            int taxId = this.context.HierarchyClass.First(hc => hc.hierarchyID == Hierarchies.Tax).hierarchyClassID;
            int brandId = this.context.HierarchyClass.First(hc => hc.hierarchyID == Hierarchies.Brands && hc.HierarchyClassTrait.Any(hct => hct.traitID == Traits.SentToEsb)).hierarchyClassID;
            ItemHierarchyClass itemMerch = new ItemHierarchyClass { hierarchyClassID = this.merchLevelFive.hierarchyClassID, itemID = item.itemID, localeID = 1 };
            ItemHierarchyClass itemTax = new ItemHierarchyClass { hierarchyClassID = taxId, itemID = item.itemID, localeID = 1 };
            ItemHierarchyClass itemBrand = new ItemHierarchyClass { hierarchyClassID = brandId, itemID = item.itemID, localeID = 1 };
            this.context.ItemHierarchyClass.Add(itemMerch);
            this.context.ItemHierarchyClass.Add(itemTax);
            this.context.ItemHierarchyClass.Add(itemBrand);
            this.context.SaveChanges();
        }
    }
}
