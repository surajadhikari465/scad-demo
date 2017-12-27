using MammothWebApi.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MammothWebApi.Tests.DataAccess.ModelBuilders
{
    public class TestStagingItemLocaleSupplierModelBuilder
    {
        private string region;
        private int businessUnit;
        private string scanCode;
        public string supplierName;
        public string supplierItemId;
        public decimal? supplierCaseSize;
        public string irmaVendorKey;
        private DateTime timestamp;
        private Guid transactionId;

        internal TestStagingItemLocaleSupplierModelBuilder()
        {
            region = "SW";
            businessUnit = 1;
            scanCode = "77777777771";
            supplierName = "Test Supplier";
            supplierItemId = "Test Item ID";
            supplierCaseSize = 1;
            irmaVendorKey = "Test Key";
            timestamp = DateTime.Now;
            transactionId = Guid.NewGuid();
        }

        internal TestStagingItemLocaleSupplierModelBuilder WithRegion(string region)
        {
            this.region = region;
            return this;
        }

        internal TestStagingItemLocaleSupplierModelBuilder WithBusinessUnit(int businessUnit)
        {
            this.businessUnit = businessUnit;
            return this;
        }

        internal TestStagingItemLocaleSupplierModelBuilder WithScanCode(string scanCode)
        {
            this.scanCode = scanCode;
            return this;
        }

        internal TestStagingItemLocaleSupplierModelBuilder WithTimestamp(DateTime timestamp)
        {
            this.timestamp = timestamp;
            return this;
        }

        internal TestStagingItemLocaleSupplierModelBuilder WithTransactionId(Guid transactionId)
        {
            this.transactionId = transactionId;
            return this;
        }

        internal TestStagingItemLocaleSupplierModelBuilder WithSupplierName(string supplierName)
        {
            this.supplierName = supplierName;
            return this;
        }

        internal TestStagingItemLocaleSupplierModelBuilder WithSupplierItemId(string supplierItemId)
        {
            this.supplierItemId = supplierItemId;
            return this;
        }

        internal TestStagingItemLocaleSupplierModelBuilder WithSupplierCaseSize(decimal? supplierCaseSize)
        {
            this.supplierCaseSize = supplierCaseSize;
            return this;
        }

        internal TestStagingItemLocaleSupplierModelBuilder WithIrmaVendorKey(string irmaVendorKey)
        {
            this.irmaVendorKey = irmaVendorKey;
            return this;
        }

        internal StagingItemLocaleSupplierModel Build()
        {
            var itemLocaleStaging = new StagingItemLocaleSupplierModel
            {
                BusinessUnitID = businessUnit,
                IrmaVendorKey = irmaVendorKey,
                Region = region,
                ScanCode = scanCode,
                SupplierCaseSize = supplierCaseSize,
                SupplierItemId = supplierItemId,
                SupplierName = supplierName,
                Timestamp = timestamp,
                TransactionId = transactionId
            };

            return itemLocaleStaging;
        }
    }
}
