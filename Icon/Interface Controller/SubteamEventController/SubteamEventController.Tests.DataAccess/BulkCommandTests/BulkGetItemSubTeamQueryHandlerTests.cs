using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Icon.Framework;
using SubteamEventController.DataAccess.BulkCommands;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using Icon.Testing.Builders;

namespace SubteamEventController.Tests.DataAccess.BulkCommandTests
{
    [TestClass]
    public class BulkGetItemSubTeamQueryHandlerTests
    {
        private BulkGetItemsWithSubTeamQueryHandler handler;
        private BulkGetItemsWithSubTeamQuery query;

        private IconContext context;
        private DbContextTransaction transaction;

        private List<string> scanCodeList;

        [TestInitialize]
        public void InitializeData()
        {
            this.context = new IconContext();
            this.query = new BulkGetItemsWithSubTeamQuery();
            this.handler = new BulkGetItemsWithSubTeamQueryHandler(this.context);
            this.scanCodeList = new List<string>();

            transaction = context.Database.BeginTransaction();
        }

        [TestCleanup]
        public void CleanupData()
        {
            transaction.Rollback();
            transaction.Dispose();
            context.Dispose();
        }

        [TestMethod]
        public void BulkGetItemSubTeam_ValidatedScanCodes_ReturnsItemSubTeamModelListForScanCodes()
        {
            // Given
            string finCode = "123123";
            HierarchyClass merch = new TestHierarchyClassBuilder()
                .WithHierarchyId(Hierarchies.Merchandise)
                .WithMerchFinMapping(finCode);
            HierarchyClass fin = new TestHierarchyClassBuilder()
                .WithHierarchyId(Hierarchies.Financial)
                .WithHierarchyClassName(finCode);
            HierarchyClass brand = new TestHierarchyClassBuilder()
                .WithHierarchyId(Hierarchies.Brands);

            context.HierarchyClass.Add(merch);
            context.HierarchyClass.Add(fin);
            context.HierarchyClass.Add(brand);
            context.SaveChanges();

            Item item = new TestItemBuilder()
                .WithScanCode("12345654321")
                .WithValidationDate(DateTime.Now.ToString());
            item.ItemHierarchyClass.Add(new ItemHierarchyClass { HierarchyClass = merch });
            item.ItemHierarchyClass.Add(new ItemHierarchyClass { HierarchyClass = brand });

            context.Item.Add(item);
            context.SaveChanges();

            //When
            var results = handler.Handle(new BulkGetItemsWithSubTeamQuery
                {
                    ScanCodes = new List<string> { item.ScanCode.First().scanCode } 
                });

            //Then
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(item.ScanCode.First().scanCode, results.First().ScanCode);
            //Assert.Fail("Test needs to be written");
        }
    }
}
