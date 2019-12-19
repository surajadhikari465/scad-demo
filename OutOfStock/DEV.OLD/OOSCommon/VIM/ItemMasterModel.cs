using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Linq;
using System.Linq;
using System.Text;

namespace OOSCommon.VIM
{
    public class ItemMasterModel
    {
        public string NAT_UPC { get; set; }
        public string BRAND { get; set; }
        public string BRAND_NAME { get; set; }
        public string LONG_DESCRIPTION { get; set; }
        public string ITEM_SIZE { get; set; }
        public string ITEM_UOM { get; set; }
        public string CATEGORY_NAME { get; set; }
        public string CLASS_NAME { get; set; }

        public const string queryOracleBase =
            "SELECT NAT_UPC, BRAND, BRAND_NAME, LONG_DESCRIPTION, ITEM_SIZE, ITEM_UOM, CATEGORY_NAME, CLASS_NAME " +
            "FROM ( " +
            "SELECT im.nat_upc AS NAT_UPC, im.brand AS BRAND, b.brand_name, " +
            "im.long_description AS LONG_DESCRIPTION,  im.ITEM_SIZE AS ITEM_SIZE, " +
            "im.ITEM_UOM AS ITEM_UOM, h2.hier_full_name AS CATEGORY_NAME, " +
            "h4.hier_full_name AS CLASS_NAME " +
            "FROM vim.hierarchy h1, vim.hierarchy h2, vim.hierarchy h3, vim.hierarchy h4,  vim.item_master im, brand b " +
            "WHERE h1.HIER_LVL_ID = 10 AND h2.HIER_PARENT = h1.hierarchy_ref " +
            "AND h2.HIER_LVL_ID = 20  AND h3.HIER_PARENT = h2.hierarchy_ref " +
            "AND h3.HIER_LVL_ID = 30 AND h4.HIER_PARENT = h3.hierarchy_ref " +
            "AND h4.HIER_LVL_ID = 40 AND im.HIERARCHY_REF = h4.HIERARCHY_REF " +
            "AND im.brand = b.brand " + 
            "AND im.nat_upc {0} )";
        public static string querySQLServerBase =
            "SELECT NAT_UPC, BRAND, BRAND_NAME, LONG_DESCRIPTION, ITEM_SIZE, ITEM_UOM, CATEGORY_NAME, CLASS_NAME " +
            "FROM OPENQUERY(" + OOSCommon.VIM.VIMRepository.oosVIMServiceName + ", '{0}')";

        public ItemMasterModel()
        {
        }

        public ItemMasterModel(string NAT_UPC, string BRAND, string LONG_DESCRIPTION,
            string ITEM_SIZE, string ITEM_UOM, string CATEGORY_NAME, string CLASS_NAME)
        {
            this.NAT_UPC = NAT_UPC;
            this.BRAND = BRAND;
            this.LONG_DESCRIPTION = LONG_DESCRIPTION;
            this.ITEM_SIZE = ITEM_SIZE;
            this.ITEM_UOM = ITEM_UOM;
            this.CATEGORY_NAME = CATEGORY_NAME;
            this.CLASS_NAME = CLASS_NAME;
        }

        public ItemMasterModel(string NAT_UPC, string BRAND, string BRAND_NAME, string LONG_DESCRIPTION,
          string ITEM_SIZE, string ITEM_UOM, string CATEGORY_NAME, string CLASS_NAME)
        {
            this.NAT_UPC = NAT_UPC;
            this.BRAND = BRAND;
            this.BRAND_NAME = BRAND_NAME;
            this.LONG_DESCRIPTION = LONG_DESCRIPTION;
            this.ITEM_SIZE = ITEM_SIZE;
            this.ITEM_UOM = ITEM_UOM;
            this.CATEGORY_NAME = CATEGORY_NAME;
            this.CLASS_NAME = CLASS_NAME;
        }

        public static Dictionary<string,ItemMasterModel> RunQuery(IEnumerable<string> upcs)
        {

            /* 
             * 
             */
            var results = new Dictionary<string, ItemMasterModel>();

            var csvUPCs = new UpcSet(6000)                  // set length of comma separated string of upc's to 6000 max.
                                .ToPureSqlStrings(upcs);    // create comma separated string from input UPC's
            
            // Grab records in potentially several steps)
            foreach (string csvUPC in csvUPCs)
            {
                string queryOracle = string.Format(queryOracleBase, "IN (" + csvUPC + ")");
                // The SQL Server query with the Oracle query within
                var query = string.Format(querySQLServerBase, queryOracle.Replace("'", "''"));
                var dc = new System.Data.Linq.DataContext(VIMRepository.oosConnectionString);
                var resultInner = dc.ExecuteQuery<ItemMasterModel>(query, new object[] { });
                // Add to results
                foreach (var item in resultInner.Where(item => !results.ContainsKey(item.NAT_UPC)))
                {
                    results.Add(item.NAT_UPC, item);
                }
            }
            return results;
        }

        public static ItemMasterModel RunQuery(string upc)
        {
            ItemMasterModel result = new ItemMasterModel();
            if (!string.IsNullOrWhiteSpace(upc))
            {
                string queryOracle = string.Format(queryOracleBase, "='" + upc + "' ");
                // The SQL Server query with the Oracle query within
                string query = string.Format(querySQLServerBase, queryOracle.Replace("'", "''"));
                System.Data.Linq.DataContext dc =
                    new System.Data.Linq.DataContext(OOSCommon.VIM.VIMRepository.oosConnectionString);
                IEnumerable<ItemMasterModel> resultInner =
                    dc.ExecuteQuery<ItemMasterModel>(query, new object[] { });
                if (resultInner != null)
                    result = resultInner.FirstOrDefault();
            }
            return result;
        }

        public static IEnumerable<ItemMasterModel> RunQuery()
        {
            List<ItemMasterModel> results = new List<ItemMasterModel>();
//   select nat_upc, long_description, item_size, item_uom,
//   brand, timestamp, family_name, category_name, sub_cat_name, class_name 
//   from (
//       select im.nat_upc, im.long_description, im.ITEM_SIZE, im.ITEM_UOM,
//       im.brand, to_char(im.TIMESTAMP, 'YYYY-MM-DD HH24:MI:SS') timestamp, 
//       h1.hierarchy_ref family_hr, h1.hier_full_name family_name,
//       h2.hierarchy_ref category_hr, h2.hier_full_name category_name,
//       h3.hierarchy_ref sub_cat_hr, h3.hier_full_name sub_cat_name,
//       h4.hierarchy_ref class_hr, h4.hier_full_name class_name
//       from vim.hierarchy h1, vim.hierarchy h2, 
//       vim.hierarchy h3, vim.hierarchy h4,
//      vim.item_master im
//       where h1.HIER_LVL_ID = 10
//       and h2.HIER_PARENT = h1.hierarchy_ref
//       and h2.HIER_LVL_ID = 20
//       and h3.HIER_PARENT = h2.hierarchy_ref
//       and h3.HIER_LVL_ID = 30
//       and h4.HIER_PARENT = h3.hierarchy_ref
//       and h4.HIER_LVL_ID = 40
//       and im.HIERARCHY_REF = h4.HIERARCHY_REF
//       --and im.nat_upc = '0000000002836'
//        )
            return results;
        }

    }
}
