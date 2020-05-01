using Dapper;
using Icon.Monitoring.Common.Enums;
using Icon.Monitoring.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Monitoring.DataAccess.Queries
{
    public class GetMammothFailedEventsQuery : IQueryByRegionHandler<GetMammothFailedEventsParameters, List<MammothFailedEvent>>
    {
        public IrmaRegions TargetRegion { get; set; }
        
        private IDbProvider db;

        public GetMammothFailedEventsQuery(IDbProvider db)
        {
            this.db = db;
        }

        public List<MammothFailedEvent> Search(GetMammothFailedEventsParameters parameters)
        {
            return db.Connection.Query<MammothFailedEvent>(
                @"SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
                SELECT 
                    Identifier, 
                    Item_Key as ItemKey, 
                    Store_No as StoreNo, 
                    InsertDate 
                FROM mammoth.ChangeQueueHistory 
                WHERE InsertDate BETWEEN @BeginDate AND @EndDate
                    AND ErrorCode IS NOT NULL",
                parameters)
                .ToList();
        }
    }
}
