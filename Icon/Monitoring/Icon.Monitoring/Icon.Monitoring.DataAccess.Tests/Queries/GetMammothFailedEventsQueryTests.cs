using Dapper;
using Icon.Monitoring.DataAccess.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Icon.Monitoring.DataAccess.Tests.Queries
{
    [TestClass]
    public class GetMammothFailedEventsQueryTests
    {
        private GetMammothFailedEventsQuery query;
        private GetMammothFailedEventsParameters parameters;
        private SqlDbProvider db;
        private TransactionScope transaction;

        [TestInitialize]
        public void Initialize()
        {
            transaction = new TransactionScope();
            db = new SqlDbProvider();
            parameters = new GetMammothFailedEventsParameters();
            query = new GetMammothFailedEventsQuery(db);
            db.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ItemCatalog_FL"].ConnectionString);
        }

        [TestCleanup]
        public void Cleanup()
        {
            db.Connection.Dispose();
            transaction.Dispose();
        }

        [TestMethod]
        public void GetMammothFailedEvents_NoEventsExist_ReturnsEmptyList()
        {
            //Given
            parameters.BeginDate = DateTime.Now.AddDays(1);
            parameters.EndDate = parameters.BeginDate.AddMinutes(15);

            //When
            var result = query.Search(parameters);

            //Then
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void GetMammothFailedEvents_NoFailedEventsExist_ReturnsEmptyList()
        {
            //Given
            parameters.BeginDate = DateTime.Now.AddDays(1);
            parameters.EndDate = parameters.BeginDate.AddMinutes(15);
            InsertChangeQueueHistory(parameters.BeginDate.AddMinutes(1), null);

            //When
            var result = query.Search(parameters);

            //Then
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void GetMammothFailedEvents_FailedEventExist_ReturnsFailedEvent()
        {
            //Given
            parameters.BeginDate = DateTime.Now.AddDays(1);
            parameters.EndDate = parameters.BeginDate.AddMinutes(15);
            InsertChangeQueueHistory(parameters.BeginDate.AddMinutes(1), "test");
            InsertChangeQueueHistory(parameters.BeginDate.AddMinutes(1), null);

            //When
            var result = query.Search(parameters);

            //Then
            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void GetMammothFailedEvents_FailedEventsExist_ReturnsFailedEvents()
        {
            //Given
            parameters.BeginDate = DateTime.Now.AddDays(1);
            parameters.EndDate = parameters.BeginDate.AddMinutes(15);
            InsertChangeQueueHistory(parameters.BeginDate.AddMinutes(1), "test");
            InsertChangeQueueHistory(parameters.BeginDate.AddMinutes(1), null);
            InsertChangeQueueHistory(parameters.BeginDate.AddMinutes(1), "test");
            InsertChangeQueueHistory(parameters.BeginDate.AddMinutes(1), null);
            InsertChangeQueueHistory(parameters.BeginDate.AddMinutes(1), "test");
            InsertChangeQueueHistory(parameters.BeginDate.AddMinutes(1), null);

            //When
            var result = query.Search(parameters);

            //Then
            Assert.AreEqual(3, result.Count);
        }

        private void InsertChangeQueueHistory(DateTime insertDate, string errorCode)
        {
            db.Connection.Execute(
                @"INSERT INTO mammoth.ChangeQueueHistory
                    (EventTypeId
                    ,Identifier
                    ,Item_Key
                    ,Store_No
                    ,QueueID
                    ,EventReferenceId
                    ,QueueInsertDate
                    ,InsertDate
                    ,MachineName
                    ,Context
                    ,ErrorCode
                    ,ErrorDetails) 
                SELECT 
                    1 AS EventTypeId,
                    '2' AS Identifier,
                    3 AS Item_Key,
                    4 AS Store_No,
                    5 AS QueueID,
                    NULL AS EventReference,
                    GETDATE() AS QueueInsertDate,
                    @InsertDate AS InsertDate,
                    'test' AS MachineName,
                    'test' AS Context,
                    @ErrorCode,
                    NULL AS ErrorDetails",
                new { InsertDate = insertDate, ErrorCode = errorCode });
        }
    }
}
