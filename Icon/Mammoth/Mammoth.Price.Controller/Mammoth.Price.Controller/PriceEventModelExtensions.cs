using Mammoth.Common;
using Mammoth.Price.Controller.DataAccess.Models;
using Mammoth.Price.Controller.Services;
using Newtonsoft.Json;
using System;
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
            string missingCancelledSaleProperty = ValidateCancelledSalePrice(priceEventModel);
            if (!String.IsNullOrEmpty(missingCancelledSaleProperty))
            {
                ArgumentException argumentException = new ArgumentException(String.Format("Missing a property for the Cancelled Sale: {0}", missingCancelledSaleProperty));
                priceEventModel.ErrorMessage = "ArgumentException";
                priceEventModel.ErrorDetails = argumentException.ToString();
                priceEventModel.ErrorSource = Constants.SourceSystem.MammothPriceController;
                throw argumentException;
            }   

            var updatedCancelledSalePrice = new PriceModel();
            updatedCancelledSalePrice.BusinessUnitId = priceEventModel.BusinessUnitId;
            updatedCancelledSalePrice.ScanCode = priceEventModel.ScanCode;
            updatedCancelledSalePrice.Region = priceEventModel.Region;
            updatedCancelledSalePrice.Multiple = priceEventModel.CurrentSaleMultiple.Value;
            updatedCancelledSalePrice.Price = priceEventModel.CurrentSalePrice.Value;
            updatedCancelledSalePrice.StartDate = priceEventModel.CurrentSaleStartDate.Value;
            updatedCancelledSalePrice.EndDate = priceEventModel.NewStartDate; // confusing but this is how IRMA cancels a sale
            updatedCancelledSalePrice.PriceType = priceEventModel.CurrentPriceType;
            updatedCancelledSalePrice.CurrencyCode = priceEventModel.CurrencyCode;
            updatedCancelledSalePrice.PriceUom = priceEventModel.PriceUom;
            updatedCancelledSalePrice.CancelAllSales = priceEventModel.CancelAllSales ?? false;

            return updatedCancelledSalePrice;
        }

        private static string ValidateCancelledSalePrice(PriceEventModel priceEventModel)
        {
            if (!priceEventModel.CurrentSaleMultiple.HasValue)
                return "CurrentSaleMultiple is null";
            if (!priceEventModel.CurrentSalePrice.HasValue)
                return "CurrentSalePrice is null";
            if (!priceEventModel.CurrentSaleStartDate.HasValue)
                return "CurrentSaleStartDate is null";

            return String.Empty;
        }
    }
}
