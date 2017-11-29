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
    public class GetLatestAppLogByAppNameQueryTests
    {
        private SqlDbProvider provider;
        private GetLatestAppLogByAppNameQuery query;
        private GetLatestAppLogByAppNameParameters parameters;
        private const string AppName = "TLog Controller";
        DateTime curentDate = DateTime.Now;

        [TestInitialize]
        public void Initialize()
        {
            provider = new SqlDbProvider();
            provider.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Icon"].ConnectionString);
            provider.Connection.Open();
            provider.Transaction = provider.Connection.BeginTransaction();
            query = new GetLatestAppLogByAppNameQuery(provider);
            parameters = new GetLatestAppLogByAppNameParameters(AppName);
        }

        [TestMethod]
        public void GetLatestAppLogByAppNameSearch_DataExists_ReturnsAppLog()
        {
            //Given
            int applogId = InsertAppLog(curentDate);

            //When
            AppLog applog = query.Search(parameters);

            //Then
            Assert.AreEqual(curentDate.Hour, applog.LogDate.Hour);
            Assert.AreEqual(curentDate.Minute, applog.LogDate.Minute);
            Assert.AreEqual(curentDate.Day, applog.LogDate.Day);
            Assert.AreEqual(curentDate.Month, applog.LogDate.Month);
        }

        private int InsertAppLog(DateTime curentDate)
        {
            return provider.Connection.Query<int>(
        @"INSERT INTO [app].[AppLog]
           ([AppID]
           ,[UserName]
           ,[InsertDate]
           ,[LogDate]
           ,[Level]
           ,[Logger]
           ,[Message]
           ,[MachineName])
     VALUES
           (@AppID
           ,@UserName
           ,@InsertDate
           ,@LogDate
           ,@Level
           ,@Logger
           ,@Message
           ,@MachineName
            )
            select SCOPE_IDENTITY()",
                            new
                            {
                                AppID = 11,
                                UserName = "Test",
                                InsertDate = curentDate,
                                LogDate = curentDate,
                                Level = "Info",
                                Logger = "Test",
                                Message = "test",
                                MachineName = "CEWD6592"
                            },
                            provider.Transaction)
                            .FirstOrDefault();
        }
    }
}
