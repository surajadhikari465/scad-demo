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
    [TestClass] [Ignore]
    public class DeleteHierarchyClassCommandHandlerTests
    {
        private IconContext context;
        private DeleteHierarchyClassCommand deleteHierarchyClassCommand;
        private DeleteHierarchyClassCommandHandler deleteHierarchyClassCommandHandler;
        private int hierarchyClassId;
        
        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            deleteHierarchyClassCommandHandler = new DeleteHierarchyClassCommandHandler(this.context);
            deleteHierarchyClassCommand = new DeleteHierarchyClassCommand
            {
                DeletedHierarchyClass = new HierarchyClass
                {
                    hierarchyID = 1,
                    hierarchyClassName = "Edit Hierarchy Class Integration Test"
                }
            };

            // Remove any existing instances of the test hierarchy class traits.
            context.HierarchyClassTrait.RemoveRange(context.HierarchyClassTrait
                .Where(hct => hct.HierarchyClass.hierarchyClassName == deleteHierarchyClassCommand.DeletedHierarchyClass.hierarchyClassName));
            context.SaveChanges();

            // Remove any existing instances of the test hierarchy class.
            context.HierarchyClass.RemoveRange(context.HierarchyClass.Where(hc => hc.hierarchyClassName == deleteHierarchyClassCommand.DeletedHierarchyClass.hierarchyClassName).ToList());
            context.SaveChanges();

            // Add the hierarchy class so that it will be available for edit.
            context.HierarchyClass.Add(deleteHierarchyClassCommand.DeletedHierarchyClass);
            context.SaveChanges();

            // Capture the just-added hierarchy class ID.
            hierarchyClassId = deleteHierarchyClassCommand.DeletedHierarchyClass.hierarchyClassID;
        }

        [TestCleanup]
        public void Cleanup()
        {
            context = new IconContext();
            context.HierarchyClassTrait.RemoveRange(context.HierarchyClassTrait.Where(hct => hct.HierarchyClass.hierarchyClassName == deleteHierarchyClassCommand.DeletedHierarchyClass.hierarchyClassName));
            context.SaveChanges();
            context.HierarchyClass.RemoveRange(context.HierarchyClass.Where(hc => hc.hierarchyClassName == deleteHierarchyClassCommand.DeletedHierarchyClass.hierarchyClassName).ToList());
            context.SaveChanges();
            context.MessageQueueHierarchy.RemoveRange(context.MessageQueueHierarchy.Where(mq => mq.HierarchyClassName == deleteHierarchyClassCommand.DeletedHierarchyClass.hierarchyClassName));
            context.SaveChanges();
            context.Dispose();
            context = null;
        }

        [TestMethod]
        public void DeleteHierarchyClass_SuccessfulExecution_HierarchyClassDeleteShouldBeApplied()
        {
            // When.
            deleteHierarchyClassCommand.DeletedHierarchyClass.hierarchyClassID = hierarchyClassId;
            deleteHierarchyClassCommand.DeletedHierarchyClass.hierarchyLevel = 1;
            deleteHierarchyClassCommand.DeletedHierarchyClass.hierarchyID = 3;
            deleteHierarchyClassCommand.DeletedHierarchyClass.hierarchyParentClassID = null;
            deleteHierarchyClassCommandHandler.Execute(deleteHierarchyClassCommand);

            // Then.
            bool hierarchyClassExists = context.HierarchyClass.Any(hc => hc.hierarchyClassID == deleteHierarchyClassCommand.DeletedHierarchyClass.hierarchyClassID);
            Assert.IsFalse(hierarchyClassExists);
        }

        [TestMethod]
        public void DeleteHierarchyClass_HierarchyClassHasHierarchyClassTraits_TraitsRecordsAreAlsoDeleted()
        {
            // Given
            deleteHierarchyClassCommand.DeletedHierarchyClass.hierarchyClassID = hierarchyClassId;
            deleteHierarchyClassCommand.DeletedHierarchyClass.hierarchyLevel = 1;
            deleteHierarchyClassCommand.DeletedHierarchyClass.hierarchyID = Hierarchies.Merchandise;
            deleteHierarchyClassCommand.DeletedHierarchyClass.hierarchyParentClassID = null;
            List<HierarchyClassTrait> hierarchyClassTraits = new List<HierarchyClassTrait>
            {
                new HierarchyClassTrait
                {
                    hierarchyClassID = hierarchyClassId,
                    traitID = Traits.TaxAbbreviation,
                    traitValue = "Delete Class Tax Abbrev"
                },
                new HierarchyClassTrait
                {
                    hierarchyClassID = hierarchyClassId,
                    traitID = Traits.NonMerchandise,
                    traitValue = NonMerchandiseTraits.BottleDeposit
                },
                new HierarchyClassTrait
                {
                    hierarchyClassID = hierarchyClassId,
                    traitID = Traits.GlAccount,
                    traitValue = "10023400001"
                },
                new HierarchyClassTrait
                {
                    hierarchyClassID = hierarchyClassId,
                    traitID = Traits.MerchFinMapping,
                    traitValue = "Grocery (1000)"
                },
                new HierarchyClassTrait
                {
                    hierarchyClassID = hierarchyClassId,
                    traitID = Traits.SentToEsb,
                    traitValue = DateTime.UtcNow.ToString()
                }
            };
            this.context.HierarchyClassTrait.AddRange(hierarchyClassTraits);
            this.context.SaveChanges();

            // When
            deleteHierarchyClassCommandHandler.Execute(deleteHierarchyClassCommand);

            // Then
            bool hierarchyClassExists = context.HierarchyClass.Any(hc => hc.hierarchyClassID == hierarchyClassId);
            bool hierarchyClassTraitsExists = context.HierarchyClassTrait.Any(hct => hct.hierarchyClassID == hierarchyClassId);

            Assert.IsFalse(hierarchyClassExists);
            Assert.IsFalse(hierarchyClassTraitsExists);
        }

        [TestMethod]
        [ExpectedException(typeof(CommandException))]
        public void EditHierarchyClass_FailedExecution_ShouldThrowCommandException()
        {
            // When.
            deleteHierarchyClassCommand.DeletedHierarchyClass.hierarchyClassID = 0;
            deleteHierarchyClassCommandHandler.Execute(deleteHierarchyClassCommand);
        }
    }
}
