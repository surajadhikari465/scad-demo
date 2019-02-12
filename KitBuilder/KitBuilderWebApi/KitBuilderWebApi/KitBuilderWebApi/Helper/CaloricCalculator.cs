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
using Microsoft.Extensions.Configuration;

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
							IRepository<Locale> localeRepository,
							ILogger<CaloricCalculator> logger)
		{
			this.kitLocaleId = kitLocaleId;
			this.storeLocaleId = storeLocaleId;
			this.kitLocaleRepository = kitLocaleRepository;
			this.localeRepository = localeRepository;
			this.logger = logger;
		}

		public async Task<KitLocaleDto> Run()
		{
			KitLocaleDto kitLocaleDto = GetKitByKitLocaleId(kitLocaleId);

			if (kitLocaleDto != null)
			{
				//Get each modifier's Authorization Status and REG price 
				List<StoreItem> storeItemsList = ParseParametersForPriceCall(kitLocaleDto);
				var resultPrice = GetAuthorizedStatus(storeItemsList);
				var itemStorePriceModelList = await resultPrice;

				//update kitlocale with Price and Store Authorization Status
				UpdateKitLocaleForPrice(kitLocaleDto, itemStorePriceModelList);

				//Get each modifier's caloric info
				ItemNutritionRequestModel itemIds = ParseParametersForNutritionCall(kitLocaleDto);

				var resultCalories = GetCaloricInfo(itemIds);
				var itemCaloriesList = await resultCalories;

				// update kitlocale with Nutrition info
				UpdateKitLocaleForNutrition(kitLocaleDto, itemCaloriesList);

				SetMaxCalories(kitLocaleDto);
			}
			return kitLocaleDto;

		}

		internal KitLocaleDto GetKitByKitLocaleId(int kitLocaleId)
		{
			var kitLocale = (kitLocaleRepository.UnitOfWork.Context.KitLocale.Where(kl => kl.KitLocaleId == kitLocaleId)
					 .Include(k => k.Kit).ThenInclude(i => i.Item)
					 .Include(kll => kll.KitLinkGroupLocale).ThenInclude(k => k.KitLinkGroupItemLocale)
					 .ThenInclude(i => i.KitLinkGroupItem).ThenInclude(i => i.LinkGroupItem)
					 .ThenInclude(i => i.Item)).FirstOrDefault();

			return Mapper.Map<KitLocaleDto>(kitLocale);
		}

		internal List<StoreItem> ParseParametersForPriceCall(KitLocaleDto kitLocaleDto)
		{
			var scanCodes = (from k in kitLocaleDto.KitLinkGroupLocale
						  .SelectMany(s => s.KitLinkGroupItemLocales.ToList())
						  .Select(r => r.KitLinkGroupItem.LinkGroupItem.Item.ScanCode)
							select k).ToList();

			scanCodes.Add(kitLocaleDto.Kit.Item.ScanCode);

			var storeBusinessUnitId = localeRepository.UnitOfWork.Context.Locale.Where(s => s.LocaleId == storeLocaleId).Select(s => s.BusinessUnitId).FirstOrDefault();
			int businessUnitId = storeBusinessUnitId == null ? 0 : (int)storeBusinessUnitId;

			List<StoreItem> storeItemsList = new List<StoreItem>();

			foreach (string scancode in scanCodes)
			{
				storeItemsList.Add(new StoreItem()
				{
					BusinessUnitId = businessUnitId,
					ScanCode = scancode
				});
			}

			return storeItemsList;
		}

		internal ItemNutritionRequestModel ParseParametersForNutritionCall(KitLocaleDto kitLocaleDto)
		{
			var itemIds = (from k in kitLocaleDto.KitLinkGroupLocale
						  .SelectMany(s => s.KitLinkGroupItemLocales.ToList())
						  .Select(r => r.KitLinkGroupItem.LinkGroupItem.ItemId)
						   select k).ToList();
			itemIds.Add(kitLocaleDto.Kit.ItemId);

			ItemNutritionRequestModel itemIdList = new ItemNutritionRequestModel();

			itemIdList.ItemIds = itemIds.AsEnumerable();

			return itemIdList;
		}
		internal void UpdateKitLocaleForPrice(KitLocaleDto kitLocaleDto, IEnumerable<ItemStorePriceModel> itemStorePriceModelList)
		{
			foreach (ItemStorePriceModel itemStorePriceModel in itemStorePriceModelList)
			{
				if (itemStorePriceModel.ItemId == kitLocaleDto.Kit.ItemId)
				{
					kitLocaleDto.AuthorizedByStore = itemStorePriceModel.Authorized;
				}

				var kitLinkGroupItemLocaleDtos = from kitLinkGroupLocales in kitLocaleDto.KitLinkGroupLocale
												 from kitLinkGroupItemLocales in kitLinkGroupLocales.KitLinkGroupItemLocales
												 where (kitLinkGroupItemLocales.KitLinkGroupItem.LinkGroupItem.ItemId == itemStorePriceModel.ItemId)
												 select kitLinkGroupItemLocales;

				foreach (KitLinkGroupItemLocaleDto kitLinkGroupItemLocaleDto in kitLinkGroupItemLocaleDtos)
				{
					kitLinkGroupItemLocaleDto.ItemId = itemStorePriceModel.ItemId;
					kitLinkGroupItemLocaleDto.RegularPrice = itemStorePriceModel.Price;
					kitLinkGroupItemLocaleDto.AuthorizedByStore = itemStorePriceModel.Authorized;
				}
			}
		}

		internal void UpdateKitLocaleForNutrition(KitLocaleDto kitLocaleDto, IEnumerable<ItemNutritionAttributesDictionary> itemCaloriesList)
		{
			foreach (ItemNutritionAttributesDictionary itemNutritionModel in itemCaloriesList)
			{
				if (itemNutritionModel.Key == kitLocaleDto.Kit.ItemId)
				{
					kitLocaleDto.MinimumCalories = itemNutritionModel.Value.Calories;
				}

				var kitLinkGroupItemLocaleDtos = from kitLinkGroupLocales in kitLocaleDto.KitLinkGroupLocale
												 from kitLinkGroupItemLocales in kitLinkGroupLocales.KitLinkGroupItemLocales
												 where (kitLinkGroupItemLocales.ItemId == itemNutritionModel.Key)
												 select kitLinkGroupItemLocales;

				foreach (KitLinkGroupItemLocaleDto kitLinkGroupItemLocaleDto in kitLinkGroupItemLocaleDtos)
				{
					kitLinkGroupItemLocaleDto.Calories = itemNutritionModel.Value.Calories;
				}
			}
		}

		internal void SetMaxCalories(KitLocaleDto kitLocaleDto)
		{
			foreach (KitLinkGroupLocaleDto kitLinkGroupDto in kitLocaleDto.KitLinkGroupLocale.Where(i => i.Exclude == false)
			{
				dynamic kitLinkGroupProperties = JsonConvert.DeserializeObject(kitLinkGroupDto.Properties);
				int kitLinkGroupMaxCalories = 0;
				int kitLinkGroupMaxPortion = kitLinkGroupProperties.Maximum;
				//int kitLinkGroupItemMaxCalories = 0;
				//int modifierCount = kitLocaleDto.KitLinkGroupLocale == null ? 0 : kitLocaleDto.KitLinkGroupLocale.Count();
				int arrayIndex = 0;
				int modifierCounter = kitLinkGroupDto.KitLinkGroupItemLocales.Count();
				int[,] modifierMax = new int[modifierCounter, 2];

				foreach (KitLinkGroupItemLocaleDto kitLinkGroupItemLocaleDto in kitLinkGroupDto.KitLinkGroupItemLocales.Where(i => i.Exclude == false && i.AuthorizedByStore == true))
				{
					dynamic kitLinkGroupItemProperties = JsonConvert.DeserializeObject(kitLinkGroupItemLocaleDto.Properties);
					int kitLinkGroupItemMax = kitLinkGroupItemProperties.Maximum;
					int kitLinkGroupItemMin = kitLinkGroupItemProperties.Minimum;
					int kitLinkGroupItemCalories = kitLinkGroupItemLocaleDto.Calories.HasValue ? kitLinkGroupItemLocaleDto.Calories.Value : 0;

					//If a modifier is mandatory, then the minimum portion of the modifier will be included in the caloric calculation
					if (Convert.ToBoolean(kitLinkGroupItemProperties.MandatoryItem))
					{
						kitLinkGroupMaxCalories = kitLinkGroupMaxCalories + kitLinkGroupItemCalories * kitLinkGroupItemMin;
						//Get the maximum number of portions are allowed after the minimum number of portions of a mandatory modifier is considered.
						kitLinkGroupMaxPortion = kitLinkGroupMaxPortion - kitLinkGroupItemMin;

						if (kitLinkGroupMaxPortion > 0)
						{
							modifierMax[arrayIndex, 0] = kitLinkGroupItemCalories;
							//In case the mandatory modifier has the highest calories, get the remaining of the number of portions left that can be used in the max calories calculation
							//after the minimum number of portion is used in the calculation.
							modifierMax[arrayIndex, 1] = kitLinkGroupItemMax - kitLinkGroupItemMin;

							arrayIndex++;
						}
					}
					else if (kitLinkGroupMaxPortion > 0)
					{
						modifierMax[arrayIndex, 0] = kitLinkGroupItemCalories;
						modifierMax[arrayIndex, 1] = kitLinkGroupItemMax;

						arrayIndex++;
					}
				}

				//Sort the two dimentional array modifierMax by the first element, which is modifier's calories. The second element
				//is maximum portion of a modifier.
				int[,] sortedByFirstElement = modifierMax.OrderByDescending(x => x[0]);

				while (kitLinkGroupMaxPortion > 0)
				{
					for (int i = 0; i < modifierMax.GetLength(0); i++)
					{
						int counter = 0;
						if (kitLinkGroupMaxPortion >= modifierMax[i, 1])
						{
							counter = modifierMax[i, 1];
							kitLinkGroupMaxPortion = kitLinkGroupMaxPortion - modifierMax[i, 1];
						}
						else
						{
							counter = kitLinkGroupMaxPortion;
							kitLinkGroupMaxPortion = 0;
						}

						kitLinkGroupMaxCalories = kitLinkGroupMaxCalories + modifierMax[i,0] * counter;
					}
				}

				kitLinkGroupDto.MaximumCalories = kitLinkGroupMaxCalories;
				kitLocaleDto.MaximumCalories = kitLocaleDto.MaximumCalories.HasValue ? kitLocaleDto.MaximumCalories.Value : 0 + kitLinkGroupMaxCalories;
			}
		}

		internal async Task<IEnumerable<ItemStorePriceModel>> GetAuthorizedStatus(IEnumerable<StoreItem> storeItems)
		{
			PriceCollectionRequestModel pricesRequestModel = new PriceCollectionRequestModel
			{
				StoreItems = storeItems,
				IncludeFuturePrices = false,
				PriceType = "REG"
			};

			string pricesRequestJson = JsonConvert.SerializeObject(pricesRequestModel);
			HttpContent inputContent = new StringContent(pricesRequestJson, Encoding.UTF8, "application/json");

			ApiHelper.InitializeClient();
			string uri = ApiHelper.BasedUri + "price";
			ApiHelper.ApiClient.BaseAddress = new Uri(uri);

			try
			{
				using (HttpResponseMessage response = await ApiHelper.ApiClient.PostAsync(uri, inputContent))
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

		internal async Task<IEnumerable<ItemNutritionAttributesDictionary>> GetCaloricInfo(ItemNutritionRequestModel itemIds)
		{
			string pricesRequestJson = JsonConvert.SerializeObject(itemIds);
			HttpContent inputContent = new StringContent(pricesRequestJson, Encoding.UTF8, "application/json");

			ApiHelper.InitializeClient();
			string uri = ApiHelper.BasedUri + "itemNutrition";
			ApiHelper.ApiClient.BaseAddress = new Uri(uri);

			try
			{
				using (HttpResponseMessage response = await ApiHelper.ApiClient.PostAsync(uri, inputContent))
				{
					if (response.IsSuccessStatusCode)
					{
						IEnumerable<ItemNutritionAttributesDictionary> itemCaloricInfo = await response.Content.ReadAsAsync<IEnumerable<ItemNutritionAttributesDictionary>>();
						return itemCaloricInfo;
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