using System;
using System.Collections.Generic;
using System.Linq;

using Mammoth.Common.DataAccess.CommandQuery;
using MammothWebApi.DataAccess.Models;
using MammothWebApi.DataAccess.Queries;
using MammothWebApi.Service.Models;
using MammothWebApi.Service.Services;

namespace MammothWebApi.Service.Decorators
{
    public class CancelAllSalesAddUpdatePriceServiceDecorator : IService<AddUpdatePrice>
    {
        private const string RegPriceType = "REG";

        private IService<AddUpdatePrice> priceService;
        private IQueryHandler<GetPricesByScanCodeAndStoreQuery, List<ItemPriceModel>> getPricesByScanCodeAndStoreQuery;

        public CancelAllSalesAddUpdatePriceServiceDecorator(IService<AddUpdatePrice> priceService,
            IQueryHandler<GetPricesByScanCodeAndStoreQuery, List<ItemPriceModel>> getPricesByScanCodeAndStoreQuery)
        {
            this.priceService = priceService;
            this.getPricesByScanCodeAndStoreQuery = getPricesByScanCodeAndStoreQuery;
        }

        public void Handle(AddUpdatePrice data)
        {
            var stackedPricesToCancel = this.CreateCancelAllSales(data);
            data.Prices.AddRange(stackedPricesToCancel);

            this.priceService.Handle(data);
        }

        private List<PriceServiceModel> CreateCancelAllSales(AddUpdatePrice priceData)
        {
            var stackedPricesToCancel = new List<PriceServiceModel>();
            var pricesToCancelAllSales = priceData.Prices.Where(p => p.CancelAllSales).ToList();
            if(pricesToCancelAllSales.Any())
            {
                var stackedSalePrices = this.GetRelatedSalePrices(pricesToCancelAllSales);

                foreach(var p in pricesToCancelAllSales)
                {
                    // Create related stacked prices to cancel sale.
                    stackedSalePrices.Where(sp => IsRelatedStackedPrice(p, sp))
                        .Select(sp => this.CreateRelatedStackPriceToUpdate(sp, p.EndDate.Value))
                        .ToList()
                        .ForEach(stackedPricesToCancel.Add);                    
                }

                return stackedPricesToCancel;
            }
            else
            {
                return Enumerable.Empty<PriceServiceModel>().ToList();
            }
        }
        
        private List<ItemPriceModel> GetRelatedSalePrices(List<PriceServiceModel> pricesToCancelAllSales)
        {
            // Get Data from the Database
            var relatedStackedPrices = new List<ItemPriceModel>();
            var pricesByRegion = pricesToCancelAllSales.GroupBy(p => p.Region);

            foreach (var pbr in pricesByRegion)
            {
                var queryParams = new GetPricesByScanCodeAndStoreQuery
                {
                    Region = pbr.Key,
                    BusinessUnitIds = pbr.Select(p => p.BusinessUnitId).Distinct().ToList(),
                    ScanCodes = pbr.Select(p => p.ScanCode).Distinct().ToList()
                };

                var pricesByScanCode = this.getPricesByScanCodeAndStoreQuery.Search(queryParams);
                relatedStackedPrices.AddRange(pricesByScanCode);
            }

            return relatedStackedPrices.Where(p => p.PriceType != RegPriceType).ToList();
        }

        /// <summary>
        /// Filters the stacked prices so it doesn't add an additonal update.
        /// </summary>
        private bool IsRelatedStackedPrice(PriceServiceModel psm, ItemPriceModel ipm)
        {
            return psm.BusinessUnitId == ipm.BusinessUnitId
                && psm.ScanCode == ipm.ScanCode
                && (psm.PriceType != ipm.PriceType || psm.StartDate != ipm.StartDate)
                && ipm.StartDate <= DateTime.Today
                && ipm.EndDate >= DateTime.Today;
        }

        private PriceServiceModel CreateRelatedStackPriceToUpdate(ItemPriceModel stackedPrice, DateTime endDate)
        {
            return new PriceServiceModel
            {
                BusinessUnitId = stackedPrice.BusinessUnitId,
                CancelAllSales = true,
                CurrencyCode = stackedPrice.CurrencyCode,
                EndDate = endDate, // Update EndDate to the EndDate of original Cancel All Sales price row
                Multiple = stackedPrice.Multiple,
                Price = stackedPrice.Price,
                PriceType = stackedPrice.PriceType,
                PriceUom = stackedPrice.PriceUom,
                Region = stackedPrice.Region,
                ScanCode = stackedPrice.ScanCode,
                StartDate = stackedPrice.StartDate
            };
        }
    }
}
