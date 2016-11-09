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

namespace Icon.Monitoring.DataAccess.Tests.Queries
{
    [TestClass]
    public class GetGloConItemQueueIdQueryTest
    {
        private SqlDbProvider provider;
        private GetGloConItemQueueIdQuery query;
        private GetGloConItemQueueIdParameters parameters;

        [TestInitialize]
        public void Initialize()
        {
            provider = new SqlDbProvider();
            provider.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Icon"].ConnectionString);
            provider.Connection.Open();
            provider.Transaction = provider.Connection.BeginTransaction();
            query = new GetGloConItemQueueIdQuery(provider);
            parameters = new GetGloConItemQueueIdParameters();
            UpdateEventQueueSetProcessedFailedToNonNull();
           
        }
        [TestMethod]
        public void GetGloConItemQueueIdSearch_DataExist_ReturnQueueId()
        {
            //Given
            InsertGloConItemQueue();
            int result;

            //When
            result = query.Search(parameters);

            //Then
            Assert.AreEqual(1, result);

        }

        [TestMethod]
        public void GetGloConItemQueueIdSearch_DataDoesNotExist_ReturnZero()
        {
            //Given           
            int result;

            //When
            result = query.Search(parameters);

            //Then
            Assert.AreEqual(0, result);

        }

        [TestCleanup]
        public void CleanUp()
        {
            provider.Transaction.Rollback();
            provider.Connection.Close();
        }

        private void InsertGloConItemQueue()
        {
            provider.Connection.Query<int>(
            @"Set Identity_Insert app.EventQueue ON  INSERT INTO[app].[EventQueue]
           (QueueId
           ,[EventId]
           ,[EventMessage]
           ,[EventReferenceId]
           ,[RegionCode]
           ,[InsertDate]
           ,[ProcessFailedDate]
           ,[InProcessBy])
     VALUES
           ( 1
            ,2
            ,'Test'
           ,4011
           ,'SP'
           ,GetDate()
           ,Null
           ,'19911')  Set Identity_Insert app.EventQueue OFF",
            null, 
                provider.Transaction)
                .FirstOrDefault();

        }

        private void UpdateEventQueueSetProcessedFailedToNonNull()
        {
            provider.Connection.Query<int>(
            @"UPDATE[app].[EventQueue]
        SET [ProcessFailedDate] = GetDate()", null,
                provider.Transaction)
                .FirstOrDefault();

        }
    }
}
