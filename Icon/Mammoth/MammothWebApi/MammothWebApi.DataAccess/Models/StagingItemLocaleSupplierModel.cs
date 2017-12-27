using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MammothWebApi.DataAccess.Models
{
    public class StagingItemLocaleSupplierModel
    {
        public string Region { get; set; }
        public string ScanCode { get; set; }
        public int BusinessUnitID { get; set; }
        public string SupplierName { get; set; }
        public string SupplierItemId { get; set; }
        public decimal? SupplierCaseSize { get; set; }
        public string IrmaVendorKey { get; set; }
        public DateTime Timestamp { get; set; }
        public Guid TransactionId { get; set; }
    }
}
