using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Linq;
using System.Linq;

namespace OOSCommon.VIM
{
    public class VimUPC
    {
        public string UPC { get; set; }

        public static string GetVimUPC(string upc, string userRegion)
        {
            string queryOracle = "SELECT DISTINCT upc FROM vim.Item_Region";
            if (!string.IsNullOrWhiteSpace(upc) || !string.IsNullOrWhiteSpace(userRegion))
            {
                queryOracle += " WHERE ";
                if (!string.IsNullOrWhiteSpace(upc))
                    queryOracle += "upc='" + upc + "' ";
                if (!string.IsNullOrWhiteSpace(userRegion))
                    queryOracle += "region='" + userRegion + "' ";
            }
            // A query in code to access Linked server 
            var querySQLServer =
                // The SQL  erver part that needs to match this class
                "SELECT upc " +
                "FROM OPENQUERY(" + VIM.VIMRepository.oosVIMServiceName + ", '" +
                // The Oracle part ... remember to escape single quotes
                queryOracle.Replace("'", "''") +
            "')";
            var dc =
                new System.Data.Linq.DataContext(VIM.VIMRepository.oosConnectionString);
            var results =
                dc.ExecuteQuery<VimUPC>(querySQLServer, new object[] { }).FirstOrDefault();
            return results == null ? null : results.UPC;
        }

        public static HashSet<string> GetVimUPC(List<string> upcs, string userRegion)
        {
            HashSet<string> result = new HashSet<string>();
            string oracleQueryTemplate = "SELECT DISTINCT upc FROM vim.Item_Region WHERE ";
            if (!string.IsNullOrWhiteSpace(userRegion))
                oracleQueryTemplate += "region='" + userRegion + "' AND upc IN ({0})";
            System.Data.Linq.DataContext dc =
                new System.Data.Linq.DataContext(VIM.VIMRepository.oosConnectionString);
            string upcList = string.Empty;
            for (int ix = 0; ix < upcs.Count; ++ix)
            {
                if (!string.IsNullOrWhiteSpace(upcs[ix]))
                {
                    if (upcList.Length > 0)
                        upcList += ",";
                    upcList += upcs[ix];
                    if (upcList.Length > 4000 || ix >= (upcs.Count - 1))
                    {
                        string oracleQuery = string.Format(oracleQueryTemplate, upcList);
                        // A query in code to access Linked server 
                        string querySQLServer =
                            // The SQL  erver part that needs to match this class
                            "SELECT upc " +
                            "FROM OPENQUERY(" + VIM.VIMRepository.oosVIMServiceName + ", '" +
                            // The Oracle part ... remember to escape single quotes
                            oracleQuery.Replace("'", "''") +
                        "')";
                        IEnumerable<VimUPC> resultsInner =
                            dc.ExecuteQuery<VimUPC>(querySQLServer, new object[] { });
                        foreach (VimUPC item in resultsInner)
                        {
                            if (!result.Contains(item.UPC))
                                result.Add(item.UPC);
                        }
                    }
                }
            }
            return result;
        }

    }
}