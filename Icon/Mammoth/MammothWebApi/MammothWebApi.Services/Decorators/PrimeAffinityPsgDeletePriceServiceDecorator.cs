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
    public class PrimeAffinityPsgDeletePriceServiceDecorator : IUpdateService<DeletePrice>
    {
        private IUpdateService<DeletePrice> priceService;
        private IQueryHandler<GetPrimePsgItemDataByScanCodeQuery, IEnumerable<PrimePsgItemStoreDataModel>> getPsgItemDataQuery;
        private IQueryHandler<GetActivePricesByScanCodeAndStoreQuery, List<ItemPriceModel>> getActivePricesByScanCodeAndStoreQuery;
        private IPrimeAffinityPsgProcessor<PrimeAffinityPsgProcessorParameters> primeAffinityPsgProcessor;
        private IPrimeAffinityPsgSettings settings;

        public PrimeAffinityPsgDeletePriceServiceDecorator(
            IUpdateService<DeletePrice> priceService,
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

        public void Handle(DeletePrice data)
        {
            // first add or update prices
            this.priceService.Handle(data);

            if (settings.EnablePrimeAffinityPsgMessages)
            {
                List<StoreScanCode> psgEligibleItemStoreKeys = GetPsgEligibleItemStoreKeys(data);

                // execute prime psg logic
                List<StoreScanCode> psgDeleteItemStoreKeys = data.Prices.Where(p =>
                        (p.PriceType == "SAL"
                            || p.PriceType == "FRZ"
                            || p.PriceType == "ISS")
                        && p.StartDate == DateTime.Today)
                    .Select(p => new StoreScanCode { ScanCode = p.ScanCode, BusinessUnitID = p.BusinessUnitId })
                    .Distinct()
                    .ToList();
                psgDeleteItemStoreKeys = psgDeleteItemStoreKeys.ExceptBy(psgEligibleItemStoreKeys, k => new { k.BusinessUnitID, k.ScanCode }).ToList();

                PsgHelpers.SendPsgsForStoreScanCodes(psgDeleteItemStoreKeys, psgEligibleItemStoreKeys, getPsgItemDataQuery, primeAffinityPsgProcessor);
            }
        }

        private List<StoreScanCode> GetPsgEligibleItemStoreKeys(DeletePrice data)
        {
            var tprItemStoreKeys = data.Prices
                .Where(p => p.PriceType != "REG"
                    && p.StartDate == DateTime.Today)
                .Select(p => new StoreScanCode { ScanCode = p.ScanCode, BusinessUnitID = p.BusinessUnitId })
                .Distinct()
                .ToList();

            if(tprItemStoreKeys.Count > 0)
            {
                var activePrices = getActivePricesByScanCodeAndStoreQuery.Search(new GetActivePricesByScanCodeAndStoreQuery
                {
                    Region = data.Prices.First().Region,
                    StoreScanCodes = tprItemStoreKeys
                });

                //This LINQ statement returns the most recent active TPRs because we need to send adds for them
                //in nested TPR scenarios
                return activePrices
                    .Where(p => p.PriceType != "REG")
                    .GroupBy(p => new { p.ScanCode, p.BusinessUnitId })
                    .SelectMany(
                        g => g.OrderByDescending(p => p.StartDate)
                        .Take(1))
                    .Where(p => p.PriceType == "SAL"
                        || p.PriceType == "FRZ"
                        || p.PriceType == "ISS")
                    .Join(
                        tprItemStoreKeys,
                        o => new { ScanCode = o.ScanCode, BusinessUnitId = o.BusinessUnitId },
                        i => new { ScanCode = i.ScanCode, BusinessUnitId = i.BusinessUnitID },
                        (o, i) => i)
                    .ToList();
            }
            else
            {
                return new List<StoreScanCode>();
            }
        }        
    }
}
