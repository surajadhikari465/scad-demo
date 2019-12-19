using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using OOSCommon;
using SharedKernel;

namespace OOS.Model.Repository
{
    public class ProductRepository : IProductRepository
    {
        private IOOSLog logger;
        private IConfigurator config;
        private IProductFactory productFactory;

        private const string SELECT_MASTER_JOIN_HIER_ORACLE_SQL = 
                "SELECT NAT_UPC as UPC, BRAND, LONG_DESCRIPTION, ITEM_SIZE, ITEM_UOM, CATEGORY_NAME, CLASS_NAME " +
                "FROM (SELECT im.nat_upc AS NAT_UPC, im.brand AS BRAND, " +
                "im.long_description AS LONG_DESCRIPTION,  im.ITEM_SIZE AS ITEM_SIZE, " +
                "im.ITEM_UOM AS ITEM_UOM, h2.hier_full_name AS CATEGORY_NAME, " +
                "h4.hier_full_name AS CLASS_NAME " +
                "FROM vim.hierarchy h1, vim.hierarchy h2, vim.hierarchy h3, vim.hierarchy h4,  vim.item_master im " +
                "WHERE h1.HIER_LVL_ID = 10 AND h2.HIER_PARENT = h1.hierarchy_ref " +
                "AND h2.HIER_LVL_ID = 20  AND h3.HIER_PARENT = h2.hierarchy_ref " +
                "AND h3.HIER_LVL_ID = 30 AND h4.HIER_PARENT = h3.hierarchy_ref " +
                "AND h4.HIER_LVL_ID = 40 AND im.HIERARCHY_REF = h4.HIERARCHY_REF " +
                "AND im.nat_upc IN ( {0} ))";
        
        private const string SELECT_REGION_JOIN_HIER_ORACLE_SQL = 
                "SELECT im.upc AS UPC, im.brand AS BRAND, im.long_description AS LONG_DESCRIPTION, im.ITEM_SIZE AS ITEM_SIZE, " +
                "im.ITEM_UOM AS ITEM_UOM, h1.hier_full_name AS CATEGORY_NAME, h2.hier_full_name AS CLASS_NAME " +
                "FROM vim.item_region im LEFT join vim.reg_hierarchy h1 on im.reg_hier_ref = h1.reg_hier_ref " +
                "LEFT join vim.reg_hierarchy h2 on h1.hier_parent = h2.reg_hier_ref " +
                "LEFT join vim.reg_hierarchy h3 on h2.hier_parent = h3.reg_hier_ref " +
                "WHERE im.upc IN ( {0} )";

        private const string SELECT_REGION_PRODUCT_INFO_ORACLE_SQL = 
            "SELECT im.upc AS UPC, im.brand AS BRAND, br.brand_name as BRAND_NAME, im.long_description AS LONG_DESCRIPTION, " +
            "im.ITEM_SIZE AS ITEM_SIZE, im.ITEM_UOM AS ITEM_UOM, h1.hier_full_name AS CATEGORY_NAME, h2.hier_full_name AS CLASS_NAME " +
            "FROM vim.item_region im " +
            "LEFT join vim.Brand br on UPPER(trim(im.brand)) = UPPER(trim(br.brand)) " + 
            "LEFT join vim.reg_hierarchy h1 on im.reg_hier_ref = h1.reg_hier_ref " +
            "LEFT join vim.reg_hierarchy h2 on h1.hier_parent = h2.reg_hier_ref " +
            "LEFT join vim.reg_hierarchy h3 on h2.hier_parent = h3.reg_hier_ref " +
            "WHERE im.upc IN ({0}) " +
            "ORDER by im.UPC";

        private const string SELECT_MASTER_PRODUCT_INFO_ORACLE_SQL = 
            "SELECT im.nat_upc AS UPC, im.brand AS BRAND, br.brand_name as BRAND_NAME, " +
            "im.long_description AS LONG_DESCRIPTION,  im.ITEM_SIZE AS ITEM_SIZE, " +
            "im.ITEM_UOM AS ITEM_UOM, h2.hier_full_name AS CATEGORY_NAME, " +
            "h4.hier_full_name AS CLASS_NAME " +
            "FROM vim.hierarchy h1, vim.hierarchy h2, vim.hierarchy h3, vim.hierarchy h4,  vim.item_master im " +
            "LEFT join vim.Brand br on UPPER(trim(im.brand)) = UPPER(trim(br.brand)) " +
            "WHERE h1.HIER_LVL_ID = 10 AND h2.HIER_PARENT = h1.hierarchy_ref " +
            "AND h2.HIER_LVL_ID = 20  AND h3.HIER_PARENT = h2.hierarchy_ref " +
            "AND h3.HIER_LVL_ID = 30 AND h4.HIER_PARENT = h3.hierarchy_ref " +
            "AND h4.HIER_LVL_ID = 40 AND im.HIERARCHY_REF = h4.HIERARCHY_REF " +
            "AND im.nat_upc IN ({0})";

        private const string SELECT_PRODUCT_INFO_LINKED_SQL = "";

        public ProductRepository(IConfigurator config, ILogService logService, IProductFactory productFactory)
        {
            this.config = config;
            this.logger = logService.GetLogger();
            this.productFactory = productFactory;
        }

        public IEnumerable<IProduct> For(IEnumerable<string> upcs)
        {
            var upcList = upcs.ToList();

            //logger.Trace(string.Format("For() Enter, UPCs={0}", upcList.Count));
            var dtStart = DateTime.Now;

            var fromMaster = QueryMaster(upcList).ToList();
            var upcsMaster = fromMaster.Select(p => p.UPC).ToList();
            //logger.Trace(string.Format("UPCs from Master={0}", upcsMaster.Count));

            var notInMaster = upcList.Where(p => !upcsMaster.Contains(p)).ToList();
            //logger.Trace(string.Format("UPCs NOT in Master={0}", notInMaster.Count));
            
            var fromRegion = QueryRegion(notInMaster).ToList();
            //logger.Trace(string.Format("UPCs from Region={0}", fromRegion.Count));

            var union = fromMaster.Union(fromRegion).ToList();
            var result = ReconstituteProduct(upcList, union);

            TimeSpan ts = DateTime.Now.Subtract(dtStart);
            logger.Debug(string.Format("UPCs={0} In Master={1} Not In Master={2} From Region={3} Elapsed={4}", upcList.Count, upcsMaster.Count, notInMaster.Count, fromRegion.Count, ts));
            //logger.Trace("For() Exit, Elapsed=" + ts);
            return result;
        }

        private IEnumerable<ProductDTO> QueryMaster(IEnumerable<string> upcs)
        {
            //return Query(SELECT_MASTER_JOIN_HIER_ORACLE_SQL, upcs);
            return Query(SELECT_MASTER_PRODUCT_INFO_ORACLE_SQL, upcs);
        }

        private IEnumerable<ProductDTO> Query(string sql, IEnumerable<string> upcs)
        {
            // Queries must be less than 8000 characters so there may be several
            var segments = new UpcSet(6000).ToPureSqlStrings(upcs);
            return segments.Select(segment => ProductSelectLinkedSQL(sql, segment)).Select(Execute).SelectMany(p => p).ToList();
        }

        private string ProductSelectLinkedSQL(string sql, string upcSet)
        {
            var oracleQuery = string.Format(sql, upcSet).Replace("'", "''");
            const string linkedServerQuery =
                "SELECT UPC, BRAND, BRAND_NAME, LONG_DESCRIPTION, ITEM_SIZE, ITEM_UOM, CATEGORY_NAME, CLASS_NAME from OPENQUERY({0}, '{1}')";
            return string.Format(linkedServerQuery, config.GetVIMServiceName(), oracleQuery);
            
            //return new StringBuilder("SELECT UPC, BRAND, BRAND_NAME, LONG_DESCRIPTION, ITEM_SIZE, ITEM_UOM, CATEGORY_NAME, CLASS_NAME ")
            //    .Append("from OPENQUERY(").Append(config.GetVIMServiceName())
            //    .Append(", '").Append(oracleQuery).Append("')").ToString();
        }

        private IEnumerable<ProductDTO> QueryRegion(IEnumerable<string> upcs)
        {
            //return Query(SELECT_REGION_JOIN_HIER_ORACLE_SQL, upcs);
            return Query(SELECT_REGION_PRODUCT_INFO_ORACLE_SQL, upcs);
        }

        private IEnumerable<ProductDTO> Execute(string sqlQuery)
        {
            using (var oosDataContext = new System.Data.Linq.DataContext(config.GetOOSConnectionString()))
            {
                var items = oosDataContext.ExecuteQuery<ProductDTO>(sqlQuery, new object[] { });
                var products = items.ToList();
                return products;
            }
        }

        private IEnumerable<IProduct> ReconstituteProduct(IEnumerable<string> upcs, IEnumerable<ProductDTO> products)
        {
            return productFactory.Reconstitute(upcs, products);
        }

        private IEnumerable<IProduct> RemoveDuplicates(IEnumerable<IProduct> products)
        {
            var productListMap = new Dictionary<string, List<IProduct>>();
            foreach (var product in products)
            {
                if (productListMap.ContainsKey(product.UPC.Code))
                {
                    productListMap[product.UPC.Code].Add(product);
                }
                else
                {
                    productListMap.Add(product.UPC.Code, new List<IProduct> { product });
                }
            }
            return productListMap.Values.Select(p => p.First());

        }

        public IProduct For(string upc)
        {
            var upcs = new List<string> { upc };
            var product = For(upcs);
            return product.FirstOrDefault();
        }

    }
}
