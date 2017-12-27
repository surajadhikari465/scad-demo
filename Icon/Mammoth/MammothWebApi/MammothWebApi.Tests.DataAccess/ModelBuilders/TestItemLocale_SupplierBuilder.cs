using MammothWebApi.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MammothWebApi.Tests.DataAccess.ModelBuilders
{
    public class TestItemLocale_SupplierBuilder
    {
        private string region;
        private DateTime addedDateUtc;
        private int businessUnit;
        public string irmaVendorKey;
        private int itemId;
        private DateTime? modifiedDateUtc;
        public string supplierName;
        public string supplierItemId;
        public decimal? supplierCaseSize;

        internal TestItemLocale_SupplierBuilder()
        {
            region = "SW";
            addedDateUtc = DateTime.UtcNow;
            businessUnit = 1;
            irmaVendorKey = "Test Key";
            itemId = 1;
            modifiedDateUtc = null;
            supplierName = "Test Supplier";
            supplierItemId = "Test Item ID";
            supplierCaseSize = 1m;
        }

        internal TestItemLocale_SupplierBuilder WithRegion(string region)
        {
            this.region = region;
            return this;
        }

        internal TestItemLocale_SupplierBuilder WithBusinessUnit(int businessUnit)
        {
            this.businessUnit = businessUnit;
            return this;
        }

        internal TestItemLocale_SupplierBuilder WithItemId(int itemId)
        {
            this.itemId = itemId;
            return this;
        }

        internal TestItemLocale_SupplierBuilder WithAddedDateUtc(DateTime addedDateUtc)
        {
            this.addedDateUtc = addedDateUtc;
            return this;
        }

        internal TestItemLocale_SupplierBuilder WithModifiedDateUtc(DateTime modifiedDateUtc)
        {
            this.modifiedDateUtc = modifiedDateUtc;
            return this;
        }

        internal TestItemLocale_SupplierBuilder WithSupplierName(string supplierName)
        {
            this.supplierName = supplierName;
            return this;
        }

        internal TestItemLocale_SupplierBuilder WithSupplierItemId(string supplierItemId)
        {
            this.supplierItemId = supplierItemId;
            return this;
        }

        internal TestItemLocale_SupplierBuilder WithSupplierCaseSize(int supplierCaseSize)
        {
            this.supplierCaseSize = supplierCaseSize;
            return this;
        }

        internal TestItemLocale_SupplierBuilder WithIrmaVendorKey(string irmaVendorKey)
        {
            this.irmaVendorKey = irmaVendorKey;
            return this;
        }

        internal ItemLocale_Supplier Build()
        {
            var itemLocale_Supplier = new ItemLocale_Supplier
            {
                AddedDateUtc = addedDateUtc,
                BusinessUnitID = businessUnit,
                IrmaVendorKey = irmaVendorKey,
                ItemID = itemId,
                ModifiedDateUtc = modifiedDateUtc,
                Region = region,
                SupplierCaseSize = supplierCaseSize,
                SupplierItemID = supplierItemId,
                SupplierName = supplierName,
            };

            return itemLocale_Supplier;
        }
    }
}
