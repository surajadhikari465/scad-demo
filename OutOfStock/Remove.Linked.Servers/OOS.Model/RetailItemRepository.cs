using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using OOSCommon;
using OOSCommon.DataAccess;
using OOSCommon.VIM;
using SharedKernel;

namespace OOS.Model.Repository
{
    public class RetailItemRepository : IRetailItemRepository
    {
        private readonly IStoreRepository _storeRepository;
        private IConfigurator _config;
        private readonly IProductRepository _productRepository;

        public RetailItemRepository(IConfigurator config, IStoreRepository storeRepository, IProductRepository productRepository)
        {
            this._config = config;
            this._storeRepository = storeRepository;
            this._productRepository = productRepository;
        }

        public List<IRetailItem> For(IEnumerable<string> upcsQueried, string storeAbbrev)
        {
            // bug oracle dependency [fixed]
            var upcs = upcsQueried.Distinct().ToList();
            var products = _productRepository.For(upcs);
            var productMap = products.ToDictionary(p => p.UPC.Code, q => q);

            var result = new Dictionary<string, IRetailItem>();
            var psBu = _storeRepository.ForAbbrev(storeAbbrev);
            string queryOracle =
                "SELECT  iv.vendor_key VENDOR_KEY,vsi.VEND_ITEM_NUM VEND_ITEM_NUM, " +
                "crs.PS_DEPT_TEAM PS_DEPT_TEAM, crs.PS_PROD_SUBTEAM PS_PROD_SUBTEAM, " +
                "crs.TEAM_NAME TEAM_NAME,crs.SUBTEAM_NAME SUBTEAM_NAME, " +
                "ir.upc UPC,vsi.ps_bu PS_BU,vsi.V_AUTH_STATUS V_AUTH_STATUS, " +
                "vc.eff_cost EFF_COST,vc.CASE_SIZE CASE_SIZE,rp.EFF_PRICETYPE EFF_PRICETYPE, " +
                "TO_BINARY_FLOAT(" +
                    "rp.EFF_PRICE / DECODE(rp.EFF_MULTIPLE,  NULL, 1,  0, 1, rp.EFF_MULTIPLE) " +
                ") AS EFF_PRICE " +
                "FROM vim.ir_vendor iv,vim.CRS_TEAM_REF crs,vim.VENDOR_STORE_ITEM vsi, " +
                "vim.item_region ir,vim.retail_price rp,vim.vendor_Cost vc " +
                "WHERE ir.upc in ({0}) AND vsi.ps_bu = '" + psBu.BusinessUnitNo + "' " +
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
            var upcList = string.Empty;
            var flushLength = 8000 - queryOracle.Length - 500;
            for (var ix = 0; ix < upcs.Count; ++ix)
            {
                if (upcList.Length > 0) upcList += ",";
                upcList += $"'{upcs[ix]}'";

                if (ix + 1 < upcs.Count && upcList.Length < flushLength) continue;

                var queryOracleInner = string.Format(queryOracle, upcList);
                upcList = string.Empty;
                // A query in code to access Linked server 

                //string querySQLServer =
                //    // The SQL  erver part that needs to match this class
                //    "SELECT VENDOR_KEY,VEND_ITEM_NUM,PS_DEPT_TEAM,PS_PROD_SUBTEAM,TEAM_NAME,SUBTEAM_NAME,UPC,PS_BU,V_AUTH_STATUS " +
                //    ",EFF_COST,CASE_SIZE,EFF_PRICETYPE,CAST(EFF_PRICE_FLOAT AS decimal(9,3)) AS EFF_PRICE " +
                //    "FROM OPENQUERY(" + config.GetVIMServiceName() + ", '" +
                //    // The Oracle part ... remember to escape single quotes
                //    queryOracleInner.Replace("'", "''") +
                //    "')";

                OracleDataAccess oracle;
                IEnumerable<RetailItemDTO> resultInner;
                using (oracle = new OracleDataAccess(AppConfig.ConnectionStrings["VIM"].ConnectionString))
                {
                    resultInner = oracle.ReturnList<RetailItemDTO>(CommandType.Text, queryOracleInner);
                }

                foreach (var item in resultInner)
                {
                    var upc = item.UPC;
                    if (result.ContainsKey(upc) || !productMap.ContainsKey(upc)) continue;

                    var vendorKey = item.VENDOR_KEY;
                    var vin = item.VEND_ITEM_NUM;
                    var teamName = item.TEAM_NAME;
                    var subTeamName = item.SUBTEAM_NAME.Replace(",", ""); // remove commas. "Tea, Coffee, and Housewares" breaks things
                    var priceType = item.EFF_PRICETYPE;
                    var businessUnit = item.PS_BU;
                    var caseSize = item.CASE_SIZE.HasValue ? Convert.ToDecimal(item.CASE_SIZE) : 0;
                    var cost = item.EFF_COST.HasValue ? Convert.ToDouble(item.EFF_COST.Value) : 0;
                    var price = item.EFF_PRICE.HasValue ? Convert.ToDouble(item.EFF_PRICE) : 0;
                    var retailItem = new RetailItem(productMap[upc])
                    {
                        VendorKey = vendorKey,
                        VendorItemNumber = vin,
                        TeamName = teamName,
                        SubTeamName = subTeamName,
                        PriceType = priceType,
                        PepoleSoftBusinessUnit = businessUnit,
                        CaseSize = caseSize,
                        Cost = cost,
                        Price = price
                    };
                    result.Add(item.UPC, retailItem);
                }
                //using (var dc = new System.Data.Linq.DataContext(config.GetOOSConnectionString()))
                //{
                //    dc.CommandTimeout = 600;
                //    IEnumerable<RetailItemDTO> resultInner = dc.ExecuteQuery<RetailItemDTO>(querySQLServer, new object[] { });
                    
                //}
            }
            return result.Values.ToList();
        }

        public IRetailItem For(string upc, string storeAbbrev)
        {
            var upcs = new List<string> {upc};
            return For(upcs, storeAbbrev).FirstOrDefault();
        }
    }
}
