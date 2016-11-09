using Mammoth.Common.DataAccess;
using Mammoth.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mammoth.Common.Testing.Builders
{
    public class TestMessageQueuePriceBuilder
    {
        private int messageQueueId;
        private int messageTypeId;
        private int messageStatusId;
        private int? messageHistoryId;
        private int messageActionId;
        private DateTime insertDate;
        private int itemId;
        private string itemTypeCode;
        private string itemTypeDesc;
        private int businessUnitId;
        private string localeName;
        private string scanCode;
        private string uomCode;
        private string currencyCode;
        private string priceTypeCode;
        private string subPriceTypeCode;
        private decimal price;
        private int multiple;
        private DateTime startDate;
        private DateTime? endDate;
        private int? inProcessBy;
        private DateTime? processedDate;

        public TestMessageQueuePriceBuilder()
        {
            this.messageQueueId = 0;
            this.messageTypeId = MessageTypes.Price;
            this.messageStatusId = MessageStatusTypes.Ready;
            this.messageHistoryId = null;
            this.messageActionId = MessageActions.AddOrUpdate;
            this.insertDate = DateTime.Now;
            this.itemId = 0;
            this.itemTypeCode = "TST";
            this.itemTypeDesc = "test item type";
            this.businessUnitId = 0;
            this.localeName = null;
            this.scanCode = null;
            this.uomCode = null;
            this.currencyCode = CurrencyCodes.UsDollar;
            this.price = 0;
            this.multiple = 0;
            this.startDate = DateTime.Now;
            this.endDate = null;
            this.inProcessBy = null;
            this.processedDate = null;
        }

        public TestMessageQueuePriceBuilder WithMessageQueueId(int messageQueueId)
        {
            this.messageQueueId = messageQueueId;
            return this;
        }

        public TestMessageQueuePriceBuilder WithMessageTypeId(int messageTypeId)
        {
            this.messageTypeId = messageTypeId;
            return this;
        }

        public TestMessageQueuePriceBuilder WithMessageStatusId(int messageStatusId)
        {
            this.messageStatusId = messageStatusId;
            return this;
        }

        public TestMessageQueuePriceBuilder WithMessageHistoryId(int? messageHistoryId)
        {
            this.messageHistoryId = messageHistoryId;
            return this;
        }

        public TestMessageQueuePriceBuilder WithMessageActionId(int messageActionId)
        {
            this.messageActionId = messageActionId;
            return this;
        }

        public TestMessageQueuePriceBuilder WithInsertDate(DateTime insertDate)
        {
            this.insertDate = insertDate;
            return this;
        }

        public TestMessageQueuePriceBuilder WithItemId(int itemId)
        {
            this.itemId = itemId;
            return this;
        }

        public TestMessageQueuePriceBuilder WithBusinessUnitId(int businessUnitId)
        {
            this.businessUnitId = businessUnitId;
            return this;
        }

        public TestMessageQueuePriceBuilder WithLocaleName(string localeName)
        {
            this.localeName = localeName;
            return this;
        }

        public TestMessageQueuePriceBuilder WithScanCode(string scanCode)
        {
            this.scanCode = scanCode;
            return this;
        }

        public TestMessageQueuePriceBuilder WithUomCode(string uomCode)
        {
            this.uomCode = uomCode;
            return this;
        }

        public TestMessageQueuePriceBuilder WithCurrencyCode(string currencyCode)
        {
            this.currencyCode = currencyCode;
            return this;
        }

        public TestMessageQueuePriceBuilder WithPriceTypeCode(string priceTypeCode)
        {
            this.priceTypeCode = priceTypeCode;
            return this;
        }

        public TestMessageQueuePriceBuilder WithSubPriceTypeCode(string subPriceTypeCode)
        {
            this.subPriceTypeCode = subPriceTypeCode;
            return this;
        }

        public TestMessageQueuePriceBuilder WithPrice(decimal price)
        {
            this.price = price;
            return this;
        }

        public TestMessageQueuePriceBuilder WithMultiple(int multiple)
        {
            this.multiple = multiple;
            return this;
        }

        public TestMessageQueuePriceBuilder WithStartDate(DateTime startDate)
        {
            this.startDate = startDate;
            return this;
        }

        public TestMessageQueuePriceBuilder WithEndDate(DateTime? endDate)
        {
            this.endDate = endDate;
            return this;
        }

        public TestMessageQueuePriceBuilder WithInProcessBy(int? inProcessBy)
        {
            this.inProcessBy = inProcessBy;
            return this;
        }

        public TestMessageQueuePriceBuilder WithProcessedDate(DateTime? processedDate)
        {
            this.processedDate = processedDate;
            return this;
        }

        public TestMessageQueuePriceBuilder PopulateAllAttributesReg()
        {
            this.messageTypeId = MessageTypes.Price;
            this.messageStatusId = MessageStatusTypes.Ready;
            this.messageActionId = MessageActions.AddOrUpdate;
            this.insertDate = DateTime.Now;
            this.businessUnitId = 1234;
            this.localeName = "Test Locale Name";
            this.scanCode = "12345";
            this.uomCode = "EA";
            this.currencyCode = CurrencyCodes.UsDollar;
            this.priceTypeCode = ItemPriceTypes.Codes.RegularPrice;
            this.price = 1.99m;
            this.multiple = 1;
            this.startDate = DateTime.Now;
            return this;
        }

        public TestMessageQueuePriceBuilder PopulateAllAttributesTpr()
        {
            this.messageTypeId = MessageTypes.Price;
            this.messageStatusId = MessageStatusTypes.Ready;
            this.messageActionId = MessageActions.AddOrUpdate;
            this.insertDate = DateTime.Now;
            this.businessUnitId = 1234;
            this.localeName = "Test Locale Name";
            this.scanCode = "12345";
            this.uomCode = "EA";
            this.currencyCode = CurrencyCodes.UsDollar;
            this.priceTypeCode = ItemPriceTypes.Codes.TemporaryPriceReduction;
            this.subPriceTypeCode = "SAL";
            this.price = 1.99m;
            this.multiple = 1;
            this.startDate = DateTime.Now.AddDays(5);
            this.endDate = DateTime.Now.AddDays(10);
            return this;
        }

        public MessageQueuePrice Build()
        {
            MessageQueuePrice messageQueuePrice = new MessageQueuePrice();

            messageQueuePrice.MessageQueueId = this.messageQueueId;
            messageQueuePrice.MessageTypeId = this.messageTypeId;
            messageQueuePrice.MessageStatusId = this.messageStatusId;
            messageQueuePrice.MessageHistoryId = this.messageHistoryId;
            messageQueuePrice.MessageActionId = this.messageActionId;
            messageQueuePrice.InsertDate = this.insertDate;
            messageQueuePrice.ItemId = this.itemId;
            messageQueuePrice.ItemTypeCode = this.itemTypeCode;
            messageQueuePrice.ItemTypeDesc = this.itemTypeDesc;
            messageQueuePrice.BusinessUnitId = this.businessUnitId;
            messageQueuePrice.LocaleName = this.localeName;
            messageQueuePrice.ScanCode = this.scanCode;
            messageQueuePrice.UomCode = this.uomCode;
            messageQueuePrice.CurrencyCode = this.currencyCode;
            messageQueuePrice.PriceTypeCode = this.priceTypeCode;
            messageQueuePrice.SubPriceTypeCode = this.subPriceTypeCode;
            messageQueuePrice.Price = this.price;
            messageQueuePrice.Multiple = this.multiple;
            messageQueuePrice.StartDate = this.startDate;
            messageQueuePrice.EndDate = this.endDate;
            messageQueuePrice.InProcessBy = this.inProcessBy;
            messageQueuePrice.ProcessedDate = this.processedDate;

            return messageQueuePrice;
        }

        public static implicit operator MessageQueuePrice(TestMessageQueuePriceBuilder builder)
        {
            return builder.Build();
        }
    }
}