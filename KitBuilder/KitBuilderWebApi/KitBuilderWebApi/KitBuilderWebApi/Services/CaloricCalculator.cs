using KitBuilder.DataAccess.Dto;
using KitBuilderWebApi.QueryParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KitBuilder.DataAccess.Repository;
using KitBuilder.DataAccess.DatabaseModels;
using AutoMapper;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
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
		private ILogger<CaloricCalculator> logger;

		public CaloricCalculator(IQueryHandler<GetKitByKitLocaleIdParameters, KitLocale> getKitLocaleQuery,
							IRepository<Locale> localeRepository,
							IService<IEnumerable<StoreItem>, Task<IEnumerable<ItemStorePriceModel>>> getAuthorizedStatusAndPriceService,
							IService<ItemNutritionRequestModel, Task<IEnumerable<ItemNutritionAttributesDictionary>>> getNutritionService,
							ILogger<CaloricCalculator> logger)
		{
			this.getKitLocaleQuery = getKitLocaleQuery;
			this.localeRepository = localeRepository;
			this.getAuthorizedStatusAndPriceService = getAuthorizedStatusAndPriceService;
			this.getNutritionService = getNutritionService;
			this.logger = logger;
		}

		public async Task<KitLocaleDto> Run(GetKitLocaleByStoreParameters kitLocaleByStoreParameters)
		{
			KitLocaleDto kitLocaleDto = new KitLocaleDto();

			try
			{
				GetKitByKitLocaleIdParameters searchKitLocaleParameters = new GetKitByKitLocaleIdParameters
				{
					KitLocaleId = kitLocaleByStoreParameters.KitLocaleId
				};

				KitLocale kitLocale = getKitLocaleQuery.Search(searchKitLocaleParameters);

				if (kitLocale != null)
				{
					kitLocaleDto = Mapper.Map<KitLocaleDto>(kitLocale);
				}
				else
				{
					logger.LogWarning("No KitLocale can be found by KitLocaleId : " + kitLocaleByStoreParameters.KitLocaleId.ToString());
				}

				if (kitLocaleDto != null)
				{
					//Get each modifier's Authorization Status and REG price 
					List<StoreItem> storeItemsList = ParseParametersForPriceCall(kitLocaleDto, kitLocaleByStoreParameters.StoreLocaleId);

					var resultPrice = getAuthorizedStatusAndPriceService.Run(storeItemsList);
					var itemStorePriceModelList = await resultPrice;

					//update kitlocale with Price and Store Authorization Status
					UpdateKitLocaleForPrice(kitLocaleDto, itemStorePriceModelList);

					if (kitLocaleDto.AuthorizedByStore == true)
					{
						//Get each modifier's caloric info
						ItemNutritionRequestModel itemIds = ParseParametersForNutritionCall(kitLocaleDto);

						var resultCalories = getNutritionService.Run(itemIds);
						var itemCaloriesList = await resultCalories;

						// update kitlocale with Nutrition info
						UpdateKitLocaleForNutrition(kitLocaleDto, itemCaloriesList);

						SetMaxCalories(kitLocaleDto);
					}
					else
					{
						logger.LogWarning(string.Format("The kit main item {0} is not authorized for store with Locale Id of {1}.", kitLocaleDto.Kit.ItemId, kitLocaleByStoreParameters.StoreLocaleId));
					}
				}
				else
				{
					logger.LogWarning("kitLocaleDto cannot be derived from KitLocale with KitLocaleId : " + kitLocaleByStoreParameters.KitLocaleId.ToString());
				}
			}
			catch (Exception e)
			{
				logger.LogError(e.Message);
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

			var storeBusinessUnitId = localeRepository.GetAll().Where(s => s.LocaleId == storeLocaleId).Select(s => s.BusinessUnitId).FirstOrDefault();
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
				if (itemNutritionModel.Key == kitLocaleDto.Kit.ItemId && kitLocaleDto.MinimumCalories == null)
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
					kitLinkGroupItemLocaleDto.ServingSizeDesc = itemNutritionModel.Value.ServingSizeDesc;
					kitLinkGroupItemLocaleDto.ServingsPerPortion = itemNutritionModel.Value.ServingsPerPortion;
				}
			}
		}

		internal void SetMaxCalories(KitLocaleDto kitLocaleDto)
		{
			try
			{

				foreach (KitLinkGroupLocaleDto kitLinkGroupDto in kitLocaleDto.KitLinkGroupLocale.Where(i => i.Exclude == false))
				{
					dynamic kitLinkGroupProperties = JsonConvert.DeserializeObject(kitLinkGroupDto.Properties);
					int kitLinkGroupMaxCalories = 0;
					int kitLinkGroupMaxPortion = kitLinkGroupProperties.Maximum;
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
						for (int i = 0; i < sortedByFirstElement.GetLength(0); i++)
						{
							int counter = 0;
							if (kitLinkGroupMaxPortion >= sortedByFirstElement[i, 1])
							{
								counter = sortedByFirstElement[i, 1];
								kitLinkGroupMaxPortion = kitLinkGroupMaxPortion - sortedByFirstElement[i, 1];
							}
							else
							{
								counter = kitLinkGroupMaxPortion;
								kitLinkGroupMaxPortion = 0;
							}

							kitLinkGroupMaxCalories = kitLinkGroupMaxCalories + sortedByFirstElement[i, 0] * counter;
						}
					}

					kitLinkGroupDto.MaximumCalories = kitLinkGroupMaxCalories;
					kitLocaleDto.MaximumCalories = (kitLocaleDto.MaximumCalories.HasValue ? kitLocaleDto.MaximumCalories.Value : 0) + kitLinkGroupMaxCalories;
				}
			}
			catch (Exception e)
			{
				throw new Exception(e.Message);
			}
		}
	}
}
