using Icon.ApiController.DataAccess.Queries;
using Icon.Framework;
using Icon.Framework.RenewableContext;
using Icon.Testing.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity;
using System.Linq;
using System.Transactions;

namespace Icon.ApiController.Tests.Queries
{
    [TestClass]
    public class GetLocaleLineageQueryTests
    {
        private GetLocaleLineageQuery getLocaleLineageQuery;
        private IconContext context;
        private TransactionScope transaction;
        private int localeId;
        private string testRegionName;
        private string testMetroName;
        private string testStoreName;
        private string emptyMetroName;

        [TestInitialize]
        public void Initialize()
        {
            transaction = new TransactionScope();

            context = new IconContext();

            getLocaleLineageQuery = new GetLocaleLineageQuery(new IconDbContextFactory());
            testRegionName = "Southern Pacific";
            testMetroName = "MET_SD";
            testStoreName = "Hillcrest";
            emptyMetroName = "MET_NOSTORE";
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose();
        }

        private void StageEmptyMetro()
        {
            Locale emptyMetro = new TestLocaleBuilder().WithLocaleName(emptyMetroName).WithParentLocaleId(localeId);
            context.Locale.Add(emptyMetro);
            context.SaveChanges();
        }

        [TestMethod]
        public void GetLocaleLineage_Region_ShouldReturnChainAsParentLocale()
        {
            // Given.
            localeId = context.Locale.Single(l => l.localeName == testRegionName).localeID;

            var parameters = new GetLocaleLineageParameters
            {
                LocaleTypeId = LocaleTypes.Region,
                LocaleId = localeId
            };

            // When.
            var localeLineage = getLocaleLineageQuery.Search(parameters);

            // Then.
            Assert.AreEqual(Locales.Names.WholeFoods, localeLineage.LocaleName);
            Assert.AreEqual(Locales.WholeFoods, localeLineage.LocaleId);
        }

        [TestMethod]
        public void GetLocaleLineage_Metro_ShouldReturnChainAsParentLocale()
        {
            // Given.
            localeId = context.Locale.Single(l => l.localeName == testMetroName).localeID;

            var parameters = new GetLocaleLineageParameters
            {
                LocaleTypeId = LocaleTypes.Metro,
                LocaleId = localeId
            };

            // When.
            var localeLineage = getLocaleLineageQuery.Search(parameters);

            // Then.
            Assert.AreEqual(Locales.Names.WholeFoods, localeLineage.LocaleName);
            Assert.AreEqual(Locales.WholeFoods, localeLineage.LocaleId);
        }

        [TestMethod]
        public void GetLocaleLineage_Store_ShouldReturnChainAsParentLocale()
        {
            // Given.
            localeId = context.Locale.Single(l => l.localeName == testStoreName).localeID;

            var parameters = new GetLocaleLineageParameters
            {
                LocaleTypeId = LocaleTypes.Store,
                LocaleId = localeId
            };

            // When.
            var localeLineage = getLocaleLineageQuery.Search(parameters);

            // Then.
            Assert.AreEqual(Locales.Names.WholeFoods, localeLineage.LocaleName);
            Assert.AreEqual(Locales.WholeFoods, localeLineage.LocaleId);
        }

        [TestMethod]
        public void GetLocaleLineage_Region_ResultShouldIncludeMetrosAndStores()
        {
            // Given.
            localeId = context.Locale.Single(l => l.localeName == testRegionName).localeID;

            var parameters = new GetLocaleLineageParameters
            {
                LocaleTypeId = LocaleTypes.Region,
                LocaleId = localeId
            };

            // When.
            var localeLineage = getLocaleLineageQuery.Search(parameters);

            // Then.
            string regionName = localeLineage.DescendantLocales[0].LocaleName;
            var metros = localeLineage.DescendantLocales[0].DescendantLocales;
            var storesInFirstMetro = metros[0].DescendantLocales;

            Assert.AreEqual(testRegionName, regionName);
            Assert.IsNotNull(metros);
            Assert.IsNotNull(storesInFirstMetro);
        }

        [TestMethod]
        public void GetLocaleLineage_RegionWithEmptyMetro_EmptyMetroShouldNotBeReturned()
        {
            // Given.
            localeId = context.Locale.Single(l => l.localeName == testRegionName).localeID;

            StageEmptyMetro();

            var parameters = new GetLocaleLineageParameters
            {
                LocaleTypeId = LocaleTypes.Region,
                LocaleId = localeId
            };

            // When.
            var localeLineage = getLocaleLineageQuery.Search(parameters);

            // Then.
            var metros = localeLineage.DescendantLocales[0].DescendantLocales;
            bool emptyMetroIsExcluded = !metros.Any(m => m.LocaleName == emptyMetroName);

            Assert.IsTrue(emptyMetroIsExcluded);
        }

        [TestMethod]
        public void GetLocaleLineage_Metro_ResultShouldIncludeRegionButNotStores()
        {
            // Given.
            localeId = context.Locale.Single(l => l.localeName == testMetroName).localeID;

            var parameters = new GetLocaleLineageParameters
            {
                LocaleTypeId = LocaleTypes.Metro,
                LocaleId = localeId
            };

            // When.
            var localeLineage = getLocaleLineageQuery.Search(parameters);

            // Then.
            string regionName = localeLineage.DescendantLocales[0].LocaleName;
            var metro = localeLineage.DescendantLocales[0].DescendantLocales[0];
            var stores = metro.DescendantLocales;
            
            Assert.AreEqual(testRegionName, regionName);
            Assert.AreEqual(testMetroName, metro.LocaleName);
            Assert.AreEqual(0, stores.Count);
        }

        [TestMethod]
        public void GetLocaleLineage_Store_ResultShouldIncludeRegionAndMetro()
        {
            // Given.
            localeId = context.Locale.Single(l => l.localeName == testStoreName).localeID;

            var parameters = new GetLocaleLineageParameters
            {
                LocaleTypeId = LocaleTypes.Store,
                LocaleId = localeId
            };

            // When.
            var localeLineage = getLocaleLineageQuery.Search(parameters);

            // Then.
            string regionName = localeLineage.DescendantLocales[0].LocaleName;
            string metroName = localeLineage.DescendantLocales[0].DescendantLocales[0].LocaleName;
            string storeName = localeLineage.DescendantLocales[0].DescendantLocales[0].DescendantLocales[0].LocaleName;

            Assert.AreEqual(testRegionName, regionName);
            Assert.AreEqual(testMetroName, metroName);
            Assert.AreEqual(testStoreName, storeName);
        }
    }
}
