using Dapper;
using Icon.Monitoring.DataAccess.Model;
using Icon.Monitoring.DataAccess.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace Icon.Monitoring.DataAccess.Tests.Queries
{
    [TestClass()]
    public class GetInStockOldestUnprocessedRecordDateByQueueTableQueryTests
    {
        private SqlDbProvider provider;
        private GetInStockOldestUnprocessedRecordDateByQueueTableQuery query;
        private GetInStockOldestUnprocessedRecordDateByQueueTableParameters parameters;
        private const string AppName = "In Stock Monitor";
        DateTime curentDate = DateTime.Now;

        [TestInitialize]
        public void Initialize()
        {
            provider = new SqlDbProvider();
            provider.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ItemCatalog_FL"].ConnectionString);
            provider.Connection.Open();
            provider.Transaction = provider.Connection.BeginTransaction();
            query = new GetInStockOldestUnprocessedRecordDateByQueueTableQuery(provider);
            parameters = new GetInStockOldestUnprocessedRecordDateByQueueTableParameters();
            InsertOrderQueue(curentDate);
        }

        [TestCleanup]
        public void CleanupTest()
        {
            provider.Transaction.Rollback();
            provider.Connection.Close();
            provider.Connection.Dispose();
        }

        [TestMethod]
        public void Get_InStockOldestUnprocessedRecordForOrderQueue_ReturnsOldestUnprocessedRecordInsertDateUtc()
        {
            //Given
            parameters.QueueTableName = "amz.OrderQueue";

            //When
            DateTime? oldestUnprocessedRecordInsertDateUtc = query.Search(parameters);

            //Then
            Assert.AreEqual(oldestUnprocessedRecordInsertDateUtc.ToString(), curentDate.ToString());
        }

        [TestMethod]
        public void Get_InStockOldestUnprocessedRecordForReceiptQueue_ReturnsOldestUnprocessedRecordInsertDateUtc()
        {
            //Given
            parameters.QueueTableName = "amz.receiptqueue";
            InsertReceiptQueue(curentDate);

            //When
            DateTime? oldestUnprocessedRecordInsertDateUtc = query.Search(parameters);

            //Then
            Assert.AreEqual(oldestUnprocessedRecordInsertDateUtc.ToString(), curentDate.ToString());
        }


        private int InsertReceiptQueue(DateTime curentDate)
        {
            return provider.Connection.Query<int>(
        @"INSERT INTO [amz].[receiptqueue]
           ([EventTypeCode]
           ,[MessageType]
           ,[KeyID]
           ,[SecondaryKeyID]
           ,[InsertDate]
           ,[MessageTimestampUtc])
     VALUES
           (@EventTypeCode
           ,@MessageType
           ,@KeyID
           ,@SecondaryKeyID
           ,@InsertDate
           ,@MessageTimestampUtc
            )
            select SCOPE_IDENTITY()",
                            new
                            {
                                EventTypeCode = 11,
                                MessageType = "Test",
                                KeyID = 1,
                                SecondaryKeyID = 2,
                                InsertDate = curentDate,
                                MessageTimestampUtc = curentDate,
                                MachineName = "CEWD6592"
                            },
                            provider.Transaction)
                            .FirstOrDefault();
        }

        private int InsertOrderQueue(DateTime curentDate)
        {
            return provider.Connection.Query<int>(
        @"INSERT INTO [amz].[OrderQueue]
           ([EventTypeCode]
           ,[MessageType]
           ,[KeyID]
           ,[SecondaryKeyID]
           ,[InsertDate]
           ,[MessageTimestampUtc])
     VALUES
           (@EventTypeCode
           ,@MessageType
           ,@KeyID
           ,@SecondaryKeyID
           ,@InsertDate
           ,@MessageTimestampUtc
            )
            select SCOPE_IDENTITY()",
                            new
                            {
                                EventTypeCode = 11,
                                MessageType = "Test",
                                KeyID = 1,
                                SecondaryKeyID = 2,
                                InsertDate = curentDate,
                                MessageTimestampUtc = curentDate,
                                MachineName = "CEWD6592"
                            },
                            provider.Transaction)
                            .FirstOrDefault();
        }
    }
}
