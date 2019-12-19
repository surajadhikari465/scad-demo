using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Linq;
using System.Data.Linq.SqlClient;
using System.Linq;
using System.Text;

namespace OOSCommon.VIM
{
    public class VIMOOSItemDataView
    {
        public string VENDOR_KEY { get; set; }
        public string VEND_ITEM_NUM { get; set; }
        public string PS_DEPT_TEAM { get; set; }
        public string PS_PROD_SUBTEAM { get; set; }
        public string TEAM_NAME { get; set; }
        public string SUBTEAM_NAME { get; set; }
        public string UPC { get; set; }
        public string PS_BU { get; set; }
        public string V_AUTH_STATUS { get; set; }
        public decimal? EFF_COST { get; set; }
        public decimal? CASE_SIZE { get; set; }
        public string EFF_PRICETYPE { get; set; }
        public decimal? EFF_PRICE { get; set; }

        public static IEnumerable<VIMOOSItemDataView> GetVIMOOSItemDataView(string upc, string ps_bu)
        {
            // The VIM view returns either a decimal or an int.  This causes problems
            // with the LINQ name/type binding. To reconcile this  we cast it to a float 
            // in the Oracle query. We cast it back to decimal in the SQL Server query.
            string queryOracle =
                "SELECT  iv.vendor_key VENDOR_KEY,vsi.VEND_ITEM_NUM VEND_ITEM_NUM, " +
	            "crs.PS_DEPT_TEAM PS_DEPT_TEAM, crs.PS_PROD_SUBTEAM PS_PROD_SUBTEAM, " +
	            "crs.TEAM_NAME TEAM_NAME,crs.SUBTEAM_NAME SUBTEAM_NAME, " +
	            "ir.upc UPC,vsi.ps_bu PS_BU,vsi.V_AUTH_STATUS V_AUTH_STATUS, " +
	            "vc.eff_cost EFF_COST,vc.CASE_SIZE CASE_SIZE,rp.EFF_PRICETYPE EFF_PRICETYPE, " +
                "TO_BINARY_FLOAT(" +
	                "rp.EFF_PRICE / DECODE(rp.EFF_MULTIPLE,  NULL, 1,  0, 1, rp.EFF_MULTIPLE) " +
                ") AS EFF_PRICE_FLOAT " +
                "FROM vim.ir_vendor iv,vim.CRS_TEAM_REF crs,vim.VENDOR_STORE_ITEM vsi, " +
	            "vim.item_region ir,vim.retail_price rp,vim.vendor_Cost vc " +
                "WHERE ir.upc = '" + upc + "' AND vsi.ps_bu = '" + ps_bu + "' " +
	            "AND iv.region = vsi.region AND iv.region = crs.region " +
	            "AND iv.region = ir.region AND iv.region = rp.region " +
	            "AND iv.region = vc.region AND vsi.ps_bu = crs.PS_BU " +
	            "AND vsi.ps_bu = DECODE(rp.ps_bu, '0', vsi.ps_bu, rp.ps_bu) " +
	            "AND vsi.ps_bu = DECODE(vc.ps_bu, '0', vsi.ps_bu, vc.ps_bu) " +
	            "AND vsi.REG_VEND_NUM_CZ = iv.VENDOR_ID " +
	            "AND vsi.reg_vend_num_cz = vc.reg_vend_num_cz " +
	            "AND crs.POS_DEPT = ir.POS_DEPT AND LENGTH(crs.TEAM_NAME) > 1 " +
	            "AND ir.upc = vsi.upc AND ir.upc = rp.upc AND ir.upc = vc.upc " +
                "AND vsi.VEND_ITEM_NUM = vc.VEND_ITEM_NUM " +
                "ORDER BY vsi.V_AUTH_STATUS DESC";
            // A query in code to access Linked server 
            string querySQLServer =
                // The SQL  erver part that needs to match this class
                "SELECT VENDOR_KEY,VEND_ITEM_NUM,PS_DEPT_TEAM,PS_PROD_SUBTEAM,TEAM_NAME,SUBTEAM_NAME,UPC,PS_BU,V_AUTH_STATUS " +
                ",EFF_COST,CASE_SIZE,EFF_PRICETYPE,CAST(EFF_PRICE_FLOAT AS decimal(9,3)) AS EFF_PRICE " +
                "FROM OPENQUERY(" + VIM.VIMRepository.oosVIMServiceName + ", '" +
                // The Oracle part ... remember to escape single quotes
                    queryOracle.Replace("'", "''") +
                "')";
            System.Data.Linq.DataContext dc = 
                new System.Data.Linq.DataContext(VIM.VIMRepository.oosConnectionString);
            IEnumerable<VIMOOSItemDataView> results = new List<VIMOOSItemDataView>();
            results = dc.ExecuteQuery<VIMOOSItemDataView>(querySQLServer, new object[] { });
            return results;
        }

        public static Dictionary<string, List<VIMOOSItemDataView>> GetVIMOOSItemDataView(List<string> upcs, string ps_bu)
        {
            Dictionary<string, List<VIMOOSItemDataView>> result = new Dictionary<string, List<VIMOOSItemDataView>>();
            // The VIM view returns either a decimal or an int.  This causes problems
            // with the LINQ name/type binding. To reconcile this  we cast it to a float 
            // in the Oracle query. We cast it back to decimal in the SQL Server query.
            string queryOracle =
                "SELECT  iv.vendor_key VENDOR_KEY,vsi.VEND_ITEM_NUM VEND_ITEM_NUM, " +
                "crs.PS_DEPT_TEAM PS_DEPT_TEAM, crs.PS_PROD_SUBTEAM PS_PROD_SUBTEAM, " +
                "crs.TEAM_NAME TEAM_NAME,crs.SUBTEAM_NAME SUBTEAM_NAME, " +
                "ir.upc UPC,vsi.ps_bu PS_BU,vsi.V_AUTH_STATUS V_AUTH_STATUS, " +
                "vc.eff_cost EFF_COST,vc.CASE_SIZE CASE_SIZE,rp.EFF_PRICETYPE EFF_PRICETYPE, " +
                "TO_BINARY_FLOAT(" +
                    "rp.EFF_PRICE / DECODE(rp.EFF_MULTIPLE,  NULL, 1,  0, 1, rp.EFF_MULTIPLE) " +
                ") AS EFF_PRICE_FLOAT " +
                "FROM vim.ir_vendor iv,vim.CRS_TEAM_REF crs,vim.VENDOR_STORE_ITEM vsi, " +
                "vim.item_region ir,vim.retail_price rp,vim.vendor_Cost vc " +
                "WHERE ir.upc in ({0}) AND vsi.ps_bu = '" + ps_bu + "' " +
                "AND iv.region = vsi.region AND iv.region = crs.region " +
                "AND iv.region = ir.region AND iv.region = rp.region " +
                "AND iv.region = vc.region AND vsi.ps_bu = crs.PS_BU " +
                "AND vsi.ps_bu = DECODE(rp.ps_bu, '0', vsi.ps_bu, rp.ps_bu) " +
                "AND vsi.ps_bu = DECODE(vc.ps_bu, '0', vsi.ps_bu, vc.ps_bu) " +
                "AND vsi.REG_VEND_NUM_CZ = iv.VENDOR_ID " +
                "AND vsi.reg_vend_num_cz = vc.reg_vend_num_cz " +
                "AND crs.POS_DEPT = ir.POS_DEPT AND LENGTH(crs.TEAM_NAME) > 1 " +
                "AND ir.upc = vsi.upc AND ir.upc = rp.upc AND ir.upc = vc.upc " +
                "AND vsi.VEND_ITEM_NUM = vc.VEND_ITEM_NUM " +
                "ORDER BY vsi.V_AUTH_STATUS DESC";
            System.Data.Linq.DataContext dc =
                new System.Data.Linq.DataContext(VIM.VIMRepository.oosConnectionString);
            string upcList = string.Empty;
            int flushLength = 8000 - queryOracle.Length - 500;
            for (int ix = 0; ix < upcs.Count; ++ix)
            {
                if (upcList.Length > 0)
                    upcList += ",";
                upcList += "'" + upcs[ix] + "'";
                if ((ix + 1) >= upcs.Count || upcList.Length >= flushLength)
                {
                    string queryOracleInner = string.Format(queryOracle, upcList);
                    upcList = string.Empty;
                    // A query in code to access Linked server 
                    string querySQLServer =
                        // The SQL  erver part that needs to match this class
                        "SELECT VENDOR_KEY,VEND_ITEM_NUM,PS_DEPT_TEAM,PS_PROD_SUBTEAM,TEAM_NAME,SUBTEAM_NAME,UPC,PS_BU,V_AUTH_STATUS " +
                        ",EFF_COST,CASE_SIZE,EFF_PRICETYPE,CAST(EFF_PRICE_FLOAT AS decimal(9,3)) AS EFF_PRICE " +
                        "FROM OPENQUERY(" + VIM.VIMRepository.oosVIMServiceName + ", '" +
                        // The Oracle part ... remember to escape single quotes
                            queryOracleInner.Replace("'", "''") +
                        "')";
                    IEnumerable<VIMOOSItemDataView> resultInner =
                        dc.ExecuteQuery<VIMOOSItemDataView>(querySQLServer, new object[] { });
                    foreach (VIMOOSItemDataView item in resultInner)
                    {
                        if (result.Keys.Contains(item.UPC))
                            result[item.UPC].Add(item);
                        else
                            result.Add(item.UPC, new List<VIMOOSItemDataView>(){item});
                    }
                }
            }
            return result;
        }

    }
}
