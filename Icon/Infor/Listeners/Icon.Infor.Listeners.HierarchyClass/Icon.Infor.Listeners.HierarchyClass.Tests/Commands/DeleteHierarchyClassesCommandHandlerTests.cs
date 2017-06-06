using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Icon.Infor.Listeners.HierarchyClass.Commands;
using Icon.Framework;
using System.Data.Entity;
using Icon.Common.Context;
using Moq;
using System.Collections.Generic;
using Icon.Infor.Listeners.HierarchyClass.Models;
using System.Linq;
using Icon.Infor.Listeners.HierarchyClass.Constants;

namespace Icon.Infor.Listeners.HierarchyClass.Tests.Commands
{
    [TestClass]
    public class DeleteHierarchyClassesCommandHandlerTests : BaseHierarchyClassesCommandTest
    {
        private DeleteHierarchyClassesCommandHandler commandHandler;
        private DeleteHierarchyClassesCommand command;

        [TestInitialize]
        public void Initialize()
        {
            commandHandler = new DeleteHierarchyClassesCommandHandler(mockRenewableContext.Object);
            command = new DeleteHierarchyClassesCommand();
        }

        [TestMethod]
        public void DeleteHierarchyClassesCommand_Brand_WhenExists_DeletesBrandClass()
        {
            //Given
            var hierarchyClasses = CreateTestHierarchyClassesForDelete(Hierarchies.Brands, 3);
            SaveTestHierarchyClasses(context, hierarchyClasses);
            AddDataToDeleteCommand(command, hierarchyClasses, Hierarchies.Names.Brands);
            var countBefore = context.HierarchyClass.Count(hc => hc.hierarchyID == Hierarchies.Brands);
            //When
            commandHandler.Execute(command);
            //Then
            var countAfter = context.HierarchyClass.Count(hc => hc.hierarchyID == Hierarchies.Brands);
            Assert.AreEqual(countAfter, countBefore- hierarchyClasses.Count,
                $"Should have found {hierarchyClasses.Count} fewer hierarchy classes after delete");
            var hierarchyClassIds = hierarchyClasses.Select(hc => hc.hierarchyClassID).ToList();
            Assert.IsFalse(
                context.HierarchyClass
                .AsNoTracking()
                .Any(hc => hierarchyClassIds.Any(id => id == hc.hierarchyClassID)));
        }

        [TestMethod]
        public void DeleteHierarchyClassesCommand_Brand_WhenNotFound_DoesNotDeleteBrandClass()
        {
            //Given
            var hierarchyClasses = CreateTestHierarchyClassesForDelete(Hierarchies.Brands, 3);
            AddDataToDeleteCommand(command, hierarchyClasses, Hierarchies.Names.Brands);
            var countBefore = context.HierarchyClass.Count(hc => hc.hierarchyID == Hierarchies.Brands);
            //When
            commandHandler.Execute(command);
            //Then
            var countAfter = context.HierarchyClass.Count(hc => hc.hierarchyID == Hierarchies.Brands);
            Assert.AreEqual(countAfter, countBefore,
                "Attempted delete of non-existent hierarchy classes should not have affected count");
        }

        [TestMethod]
        public void DeleteHierarchyClassesCommand_Brand_WhenNotFound_ShouldNotThrowException()
        {
            //Given
            var hierarchyClasses = CreateTestHierarchyClassesForDelete(Hierarchies.Brands, 3);
            AddDataToDeleteCommand(command, hierarchyClasses, Hierarchies.Names.Brands);
            //When
            try
            {
                commandHandler.Execute(command);
            }
            //Then
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception occurred: " + ex.Message);
            };
        }

        [TestMethod]
        public void DeleteHierarchyClassesCommand_Brand_WhenNotFound_ShouldSetError()
        {
            //Given
            var hierarchyClasses = CreateTestHierarchyClassesForDelete(Hierarchies.Brands, 3);
            AddDataToDeleteCommand(command, hierarchyClasses, Hierarchies.Names.Brands);
            //When
            commandHandler.Execute(command);
            //Then
            foreach( var undeleteableHierarchyClass in command.HierarchyClasses)
            {
                Assert.AreEqual(ApplicationErrors.Codes.UnableToFindMatchingHierarchyClass,
                    undeleteableHierarchyClass.ErrorCode);
                Assert.AreEqual(ApplicationErrors.Descriptions.UnableToFindMatchingHierarchyClassToDeleteMessage,
                    undeleteableHierarchyClass.ErrorDetails);
            }
        }

        [TestMethod]
        public void DeleteHierarchyClassesCommand_Brand_WhenAssocWithItem_ShouldNotDeleteAssociatedHierarchies()
        {
            //Given
            var hierarchyClasses = CreateTestHierarchyClassesForDelete(Hierarchies.Brands, 2, true);
            SaveTestHierarchyClasses(context, hierarchyClasses);
            AddDataToDeleteCommand(command, hierarchyClasses, Hierarchies.Names.Brands);
            var expectedRemainingHierarchyClassID = hierarchyClasses
                .Single(hc => hc.hierarchyClassName == "Test Brand 1").hierarchyClassID;
            //When
            commandHandler.Execute(command);
            //Then
            var hierarchyClassIds = command.HierarchyClasses.Select(hc => hc.HierarchyClassId).ToList();
            Assert.AreEqual(1, context.HierarchyClass.AsNoTracking()
                .Count(hc => hierarchyClassIds.Any(id => id == hc.hierarchyClassID)));
            Assert.AreEqual(expectedRemainingHierarchyClassID, context.HierarchyClass
                .Where(hc => hc.hierarchyClassName == "Test Brand 1").Single().hierarchyClassID);
            Assert.IsFalse(context.HierarchyClass.Any(hc => hc.hierarchyClassName == "Test Brand 2"));
        }

        [TestMethod]
        public void DeleteHierarchyClassesCommand_Brand_WhenAssocWithItem_ShouldSetError()
        {
            //Given
            var hierarchyClasses = CreateTestHierarchyClassesForDelete(Hierarchies.Brands, 2, true);
            SaveTestHierarchyClasses(context, hierarchyClasses);
            AddDataToDeleteCommand(command, hierarchyClasses, Hierarchies.Names.Brands);
            var expectedRemainingHierarchyClassID = hierarchyClasses
                .Single(hc => hc.hierarchyClassName == "Test Brand 1").hierarchyClassID;
            //When
            commandHandler.Execute(command);
            //Then
            var hierarchyClassStillAssociatedWithItem = command.HierarchyClasses
                .Single(hc => hc.HierarchyClassId == expectedRemainingHierarchyClassID);
            Assert.AreEqual(ApplicationErrors.Codes.HierarchyClassAssociatedToItemsOnDelete,
                hierarchyClassStillAssociatedWithItem.ErrorCode);
            Assert.AreEqual(ApplicationErrors.Descriptions.HierarchyClassAssociatedToItemsOnDelete,
                hierarchyClassStillAssociatedWithItem.ErrorDetails);
        } 

        [TestMethod]
        public void DeleteHierarchyClassesCommand_National_WhenExists_DeletesNationalClass()
        {
            //Given
            var hierarchyClasses = CreateTestHierarchyClassesForDelete(Hierarchies.National, 3);
            SaveTestHierarchyClasses(context, hierarchyClasses);
            AddDataToDeleteCommand(command, hierarchyClasses, Hierarchies.Names.National);
            var countBefore = context.HierarchyClass
                .Count(hc => hc.hierarchyID == Hierarchies.National);
            //When
            commandHandler.Execute(command);
            //Then
            var countAfter = context.HierarchyClass
                .Count(hc => hc.hierarchyID == Hierarchies.National);
            Assert.AreEqual(countAfter, countBefore - hierarchyClasses.Count,
                $"Should have found {hierarchyClasses.Count} fewer hierarchy classes after delete");
            var hierarchyClassIds = hierarchyClasses
                .Select(hc => hc.hierarchyClassID).ToList();
            Assert.IsFalse(
                context.HierarchyClass
                .AsNoTracking()
                .Any(hc => hierarchyClassIds.Any(id => id == hc.hierarchyClassID)));
        }

        [TestMethod]
        public void DeleteHierarchyClassesCommand_National_WhenNotFound_DoesNotDeleteNationalClass()
        {
            //Given
            var hierarchyClasses = CreateTestHierarchyClassesForDelete(Hierarchies.National, 3);
            AddDataToDeleteCommand(command, hierarchyClasses, Hierarchies.Names.National); ;
            var countBefore = context.HierarchyClass
                .Count(hc => hc.hierarchyID == Hierarchies.National);
            //When
            commandHandler.Execute(command);
            //Then
            var countAfter = context.HierarchyClass
                .Count(hc => hc.hierarchyID == Hierarchies.National);
            Assert.AreEqual(countAfter, countBefore,
                "Attempted delete of non-existent hierarchy classes should not have affected count");
        }

        [TestMethod]
        public void DeleteHierarchyClassesCommand_National_WhenNotFound_ShouldNotThrowException()
        {
            //Given
            var hierarchyClasses = CreateTestHierarchyClassesForDelete(Hierarchies.National, 3);
            AddDataToDeleteCommand(command, hierarchyClasses, Hierarchies.Names.National);
            //When
            try
            {
                commandHandler.Execute(command);
            }
            //Then
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception occurred: " + ex.Message);
            }
        }

        [TestMethod]
        public void DeleteHierarchyClassesCommand_National_WhenNotFound_ShouldSetError()
        {
            //Given
            var hierarchyClasses = CreateTestHierarchyClassesForDelete(Hierarchies.National, 3);
            AddDataToDeleteCommand(command, hierarchyClasses, Hierarchies.Names.National);
            //When
            commandHandler.Execute(command);
            //Then
            foreach (var undeleteableHierarchyClass in command.HierarchyClasses)
            {
                Assert.AreEqual(ApplicationErrors.Codes.UnableToFindMatchingHierarchyClass,
                    undeleteableHierarchyClass.ErrorCode);
                Assert.AreEqual(ApplicationErrors.Descriptions.UnableToFindMatchingHierarchyClassToDeleteMessage,
                    undeleteableHierarchyClass.ErrorDetails);
            }
        }

        [TestMethod]
        public void DeleteHierarchyClassesCommand_National_WhenAssocWithItem_ShouldNotDeleteAssociatedHierarchies()
        {
            //Given
            var hierarchyClasses = CreateTestHierarchyClassesForDelete(Hierarchies.National, 2, true);
            SaveTestHierarchyClasses(context, hierarchyClasses);
            AddDataToDeleteCommand(command, hierarchyClasses, Hierarchies.Names.National);
            var expectedRemainingHierarchyClassID = hierarchyClasses
                .Single(hc => hc.hierarchyClassName == "Test National 1").hierarchyClassID;
            //When
            commandHandler.Execute(command);
            //Then
            var hierarchyClassIds = command.HierarchyClasses.Select(hc => hc.HierarchyClassId).ToList();
            Assert.AreEqual(1, context.HierarchyClass.AsNoTracking()
                .Count(hc => hierarchyClassIds.Any(id => id == hc.hierarchyClassID)));
            Assert.AreEqual(expectedRemainingHierarchyClassID, context.HierarchyClass
                .Where(hc => hc.hierarchyClassName == "Test National 1").Single().hierarchyClassID);
            Assert.IsFalse(context.HierarchyClass.Any(hc => hc.hierarchyClassName == "Test National 2"));
        }

        [TestMethod]
        public void DeleteHierarchyClassesCommand_National_WhenAssocWithItem_ShouldSetError()
        {
            //Given
            var hierarchyClasses = CreateTestHierarchyClassesForDelete(Hierarchies.National, 2, true);
            SaveTestHierarchyClasses(context, hierarchyClasses);
            AddDataToDeleteCommand(command, hierarchyClasses, Hierarchies.Names.National);
            var expectedRemainingHierarchyClassID = hierarchyClasses
                .Single(hc => hc.hierarchyClassName == "Test National 1").hierarchyClassID;

            //When
            commandHandler.Execute(command);

            //Then
            var hierarchyClassStillAssociatedWithItem = command.HierarchyClasses
                .Single(hc => hc.HierarchyClassId == expectedRemainingHierarchyClassID);
            Assert.AreEqual(ApplicationErrors.Codes.HierarchyClassAssociatedToItemsOnDelete,
                hierarchyClassStillAssociatedWithItem.ErrorCode);
            Assert.AreEqual(ApplicationErrors.Descriptions.HierarchyClassAssociatedToItemsOnDelete,
                hierarchyClassStillAssociatedWithItem.ErrorDetails);
        }
    } 
}
