using System;
using System.Collections.Generic;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using OOSCommon.DataContext;
using StructureMap;

namespace OOS.Model
{
    public class RegionSummaryRepository : AbstractSummaryRepository
    {
        private IOOSEntitiesFactory dbFactory;
        private Region region;
        private IConfigurator config;

        public RegionSummaryRepository(Region region, IOOSEntitiesFactory dbFactory, IConfigurator config)
        {
            this.region = region;
            this.dbFactory = dbFactory;
            this.config = config;
        }


        internal override OOSCountSummary OOSCountSummaryForOverlay()
        {
            var regionSummary = new OOSCountSummary();
            List<Store> stores = region.OpenStores();
            foreach (var store in stores)
            {
                regionSummary.Add(store.Name, "Grocery", 0);
                regionSummary.Add(store.Name, "Whole Body", 0);
            }
            return regionSummary;
        }


        internal OOSCountSummary OOSCountSummaryBy(DateTime startDate, DateTime endDate)
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
                        //&& (true && (startDate <= rh.OffsetCorrectedCreateDate && endDate >= rh.OffsetCorrectedCreateDate) )
                        //&& rh.CREATED_DATE >= startDate && rh.CREATED_DATE <= endDate
                          && rd.UPC != null
                          && st.REGION_ID == region.Id
                          && rh.EXCLUDE_FLAG == null
                    select
                        new
                        {
                            st.STORE_NAME,
                            rd.PS_TEAM,
                            rd.UPC,
                            rh.CREATED_DATE,
                            rh.OffsetCorrectedCreateDate,
                            rh.TimeOffsetFromCentral
                        });


                var filtered = from i in query
                    where i.OffsetCorrectedCreateDate >= startDate && i.OffsetCorrectedCreateDate <= endDate
                    select i;

                var grouped = from f in filtered
                    group new {f.STORE_NAME, f.PS_TEAM} by new {f.STORE_NAME, f.PS_TEAM}
                    into g
                    select g;

                    //query.Where(q => q.OffsetCorrectedCreateDate >= startDate && q.OffsetCorrectedCreateDate <= endDate);

                



                             //group new { st.STORE_NAME, rd.PS_TEAM } by new { st.STORE_NAME, rd.PS_TEAM } into g
                             //select g);

                var cache = grouped.ToList();
                var summary = new OOSCountSummary();
                cache.ForEach(p => summary.Add(p.Key.STORE_NAME, p.Key.PS_TEAM, p.Count()));
                return summary;
            }
        }

        internal override OOSCountSummary OOSCountSummaryFor(DateTime startDate, DateTime endDate)
        {
            string sql = GetCountSummarySQL();
            var fromDate = startDate;
            var toDate = endDate;
            var query = string.Format(sql, fromDate, toDate, region.Abbrev);
            using (var oosDataContext = new System.Data.Linq.DataContext(config.GetOOSConnectionString()))
            {
                var oosItems = oosDataContext.ExecuteQuery<OOSCountDTO>(query, new object[] { });
                var itemList = oosItems.ToList();
                var summary = new OOSCountSummary();
                itemList.ForEach(p => summary.Add(p.StoreName, p.TeamName, p.OOSCount));
                return summary;
            }
        }

        private string GetCountSummarySQL()
        {
            return @"select     st.STORE_NAME as StoreName, 
                                rd.PS_TEAM as TeamName, 
                                count(rd.PS_TEAM) as OOSCount 
                     from       REPORT_DETAIL rd, 
                                REPORT_HEADER rh, 
                                STORE st, 
                                TEAM_Interim ti 
                    where       rd.REPORT_HEADER_ID=rh.ID and 
                                st.ID=rh.STORE_ID and 
                                ti.teamName=rd.PS_TEAM and 
                                rh.OffsetCorrectedCreateDate between '{0}' and '{1}' and 
                                rd.UPC is not null 
                                and st.REGION_ID=(select ID from REGION where REGION_ABBR='{2}') and 
                                rh.EXCLUDE_FLAG is null 
                                group by st.STORE_NAME, rd.PS_TEAM order by st.STORE_NAME, rd.PS_TEAM";
        }

        internal override SKUSummary SKUSummaryBy()
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
                                   && re.REGION_ABBR == region.Abbrev
                             orderby st.STORE_NAME, ti.teamName
                             select new { Store = st.STORE_NAME, TeamName = ti.teamName, SKUCount = sc.numberOfSKUs });
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
                             from re in db.REGION
                             where rh.STORE_ID == st.ID
                                && st.REGION_ID == re.ID
                                && rh.OffsetCorrectedCreateDate >= startDate
                                && rh.OffsetCorrectedCreateDate <= endDate
                                && re.REGION_ABBR == region.Abbrev
                                && rh.EXCLUDE_FLAG == null
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
