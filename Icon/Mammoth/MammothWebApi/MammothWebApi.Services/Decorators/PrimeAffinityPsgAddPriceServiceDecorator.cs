using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.PrimeAffinity.Library.Processors;
using MammothWebApi.Common;
using MammothWebApi.DataAccess.Models;
using MammothWebApi.DataAccess.Queries;
using MammothWebApi.Service.Services;
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
    /// - price is not an SAL, FRZ, or ISS with a StartDate of today
    /// - cancel all sales record with StartDate of today
    /// send add to PSG:
    /// - SAL, FRZ, or ISS prices starting today
    /// </summary>
    public class PrimeAffinityPsgAddPriceServiceDecorator : IUpdateService<AddUpdatePrice>
    {
        private IUpdateService<AddUpdatePrice> priceService;
        private IQueryHandler<GetPrimePsgItemDataByScanCodeQuery, IEnumerable<PrimePsgItemStoreDataModel>> getPsgItemDataQuery;
        private IPrimeAffinityPsgProcessor<PrimeAffinityPsgProcessorParameters> primeAffinityPsgProcessor;
        private IPrimeAffinityPsgSettings settings;

        public PrimeAffinityPsgAddPriceServiceDecorator(
            IUpdateService<AddUpdatePrice> priceService,
            IQueryHandler<GetPrimePsgItemDataByScanCodeQuery, IEnumerable<PrimePsgItemStoreDataModel>> getPsgItemDataQuery,
            IPrimeAffinityPsgProcessor<PrimeAffinityPsgProcessorParameters> primeAffinityPsgProcessor,
            IPrimeAffinityPsgSettings settings)
        {
            this.priceService = priceService;
            this.getPsgItemDataQuery = getPsgItemDataQuery;
            this.primeAffinityPsgProcessor = primeAffinityPsgProcessor;
            this.settings = settings;
        }

        public void Handle(AddUpdatePrice data)
        {
            // first add or update prices
            this.priceService.Handle(data);

            if (settings.EnablePrimeAffinityPsgMessages)
            {
                // execute prime psg logic
                List<StoreScanCode> psgAddItemStoreKeys = data.Prices.Where(p =>
                        (p.PriceType == "SAL"
                            || p.PriceType == "FRZ"
                            || p.PriceType == "ISS")
                        && p.StartDate == DateTime.Today)
                    .Select(p => new StoreScanCode { ScanCode = p.ScanCode, BusinessUnitID = p.BusinessUnitId })
                    .Distinct()
                    .ToList();

                List<StoreScanCode> psgDeleteItemStoreKeys = data.Prices.Where(p =>
                        (p.PriceType != "SAL"
                            && p.PriceType != "FRZ"
                            && p.PriceType != "ISS"
                            && p.PriceType != "REG"
                            && p.StartDate == DateTime.Today))
                    .Select(p => new StoreScanCode { ScanCode = p.ScanCode, BusinessUnitID = p.BusinessUnitId })
                    .Distinct()
                    .ToList();

                PsgHelpers.SendPsgsForStoreScanCodes(psgDeleteItemStoreKeys, psgAddItemStoreKeys, getPsgItemDataQuery, primeAffinityPsgProcessor);

            }
        }
    }
}
