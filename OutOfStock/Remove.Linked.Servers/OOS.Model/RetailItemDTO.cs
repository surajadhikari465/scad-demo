using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOS.Model
{
    public class RetailItemDTO
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
    }
}
