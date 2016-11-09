using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Icon.Framework;
using RegionalEventController.DataAccess.Queries;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using Icon.Testing.Builders;

namespace RegionalEventController.Tests.DataAccess.QueryTests
{
    [TestClass]
    public class GetValidatedItemsQueryHandlerTests
    {
        private IconContext context;
        private GetValidatedItemsQuery query;
        private GetValidatedItemsQueryHandler handler;
        private DbContextTransaction transaction;

        [TestInitialize]
        public void InitializeData()
        {
            this.context = new IconContext();
            this.query = new GetValidatedItemsQuery();
            this.handler = new GetValidatedItemsQueryHandler(this.context);
            this.transaction = this.context.Database.BeginTransaction();
        }

        [TestCleanup]
        public void CleanupData()
        {
            if (this.transaction != null)
            {
                this.transaction.Rollback();
            }

            if (this.context != null)
            {
                this.context.Dispose();
            }
        }

        [TestMethod]
        public void GetValidatedItems_IdentifiersAreValidated_ReturnsDictionaryOfScanCodeToItemID()
        {
            // Given
            List<Item> items = GetValidatedItemList();
            this.context.Item.AddRange(items);
            this.context.SaveChanges();

            this.query.identifiers = items.Select(i => i.ScanCode.First().scanCode).ToList();

            IEnumerable<string> scanCodes = this.context.ScanCode.Where(sc => this.query.identifiers.Contains(sc.scanCode)).Select(sc => sc.scanCode);
            var expectedQuery = (from i in context.Item
                                   join it in context.ItemTrait on i.itemID equals it.itemID
                                   join sc in context.ScanCode.AsQueryable<ScanCode>() on i.itemID equals sc.itemID
                                   join t in context.Trait on it.traitID equals t.traitID
                                   where scanCodes.Contains(sc.scanCode) && t.traitCode == TraitCodes.ValidationDate
                                   select new
                                   {
                                       itemID = i.itemID,
                                       scanCode = sc.scanCode
                                   }).ToList();

            Dictionary<string, int> expected = new Dictionary<string, int>();
            foreach (var item in expectedQuery)
            {
                expected.Add(item.scanCode, item.itemID);
            }

            // When
            Dictionary<string, int> actual = this.handler.Execute(this.query);

            // Then
            foreach (var item in actual)
            {
                Assert.IsTrue(expected.Keys.Contains(item.Key), "The actual Keys returned in Dictionary are not contained in the expected Dictionary.");
                Assert.AreEqual(expected[item.Key], item.Value, "The actual Value returned in Dictionary does not match the expected Value.");
            }
        }

        [TestMethod]
        public void GetValidatedItems_IdentifiersAreNotValidated_ReturnsEmptyDictionary()
        {
            // Given
            List<Item> items = GetNonValidatedItemList();
            this.context.Item.AddRange(items);
            this.context.SaveChanges();

            this.query.identifiers = items.Select(i => i.ScanCode.First().scanCode).ToList();

            // When
            Dictionary<string, int> actual = this.handler.Execute(this.query);

            // Then
            Assert.IsTrue(actual.Count == 0, "The actual dictionary is not empty.");
        }

        [TestMethod]
        public void GetValidatedItem_IdentifierAreNotInScanCodeTable_ReturnsEmptyDictionary()
        {
            // Given
            List<Item> items = GetNonValidatedItemList();
            this.query.identifiers = items.Select(i => i.ScanCode.First().scanCode).ToList();

            // When
            Dictionary<string, int> actual = this.handler.Execute(this.query);

            // Then
            Assert.IsTrue(actual.Count == 0, "The actual dictionary is not empty.");
        }

        private List<Item> GetValidatedItemList()
        {
            List<Item> items = new List<Item>();
            Item itemOne = new TestItemBuilder().WithValidationDate(DateTime.Now.ToString()).WithScanCode("777700000001").Build();
            items.Add(itemOne);

            Item itemTwo = new TestItemBuilder().WithValidationDate(DateTime.Now.ToString()).WithScanCode("777700000002").Build();
            items.Add(itemTwo);

            Item itemThree = new TestItemBuilder().WithValidationDate(DateTime.Now.ToString()).WithScanCode("777700000003").Build();
            items.Add(itemThree);
            
            return items;
        }

        private List<Item> GetNonValidatedItemList()
        {
            List<Item> items = new List<Item>();
            Item itemOne = new TestItemBuilder().WithScanCode("777700000001").Build();
            items.Add(itemOne);

            Item itemTwo = new TestItemBuilder().WithScanCode("777700000002").Build();
            items.Add(itemTwo);

            Item itemThree = new TestItemBuilder().WithScanCode("777700000003").Build();
            items.Add(itemThree);

            return items;
        }
    }
}
