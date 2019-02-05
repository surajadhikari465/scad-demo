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
using Newtonsoft.Json;
using System.Text;

namespace KitBuilderWebApi.Helper
{
	public class CaloricCalculator
	{
		private int kitLocaleId;
		private int storeLocaleId;
		private IRepository<KitLocale> kitLocaleRepository;
		private IRepository<Locale> localeRepository;
		private ILogger<CaloricCalculator> logger;

		public CaloricCalculator(int kitLocaleId,
							int storeLocaleId,
							IRepository<KitLocale> kitLocaleRepository,
							IRepository<Locale> localeRepository)
		{
			this.kitLocaleId = kitLocaleId;
			this.storeLocaleId = storeLocaleId;
			this.kitLocaleRepository = kitLocaleRepository;
			this.localeRepository = localeRepository;

		}

		public async Task<KitLocaleDto> Run()
		{
			KitLocaleDto kitLocaleDto = GetKitByKitLocaleId(kitLocaleId);
			var scanCode = from k in kitLocaleDto.KitLinkGroupLocale
						  .SelectMany(s => s.KitLinkGroupItemLocales.ToList())
						  .Select(r => r.KitLinkGroupItem.LinkGroupItem.Item.ScanCode)
						   select k;

			var storeBusinessUnitId = localeRepository.UnitOfWork.Context.Locale.Where(s => s.LocaleId == storeLocaleId).Select(s => s.BusinessUnitId).FirstOrDefault();
			int businessUnitId = storeBusinessUnitId == null ? 0 : (int)storeBusinessUnitId;

			List<StoreItem> storeItemsList = new List<StoreItem>();

			foreach (string scancode in scanCode)
			{
				storeItemsList.Add(new StoreItem()
				{
					BusinessUnitId = businessUnitId,
					ScanCode = scancode
				});
			}

			var result = GetAuthorizedStatus(storeItemsList);
			var itemStorePriceModelList = await result;

			//update kit locale
			foreach (ItemStorePriceModel itemStorePriceModel in itemStorePriceModelList)
			{
				var kitLinkGroupItemLocaleDtos = (kitLocaleDto.KitLinkGroupLocale.Select(klli => klli.KitLinkGroupItemLocales)).Select(s => s.Where(i =>i.KitLinkGroupItem.LinkGroupItem.ItemId == itemStorePriceModel.ItemId)).ToList();
				//var kitLinkGroupItemLocaleDtos = kitLocaleDto.KitLinkGroupItemLocale.Where(s => s.KitLinkGroupItem.LinkGroupItem.ItemId == itemStorePriceModel.ItemId).ToList();
				
				foreach (KitLinkGroupItemLocaleDto kitLinkGroupItemLocaleDto in kitLinkGroupItemLocaleDtos.FirstOrDefault())
				{
					//kitLinkGroupItemDto.   itemStorePriceModel.Price;

					kitLinkGroupItemLocaleDto.ItemId = itemStorePriceModel.ItemId;
					kitLinkGroupItemLocaleDto.RegularPrice = itemStorePriceModel.Price;
					kitLinkGroupItemLocaleDto.Exclude = !itemStorePriceModel.Authorized;
				}
			}

			return kitLocaleDto;

		}

		internal KitLocaleDto GetKitByKitLocaleId(int kitLocaleId)
		{
			var kitLocale = (kitLocaleRepository.UnitOfWork.Context.KitLocale.Where(kl => kl.KitLocaleId == kitLocaleId)
					 .Include(kll => kll.KitLinkGroupLocale).ThenInclude(k => k.KitLinkGroupItemLocale)
					 .ThenInclude(i => i.KitLinkGroupItem).ThenInclude(i => i.LinkGroupItem)
					 .ThenInclude(i => i.Item)).FirstOrDefault();

			try
			{
				KitLocaleDto kitLocaleDto = Mapper.Map<KitLocaleDto>(kitLocale);
			}
			catch (Exception e)
			{
				throw new Exception(e.Message);
			}

			return Mapper.Map<KitLocaleDto>(kitLocale);
		}

		internal async Task<IEnumerable<ItemStorePriceModel>> GetAuthorizedStatus(IEnumerable<StoreItem> storeItems)
		{
			string url = "http://mammoth-test/api/price/";
			//string url = "http://localhost:30680/api/price/";

			PriceCollectionRequestModel pricesRequestModel = new PriceCollectionRequestModel
			{
				StoreItems = storeItems,
				IncludeFuturePrices = false,
				PriceType = "REG"
			};

			string pricesRequestJson = JsonConvert.SerializeObject(pricesRequestModel);
			HttpContent inputContent = new StringContent(pricesRequestJson, Encoding.UTF8, "application/json");

			ApiHelper.InitializeClient(url);

			try
			{
				using (HttpResponseMessage response = await ApiHelper.ApiClient.PostAsync(url, inputContent))
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
			catch (Exception e)
			{
				throw new Exception(e.Message);
			}
		}
	}
}