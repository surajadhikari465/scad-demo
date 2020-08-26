using Icon.Framework;
using Icon.Logging;
using Icon.Web.DataAccess.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Data.Entity;
using System.Linq;

namespace Icon.Web.Tests.Integration.Commands
{
    [TestClass] 
    public class UpdateHierarchyClassCommandHandlerTests
    {
        private IconContext context;
        private UpdateHierarchyClassCommand editHierarchyClassCommand;
        private UpdateHierarchyClassCommandHandler editHierarchyClassCommandHandler;
        private Mock<ILogger> mockLogger;
        private int hierarchyClassId;
        private int secondHierarchyClassId;
        private int thirdHierarchyClassId;
        private int taxHierarchyClassId;
        private int subBrickId;
        private int brickId;
        private HierarchyClass editedHierarchyClass;
        private HierarchyClass secondHierarchyClass;
        private HierarchyClass thirdHierarchyClass;
        private HierarchyClass taxHierarchyClass;
        private HierarchyClass brandHierarchyClass;
        private HierarchyClass subBrick;
        private HierarchyClass brick;
        private int irmaItemId;
        private int brandClassId;
        private DbContextTransaction transaction;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            transaction = context.Database.BeginTransaction();

            editHierarchyClassCommand = new UpdateHierarchyClassCommand
            {
                UpdatedHierarchyClass = new HierarchyClass
                {
                    hierarchyID = 1,
                    hierarchyClassName = "Edit Hierarchy Class Integration Test",
                    hierarchyLevel = 5
                }
            };

            IRMAItem irmaItem = new IRMAItem
            {
                identifier = "99889988777",
                defaultIdentifier = true,
                regioncode = "MW",
                brandName = "Test Brand Class",
                itemDescription = "IRMA Test Item Description 1",
                posDescription = "IRMA Test Pos Description 1",
                packageUnit = 3,
                foodStamp = true,
                posScaleTare = 0,
                departmentSale = true,
                insertDate = DateTime.Now,
                retailSize = 5,
                retailUom = "EACH"
            };

            editedHierarchyClass = new HierarchyClass { hierarchyClassName = "Edited Hierarchy Class Integration Test" };

            // Remove any existing instances of the test hierarchy class.
            context.HierarchyClassTrait.RemoveRange(context.HierarchyClassTrait.Where(hct => hct.HierarchyClass.hierarchyClassName == "Auto Itgrtn Test Brick").ToList());
            context.HierarchyClassTrait.RemoveRange(context.HierarchyClassTrait.Where(hct => hct.HierarchyClass.hierarchyClassName == "Auto Itgrtn Test Sub-Brick").ToList());
            context.HierarchyClassTrait.RemoveRange(context.HierarchyClassTrait.Where(hct => hct.HierarchyClass.hierarchyClassName == "Auto Intgrtn Test Tax Class").ToList());
            context.HierarchyClass.RemoveRange(context.HierarchyClass.Where(hc => hc.hierarchyClassName == editHierarchyClassCommand.UpdatedHierarchyClass.hierarchyClassName).ToList());
            context.HierarchyClass.RemoveRange(context.HierarchyClass.Where(hc => hc.hierarchyClassName == "Some Random Hierarchy Class Name").ToList());
            context.HierarchyClass.RemoveRange(context.HierarchyClass.Where(hc => hc.hierarchyClassName == "Another Random Hierarchy Class Name").ToList());
            context.HierarchyClass.RemoveRange(context.HierarchyClass.Where(hc => hc.hierarchyClassName == "Integration Test Tax Class").ToList());
            context.HierarchyClass.RemoveRange(context.HierarchyClass.Where(hc => hc.hierarchyClassName == "Auto Itgrtn Test Sub-Brick").ToList());
            context.HierarchyClass.RemoveRange(context.HierarchyClass.Where(hc => hc.hierarchyClassName == "Auto Itgrtn Test Brick").ToList());
            context.HierarchyClass.RemoveRange(context.HierarchyClass.Where(hc => hc.hierarchyClassName == "Test Brand Class").ToList());
            context.SaveChanges();

            // Remove any existing instances of the edited version of the test hierarchy class.
            context.HierarchyClass.RemoveRange(context.HierarchyClass.Where(hc => hc.hierarchyClassName == editedHierarchyClass.hierarchyClassName).ToList());
            context.SaveChanges();

            // Remove any instances of this update in the queue table.
            context.EventQueue.RemoveRange(context.EventQueue.Where(q => q.EventMessage == editedHierarchyClass.hierarchyClassName).ToList());
            context.SaveChanges();

            //Remove exisitng IrmaItem
            context.IRMAItem.RemoveRange(context.IRMAItem.Where(ii => ii.identifier == "728795612402").ToList());
            context.SaveChanges();

            // Add the hierarchy class so that it will be available for edit.
            context.HierarchyClass.Add(editHierarchyClassCommand.UpdatedHierarchyClass);
            context.SaveChanges();

            // Add a second & third hierarchy class for testing against duplicate prevention
            secondHierarchyClass = new HierarchyClass { hierarchyID = 1, hierarchyClassName = "Some Random Hierarchy Class Name", hierarchyLevel = 5, hierarchyParentClassID = null };
            thirdHierarchyClass = new HierarchyClass { hierarchyID = 1, hierarchyClassName = "Another Random Hierarchy Class Name", hierarchyLevel = 2, hierarchyParentClassID = null };
            taxHierarchyClass = new HierarchyClass
            {
                hierarchyID = context.Hierarchy.FirstOrDefault(h => h.hierarchyName == HierarchyNames.Tax).hierarchyID,
                hierarchyClassName = "Auto Intgrtn Test Tax Class",
                hierarchyLevel = 1,
                hierarchyParentClassID = null
            };
            subBrick = new HierarchyClass
            {
                hierarchyID = context.Hierarchy.FirstOrDefault(h => h.hierarchyName == HierarchyNames.Merchandise).hierarchyID,
                hierarchyClassName = "Auto Itgrtn Test Sub-Brick",
                hierarchyLevel = 5,
                hierarchyParentClassID = null
            };
            brick = new HierarchyClass
            {
                hierarchyID = context.Hierarchy.FirstOrDefault(h => h.hierarchyName == HierarchyNames.Merchandise).hierarchyID,
                hierarchyClassName = "Auto Itgrtn Test Brick",
                hierarchyLevel = 4,
                hierarchyParentClassID = null
            };

            brandHierarchyClass = new HierarchyClass
            {
                hierarchyID = context.Hierarchy.FirstOrDefault(h => h.hierarchyName == HierarchyNames.Brands).hierarchyID,
                hierarchyClassName = "Test Brand Class",
                hierarchyLevel = 1,
                hierarchyParentClassID = null
            };

            context.HierarchyClass.Add(secondHierarchyClass);
            context.HierarchyClass.Add(thirdHierarchyClass);
            context.HierarchyClass.Add(taxHierarchyClass);
            context.HierarchyClass.Add(subBrick);
            context.HierarchyClass.Add(brick);
            context.HierarchyClass.Add(brandHierarchyClass);
            context.IRMAItem.Add(irmaItem);
            context.SaveChanges();

            // Capture the just-added hierarchy class IDs.
            hierarchyClassId = editHierarchyClassCommand.UpdatedHierarchyClass.hierarchyClassID;
            secondHierarchyClassId = secondHierarchyClass.hierarchyClassID;
            thirdHierarchyClassId = thirdHierarchyClass.hierarchyClassID;
            taxHierarchyClassId = taxHierarchyClass.hierarchyClassID;
            subBrickId = subBrick.hierarchyClassID;
            brickId = brick.hierarchyClassID;
            brandClassId = brandHierarchyClass.hierarchyClassID;
            irmaItemId = irmaItem.irmaItemID;

            mockLogger = new Mock<ILogger>();

            editHierarchyClassCommandHandler = new UpdateHierarchyClassCommandHandler(this.context);
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
            context.Dispose();
        }

        [TestMethod]
        public void EditHierarchyClass_SuccessfulExecution_OnlyHierarchyClassUpdateShouldBeApplied()
        {
            // Given.
            var a = context.HierarchyClass.Single(hc => hc.hierarchyClassID == hierarchyClassId);
            editedHierarchyClass.hierarchyClassID = hierarchyClassId;
            editedHierarchyClass.hierarchyID = 1;
            var command = new UpdateHierarchyClassCommand { UpdatedHierarchyClass = editedHierarchyClass };

            // When.
            editHierarchyClassCommandHandler.Execute(command);

            // Then.
            var newHierarchyClassName = context.HierarchyClass.Single(hc => hc.hierarchyClassName == editedHierarchyClass.hierarchyClassName).hierarchyClassName;
            Assert.AreEqual(editedHierarchyClass.hierarchyClassName, newHierarchyClassName);

            // Verify no HierarchyClassTraits were added/changed.
            Assert.IsTrue(context.HierarchyClassTrait.Where(hct => hct.hierarchyClassID == editedHierarchyClass.hierarchyClassID).Count() == 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void EditHierarchyClass_DuplicateName_ExceptionShouldBeThrown()
        {
            // Given.
            var command = new UpdateHierarchyClassCommand
            {
                UpdatedHierarchyClass = new HierarchyClass
                {
                    hierarchyClassID = secondHierarchyClassId,
                    hierarchyClassName = "Edit Hierarchy Class Integration Test",
                    hierarchyLevel = secondHierarchyClass.hierarchyLevel,
                    hierarchyID = secondHierarchyClass.hierarchyID
                }
            };

            // When.
            editHierarchyClassCommandHandler.Execute(command);

            // Then.
            // Expected exception.
        }

        [TestMethod]
        public void EditHierarchyClass_DuplicateNameDifferentLevel_HierarchyClassUpdateShouldOccur()
        {
            // Given
            var command = new UpdateHierarchyClassCommand
            {
                UpdatedHierarchyClass = new HierarchyClass
                {
                    hierarchyClassID = thirdHierarchyClassId,
                    hierarchyClassName = "Edit Hierarchy Class Integration Test",
                    hierarchyLevel = thirdHierarchyClass.hierarchyLevel,
                    hierarchyID = thirdHierarchyClass.hierarchyID
                }
            };

            string expectedName = command.UpdatedHierarchyClass.hierarchyClassName;

            // When
            editHierarchyClassCommandHandler.Execute(command);

            // Then
            var entry = context.Entry(thirdHierarchyClass);
            bool modified = entry.Property("hierarchyClassName").IsModified;
            var actualName = context.HierarchyClass.Single(hc => hc.hierarchyClassID == thirdHierarchyClassId).hierarchyClassName;

            Assert.IsFalse(modified);
            Assert.AreEqual(expectedName, actualName);
        }

        [TestMethod]
        public void EditHierarchyClass_UpdateName_ShouldUpdateHierarchyClassName()
        {
            //Given
            string originalName = "Test HierarchyClass Name";
            string updatedName = "Test Updated HierarchyClass Name";

            context.HierarchyClass.RemoveRange(
                context.HierarchyClass.Where(hc => hc.hierarchyClassName == originalName || hc.hierarchyClassName == updatedName));
            context.SaveChanges();

            UpdateHierarchyClassCommandHandler handler = new UpdateHierarchyClassCommandHandler(context);
            HierarchyClass testHierarchyClass = new HierarchyClass
            {
                hierarchyClassName = originalName,
                hierarchyID = Hierarchies.Tax,
                hierarchyLevel = 1,
                Hierarchy = context.Hierarchy.First(h => h.hierarchyID == Hierarchies.Tax)
            };
            context.HierarchyClass.Add(testHierarchyClass);
            context.SaveChanges();

            //When
            handler.Execute(new UpdateHierarchyClassCommand
            {
                UpdatedHierarchyClass = new HierarchyClass
                {
                    hierarchyClassID = testHierarchyClass.hierarchyClassID,
                    hierarchyClassName = updatedName,
                    hierarchyID = Hierarchies.Tax,
                    hierarchyLevel = 1,
                    Hierarchy = context.Hierarchy.First(h => h.hierarchyID == Hierarchies.Tax)
                }
            });

            //Then
            Assert.AreEqual(updatedName, testHierarchyClass.hierarchyClassName);
            Assert.AreEqual(Hierarchies.Tax, testHierarchyClass.hierarchyID);
            Assert.AreEqual(1, testHierarchyClass.hierarchyLevel);

            //Cleanup
            context.HierarchyClass.RemoveRange(
                context.HierarchyClass.Where(hc => hc.hierarchyClassName == originalName || hc.hierarchyClassName == updatedName));
            context.SaveChanges();
        }

        [TestMethod]
        public void EditHierarchyClass_NoUpdateToName_ShouldHaveOriginalHierarchyClassName()
        {
            //Given
            string originalName = "Test HierarchyClass Name";

            context.HierarchyClass.RemoveRange(
                context.HierarchyClass.Where(hc => hc.hierarchyClassName == originalName));
            context.SaveChanges();

            UpdateHierarchyClassCommandHandler handler = new UpdateHierarchyClassCommandHandler(context);
            HierarchyClass testHierarchyClass = new HierarchyClass
            {
                hierarchyClassName = originalName,
                hierarchyID = Hierarchies.Tax,
                hierarchyLevel = 1,
                Hierarchy = context.Hierarchy.First(h => h.hierarchyID == Hierarchies.Tax)
            };
            context.HierarchyClass.Add(testHierarchyClass);
            context.SaveChanges();

            //When
            handler.Execute(new UpdateHierarchyClassCommand
            {
                UpdatedHierarchyClass = new HierarchyClass
                {
                    hierarchyClassID = testHierarchyClass.hierarchyClassID,
                    hierarchyClassName = originalName,
                    hierarchyID = Hierarchies.Tax,
                    hierarchyLevel = 1,
                    Hierarchy = context.Hierarchy.First(h => h.hierarchyID == Hierarchies.Tax)
                }
            });

            //Then
            Assert.AreEqual(originalName, testHierarchyClass.hierarchyClassName);
            Assert.AreEqual(Hierarchies.Tax, testHierarchyClass.hierarchyID);
            Assert.AreEqual(1, testHierarchyClass.hierarchyLevel);

            //Cleanup
            context.HierarchyClass.RemoveRange(
                context.HierarchyClass.Where(hc => hc.hierarchyClassName == originalName));
            context.SaveChanges();
        }
    }
}