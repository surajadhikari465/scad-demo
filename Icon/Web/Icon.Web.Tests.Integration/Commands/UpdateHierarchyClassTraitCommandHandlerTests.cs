using Icon.Framework;
using Icon.Testing.Builders;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Icon.Web.Tests.Integration.Commands
{
    [TestClass] [Ignore]
    public class UpdateHierarchyClassTraitCommandHandlerTests
    {
        private IconContext context;
        private UpdateHierarchyClassTraitCommandHandler updateHierarchyClassTraitCommandHandler;
        private UpdateHierarchyClassTraitCommand updateTraitCommand;
        private DbContextTransaction transaction;
        private HierarchyClass merchandiseClassLevelOne;
        private HierarchyClass merchandiseClassLevelTwo;
        private HierarchyClass merchandiseClassLevelThree;
        private HierarchyClass merchandiseClassLevelFour;
        private HierarchyClass merchandiseClassLevelFiveA;
        private HierarchyClass merchandiseClassLevelFiveB;
        private HierarchyClass merchandiseClassLevelFiveC;
        private HierarchyClass taxClass;
        private HierarchyClass brand;
        private HierarchyClass financialClass;
        private Item associatedItem;
        private string testGlAccount;
        private List<string> testScanCodes;

        [TestInitialize]
        public void InitializeData()
        {
            this.context = new IconContext();
            this.updateHierarchyClassTraitCommandHandler = new UpdateHierarchyClassTraitCommandHandler(this.context);
            this.updateTraitCommand = new UpdateHierarchyClassTraitCommand();
            this.testScanCodes = new List<string> { "8428428428", "84284284280", "84284284281" };
            this.testGlAccount = "6101014";

            this.transaction = this.context.Database.BeginTransaction();
        }

        [TestCleanup]
        public void Cleanup()
        {
            this.transaction.Rollback();

            this.context.MessageQueueProduct.RemoveRange(this.context.MessageQueueProduct.Where(mq => this.testScanCodes.Contains(mq.ScanCode)));
            this.context.SaveChanges();
        }

        [TestMethod]
        public void UpdateHierarchyClassTrait_AddGlAccount_ShouldOnlyAddGlAccount()
        {
            // Given.
            AddTestMerchandiseHierarchyClassLevels();

            this.updateTraitCommand.GlAccount = this.testGlAccount;
            this.updateTraitCommand.UpdatedHierarchyClass = merchandiseClassLevelThree;

            // When.
            updateHierarchyClassTraitCommandHandler.Execute(this.updateTraitCommand);

            // Then.
            HierarchyClassTrait actualTrait = this.context.HierarchyClassTrait
                .SingleOrDefault(hct => hct.hierarchyClassID == this.updateTraitCommand.UpdatedHierarchyClass.hierarchyClassID && hct.Trait.traitCode == TraitCodes.GlAccount);
            var entry = this.context.Entry(actualTrait);

            Assert.AreEqual(this.updateTraitCommand.GlAccount, actualTrait.traitValue, "GL Account was not updated as expected.");
            Assert.IsTrue(entry.State == EntityState.Unchanged);
            Assert.IsTrue(this.context.HierarchyClassTrait.Where(hct => hct.hierarchyClassID == merchandiseClassLevelThree.hierarchyClassID).Count() == 1);
            Assert.IsNull(this.context.HierarchyClassTrait
                .SingleOrDefault(hct => hct.hierarchyClassID == merchandiseClassLevelThree.hierarchyClassID && hct.Trait.traitCode == TraitCodes.TaxAbbreviation));
            Assert.IsNull(this.context.HierarchyClassTrait
                .SingleOrDefault(hct => hct.hierarchyClassID == merchandiseClassLevelThree.hierarchyClassID && hct.Trait.traitCode == TraitCodes.MerchFinMapping));
        }

        [TestMethod]
        public void UpdateHierarchyClassTrait_UpdateGlAccountToEmptyString_ShouldDeleteGlAccountTrait()
        {
            // Given.
            AddTestMerchandiseHierarchyClassLevels();

            this.updateTraitCommand.GlAccount = String.Empty;
            this.updateTraitCommand.UpdatedHierarchyClass = merchandiseClassLevelThree;

            // When.
            updateHierarchyClassTraitCommandHandler.Execute(this.updateTraitCommand);

            // Then.
            HierarchyClassTrait actualTrait = this.context.HierarchyClassTrait
                .SingleOrDefault(hct => hct.hierarchyClassID == this.updateTraitCommand.UpdatedHierarchyClass.hierarchyClassID && hct.Trait.traitCode == TraitCodes.GlAccount);
            Assert.IsNull(actualTrait);
        }

        [TestMethod]
        public void UpdateHierarchyClassTrait_AddTaxAbbreviation_ShouldOnlyAddTaxAbbreviation()
        {
            // Given.
            AddTestTaxClass();

            this.updateTraitCommand.TaxAbbreviation = "4444444 TestTaxAbbreviation";
            this.updateTraitCommand.UpdatedHierarchyClass = taxClass;

            // When.
            this.updateHierarchyClassTraitCommandHandler.Execute(this.updateTraitCommand);

            // Then.
            HierarchyClassTrait actualTrait = this.context.HierarchyClassTrait
                .SingleOrDefault(hct => hct.hierarchyClassID == this.updateTraitCommand.UpdatedHierarchyClass.hierarchyClassID && hct.Trait.traitCode == TraitCodes.TaxAbbreviation);
            var entry = this.context.Entry(actualTrait);

            Assert.AreEqual(this.updateTraitCommand.TaxAbbreviation, actualTrait.traitValue, "Tax Abbreviation was not updated as expected.");
            Assert.IsTrue(entry.State == EntityState.Unchanged);
            Assert.IsTrue(context.HierarchyClassTrait.Where(hct => hct.hierarchyClassID == taxClass.hierarchyClassID).Count() == 1);
            Assert.IsNull(this.context.HierarchyClassTrait
                .SingleOrDefault(hct => hct.hierarchyClassID == taxClass.hierarchyClassID && hct.Trait.traitCode == TraitCodes.GlAccount));
            Assert.IsNull(this.context.HierarchyClassTrait
                .SingleOrDefault(hct => hct.hierarchyClassID == taxClass.hierarchyClassID && hct.Trait.traitCode == TraitCodes.MerchFinMapping));
        }

        [TestMethod]
        [ExpectedException(typeof(HierarchyClassTraitUpdateException))]
        public void UpdateHierarchyClassTrait_TaxCodeAlreadyExistsForAnotherTaxClass_ThrowsUpdateHierarchyClassTraitException()
        {
            // Given.
            string existingTaxAbbreviation = this.context.HierarchyClassTrait.First(hct => hct.traitID == Traits.TaxAbbreviation).traitValue;
            string taxCode = existingTaxAbbreviation.Split()[0];

            AddTestTaxClass();

            this.updateTraitCommand.TaxAbbreviation = String.Format("{0} TestTaxAbbreviation", taxCode);
            this.updateTraitCommand.UpdatedHierarchyClass = taxClass;

            // When.
            updateHierarchyClassTraitCommandHandler.Execute(this.updateTraitCommand);

            // Then.
            // Expected exception.
        }

        [TestMethod]
        public void UpdateHierarchyClassTrait_TaxCodeExistsButNotAsFirstSevenDigitsOfString_DoesNotThrowHierarchyClassTraitException()
        {
            // Given.
            string existingTaxAbbreviation = this.context.HierarchyClassTrait.First(hct => hct.traitID == Traits.TaxAbbreviation).traitValue;
            string taxCode = existingTaxAbbreviation.Split()[0];

            AddTestTaxClass();

            this.updateTraitCommand.TaxAbbreviation = String.Format("8484848 TestTaxAbbreviation {0}", taxCode);
            this.updateTraitCommand.UpdatedHierarchyClass = taxClass;

            // When.
            updateHierarchyClassTraitCommandHandler.Execute(this.updateTraitCommand);

            // Then.
            HierarchyClassTrait actualTrait = this.context.HierarchyClassTrait
                .SingleOrDefault(hct => hct.hierarchyClassID == this.updateTraitCommand.UpdatedHierarchyClass.hierarchyClassID && hct.Trait.traitCode == TraitCodes.TaxAbbreviation);
            var entry = this.context.Entry(actualTrait);

            Assert.AreEqual(this.updateTraitCommand.TaxAbbreviation, actualTrait.traitValue, "Tax Abbreviation was not updated as expected.");
            Assert.IsTrue(entry.State == EntityState.Unchanged);
            Assert.IsTrue(context.HierarchyClassTrait.Where(hct => hct.hierarchyClassID == taxClass.hierarchyClassID).Count() == 1);
            Assert.IsNull(this.context.HierarchyClassTrait
                .SingleOrDefault(hct => hct.hierarchyClassID == taxClass.hierarchyClassID && hct.Trait.traitCode == TraitCodes.GlAccount));
            Assert.IsNull(this.context.HierarchyClassTrait
                .SingleOrDefault(hct => hct.hierarchyClassID == taxClass.hierarchyClassID && hct.Trait.traitCode == TraitCodes.MerchFinMapping));
        }

        [TestMethod]
        public void UpdateHierarchyClassTrait_UpdateSubTeamAssociation_ShouldOnlyUpdateSubTeamAssociation()
        {
            // Given.
            AddTestMerchandiseHierarchyClassLevels();

            this.updateTraitCommand.SubTeamHierarchyClassId = this.context.HierarchyClass.First(hc => hc.hierarchyID == Hierarchies.Financial).hierarchyClassID;
            this.updateTraitCommand.UpdatedHierarchyClass = merchandiseClassLevelFiveA;

            var expectedSubTeam = this.context.HierarchyClass.First(hc => hc.hierarchyID == Hierarchies.Financial).hierarchyClassName;
            var expectedClassName = this.merchandiseClassLevelFiveA.hierarchyClassName;

            // When.
            updateHierarchyClassTraitCommandHandler.Execute(this.updateTraitCommand);

            // Then.
            HierarchyClassTrait actualTrait = this.context.HierarchyClassTrait
                .SingleOrDefault(hct => hct.hierarchyClassID == this.updateTraitCommand.UpdatedHierarchyClass.hierarchyClassID && hct.Trait.traitCode == TraitCodes.MerchFinMapping);
            var entry = this.context.Entry(actualTrait);

            Assert.AreEqual(expectedSubTeam, actualTrait.traitValue, "SubTeam was not updated as expected.");
            Assert.IsTrue(entry.State == EntityState.Unchanged);
            Assert.IsTrue(context.HierarchyClassTrait.Where(hct => hct.hierarchyClassID == merchandiseClassLevelFiveA.hierarchyClassID).Count() == 1);
            Assert.IsNull(this.context.HierarchyClassTrait
                .SingleOrDefault(hct => hct.hierarchyClassID == merchandiseClassLevelFiveA.hierarchyClassID && hct.Trait.traitCode == TraitCodes.GlAccount));
            Assert.IsNull(this.context.HierarchyClassTrait
                .SingleOrDefault(hct => hct.hierarchyClassID == merchandiseClassLevelFiveA.hierarchyClassID && hct.Trait.traitCode == TraitCodes.TaxAbbreviation));
        }

        [TestMethod]
        public void UpdateHierarchyClassTrait_AddProhibitDiscountTraitForBrick_ProhibitDiscountTraitShouldBeCreatedForBrickAndEachSubBrick()
        {
            // Given.
            AddTestMerchandiseHierarchyClassLevels();

            this.updateTraitCommand.ProhibitDiscount = "1";
            this.updateTraitCommand.UpdatedHierarchyClass = merchandiseClassLevelFour;

            // When.
            updateHierarchyClassTraitCommandHandler.Execute(this.updateTraitCommand);

            // Then.
            var brickHierarchyClassTrait = this.context.HierarchyClassTrait
                .Single(hct => hct.hierarchyClassID == this.updateTraitCommand.UpdatedHierarchyClass.hierarchyClassID && hct.Trait.traitCode == TraitCodes.ProhibitDiscount);

            var subBricksId = this.context.HierarchyClass.Where(hc => hc.hierarchyParentClassID == this.merchandiseClassLevelFour.hierarchyClassID).Select(hc => hc.hierarchyClassID).ToList();
            var subBrickTraits = this.context.HierarchyClassTrait.Where(hct => hct.Trait.traitCode == TraitCodes.ProhibitDiscount && subBricksId.Contains(hct.hierarchyClassID)).ToList();

            bool subBricksHaveNewTrait = subBrickTraits.TrueForAll(sbt => sbt.traitValue == this.updateTraitCommand.ProhibitDiscount);

            Assert.AreEqual(this.updateTraitCommand.ProhibitDiscount, brickHierarchyClassTrait.traitValue);
            Assert.IsTrue(subBricksHaveNewTrait);
        }

        [TestMethod]
        public void UpdateHierarchyClassTrait_RemoveProhibitDiscountTraitFromBrick_ProhibitDiscountTraitShouldBeRemovedFromBrickAndEachSubBrick()
        {
            // Given.
            AddTestMerchandiseHierarchyClassLevels();

            var existingProhibitDiscountTraits = new List<HierarchyClassTrait>
            {
                new HierarchyClassTrait
                {
                    hierarchyClassID = this.merchandiseClassLevelFour.hierarchyClassID,
                    traitID = Traits.ProhibitDiscount,
                    traitValue = "1"
                },
                new HierarchyClassTrait
                {
                    hierarchyClassID = this.merchandiseClassLevelFiveA.hierarchyClassID,
                    traitID = Traits.ProhibitDiscount,
                    traitValue = "1"
                },
                new HierarchyClassTrait
                {
                    hierarchyClassID = this.merchandiseClassLevelFiveB.hierarchyClassID,
                    traitID = Traits.ProhibitDiscount,
                    traitValue = "1"
                },
                new HierarchyClassTrait
                {
                    hierarchyClassID = this.merchandiseClassLevelFiveC.hierarchyClassID,
                    traitID = Traits.ProhibitDiscount,
                    traitValue = "1"
                },
            };

            this.context.HierarchyClassTrait.AddRange(existingProhibitDiscountTraits);
            this.context.SaveChanges();

            this.updateTraitCommand.UpdatedHierarchyClass = merchandiseClassLevelFour;
            this.updateTraitCommand.ProhibitDiscount = String.Empty;

            // When.
            updateHierarchyClassTraitCommandHandler.Execute(this.updateTraitCommand);

            // Then.
            var brickHierarchyClassTrait = this.context.HierarchyClassTrait
                .SingleOrDefault(hct => hct.hierarchyClassID == this.updateTraitCommand.UpdatedHierarchyClass.hierarchyClassID && hct.Trait.traitCode == TraitCodes.ProhibitDiscount);

            var subBricksId = this.context.HierarchyClass.Where(hc => hc.hierarchyParentClassID == this.merchandiseClassLevelFour.hierarchyClassID).Select(hc => hc.hierarchyClassID).ToList();
            var subBrickTraits = this.context.HierarchyClassTrait.Where(hct => hct.Trait.traitCode == TraitCodes.ProhibitDiscount && subBricksId.Contains(hct.hierarchyClassID)).ToList();

            Assert.IsNull(brickHierarchyClassTrait);
            Assert.AreEqual(0, subBrickTraits.Count);
        }

        [TestMethod]
        public void UpdateHierarchyClassTrait_ProhibitDiscountTraitDoesNotExistForBrick_ProhibitDiscountTraitShouldNotBeCreated()
        {
            // Given.
            AddTestMerchandiseHierarchyClassLevels();

            this.updateTraitCommand.ProhibitDiscount = String.Empty;
            this.updateTraitCommand.UpdatedHierarchyClass = this.merchandiseClassLevelFour;

            // When.
            updateHierarchyClassTraitCommandHandler.Execute(this.updateTraitCommand);

            // Then.
            var brickHierarchyClassTrait = this.context.HierarchyClassTrait
                .SingleOrDefault(hct => hct.hierarchyClassID == this.updateTraitCommand.UpdatedHierarchyClass.hierarchyClassID && hct.Trait.traitCode == TraitCodes.ProhibitDiscount);

            var subBricksId = this.context.HierarchyClass.Where(hc => hc.hierarchyParentClassID == this.merchandiseClassLevelFour.hierarchyClassID).Select(hc => hc.hierarchyClassID).ToList();
            var subBrickTraits = this.context.HierarchyClassTrait.Where(hct => hct.Trait.traitCode == TraitCodes.ProhibitDiscount && subBricksId.Contains(hct.hierarchyClassID)).ToList();

            Assert.IsNull(brickHierarchyClassTrait);
            Assert.AreEqual(0, subBrickTraits.Count);
        }

        [TestMethod]
        public void UpdateHierarchyClassTrait_SubBrickIsUpdated_ProhibitDiscountTraitShouldNotBeRemovedFromSubBrick()
        {
            // Given.
            AddTestMerchandiseHierarchyClassLevels();

            var existingBrickHierarchyClassTrait = new HierarchyClassTrait
            {
                traitID = Traits.ProhibitDiscount,
                traitValue = "1",
                hierarchyClassID = this.merchandiseClassLevelFour.hierarchyClassID
            };

            this.context.HierarchyClassTrait.Add(existingBrickHierarchyClassTrait);

            var existingSubBrickProhibitDiscountTrait = new List<HierarchyClassTrait>
            {
                new HierarchyClassTrait
                {
                    traitID = Traits.ProhibitDiscount,
                    traitValue = "1",
                    hierarchyClassID = this.merchandiseClassLevelFiveA.hierarchyClassID
                },
                new HierarchyClassTrait
                {
                    traitID = Traits.ProhibitDiscount,
                    traitValue = "1",
                    hierarchyClassID = this.merchandiseClassLevelFiveB.hierarchyClassID
                },
                new HierarchyClassTrait
                {
                    traitID = Traits.ProhibitDiscount,
                    traitValue = "1",
                    hierarchyClassID = this.merchandiseClassLevelFiveC.hierarchyClassID
                }
            };

            this.context.HierarchyClassTrait.AddRange(existingSubBrickProhibitDiscountTrait);
            this.context.SaveChanges();

            this.updateTraitCommand.UpdatedHierarchyClass = this.merchandiseClassLevelFiveA;
            this.updateTraitCommand.ProhibitDiscount = String.Empty;

            // When.
            updateHierarchyClassTraitCommandHandler.Execute(this.updateTraitCommand);

            // Then.
            var brickHierarchyClassTrait = this.context.HierarchyClassTrait
                .SingleOrDefault(hct => hct.hierarchyClassID == this.merchandiseClassLevelFour.hierarchyClassID && hct.Trait.traitCode == TraitCodes.ProhibitDiscount);

            var subBricksById = this.context.HierarchyClass.Where(hc => hc.hierarchyParentClassID == this.merchandiseClassLevelFour.hierarchyClassID).Select(hc => hc.hierarchyClassID).ToList();
            var subBrickTraits = this.context.HierarchyClassTrait.Where(hct => hct.Trait.traitCode == TraitCodes.ProhibitDiscount && subBricksById.Contains(hct.hierarchyClassID)).ToList();

            Assert.IsNotNull(brickHierarchyClassTrait);
            Assert.AreEqual(3, subBrickTraits.Count);
        }

        [TestMethod]
        public void UpdateHierarchyClassTrait_AddNonMerchandiseTraitForSubBrick_NonMerchandiseTraitIsCreated()
        {
            // Given.
            AddTestMerchandiseHierarchyClassLevels();

            this.updateTraitCommand.NonMerchandiseTrait = NonMerchandiseTraits.BottleDeposit;
            this.updateTraitCommand.SubTeamHierarchyClassId = this.context.HierarchyClass.First(hc => hc.hierarchyID == Hierarchies.Financial).hierarchyClassID;
            this.updateTraitCommand.UpdatedHierarchyClass = merchandiseClassLevelFiveA;

            var expectedNonMerchandise = NonMerchandiseTraits.BottleDeposit;

            // When.
            updateHierarchyClassTraitCommandHandler.Execute(this.updateTraitCommand);

            // Then.
            HierarchyClassTrait actualTrait = this.context.HierarchyClassTrait
                .Single(hct => hct.hierarchyClassID == this.updateTraitCommand.UpdatedHierarchyClass.hierarchyClassID && hct.Trait.traitCode == TraitCodes.NonMerchandise);
            var entry = this.context.Entry(actualTrait);

            Assert.AreEqual(expectedNonMerchandise, actualTrait.traitValue);
            Assert.IsTrue(entry.State == EntityState.Unchanged);
            Assert.IsNull(this.context.HierarchyClassTrait
                .SingleOrDefault(hct => hct.hierarchyClassID == merchandiseClassLevelFiveA.hierarchyClassID && hct.Trait.traitCode == TraitCodes.GlAccount));
            Assert.IsNull(this.context.HierarchyClassTrait
                .SingleOrDefault(hct => hct.hierarchyClassID == merchandiseClassLevelFiveA.hierarchyClassID && hct.Trait.traitCode == TraitCodes.TaxAbbreviation));
        }

        [TestMethod]
        public void UpdateHierarchyClassTrait_UpdateNonMerchandiseTraitForSubBrick_NonMerchandiseTraitShouldBeUpdated()
        {
            // Given.
            AddTestMerchandiseHierarchyClassLevels();

            this.updateTraitCommand.UpdatedHierarchyClass = merchandiseClassLevelFiveA;

            HierarchyClassTrait nonMerchTrait = new HierarchyClassTrait
            {
                traitValue = NonMerchandiseTraits.Coupon,
                hierarchyClassID = this.updateTraitCommand.UpdatedHierarchyClass.hierarchyClassID,
                traitID = Traits.NonMerchandise
            };

            this.context.HierarchyClassTrait.Add(nonMerchTrait);
            this.context.SaveChanges();

            this.updateTraitCommand.NonMerchandiseTrait = NonMerchandiseTraits.BottleDeposit;
            this.updateTraitCommand.SubTeamHierarchyClassId = this.context.HierarchyClass.First(hc => hc.hierarchyID == Hierarchies.Financial).hierarchyClassID;

            var expectedNonMerchandise = NonMerchandiseTraits.BottleDeposit;

            // When.
            updateHierarchyClassTraitCommandHandler.Execute(this.updateTraitCommand);

            // Then.
            HierarchyClassTrait actualTrait = this.context.HierarchyClassTrait
                .Single(hct => hct.hierarchyClassID == this.updateTraitCommand.UpdatedHierarchyClass.hierarchyClassID && hct.Trait.traitCode == TraitCodes.NonMerchandise);
            var entry = this.context.Entry(actualTrait);

            Assert.AreEqual(expectedNonMerchandise, actualTrait.traitValue);
            Assert.IsTrue(entry.State == EntityState.Unchanged);
            Assert.IsNull(this.context.HierarchyClassTrait
                .SingleOrDefault(hct => hct.hierarchyClassID == merchandiseClassLevelFiveA.hierarchyClassID && hct.Trait.traitCode == TraitCodes.GlAccount));
            Assert.IsNull(this.context.HierarchyClassTrait
                .SingleOrDefault(hct => hct.hierarchyClassID == merchandiseClassLevelFiveA.hierarchyClassID && hct.Trait.traitCode == TraitCodes.TaxAbbreviation));
        }

        [TestMethod]
        public void UpdateHierarchyClassTrait_RemoveNonMerchandiseTraitFromSubBrick_NonMerchandiseTraitShouldBeRemovedFromSubBrick()
        {
            // Given.
            AddTestMerchandiseHierarchyClassLevels();

            this.updateTraitCommand.UpdatedHierarchyClass = merchandiseClassLevelFiveA;

            HierarchyClassTrait nonMerchTrait = new HierarchyClassTrait
            {
                traitValue = NonMerchandiseTraits.Crv,
                hierarchyClassID = this.updateTraitCommand.UpdatedHierarchyClass.hierarchyClassID,
                traitID = Traits.NonMerchandise
            };

            this.context.HierarchyClassTrait.Add(nonMerchTrait);
            this.context.SaveChanges();

            this.updateTraitCommand.NonMerchandiseTrait = String.Empty;
            this.updateTraitCommand.SubTeamHierarchyClassId = this.context.HierarchyClass.First(hc => hc.hierarchyID == Hierarchies.Financial).hierarchyClassID;

            // When.
            updateHierarchyClassTraitCommandHandler.Execute(this.updateTraitCommand);

            // Then.
            HierarchyClassTrait actualTrait = this.context.HierarchyClassTrait
                .SingleOrDefault(hct => hct.hierarchyClassID == this.updateTraitCommand.UpdatedHierarchyClass.hierarchyClassID && hct.Trait.traitCode == TraitCodes.NonMerchandise);

            Assert.IsNull(actualTrait);
            Assert.IsNull(this.context.HierarchyClassTrait
                .SingleOrDefault(hct => hct.hierarchyClassID == merchandiseClassLevelFiveA.hierarchyClassID && hct.Trait.traitCode == TraitCodes.GlAccount));
            Assert.IsNull(this.context.HierarchyClassTrait
                .SingleOrDefault(hct => hct.hierarchyClassID == merchandiseClassLevelFiveA.hierarchyClassID && hct.Trait.traitCode == TraitCodes.TaxAbbreviation));
        }

        [TestMethod]
        public void UpdateHierarchyClassTrait_RemoveNonMerchandiseTraitFromSubBrick_AssociatedItemsShouldBeOfTypeRetailSale()
        {
            // Given.
            AddTestMerchandiseHierarchyClassLevels();
            AddItemAssociationToSubBrick();

            this.updateTraitCommand.UpdatedHierarchyClass = merchandiseClassLevelFiveA;
            this.associatedItem.itemTypeID = this.context.ItemType.First(it => it.itemTypeCode == ItemTypeCodes.Deposit).itemTypeID;

            HierarchyClassTrait nonMerchTrait = new HierarchyClassTrait
            {
                traitValue = NonMerchandiseTraits.Crv,
                hierarchyClassID = this.updateTraitCommand.UpdatedHierarchyClass.hierarchyClassID,
                traitID = Traits.NonMerchandise
            };

            this.context.HierarchyClassTrait.Add(nonMerchTrait);
            this.context.SaveChanges();

            this.updateTraitCommand.NonMerchandiseTrait = String.Empty;
            this.updateTraitCommand.SubTeamHierarchyClassId = this.context.HierarchyClass.First(hc => hc.hierarchyID == Hierarchies.Financial).hierarchyClassID;

            // When.
            updateHierarchyClassTraitCommandHandler.Execute(this.updateTraitCommand);

            // Then.
            context.Entry(this.associatedItem).Reload();
            int expectedTypeId = this.context.ItemType.Single(it => it.itemTypeCode == ItemTypeCodes.RetailSale).itemTypeID;
            int actualTypeId = this.associatedItem.itemTypeID;

            Assert.AreEqual(expectedTypeId, actualTypeId);
        }

        [TestMethod]
        public void UpdateHierarchyClassTrait_UpdateNonMerchandiseTraitToBottleDeposit_AssociatedItemsShouldBeOfTypeDeposit()
        {
            // Given.
            AddTestMerchandiseHierarchyClassLevels();
            AddItemAssociationToSubBrick();

            this.updateTraitCommand.UpdatedHierarchyClass = merchandiseClassLevelFiveA;

            HierarchyClassTrait nonMerchTrait = new HierarchyClassTrait
            {
                traitValue = NonMerchandiseTraits.Coupon,
                hierarchyClassID = this.updateTraitCommand.UpdatedHierarchyClass.hierarchyClassID,
                traitID = Traits.NonMerchandise
            };

            this.context.HierarchyClassTrait.Add(nonMerchTrait);
            this.context.SaveChanges();

            this.updateTraitCommand.NonMerchandiseTrait = NonMerchandiseTraits.BottleDeposit;
            this.updateTraitCommand.SubTeamHierarchyClassId = this.context.HierarchyClass.First(hc => hc.hierarchyID == Hierarchies.Financial).hierarchyClassID;

            // When.
            updateHierarchyClassTraitCommandHandler.Execute(this.updateTraitCommand);

            // Then.
            context.Entry(this.associatedItem).Reload();
            int expectedTypeId = this.context.ItemType.Single(it => it.itemTypeCode == ItemTypeCodes.Deposit).itemTypeID;
            int actualTypeId = this.associatedItem.itemTypeID;

            Assert.AreEqual(expectedTypeId, actualTypeId);
        }

        [TestMethod]
        public void UpdateHierarchyClassTrait_UpdateNonMerchandiseTraitToCrv_AssociatedItemsShouldBeOfTypeDeposit()
        {
            // Given.
            AddTestMerchandiseHierarchyClassLevels();
            AddItemAssociationToSubBrick();

            this.updateTraitCommand.UpdatedHierarchyClass = merchandiseClassLevelFiveA;

            HierarchyClassTrait nonMerchTrait = new HierarchyClassTrait
            {
                traitValue = NonMerchandiseTraits.Coupon,
                hierarchyClassID = this.updateTraitCommand.UpdatedHierarchyClass.hierarchyClassID,
                traitID = Traits.NonMerchandise
            };

            this.context.HierarchyClassTrait.Add(nonMerchTrait);
            this.context.SaveChanges();

            this.updateTraitCommand.NonMerchandiseTrait = NonMerchandiseTraits.Crv;
            this.updateTraitCommand.SubTeamHierarchyClassId = this.context.HierarchyClass.First(hc => hc.hierarchyID == Hierarchies.Financial).hierarchyClassID;

            // When.
            updateHierarchyClassTraitCommandHandler.Execute(this.updateTraitCommand);

            // Then.
            context.Entry(this.associatedItem).Reload();
            int expectedTypeId = this.context.ItemType.Single(it => it.itemTypeCode == ItemTypeCodes.Deposit).itemTypeID;
            int actualTypeId = this.associatedItem.itemTypeID;

            Assert.AreEqual(expectedTypeId, actualTypeId);
        }

        [TestMethod]
        public void UpdateHierarchyClassTrait_UpdateNonMerchandiseTraitToBottleReturn_AssociatedItemsShouldBeOfTypeReturn()
        {
            // Given.
            AddTestMerchandiseHierarchyClassLevels();
            AddItemAssociationToSubBrick();

            this.updateTraitCommand.UpdatedHierarchyClass = merchandiseClassLevelFiveA;

            HierarchyClassTrait nonMerchTrait = new HierarchyClassTrait
            {
                traitValue = NonMerchandiseTraits.Coupon,
                hierarchyClassID = this.updateTraitCommand.UpdatedHierarchyClass.hierarchyClassID,
                traitID = Traits.NonMerchandise
            };

            this.context.HierarchyClassTrait.Add(nonMerchTrait);
            this.context.SaveChanges();

            this.updateTraitCommand.NonMerchandiseTrait = NonMerchandiseTraits.BottleReturn;
            this.updateTraitCommand.SubTeamHierarchyClassId = this.context.HierarchyClass.First(hc => hc.hierarchyID == Hierarchies.Financial).hierarchyClassID;

            // When.
            updateHierarchyClassTraitCommandHandler.Execute(this.updateTraitCommand);

            // Then.
            context.Entry(this.associatedItem).Reload();
            int expectedTypeId = this.context.ItemType.Single(it => it.itemTypeCode == ItemTypeCodes.Return).itemTypeID;
            int actualTypeId = this.associatedItem.itemTypeID;

            Assert.AreEqual(expectedTypeId, actualTypeId);
        }

        [TestMethod]
        public void UpdateHierarchyClassTrait_UpdateNonMerchandiseTraitToCrvCredit_AssociatedItemsShouldBeOfTypeReturn()
        {
            // Given.
            AddTestMerchandiseHierarchyClassLevels();
            AddItemAssociationToSubBrick();

            this.updateTraitCommand.UpdatedHierarchyClass = merchandiseClassLevelFiveA;

            HierarchyClassTrait nonMerchTrait = new HierarchyClassTrait
            {
                traitValue = NonMerchandiseTraits.Coupon,
                hierarchyClassID = this.updateTraitCommand.UpdatedHierarchyClass.hierarchyClassID,
                traitID = Traits.NonMerchandise
            };

            this.context.HierarchyClassTrait.Add(nonMerchTrait);
            this.context.SaveChanges();

            this.updateTraitCommand.NonMerchandiseTrait = NonMerchandiseTraits.CrvCredit;
            this.updateTraitCommand.SubTeamHierarchyClassId = this.context.HierarchyClass.First(hc => hc.hierarchyID == Hierarchies.Financial).hierarchyClassID;

            // When.
            updateHierarchyClassTraitCommandHandler.Execute(this.updateTraitCommand);

            // Then.
            context.Entry(this.associatedItem).Reload();
            int expectedTypeId = this.context.ItemType.Single(it => it.itemTypeCode == ItemTypeCodes.Return).itemTypeID;
            int actualTypeId = this.associatedItem.itemTypeID;

            Assert.AreEqual(expectedTypeId, actualTypeId);
        }

        [TestMethod]
        public void UpdateHierarchyClassTrait_UpdateNonMerchandiseTraitToLegacyPosOnly_AssociatedItemsShouldBeOfTypeNonRetail()
        {
            // Given.
            AddTestMerchandiseHierarchyClassLevels();
            AddItemAssociationToSubBrick();

            this.updateTraitCommand.UpdatedHierarchyClass = merchandiseClassLevelFiveA;

            HierarchyClassTrait nonMerchTrait = new HierarchyClassTrait
            {
                traitValue = NonMerchandiseTraits.Coupon,
                hierarchyClassID = this.updateTraitCommand.UpdatedHierarchyClass.hierarchyClassID,
                traitID = Traits.NonMerchandise
            };

            this.context.HierarchyClassTrait.Add(nonMerchTrait);
            this.context.SaveChanges();

            this.updateTraitCommand.NonMerchandiseTrait = NonMerchandiseTraits.LegacyPosOnly;
            this.updateTraitCommand.SubTeamHierarchyClassId = this.context.HierarchyClass.First(hc => hc.hierarchyID == Hierarchies.Financial).hierarchyClassID;

            // When.
            updateHierarchyClassTraitCommandHandler.Execute(this.updateTraitCommand);

            // Then.
            context.Entry(this.associatedItem).Reload();
            int expectedTypeId = this.context.ItemType.Single(it => it.itemTypeCode == ItemTypeCodes.NonRetail).itemTypeID;
            int actualTypeId = this.associatedItem.itemTypeID;

            Assert.AreEqual(expectedTypeId, actualTypeId);
        }

        [TestMethod]
        public void UpdateHierarchyClassTrait_UpdateNonMerchandiseTraitToBlackhawkFee_AssociatedItemsShouldBeOfTypeFee()
        {
            // Given.
            AddTestMerchandiseHierarchyClassLevels();
            AddItemAssociationToSubBrick();

            this.updateTraitCommand.UpdatedHierarchyClass = merchandiseClassLevelFiveA;

            HierarchyClassTrait nonMerchTrait = new HierarchyClassTrait
            {
                traitValue = NonMerchandiseTraits.Coupon,
                hierarchyClassID = this.updateTraitCommand.UpdatedHierarchyClass.hierarchyClassID,
                traitID = Traits.NonMerchandise
            };

            this.context.HierarchyClassTrait.Add(nonMerchTrait);
            this.context.SaveChanges();

            this.updateTraitCommand.NonMerchandiseTrait = NonMerchandiseTraits.BlackhawkFee;
            this.updateTraitCommand.SubTeamHierarchyClassId = this.context.HierarchyClass.First(hc => hc.hierarchyID == Hierarchies.Financial).hierarchyClassID;

            // When.
            updateHierarchyClassTraitCommandHandler.Execute(this.updateTraitCommand);

            // Then.
            context.Entry(this.associatedItem).Reload();
            int expectedTypeId = this.context.ItemType.Single(it => it.itemTypeCode == ItemTypeCodes.Fee).itemTypeID;
            int actualTypeId = this.associatedItem.itemTypeID;

            Assert.AreEqual(expectedTypeId, actualTypeId);
        }

        [TestMethod]
        public void UpdateHierarchyClassTrait_UpdateNonMerchandiseTrait_ModifiedDateTraitShouldBeUpdatedForAssociatedItems()
        {
            // Given.
            string now = DateTime.Now.ToString();

            AddTestMerchandiseHierarchyClassLevels();
            AddItemAssociationToSubBrick();

            this.updateTraitCommand.UpdatedHierarchyClass = merchandiseClassLevelFiveA;

            HierarchyClassTrait nonMerchTrait = new HierarchyClassTrait
            {
                traitValue = NonMerchandiseTraits.Coupon,
                hierarchyClassID = this.updateTraitCommand.UpdatedHierarchyClass.hierarchyClassID,
                traitID = Traits.NonMerchandise
            };

            this.context.HierarchyClassTrait.Add(nonMerchTrait);
            this.context.SaveChanges();

            this.updateTraitCommand.NonMerchandiseTrait = NonMerchandiseTraits.Crv;

            // When.
            updateHierarchyClassTraitCommandHandler.Execute(this.updateTraitCommand);

            // Then.
            ItemTrait itemTrait = this.context.ItemTrait.SingleOrDefault(it => it.itemID == this.associatedItem.itemID && it.traitID == Traits.ModifiedDate);

            this.context.Entry(itemTrait).Reload();
            string actualModifiedDate = itemTrait.traitValue;
                        
            Assert.IsNotNull(actualModifiedDate);
            Assert.IsTrue(Convert.ToDateTime(actualModifiedDate) > Convert.ToDateTime(now));
        }

        [TestMethod]
        public void UpdateHierarchyClassTrait_NoUpdateToNonMerchandiseTrait_NoUpdateShouldOccurr()
        {
            // Given.
            AddTestMerchandiseHierarchyClassLevels();
            AddItemAssociationToSubBrick();

            this.updateTraitCommand.UpdatedHierarchyClass = merchandiseClassLevelFiveA;

            HierarchyClassTrait nonMerchTrait = new HierarchyClassTrait
            {
                traitValue = NonMerchandiseTraits.BottleReturn,
                hierarchyClassID = this.updateTraitCommand.UpdatedHierarchyClass.hierarchyClassID,
                traitID = Traits.NonMerchandise
            };

            this.context.HierarchyClassTrait.Add(nonMerchTrait);
            this.context.SaveChanges();

            this.updateTraitCommand.NonMerchandiseTrait = NonMerchandiseTraits.BottleReturn;
            this.updateTraitCommand.SubTeamHierarchyClassId = this.context.HierarchyClass.First(hc => hc.hierarchyID == Hierarchies.Financial).hierarchyClassID;

            // When.
            updateHierarchyClassTraitCommandHandler.Execute(this.updateTraitCommand);

            // Then.
            HierarchyClassTrait actualTrait = this.context.HierarchyClassTrait
                .SingleOrDefault(hct => hct.hierarchyClassID == this.updateTraitCommand.UpdatedHierarchyClass.hierarchyClassID && hct.Trait.traitCode == TraitCodes.NonMerchandise);
            var entry = this.context.Entry(actualTrait);

            Assert.AreEqual(NonMerchandiseTraits.BottleReturn, actualTrait.traitValue);
            Assert.IsTrue(entry.State == EntityState.Unchanged);
        }

        [TestMethod]
        public void UpdateHierarchyClassTrait_NonMerchandiseTraitRemainsEmptyString_NonMerchandiseTraitShouldNotBeCreated()
        {
            // Given.
            AddTestMerchandiseHierarchyClassLevels();
            AddItemAssociationToSubBrick();

            this.updateTraitCommand.UpdatedHierarchyClass = merchandiseClassLevelFiveA;
            this.updateTraitCommand.NonMerchandiseTrait = String.Empty;
            this.updateTraitCommand.SubTeamHierarchyClassId = 0;

            // When.
            updateHierarchyClassTraitCommandHandler.Execute(this.updateTraitCommand);

            // Then.
            HierarchyClassTrait actualTrait = this.context.HierarchyClassTrait
                .SingleOrDefault(hct => hct.hierarchyClassID == this.updateTraitCommand.UpdatedHierarchyClass.hierarchyClassID && hct.Trait.traitCode == TraitCodes.NonMerchandise);
            Assert.IsNull(actualTrait);
        }

        [TestMethod]
        public void UpdateHierarchyClassTrait_NonMerchandiseTraitAssignedToSubBrick_ProductMessageShouldBeGeneratedForEachItemAssocaitedToTheSubBrick()
        {
            // Given.
            DateTime now = DateTime.Now;
            AddTestMerchandiseHierarchyClassLevels();
            AddTestTaxClass();
            AddTestBrand();

            var testItems = new List<Item>
            {
                new TestItemBuilder()
                    .WithValidationDate(DateTime.Now.ToString())
                    .WithScanCode(this.testScanCodes[0])
                    .WithSubBrickAssociation(this.merchandiseClassLevelFiveA.hierarchyClassID)
                    .WithTaxClassAssociation(this.taxClass.hierarchyClassID)
                    .WithBrandAssociation(this.brand.hierarchyClassID),

                new TestItemBuilder()
                    .WithValidationDate(DateTime.Now.ToString())
                    .WithScanCode(this.testScanCodes[1])
                    .WithSubBrickAssociation(this.merchandiseClassLevelFiveA.hierarchyClassID)
                    .WithTaxClassAssociation(this.taxClass.hierarchyClassID)
                    .WithBrandAssociation(this.brand.hierarchyClassID),

                new TestItemBuilder()
                    .WithValidationDate(DateTime.Now.ToString())
                    .WithScanCode(this.testScanCodes[2])
                    .WithSubBrickAssociation(this.merchandiseClassLevelFiveA.hierarchyClassID)
                    .WithTaxClassAssociation(this.taxClass.hierarchyClassID)
                    .WithBrandAssociation(this.brand.hierarchyClassID)
            };

            this.context.Item.AddRange(testItems);
            this.context.SaveChanges();

            this.updateTraitCommand.UpdatedHierarchyClass = this.merchandiseClassLevelFiveA;
            this.updateTraitCommand.NonMerchandiseTrait = NonMerchandiseTraits.Crv;

            // When.
            this.updateHierarchyClassTraitCommandHandler.Execute(this.updateTraitCommand);

            // Then.
            var generatedMessages = this.context.MessageQueueProduct.Where(mq => testScanCodes.Contains(mq.ScanCode)).ToList();
            bool allMessagesAreTypeDeposit = generatedMessages.TrueForAll(m => m.ItemTypeCode == ItemTypeCodes.Deposit);
            var testItem = testItems[0];

            var testItem1BrandClass = testItem.ItemHierarchyClass.Single(ihc => ihc.HierarchyClass.hierarchyID == Hierarchies.Brands).HierarchyClass;
            var testItem1BrowsingClass = testItem.ItemHierarchyClass.SingleOrDefault(ihc => ihc.HierarchyClass.hierarchyID == Hierarchies.Browsing);
            var testItem1MerchandiseClass = testItem.ItemHierarchyClass.Single(ihc => ihc.HierarchyClass.hierarchyID == Hierarchies.Merchandise).HierarchyClass;
            var testItem1TaxClass = testItem.ItemHierarchyClass.Single(ihc => ihc.HierarchyClass.hierarchyID == Hierarchies.Tax).HierarchyClass;
            string testItem1MerchFinMapping = testItem1MerchandiseClass.HierarchyClassTrait.Single(hct => hct.traitID == Traits.MerchFinMapping).traitValue;
            var testItem1FinancialClass = this.context.HierarchyClass.Single(hc => hc.hierarchyID == Hierarchies.Financial && hc.hierarchyClassName == testItem1MerchFinMapping);

            Assert.AreEqual(testItems.Count, generatedMessages.Count);
            Assert.IsTrue(allMessagesAreTypeDeposit);
            Assert.AreEqual(MessageTypes.Product, generatedMessages[0].MessageTypeId);
            Assert.AreEqual(MessageStatusTypes.Staged, generatedMessages[0].MessageStatusId);
            Assert.IsNull(generatedMessages[0].MessageHistoryId);
            Assert.AreEqual(now.Date.ToShortDateString(), generatedMessages[0].InsertDate.Date.ToShortDateString());
            Assert.AreEqual(testItem.itemID, generatedMessages[0].ItemId);
            Assert.AreEqual(Locales.WholeFoods, generatedMessages[0].LocaleId);
            Assert.AreEqual(ItemTypeDescriptions.Deposit, generatedMessages[0].ItemTypeDesc);
            Assert.AreEqual(testItem.ScanCode.Single().scanCodeID, generatedMessages[0].ScanCodeId);
            Assert.AreEqual(testScanCodes[0], generatedMessages[0].ScanCode);
            Assert.AreEqual(ScanCodeTypes.Upc, generatedMessages[0].ScanCodeTypeId);
            Assert.AreEqual(ScanCodeTypeDescriptions.Upc, generatedMessages[0].ScanCodeTypeDesc);
            Assert.AreEqual(testItem.ItemTrait.Single(it => it.traitID == Traits.ProductDescription).traitValue, generatedMessages[0].ProductDescription);
            Assert.AreEqual(testItem.ItemTrait.Single(it => it.traitID == Traits.PosDescription).traitValue, generatedMessages[0].PosDescription);
            Assert.AreEqual(testItem.ItemTrait.Single(it => it.traitID == Traits.PackageUnit).traitValue, generatedMessages[0].PackageUnit);
            Assert.AreEqual(testItem.ItemTrait.Single(it => it.traitID == Traits.RetailSize).traitValue, generatedMessages[0].RetailSize);
            Assert.AreEqual(testItem.ItemTrait.Single(it => it.traitID == Traits.RetailUom).traitValue, generatedMessages[0].RetailUom);
            Assert.AreEqual(testItem.ItemTrait.Single(it => it.traitID == Traits.FoodStampEligible).traitValue, generatedMessages[0].FoodStampEligible);
            Assert.IsFalse(generatedMessages[0].ProhibitDiscount);
            Assert.AreEqual("0", generatedMessages[0].DepartmentSale);
            Assert.IsNull(testItem.ItemTrait.SingleOrDefault(it => it.traitID == Traits.DepartmentSale));
            Assert.AreEqual(testItem1BrandClass.hierarchyClassID, generatedMessages[0].BrandId);
            Assert.AreEqual(testItem1BrandClass.hierarchyClassName, generatedMessages[0].BrandName);
            Assert.AreEqual(testItem1BrandClass.hierarchyLevel, generatedMessages[0].BrandLevel);
            Assert.AreEqual(testItem1BrandClass.hierarchyParentClassID, generatedMessages[0].BrandParentId);
            Assert.IsNull(generatedMessages[0].BrowsingClassId);
            Assert.IsNull(generatedMessages[0].BrowsingClassName);
            Assert.IsNull(generatedMessages[0].BrowsingLevel);
            Assert.IsNull(generatedMessages[0].BrowsingParentId);
            Assert.AreEqual(testItem1MerchandiseClass.hierarchyClassID, generatedMessages[0].MerchandiseClassId);
            Assert.AreEqual(testItem1MerchandiseClass.hierarchyClassName, generatedMessages[0].MerchandiseClassName);
            Assert.AreEqual(testItem1MerchandiseClass.hierarchyLevel, generatedMessages[0].MerchandiseLevel);
            Assert.AreEqual(testItem1MerchandiseClass.hierarchyParentClassID, generatedMessages[0].MerchandiseParentId);
            Assert.AreEqual(testItem1TaxClass.hierarchyClassID, generatedMessages[0].TaxClassId);
            Assert.AreEqual(testItem1TaxClass.hierarchyClassName, generatedMessages[0].TaxClassName);
            Assert.AreEqual(testItem1TaxClass.hierarchyLevel, generatedMessages[0].TaxLevel);
            Assert.AreEqual(testItem1TaxClass.hierarchyParentClassID, generatedMessages[0].TaxParentId);
            Assert.AreEqual(testItem1FinancialClass.hierarchyClassName.Split('(')[1].Trim(')'), generatedMessages[0].FinancialClassId);
            Assert.AreEqual(testItem1FinancialClass.hierarchyClassName, generatedMessages[0].FinancialClassName);
            Assert.AreEqual(testItem1FinancialClass.hierarchyLevel, generatedMessages[0].FinancialLevel);
            Assert.AreEqual(testItem1FinancialClass.hierarchyParentClassID, generatedMessages[0].FinancialParentId);
            Assert.IsNull(generatedMessages[0].InProcessBy);
            Assert.IsNull(generatedMessages[0].ProcessedDate);
        }

        [TestMethod]
        public void UpdateHierarchyClassTrait_ProhibitDiscountTraitAssignedToBrick_ProductMessageShouldBeGeneratedForEachItemAssocaitedToTheSubBricksOfTheBrick()
        {
            // Given.
            AddTestMerchandiseHierarchyClassLevels();
            AddTestTaxClass();
            AddTestBrand();

            var testItems = new List<Item>
            {
                new TestItemBuilder()
                    .WithValidationDate(DateTime.Now.ToString())
                    .WithScanCode(this.testScanCodes[0])
                    .WithSubBrickAssociation(this.merchandiseClassLevelFiveB.hierarchyClassID)
                    .WithTaxClassAssociation(this.taxClass.hierarchyClassID)
                    .WithBrandAssociation(this.brand.hierarchyClassID),

                new TestItemBuilder()
                    .WithValidationDate(DateTime.Now.ToString())
                    .WithScanCode(this.testScanCodes[1])
                    .WithSubBrickAssociation(this.merchandiseClassLevelFiveB.hierarchyClassID)
                    .WithTaxClassAssociation(this.taxClass.hierarchyClassID)
                    .WithBrandAssociation(this.brand.hierarchyClassID),

                new TestItemBuilder()
                    .WithValidationDate(DateTime.Now.ToString())
                    .WithScanCode(this.testScanCodes[2])
                    .WithSubBrickAssociation(this.merchandiseClassLevelFiveB.hierarchyClassID)
                    .WithTaxClassAssociation(this.taxClass.hierarchyClassID)
                    .WithBrandAssociation(this.brand.hierarchyClassID)
            };

            this.context.Item.AddRange(testItems);
            this.context.SaveChanges();

            this.updateTraitCommand.UpdatedHierarchyClass = this.merchandiseClassLevelFour;
            this.updateTraitCommand.ProhibitDiscount = "1";

            // When.
            this.updateHierarchyClassTraitCommandHandler.Execute(this.updateTraitCommand);

            // Then.
            var generatedMessages = this.context.MessageQueueProduct.Where(mq => this.testScanCodes.Contains(mq.ScanCode)).ToList();

            bool eachMessageIsRetailSale = generatedMessages.TrueForAll(m => m.ItemTypeCode == ItemTypeCodes.RetailSale);
            bool eachMessageIsProhibitDiscount = generatedMessages.TrueForAll(m => m.ProhibitDiscount == true);

            Assert.AreEqual(testItems.Count, generatedMessages.Count);
            Assert.IsTrue(eachMessageIsRetailSale);
            Assert.IsTrue(eachMessageIsProhibitDiscount);
        }

        [TestMethod]
        public void UpdateHierarchyClassTrait_NonMerchandiseTraitValueIsCoupon_NoProductMessagesShouldBeGenerated()
        {
            // Given.
            DateTime now = DateTime.Now;

            AddTestMerchandiseHierarchyClassLevels();
            AddTestTaxClass();
            AddTestBrand();

            var testItems = new List<Item>
            {
                new TestItemBuilder()
                    .WithValidationDate(DateTime.Now.ToString())
                    .WithScanCode(this.testScanCodes[0])
                    .WithSubBrickAssociation(this.merchandiseClassLevelFiveA.hierarchyClassID)
                    .WithTaxClassAssociation(this.taxClass.hierarchyClassID)
                    .WithBrandAssociation(this.brand.hierarchyClassID),

                new TestItemBuilder()
                    .WithValidationDate(DateTime.Now.ToString())
                    .WithScanCode(this.testScanCodes[1])
                    .WithSubBrickAssociation(this.merchandiseClassLevelFiveA.hierarchyClassID)
                    .WithTaxClassAssociation(this.taxClass.hierarchyClassID)
                    .WithBrandAssociation(this.brand.hierarchyClassID),

                new TestItemBuilder()
                    .WithValidationDate(DateTime.Now.ToString())
                    .WithScanCode(this.testScanCodes[2])
                    .WithSubBrickAssociation(this.merchandiseClassLevelFiveA.hierarchyClassID)
                    .WithTaxClassAssociation(this.taxClass.hierarchyClassID)
                    .WithBrandAssociation(this.brand.hierarchyClassID)  
            };

            this.context.Item.AddRange(testItems);
            this.context.SaveChanges();

            this.updateTraitCommand.UpdatedHierarchyClass = this.merchandiseClassLevelFiveA;
            this.updateTraitCommand.NonMerchandiseTrait = NonMerchandiseTraits.Coupon;

            // When.
            this.updateHierarchyClassTraitCommandHandler.Execute(this.updateTraitCommand);

            // Then.
            var generatedMessages = this.context.MessageQueueProduct.Where(mq => mq.InsertDate > now && testScanCodes.Contains(mq.ScanCode)).ToList();
            Assert.IsTrue(generatedMessages.Count == 0);
        }

        [TestMethod]
        public void UpdateHierarchyClassTrait_NonMerchandiseTraitValueIsLegacyPosOnly_NoProductMessagesShouldBeGenerated()
        {
            // Given.
            DateTime now = DateTime.Now;

            AddTestMerchandiseHierarchyClassLevels();
            AddTestTaxClass();
            AddTestBrand();

            var testItems = new List<Item>
            {
                new TestItemBuilder()
                    .WithValidationDate(DateTime.Now.ToString())
                    .WithScanCode(this.testScanCodes[0])
                    .WithSubBrickAssociation(this.merchandiseClassLevelFiveA.hierarchyClassID)
                    .WithTaxClassAssociation(this.taxClass.hierarchyClassID)
                    .WithBrandAssociation(this.brand.hierarchyClassID),

                new TestItemBuilder()
                    .WithValidationDate(DateTime.Now.ToString())
                    .WithScanCode(this.testScanCodes[1])
                    .WithSubBrickAssociation(this.merchandiseClassLevelFiveA.hierarchyClassID)
                    .WithTaxClassAssociation(this.taxClass.hierarchyClassID)
                    .WithBrandAssociation(this.brand.hierarchyClassID),

                new TestItemBuilder()
                    .WithValidationDate(DateTime.Now.ToString())
                    .WithScanCode(this.testScanCodes[2])
                    .WithSubBrickAssociation(this.merchandiseClassLevelFiveA.hierarchyClassID)
                    .WithTaxClassAssociation(this.taxClass.hierarchyClassID)
                    .WithBrandAssociation(this.brand.hierarchyClassID)
            };

            this.context.Item.AddRange(testItems);
            this.context.SaveChanges();

            this.updateTraitCommand.UpdatedHierarchyClass = this.merchandiseClassLevelFiveA;
            this.updateTraitCommand.NonMerchandiseTrait = NonMerchandiseTraits.LegacyPosOnly;

            // When.
            this.updateHierarchyClassTraitCommandHandler.Execute(this.updateTraitCommand);

            // Then.
            var generatedMessages = this.context.MessageQueueProduct.Where(mq => mq.InsertDate > now && testScanCodes.Contains(mq.ScanCode)).ToList();
            Assert.IsTrue(generatedMessages.Count == 0);
        }

        [TestMethod]
        public void UpdateHierarchyClassTrait_TaxAbbreviationChanged_NoProductMessageShouldBeGenerated()
        {
            // Given.
            DateTime now = DateTime.Now;

            AddTestTaxClass();

            Item testItem = new TestItemBuilder()
                .WithValidationDate(DateTime.Now.ToString())
                .WithTaxClassAssociation(this.taxClass.hierarchyClassID);

            this.updateTraitCommand.TaxAbbreviation = "2488424 Edit Tax Abbreviation";
            this.updateTraitCommand.UpdatedHierarchyClass = this.taxClass;

            // When.
            updateHierarchyClassTraitCommandHandler.Execute(this.updateTraitCommand);

            // Then.
            MessageQueueProduct message = this.context.MessageQueueProduct.OrderByDescending(q => q.InsertDate).FirstOrDefault(q => q.ItemId == testItem.itemID && q.InsertDate > now);
            Assert.IsNull(message);
        }

        [TestMethod]
        public void UpdateHierarchyClassTrait_UpdatePosDeptNumberTeamNameTeamNumber_ShouldOnlyUpdatePosDeptNumberTeamNameAndTeamNumber()
        {
            // Given.
            AddTestFinancialClass("0001", String.Empty);

            this.updateTraitCommand.PosDeptNumber = "001";
            this.updateTraitCommand.TeamName = "TestTeamNameUpdate";
            this.updateTraitCommand.TeamNumber = "001";
            this.updateTraitCommand.UpdatedHierarchyClass = financialClass;

            // When.
            this.updateHierarchyClassTraitCommandHandler.Execute(this.updateTraitCommand);

            // Then.
            HierarchyClassTrait actualPosDeptNumberTrait = this.context.HierarchyClassTrait.SingleOrDefault(hct => hct.hierarchyClassID == this.updateTraitCommand.UpdatedHierarchyClass.hierarchyClassID && hct.Trait.traitCode == TraitCodes.PosDepartmentNumber);
            HierarchyClassTrait actualTeamNumberTrait = this.context.HierarchyClassTrait.SingleOrDefault(hct => hct.hierarchyClassID == this.updateTraitCommand.UpdatedHierarchyClass.hierarchyClassID && hct.Trait.traitCode == TraitCodes.TeamNumber);
            HierarchyClassTrait actualTeamNameTrait = this.context.HierarchyClassTrait.SingleOrDefault(hct => hct.hierarchyClassID == this.updateTraitCommand.UpdatedHierarchyClass.hierarchyClassID && hct.Trait.traitCode == TraitCodes.TeamName);

            var posDeptEntry = this.context.Entry(actualPosDeptNumberTrait);
            var teamNumberEntry = this.context.Entry(actualTeamNumberTrait);
            var teamNameEntry = this.context.Entry(actualTeamNameTrait);

            Assert.AreEqual(this.updateTraitCommand.PosDeptNumber, actualPosDeptNumberTrait.traitValue, "PosDeptNumber was not updated as expected.");
            Assert.AreEqual(this.updateTraitCommand.TeamName, actualTeamNameTrait.traitValue, "TeamName was not updated as expected.");
            Assert.AreEqual(this.updateTraitCommand.TeamNumber, actualTeamNumberTrait.traitValue, "TeamNumber was not updated as expected.");
            Assert.IsTrue(posDeptEntry.State == EntityState.Unchanged);
            Assert.IsTrue(teamNumberEntry.State == EntityState.Unchanged);
            Assert.IsTrue(teamNameEntry.State == EntityState.Unchanged);
            Assert.IsTrue(context.HierarchyClassTrait.Where(hct => hct.hierarchyClassID == financialClass.hierarchyClassID).Count() == 3);
        }

        [TestMethod]
        [ExpectedException(typeof(HierarchyClassTraitUpdateException))]
        public void UpdateHierarchyClassTrait_PosDeptNumberAlreadyExistsForAnotherFinancialClass_ShouldThrowUpdateHierarchyClassTraitException()
        {
            // Given.
            AddTestFinancialClass("0001", "001");
            AddTestFinancialClass("0002", "002");

            string existingPosDeptNumber = this.context.HierarchyClassTrait
                .First(hct => hct.traitID == Traits.PosDepartmentNumber && hct.traitValue == "001").traitValue;

            this.updateTraitCommand.PosDeptNumber = existingPosDeptNumber;
            this.updateTraitCommand.UpdatedHierarchyClass = financialClass;

            // When.
            updateHierarchyClassTraitCommandHandler.Execute(this.updateTraitCommand);

            // Then.
            // Expected exception.
        }

        [TestMethod]
        public void UpdateHierarchyClassTrait_AddDisableEventGenerationTraitWhenTraitAlreadyExists_OriginalTraitShouldExist()
        {
            // Given.
            AddTestFinancialClass("0001", "001");

            this.financialClass.HierarchyClassTrait.Add(new HierarchyClassTrait
                {
                    traitID = Traits.NonAlignedSubteam,
                    traitValue = "Test Trait Value"
                });

            updateTraitCommand.UpdatedHierarchyClass = this.financialClass;
            updateTraitCommand.NonAlignedSubteam = true;

            // When.
            updateHierarchyClassTraitCommandHandler.Execute(updateTraitCommand);

            // Then.
            var financialClassTrait = this.context.HierarchyClassTrait.SingleOrDefault(hct => hct.traitID == Traits.NonAlignedSubteam && hct.hierarchyClassID == financialClass.hierarchyClassID);

            Assert.IsNotNull(financialClassTrait);
            Assert.AreEqual("Test Trait Value", financialClassTrait.traitValue);
        }

        [TestMethod]
        public void UpdateHierarchyClassTrait_RemoveDisableEventGenerationTraitWhenTraitDoesNotExist_ShouldSuccessfullyFinishExecution()
        {
            // Given.
            AddTestFinancialClass("0001", "001");

            updateTraitCommand.UpdatedHierarchyClass = this.financialClass;
            updateTraitCommand.NonAlignedSubteam = false;

            // When.
            updateHierarchyClassTraitCommandHandler.Execute(updateTraitCommand);

            // Then.
            var financialClassTrait = this.context.HierarchyClassTrait.SingleOrDefault(hct => hct.traitID == Traits.NonAlignedSubteam && hct.hierarchyClassID == financialClass.hierarchyClassID);

            Assert.IsNull(financialClassTrait);
        }

        private void AddItemAssociationToSubBrick()
        {
            this.associatedItem = new TestItemBuilder().WithSubBrickAssociation(this.merchandiseClassLevelFiveA.hierarchyClassID);
            this.context.Item.Add(this.associatedItem);
            this.context.SaveChanges();
        }

        private void AddTestBrand()
        {
            this.brand = new TestHierarchyClassBuilder().WithHierarchyClassName("TestBrand").WithHierarchyId(Hierarchies.Brands);
            this.brand.Hierarchy = this.context.Hierarchy.Single(h => h.hierarchyID == this.brand.hierarchyID);
            this.context.HierarchyClass.Add(this.brand);
            this.context.SaveChanges();
        }

        private void AddTestTaxClass()
        {
            this.taxClass = new TestHierarchyClassBuilder().WithHierarchyLevel(1).WithHierarchyClassName("TestTaxClass")
                .WithHierarchyId(Hierarchies.Tax).WithHierarchyParentClassId(null).WithTaxAbbreviationTrait("2222222 TestTaxAbbrev");
            this.taxClass.Hierarchy = this.context.Hierarchy.Single(h => h.hierarchyID == this.taxClass.hierarchyID);
            this.context.HierarchyClass.Add(this.taxClass);
            this.context.SaveChanges();
        }

        private void AddTestMerchandiseHierarchyClassLevels()
        {
            this.merchandiseClassLevelOne = new TestHierarchyClassBuilder()
                .WithHierarchyClassName("TestMerchandiseLevel1")
                .WithHierarchyId(Hierarchies.Merchandise)
                .WithHierarchyLevel(1)
                .WithHierarchyParentClassId(null);
            this.merchandiseClassLevelOne.Hierarchy = this.context.Hierarchy.Single(h => h.hierarchyID == this.merchandiseClassLevelOne.hierarchyID);
            this.context.HierarchyClass.Add(merchandiseClassLevelOne);
            this.context.SaveChanges();

            this.merchandiseClassLevelTwo = new TestHierarchyClassBuilder()
                .WithHierarchyId(Hierarchies.Merchandise)
                .WithHierarchyClassName("TestMerchandiseLevel2")
                .WithHierarchyLevel(2)
                .WithHierarchyParentClassId(this.merchandiseClassLevelOne.hierarchyClassID);
            this.merchandiseClassLevelTwo.Hierarchy = this.context.Hierarchy.Single(h => h.hierarchyID == this.merchandiseClassLevelTwo.hierarchyID);
            this.context.HierarchyClass.Add(merchandiseClassLevelTwo);
            this.context.SaveChanges();

            this.merchandiseClassLevelThree = new TestHierarchyClassBuilder()
                .WithHierarchyId(Hierarchies.Merchandise)
                .WithHierarchyClassName("TestMerchandiseLevel3")
                .WithHierarchyLevel(3)
                .WithHierarchyParentClassId(this.merchandiseClassLevelTwo.hierarchyClassID)
                .WithGlAccountTrait("4500001");
            this.merchandiseClassLevelThree.Hierarchy = this.context.Hierarchy.Single(h => h.hierarchyID == this.merchandiseClassLevelThree.hierarchyID);
            this.context.HierarchyClass.Add(merchandiseClassLevelThree);
            this.context.SaveChanges();

            this.merchandiseClassLevelFour = new TestHierarchyClassBuilder()
                .WithHierarchyId(Hierarchies.Merchandise)
                .WithHierarchyClassName("TestMerchandiseLevel4")
                .WithHierarchyLevel(4)
                .WithHierarchyParentClassId(this.merchandiseClassLevelThree.hierarchyClassID);
            this.merchandiseClassLevelFour.Hierarchy = this.context.Hierarchy.Single(h => h.hierarchyID == this.merchandiseClassLevelFour.hierarchyID);
            this.context.HierarchyClass.Add(merchandiseClassLevelFour);
            this.context.SaveChanges();

            this.merchandiseClassLevelFiveA = new TestHierarchyClassBuilder()
                .WithHierarchyId(Hierarchies.Merchandise)
                .WithHierarchyClassName("TestMerchandiseLevel5A")
                .WithHierarchyLevel(5)
                .WithHierarchyParentClassId(this.merchandiseClassLevelFour.hierarchyClassID)
                .WithMerchFinMapping(this.context.HierarchyClass.FirstOrDefault(hc => hc.hierarchyID == Hierarchies.Financial).hierarchyClassName);
            this.merchandiseClassLevelFiveA.Hierarchy = this.context.Hierarchy.Single(h => h.hierarchyID == this.merchandiseClassLevelFiveA.hierarchyID);
            this.context.HierarchyClass.Add(merchandiseClassLevelFiveA);
            this.context.SaveChanges();

            this.merchandiseClassLevelFiveB = new TestHierarchyClassBuilder()
                .WithHierarchyId(Hierarchies.Merchandise)
                .WithHierarchyClassName("TestMerchandiseLevel5B")
                .WithHierarchyLevel(5)
                .WithHierarchyParentClassId(this.merchandiseClassLevelFour.hierarchyClassID)
                .WithMerchFinMapping(this.context.HierarchyClass.FirstOrDefault(hc => hc.hierarchyID == Hierarchies.Financial).hierarchyClassName);
            this.merchandiseClassLevelFiveB.Hierarchy = this.context.Hierarchy.Single(h => h.hierarchyID == this.merchandiseClassLevelFiveB.hierarchyID);
            this.context.HierarchyClass.Add(merchandiseClassLevelFiveB);
            this.context.SaveChanges();

            this.merchandiseClassLevelFiveC = new TestHierarchyClassBuilder()
                .WithHierarchyId(Hierarchies.Merchandise)
                .WithHierarchyClassName("TestMerchandiseLevel5C")
                .WithHierarchyLevel(5)
                .WithHierarchyParentClassId(this.merchandiseClassLevelFour.hierarchyClassID)
                .WithMerchFinMapping(this.context.HierarchyClass.FirstOrDefault(hc => hc.hierarchyID == Hierarchies.Financial).hierarchyClassName);
            this.merchandiseClassLevelFiveC.Hierarchy = this.context.Hierarchy.Single(h => h.hierarchyID == this.merchandiseClassLevelFiveC.hierarchyID);
            this.context.HierarchyClass.Add(merchandiseClassLevelFiveC);
            this.context.SaveChanges();
        }

        private void AddTestFinancialClass(string peopleSoftNumber, string posDeptNumber)
        {
            string hierarchyClassName = "TestFinancialClass" + " (" + peopleSoftNumber + ")";

            this.financialClass = new TestHierarchyClassBuilder()
                .WithHierarchyLevel(1)
                .WithHierarchyClassName(hierarchyClassName)
                .WithHierarchyId(Hierarchies.Financial)
                .WithHierarchyParentClassId(null)
                .WithPosDeptNumberTrait(posDeptNumber)
                .WithTeamNameTrait("TestTeamName")
                .WithTeamNumberTrait("000");
            this.financialClass.Hierarchy = this.context.Hierarchy.Single(h => h.hierarchyID == this.financialClass.hierarchyID);

            this.context.HierarchyClass.Add(this.financialClass);
            this.context.SaveChanges();
        }
    }
}
