using Icon.Framework;
using System;

namespace Icon.Testing.Builders
{
    public class TestItemLocaleMessageBuilder
    {
        private int messageTypeId;
        private int messageStatusId;
        private int? messageHistoryId;
        private int messageActionId;
        private int irmaPushId;
        private DateTime insertDate;
        private string regionCode;
        private int businessUnitId;
        private int itemId;
        private string itemTypeCode;
        private string itemTypeDesc;
        private int localeId;
        private string localeName;
        private int scanCodeId;
        private string scanCode;
        private int scanCodeTypeId;
        private string scanCodeTypeDesc;
        private string changeType;
        private bool lockedForSale;
        private bool recall;
        private bool tmDiscountEligible;
        private bool caseDiscount;
        private int? ageCode;
        private bool restrictedHours;
        private bool soldByWeight;
        private bool scaleForcedTare;
        private bool quantityRequired;
        private bool priceRequired;
        private bool quantityProhibit;
        private bool visualVerify;
        private string linkedItemScanCode;
        private int? posScaleTare;
        private int? inProcessBy;
        private DateTime? processedDate;
        private string previousLinkedItemScanCode;

        public TestItemLocaleMessageBuilder()
        {
            this.messageTypeId = MessageTypes.ItemLocale;
            this.messageStatusId = MessageStatusTypes.Ready;
            this.messageHistoryId = null;
            this.messageActionId = MessageActionTypes.AddOrUpdate;
            this.irmaPushId = 0;
            this.insertDate = DateTime.Now;
            this.regionCode = "SW";
            this.businessUnitId = 10145;
            this.itemId = 1;
            this.itemTypeCode = ItemTypeCodes.RetailSale;
            this.itemTypeDesc = "Retail Sale";
            this.localeId = 100;
            this.localeName = "Test Store";
            this.scanCodeId = 222;
            this.scanCode = "2222222";
            this.scanCodeTypeId = ScanCodeTypes.Upc;
            this.scanCodeTypeDesc = "UPC";
            this.changeType = "ScanCodeAdd";
            this.lockedForSale = false;
            this.recall = false;
            this.tmDiscountEligible = true;
            this.caseDiscount = true;
            this.ageCode = null;
            this.restrictedHours = false;
            this.soldByWeight = false;
            this.scaleForcedTare = false;
            this.quantityRequired = false;
            this.priceRequired = false;
            this.quantityProhibit = false;
            this.visualVerify = false;
            this.linkedItemScanCode = null;
            this.previousLinkedItemScanCode = null;
            this.posScaleTare = 1;
            this.inProcessBy = null;
            this.processedDate = null;
        }

        public TestItemLocaleMessageBuilder WithIrmaPushId(int irmaPushId)
        {
            this.irmaPushId = irmaPushId;
            return this;
        }

        public TestItemLocaleMessageBuilder WithMessageTypeId(int messageTypeId)
        {
            this.messageTypeId = messageTypeId;
            return this;
        }

        public TestItemLocaleMessageBuilder WithMessageHistoryId(int messageHistoryId)
        {
            this.messageHistoryId = messageHistoryId;
            return this;
        }

        public TestItemLocaleMessageBuilder WithLinkedItem(string linkedItemScanCode)
        {
            this.linkedItemScanCode = linkedItemScanCode;
            return this;
        }

        public TestItemLocaleMessageBuilder WithItemId(int itemId)
        {
            this.itemId = itemId;
            return this;
        }

        public TestItemLocaleMessageBuilder WithMessageStatusId(int messageStatusId)
        {
            this.messageStatusId = messageStatusId;
            return this;
        }

        public TestItemLocaleMessageBuilder WithInProcessBy(int? instance)
        {
            this.inProcessBy = instance;
            return this;
        }

        public TestItemLocaleMessageBuilder WithBusinessUnitId(int businessUnitId)
        {
            this.businessUnitId = businessUnitId;
            return this;
        }

        public TestItemLocaleMessageBuilder WithPreviousLinkedItem(string linkedItemScanCode)
        {
            this.previousLinkedItemScanCode = linkedItemScanCode;
            return this;
        }

        public MessageQueueItemLocale Build()
        {
            return new MessageQueueItemLocale
            {
                MessageTypeId = this.messageTypeId,
                MessageStatusId = this.messageStatusId,
                MessageHistoryId = this.messageHistoryId,
                MessageActionId = this.messageActionId,
                IRMAPushID = this.irmaPushId,
                InsertDate = this.insertDate,
                RegionCode = this.regionCode,
                BusinessUnit_ID = this.businessUnitId,
                ItemId = this.itemId,
                ItemTypeCode = this.itemTypeCode,
                ItemTypeDesc = this.itemTypeDesc,
                LocaleId = this.localeId,
                LocaleName = this.localeName,
                ScanCodeId = this.scanCodeId,
                ScanCode = this.scanCode,
                ScanCodeTypeId = this.scanCodeTypeId,
                ScanCodeTypeDesc = this.scanCodeTypeDesc,
                ChangeType = this.changeType,
                LockedForSale = this.lockedForSale,
                Recall = this.recall,
                TMDiscountEligible = this.tmDiscountEligible,
                Case_Discount = this.caseDiscount,
                AgeCode = this.ageCode,
                Restricted_Hours = this.restrictedHours,
                Sold_By_Weight = this.soldByWeight,
                ScaleForcedTare = this.scaleForcedTare,
                Quantity_Required = this.quantityRequired,
                Price_Required = this.priceRequired,
                QtyProhibit = this.quantityProhibit,
                VisualVerify = this.visualVerify,
                LinkedItemScanCode = this.linkedItemScanCode,
                PreviousLinkedItemScanCode = this.previousLinkedItemScanCode,
                PosScaleTare = this.posScaleTare,
                InProcessBy = this.inProcessBy,
                ProcessedDate = this.processedDate
            };
        }

        public static implicit operator MessageQueueItemLocale(TestItemLocaleMessageBuilder builder)
        {
            return builder.Build();
        }
    }
}
