using Dapper;
using Icon.Monitoring.DataAccess.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace Icon.Monitoring.DataAccess.Tests.Queries
{
    [TestClass]
    public class GetMammothPriceChangeQueueIdTests
    {
        private SqlDbProvider provider;
        private GetMammothPriceChangeQueueIdQuery query;
        private GetMammothPriceChangeQueueIdQueryParameters parameters;

        [TestInitialize]
        public void Initialize()
        {
            provider = new SqlDbProvider();
            provider.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ItemCatalog_FL"].ConnectionString);
            provider.Connection.Open();
            provider.Transaction = provider.Connection.BeginTransaction();
            query = new GetMammothPriceChangeQueueIdQuery(provider);
            parameters = new GetMammothPriceChangeQueueIdQueryParameters();
        }

        [TestCleanup]
        public void CleanUp()
        {
            provider.Transaction.Rollback();
            provider.Connection.Close();
        }

        [TestMethod]
        public void GetMammothPriceChangeQueueIdSearch_UnprocessedRowsExist_ReturnId()
        {
            //Given
            int result, insertId;
            insertId = InsertMammothPriceChangeQueueTable();

            //When
            result = query.Search(parameters);

            //Then
            Assert.AreEqual(insertId, result);
        }

        [TestMethod]
        public void GetMammothPriceChangeQueueIdSearch_UnprocessedRowsDontExist_ReturnZero()
        {
            //Given
            int result;
            UpdateMammothPriceChangeQueueTable_SetProcessFailedDateToNonNull();

            //When
            result = query.Search(parameters);

            //Then
            Assert.AreEqual(0, result);
        }

        private int InsertMammothPriceChangeQueueTable()
        {
            return provider.Connection.Query<int>(
            @"INSERT INTO ItemCatalog.mammoth.PriceChangeQueue
               ([Item_Key]
               ,[Store_No]
               ,[Identifier]
               ,[EventTypeID]
               ,[EventReferenceID]
               ,[InsertDate]
               ,[ProcessFailedDate]
               ,[InProcessBy])
                VALUES
               (16996
               ,NULL
               ,'4011'
               ,1
               ,NULL
               ,GETDATE()
               ,NULL
               ,456);
                
            select SCOPE_IDENTITY();",
            null,
                provider.Transaction)
                .FirstOrDefault();
        }

        private void UpdateMammothPriceChangeQueueTable_SetProcessFailedDateToNonNull()
        {
            provider.Connection.Query<int>(
                @"Update [mammoth].[PriceChangeQueue] set [ProcessFailedDate] = GetDate()",
                null, provider.Transaction)
                .FirstOrDefault();
        }
    }
}
