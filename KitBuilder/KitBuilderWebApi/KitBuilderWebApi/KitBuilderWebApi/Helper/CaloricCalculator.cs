using KitBuilder.DataAccess.Dto;
using KitBuilderWebApi.QueryParameters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using KitBuilder.DataAccess.Repository;
using KitBuilder.DataAccess.DatabaseModels;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace KitBuilderWebApi.Helper
{
    public class CaloricCalculator
    {
        private int kitLocaleId;
        private int storeLocaleId;
        private IRepository<KitLocale> kitLocaleRepository;
        private ILogger<CaloricCalculator> logger;

        public CaloricCalculator(int kitLocaleId, int storeLocaleId, IRepository<KitLocale> kitLocaleRepository, ILogger<CaloricCalculator> logger)
        {
            this.logger = logger;
            this.kitLocaleId = kitLocaleId;
            this.storeLocaleId = storeLocaleId;
            this.kitLocaleRepository = kitLocaleRepository;
        }

        public async Task<KitLocaleDto> Run()
        {
            KitLocaleDto kitLocaledto = GetKitByKitLocaleId(kitLocaleId);
            var scanCode = from k in kitLocaledto.KitLinkGroupLocale
                          .SelectMany(s => s.KitLinkGroupItemLocales.ToList())
                          .Select(r => r.KitLinkGroupItem.LinkGroupItem.Item.ScanCode)
                          select k;
            int businessUnitID = 10;
            List<StoreItem> storeItemsList = new List<StoreItem>();

            foreach (string scancode in scanCode)
            {
                storeItemsList.Add(new StoreItem()
                {
                    BusinessUnitId = businessUnitID,
                    ScanCode = scancode
                }

                );
            }

            var result = GetAuthorizedStatus(storeItemsList);
            var itemStorePriceModelList = await result;
            
            //update kit locale
            foreach(ItemStorePriceModel itemStorePriceModel in itemStorePriceModelList)
            {
                var kitLinkGroupItemDtos = kitLocaledto.KitLinkGroupItemLocale.Where(s => s.KitLinkGroupItem.LinkGroupItem.ItemId == itemStorePriceModel.ItemId).Select(s=>s.KitLinkGroupItem;
                foreach(KitLinkGroupItemDto kitLinkGroupItemDto in kitLinkGroupItemDtos)
                {
                    // kitLinkGroupItemDto.   itemStorePriceModel.Price;
                }
            }
        }

        internal KitLocaleDto GetKitByKitLocaleId(int kitLocaleId)
        {
           var kitLocale= ((kitLocaleRepository.UnitOfWork.Context.KitLocale.Where(kl => kl.KitLocaleId == kitLocaleId)
                    .Include(kll => kll.KitLinkGroupLocale).ThenInclude(k => k.KitLinkGroupItemLocale)
                    .ThenInclude(i => i.KitLinkGroupItem).ThenInclude(i => i.LinkGroupItem)
                    .ThenInclude(i => i.Item)).FirstOrDefault());

            return Mapper.Map<KitLocaleDto>(kitLocale);
        }

        internal async Task<IEnumerable<ItemStorePriceModel>> GetAuthorizedStatus(ICollection<StoreItem> storeItems)
        {
            string url = "http://mammoth-test/api/price/";

            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    IEnumerable<ItemStorePriceModel> itemStorePrices = await response.Content.ReadAsAsync<IEnumerable<ItemStorePriceModel>>();
                    return itemStorePrices;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
