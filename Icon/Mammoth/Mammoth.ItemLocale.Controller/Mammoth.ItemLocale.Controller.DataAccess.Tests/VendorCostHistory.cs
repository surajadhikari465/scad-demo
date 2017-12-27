using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mammoth.ItemLocale.Controller.DataAccess.Tests
{
    public class VendorCostHistory
    {
        public int VendorCostHistoryID { get; set; }
        public int StoreItemVendorID { get; set; }
        public bool Promotional { get; set; }
        public decimal UnitCost { get; set; }
        public decimal UnitFreight { get; set; }
        public decimal Package_Desc1 { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool FromVendor { get; set; }
        public decimal MSRP { get; set; }
        public DateTime InsertDate { get; set; }
        public int CostUnit_ID { get; set; }
        public int FreightUnit_ID { get; set; }
    }
}
