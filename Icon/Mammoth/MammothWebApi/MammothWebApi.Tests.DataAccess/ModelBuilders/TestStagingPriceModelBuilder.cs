using MammothWebApi.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MammothWebApi.Tests.DataAccess.ModelBuilders
{
    public class TestStagingPriceModelBuilder
    {
        private string region;
        private int businessUnitId;
        private string scanCode;
        private int multiple;
        private decimal price;
        private DateTime startDate;
        private DateTime? endDate;
        private string priceUom;
        private string priceType;
        private string currencyCode;
        private DateTime timestamp;
        private Guid transactionId;

        internal TestStagingPriceModelBuilder()
        {
            this.region = "SW";
            this.businessUnitId = 1;
            this.scanCode = "111122223333";
            this.multiple = 1;
            this.price = 3.99M;
            this.startDate = DateTime.Now;
            this.endDate = null;
            this.priceUom = "EA";
            this.priceType = "REG";
            this.currencyCode = "USD";
            this.timestamp = DateTime.Now;
            this.transactionId = Guid.NewGuid();
        }

        internal TestStagingPriceModelBuilder WithRegion(string region)
        {
            this.region = region;
            return this;
        }

        internal TestStagingPriceModelBuilder WithBusinessUnit(int bu)
        {
            this.businessUnitId = bu;
            return this;
        }

        internal TestStagingPriceModelBuilder WithScanCode(string scanCode)
        {
            this.scanCode = scanCode;
            return this;
        }

        internal TestStagingPriceModelBuilder WithMultiple(int multiple)
        {
            this.multiple = multiple;
            return this;
        }

        internal TestStagingPriceModelBuilder WithPrice(decimal price)
        {
            this.price = price;
            return this;
        }

        internal TestStagingPriceModelBuilder WithStartDate(DateTime startDate)
        {
            this.startDate = startDate;
            return this;
        }

        internal TestStagingPriceModelBuilder WithEndDate(DateTime? endDate)
        {
            this.endDate = endDate;
            return this;
        }

        internal TestStagingPriceModelBuilder WithPriceUom(string priceUom)
        {
            this.priceUom = priceUom;
            return this;
        }

        internal TestStagingPriceModelBuilder WithPriceType(string priceType)
        {
            this.priceType = priceType;
            return this;
        }

        internal TestStagingPriceModelBuilder WithCurrencyCode(string currency)
        {
            this.currencyCode = currency;
            return this;
        }

        internal TestStagingPriceModelBuilder WithTimestamp(DateTime timestamp)
        {
            this.timestamp = timestamp;
            return this;
        }

        internal TestStagingPriceModelBuilder WithTransactionId(Guid transactionId)
        {
            this.transactionId = transactionId;
            return this;
        }

        internal StagingPriceModel Build()
        {
            StagingPriceModel price = new StagingPriceModel
            {
                Region = this.region,
                BusinessUnitId = this.businessUnitId,
                ScanCode = this.scanCode,
                Multiple = this.multiple,
                Price = this.price,
                StartDate = this.startDate,
                EndDate = this.endDate,
                PriceType = this.priceType,
                PriceUom = this.priceUom,
                CurrencyCode = this.currencyCode,
                Timestamp = this.timestamp,
                TransactionId = this.transactionId
            };

            return price;
        }
    }
}
