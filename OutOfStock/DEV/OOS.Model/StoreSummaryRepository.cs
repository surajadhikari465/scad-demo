using System;
using System.Collections.Generic;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Text;
using OOSCommon.DataContext;

namespace OOS.Model
{
    public class StoreSummaryRepository : AbstractSummaryRepository
    {
        private IOOSEntitiesFactory dbFactory; 
        private Store store;

        public StoreSummaryRepository(Store store, IOOSEntitiesFactory dbFactory)
        {
            this.dbFactory = dbFactory;
            this.store = store;
        }

        internal override OOSCountSummary OOSCountSummaryFor(DateTime startDate, DateTime endDate)
        {
            using (var db = dbFactory.New())
            {
                var query = (from rd in db.REPORT_DETAIL
                             from rh in db.REPORT_HEADER
                             from st in db.STORE
                             from ti in db.TEAM_Interim
                             where rd.REPORT_HEADER_ID == rh.ID
                                   && st.ID == rh.STORE_ID
                                   && ti.teamName == rd.PS_TEAM
                                   && rh.OffsetCorrectedCreateDate >= startDate
                                   && rh.OffsetCorrectedCreateDate <= endDate
                                   && rd.UPC != null
                                   && st.STORE_ABBREVIATION == store.Abbrev
                             group new { st.STORE_NAME, rd.PS_TEAM } by new { st.STORE_NAME, rd.PS_TEAM } into g
                             select g);
                var cache = query.ToList();
                var summary = new OOSCountSummary();
                cache.ForEach(p => summary.Add(p.Key.STORE_NAME, p.Key.PS_TEAM, p.Count()));
                return summary;
            }
        }


        internal override OOSCountSummary OOSCountSummaryForOverlay()
        {
            var storeSummary = new OOSCountSummary();
            storeSummary.Add(store.Name, "Grocery", 0);
            storeSummary.Add(store.Name, "Whole Body", 0);
            return storeSummary;
        }


        internal override SKUSummary SKUSummaryBy()
        {
            using (var db = dbFactory.New())
            {
                var query = (from sc in db.SKUCount
                             from ti in db.TEAM_Interim
                             from st in db.STORE
                             where sc.TEAM_ID == ti.idTeam
                                   && SqlFunctions.StringConvert((decimal)sc.STORE_PS_BU).Trim() == st.PS_BU.Trim()
                                   && st.STORE_ABBREVIATION == store.Abbrev
                                   && !st.Hidden
                             select new { Store = st.STORE_NAME, TeamName = ti.teamName, SKUCount = sc.numberOfSKUs }).Distinct();
                var cache = query.ToList();
                var summary = new SKUSummary();
                cache.ForEach(p => summary.Add(p.Store, p.TeamName, (int)p.SKUCount));
                return summary;
            }
        }

        internal override ScanSummary NumberOfScansBy(DateTime startDate, DateTime endDate)
        {
            using (var db = dbFactory.New())
            {
                var query = (from rh in db.REPORT_HEADER
                             from st in db.STORE
                             where rh.STORE_ID == st.ID
                                && rh.OffsetCorrectedCreateDate >= startDate
                                && rh.OffsetCorrectedCreateDate <= endDate
                                && st.STORE_ABBREVIATION == store.Abbrev
                                && !st.Hidden
                             group new { st.STORE_NAME } by new { st.STORE_NAME } into g
                             select g);
                var cache = query.ToList();
                var summary = new ScanSummary();
                cache.ForEach(p => summary.Add(p.Key.STORE_NAME, p.Count()));
                return summary;
            }
        }

    }

}
