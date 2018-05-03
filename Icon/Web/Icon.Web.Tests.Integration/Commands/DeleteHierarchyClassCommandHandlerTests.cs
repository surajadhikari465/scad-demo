using Icon.Framework;
using Icon.Logging;
using Icon.Web.Common;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Web.Tests.Integration.Commands
{
    [TestClass]
    public class DeleteHierarchyClassCommandHandlerTests
    {
        private IconContext context;
        private DeleteHierarchyClassCommandHandler deleteHierarchyClassCommandHandler;
        private HierarchyClass testHierarchyClass;

        private void SaveHierarchyClassToDatabaseForTest()
        {
            // Remove any existing instances of the test hierarchy class traits.
            context.HierarchyClassTrait.RemoveRange(context.HierarchyClassTrait
                .Where(hct => hct.HierarchyClass.hierarchyClassName == this.testHierarchyClass.hierarchyClassName));
            context.SaveChanges();

            // Remove any existing instances of the test hierarchy class.
            context.HierarchyClass.RemoveRange(context.HierarchyClass
                .Where(hc => hc.hierarchyClassName == this.testHierarchyClass.hierarchyClassName).ToList());
            context.SaveChanges();

            // Add the hierarchy class so that it will be available for test.
            context.HierarchyClass.Add(this.testHierarchyClass);
            context.SaveChanges();
        }

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            deleteHierarchyClassCommandHandler = new DeleteHierarchyClassCommandHandler(this.context);
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (testHierarchyClass != null)
            {
                context = new IconContext();
                context.HierarchyClassTrait.RemoveRange(context.HierarchyClassTrait
                    .Where(hct => hct.HierarchyClass.hierarchyClassName == testHierarchyClass.hierarchyClassName));
                context.SaveChanges();
                context.HierarchyClass.RemoveRange(context.HierarchyClass
                    .Where(hc => hc.hierarchyClassName == testHierarchyClass.hierarchyClassName).ToList());
                context.SaveChanges();
                context.MessageQueueHierarchy.RemoveRange(context.MessageQueueHierarchy
                    .Where(mq => mq.HierarchyClassName == testHierarchyClass.hierarchyClassName));
                context.SaveChanges();
                context.Dispose();
                context = null;
            }
        }

        [TestMethod]
        public void DeleteHierarchyClass_SuccessfulExecution_HierarchyClassDeleteShouldBeAppliedForTax()
        {
            // Given.
            this.testHierarchyClass = new HierarchyClass
            {
                hierarchyID = Hierarchies.Tax,
                hierarchyClassName = "Delete Tax Hierarchy Class Integration Test",
                hierarchyLevel = 1,
                hierarchyParentClassID = null
            };
            SaveHierarchyClassToDatabaseForTest();
            Assert.IsTrue(this.testHierarchyClass.hierarchyClassID > 0, "error saving test data");

            var deleteHierarchyClassCommand = new DeleteHierarchyClassCommand
            {
                DeletedHierarchyClass = this.testHierarchyClass
            };

            // When.
            deleteHierarchyClassCommandHandler.Execute(deleteHierarchyClassCommand);

            // Then.
            bool hierarchyClassExists = context.HierarchyClass
                .Any(hc => hc.hierarchyClassID == deleteHierarchyClassCommand.DeletedHierarchyClass.hierarchyClassID);
            Assert.IsFalse(hierarchyClassExists);
        }

        [TestMethod]
        public void DeleteHierarchyClass_SuccessfulExecution_HierarchyClassDeleteShouldBeAppliedForMerch()
        {
            // Given.
            this.testHierarchyClass = new HierarchyClass
            {
                hierarchyID = Hierarchies.Merchandise,
                hierarchyClassName = "Delete Merch Hierarchy Class Integration Test",
                hierarchyLevel = 1,
                hierarchyParentClassID = null
            };
            SaveHierarchyClassToDatabaseForTest();
            Assert.IsTrue(this.testHierarchyClass.hierarchyClassID > 0, "error saving test data");

            var deleteHierarchyClassCommand = new DeleteHierarchyClassCommand
            {
                DeletedHierarchyClass = this.testHierarchyClass
            };

            // When.
            deleteHierarchyClassCommandHandler.Execute(deleteHierarchyClassCommand);

            // Then.
            bool hierarchyClassExists = context.HierarchyClass
                .Any(hc => hc.hierarchyClassID == deleteHierarchyClassCommand.DeletedHierarchyClass.hierarchyClassID);
            Assert.IsFalse(hierarchyClassExists);
        }

        [TestMethod]
        public void DeleteHierarchyClass_SuccessfulExecution_HierarchyClassDeleteShouldBeAppliedForBrand()
        {
            // Given.
            this.testHierarchyClass = new HierarchyClass
            {
                hierarchyID = Hierarchies.Brands,
                hierarchyClassName = "Delete Brand Hierarchy Class Integration Test",
                hierarchyLevel = 1,
                hierarchyParentClassID = null
            };
            SaveHierarchyClassToDatabaseForTest();
            Assert.IsTrue(this.testHierarchyClass.hierarchyClassID > 0, "error saving test data");

            var deleteHierarchyClassCommand = new DeleteHierarchyClassCommand
            {
                DeletedHierarchyClass = this.testHierarchyClass
            };

            // When.
            deleteHierarchyClassCommandHandler.Execute(deleteHierarchyClassCommand);

            // Then.
            bool hierarchyClassExists = context.HierarchyClass
                .Any(hc => hc.hierarchyClassID == deleteHierarchyClassCommand.DeletedHierarchyClass.hierarchyClassID);
            Assert.IsFalse(hierarchyClassExists);
        }

        [TestMethod]
        public void DeleteHierarchyClass_SuccessfulExecution_HierarchyClassDeleteShouldBeAppliedForNational()
        {
            // Given.
            this.testHierarchyClass = new HierarchyClass
            {
                hierarchyID = Hierarchies.National,
                hierarchyClassName = "Delete National Hierarchy Class Integration Test",
                hierarchyLevel = 1,
                hierarchyParentClassID = null
            };
            SaveHierarchyClassToDatabaseForTest();
            Assert.IsTrue(this.testHierarchyClass.hierarchyClassID > 0, "error saving test data");

            var deleteHierarchyClassCommand = new DeleteHierarchyClassCommand
            {
                DeletedHierarchyClass = this.testHierarchyClass
            };

            // When.
            deleteHierarchyClassCommandHandler.Execute(deleteHierarchyClassCommand);

            // Then.
            bool hierarchyClassExists = context.HierarchyClass
                .Any(hc => hc.hierarchyClassID == deleteHierarchyClassCommand.DeletedHierarchyClass.hierarchyClassID);
            Assert.IsFalse(hierarchyClassExists);
        }

        [TestMethod]
        public void DeleteHierarchyClass_HierarchyClassHasHierarchyClassTraits_TraitsRecordsAreAlsoDeleted()
        {
            // Given.
            this.testHierarchyClass = new HierarchyClass
            {
                hierarchyID = Hierarchies.Merchandise,
                hierarchyClassName = "Delete Hierarchy Class Integration Test",
                hierarchyLevel = 1,
                hierarchyParentClassID = null
            };
            SaveHierarchyClassToDatabaseForTest();
            Assert.IsTrue(this.testHierarchyClass.hierarchyClassID > 0, "error saving test data");

            List<HierarchyClassTrait> hierarchyClassTraits = new List<HierarchyClassTrait>
            {
                new HierarchyClassTrait
                {
                    hierarchyClassID = this.testHierarchyClass.hierarchyClassID,
                    traitID = Traits.TaxAbbreviation,
                    traitValue = "Delete Class Tax Abbrev"
                },
                new HierarchyClassTrait
                {
                    hierarchyClassID = this.testHierarchyClass.hierarchyClassID,
                    traitID = Traits.NonMerchandise,
                    traitValue = NonMerchandiseTraits.BottleDeposit
                },
                new HierarchyClassTrait
                {
                    hierarchyClassID = this.testHierarchyClass.hierarchyClassID,
                    traitID = Traits.GlAccount,
                    traitValue = "10023400001"
                },
                new HierarchyClassTrait
                {
                    hierarchyClassID = this.testHierarchyClass.hierarchyClassID,
                    traitID = Traits.MerchFinMapping,
                    traitValue = "Grocery (1000)"
                },
                new HierarchyClassTrait
                {
                    hierarchyClassID = this.testHierarchyClass.hierarchyClassID,
                    traitID = Traits.SentToEsb,
                    traitValue = DateTime.UtcNow.ToString()
                }
            };
            this.context.HierarchyClassTrait.AddRange(hierarchyClassTraits);
            this.context.SaveChanges();

            var deleteHierarchyClassCommand = new DeleteHierarchyClassCommand
            {
                DeletedHierarchyClass = this.testHierarchyClass
            };

            // When
            deleteHierarchyClassCommandHandler.Execute(deleteHierarchyClassCommand);

            // Then
            bool hierarchyClassExists = context.HierarchyClass
                .Any(hc => hc.hierarchyClassID == this.testHierarchyClass.hierarchyClassID);
            bool hierarchyClassTraitsExists = context.HierarchyClassTrait
                .Any(hct => hct.hierarchyClassID == this.testHierarchyClass.hierarchyClassID);

            Assert.IsFalse(hierarchyClassExists);
            Assert.IsFalse(hierarchyClassTraitsExists);
        }

        [TestMethod]
        [ExpectedException(typeof(CommandException))]
        public void DeleteHierarchyClass_FailedExecution_ShouldThrowCommandException()
        {
            // Given.
            this.testHierarchyClass = new HierarchyClass
            {
                hierarchyID = 1,
                hierarchyClassName = "Delete Hierarchy Class Integration Test",
                hierarchyLevel = 1,
                hierarchyParentClassID = null
            };
            var deleteHierarchyClassCommand = new DeleteHierarchyClassCommand
            {
                DeletedHierarchyClass = testHierarchyClass
            };

            // When.
            deleteHierarchyClassCommand.DeletedHierarchyClass.hierarchyClassID = 0;
            deleteHierarchyClassCommandHandler.Execute(deleteHierarchyClassCommand);
        }

        [TestMethod]
        public void DeleteHierarchyClass_SuccessfulExecution_HierarchyClassDeleteShouldNotGenerateEventForMerch()
        {
            // Given.
            this.testHierarchyClass = new HierarchyClass
            {
                hierarchyID = Hierarchies.Merchandise,
                hierarchyClassName = "Delete Merch Hierarchy Class Integration Test",
                hierarchyLevel = 1,
                hierarchyParentClassID = null
            };
            SaveHierarchyClassToDatabaseForTest();
            Assert.IsTrue(this.testHierarchyClass.hierarchyClassID > 0, "error saving test data");

            var deleteHierarchyClassCommand = new DeleteHierarchyClassCommand
            {
                DeletedHierarchyClass = this.testHierarchyClass
            };

            // When.
            deleteHierarchyClassCommandHandler.Execute(deleteHierarchyClassCommand);

            // Then.
            bool eventExists = context.EventQueue
                .Any(eq => eq.EventType.EventName == "Hierarchy Class Delete"
                        && eq.EventMessage == deleteHierarchyClassCommand.DeletedHierarchyClass.hierarchyClassName);
            Assert.IsFalse(eventExists);
        }

        [TestMethod]
        public void DeleteHierarchyClass_SuccessfulExecution_HierarchyClassDeleteShouldGenerateEventForBrand()
        {
            // Given.
            this.testHierarchyClass = new HierarchyClass
            {
                hierarchyID = Hierarchies.Brands,
                hierarchyClassName = "Delete Brand Hierarchy Class Integration Test",
                hierarchyLevel = 1,
                hierarchyParentClassID = null
            };
            SaveHierarchyClassToDatabaseForTest();
            Assert.IsTrue(this.testHierarchyClass.hierarchyClassID > 0, "error saving test data");

            var deleteHierarchyClassCommand = new DeleteHierarchyClassCommand
            {
                DeletedHierarchyClass = this.testHierarchyClass
            };

            // When.
            deleteHierarchyClassCommandHandler.Execute(deleteHierarchyClassCommand);

            // Then.
            bool eventExists = context.EventQueue
                .Any(eq => eq.EventType.EventName == "Brand Delete" 
                        && eq.EventMessage == deleteHierarchyClassCommand.DeletedHierarchyClass.hierarchyClassName);
            Assert.IsTrue(eventExists);
        }
    }
}
