using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace OOS.Model
{
    public class CustomReportRepository : ICustomReportRepository
    {
        private IConfigurator config;

        private const string SelectClause =
         @"SELECT  st.STORE_NAME ,
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
        ps.StartDate ,
        COUNT(rd.VIN) AS timesScanned
FROM    REPORT_HEADER rh
        JOIN REPORT_DETAIL rd ON rd.REPORT_HEADER_ID = rh.ID
        JOIN STORE st ON rh.STORE_ID = st.ID
        JOIN REGION rg ON rg.ID = st.REGION_ID
        LEFT JOIN ProductStatus ps ON ( ps.Region = rg.REGION_ABBR
                                        AND ps.UPC = rd.UPC
										AND (ExpirationDate is null or ExpirationDate > getdate())
                                      )
WHERE   rh.CREATED_DATE BETWEEN '{0}' AND '{1}'";


        private const string GroupByClause = "group by st.STORE_NAME, st.STORE_ABBREVIATION, rd.UPC, rd.PS_TEAM, rd.PS_SUBTEAM, rd.CASE_SIZE, rd.EFF_COST, rd.EFF_PRICE, rd.EFF_PRICETYPE, rd.MOVEMENT, rd.NOTES, rd.VENDOR_KEY, rd.VIN, rd.BRAND, rd.BRAND_NAME, rd.LONG_DESCRIPTION, rd.ITEM_SIZE, rd.ITEM_UOM, rd.CATEGORY_NAME, rd.CLASS_NAME, ps.ProductStatus, ps.StartDate";

        public CustomReportRepository(IConfigurator config)
        {
            this.config = config;
        }

        public OOSCustomReport For(DateTime startDate, DateTime endDate, List<int> storeIds, List<string> teams, List<string> subTeams)
        {
            var customReportDtos = CustomReportFor(startDate, endDate, storeIds, teams, subTeams);

            var StoresByUPC = from i in customReportDtos
                              group i.STORE_ABBREVIATION by i.UPC
                              into grp
                              select new {UPC = grp.Key, Stores = string.Join(",", grp)};
                              

            

            var entries = (from c in customReportDtos join s in StoresByUPC on c.UPC equals s.UPC
                           group c by new { c.UPC, c.PS_TEAM, c.PS_SUBTEAM, c.CASE_SIZE, c.EFF_PRICE, c.EFF_PRICETYPE, c.NOTES, c.VENDOR_KEY, c.VIN, s.Stores } into g
                             select new CustomReportEntry (g.Key.UPC,
                                 g.Key.PS_TEAM, 
                                 g.Key.PS_SUBTEAM, 
                                 g.Key.CASE_SIZE.HasValue ? g.Key.CASE_SIZE.Value : 0, 
                                 g.Any(p => p.EFF_COST.HasValue) ? g.Where(p => p.EFF_COST.HasValue).Average(p => p.EFF_COST.Value) : 0,
                                 g.Key.EFF_PRICE.HasValue ? g.Key.EFF_PRICE.Value : 0, 
                                 g.Key.EFF_PRICETYPE, 
                                 g.Any(p => p.MOVEMENT.HasValue) ? g.Where(p => p.MOVEMENT.HasValue).Average(p => p.MOVEMENT.Value) : 0,
                                 g.Key.NOTES, 
                                 g.Key.VENDOR_KEY, 
                                 g.Key.VIN, 
                                 g.First().BRAND,
                                 g.First().BRAND_NAME,
                                 g.First().LONG_DESCRIPTION,
                                 g.First().ITEM_SIZE,
                                 g.First().ITEM_UOM,
                                 g.First().CATEGORY_NAME,
                                 g.First().CLASS_NAME,
                                 g.First().ProductStatus,
                                 g.First().StartDate,
                                 (int)g.Sum(p => p.timesScanned), g.Key.Stores)).ToList();
            var report = new OOSCustomReport(startDate, endDate);           
            entries.ForEach(report.Add);
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
                Append(GroupByClause).ToString();
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
