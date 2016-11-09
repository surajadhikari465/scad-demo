using Irma.Framework;
using System;

namespace Irma.Testing.Builders
{
    public class TestIconPosPushPublishBuilder
    {
        private int iconPosPushPublishId;
        private int? priceBatchHeaderId;
        private string regionCode;
        private int storeNumber;
        private int itemKey;
        private string identifier;
        private string changeType;
        private DateTime insertDate;
        private int businessUnitId;
        private decimal? retailSize;
        private string retailPackageUom;
        private bool tmDiscountEligible;
        private bool caseDiscount;
        private int? ageCode;
        private bool recall;
        private bool restrictedHours;
        private bool soldByWeight;
        private bool scaleForcedTare;
        private bool quantityRequired;
        private bool priceRequired;
        private bool quantityProhibit;
        private bool visualVerify;
        private bool restrictSale;
        private decimal? price;
        private string retailUom;
        private decimal? salePrice;
        private int? multiple;
        private int? saleMultiple;
        private DateTime? saleStartDate;
        private DateTime? saleEndDate;
        private int? inProcessBy;
        private DateTime? processedDate;
        private DateTime? processingFailedDate;
        private string linkCodeItemIdentifier;
        private int? posTare;

        public TestIconPosPushPublishBuilder()
        {
            this.priceBatchHeaderId = null;
            this.regionCode = "SP";
            this.storeNumber = 300;
            this.itemKey = 1;
            this.identifier = "2222222";
            this.changeType = "ScanCodeAdd";
            this.insertDate = DateTime.Now;
            this.businessUnitId = 10145;
            this.retailSize = 1m;
            this.retailPackageUom = "EA";
            this.tmDiscountEligible = true;
            this.caseDiscount = true;
            this.ageCode = null;
            this.recall = false;
            this.restrictedHours = false;
            this.soldByWeight = false;
            this.scaleForcedTare = false;
            this.quantityRequired = false;
            this.priceRequired = false;
            this.quantityProhibit = false;
            this.visualVerify = false;
            this.restrictSale = false;
            this.price = 2.99m;
            this.retailUom = "EA";
            this.salePrice = null;
            this.multiple = 1;
            this.saleMultiple = null;
            this.saleStartDate = null;
            this.saleEndDate = null;
            this.inProcessBy = null;
            this.processedDate = null;
            this.processingFailedDate = null;
            this.linkCodeItemIdentifier = null;
            this.posTare = null;
        }

        public TestIconPosPushPublishBuilder WithIconPosPushPublishId(int publishId)
        {
            this.iconPosPushPublishId = publishId;
            return this;
        }

        public TestIconPosPushPublishBuilder WithIdentifier(string identifier)
        {
            this.identifier = identifier;
            return this;
        }

        public TestIconPosPushPublishBuilder WithInsertDate(DateTime insertDate)
        {
            this.insertDate = insertDate;
            return this;
        }

        public TestIconPosPushPublishBuilder WithInProcessBy(int? inProcessBy)
        {
            this.inProcessBy = inProcessBy;
            return this;
        }

        public TestIconPosPushPublishBuilder WithProcessedDate(DateTime processedDate)
        {
            this.processedDate = processedDate;
            return this;
        }

        public TestIconPosPushPublishBuilder WithProcessingFailedDate(DateTime? processingFailedDate)
        {
            this.processingFailedDate = processingFailedDate;
            return this;
        }

        public TestIconPosPushPublishBuilder WithStoreNumber(int storeNumber)
        {
            this.storeNumber = storeNumber;
            return this;
        }

        public TestIconPosPushPublishBuilder WithSaleStartDate(DateTime startDate)
        {
            this.saleStartDate = startDate;
            return this;
        }

        public TestIconPosPushPublishBuilder WithSaleEndDate(DateTime endDate)
        {
            this.saleEndDate = endDate;
            return this;
        }

        public TestIconPosPushPublishBuilder WithChangeType(String changeType)
        {
            this.changeType  = changeType;
            return this;
        }

        public TestIconPosPushPublishBuilder WithSalePrice(decimal salePrice)
        {
            this.salePrice = salePrice;
            return this;
        }

        public IConPOSPushPublish Build()
        {
            return new IConPOSPushPublish
            {
                IConPOSPushPublishID = this.iconPosPushPublishId,
                PriceBatchHeaderID = this.priceBatchHeaderId,
                RegionCode = this.regionCode,
                Store_No = this.storeNumber,
                Item_Key = this.itemKey,
                Identifier = this.identifier,
                ChangeType = this.changeType,
                InsertDate = this.insertDate,
                BusinessUnit_ID = this.businessUnitId,
                RetailSize = this.retailSize,
                RetailPackageUom = this.retailPackageUom,
                TMDiscountEligible = this.tmDiscountEligible,
                Case_Discount = this.caseDiscount,
                AgeCode = this.ageCode,
                Recall_Flag = this.recall,
                Restricted_Hours = this.restrictedHours,
                Sold_By_Weight = this.soldByWeight,
                ScaleForcedTare = this.scaleForcedTare,
                Quantity_Required = this.quantityRequired,
                Price_Required = this.priceRequired,
                QtyProhibit = this.quantityProhibit,
                VisualVerify = this.visualVerify,
                RestrictSale = this.restrictSale,
                Price = this.price,
                RetailUom = this.retailUom,
                Sale_Price = this.salePrice,
                Multiple = this.multiple,
                SaleMultiple = this.saleMultiple,
                Sale_Start_Date = this.saleStartDate,
                Sale_End_Date = this.saleEndDate,
                InProcessBy = this.inProcessBy,
                ProcessedDate = this.processedDate,
                ProcessingFailedDate = this.processingFailedDate,
                LinkCode_ItemIdentifier = this.linkCodeItemIdentifier,
                POSTare = this.posTare
            };
        }

        public static implicit operator IConPOSPushPublish(TestIconPosPushPublishBuilder builder)
        {
            return builder.Build();
        }
    }
}
