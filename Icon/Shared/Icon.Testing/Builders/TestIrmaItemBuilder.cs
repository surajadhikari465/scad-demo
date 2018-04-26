using Icon.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Testing.Builders
{
    public class TestIrmaItemBuilder
    {
        private int irmaItemId;
        private string regionCode;
        private string identifier;
        private bool defaultIdentifier;
        private string brandName;
        private string itemDescription;
        private string posDescription;
        private int packageUnit;
        private decimal? retailSize;
        private string retailUom;
        private bool foodStamp;
        private decimal posScaleTare;
        private bool departmentSale;
        private Nullable<bool> giftCard;
        private Nullable<int> taxClassId;
        private Nullable<int> merchandiseClassId;
        private System.DateTime insertDate;
        private Nullable<int> nationalClassId;

        public TestIrmaItemBuilder()
        {
            this.irmaItemId = 0;
            this.regionCode = "FL";
            this.identifier = "1234567890";
            this.defaultIdentifier = true;
            this.brandName = "Unit Test Item Brand 1";
            this.itemDescription = "Unit Test Item Description 1";
            this.posDescription = "UNIT TEST ITEM POS DESCRIPTION 1";
            this.packageUnit = 1;
            this.retailSize = 12;
            this.retailUom = "Each";
            this.foodStamp = true;
            this.posScaleTare = 0.6m;
            this.departmentSale = false;
            this.giftCard = false;
            this.taxClassId = 0;
            this.merchandiseClassId = null;
            this.insertDate = DateTime.Now;
            this.nationalClassId = null;
        }

        public TestIrmaItemBuilder WithIrmaItemId(int irmaItemId)
        {
            this.irmaItemId = irmaItemId;
            return this;
        }

        public TestIrmaItemBuilder WithRegionCode(string regionCode)
        {
            this.regionCode = regionCode;
            return this;
        }

        public TestIrmaItemBuilder WithIdentifier(string identifier)
        {
            this.identifier = identifier;
            return this;
        }

        public TestIrmaItemBuilder WithDefaultIdentifier(bool defaultIdentifier)
        {
            this.defaultIdentifier = defaultIdentifier;
            return this;
        }

        public TestIrmaItemBuilder WithBrandName(string brandName)
        {
            this.brandName = brandName;
            return this;
        }

        public TestIrmaItemBuilder WithItemDescription(string itemDescription)
        {
            this.itemDescription = itemDescription;
            return this;
        }

        public TestIrmaItemBuilder WithPosDescription(string posDescription)
        {
            this.posDescription = posDescription;
            return this;
        }

        public TestIrmaItemBuilder WithPackageUnit(int packageUnit)
        {
            this.packageUnit = packageUnit;
            return this;
        }

        public TestIrmaItemBuilder WithRetailSize(decimal retailSize)
        {
            this.retailSize = retailSize;
            return this;
        }

        public TestIrmaItemBuilder WithRetailUom(string retailUom)
        {
            this.retailUom = retailUom;
            return this;
        }
        public TestIrmaItemBuilder WithFoodStamp(bool foodStamp)
        {
            this.foodStamp = foodStamp;
            return this;
        }

        public TestIrmaItemBuilder WithPosScaleTare(decimal posScaleTare)
        {
            this.posScaleTare = posScaleTare;
            return this;
        }

        public TestIrmaItemBuilder WithdepartmentSale(bool departmentSale)
        {
            this.departmentSale = departmentSale;
            return this;
        }

        public TestIrmaItemBuilder WithgiftCard(bool giftCard)
        {
            this.giftCard = giftCard;
            return this;
        }

        public TestIrmaItemBuilder WithTaxClassId(int taxClassId)
        {
            this.taxClassId = taxClassId;
            return this;
        }

        public TestIrmaItemBuilder WithMerchandiseClassId(int merchandiseClassId)
        {
            this.merchandiseClassId = merchandiseClassId;
            return this;
        }

        public TestIrmaItemBuilder WithInsertDate(DateTime insertDate)
        {
            this.insertDate = insertDate;
            return this;
        }

        public TestIrmaItemBuilder WithNationalClassId(int nationalClassId)
        {
            this.nationalClassId = nationalClassId;
            return this;
        }

        public IRMAItem Build()
        {
            IRMAItem irmaItem = new IRMAItem();
            irmaItem.irmaItemID = this.irmaItemId;
            irmaItem.regioncode = this.regionCode;
            irmaItem.identifier = this.identifier;
            irmaItem.defaultIdentifier = this.defaultIdentifier;
            irmaItem.brandName = this.brandName;
            irmaItem.itemDescription = this.itemDescription;
            irmaItem.posDescription = this.posDescription;
            irmaItem.packageUnit = this.packageUnit;
            irmaItem.retailSize = this.retailSize;
            irmaItem.retailUom = this.retailUom;
            irmaItem.foodStamp = this.foodStamp;
            irmaItem.posScaleTare = this.posScaleTare;
            irmaItem.departmentSale = this.departmentSale;
            irmaItem.giftCard = this.giftCard;
            irmaItem.taxClassID = this.taxClassId;
            irmaItem.merchandiseClassID = this.merchandiseClassId;
            irmaItem.insertDate = this.insertDate;
            irmaItem.nationalClassID = this.nationalClassId;

            return irmaItem;
        }

        public static implicit operator IRMAItem(TestIrmaItemBuilder builder)
        {
            return builder.Build();
        }
    }
}
