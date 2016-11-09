using Icon.Framework;
using System;

namespace Icon.Testing.Builders
{
    public class TestItemPriceBuilder
    {
        private int itemId;
        private int localeId;
        private int itemPriceTypeId;
        private int uomId;
        private int currencyTypeId;
        private decimal priceAmount;
        private int? breakPointStartQuantity;
        private DateTime? startDate;
        private DateTime? endDate;

        public TestItemPriceBuilder()
        {
            this.itemId = 1;
            this.localeId = 100;
            this.itemPriceTypeId = ItemPriceTypes.Reg;
            this.uomId = UOMs.Each;
            this.currencyTypeId = CurrencyTypes.Usd;
            this.priceAmount = 2.99m;
            this.breakPointStartQuantity = 1;
            this.startDate = DateTime.Now.Date;
            this.endDate = DateTime.Now.AddDays(1).Date;
        }

        public TestItemPriceBuilder WithItemId(int itemId)
        {
            this.itemId = itemId;
            return this;
        }

        public TestItemPriceBuilder WithLocaleId(int localeId)
        {
            this.localeId = localeId;
            return this;
        }

        public TestItemPriceBuilder WithItemPriceTypeId(int itemPriceTypeId)
        {
            this.itemPriceTypeId = itemPriceTypeId;
            return this;
        }

        public TestItemPriceBuilder WithPriceAmount(decimal price)
        {
            this.priceAmount = price;
            return this;
        }

        public TestItemPriceBuilder WithUomId(int uomId)
        {
            this.uomId = uomId;
            return this;
        }

        public ItemPrice Build()
        {
            return new ItemPrice
            {
                itemID = this.itemId,
                localeID = this.localeId,
                itemPriceTypeID = this.itemPriceTypeId,
                uomID = this.uomId,
                currencyTypeID = this.currencyTypeId,
                itemPriceAmt = this.priceAmount,
                breakPointStartQty = this.breakPointStartQuantity,
                startDate = this.startDate,
                endDate = this.endDate
            };
        }

        public static implicit operator ItemPrice(TestItemPriceBuilder builder)
        {
            return builder.Build();
        }
    }
}
