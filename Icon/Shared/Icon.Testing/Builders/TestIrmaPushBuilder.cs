using Icon.Framework;
using System;

namespace Icon.Testing.Builders
{
    public class TestIrmaPushBuilder
    {
        private string regionCode;
        private int businessUnitId;
        private string identifier;
        private string changeType;
        private DateTime insertDate;
        private decimal retailSize;
        private string retailPackageUom;
        private bool tmDiscountEligible;
        private bool caseDiscount;
        private int? ageCode;
        private bool recallFlag;
        private bool restrictedHours;
        private bool soldByWeight;
        private bool scaleForcedTare;
        private bool quantityRequired;
        private bool priceRequired;
        private bool quantityProhibit;
        private bool visualVerify;
        private bool restrictSale;
        private int? posScaleTare;
        private string linkedIdentifier;
        private decimal? price;
        private string retailUom;
        private int? multiple;
        private int? saleMultiple;
        private decimal? salePrice;
        private DateTime? saleStartDate;
        private DateTime? saleEndDate;
        private int? inProcessBy;
        private DateTime? inUdmDate;
        private DateTime? esbReadyDate;
        private DateTime? udmFailedDate;
        private DateTime? esbReadyFailedDate;

        public TestIrmaPushBuilder()
        {
            this.regionCode = "SW";
            this.businessUnitId = 10145;
            this.identifier = "2222222";
            this.changeType = "ScanCodeAdd";
            this.insertDate = DateTime.Now;
            this.retailSize = 1m;
            this.retailPackageUom = "EA";
            this.tmDiscountEligible = true;
            this.caseDiscount = true;
            this.ageCode = null;
            this.recallFlag = false;
            this.restrictedHours = false;
            this.soldByWeight = false;
            this.scaleForcedTare = false;
            this.quantityRequired = false;
            this.priceRequired = false;
            this.quantityProhibit = false;
            this.visualVerify = false;
            this.restrictSale = false;
            this.posScaleTare = 1;
            this.linkedIdentifier = null;
            this.price = 1.99m;
            this.retailUom = "EA";
            this.multiple = 1;
            this.saleMultiple = null;
            this.salePrice = null;
            this.saleStartDate = null;
            this.saleEndDate = null;
            this.inProcessBy = null;
            this.inUdmDate = null;
            this.esbReadyDate = null;
            this.udmFailedDate = null;
            this.esbReadyFailedDate = null;
        }

        public TestIrmaPushBuilder WithIdentifier(string identifier)
        {
            this.identifier = identifier;
            return this;
        }

        public TestIrmaPushBuilder WithChangeType(string changeType)
        {
            this.changeType = changeType;
            return this;
        }

        public TestIrmaPushBuilder WithBusinessUnitId(int businessUnitId)
        {
            this.businessUnitId = businessUnitId;
            return this;
        }

        public TestIrmaPushBuilder WithLinkedIdentifier(string linkedIdentifier)
        {
            this.linkedIdentifier = linkedIdentifier;
            return this;
        }

        public TestIrmaPushBuilder WithSoldByWeight(bool soldByWeight)
        {
            this.soldByWeight = soldByWeight;
            return this;
        }

        public TestIrmaPushBuilder WithSaleInformation(decimal? salePrice, int? saleMultiple, DateTime? saleStartDate, DateTime? saleEndDate)
        {
            this.salePrice = salePrice;
            this.saleMultiple = saleMultiple;
            this.saleStartDate = saleStartDate;
            this.saleEndDate = saleEndDate;

            return this;
        }

        public TestIrmaPushBuilder WithInsertDate(DateTime insertDate)
        {
            this.insertDate = insertDate;
            return this;
        }

        public TestIrmaPushBuilder WithInProcessBy(int? inProcessBy)
        {
            this.inProcessBy = inProcessBy;
            return this;
        }

        public TestIrmaPushBuilder WithEsbReadyDate(DateTime? esbReadyDate)
        {
            this.esbReadyDate = esbReadyDate;
            return this;
        }

        public TestIrmaPushBuilder WithEsbReadyFailedDate(DateTime? esbReadyFailedDate)
        {
            this.esbReadyFailedDate = esbReadyFailedDate;
            return this;
        }

        public TestIrmaPushBuilder WithInUdmDate(DateTime? inUdmDate)
        {
            this.inUdmDate = inUdmDate;
            return this;
        }

        public TestIrmaPushBuilder WithUdmFailedDate(DateTime? udmFailedDate)
        {
            this.udmFailedDate = udmFailedDate;
            return this;
        }

        public TestIrmaPushBuilder WithRegionCode(string regionCode)
        {
            this.regionCode = regionCode;
            return this;
        }

        public IRMAPush Build()
        {
            return new IRMAPush
            {
                RegionCode = this.regionCode,
                BusinessUnit_ID = this.businessUnitId,
                Identifier = this.identifier,
                ChangeType = this.changeType,
                InsertDate = this.insertDate,
                RetailSize = this.retailSize,
                RetailPackageUom = this.retailPackageUom,
                TMDiscountEligible = this.tmDiscountEligible,
                Case_Discount = this.caseDiscount,
                AgeCode = this.ageCode,
                Recall_Flag = this.recallFlag,
                Restricted_Hours = this.restrictedHours,
                Sold_By_Weight = this.soldByWeight,
                ScaleForcedTare = this.scaleForcedTare,
                Quantity_Required = this.quantityRequired,
                Price_Required = this.priceRequired,
                QtyProhibit = this.quantityProhibit,
                VisualVerify = this.visualVerify,
                RestrictSale = this.restrictSale,
                PosScaleTare = this.posScaleTare,
                LinkedIdentifier = this.linkedIdentifier,
                Price = this.price,
                RetailUom = this.retailUom,
                Multiple = this.multiple,
                SaleMultiple = this.saleMultiple,
                Sale_Price = this.salePrice,
                Sale_Start_Date = this.saleStartDate,
                Sale_End_Date = this.saleEndDate,
                InProcessBy = this.inProcessBy,
                InUdmDate = this.inUdmDate,
                EsbReadyDate = this.esbReadyDate,
                UdmFailedDate = this.udmFailedDate,
                EsbReadyFailedDate = this.esbReadyFailedDate
            };
        }

        public static implicit operator IRMAPush(TestIrmaPushBuilder builder)
        {
            return builder.Build();
        }
    }
}
