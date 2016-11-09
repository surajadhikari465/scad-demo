using Icon.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Testing.Builders
{
    public class TestPriceMessageBuilder
    {
        private int messageTypeId;
        private int messageStatusId;
        private int? messageHistoryId;
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
        private string uomCode;
        private string uomName;
        private string currencyCode;
        private decimal? price;
        private int? multiple;
        private decimal? salePrice;
        private int? saleMultiple;
        private DateTime? saleStartDate;
        private DateTime? saleEndDate;
        private decimal? previousSalePrice;
        private int? previousSaleMultiple;
        private DateTime? previousSaleStartDate;
        private DateTime? previousSaleEndDate;
        private int? inProcessBy;
        private DateTime? processedDate;

        public TestPriceMessageBuilder()
        {
            this.messageTypeId = MessageTypes.ItemLocale;
            this.messageStatusId = MessageStatusTypes.Ready;
            this.messageHistoryId = null;
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
            this.uomCode = "EA";
            this.uomName = "Each";
            this.currencyCode = CurrencyCodes.Usd;
            this.price = 1.99m;
            this.multiple = 1;
            this.salePrice = null;
            this.saleMultiple = null;
            this.saleStartDate = null;
            this.saleEndDate = null;
            this.previousSalePrice = null;
            this.previousSaleMultiple = null;
            this.previousSaleStartDate = null;
            this.previousSaleEndDate = null;
            this.inProcessBy = null;
            this.processedDate = null;
        }

        public TestPriceMessageBuilder WithIrmaPushId(int irmaPushId)
        {
            this.irmaPushId = irmaPushId;
            return this;
        }

        public TestPriceMessageBuilder WithMessageTypeId(int messageTypeId)
        {
            this.messageTypeId = messageTypeId;
            return this;
        }

        public TestPriceMessageBuilder WithItemId(int itemId)
        {
            this.itemId = itemId;
            return this;
        }

        public TestPriceMessageBuilder WithStatusId(int id)
        {
            this.messageStatusId = id;
            return this;
        }

        public MessageQueuePrice Build()
        {
            return new MessageQueuePrice
            {
                MessageTypeId = this.messageTypeId,
                MessageStatusId = this.messageStatusId,
                MessageHistoryId = this.messageHistoryId,
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
                UomCode = this.uomCode,
                UomName = this.uomName,
                CurrencyCode = this.currencyCode,
                Price = this.price,
                Multiple = this.multiple,
                SalePrice = this.salePrice,
                SaleMultiple = this.saleMultiple,
                SaleStartDate = this.saleStartDate,
                SaleEndDate = this.saleEndDate,
                PreviousSalePrice = this.previousSalePrice,
                PreviousSaleMultiple = this.previousSaleMultiple,
                PreviousSaleStartDate = this.previousSaleStartDate,
                PreviousSaleEndDate = this.previousSaleEndDate,
                InProcessBy = this.inProcessBy,
                ProcessedDate = this.processedDate
            };
        }

        public static implicit operator MessageQueuePrice(TestPriceMessageBuilder builder)
        {
            return builder.Build();
        }
    }
}
