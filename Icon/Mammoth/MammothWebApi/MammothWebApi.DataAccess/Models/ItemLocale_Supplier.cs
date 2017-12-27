using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MammothWebApi.DataAccess.Models
{
    public partial class ItemLocale_Supplier
    {
        public string Region { get; set; }
        public int ItemLocaleSupplierID { get; set; }
        public int ItemID { get; set; }
        public int BusinessUnitID { get; set; }
        public string SupplierName { get; set; }
        public string SupplierItemID { get; set; }
        public decimal? SupplierCaseSize { get; set; }
        public string IrmaVendorKey { get; set; }
        public DateTime AddedDateUtc { get; set; }
        public DateTime? ModifiedDateUtc { get; set; }
    }
}
