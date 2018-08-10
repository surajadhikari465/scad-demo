using Mammoth.Common.DataAccess.CommandQuery;
using MammothWebApi.DataAccess.Models;
using MammothWebApi.DataAccess.Queries;
using MammothWebApi.Service.Models;
using System.Collections.Generic;
using System.Linq;

namespace MammothWebApi.Service.Services
{
    public class GetItemStorePriceAttributesService : IQueryService<GetItemStorePriceAttributes, IEnumerable<ItemStorePriceModel>>
    {
        private IQueryHandler<GetItemPriceAttributesByStoreAndScanCodeQuery, IEnumerable<ItemStorePriceModel>> getItemPriceQueryHandler;
        private IQueryHandler<GetLocalesByBusinessUnitsQuery, IEnumerable<Locales>> getLocalesQueryHandler;

        public GetItemStorePriceAttributesService(
            IQueryHandler<GetItemPriceAttributesByStoreAndScanCodeQuery, IEnumerable<ItemStorePriceModel>> getItemPriceQueryHandler,
            IQueryHandler<GetLocalesByBusinessUnitsQuery, IEnumerable<Locales>> getLocalesQueryHandler)
        {
            this.getItemPriceQueryHandler = getItemPriceQueryHandler;
            this.getLocalesQueryHandler = getLocalesQueryHandler;
        }

        public IEnumerable<ItemStorePriceModel> Get(GetItemStorePriceAttributes data)
        {
            List<ItemStorePriceModel> itemStorePrices = new List<ItemStorePriceModel>();

            // Get Locales so we can map BusinessUnitID to Region
            var getLocalesParameters = new GetLocalesByBusinessUnitsQuery { BusinessUnits = data.ItemStores.Select(s => s.BusinessUnitId) };
            IEnumerable<Locales> locales = this.getLocalesQueryHandler.Search(getLocalesParameters);

            // Populate a Dictionary by region
            Dictionary<string, List<StoreScanCodeServiceModel>> regionToItems = locales
                .Join(data.ItemStores,
                    l => l.BusinessUnitID,
                    itm => itm.BusinessUnitId,
                    (l, itm) => new StoreScanCodeServiceModel
                    {
                        Region = l.Region,
                        BusinessUnitId = itm.BusinessUnitId,
                        ScanCode = itm.ScanCode
                    })
                .GroupBy(g => g.Region)
                .ToDictionary(d => d.Key, i => i.ToList());

            foreach (var row in regionToItems)
            {
                GetItemPriceAttributesByStoreAndScanCodeQuery param = new GetItemPriceAttributesByStoreAndScanCodeQuery
                {
                    Region = row.Key,
                    EffectiveDate = data.EffectiveDate,
                    IncludeFuturePrices = data.IncludeFuturePrices,
                    StoreScanCodeCollection = row.Value.Select(v => new StoreScanCode { BusinessUnitID = v.BusinessUnitId, ScanCode = v.ScanCode }),
                    PriceType = data.PriceType
                };

                var currentPrices = this.getItemPriceQueryHandler.Search(param);
                itemStorePrices.AddRange(currentPrices);
            }

            return itemStorePrices;
        }
    }
}
