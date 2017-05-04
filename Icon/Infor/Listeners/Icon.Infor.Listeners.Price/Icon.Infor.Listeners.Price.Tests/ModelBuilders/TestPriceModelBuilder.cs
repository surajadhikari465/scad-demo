using Icon.Esb.Schemas.Wfm.ContractTypes;
using Icon.Infor.Listeners.Price.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.Listeners.Price.Tests.ModelBuilders
{
    public class TestPriceModelBuilder
    {
        private Guid gpmId;
        private ActionEnum action;
        private int itemId;
        private int businessUnitId;
        private PriceTypeIdType priceType;
        private PriceTypeIdType priceTypeAttribute;
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

        internal TestPriceModelBuilder()
        {
            this.gpmId = Guid.NewGuid();
            this.action = ActionEnum.Add;
            this.itemId = 1;
            this.businessUnitId = 1;
            this.priceType = PriceTypeIdType.REG;
            this.priceTypeAttribute = PriceTypeIdType.REG;
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

        internal TestPriceModelBuilder WithAction(ActionEnum action)
        {
            this.action = action;
            return this;
        }

        internal TestPriceModelBuilder WithReplaceGpmId(Guid? replaceGpmId)
        {
            this.replaceGpmId = replaceGpmId;
            return this;
        }

        internal TestPriceModelBuilder WithGpmId(Guid gpmId)
        {
            this.gpmId = gpmId;
            return this;
        }

        internal PriceModel Build()
        {
            var price = new PriceModel();
            price.GpmId = this.gpmId;
            price.Action = this.action;
            price.ItemId = this.itemId;
            price.BusinessUnitId = this.businessUnitId;
            price.PriceType = this.priceType;
            price.PriceTypeAttribute = this.priceTypeAttribute;
            price.SellableUom = this.sellableUom;
            price.Price = this.price;
            price.Multiple = this.multiple;
            price.CurrencyCode = this.currencyCode;
            price.CurrencyId = this.currencyId;
            price.StartDate = this.startDate;
            price.EndDate = this.endDate;
            price.ReplacedGpmId = this.replaceGpmId;
            price.NewTagExpiration = this.newTagExpiration;
            price.Region = this.region;
            price.AddedDate = this.addedDate;
            price.ErrorCode = this.errorCode;
            price.ErrorDetails = this.errorDetails;

            return price;
        }
    }
}
