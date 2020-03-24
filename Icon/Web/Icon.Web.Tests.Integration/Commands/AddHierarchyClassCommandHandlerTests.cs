using Icon.Framework;
using Icon.Logging;
using Icon.Testing.Builders;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;

namespace Icon.Web.Tests.Integration.Commands
{
    [TestClass] 
    public class AddHierarchyClassCommandHandlerTests
    {
        private IconContext context;
        private DbContextTransaction transaction;
        private AddHierarchyClassCommandHandler commandHandler;
        private Mock<ILogger> mockLogger;
        private HierarchyClass testHierarchyClass;
        private string hierarchyClassName;
        private string financialHierarchyClassName;
        private string duplicateFinancialHierarchyClassName;
        private HierarchyClass parentHierarchyClass;

        [TestInitialize]
        public void Initialize()
        {            
            context = new IconContext();
            mockLogger = new Mock<ILogger>();

            hierarchyClassName = "Add Hierarchy Class Integration Test";
            financialHierarchyClassName = "Add Financial Hierarchy Class Integration Test (1000)";
            duplicateFinancialHierarchyClassName = "Add Financial Hierarchy Class Integration Test Duplicate POS Department Number(1001)";

            commandHandler = new AddHierarchyClassCommandHandler(this.context);

            transaction = context.Database.BeginTransaction();

            parentHierarchyClass = new TestHierarchyClassBuilder()
                .WithHierarchyId(Hierarchies.Merchandise)
                .WithHierarchyLevel(HierarchyLevels.Gs1Brick)
                .WithHierarchyClassName("Test Parent HierarchyClass");

            context.HierarchyClass.Add(parentHierarchyClass);
            context.SaveChanges();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
        }

        [TestMethod]
        public void AddHierarchyClass_SuccessfulExecution_NewHierarchyClassShouldBeAdded()
        {
            // Given.
            testHierarchyClass = new TestHierarchyClassBuilder()
                .WithHierarchyId(Hierarchies.Merchandise)
                .WithHierarchyLevel(HierarchyLevels.SubBrick)
                .WithHierarchyClassName(hierarchyClassName)
                .WithHierarchyParentClassId(parentHierarchyClass.hierarchyClassID);

            var addHierarchyClassCommand = new AddHierarchyClassCommand
            {
                NewHierarchyClass = testHierarchyClass
            };

            // When.
            commandHandler.Execute(addHierarchyClassCommand);

            // Then.
            var newHierarchyClass = context.HierarchyClass.Single(hc => hc.hierarchyClassName == testHierarchyClass.hierarchyClassName);

            Assert.AreEqual(testHierarchyClass.hierarchyID, newHierarchyClass.hierarchyID);
            Assert.AreEqual(testHierarchyClass.hierarchyClassName, newHierarchyClass.hierarchyClassName);
            Assert.AreEqual(testHierarchyClass.hierarchyParentClassID, newHierarchyClass.hierarchyParentClassID);
            Assert.AreEqual(testHierarchyClass.hierarchyLevel, newHierarchyClass.hierarchyLevel);
        }

        [TestMethod]
        [ExpectedException(typeof(DuplicateValueException))]
        public void AddHierarchyClass_DuplicateNameOnSameLevel_ExceptionShouldBeThrown()
        {
            // Given.
            testHierarchyClass = new TestHierarchyClassBuilder()
                .WithHierarchyId(Hierarchies.Merchandise)
                .WithHierarchyLevel(HierarchyLevels.SubBrick)
                .WithHierarchyClassName(hierarchyClassName)
                .WithHierarchyParentClassId(parentHierarchyClass.hierarchyClassID);

            var addHierarchyClassCommand = new AddHierarchyClassCommand
            {
                NewHierarchyClass = testHierarchyClass
            };

            // When.
            commandHandler.Execute(addHierarchyClassCommand);

            // Alter capitalization to ensure that the duplicate matching isn't case-sensitive.
            addHierarchyClassCommand.NewHierarchyClass.hierarchyClassName = hierarchyClassName.ToUpper();

            commandHandler.Execute(addHierarchyClassCommand);

            // Then.
            // Expected exception.
        }

        [TestMethod]
        public void AddHierarchyClass_DuplicateNameOnDifferentLevel_NewHierarchyClassShouldBeAdded()
        {
            // Given.
            HierarchyClass testSubBrick = new TestHierarchyClassBuilder()
                .WithHierarchyId(Hierarchies.Merchandise)
                .WithHierarchyLevel(HierarchyLevels.SubBrick)
                .WithHierarchyClassName(hierarchyClassName)
                .WithHierarchyParentClassId(parentHierarchyClass.hierarchyClassID);

            var addHierarchyClassCommand = new AddHierarchyClassCommand
            {
                NewHierarchyClass = testSubBrick
            };

            // When.
            commandHandler.Execute(addHierarchyClassCommand);

            HierarchyClass testBrick = new TestHierarchyClassBuilder().WithHierarchyId(Hierarchies.Merchandise).WithHierarchyLevel(HierarchyLevels.Gs1Brick).WithHierarchyClassName(hierarchyClassName);

            addHierarchyClassCommand.NewHierarchyClass = testBrick;

            commandHandler.Execute(addHierarchyClassCommand);

            // Then.
            var newHierarchyClassSubBrickLevel = context.HierarchyClass
                .Single(hc => hc.hierarchyClassName == hierarchyClassName && hc.hierarchyLevel == HierarchyLevels.SubBrick);

            var newHierarchyClassBrickLevel = context.HierarchyClass
                .Single(hc => hc.hierarchyClassName == hierarchyClassName && hc.hierarchyLevel == HierarchyLevels.Gs1Brick);

            Assert.AreEqual(testSubBrick.hierarchyClassName, newHierarchyClassSubBrickLevel.hierarchyClassName);
            Assert.AreEqual(testSubBrick.hierarchyLevel, newHierarchyClassSubBrickLevel.hierarchyLevel);

            Assert.AreEqual(testBrick.hierarchyClassName, newHierarchyClassBrickLevel.hierarchyClassName);
            Assert.AreEqual(testBrick.hierarchyLevel, newHierarchyClassBrickLevel.hierarchyLevel);
        }

        [TestMethod]
        [ExpectedException(typeof(DbEntityValidationException))]
        public void AddHierarchyClass_FailedExecution_ShouldThrowCommandException()
        {
            // Given.
            string tooLongClassName = new string('z', 256);

            testHierarchyClass = new TestHierarchyClassBuilder()
                .WithHierarchyId(Hierarchies.Merchandise)
                .WithHierarchyLevel(HierarchyLevels.SubBrick)
                .WithHierarchyClassName(tooLongClassName)
                .WithHierarchyParentClassId(parentHierarchyClass.hierarchyClassID);

            var addHierarchyClassCommand = new AddHierarchyClassCommand
            {
                NewHierarchyClass = testHierarchyClass
            };

            // When.
            commandHandler.Execute(addHierarchyClassCommand);

            // Then.
            // Expected exception.
        }

        [TestMethod]
        public void AddHierarchyClass_NewHierarchyClassWithSubTeamTrait_HierarchyClassAndTraitShouldBeAdded()
        {
            // Given.
            testHierarchyClass = new TestHierarchyClassBuilder()
                .WithHierarchyId(Hierarchies.Merchandise)
                .WithHierarchyLevel(HierarchyLevels.SubBrick)
                .WithHierarchyClassName(hierarchyClassName)
                .WithHierarchyParentClassId(parentHierarchyClass.hierarchyClassID); ;

            var addHierarchyClassCommand = new AddHierarchyClassCommand
            {
                NewHierarchyClass = testHierarchyClass,
                SubTeamHierarchyClassId = context.HierarchyClass.First(hc => hc.Hierarchy.hierarchyName == HierarchyNames.Financial).hierarchyClassID
            };

            string expectedClassName = hierarchyClassName;
            string expectedLevel = addHierarchyClassCommand.NewHierarchyClass.hierarchyLevel.ToString();
            string expectedSubTeam = context.HierarchyClass.FirstOrDefault(hc => hc.hierarchyClassID == addHierarchyClassCommand.SubTeamHierarchyClassId).hierarchyClassName;

            // When.
            commandHandler.Execute(addHierarchyClassCommand);

            // Then.
            var actual = context.HierarchyClass.Single(hc => hc.hierarchyClassName == hierarchyClassName);

            Assert.AreEqual(expectedClassName, actual.hierarchyClassName);
            Assert.AreEqual(expectedLevel, actual.hierarchyLevel.ToString());
            Assert.AreEqual(addHierarchyClassCommand.SubTeamHierarchyClassId, int.Parse(actual.HierarchyClassTrait.Single(hct => hct.Trait.traitCode == TraitCodes.MerchFinMapping).traitValue));
        }

        [TestMethod]
        public void AddHierarchyClass_NewHierarchyClassWithNonMerchandiseTrait_HierarchyClassAndTraitsShouldBeAdded()
        {
            // Given.
            testHierarchyClass = new TestHierarchyClassBuilder()
                .WithHierarchyId(Hierarchies.Merchandise)
                .WithHierarchyLevel(HierarchyLevels.SubBrick)
                .WithHierarchyClassName(hierarchyClassName)
                .WithHierarchyParentClassId(parentHierarchyClass.hierarchyClassID);

            var addHierarchyClassCommand = new AddHierarchyClassCommand
            {
                NewHierarchyClass = testHierarchyClass,
                SubTeamHierarchyClassId = context.HierarchyClass.First(hc => hc.Hierarchy.hierarchyName == HierarchyNames.Financial).hierarchyClassID,
                NonMerchandiseTrait = NonMerchandiseTraits.Crv
            };


            string expectedClassName = hierarchyClassName;
            string expectedLevel = addHierarchyClassCommand.NewHierarchyClass.hierarchyLevel.ToString();
            string expectedSubTeam = context.HierarchyClass.FirstOrDefault(hc => hc.hierarchyClassID == addHierarchyClassCommand.SubTeamHierarchyClassId).hierarchyClassName;
            string expectedNonMerchandise = NonMerchandiseTraits.Crv;

            // When.
            commandHandler.Execute(addHierarchyClassCommand);

            // Then.
            var actual = context.HierarchyClass.Single(hc => hc.hierarchyClassName == hierarchyClassName);

            Assert.AreEqual(expectedClassName, actual.hierarchyClassName);
            Assert.AreEqual(expectedLevel, actual.hierarchyLevel.ToString());
            Assert.AreEqual(addHierarchyClassCommand.SubTeamHierarchyClassId, int.Parse(actual.HierarchyClassTrait.Single(hct => hct.Trait.traitCode == TraitCodes.MerchFinMapping).traitValue));
            Assert.AreEqual(expectedNonMerchandise, actual.HierarchyClassTrait.Single(hct => hct.Trait.traitCode == TraitCodes.NonMerchandise).traitValue);
        }

        [TestMethod]
        public void AddHierarchyClass_NewHierarchyClassWithPosDeptTeamNameAndTeamNumberTraits()
        {
            // Given.
            testHierarchyClass = new TestHierarchyClassBuilder().WithHierarchyId(Hierarchies.Financial).WithHierarchyLevel(HierarchyLevels.Parent).WithHierarchyClassName(financialHierarchyClassName);

            var addHierarchyClassCommand = new AddHierarchyClassCommand
            {
                NewHierarchyClass = testHierarchyClass,
                PosDeptNumber = "9999",
                TeamName = "TestTeamName",
                TeamNumber = "223"
            };

            string expectedClassName = financialHierarchyClassName;
            string expectedLevel = addHierarchyClassCommand.NewHierarchyClass.hierarchyLevel.ToString();
            string expectedPosDeptNumber = addHierarchyClassCommand.PosDeptNumber;
            string expectedTeamNumber = addHierarchyClassCommand.TeamNumber;
            string expectedTeamName = addHierarchyClassCommand.TeamName;

            // When.
            commandHandler.Execute(addHierarchyClassCommand);

            // Then.
            var actual = context.HierarchyClass.Single(hc => hc.hierarchyClassName == financialHierarchyClassName);

            Assert.AreEqual(expectedClassName, actual.hierarchyClassName);
            Assert.AreEqual(expectedLevel, actual.hierarchyLevel.ToString());
            Assert.AreEqual(expectedPosDeptNumber, actual.HierarchyClassTrait.Single(hct => hct.traitID == Traits.PosDepartmentNumber).traitValue);
            Assert.AreEqual(expectedTeamNumber, actual.HierarchyClassTrait.Single(hct => hct.traitID == Traits.TeamNumber).traitValue);
            Assert.AreEqual(expectedTeamName, actual.HierarchyClassTrait.Single(hct => hct.traitID == Traits.TeamName).traitValue);
        }

        [TestMethod]
        public void AddHierarchyClass_NewHierarchyClassWithNoPosDeptTeamNameAndTeamNumberTraits_ThoseTraitsShouldNotBeCreated()
        {
            // Given.
            testHierarchyClass = new TestHierarchyClassBuilder().WithHierarchyId(Hierarchies.Financial).WithHierarchyLevel(HierarchyLevels.Parent).WithHierarchyClassName(financialHierarchyClassName);

            var addHierarchyClassCommand = new AddHierarchyClassCommand
            {
                NewHierarchyClass = testHierarchyClass
            };

            string expectedClassName = financialHierarchyClassName;
            string expectedLevel = addHierarchyClassCommand.NewHierarchyClass.hierarchyLevel.ToString();

            // When.
            commandHandler.Execute(addHierarchyClassCommand);

            // Then.
            var actual = context.HierarchyClass.Single(hc => hc.hierarchyClassName == financialHierarchyClassName);

            Assert.AreEqual(expectedClassName, actual.hierarchyClassName);
            Assert.AreEqual(expectedLevel, actual.hierarchyLevel.ToString());
            Assert.IsNull(actual.HierarchyClassTrait.Where(hct => hct.traitID == Traits.PosDepartmentNumber).Select(hct => hct.traitValue).SingleOrDefault());
            Assert.IsNull(actual.HierarchyClassTrait.Where(hct => hct.traitID == Traits.TeamNumber).Select(hct => hct.traitValue).SingleOrDefault());
            Assert.IsNull(actual.HierarchyClassTrait.Where(hct => hct.traitID == Traits.TeamName).Select(hct => hct.traitValue).SingleOrDefault());
        }

        [TestMethod]
        public void AddHierarchyClass_NewHierarchyClassWithDuplicatePosDeptTrait_ThrowsException()
        {
            // Given.
            testHierarchyClass = new TestHierarchyClassBuilder()
                .WithHierarchyId(Hierarchies.Financial)
                .WithHierarchyLevel(HierarchyLevels.Parent)
                .WithHierarchyClassName(financialHierarchyClassName);

            var addHierarchyClassCommand = new AddHierarchyClassCommand
            {
                NewHierarchyClass = testHierarchyClass,
                PosDeptNumber = "000"
            };

            var duplicateAddHierarchyClassCommand = new AddHierarchyClassCommand
            {
                NewHierarchyClass = new HierarchyClass
                {
                    hierarchyID = Hierarchies.Financial,
                    hierarchyClassName = duplicateFinancialHierarchyClassName,
                    hierarchyLevel = 1
                },
                PosDeptNumber = "000"
            };

            // When.
            commandHandler.Execute(addHierarchyClassCommand);

            try
            {
                commandHandler.Execute(duplicateAddHierarchyClassCommand);
                Assert.Fail("Expected exception not thrown.");
            }
            catch (DuplicateValueException dvex)
            {
                Assert.AreEqual("The POS Department Number 000 is already assigned to a different subteam.", dvex.Message);
            }
        }

        [TestMethod]
        public void AddHierarchyClass_AnyHierarchyExceptTax_SentToEsbTraitShouldBeCreatedWithNullValue()
        {
            // Given.
            HierarchyClass testBrand = new TestHierarchyClassBuilder().WithHierarchyId(Hierarchies.Brands).WithHierarchyLevel(HierarchyLevels.Parent).WithHierarchyClassName(hierarchyClassName);
            HierarchyClass testMerchandise = new TestHierarchyClassBuilder().WithHierarchyId(Hierarchies.Merchandise).WithHierarchyLevel(HierarchyLevels.SubBrick).WithHierarchyClassName(hierarchyClassName).WithHierarchyParentClassId(parentHierarchyClass.hierarchyClassID);
            HierarchyClass testFinancial = new TestHierarchyClassBuilder().WithHierarchyId(Hierarchies.Financial).WithHierarchyLevel(HierarchyLevels.Parent).WithHierarchyClassName(hierarchyClassName);
            HierarchyClass testBrowsing = new TestHierarchyClassBuilder().WithHierarchyId(Hierarchies.Browsing).WithHierarchyLevel(HierarchyLevels.Parent).WithHierarchyClassName(hierarchyClassName);

            var command = new AddHierarchyClassCommand();

            // When.
            command.NewHierarchyClass = testBrand;
            commandHandler.Execute(command);

            command.NewHierarchyClass = testMerchandise;
            commandHandler.Execute(command);

            command.NewHierarchyClass = testFinancial;
            commandHandler.Execute(command);

            command.NewHierarchyClass = testBrowsing;
            commandHandler.Execute(command);

            // Then.
            var newBrand = context.HierarchyClass.Single(hc => hc.Hierarchy.hierarchyName == HierarchyNames.Brands && hc.hierarchyClassName == hierarchyClassName);
            var newBrandSentToEsbTrait = newBrand.HierarchyClassTrait.Single(hct => hct.Trait.traitCode == TraitCodes.SentToEsb);

            var newMerchandise = context.HierarchyClass.Single(hc => hc.Hierarchy.hierarchyName == HierarchyNames.Merchandise && hc.hierarchyClassName == hierarchyClassName);
            var newMerchandiseSentToEsbTrait = newMerchandise.HierarchyClassTrait.Single(hct => hct.Trait.traitCode == TraitCodes.SentToEsb);

            var newFinancial = context.HierarchyClass.Single(hc => hc.Hierarchy.hierarchyName == HierarchyNames.Financial && hc.hierarchyClassName == hierarchyClassName);
            var newFinancialSentToEsbTrait = newFinancial.HierarchyClassTrait.Single(hct => hct.Trait.traitCode == TraitCodes.SentToEsb);

            var newBrowsing = context.HierarchyClass.Single(hc => hc.Hierarchy.hierarchyName == HierarchyNames.Browsing && hc.hierarchyClassName == hierarchyClassName);
            var newBrowsingSentToEsbTrait = newBrowsing.HierarchyClassTrait.Single(hct => hct.Trait.traitCode == TraitCodes.SentToEsb);

            Assert.IsNull(newBrandSentToEsbTrait.traitValue);
            Assert.IsNull(newMerchandiseSentToEsbTrait.traitValue);
            Assert.IsNull(newFinancialSentToEsbTrait.traitValue);
            Assert.IsNull(newBrowsingSentToEsbTrait.traitValue);
        }

        [TestMethod]
        public void AddHierarchyClass_AnyNewHierarchyClass_SentToEsbTraitShouldBeCreatedWithNullValue()
        {
            // Given.
            HierarchyClass testBrand = new TestHierarchyClassBuilder().WithHierarchyId(Hierarchies.Brands).WithHierarchyLevel(HierarchyLevels.Parent).WithHierarchyClassName(hierarchyClassName);
            HierarchyClass testMerchandise = new TestHierarchyClassBuilder().WithHierarchyId(Hierarchies.Merchandise).WithHierarchyLevel(HierarchyLevels.SubBrick).WithHierarchyClassName(hierarchyClassName).WithHierarchyParentClassId(parentHierarchyClass.hierarchyClassID);
            HierarchyClass testFinancial = new TestHierarchyClassBuilder().WithHierarchyId(Hierarchies.Financial).WithHierarchyLevel(HierarchyLevels.Parent).WithHierarchyClassName(hierarchyClassName);
            HierarchyClass testBrowsing = new TestHierarchyClassBuilder().WithHierarchyId(Hierarchies.Browsing).WithHierarchyLevel(HierarchyLevels.Parent).WithHierarchyClassName(hierarchyClassName);

            var command = new AddHierarchyClassCommand();

            // When.
            command.NewHierarchyClass = testBrand;
            commandHandler.Execute(command);

            command.NewHierarchyClass = testMerchandise;
            commandHandler.Execute(command);

            command.NewHierarchyClass = testFinancial;
            commandHandler.Execute(command);

            command.NewHierarchyClass = testBrowsing;
            commandHandler.Execute(command);

            // Then.
            var newBrand = context.HierarchyClass.Single(hc => hc.Hierarchy.hierarchyName == HierarchyNames.Brands && hc.hierarchyClassName == hierarchyClassName);
            var newBrandSentToEsbTrait = newBrand.HierarchyClassTrait.Single(hct => hct.Trait.traitCode == TraitCodes.SentToEsb);

            var newMerchandise = context.HierarchyClass.Single(hc => hc.Hierarchy.hierarchyName == HierarchyNames.Merchandise && hc.hierarchyClassName == hierarchyClassName);
            var newMerchandiseSentToEsbTrait = newMerchandise.HierarchyClassTrait.Single(hct => hct.Trait.traitCode == TraitCodes.SentToEsb);

            var newFinancial = context.HierarchyClass.Single(hc => hc.Hierarchy.hierarchyName == HierarchyNames.Financial && hc.hierarchyClassName == hierarchyClassName);
            var newFinancialSentToEsbTrait = newFinancial.HierarchyClassTrait.Single(hct => hct.Trait.traitCode == TraitCodes.SentToEsb);

            var newBrowsing = context.HierarchyClass.Single(hc => hc.Hierarchy.hierarchyName == HierarchyNames.Browsing && hc.hierarchyClassName == hierarchyClassName);
            var newBrowsingSentToEsbTrait = newBrowsing.HierarchyClassTrait.Single(hct => hct.Trait.traitCode == TraitCodes.SentToEsb);

            Assert.IsNull(newBrandSentToEsbTrait.traitValue);
            Assert.IsNull(newMerchandiseSentToEsbTrait.traitValue);
            Assert.IsNull(newFinancialSentToEsbTrait.traitValue);
            Assert.IsNull(newBrowsingSentToEsbTrait.traitValue);
        }

        [TestMethod]
        public void AddHierarchyClass_SubBrickWithParentProhibitDiscount_NewSubBrickShouldBeAddedWithProhibitDiscount()
        {
            // Given.
            var parentHierarchyClass = new TestHierarchyClassBuilder()
                .WithHierarchyId(Hierarchies.Merchandise)
                .WithHierarchyLevel(HierarchyLevels.Gs1Brick)
                .WithHierarchyClassName("Blake Jones")
                .WithProhibitDiscountTrait("1")
                .Build();

            context.HierarchyClass.Add(parentHierarchyClass);
            context.SaveChanges();
                        
            testHierarchyClass = new TestHierarchyClassBuilder()
                .WithHierarchyId(Hierarchies.Merchandise)
                .WithHierarchyParentClassId(parentHierarchyClass.hierarchyClassID)
                .WithHierarchyLevel(HierarchyLevels.SubBrick)
                .WithHierarchyClassName(hierarchyClassName);

            var addHierarchyClassCommand = new AddHierarchyClassCommand
            {
                NewHierarchyClass = testHierarchyClass
            };

            // When.
            
            commandHandler.Execute(addHierarchyClassCommand);

            // Then.
            var newHierarchyClass = context.HierarchyClass.Single(hc => hc.hierarchyClassName == testHierarchyClass.hierarchyClassName);

            Assert.AreEqual(testHierarchyClass.hierarchyID, newHierarchyClass.hierarchyID);
            Assert.AreEqual(testHierarchyClass.hierarchyClassName, newHierarchyClass.hierarchyClassName);
            Assert.AreEqual(testHierarchyClass.hierarchyParentClassID, newHierarchyClass.hierarchyParentClassID);
            Assert.AreEqual(testHierarchyClass.hierarchyLevel, newHierarchyClass.hierarchyLevel);
            Assert.IsTrue(testHierarchyClass.HierarchyClassTrait.Any(t => t.traitID == Traits.ProhibitDiscount && t.traitValue == "1"));
        }

        [TestMethod]
        public void AddHierarchyClass_SubBrickWithoutParentProhibitDiscount_NewSubBrickShouldNotBeAddedWithProhibitDiscount()
        {
            // Given.
            var parentHierarchyClass = new TestHierarchyClassBuilder()
                .WithHierarchyId(Hierarchies.Merchandise)
                .WithHierarchyLevel(HierarchyLevels.Gs1Brick)
                .WithHierarchyClassName("Blake Jones")
                .WithProhibitDiscountTrait("0")
                .Build();

            context.HierarchyClass.Add(parentHierarchyClass);
            context.SaveChanges();

            testHierarchyClass = new TestHierarchyClassBuilder()
                .WithHierarchyId(Hierarchies.Merchandise)
                .WithHierarchyParentClassId(parentHierarchyClass.hierarchyClassID)
                .WithHierarchyLevel(HierarchyLevels.SubBrick)
                .WithHierarchyClassName(hierarchyClassName);

            var addHierarchyClassCommand = new AddHierarchyClassCommand
            {
                NewHierarchyClass = testHierarchyClass
            };

            // When.

            commandHandler.Execute(addHierarchyClassCommand);

            // Then.
            var newHierarchyClass = context.HierarchyClass.Single(hc => hc.hierarchyClassName == testHierarchyClass.hierarchyClassName);

            Assert.AreEqual(testHierarchyClass.hierarchyID, newHierarchyClass.hierarchyID);
            Assert.AreEqual(testHierarchyClass.hierarchyClassName, newHierarchyClass.hierarchyClassName);
            Assert.AreEqual(testHierarchyClass.hierarchyParentClassID, newHierarchyClass.hierarchyParentClassID);
            Assert.AreEqual(testHierarchyClass.hierarchyLevel, newHierarchyClass.hierarchyLevel);
            Assert.IsFalse(testHierarchyClass.HierarchyClassTrait.Any(t => t.traitID == Traits.ProhibitDiscount && t.traitValue == "1"));
        }
    }
}
