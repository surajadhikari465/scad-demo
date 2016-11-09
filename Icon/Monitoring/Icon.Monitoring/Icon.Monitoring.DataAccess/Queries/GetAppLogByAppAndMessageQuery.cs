using Dapper;
using Icon.Monitoring.Common.Enums;
using Icon.Monitoring.DataAccess.Model;
using System;
using System.Linq;

namespace Icon.Monitoring.DataAccess.Queries
{
    public class GetAppLogByAppAndMessageQuery : IQueryByRegionHandler<GetAppLogByAppAndMessageParameters, AppLog>
    {
        private IDbProvider db;
        public IrmaRegions TargetRegion { get; set; }

        public GetAppLogByAppAndMessageQuery(IDbProvider db)
        {
            this.db = db;
        }

        public AppLog Search(GetAppLogByAppAndMessageParameters parameters)
        {
            string sql = @"SELECT TOP 1
                           a.Name,
                           al.LogDate, 
                           al.HostName, 
                           al.UserName, 
                           al.Thread, 
                           al.Level, 
                           al.Logger, 
                           al.Message, 
                           al.insertdate
                        FROM
                           AppConfigApp a
                           JOIN AppLog al on al.ApplicationId = a.ApplicationId
                        WHERE
                           a.Name = @AppConfigAppName
                           and al.logdate between @StartTime and @EndTime
                           and al.message = @Message
                        ORDER BY
                           al.Id DESC";

            AppLog appLogEntry = this.db.Connection
                .Query<AppLog>(
                    sql,
                    new
                    {
                        AppConfigAppName = parameters.AppConfigAppName,
                        StartTime = parameters.StartDate,
                        EndTime = parameters.EndDate,
                        Message = parameters.Message
                    },
                    this.db.Transaction)
                .FirstOrDefault();

            return appLogEntry;
        }
    }
}
