using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.PrimeAffinity.Library.Processors;
using MammothWebApi.Common;
using MammothWebApi.DataAccess.Models;
using MammothWebApi.DataAccess.Queries;
using MammothWebApi.Service.Services;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MammothWebApi.Service.Decorators
{
    /// <summary>
    /// This decorator will execute logic to send add or delete Prime PSG messages.
    /// First the price service executes then the Prime PSG logic will occur afterwards
    /// 
    /// send delete from PSG:
    /// - for price in SAL, ISS, or FRC when the StartDate is today
    /// - and when any outer active TPR is not an SAL, ISS, or FRC
    /// send add to PSG:
    /// - for prices when any outer active TPR is an SAL, ISS, or FRC
    /// </summary>
    public class PrimeAffinityPsgCancelAllSalesPriceServiceDecorator : IUpdateService<CancelAllSales>
    {
        private IUpdateService<CancelAllSales> priceService;
        private IQueryHandler<GetPrimePsgItemDataByScanCodeQuery, IEnumerable<PrimePsgItemStoreDataModel>> getPsgItemDataQuery;
        private IQueryHandler<GetActivePricesByScanCodeAndStoreQuery, List<ItemPriceModel>> getActivePricesByScanCodeAndStoreQuery;
        private IPrimeAffinityPsgProcessor<PrimeAffinityPsgProcessorParameters> primeAffinityPsgProcessor;
        private IPrimeAffinityPsgSettings settings;

        public PrimeAffinityPsgCancelAllSalesPriceServiceDecorator(
            IUpdateService<CancelAllSales> priceService,
            IQueryHandler<GetPrimePsgItemDataByScanCodeQuery, IEnumerable<PrimePsgItemStoreDataModel>> getPsgItemDataQuery,
            IQueryHandler<GetActivePricesByScanCodeAndStoreQuery, List<ItemPriceModel>> getActivePricesByScanCodeAndStoreQuery,
            IPrimeAffinityPsgProcessor<PrimeAffinityPsgProcessorParameters> primeAffinityPsgProcessor,
            IPrimeAffinityPsgSettings settings)
        {
            this.priceService = priceService;
            this.getPsgItemDataQuery = getPsgItemDataQuery;
            this.getActivePricesByScanCodeAndStoreQuery = getActivePricesByScanCodeAndStoreQuery;
            this.primeAffinityPsgProcessor = primeAffinityPsgProcessor;
            this.settings = settings;
        }

        public void Handle(CancelAllSales data)
        {
            // first add or update prices
            this.priceService.Handle(data);

            if (settings.EnablePrimeAffinityPsgMessages)
            {
                List<StoreScanCode> psgDeleteItemStoreKeys = data.CancelAllSalesData
                    .Where(p => p.EndDate == DateTime.Today)
                    .Select(p => new StoreScanCode { ScanCode = p.ScanCode, BusinessUnitID = p.BusinessUnitId })
                    .Distinct()
                    .ToList();

                PsgHelpers.SendPsgsForStoreScanCodes(psgDeleteItemStoreKeys, new List<StoreScanCode>(), getPsgItemDataQuery, primeAffinityPsgProcessor);
            }
        }     
    }
}
