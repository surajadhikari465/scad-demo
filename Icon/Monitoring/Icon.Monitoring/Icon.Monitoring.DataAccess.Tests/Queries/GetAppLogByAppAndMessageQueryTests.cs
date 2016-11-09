using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Icon.Monitoring.DataAccess.Queries;
using System.Data.SqlClient;
using System.Configuration;
using Icon.Monitoring.Common.Constants;
using Icon.Monitoring.Common.Enums;
using Icon.Monitoring.DataAccess.Model;
using Dapper;
using System.Linq;

namespace Icon.Monitoring.DataAccess.Tests.Queries
{
    [TestClass]
    public class GetAppLogByAppAndMessageQueryTests
    {
        private SqlDbProvider db;
        private GetAppLogByAppAndMessageQuery query;
        private GetAppLogByAppAndMessageParameters parameters;
        private AppLog appLog;
        private Guid applicationId;

        [TestInitialize]
        public void Initialize()
        {
            this.db = new SqlDbProvider();
            this.db.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ItemCatalog_FL"].ConnectionString);
            this.db.Connection.Open();
            this.db.Transaction = db.Connection.BeginTransaction();

            this.parameters = new GetAppLogByAppAndMessageParameters
            {
                AppConfigAppName = IrmaAppConfigAppNames.PosPushJob,
                StartDate = DateTime.Today,
                EndDate = DateTime.Now.AddMinutes(5)
            };

            this.query = new GetAppLogByAppAndMessageQuery(this.db);
            this.query.TargetRegion = IrmaRegions.FL;

            this.applicationId = this.db.Connection
                .Query<Guid>(@"SELECT ApplicationID FROM AppConfigApp WHERE Name = @AppConfigAppName",
                    new { AppConfigAppName = IrmaAppConfigAppNames.PosPushJob }, this.db.Transaction)
                .First();
        }

        [TestCleanup]
        public void CleanupTest()
        {
            this.db.Transaction.Rollback();
            this.db.Connection.Close();
            this.db.Connection.Dispose();
        }

        [TestMethod]
        public void GetAppLogSearch_AppLogExistsWithMessage_ReturnsAppLogRecord()
        {
            // Given
            this.appLog = BuildAppLogRow();
            InsertAppLogRow(this.appLog);
            this.parameters.Message = this.appLog.Message;

            // When
            AppLog actual = this.query.Search(this.parameters);

            // Then
            Assert.IsNotNull(actual);
            Assert.AreEqual(this.appLog.HostName, actual.HostName);
            Assert.AreEqual(this.appLog.InsertDate.ToString("yyyy-dd-mm hh:mm"), actual.InsertDate.ToString("yyyy-dd-mm hh:mm"));
            Assert.AreEqual(this.appLog.Level, actual.Level);
            Assert.AreEqual(this.appLog.LogDate.ToString("yyyy-dd-mm hh:mm"), actual.LogDate.ToString("yyyy-dd-mm hh:mm"));
            Assert.AreEqual(this.appLog.Logger, actual.Logger);
            Assert.AreEqual(this.appLog.Message, actual.Message);
            Assert.AreEqual(this.appLog.Name, actual.Name);
            Assert.AreEqual(this.appLog.Thread, actual.Thread);
            Assert.AreEqual(this.appLog.UserName, actual.UserName);
        }

        [TestMethod]
        public void GetAppLogSearch_AppLogDoesNotExists_ReturnsNullAppLogObject()
        {
            // Given
            this.parameters.Message = "testing AppLog Search for No result";

            // When
            AppLog actual = this.query.Search(this.parameters);

            // Then
            Assert.IsNull(actual);
        }

        private AppLog BuildAppLogRow()
        {
            AppLog appLog = new AppLog
            {
                HostName = "unit test",
                InsertDate = DateTime.Now.AddSeconds(-5),
                Level = "Info",
                LogDate = DateTime.Now.AddSeconds(-5),
                Logger = "Icon.Monitoring.UnitTest",
                Message = "unit test is starting",
                Name = "POS PUSH JOB",
                Thread = 1,
                UserName = "myName.Test"
            };

            return appLog;
        }

        private void InsertAppLogRow(AppLog appLog)
        {
            string sql = @"INSERT INTO AppLog
                        (
                            [LogDate]
                           ,[ApplicationID]
                           ,[HostName]
                           ,[UserName]
                           ,[Thread]
                           ,[Level]
                           ,[Logger]
                           ,[Message]
                           ,[Exception]
                           ,[InsertDate]
                        )
                        VALUES
                        (
                            @LogDate
                           ,@ApplicationID
                           ,@HostName
                           ,@UserName
                           ,@Thread
                           ,@Level
                           ,@Logger
                           ,@Message
                           ,@Exception
                           ,@InsertDate
                        )";

            int affectedRows = this.db.Connection.Execute(
                sql,
                new
                {
                    LogDate = appLog.LogDate,
                    ApplicationID = this.applicationId,
                    HostName = appLog.HostName,
                    UserName = appLog.UserName,
                    Thread = appLog.Thread,
                    Level = appLog.Level,
                    Logger = appLog.Logger,
                    Message = appLog.Message,
                    Exception = String.Empty,
                    InsertDate = appLog.InsertDate
                },
                this.db.Transaction);
        }
    }
}
