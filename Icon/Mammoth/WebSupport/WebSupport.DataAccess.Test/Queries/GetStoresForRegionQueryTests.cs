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
            int initialStoreCount = context.Database.SqlQuery<int>("SELECT COUNT(*) FROM dbo.Locales_FL").First();
            context.Database.ExecuteSqlCommand(@"
                INSERT INTO dbo.Locales_FL(
                    BusinessUnitID, 
                    StoreName, 
                    StoreAbbrev, 
                    AddedDate)
                VALUES(
                    9876,
                    'Test Store',
                    'TST',
                    GETDATE())");
            parameters.Region = "FL";

            //When
            var stores = getStoresForRegionQuery.Search(parameters);

            //Then
            Assert.AreEqual(initialStoreCount + 1, stores.Count);
            var testStore = stores.Single(s => s.BusinessUnit == "9876");
            Assert.AreEqual("Test Store", testStore.Name);
            Assert.AreEqual("TST", testStore.Abbreviation);
        }
    }
}
