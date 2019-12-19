using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Linq;
using System.Linq;
using OOSCommon.DataAccess;
using OOSCommon.DataContext;

namespace OOSCommon.VIM
{
    public class VimTeam
    {
        public string team_name { get; set; }

        public static IEnumerable<VimTeam> RunQuery(List<REGION> regions)
        {
            // bug oracle dependency [fixed]


            string regionAbbreviations = string.Join(",", regions.Select(r => $"'{r.REGION_ABBR}'"));
            
            string query = $"SELECT DISTINCT team_name FROM vim.crs_team_ref where region in ({regionAbbreviations})  ORDER BY team_name";
            //string querySQLServer =
            //    // The SQL Server part that needs to match this class
            //    "SELECT team_name " +
            //    "FROM OPENQUERY(" + VIM.VIMRepository.oosVIMServiceName + ", '" +
            //        // The Oracle part ... remember to escape single quotes
            //        queryOracle.Replace("'", "''") +
            //    "')";

            OracleDataAccess oracle;
            using (oracle = new OracleDataAccess(AppConfig.ConnectionStrings["VIM"].ConnectionString))
            {
                return oracle.ReturnList<VimTeam>(CommandType.Text, query);
            }

            //System.Data.Linq.DataContext dc =
            //    new System.Data.Linq.DataContext(VIM.VIMRepository.oosConnectionString);
            //IEnumerable<VimTeam> results =
            //    dc.ExecuteQuery<VimTeam>(querySQLServer, new object[] { });
            //return results;
        }

    }
}