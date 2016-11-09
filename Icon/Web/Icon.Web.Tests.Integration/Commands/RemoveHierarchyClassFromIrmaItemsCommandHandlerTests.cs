using Icon.Framework;
using Icon.Testing.Builders;
using Icon.Web.DataAccess.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data.Entity;

namespace Icon.Web.Tests.Integration.Commands
{
    [TestClass]
    public class RemoveHierarchyClassFromIrmaItemsCommandHandlerTests
    {
        private RemoveHierarchyClassFromIrmaItemsCommandHandler commandHandler;
        private RemoveHierarchyClassFromIrmaItemsCommand command;
        private IconContext context;
        private DbContextTransaction transaction;
        private HierarchyClass testTaxHierarchyClass;
        private HierarchyClass testMerchHierarchyClass;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            command = new RemoveHierarchyClassFromIrmaItemsCommand();
            commandHandler = new RemoveHierarchyClassFromIrmaItemsCommandHandler(context);
            transaction = context.Database.BeginTransaction();
            testTaxHierarchyClass = new TestHierarchyClassBuilder()
                .WithHierarchyId(Hierarchies.Tax)
                .WithHierarchyClassName("Test Tax");
            testMerchHierarchyClass = new TestHierarchyClassBuilder()
                .WithHierarchyId(Hierarchies.Merchandise)
                .WithHierarchyClassName("Test Merch");
            context.HierarchyClass.Add(testTaxHierarchyClass);
            context.SaveChanges();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
            transaction.Dispose();
            context.Dispose();
        }

        [TestMethod]
        public void RemoveHierarchyClassFromIrmaItemTraits_HierarchyClassIsTax_RemovesTaxFromIrmaItems()
        {
            //Given
            List<IRMAItem> irmaItems = new List<IRMAItem>
            {
                new TestIrmaItemBuilder().WithIdentifier("111111111118")
                    .WithTaxClassId(testTaxHierarchyClass.hierarchyClassID),
                new TestIrmaItemBuilder().WithIdentifier("1111111111119")
                    .WithTaxClassId(testTaxHierarchyClass.hierarchyClassID)
            };
            context.IRMAItem.AddRange(irmaItems);
            context.SaveChanges();
            command.HierarchyClassId = testTaxHierarchyClass.hierarchyClassID;
            command.HierarchyId = testTaxHierarchyClass.hierarchyID;
            //When
            commandHandler.Execute(command);

            //Then
            foreach (var irmaItem in irmaItems)
            {
                Assert.IsNull(irmaItem.taxClassID);
            }
        }

        [TestMethod]
        public void RemoveHierarchyClassFromIrmaItemTraits_HierarchyClassIsMerchandise_RemovesMerchandiseFromIrmaItems()
        {
            //Given
            List<IRMAItem> irmaItems = new List<IRMAItem>
            {
                new TestIrmaItemBuilder().WithIdentifier("111111111118")
                    .WithMerchandiseClassId(testMerchHierarchyClass.hierarchyClassID),
                new TestIrmaItemBuilder().WithIdentifier("1111111111119")
                    .WithMerchandiseClassId(testMerchHierarchyClass.hierarchyClassID)
            };
            context.IRMAItem.AddRange(irmaItems);
            context.SaveChanges();
            command.HierarchyClassId = testMerchHierarchyClass.hierarchyClassID;
            command.HierarchyId = testMerchHierarchyClass.hierarchyID;
            //When
            commandHandler.Execute(command);

            //Then
            foreach (var irmaItem in irmaItems)
            {
                Assert.IsNull(irmaItem.merchandiseClassID);
            }
        }
    }
}
