using System;
using System.Collections.Generic;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Text;

namespace OOS.Model
{
    // TODO
    public class StoreSkuCountReader : IStoreSkuCountReader
    {
        private IOOSEntitiesFactory dbFactory;

        public StoreSkuCountReader(IOOSEntitiesFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public SKUSummary SkuSummary(string regionAbbrev)
        {
            return SKUSummaryBy(regionAbbrev);
        }

        private SKUSummary SKUSummaryBy(string regionAbbrev)
        {
            using (var db = dbFactory.New())
            {
                var query = (from sc in db.SKUCount
                             from ti in db.TEAM_Interim
                             from st in db.STORE
                             from re in db.REGION
                             where sc.TEAM_ID == ti.idTeam
                                   && SqlFunctions.StringConvert((decimal)sc.STORE_PS_BU).Trim() == st.PS_BU.Trim()
                                   && st.REGION_ID == re.ID
                                   && re.REGION_ABBR == regionAbbrev
                             orderby st.STORE_NAME, ti.teamName
                             select new { Store = st.STORE_NAME, TeamName = ti.teamName, SKUCount = sc.numberOfSKUs });
                var cache = query.ToList();
                var summary = new SKUSummary();
                cache.ForEach(p => summary.Add(p.Store, p.TeamName, (int)p.SKUCount));
                return summary;
            }
        }
    }
}
