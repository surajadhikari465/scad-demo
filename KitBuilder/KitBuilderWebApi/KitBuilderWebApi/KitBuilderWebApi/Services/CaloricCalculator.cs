using KitBuilder.DataAccess.Dto;
using KitBuilderWebApi.QueryParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using KitBuilder.DataAccess.Repository;
using KitBuilder.DataAccess.DatabaseModels;
using AutoMapper;
using Newtonsoft.Json;
using System.Text;
using Microsoft.Extensions.Configuration;
using KitBuilder.DataAccess.Queries;
using KitBuilderWebApi.Helper;

namespace KitBuilderWebApi.Services
{
	public class CaloricCalculator : IService<GetKitLocaleByStoreParameters, Task<KitLocaleDto>>
	{
		private IQueryHandler<GetKitByKitLocaleIdParameters, KitLocale> getKitLocaleQuery;
		private IRepository<Locale> localeRepository;
		private IService<IEnumerable<StoreItem>, Task<IEnumerable<ItemStorePriceModel>>> getAuthorizedStatusAndPriceService;
		private IService<ItemNutritionRequestModel, Task<IEnumerable<ItemNutritionAttributesDictionary>>> getNutritionService;

		public CaloricCalculator(IQueryHandler<GetKitByKitLocaleIdParameters, KitLocale> getKitLocaleQuery,
							IRepository<Locale> localeRepository,
							IService<IEnumerable<StoreItem>, Task<IEnumerable<ItemStorePriceModel>>> getAuthorizedStatusAndPriceService,
							IService<ItemNutritionRequestModel, Task<IEnumerable<ItemNutritionAttributesDictionary>>> getNutritionService)
		{
			this.getKitLocaleQuery = getKitLocaleQuery;
			this.localeRepository = localeRepository;
			this.getAuthorizedStatusAndPriceService = getAuthorizedStatusAndPriceService;
			this.getNutritionService = getNutritionService;
		}

		public async Task<KitLocaleDto> Run(GetKitLocaleByStoreParameters kitLocaleByStoreParameters)
		{
			GetKitByKitLocaleIdParameters searchKitLocaleParameters = new GetKitByKitLocaleIdParameters
			{
				KitLocaleId = kitLocaleByStoreParameters.KitLocaleId
			};

			KitLocaleDto kitLocaleDto = Mapper.Map<KitLocaleDto>(getKitLocaleQuery.Search(searchKitLocaleParameters));

			if (kitLocaleDto != null)
			{
				//Get each modifier's Authorization Status and REG price 
				List<StoreItem> storeItemsList = ParseParametersForPriceCall(kitLocaleDto, kitLocaleByStoreParameters.StoreLocaleId);
				
				var resultPrice = getAuthorizedStatusAndPriceService.Run(storeItemsList);
				var itemStorePriceModelList = await resultPrice;

				//update kitlocale with Price and Store Authorization Status
				UpdateKitLocaleForPrice(kitLocaleDto, itemStorePriceModelList);

				//Get each modifier's caloric info
				ItemNutritionRequestModel itemIds = ParseParametersForNutritionCall(kitLocaleDto);

				var resultCalories = getNutritionService.Run(itemIds);
				var itemCaloriesList = await resultCalories;

				// update kitlocale with Nutrition info
				UpdateKitLocaleForNutrition(kitLocaleDto, itemCaloriesList);

				SetMaxCalories(kitLocaleDto);
			}
			return kitLocaleDto;

		}

		internal List<StoreItem> ParseParametersForPriceCall(KitLocaleDto kitLocaleDto, int storeLocaleId)
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
			var kitLinkGroupItemLocaleDtos = from kitLinkGroupLocales in kitLocaleDto.KitLinkGroupLocale
											 from kitLinkGroupItemLocales in kitLinkGroupLocales.KitLinkGroupItemLocales
											 select kitLinkGroupItemLocales;
			foreach (KitLinkGroupItemLocaleDto kitLinkGroupItemLocaleDto in kitLinkGroupItemLocaleDtos)
			{
				ItemStorePriceModel itemStorePriceModel = itemStorePriceModelList.Where(i => i.ItemId == kitLinkGroupItemLocaleDto.KitLinkGroupItem.LinkGroupItem.ItemId).FirstOrDefault();

				if (itemStorePriceModel != null)
				{
					kitLinkGroupItemLocaleDto.ItemId = itemStorePriceModel.ItemId;
					kitLinkGroupItemLocaleDto.RegularPrice = itemStorePriceModel.Price;
					kitLinkGroupItemLocaleDto.AuthorizedByStore = itemStorePriceModel.Authorized;
				}
				else
				{
					kitLinkGroupItemLocaleDto.ItemId = kitLinkGroupItemLocaleDto.KitLinkGroupItem.LinkGroupItem.ItemId;
					kitLinkGroupItemLocaleDto.AuthorizedByStore = false;
				}
			}

			kitLocaleDto.AuthorizedByStore = itemStorePriceModelList.Where(i => i.ItemId == kitLocaleDto.Kit.ItemId).Select(a => a.Authorized).FirstOrDefault();
			kitLocaleDto.AuthorizedByStore = kitLocaleDto.AuthorizedByStore == null ? false : kitLocaleDto.AuthorizedByStore;
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
			foreach (KitLinkGroupLocaleDto kitLinkGroupDto in kitLocaleDto.KitLinkGroupLocale.Where(i => i.Exclude == false))
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

						kitLinkGroupMaxCalories = kitLinkGroupMaxCalories + modifierMax[i, 0] * counter;
					}
				}

				kitLinkGroupDto.MaximumCalories = kitLinkGroupMaxCalories;
				kitLocaleDto.MaximumCalories = kitLocaleDto.MaximumCalories.HasValue ? kitLocaleDto.MaximumCalories.Value : 0 + kitLinkGroupMaxCalories;
			}
		}
	}
}
