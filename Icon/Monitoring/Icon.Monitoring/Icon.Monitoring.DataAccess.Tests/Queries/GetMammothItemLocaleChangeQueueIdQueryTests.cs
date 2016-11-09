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
    [TestClass()]
    public class GetMammothItemLocaleChangeQueueIdQueryTests
    {
        private SqlDbProvider provider;
        private GetMammothItemLocaleChangeQueueIdQuery query;
        private GetMammothItemLocaleChangeQueueIdQueryParameters parameters;

        [TestInitialize]
        public void Initialize()
        {
            provider = new SqlDbProvider();
            provider.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ItemCatalog_FL"].ConnectionString);
            provider.Connection.Open();
            provider.Transaction = provider.Connection.BeginTransaction();
            query = new GetMammothItemLocaleChangeQueueIdQuery(provider);
            parameters = new GetMammothItemLocaleChangeQueueIdQueryParameters();
        }

        [TestCleanup]
        public void CleanUp()
        {
            provider.Transaction.Rollback();
            provider.Connection.Close();
        }

        [TestMethod]
        public void GetMammothItemLocaleChangeQueueIdSearch_UnprocessedRowsExist_ReturnId()
        {
            //Given
            int result, insertId;
            insertId = InsertMammothItemLocaleChangeQueueTable();

            //When
            result = query.Search(parameters);

            //Then
            Assert.AreEqual(insertId, result);

        }

        [TestMethod]
        public void GetMammothItemLocaleChangeQueueIdSearch_UnprocessedRowsDontExist_ReturnZero()
        {
            //Given
            int result;

            //When
            result = query.Search(parameters);

            //Then
            Assert.AreEqual(0, result);

        }
        private int InsertMammothItemLocaleChangeQueueTable()
        {
            return provider.Connection.Query<int>(
        @"INSERT INTO [mammoth].[ItemLocaleChangeQueue]
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
           ,789)select SCOPE_IDENTITY()",
        null,
            provider.Transaction)
            .FirstOrDefault();
        }

    }
}

