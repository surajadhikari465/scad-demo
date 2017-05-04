using Icon.Infor.Listeners.Price.DataAccess.Models;
using System;

namespace Icon.Infor.Listeners.Price.Integration.Tests
{
    internal class TestItemBuilder
    {
        private int itemId;
        private int? itemTypeId;
        private string scanCode;
        private Nullable<int> hierarchyMerchandiseId;
        private Nullable<int> hierarchyNationalClassId;
        private Nullable<int> brandId;
        private Nullable<int> taxClassId;
        private Nullable<int> psNumber;
        private string productDescription;
        private string posDescription;
        private string packageUnit;
        private string retailSize;
        private string retailUom;
        private bool foodStampEligible;
        private DateTime insertDate;
        private DateTime? modifiedDate;

        public TestItemBuilder()
        {
            this.itemId = 1;
            this.itemTypeId = 1;
            this.scanCode = "7777777712";
            this.hierarchyMerchandiseId = 1;
            this.hierarchyNationalClassId = 1;
            this.brandId = 1;
            this.taxClassId = 1;
            this.psNumber = 4556;
            this.productDescription = "Test Builder Item";
            this.posDescription = "Test Builder";
            this.packageUnit = "1";
            this.retailSize = "1";
            this.retailUom = "EA";
            this.foodStampEligible = true;
            this.insertDate = DateTime.UtcNow;
            this.modifiedDate = null;
        }

        internal TestItemBuilder WithItemId(int itemId)
        {
            this.itemId = itemId;
            return this;
        }

        internal TestItemBuilder WithItemTypeId(int? itemTypeId)
        {
            this.itemTypeId = itemTypeId;
            return this;
        }

        internal TestItemBuilder WithScanCode(string scanCode)
        {
            this.scanCode = scanCode;
            return this;
        }

        internal TestItemBuilder WithHierarchyMerchandiseId(int hierarchyMerchandiseId)
        {
            this.hierarchyMerchandiseId = hierarchyMerchandiseId;
            return this;
        }

        internal TestItemBuilder WithHierarchyNationalClassId(int hierarchyNationalClassId)
        {
            this.hierarchyNationalClassId = hierarchyNationalClassId;
            return this;
        }

        internal TestItemBuilder WithBrandHcid(int brandHcid)
        {
            this.brandId = brandHcid;
            return this;
        }

        internal TestItemBuilder WithTaxHcid(int taxHcid)
        {
            this.taxClassId = taxHcid;
            return this;
        }

        internal TestItemBuilder WithPsNumber(int psNumber)
        {
            this.psNumber = psNumber;
            return this;
        }

        internal TestItemBuilder WithProductDescription(string description)
        {
            this.productDescription = description;
            return this;
        }

        internal TestItemBuilder WithPosDescription(string posDescription)
        {
            this.posDescription = posDescription;
            return this;
        }

        internal TestItemBuilder WithRetailSize(string size)
        {
            this.retailSize = size;
            return this;
        }

        internal TestItemBuilder WithRetailUom(string uom)
        {
            this.retailUom = uom;
            return this;
        }

        internal TestItemBuilder WithPackageUnit(string packageUnit)
        {
            this.packageUnit = packageUnit;
            return this;
        }

        internal TestItemBuilder WithInsertDate(DateTime insertDate)
        {
            this.insertDate = insertDate;
            return this;
        }

        internal TestItemBuilder WithModifiedDate(DateTime? modifiedDate)
        {
            this.modifiedDate = modifiedDate;
            return this;
        }

        internal TestItemBuilder WithFoodStamp(bool foodStamp)
        {
            this.foodStampEligible = foodStamp;
            return this;
        }

        internal Item Build()
        {
            var item = new Item
            {
                ItemID = this.itemId,
                ItemTypeID = this.itemTypeId,
                ScanCode = this.scanCode,
                BrandHCID = this.brandId,
                Desc_POS = this.posDescription,
                Desc_Product = this.productDescription,
                PSNumber = this.psNumber,
                FoodStampEligible = this.foodStampEligible,
                HierarchyMerchandiseID = this.hierarchyMerchandiseId,
                HierarchyNationalClassID = this.hierarchyNationalClassId,
                PackageUnit = this.packageUnit,
                RetailSize = this.retailSize,
                RetailUOM = this.retailUom,
                TaxClassHCID = this.taxClassId
            };

            return item;
        }
    }
}
