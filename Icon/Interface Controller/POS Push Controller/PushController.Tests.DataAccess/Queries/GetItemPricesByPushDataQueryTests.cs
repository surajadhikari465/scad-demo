using Icon.Framework;
using Icon.Framework.RenewableContext;
using Icon.Testing.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PushController.Common;
using PushController.DataAccess.Queries;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PushController.Tests.DataAccess.Queries
{
    [TestClass]
    public class GetItemPricesByPushDataQueryTests
    {
        private GlobalIconContext globalContext;
        private IconContext context;
        private DbContextTransaction transaction;
        private GetItemPricesByPushDataQueryHandler queryHandler;

        [TestInitialize]
        public void InitializeData()
        {
            this.context = new IconContext();
            this.globalContext = new GlobalIconContext(context);
            this.queryHandler = new GetItemPricesByPushDataQueryHandler(this.globalContext);

            this.transaction = context.Database.BeginTransaction();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
        }

        [TestMethod]
        public void GetItemPricesByPushDataQuery_ItemHasNoItemPriceInformation_ReturnsEmptyList()
        {
            // Given.
            int businessUnit;
            string businessUnitString = this.globalContext.Context.LocaleTrait.First(lt => lt.traitID == Traits.PsBusinessUnitId && lt.Locale.localeName == "Hillcrest").traitValue;
            int localeId = this.globalContext.Context.LocaleTrait.First(lt => lt.traitID == Traits.PsBusinessUnitId && lt.Locale.localeName == "Hillcrest").localeID;

            if (!Int32.TryParse(businessUnitString, out businessUnit))
            {
                businessUnit = 10072;
            }

            string identifier = "94011";
            int itemId = this.globalContext.Context.ScanCode.First(sc => sc.scanCode == identifier).itemID;

            this.context.ItemPrice.RemoveRange(context.ItemPrice.Where(ip => ip.itemID == itemId));
            this.context.SaveChanges();

            IRMAPush push = new TestIrmaPushBuilder().WithBusinessUnitId(businessUnit).WithIdentifier(identifier).WithRegionCode("SP");
            var irmaPushItems = new List<IRMAPush>();
            irmaPushItems.Add(push);

            // When.
            List<ItemPriceModel> actual = this.queryHandler.Execute(new GetItemPricesByPushDataQuery { IrmaPushList = irmaPushItems });
            int expected = 0;

            // Then.
            Assert.AreEqual(expected, actual.Count, "The expected ItemPrice count did not match the actual ItemPrice count.");
        }

        [TestMethod]
        public void GetItemPricesByPushDataQuery_ItemHasItemPriceInformation_ReturnsExpectedItemPriceRows()
        {
            // Given.
            int businessUnit;
            string businessUnitString = this.globalContext.Context.LocaleTrait.First(lt => lt.traitID == Traits.PsBusinessUnitId && lt.Locale.localeName == "Hillcrest").traitValue;
            int localeId = this.globalContext.Context.LocaleTrait.First(lt => lt.traitID == Traits.PsBusinessUnitId && lt.Locale.localeName == "Hillcrest").localeID;

            if (!Int32.TryParse(businessUnitString, out businessUnit))
            {
                businessUnit = 10072;
            }

            string identifier = "94011";
            int itemId = this.globalContext.Context.ScanCode.Single(sc => sc.scanCode == identifier).itemID;

            IRMAPush push = new TestIrmaPushBuilder().WithBusinessUnitId(businessUnit).WithIdentifier(identifier).WithRegionCode("SP");
            var irmaPushItems = new List<IRMAPush>();
            irmaPushItems.Add(push);

            if (!this.context.ItemPrice.Any(p => p.itemID == itemId && p.localeID == localeId))
            {
                Assert.Fail("Test item does not have any current ItemPrice entries.");
            }

            // When.
            List<ItemPriceModel> actual = this.queryHandler.Execute(new GetItemPricesByPushDataQuery { IrmaPushList = irmaPushItems });
            List<ItemPrice> expected = this.globalContext.Context.ItemPrice
                .Where(ip => ip.localeID == localeId && ip.itemID == itemId)
                .ToList();

            // Then.
            Assert.AreEqual(expected.Count, actual.Count, "The expected ItemPrice count did not match the actual ItemPrice count.");
        }
    }
}
