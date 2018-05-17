using Icon.Esb.Schemas.Wfm.Contracts;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.PrimeAffinity.Library.MessageBuilders;
using Mammoth.PrimeAffinity.Library.Processors;
using MammothWebApi.DataAccess.Models;
using MammothWebApi.DataAccess.Queries;
using System.Collections.Generic;
using System.Linq;

namespace MammothWebApi.Service
{
    public static class PsgHelpers
    {
        public static void SendPsgsForStoreScanCodes(
            List<StoreScanCode> psgDeleteItemStoreKeys,
            List<StoreScanCode> psgAddItemStoreKeys,
            IQueryHandler<GetPrimePsgItemDataByScanCodeQuery, IEnumerable<PrimePsgItemStoreDataModel>> getPsgItemDataQuery,
            IPrimeAffinityPsgProcessor<PrimeAffinityPsgProcessorParameters> primeAffinityPsgProcessor)
        {
            if (psgDeleteItemStoreKeys.Count > 0 || psgAddItemStoreKeys.Count > 0)
            {
                // get data from db necessary for psg, e.g. ItemID, ItemTypeCode, and StoreName
                var primePsgData = getPsgItemDataQuery.Search(new GetPrimePsgItemDataByScanCodeQuery
                {
                    StoreScanCodes = psgDeleteItemStoreKeys.Union(psgAddItemStoreKeys)
                });

                // psg Adds
                var primePsgAddData = psgAddItemStoreKeys.Join(
                    primePsgData,
                    k => new { BusinessUnitID = k.BusinessUnitID, ScanCode = k.ScanCode },
                    d => new { BusinessUnitID = d.BusinessUnitId, ScanCode = d.ScanCode },
                    (k, d) => new PrimePsgItemStoreDataModel
                    {
                        BusinessUnitId = d.BusinessUnitId,
                        ItemId = d.ItemId,
                        ItemTypeCode = d.ItemTypeCode,
                        PsSubTeamNumber = d.PsSubTeamNumber,
                        ScanCode = d.ScanCode,
                        StoreName = d.StoreName,
                        Region = d.Region
                    });

                if (primePsgAddData.Any())
                {
                    var primeAffinityMessageModels = BuildPrimeAffinityPsgMessage(primePsgAddData, ActionEnum.AddOrUpdate);
                    primeAffinityPsgProcessor.SendPsgs(new PrimeAffinityPsgProcessorParameters
                    {
                        MessageAction = ActionEnum.AddOrUpdate,
                        PrimeAffinityMessageModels = primeAffinityMessageModels,
                        Region = primeAffinityMessageModels.First().Region
                    });
                }

                // psg Deletes
                var primePsgDeletes = psgDeleteItemStoreKeys.Join(
                    primePsgData,
                    k => new { BusinessUnitID = k.BusinessUnitID, ScanCode = k.ScanCode, },
                    d => new { BusinessUnitID = d.BusinessUnitId, ScanCode = d.ScanCode },
                    (k, d) => new PrimePsgItemStoreDataModel
                    {
                        BusinessUnitId = d.BusinessUnitId,
                        ItemId = d.ItemId,
                        ItemTypeCode = d.ItemTypeCode,
                        PsSubTeamNumber = d.PsSubTeamNumber,
                        ScanCode = d.ScanCode,
                        StoreName = d.StoreName,
                        Region = d.Region
                    });

                if (primePsgDeletes.Any())
                {
                    var primeAffinityMessageModels = BuildPrimeAffinityPsgMessage(primePsgDeletes, ActionEnum.Delete);
                    primeAffinityPsgProcessor.SendPsgs(new PrimeAffinityPsgProcessorParameters
                    {
                        MessageAction = ActionEnum.Delete,
                        PrimeAffinityMessageModels = primeAffinityMessageModels,
                        Region = primeAffinityMessageModels.First().Region
                    });
                }
            }
        }

        private static IEnumerable<PrimeAffinityMessageModel> BuildPrimeAffinityPsgMessage(IEnumerable<PrimePsgItemStoreDataModel> prices, ActionEnum action)
        {
            var primeAffinityPsgModels = prices.Select(p => new PrimeAffinityMessageModel
            {
                BusinessUnitID = p.BusinessUnitId,
                ItemID = p.ItemId,
                ItemTypeCode = p.ItemTypeCode,
                MessageAction = action,
                ScanCode = p.ScanCode,
                StoreName = p.StoreName,
                Region = p.Region,
                InternalPriceObject = new { Source = "Mammoth Web API", SourceObject = p }
            });

            return primeAffinityPsgModels;
        }
    }
}
