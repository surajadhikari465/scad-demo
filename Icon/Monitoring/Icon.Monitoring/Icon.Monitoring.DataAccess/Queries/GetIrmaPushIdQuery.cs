using Dapper;
using Icon.Common.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Monitoring.DataAccess.Queries
{
    public class GetIrmaPushIdQuery : IQueryHandler<GetIrmaPushIdParameters, int>
    {
        private IDbProvider db;
        

        public GetIrmaPushIdQuery(IDbProvider db)
        {
            this.db = db;
        }

        public int Search(GetIrmaPushIdParameters parameters)
        {
           string sql = @"select top 1 p.IRMAPushID 
                from app.IRMAPush p (nolock)
                where p.InUdmDate IS NULL
                AND p.EsbReadyDate IS NULL
                AND p.UdmFailedDate IS NULL
                AND p.EsbReadyFailedDate IS NULL
                order by IRMAPushID asc";
            int result = this.db.Connection.Query<int>(sql, transaction: this.db.Transaction).FirstOrDefault();

            return result;
        }
    }
}
