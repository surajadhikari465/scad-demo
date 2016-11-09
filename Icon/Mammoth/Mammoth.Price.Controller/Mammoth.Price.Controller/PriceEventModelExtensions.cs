using Mammoth.Price.Controller.DataAccess.Models;
using Mammoth.Price.Controller.Services;
using System.Collections.Generic;

namespace Mammoth.Price.Controller
{
    public static class PriceEventModelExtensions
    {
        public static IEnumerable<PriceModel> MapToPriceModel(this IEnumerable<PriceEventModel> priceEventData)
        {
            var prices = new List<PriceModel>();

            foreach (var priceEvent in priceEventData)
            {
                //if cancelled sale
                if (priceEvent.CancelAllSales.HasValue && priceEvent.CancelAllSales.Value)
                {
                    // Cancelled Sale Update row
                    PriceModel updatedCancelledSalePrice = BuildCancelledSalePrice(priceEvent);
                    prices.Add(updatedCancelledSalePrice);

                    // add regular price change row if the cancelled sale change the off-sale reg
                    if (priceEvent.NewRegularPrice != priceEvent.CurrentRegularPrice)
                    {
                        PriceModel regPriceChangeForCancelledSale = BuildRegularPrice(priceEvent);
                        prices.Add(regPriceChangeForCancelledSale);
                    }
                }
                //else a new reg or tpr
                else
                {
                    PriceModel price = BuildPrice(priceEvent);
                    prices.Add(price);

                    // add regular price row if the reg price is changing with a sale
                    if (priceEvent.NewSalePrice != null && 
                        (priceEvent.CurrentRegularPrice != priceEvent.NewRegularPrice
                        || priceEvent.CurrentRegularMultiple != priceEvent.NewRegularMultiple))
                    {
                        PriceModel regPrice = BuildRegularPrice(priceEvent);
                        prices.Add(regPrice);
                    }
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
                PriceUom = priceEventModel.PriceUom,
                CancelAllSales = false
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
                PriceUom = priceEventModel.PriceUom,
                CancelAllSales = false
            };

            return regPrice;
        }

        /// <summary>
        /// Builds a TPR for a sale that is cancelled.
        /// The start date of the TPR is the current Sale Start Date.
        /// </summary>
        /// <param name="priceEventModel"></param>
        /// <returns></returns>
        private static PriceModel BuildCancelledSalePrice(PriceEventModel priceEventModel)
        {
            var updatedCancelledSalePrice = new PriceModel
            {
                BusinessUnitId = priceEventModel.BusinessUnitId,
                ScanCode = priceEventModel.ScanCode,
                Region = priceEventModel.Region,
                Multiple = priceEventModel.CurrentSaleMultiple.Value,
                Price = priceEventModel.CurrentSalePrice.Value,
                StartDate = priceEventModel.CurrentSaleStartDate.Value,
                EndDate = priceEventModel.NewStartDate, // confusing but this is how IRMA cancels a sale
                PriceType = priceEventModel.CurrentPriceType,
                CurrencyCode = priceEventModel.CurrencyCode,
                PriceUom = priceEventModel.PriceUom,
                CancelAllSales = priceEventModel.CancelAllSales ?? false
            };

            return updatedCancelledSalePrice;
        }
    }
}
