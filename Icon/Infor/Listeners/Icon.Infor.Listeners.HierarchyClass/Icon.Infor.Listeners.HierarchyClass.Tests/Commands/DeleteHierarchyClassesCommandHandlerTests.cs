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
    public class DeleteHierarchyClassesCommandHandlerTests
    {
        private DeleteHierarchyClassesCommandHandler commandHandler;
        private DeleteHierarchyClassesCommand command;
        private IconContext context;
        private DbContextTransaction transaction;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            transaction = context.Database.BeginTransaction();

            Mock<IRenewableContext<IconContext>> mockContext = new Mock<IRenewableContext<IconContext>>();
            mockContext.SetupGet(m => m.Context).Returns(context);

            commandHandler = new DeleteHierarchyClassesCommandHandler(mockContext.Object);
            command = new DeleteHierarchyClassesCommand();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
            transaction.Dispose();
            context.Dispose();
        }

        [TestMethod]
        public void DeleteHierarchyClasses_HierarchyClassesExist_ShouldDeleteHierarchyClasses()
        {
            //Given
            List<Framework.HierarchyClass> hierarchyClasses = new List<Framework.HierarchyClass>()
            {
                new Framework.HierarchyClass
                {
                    hierarchyID = Hierarchies.Brands,
                    hierarchyClassName = "Test Brand 1",
                    hierarchyLevel = 1, HierarchyClassTrait = new List<HierarchyClassTrait> { new HierarchyClassTrait { traitID = Traits.BrandAbbreviation, traitValue = "Test" } }
                },
                new Framework.HierarchyClass
                {
                    hierarchyID = Hierarchies.Brands,
                    hierarchyClassName = "Test Brand 2",
                    hierarchyLevel = 1, HierarchyClassTrait = new List<HierarchyClassTrait> { new HierarchyClassTrait { traitID = Traits.BrandAbbreviation, traitValue = "Test" } }
                },
                new Framework.HierarchyClass
                {
                    hierarchyID = Hierarchies.Brands,
                    hierarchyClassName = "Test Brand 3",
                    hierarchyLevel = 1, HierarchyClassTrait = new List<HierarchyClassTrait> { new HierarchyClassTrait { traitID = Traits.BrandAbbreviation, traitValue = "Test" } }
                }
            };
            context.HierarchyClass.AddRange(hierarchyClasses);
            context.SaveChanges();

            command.HierarchyClasses = hierarchyClasses.Select(hc => new HierarchyClassModel
            {
                HierarchyClassId = hc.hierarchyClassID,
                HierarchyName = Hierarchies.Names.Brands
            });

            //When
            commandHandler.Execute(command);

            //Then
            var hierarchyClassIds = hierarchyClasses.Select(hc => hc.hierarchyClassID).ToList();
            Assert.IsFalse(
                context.HierarchyClass
                .AsNoTracking()
                .Any(hc => hierarchyClassIds.Any(id => id == hc.hierarchyClassID)));
        }

        [TestMethod]
        public void DeleteHierarchyClasses_HierarchyClassesDoNotExist_ShouldNotThrowException()
        {
            //Given
            var maxId = context.HierarchyClass.Max(hc => hc.hierarchyClassID);
            command.HierarchyClasses = new List<HierarchyClassModel>
            {
                new HierarchyClassModel { HierarchyClassId = (maxId + 1) },
                new HierarchyClassModel { HierarchyClassId = (maxId + 2) },
                new HierarchyClassModel { HierarchyClassId = (maxId + 3) }
            };

            //When
            commandHandler.Execute(command);

            //Then
            var hierarchyClassIds = command.HierarchyClasses.Select(hc => hc.HierarchyClassId).ToList();
            Assert.IsFalse(
                context.HierarchyClass
                .AsNoTracking()
                .Any(hc => hierarchyClassIds.Any(id => id == hc.hierarchyClassID)));
        }

        [TestMethod]
        public void DeleteHierarchyClasses_ItemHierarchyClassesExist_ShouldSetItemAssociatedErrorAndNotDeleteAssociatedBrands()
        {
            //Given
            List<Framework.HierarchyClass> hierarchyClasses = new List<Framework.HierarchyClass>()
            {
                new Framework.HierarchyClass
                {
                    hierarchyID = Hierarchies.Brands,
                    hierarchyClassName = "Test Brand 1",
                    hierarchyLevel = 1, ItemHierarchyClass = new List<ItemHierarchyClass> {
                        new ItemHierarchyClass { Item =
                            new Item { itemTypeID = ItemTypes.RetailSale } } }
                },
                 new Framework.HierarchyClass
                {
                    hierarchyID = Hierarchies.Brands,
                    hierarchyClassName = "Test Brand 2",
                    hierarchyLevel = 1,

                }
            };
            context.HierarchyClass.AddRange(hierarchyClasses);
            context.SaveChanges();

            command.HierarchyClasses = hierarchyClasses.Select(hc => new HierarchyClassModel
            {
                HierarchyClassId = hc.hierarchyClassID,
                HierarchyName = Hierarchies.Names.Brands
            }).ToList();
            var expectedHierarchyClassRemains = hierarchyClasses.Single(hc => hc.hierarchyClassName == "Test Brand 1").hierarchyClassID;

            //When
            commandHandler.Execute(command);

            //Then
            Assert.AreEqual(ApplicationErrors.Codes.HierarchyClassAssociatedToItemsOnDelete, command.HierarchyClasses.Single(hc => hc.HierarchyClassId == expectedHierarchyClassRemains).ErrorCode);
            Assert.AreEqual(ApplicationErrors.Descriptions.HierarchyClassAssociatedToItemsOnDelete, command.HierarchyClasses.Single(hc => hc.HierarchyClassId == expectedHierarchyClassRemains).ErrorDetails);

            Assert.AreEqual(expectedHierarchyClassRemains, context.HierarchyClass.Where(hc => hc.hierarchyClassName == "Test Brand 1").Single().hierarchyClassID);
            Assert.IsFalse(context.HierarchyClass.Any(hc => hc.hierarchyClassName == "Test Brand 2"));
        }
    }
}
