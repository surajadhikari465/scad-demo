using Dapper;
using Icon.Common.DataAccess;
using Icon.Monitoring.Common.Dvo;
using System;

namespace Icon.Monitoring.DataAccess.Queries
{
    public class GetTConLogServiceLastLogDate: IQueryHandler<GetTConLogServiceLastLogDateParameters, string>
    {
        private IDbProvider db;

        public GetTConLogServiceLastLogDate(IDbProvider db)
        {
            this.db = db;
        }

        public string Search(GetTConLogServiceLastLogDateParameters parameters)
        { 
            string sql = @"SELECT  TOP 1 logdate  from app.AppLog al
                            join app.App a on al.AppID = a.AppID
                            where a.AppName = 'TLog Controller'
                            order by al.AppLogID DESC";
            string result = (string)(this.db.Connection.ExecuteScalar(sql));
            return result;
        }
    }
}
