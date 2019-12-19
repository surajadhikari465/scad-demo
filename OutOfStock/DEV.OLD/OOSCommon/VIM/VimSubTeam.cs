using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Linq;
using System.Linq;

namespace OOSCommon.VIM
{
    public class VimSubTeam
    {
        public string subteam_name { get; set; }

        public static IEnumerable<VimSubTeam> RunQuery(string region, List<string> storeNumbers, List<string> teams)
        {
            string queryOracle = VimSubTeam.MakeOracleQuery(region, storeNumbers, teams);
            string querySQLServer =
                // The SQL Server part that needs to match this class
                "SELECT subteam_name " +
                "FROM OPENQUERY(" + VIM.VIMRepository.oosVIMServiceName + ", '" +
                // The Oracle part ... remember to escape single quotes
                    queryOracle.Replace("'", "''") +
                "')";
            System.Data.Linq.DataContext dc =
                new System.Data.Linq.DataContext(VIM.VIMRepository.oosConnectionString);
            IEnumerable<VimSubTeam> results =
                dc.ExecuteQuery<VimSubTeam>(querySQLServer, new object[] { });
            return results;
        }

        protected static string MakeOracleQuery(string region, List<string> storeNumbers, List<string> teams)
        {
            string queryOracle =
                "SELECT DISTINCT subteam_name FROM vim.crs_team_ref " +
                "WHERE LENGTH(subteam_name) > 1 ";
            if (!string.IsNullOrWhiteSpace(region))
                queryOracle += "AND region='" + region + "' ";
            if (storeNumbers != null && storeNumbers.Count > 0)
            {
                string storeList = string.Empty;
                foreach (string item in storeNumbers)
                {
                    if (storeList.Length > 0)
                        storeList += ",";
                    storeList += item;
                }
                queryOracle += "AND ps_bu IN (" + storeList + ") ";
            }
            if (teams != null && teams.Count > 0)
            {
                string teamList = string.Empty;
                foreach (string item in teams)
                {
                    if (teamList.Length > 0)
                        teamList += ",";
                    teamList += "'" + item + "'"; 
                }
                queryOracle += "AND team_name IN (" + teamList + ") ";
            }
            queryOracle += "ORDER BY subteam_name";
            return queryOracle;
        }

    }
}