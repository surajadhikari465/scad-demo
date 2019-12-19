using Icon.Framework;
using Icon.Testing.Builders;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Transactions;

namespace Icon.Web.Tests.Integration.Commands
{
    [TestClass] 
    public class UpdateHierarchyClassTraitCommandHandlerTests
    {
        private IconContext context;
        private TransactionScope transaction;
        private UpdateHierarchyClassTraitCommandHandler updateHierarchyClassTraitCommandHandler;
        private UpdateHierarchyClassTraitCommand updateTraitCommand;
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
        private List<string> testScanCodes;

        [TestInitialize]
        public void InitializeData()
        {
            this.transaction = new TransactionScope();
            this.context = new IconContext();
            this.updateHierarchyClassTraitCommandHandler = new UpdateHierarchyClassTraitCommandHandler(this.context);
            this.updateTraitCommand = new UpdateHierarchyClassTraitCommand();
            this.testScanCodes = new List<string> { "8428428428", "84284284280", "84284284281" };
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose();
            context.Dispose();
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
                .SingleOrDefault(hct => hct.hierarchyClassID == taxClass.hierarchyClassID && hct.Trait.traitCode == TraitCodes.MerchFinMapping));
        }

        [TestMethod]
        public void UpdateHierarchyClassTrait_UpdateSubTeamAssociation_ShouldOnlyUpdateSubTeamAssociation()
        {
            // Given.
            int existingSubTeamId = AddTestFinancialClass("TestPSNumber","TestPOSNumber");
            int expectedSubTeamId = AddTestFinancialClass("Expected Test PS Number", "Expected Test POS Dept Num");
            AddTestMerchandiseHierarchyClassLevels(existingSubTeamId);

            this.updateTraitCommand.SubTeamHierarchyClassId = expectedSubTeamId;
            this.updateTraitCommand.UpdatedHierarchyClass = merchandiseClassLevelFiveA;

            var expectedClassName = this.merchandiseClassLevelFiveA.hierarchyClassName;

            // When.
            updateHierarchyClassTraitCommandHandler.Execute(this.updateTraitCommand);

            // Then.
            HierarchyClassTrait actualTrait = this.context.HierarchyClassTrait
                .SingleOrDefault(hct => hct.hierarchyClassID == this.updateTraitCommand.UpdatedHierarchyClass.hierarchyClassID && hct.Trait.traitCode == TraitCodes.MerchFinMapping);
            var entry = this.context.Entry(actualTrait);

            Assert.AreEqual(expectedSubTeamId.ToString(), actualTrait.traitValue, "SubTeam was not updated as expected.");
            Assert.IsTrue(entry.State == EntityState.Unchanged);
            Assert.IsTrue(context.HierarchyClassTrait.Where(hct => hct.hierarchyClassID == merchandiseClassLevelFiveA.hierarchyClassID).Count() == 1);
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
            int expectedSubTeamId = AddTestFinancialClass("TestPSNumber", "TestPOSNumber");
            AddTestMerchandiseHierarchyClassLevels(expectedSubTeamId);

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
                .SingleOrDefault(hct => hct.hierarchyClassID == merchandiseClassLevelFiveA.hierarchyClassID && hct.Trait.traitCode == TraitCodes.TaxAbbreviation));
        }

        [TestMethod]
        public void UpdateHierarchyClassTrait_UpdateNonMerchandiseTraitForSubBrick_NonMerchandiseTraitShouldBeUpdated()
        {
            // Given.
            int expectedSubTeamId = AddTestFinancialClass("TestPSNumber", "TestPOSNumber");
            AddTestMerchandiseHierarchyClassLevels(expectedSubTeamId);

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
                .SingleOrDefault(hct => hct.hierarchyClassID == merchandiseClassLevelFiveA.hierarchyClassID && hct.Trait.traitCode == TraitCodes.TaxAbbreviation));
        }

        [TestMethod]
        public void UpdateHierarchyClassTrait_RemoveNonMerchandiseTraitFromSubBrick_NonMerchandiseTraitShouldBeRemovedFromSubBrick()
        {
            // Given.
            int expectedSubTeamId = AddTestFinancialClass("TestPSNumber", "TestPOSNumber");
            AddTestMerchandiseHierarchyClassLevels(expectedSubTeamId);

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
                .SingleOrDefault(hct => hct.hierarchyClassID == merchandiseClassLevelFiveA.hierarchyClassID && hct.Trait.traitCode == TraitCodes.TaxAbbreviation));
        }

        [TestMethod]
        public void UpdateHierarchyClassTrait_NoUpdateToNonMerchandiseTrait_NoUpdateShouldOccurr()
        {
            // Given.
            int expectedSubTeamId = AddTestFinancialClass("TestPSNumber", "TestPOSNumber");
            AddTestMerchandiseHierarchyClassLevels(expectedSubTeamId);

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
            int expectedSubTeamId = AddTestFinancialClass("TestPSNumber", "TestPOSNumber");
            AddTestMerchandiseHierarchyClassLevels(expectedSubTeamId);

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
        public void UpdateHierarchyClassTrait_UpdatePosDeptNumberTeamNameTeamNumber_ShouldOnlyUpdatePosDeptNumberTeamNameAndTeamNumber()
        {
            // Given.
            int subTeamId = AddTestFinancialClass("0001", String.Empty);

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
            this.associatedItem = new Item
            {
                ItemHierarchyClass = new List<ItemHierarchyClass> { new ItemHierarchyClass { hierarchyClassID = this.merchandiseClassLevelFiveA.hierarchyClassID } },
                ItemTypeId = ItemTypes.RetailSale,
                ItemAttributesJson = "{}"
            };
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

        private void AddTestMerchandiseHierarchyClassLevels(int? merchFinMapping = null)
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
                .WithHierarchyParentClassId(this.merchandiseClassLevelTwo.hierarchyClassID);
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
                .WithMerchFinMapping(merchFinMapping.ToString());
            this.merchandiseClassLevelFiveA.Hierarchy = this.context.Hierarchy.Single(h => h.hierarchyID == this.merchandiseClassLevelFiveA.hierarchyID);
            this.context.HierarchyClass.Add(merchandiseClassLevelFiveA);
            this.context.SaveChanges();

            this.merchandiseClassLevelFiveB = new TestHierarchyClassBuilder()
                .WithHierarchyId(Hierarchies.Merchandise)
                .WithHierarchyClassName("TestMerchandiseLevel5B")
                .WithHierarchyLevel(5)
                .WithHierarchyParentClassId(this.merchandiseClassLevelFour.hierarchyClassID)
                .WithMerchFinMapping(merchFinMapping.ToString());
            this.merchandiseClassLevelFiveB.Hierarchy = this.context.Hierarchy.Single(h => h.hierarchyID == this.merchandiseClassLevelFiveB.hierarchyID);
            this.context.HierarchyClass.Add(merchandiseClassLevelFiveB);
            this.context.SaveChanges();

            this.merchandiseClassLevelFiveC = new TestHierarchyClassBuilder()
                .WithHierarchyId(Hierarchies.Merchandise)
                .WithHierarchyClassName("TestMerchandiseLevel5C")
                .WithHierarchyLevel(5)
                .WithHierarchyParentClassId(this.merchandiseClassLevelFour.hierarchyClassID)
                .WithMerchFinMapping(merchFinMapping.ToString());
            this.merchandiseClassLevelFiveC.Hierarchy = this.context.Hierarchy.Single(h => h.hierarchyID == this.merchandiseClassLevelFiveC.hierarchyID);
            this.context.HierarchyClass.Add(merchandiseClassLevelFiveC);
            this.context.SaveChanges();
        }

        private int AddTestFinancialClass(string peopleSoftNumber, string posDeptNumber)
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

            return financialClass.hierarchyClassID;
        }
    }
}
