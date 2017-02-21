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
        { // get the date and time last log was written by tlog controller
            string sql = @"DECLARE @AppID int ;
                            SELECT @AppID = (SELECT AppID from app.App a  where a.AppName = 'TLog Controller');
                          	SELECT TOP 1 LogDate FROM app.AppLog al
							WHERE AppID =@AppID
							   ORDER BY al.AppLogID DESC;";
            string result = (this.db.Connection.ExecuteScalar(sql)).ToString();
            return result;
        }
    }
}
