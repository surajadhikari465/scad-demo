using Icon.Infor.Listeners.Price.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.Listeners.Price.Tests.ModelBuilders
{
    internal class TestDbPriceModelBuilder
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
        private string errorCode;
        private string errorDetails;

        internal TestDbPriceModelBuilder()
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
            this.errorCode = null;
            this.errorDetails = null;
        }

        internal TestDbPriceModelBuilder WithGpmId(Guid gpmId)
        {
            this.gpmId = gpmId;
            return this;
        }

        internal DbPriceModel Build()
        {
            var price = new DbPriceModel();
            price.GpmID = this.gpmId;
            price.ItemID = this.itemId;
            price.BusinessUnitID = this.businessUnitId;
            price.PriceType = this.priceType;
            price.PriceTypeAttribute = this.priceTypeAttribute;
            price.PriceUOM = this.sellableUom;
            price.Price = this.price;
            price.Multiple = this.multiple;
            price.CurrencyID = this.currencyId;
            price.StartDate = this.startDate;
            price.EndDate = this.endDate;
            price.NewTagExpiration = this.newTagExpiration;
            price.Region = this.region;
            price.AddedDate = this.addedDate;

            return price;
        }
    }
}
