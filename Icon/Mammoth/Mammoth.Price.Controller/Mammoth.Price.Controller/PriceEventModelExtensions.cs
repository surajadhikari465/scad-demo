using Mammoth.Price.Controller.DataAccess.Models;
using Mammoth.Price.Controller.Services;
using System.Collections.Generic;
using System.Linq;

namespace Mammoth.Price.Controller
{
    public static class PriceEventModelExtensions
    {
        public static IEnumerable<PriceModel> MapToPriceModel(this IEnumerable<PriceEventModel> priceEventData)
        {
            var prices = new List<PriceModel>();

            foreach (var priceEvent in priceEventData)
            {              
                PriceModel price = BuildPrice(priceEvent);
                prices.Add(price);

                // add regular price row if the reg price is changing with a sale or ItemChangeTypeID has value
                // when a item is deauthorized and then authorized again for store, we need to add reg price to mammoth
                if (priceEvent.ItemChangeTypeID !=null || ( priceEvent.NewSalePrice != null 
                    && (priceEvent.CurrentRegularPrice != priceEvent.NewRegularPrice
                        || priceEvent.CurrentRegularMultiple != priceEvent.NewRegularMultiple)))
                {
                    PriceModel regPrice = BuildRegularPrice(priceEvent);
                    prices.Add(regPrice);
                }
            }

            return prices;
        }

        /// <summary>
        /// Handles building both regular and tpr prices.
        /// This is for new REGs and TPRs
        /// </summary>
        /// <param name="priceEventModel"></param>
        /// <returns>PriceModel</returns>
        private static PriceModel BuildPrice(PriceEventModel priceEventModel)
        {
            var price = new PriceModel
            {
                BusinessUnitId = priceEventModel.BusinessUnitId,
                ScanCode = priceEventModel.ScanCode,
                Region = priceEventModel.Region,
                Multiple = priceEventModel.NewSaleMultiple ?? priceEventModel.NewRegularMultiple,
                Price = priceEventModel.NewSalePrice ?? priceEventModel.NewRegularPrice,
                StartDate = priceEventModel.NewStartDate,
                EndDate = priceEventModel.NewSaleEndDate,
                PriceType = priceEventModel.NewPriceType,
                CurrencyCode = priceEventModel.CurrencyCode,
                PriceUom = priceEventModel.PriceUom
            };

            return price;
        }

        /// <summary>
        /// Builds a regular price.  This is specifically used for edited REGs of Cancelled Sales
        /// and edited REGs when a sale is created.
        /// </summary>
        /// <param name="priceEventModel"></param>
        /// <returns></returns>
        private static PriceModel BuildRegularPrice(PriceEventModel priceEventModel)
        {
            var regPrice = new PriceModel
            {
                BusinessUnitId = priceEventModel.BusinessUnitId,
                ScanCode = priceEventModel.ScanCode,
                Region = priceEventModel.Region,
                Multiple = priceEventModel.NewRegularMultiple,
                Price = priceEventModel.NewRegularPrice,
                StartDate = priceEventModel.NewStartDate,
                EndDate = null,
                PriceType = "REG",
                CurrencyCode = priceEventModel.CurrencyCode,
                PriceUom = priceEventModel.PriceUom
            };

            return regPrice;
        }

        public static IEnumerable<CancelAllSalesModel> MapToCancelAllSalesModel(this IEnumerable<CancelAllSalesEventModel> data)
        {
            return data.Select(m => new CancelAllSalesModel
            {
                BusinessUnitId = m.BusinessUnitId,
                Region = m.Region,
                ScanCode = m.ScanCode,
                EndDate = m.EndDate,
                EventCreatedDate = m.EventCreatedDate
            });
        }
    }
}