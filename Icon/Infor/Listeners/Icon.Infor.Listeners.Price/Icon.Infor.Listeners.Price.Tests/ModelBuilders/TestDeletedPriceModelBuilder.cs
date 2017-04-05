using Icon.Infor.Listeners.Price.DataAccess.Models;
using System;

namespace Icon.Infor.Listeners.Price.Tests.ModelBuilders
{
    internal class TestDeletedPriceModelBuilder
    {
        private Guid gpmId;
        private int itemId;
        private int businessUnitId;
        private string priceType;
        private string priceTypeAttribute;
        private string sellableUom;
        private decimal price;
        private int multiple;
        private string currencyCode;
        private int currencyId;
        private DateTime startDate;
        private DateTime? endDate;
        private Guid? replaceGpmId;
        private DateTime? newTagExpiration;
        private string region;
        private DateTime addedDate;
        private DateTime deletedDate;

        internal TestDeletedPriceModelBuilder()
        {
            this.gpmId = Guid.NewGuid();
            this.itemId = 1;
            this.businessUnitId = 1;
            this.priceType = "REG";
            this.priceTypeAttribute = "REG";
            this.sellableUom = "EA";
            this.price = 4.99m;
            this.multiple = 1;
            this.currencyCode = "USD";
            this.currencyId = 1;
            this.startDate = DateTime.Today.AddDays(-1);
            this.endDate = null;
            this.replaceGpmId = null;
            this.newTagExpiration = null;
            this.region = "FL";
            this.addedDate = DateTime.Now;
            this.deletedDate = DateTime.Now;
        }

        internal TestDeletedPriceModelBuilder WithGpmId(Guid gpmId)
        {
            this.gpmId = gpmId;
            return this;
        }

        internal DeletedPriceModel Build()
        {
            var deletedPrice = new DeletedPriceModel();
            deletedPrice.GpmID = this.gpmId;
            deletedPrice.ItemID = this.itemId;
            deletedPrice.BusinessUnitID = this.businessUnitId;
            deletedPrice.PriceType = this.priceType;
            deletedPrice.PriceTypeAttribute = this.priceTypeAttribute;
            deletedPrice.PriceUOM = this.sellableUom;
            deletedPrice.Price = this.price;
            deletedPrice.Multiple = this.multiple;
            deletedPrice.CurrencyID = this.currencyId;
            deletedPrice.StartDate = this.startDate;
            deletedPrice.EndDate = this.endDate;
            deletedPrice.NewTagExpiration = this.newTagExpiration;
            deletedPrice.Region = this.region;
            deletedPrice.AddedDate = this.addedDate;

            return deletedPrice;
        }
    }
}
