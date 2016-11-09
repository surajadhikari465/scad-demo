using Icon.Framework;
using Icon.Framework.RenewableContext;
using Icon.Testing.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PushController.DataAccess.Queries;
using System.Data.Entity;

namespace PushController.Tests.DataAccess.Queries
{
    [TestClass]
    public class GetItemPriceQueryTests
    {
        private GetItemPriceQueryHandler getItemPriceQueryHandler;
        private GlobalIconContext context;
        private DbContextTransaction transaction;
        private ItemPrice testItemPrice;
        private int testItemPriceTypeId;
        private Item testItem;
        private Locale testLocale;

        [TestInitialize]
        public void Initialize()
        {
            context = new GlobalIconContext(new IconContext());

            getItemPriceQueryHandler = new GetItemPriceQueryHandler(context);

            testItem = new TestItemBuilder().Build();
            testLocale = new TestLocaleBuilder().Build();
            testItemPriceTypeId = ItemPriceTypes.Reg;

            transaction = context.Context.Database.BeginTransaction();

            context.Context.Item.Add(testItem);
            context.Context.Locale.Add(testLocale);
            context.Context.SaveChanges();

            testItemPrice = new ItemPrice
            {
                itemID = testItem.itemID,
                localeID = testLocale.localeID,
                itemPriceTypeID = testItemPriceTypeId,
                itemPriceAmt = 2.99m,
                currencyTypeID = CurrencyTypes.Usd,
                uomID = UOMs.Each
            };

            context.Context.ItemPrice.Add(testItemPrice);
            context.Context.SaveChanges();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
        }

        [TestMethod]
        public void GetItemPrice_AllParametersMatch_ItemPriceShouldBeReturned()
        {
            // Given.
            var query = new GetItemPriceQuery
            {
                ItemId = testItem.itemID,
                LocaleId = testLocale.localeID,
                PriceTypeId = ItemPriceTypes.Reg
            };

            // When.
            var queryResults = getItemPriceQueryHandler.Execute(query);

            // Then.
            Assert.IsNotNull(queryResults);
        }

        [TestMethod]
        public void GetItemPrice_ItemDoesNotMatch_ItemPriceShouldNotBeReturned()
        {
            // Given.
            var query = new GetItemPriceQuery
            {
                ItemId = 88888888,
                LocaleId = testLocale.localeID,
                PriceTypeId = testItemPriceTypeId
            };

            // When.
            var queryResults = getItemPriceQueryHandler.Execute(query);

            // Then.
            Assert.IsNull(queryResults);
        }

        [TestMethod]
        public void GetItemPrice_LocaleDoesNotMatch_ItemPriceShouldNotBeReturned()
        {
            // Given.
            var query = new GetItemPriceQuery
            {
                ItemId = testItem.itemID,
                LocaleId = 88888888,
                PriceTypeId = testItemPriceTypeId
            };

            // When.
            var queryResults = getItemPriceQueryHandler.Execute(query);

            // Then.
            Assert.IsNull(queryResults);
        }

        [TestMethod]
        public void GetItemPrice_ItemPriceTypeIdDoesNotMatch_ItemPriceShouldNotBeReturned()
        {
            // Given.
            var query = new GetItemPriceQuery
            {
                ItemId = testItem.itemID,
                LocaleId = testLocale.localeID,
                PriceTypeId = ItemPriceTypes.Tpr
            };

            // When.
            var queryResults = getItemPriceQueryHandler.Execute(query);

            // Then.
            Assert.IsNull(queryResults);
        }
    }
}
