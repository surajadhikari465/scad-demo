using Mammoth.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using WebSupport.DataAccess.Queries;

namespace WebSupport.DataAccess.Test.Queries
{
    [TestClass]
    public class GetStoresForRegionQueryTests
    {
        private GetStoresForRegionQuery getStoresForRegionQuery;
        private GetStoresForRegionParameters parameters;
        private TransactionScope transaction;
        private MammothContext context;
        private string testRegion = "FL";

        [TestInitialize]
        public void Initialize()
        {
            transaction = new TransactionScope();
            context = new MammothContext();
            getStoresForRegionQuery = new GetStoresForRegionQuery(context);
            parameters = new GetStoresForRegionParameters();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose();
            context.Dispose();
        }

        [TestMethod]
        public void GetStoresForRegion_StoreExistsInRegion_AllStoresAreReturned()
        {
            //Given
            int initialStoreCount = context.Database.SqlQuery<int>(GetSqlForStoreCount(testRegion)).First();
            context.Database.ExecuteSqlCommand(GetSqlForStoreInsert(testRegion, 9876, "Test Store", "TST"));
            parameters.Region = testRegion;

            //When
            var stores = getStoresForRegionQuery.Search(parameters);

            //Then
            Assert.AreEqual(initialStoreCount + 1, stores.Count);
            var testStore = stores.Single(s => s.BusinessUnit == "9876");
            Assert.AreEqual("Test Store", testStore.Name);
            Assert.AreEqual("TST", testStore.Abbreviation);
        }

        [TestMethod]
        public void GetStoresForRegion_ClosedStoreExistsInRegion_ClosedStoresAreNotReturned()
        {
            //Given
            int initialStoreCount = context.Database.SqlQuery<int>(GetSqlForStoreCount(testRegion)).First();
            context.Database.ExecuteSqlCommand(GetSqlForStoreInsert(testRegion, 9876, "Test Store", "TST", DateTime.Now.ToShortDateString()));
            parameters.Region = testRegion;

            //When
            var stores = getStoresForRegionQuery.Search(parameters);

            //Then
            Assert.AreEqual(initialStoreCount, stores.Count, "Store count should not include closed store(s)");
            var testStore = stores.SingleOrDefault(s => s.BusinessUnit == "9876");
            Assert.IsNull(testStore, "Closed store should not have been included in query results");
        }

        [TestMethod]
        public void GetStoresForRegion_StoreWithFutureCloseDateExistsInRegion_FutureDateIgnored()
        {
            //Given
            int initialStoreCount = context.Database.SqlQuery<int>(GetSqlForStoreCount(testRegion)).First();
            context.Database.ExecuteSqlCommand(GetSqlForStoreInsert(testRegion, 9876, "Test Store", "TST", "2037-01-19"));
            parameters.Region = testRegion;

            //When
            var stores = getStoresForRegionQuery.Search(parameters);

            //Then
            Assert.AreEqual(initialStoreCount + 1, stores.Count, "Store with close date in the future should count as open (?)");
            var testStore = stores.Single(s => s.BusinessUnit == "9876");
            Assert.AreEqual("Test Store", testStore.Name);
            Assert.AreEqual("TST", testStore.Abbreviation);
        }

        private string GetSqlForStoreCount(string region)
        {
            var sql = $"SELECT COUNT(*) FROM dbo.Locales_{region} WHERE LocaleCloseDate IS NULL";
            return sql;
        }

        private string GetSqlForStoreInsert(string region, int businessUnitID, string storeName, string storeAbbrev, string closeDate = null)
        {
            var localeCloseDateValue = string.IsNullOrWhiteSpace(closeDate) ? "NULL" : $"'{closeDate}'";
            var sql = $"INSERT INTO dbo.Locales_{region} (BusinessUnitID,  StoreName, StoreAbbrev, AddedDate, LocaleCloseDate) " +
                $"VALUES( {businessUnitID}, '{storeName}', '{storeAbbrev}', GETDATE(), {localeCloseDateValue}) ";
            return sql;
        }
    }
}
