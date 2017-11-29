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
            commandHandler = new DeleteHierarchyClassesCommandHandler(contextFactory);
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
            Assert.AreEqual(countAfter, countBefore - hierarchyClasses.Count,
                $"Should have found {hierarchyClasses.Count} fewer hierarchy classes after delete");
            var hierarchyClassIds = hierarchyClasses.Select(hc => hc.hierarchyClassID).ToList();
            Assert.IsFalse(
                context.HierarchyClass
                .AsNoTracking()
                .Any(hc => hierarchyClassIds.Any(id => id == hc.hierarchyClassID)));
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
    } 
}
