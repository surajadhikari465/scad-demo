using Dapper;
using Icon.Common.DataAccess;
using Icon.Monitoring.Common.Dvo;
using Icon.Monitoring.DataAccess.Model;
using System;
using System.Linq;

namespace Icon.Monitoring.DataAccess.Queries
{
    public class GetLatestAppLogByAppNameQuery : IQueryHandler<GetLatestAppLogByAppNameParameters, AppLog>
    {
        private IDbProvider db;

        public GetLatestAppLogByAppNameQuery(IDbProvider db)
        {
            this.db = db;
        }

        public AppLog Search(GetLatestAppLogByAppNameParameters parameters)
        { // get the date and time last log was written by tlog controller
            string sql = @"SELECT TOP 1
                         [AppLogID]
                          ,[AppID]
                          ,[UserName]
                          ,[InsertDate]
                          ,[LogDate]
                          ,[Level]
                          ,[Logger]
                          ,[Message]
                          ,[MachineName]
                        FROM
                           app.AppLog
                         
                        WHERE
                           AppID = (SELECT AppID from app.App a  where a.AppName = @AppName)
                           ORDER BY AppLogID DESC";

            AppLog appLogEntry = this.db.Connection
                .Query<AppLog>(
                    sql,
                    new
                    {
                        AppName = parameters.AppName
                       
                    },
                    this.db.Transaction)
                .FirstOrDefault();

            return appLogEntry;
        }
    }
}
