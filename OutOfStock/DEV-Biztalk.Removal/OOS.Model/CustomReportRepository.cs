using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using OutOfStock.MoreLinq;

namespace OOS.Model
{
   

    public class CustomReportRepository : ICustomReportRepository
    {
        private IConfigurator config;

        private const string SelectClause =
         @"SELECT rd.id, st.STORE_NAME ,
                st.STORE_ABBREVIATION,
                rd.UPC ,
                rd.PS_TEAM ,
                rd.PS_SUBTEAM ,
                rd.CASE_SIZE ,
                rd.EFF_COST ,
                rd.EFF_PRICE ,
                rd.EFF_PRICETYPE ,
                rd.MOVEMENT ,
                rd.NOTES ,
                rd.VENDOR_KEY ,
                rd.VIN ,
                rd.BRAND ,
                rd.BRAND_NAME ,
                rd.LONG_DESCRIPTION ,
                rd.ITEM_SIZE ,
                rd.ITEM_UOM ,
                rd.CATEGORY_NAME ,
                rd.CLASS_NAME ,
                ps.ProductStatus ,
                ps.StartDate
        FROM    REPORT_HEADER rh
        JOIN REPORT_DETAIL rd ON rd.REPORT_HEADER_ID = rh.ID
        JOIN STORE st ON rh.STORE_ID = st.ID
        JOIN REGION rg ON rg.ID = st.REGION_ID
        LEFT JOIN ProductStatus ps ON ( ps.Region = rg.REGION_ABBR
                                        AND ps.UPC = rd.UPC
										AND (ExpirationDate is null or ExpirationDate > getdate())
                                      )
        WHERE   rh.CREATED_DATE BETWEEN '{0}' AND '{1}'";

        private const string OrderByClause = " order by rd.id ";
        private const string GroupByClause = "";

        
        public CustomReportRepository(IConfigurator config)
        {
            this.config = config;
        }

        public OOSCustomReport For(DateTime startDate, DateTime endDate, List<int> storeIds, List<string> teams, List<string> subTeams)
        {
            // get raw data. no grouping.
            var customReportDtos = CustomReportFor(startDate, endDate, storeIds, teams, subTeams);

            // get comma separated list of stores for each UPC.
            var StoresByUPC = from i in customReportDtos
                              group i.STORE_ABBREVIATION by i.UPC
                              into grp
                              select new {UPC = grp.Key, Stores = string.Join(",", grp.Distinct())};

            // get scan counts  for each UPC
            var CountsByUPC = from i in customReportDtos
                              group i by i.UPC
                              into g
                              select new {UPC = g.Key, ScanCount = g.Count()};

            // for each distinct UPC get the most recent record by ReportDetail.ID
            // This uses a Linq Extension called MoreLinq.MaxBy included in OOS.Model
            var MostRecentByUPC = from i in customReportDtos
                                  group i by i.UPC
                                  into g
                                  select g.MaxBy(x => x.ID);

            // get avg values for each UPC
            var AveragesByUPC = from i in customReportDtos
                                group i by i.UPC
                                into g
                                select new
                                    {
                                        UPC = g.Key,
                                        AvgEffCost = g.Where(p => p.EFF_COST.HasValue).Average(p => p.EFF_COST),
                                        AvgEffPrice = g.Where(p => p.EFF_PRICE.HasValue).Average(p => p.EFF_PRICE),
                                        AvgMovement = g.Where(p => p.MOVEMENT.HasValue).Average(p => p.MOVEMENT)
                                    };
            // combine all the previous datasets together and create report data.
            var entries = from r in MostRecentByUPC
                          join s in StoresByUPC on r.UPC equals s.UPC
                          join cnt in CountsByUPC on r.UPC equals cnt.UPC
                          join a in AveragesByUPC on r.UPC equals a.UPC
                          select new CustomReportEntry(
                                  r.UPC,
                                  r.PS_TEAM,
                                  r.PS_SUBTEAM,
                                  (r.CASE_SIZE.HasValue ? r.CASE_SIZE.Value :  0),
                                  (a.AvgEffCost.HasValue ? a.AvgEffCost.Value : 0),
                                  (a.AvgEffPrice.HasValue ? a.AvgEffPrice.Value : 0),
                                  r.EFF_PRICETYPE,
                                  (a.AvgMovement.HasValue ? a.AvgMovement.Value : 0),
                                  r.NOTES,
                                  r.VENDOR_KEY,
                                  r.VIN,
                                  r.BRAND,
                                  r.BRAND_NAME,
                                  r.LONG_DESCRIPTION,
                                  r.ITEM_SIZE,
                                  r.ITEM_UOM,
                                  r.CATEGORY_NAME,
                                  r.CLASS_NAME,
                                  r.ProductStatus,
                                  r.StartDate,
                                  cnt.ScanCount,
                                  s.Stores,
                                  r.LAST_DATE_SOLD,
                                  r.DAYS_WITH_SALES,
                                  0,
                                  0
                              );

            //create report object and add data to it.
           var report = new OOSCustomReport(startDate, endDate);           
           entries.ToList().ForEach(report.Add);
           return report;
        }
 
        internal List<CustomReportDTO> CustomReportFor(DateTime startDate, DateTime endDate, List<int> storeIds, List<string> teams, List<string> subTeams)
        {
            string query = SQLFor(startDate, endDate, storeIds, teams, subTeams);
            return ExecuteSQL(query);
        }

        private string SQLFor(DateTime startDate, DateTime endDate, List<int> storeIds, List<string> teams, List<string> subTeams)
        {
            string sql = CustomReportSQL(storeIds, teams, subTeams);
            return string.Format(sql, startDate, endDate);
        }

        private string CustomReportSQL(List<int> storeIds, List<string> teams, List<string> subTeams)
        {
            return new StringBuilder(SelectClause).Append(StoreIDFilter(storeIds)).Append(TeamFilter(teams)).Append(SubteamFilter(subTeams)).
                Append(GroupByClause).Append(OrderByClause).ToString();
        }

        private string StoreIDFilter(List<int> storeIds)
        {
            if (storeIds != null && storeIds.Any())
            {
                var builder = new StringBuilder("and rh.STORE_ID in (");
                for (int i = 0; i < storeIds.Count; i++ )
                {
                    if (i > 0) builder.Append(',');
                    builder.Append(storeIds[i]);
                }
                builder.Append(") ");
                return builder.ToString();
            }
            return string.Empty;
        }

        private string TeamFilter(List<string> teams)
        {
            var teamNames = Valid(teams);
            if (teamNames.Any())
            {
                var builder = new StringBuilder("and rd.PS_TEAM in (");
                return builder.Append(BuildFilter(teams)).ToString();
            }
            return string.Empty;
        }

        private List<string> Valid(List<string> nameList)
        {
            return (nameList != null && nameList.Any()) ? nameList.Where(p => !string.IsNullOrWhiteSpace(p)).ToList() : new List<string>();
        }

        private string BuildFilter(List<string> names)
        {
            Debug.Assert(names != null && names.Any());

            var builder = new StringBuilder();
            for (int i = 0; i < names.Count; i++)
            {
                if (i > 0) builder.Append(',');
                builder.Append("'");
                builder.Append(names[i]);
                builder.Append("'");
            }
            builder.Append(") ");
            return builder.ToString();
        }

        private string SubteamFilter(List<string> subteams)
        {
            var subteamNames = Valid(subteams);
            if (subteamNames.Any())
            {
                var builder = new StringBuilder("and rd.PS_SUBTEAM in (");
                return builder.Append(BuildFilter(subteams)).ToString();
            }
            return string.Empty;
        }

        private List<CustomReportDTO> ExecuteSQL(string query)
        {
            using (var oosDataContext = new System.Data.Linq.DataContext(config.GetOOSConnectionString()))
            {
                var oosItems = oosDataContext.ExecuteQuery<CustomReportDTO>(query, new object[] { });
                var itemList = oosItems.ToList();
                return itemList;
            }
        }

    }
}
