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

			//Get each modifier's Authorization Status and REG price 
			List<StoreItem> storeItemsList = ParseParametersForPriceCall(kitLocaleDto);
			var resultPrice = GetAuthorizedStatus(storeItemsList);
			var itemStorePriceModelList = await resultPrice;

			//update kitlocale with Price and Store Authorization Status
			UpdateKitLocaleForPrice(kitLocaleDto, itemStorePriceModelList);

			//Get each modifier's caloric info
			var itemIds = from k in kitLocaleDto.KitLinkGroupLocale
						  .SelectMany(s => s.KitLinkGroupItemLocales.ToList())
						  .Select(r => r.KitLinkGroupItem.LinkGroupItem.ItemId)
						  select k;

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

		internal List<StoreItem> ParseParametersForPriceCall(KitLocaleDto kitLocaleDto)
		{
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

			return storeItemsList;
		}

		internal void UpdateKitLocaleForPrice(KitLocaleDto kitLocaleDto, IEnumerable<ItemStorePriceModel> itemStorePriceModelList)
		{
			foreach (ItemStorePriceModel itemStorePriceModel in itemStorePriceModelList)
			{
				var kitLinkGroupItemLocaleDtos = from kitLinkGroupLocales in kitLocaleDto.KitLinkGroupLocale
												 from kitLinkGroupItemLocales in kitLinkGroupLocales.KitLinkGroupItemLocales
												 where (kitLinkGroupItemLocales.KitLinkGroupItem.LinkGroupItem.ItemId == itemStorePriceModel.ItemId)
												 select kitLinkGroupItemLocales;

				foreach (KitLinkGroupItemLocaleDto kitLinkGroupItemLocaleDto in kitLinkGroupItemLocaleDtos)
				{
					kitLinkGroupItemLocaleDto.ItemId = itemStorePriceModel.ItemId;
					kitLinkGroupItemLocaleDto.RegularPrice = itemStorePriceModel.Price;
					kitLinkGroupItemLocaleDto.Exclude = !itemStorePriceModel.Authorized;
				}
			}
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

		//internal async Task<IEnumerable<ItemNutritionAttributes>> GetCaloricInfo(List<int> itemIds)
		//{
		//	string url = "http://mammoth-test/api/itemNutrition/";
		//	//string url = "http://localhost:30680/api/price/";

		//	string pricesRequestJson = JsonConvert.SerializeObject(itemIds);
		//	HttpContent inputContent = new StringContent(pricesRequestJson, Encoding.UTF8, "application/json");

		//	ApiHelper.InitializeClient(url);

		//	try
		//	{
		//		using (HttpResponseMessage response = await ApiHelper.ApiClient.PostAsync(url, inputContent))
		//		{
		//			if (response.IsSuccessStatusCode)
		//			{
		//				IEnumerable<ItemNutritionAttributes> itemCaloricInfo = await response.Content.ReadAsAsync<IEnumerable<ItemNutritionAttributes>>();
		//				return itemCaloricInfo;
		//			}
		//			else
		//			{
		//				throw new Exception(response.ReasonPhrase);
		//			}
		//		}
		//	}
		//	catch (Exception e)
		//	{
		//		throw new Exception(e.Message);
		//	}
		//}
	}
}