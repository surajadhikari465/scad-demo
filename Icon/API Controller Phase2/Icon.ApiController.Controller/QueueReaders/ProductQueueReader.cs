using Icon.ApiController.Common;
using Icon.ApiController.Controller.Extensions;
using Icon.ApiController.Controller.Mappers;
using Icon.ApiController.DataAccess.Commands;
using Icon.ApiController.DataAccess.Queries;
using Icon.Common.DataAccess;
using Icon.Common.Email;
using Icon.Framework;
using Icon.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using Contracts = Icon.Esb.Schemas.Wfm.Contracts;

namespace Icon.ApiController.Controller.QueueReaders
{
	public class ProductQueueReader : IQueueReader<MessageQueueProduct, Contracts.items>
	{
		private ILogger<ProductQueueReader> logger;
		private IEmailClient emailClient;
		private IQueryHandler<GetMessageQueueParameters<MessageQueueProduct>, List<MessageQueueProduct>> getMessageQueueQuery;
		private ICommandHandler<UpdateMessageQueueStatusCommand<MessageQueueProduct>> updateMessageQueueStatusCommandHandler;
		private IProductSelectionGroupsMapper productSelectionGroupMapper;
		private IUomMapper uomMapper;
		private ApiControllerSettings settings;
		const string DELETED = "Deleted";

		public ProductQueueReader(
				ILogger<ProductQueueReader> logger,
				IEmailClient emailClient,
				IQueryHandler<GetMessageQueueParameters<MessageQueueProduct>, List<MessageQueueProduct>> getMessageQueueQuery,
				ICommandHandler<UpdateMessageQueueStatusCommand<MessageQueueProduct>> updateMessageQueueStatusCommandHandler,
				IProductSelectionGroupsMapper productSelectionGroupMapper,
				IUomMapper uomMapper,
				ApiControllerSettings settings)
		{
			this.logger = logger;
			this.emailClient = emailClient;
			this.getMessageQueueQuery = getMessageQueueQuery;
			this.updateMessageQueueStatusCommandHandler = updateMessageQueueStatusCommandHandler;
			this.productSelectionGroupMapper = productSelectionGroupMapper;
			this.uomMapper = uomMapper;
			this.productSelectionGroupMapper.LoadProductSelectionGroups();
			this.settings = settings;
		}

		public List<MessageQueueProduct> GetQueuedMessages()
		{
			var parameters = new GetMessageQueueParameters<MessageQueueProduct>
			{
				Instance = ControllerType.Instance,
				MessageQueueStatusId = MessageStatusTypes.Ready
			};

			return getMessageQueueQuery.Search(parameters);
		}

		public List<MessageQueueProduct> GroupMessagesForMiniBulk(List<MessageQueueProduct> messages)
		{
			if(messages == null || messages.Count == 0)
			{
				throw new ArgumentException("GroupMessages() was called with invalid arguments.  The parameter 'messages' must be a non-empty, non-null list.");
			}

			// Use the first message in the queue to set the conditions for grouping the rest of the messages.
			int currentMessageIndex = 0;
			MessageQueueProduct message = messages[currentMessageIndex++];

			string baseDepartmentSale = message.DepartmentSale;
			string baseItemType = message.ItemTypeCode;

			var groupedItemsById = new HashSet<int>();
			groupedItemsById.Add(message.ItemId);

			var groupedMessages = new List<MessageQueueProduct> { message };

			int miniBulkLimit;
			if(!Int32.TryParse(ConfigurationManager.AppSettings["MiniBulkLimitProduct"], out miniBulkLimit))
			{
				miniBulkLimit = 100;
			}

			while(groupedMessages.Count < miniBulkLimit && currentMessageIndex < messages.Count)
			{
				message = messages[currentMessageIndex];

				if(!ItemAlreadyExistsInMiniBulk(groupedItemsById, message.ItemId) &&
						!ItemHasDifferentDepartmentSaleValue(baseDepartmentSale, message.DepartmentSale) &&
						!ItemHasRetailNonRetailMismatch(baseItemType, message.ItemTypeCode))
				{
					groupedMessages.Add(message);
					groupedItemsById.Add(message.ItemId);
				}

				currentMessageIndex++;
			}

			logger.Info(string.Format("Grouped {0} queued messages to be included in the mini-bulk.  Mini-bulk type: {1}",
					groupedMessages.Count, groupedMessages[0].DepartmentSale == "1" ? "DepartmentSale" : "Product"));

			return groupedMessages;
		}

		public Contracts.items BuildMiniBulk(List<MessageQueueProduct> messages)
		{
			if(messages == null || messages.Count == 0)
			{
				throw new ArgumentException("BuildMiniBulk() was called with invalid arguments.  Parameter 'messages' must be a non-null and non-empty list.");
			}

			var miniBulk = new Contracts.items();
			miniBulk.item = new Contracts.ItemType[messages.Count];

			if(messages[0].DepartmentSale == "1")
			{
				return BuildDepartmentSaleMiniBulk(miniBulk, messages);
			}
			else
			{
				return BuildProductMiniBulk(miniBulk, messages);
			}
		}

		private Contracts.items BuildDepartmentSaleMiniBulk(Contracts.items miniBulk, List<MessageQueueProduct> messages)
		{
			int currentMiniBulkIndex = 0;

			foreach(var message in messages)
			{
				try
				{
					var miniBulkEntry = new Contracts.ItemType
					{
						Action = Contracts.ActionEnum.AddOrUpdate,
						ActionSpecified = true,
						id = message.ItemId,
						@base = new Contracts.BaseItemType
						{
							type = new Contracts.ItemTypeType
							{
								code = message.ItemTypeCode,
								description = message.ItemTypeDesc
							}
						},
						locale = new Contracts.LocaleType[]
							{
														new Contracts.LocaleType
														{
																id = Locales.WholeFoods.ToString(),
																name = "Whole Foods Market",
																type = new Contracts.LocaleTypeType
																{
																		code = Contracts.LocaleCodeType.CHN,
																		description = Contracts.LocaleDescType.Chain
																},
																Item = new Contracts.EnterpriseItemAttributesType
																{
																		scanCodes = new Contracts.ScanCodeType[]
																		{
																				new Contracts.ScanCodeType
																				{
																						id = message.ScanCodeId,
																						code = message.ScanCode,
																						typeId = message.ScanCodeTypeId,
																						typeIdSpecified = true,
																						typeDescription = message.ScanCodeTypeDesc
																				}
																		},
																		hierarchies = new Contracts.HierarchyType[]
																		{
																				BuildMerchandiseHierarchy(message),
																				BuildTaxHierarchy(message)
																		},
																		traits = new Contracts.TraitType[]
																		{
																				new Contracts.TraitType
																				{
																						code = TraitCodes.DepartmentSale,
																						type = new Contracts.TraitTypeType
																						{
																								description = TraitDescriptions.DepartmentSale,
																								value = new Contracts.TraitValueType[]
																								{
																										new Contracts.TraitValueType
																										{
																												value = message.FinancialClassId
																										}
																								}
																						}
																				}
																		},
																}
														}
							}
					};

					miniBulk.item[currentMiniBulkIndex++] = miniBulkEntry;
				}
				catch(Exception ex)
				{
					HandleMiniBulkException(message, ex);
				}
			}

			// The mini-bulk was defined at the start with the maximum allowable size. If any messages failed to be included in the mini-bulk,
			// there will be null elements in the array that were never assigned or initialized.  Exclude those elements so that only valid data is returned.
			miniBulk.item = miniBulk.item.Where(i => i != null).ToArray();
			return miniBulk;
		}

		private Contracts.items BuildProductMiniBulk(Contracts.items miniBulk, List<MessageQueueProduct> messages)
		{
			int currentMiniBulkIndex = 0;

			foreach(var message in messages)
			{
				try
				{
					var miniBulkEntry = new Contracts.ItemType
					{
						Action = Contracts.ActionEnum.AddOrUpdate,
						ActionSpecified = true,
						id = message.ItemId,
                        
						@base = new Contracts.BaseItemType
						{
							type = new Contracts.ItemTypeType
							{
								code = message.ItemTypeCode,
								description = message.ItemTypeDesc
							},
							consumerInformation = BuildConsumerInformation(message) 
                            
						},
						locale = new Contracts.LocaleType[]
						{
													new Contracts.LocaleType
													{
															id = Locales.WholeFoods.ToString(),
															name = "Whole Foods Market",
															type = new Contracts.LocaleTypeType
															{
																	code = Contracts.LocaleCodeType.CHN,
																	description = Contracts.LocaleDescType.Chain
															},
															Item = new Contracts.EnterpriseItemAttributesType
															{
                                                                    isKitchenItemSpecified = false,
                                                                    isHospitalityItemSpecified = false,
																	scanCodes = new Contracts.ScanCodeType[]
																	{
																			BuildScanCodeType(message)
																	},
																	hierarchies = settings.EnableNationalHierarchy ?
																			new Contracts.HierarchyType[]
																			{
																					BuildMerchandiseHierarchy(message),
																					BuildBrandHierarchy(message),
																					BuildTaxHierarchy(message),
																					BuildFinancialHierarchy(message),
																					BuildNationalHierarchy(message)
																			} :
																			new Contracts.HierarchyType[]
																			{
																					BuildMerchandiseHierarchy(message),
																					BuildBrandHierarchy(message),
																					BuildTaxHierarchy(message),
																					BuildFinancialHierarchy(message)
																			},
																	traits = BuildItemTraits(message),
																	selectionGroups = productSelectionGroupMapper.GetProductSelectionGroups(message)
															}
													}
						}
					};
                    
                    AddHospitalityDataToItemMessage(miniBulkEntry, message, settings);
                    miniBulk.item[currentMiniBulkIndex++] = miniBulkEntry;
				}
				catch(Exception ex)
				{
					HandleMiniBulkException(message, ex);
				}
			}

			// The mini-bulk was defined at the start with the maximum allowable size. If any messages failed to be included in the mini-bulk,
			// there will be null elements in the array that were never assigned or initialized.  Exclude those elements so that only valid data is returned.
			miniBulk.item = miniBulk.item.Where(i => i != null).ToArray();
			return miniBulk;
		}

        private static void AddHospitalityDataToItemMessage(Contracts.ItemType miniBulkEntry, MessageQueueProduct message, ApiControllerSettings settings)
        {
                var item = (Contracts.EnterpriseItemAttributesType) miniBulkEntry.locale[0].Item;
                
                if (settings.EnableHospitalityImageUrl)
                    item.imageUrl = message.ImageURL;
                if (settings.EnableHospitalityKitchenDescription)
                    item.kitchenDescription = message.KitchenDescription;
                if (settings.EnableHospitalityKitchenItem)
                {
                    item.isKitchenItemSpecified = message.KitchenItem.HasValue;
                    item.isKitchenItem = message.KitchenItem ?? false;
                }

                if (settings.EnableHospitalityHospitalityItem)
                {
                    item.isHospitalityItemSpecified = message.HospitalityItem.HasValue;
                    item.isHospitalityItem = message.HospitalityItem ?? false;
                }
        }

        private Contracts.TraitType[] BuildItemTraits(MessageQueueProduct message)
		{
			var itemTraits = new List<Contracts.TraitType>
						{
								BuildTrait(TraitCodes.ProductDescription, TraitDescriptions.ProductDescription, message.ProductDescription),
								BuildTrait(TraitCodes.PosDescription, TraitDescriptions.PosDescription, message.PosDescription),
								BuildTrait(TraitCodes.FoodStampEligible, TraitDescriptions.FoodStampEligible, message.FoodStampEligible),
								BuildTrait(TraitCodes.ProhibitDiscount, TraitDescriptions.ProhibitDiscount, message.ProhibitDiscount),
								BuildTrait(TraitCodes.GlobalPricingProgram, TraitDescriptions.GlobalPricingProgram, message.GlobalPricingProgram),
								BuildTrait(TraitCodes.FairTradeCertified, TraitDescriptions.FairTradeCertified, message.FairTradeCertified),
								BuildTrait(TraitCodes.FlexibleText, TraitDescriptions.FlexibleText, message.FlexibleText),
								BuildTrait(TraitCodes.MadeWithOrganicGrapes, TraitDescriptions.MadeWithOrganicGrapes, message.MadeWithOrganicGrapes),
								BuildTrait(TraitCodes.PrimeBeef, TraitDescriptions.PrimeBeef, message.PrimeBeef),
								BuildTrait(TraitCodes.RainforestAlliance, TraitDescriptions.RainforestAlliance, message.RainforestAlliance),
								BuildTrait(TraitCodes.Refrigerated, TraitDescriptions.Refrigerated, message.Refrigerated),
								BuildTrait(TraitCodes.SmithsonianBirdFriendly, TraitDescriptions.SmithsonianBirdFriendly, message.SmithsonianBirdFriendly),
								BuildTrait(TraitCodes.WicEligible, TraitDescriptions.WicEligible, message.WicEligible),
								BuildTrait(TraitCodes.ShelfLife, TraitDescriptions.ShelfLife, message.ShelfLife),
								BuildTrait(TraitCodes.SelfCheckoutItemTareGroup, TraitDescriptions.SelfCheckoutItemTareGroup, message.SelfCheckoutItemTareGroup),
								BuildTrait(TraitCodes.HiddenItem, TraitDescriptions.HiddenItem, message.Hidden),
								BuildTrait(TraitCodes.Line, TraitDescriptions.Line, message.Line),
								BuildTrait(TraitCodes.Sku, TraitDescriptions.Sku, message.SKU),
								BuildTrait(TraitCodes.PriceLine, TraitDescriptions.PriceLine, message.PriceLine),
								BuildTrait(TraitCodes.VariantSize, TraitDescriptions.VariantSize, message.VariantSize),
								BuildTrait(TraitCodes.EstoreNutritionRequired, TraitDescriptions.EstoreNutritionRequired, message.EStoreNutritionRequired),
								BuildTrait(TraitCodes.EstoreEligible, TraitDescriptions.EstoreEligible, message.EstoreEligible),
								BuildTraitLeaveBlankIfNull(TraitCodes.PrimeNowEligible, TraitDescriptions.PrimeNowEligible, message.PrimeNowEligible),
								BuildTraitLeaveBlankIfNull(TraitCodes.Tsf365Eligible, "365 Eligible", message.TSFEligible),
								BuildTraitLeaveBlankIfNull(TraitCodes.WfmEligilble, TraitDescriptions.WfmEligilble, message.WFMEligilble),
								BuildTraitLeaveBlankIfNull(TraitCodes.Other3pEligible, TraitDescriptions.Other3pEligible, message.Other3PEligible),
						};

			if(ShouldSendPhysicalCharacteristicTraits(message))
			{
				var physicalCharacteristicTraits = BuildPhysicalTraits(message);
				itemTraits.AddRange(physicalCharacteristicTraits);
			}

			var signAttributeTraits = BuildSignAttributes(message);
			itemTraits.AddRange(signAttributeTraits);

			var nutritionTraits = BuildNutritionTraits(message);
			if(nutritionTraits != null)
			{
				itemTraits.AddRange(nutritionTraits);
			}

			return itemTraits.ToArray();
		}

		private List<Contracts.TraitType> BuildNutritionTraits(MessageQueueProduct messageQueuProduct)
		{
			MessageQueueNutrition message = messageQueuProduct.MessageQueueNutrition.FirstOrDefault();

			if(message == null)
			{
				return null;
			}

			var nutritionTraits = new List<Contracts.TraitType>
						{
								BuildTrait(TraitCodes.RecipeName, TraitDescriptions.RecipeName, message.RecipeName),
								BuildTrait(TraitCodes.Allergens, TraitDescriptions.Allergens, message.Allergens),
								BuildTrait(TraitCodes.Ingredients, TraitDescriptions.Ingredients, message.Ingredients),
								BuildTrait(TraitCodes.Hsh, TraitDescriptions.Hsh, message.HshRating),
								BuildTrait(TraitCodes.PolyunsaturatedFat, TraitDescriptions.PolyunsaturatedFat, message.PolyunsaturatedFat),
								BuildTrait(TraitCodes.MonounsaturatedFat, TraitDescriptions.MonounsaturatedFat, message.MonounsaturatedFat),
								BuildTrait(TraitCodes.PotassiumWeight, TraitDescriptions.PotassiumWeight, message.PotassiumWeight),
								BuildTrait(TraitCodes.PotassiumPercent, TraitDescriptions.PotassiumPercent, message.PotassiumPercent),
								BuildTrait(TraitCodes.DietaryFiberPercent, TraitDescriptions.DietaryFiberPercent, message.DietaryFiberPercent),
								BuildTrait(TraitCodes.SolubleFiber, TraitDescriptions.SolubleFiber, message.SolubleFiber),
								BuildTrait(TraitCodes.InsolubleFiber, TraitDescriptions.InsolubleFiber, message.InsolubleFiber),
								BuildTrait(TraitCodes.SugarAlcohol, TraitDescriptions.SugarAlcohol, message.SugarAlcohol),
								BuildTrait(TraitCodes.OtherCarbohydrates, TraitDescriptions.OtherCarbohydrates, message.OtherCarbohydrates),
								BuildTrait(TraitCodes.ProteinPercent, TraitDescriptions.ProteinPercent, message.ProteinPercent),
								BuildTrait(TraitCodes.Betacarotene, TraitDescriptions.Betacarotene, message.Betacarotene),
								BuildTrait(TraitCodes.VitaminD, TraitDescriptions.VitaminD, message.VitaminD),
								BuildTrait(TraitCodes.VitaminE, TraitDescriptions.VitaminE, message.VitaminE),
								BuildTrait(TraitCodes.Thiamin, TraitDescriptions.Thiamin, message.Thiamin),
								BuildTrait(TraitCodes.Riboflavin, TraitDescriptions.Riboflavin, message.Riboflavin),
								BuildTrait(TraitCodes.Niacin, TraitDescriptions.Niacin, message.Niacin),
								BuildTrait(TraitCodes.VitaminB6, TraitDescriptions.VitaminB6, message.VitaminB6),
								BuildTrait(TraitCodes.Folate, TraitDescriptions.Folate, message.Folate),
								BuildTrait(TraitCodes.VitaminB12, TraitDescriptions.VitaminB12, message.VitaminB12),
								BuildTrait(TraitCodes.Biotin, TraitDescriptions.Biotin, message.Biotin),
								BuildTrait(TraitCodes.PantothenicAcid, TraitDescriptions.PantothenicAcid, message.PantothenicAcid),
								BuildTrait(TraitCodes.Phosphorous, TraitDescriptions.Phosphorous, message.Phosphorous),
								BuildTrait(TraitCodes.Iodine, TraitDescriptions.Iodine, message.Iodine),
								BuildTrait(TraitCodes.Magnesium, TraitDescriptions.Magnesium, message.Magnesium),
								BuildTrait(TraitCodes.Zinc, TraitDescriptions.Zinc, message.Zinc),
								BuildTrait(TraitCodes.Copper, TraitDescriptions.Copper, message.Copper),
								BuildTrait(TraitCodes.Transfat, TraitDescriptions.Transfat, message.Transfat),
								BuildTrait(TraitCodes.Om6Fatty, TraitDescriptions.Om6Fatty, message.Om6Fatty),
								BuildTrait(TraitCodes.Om3Fatty, TraitDescriptions.Om3Fatty, message.Om3Fatty),
								BuildTrait(TraitCodes.Starch, TraitDescriptions.Starch, message.Starch),
								BuildTrait(TraitCodes.Chloride, TraitDescriptions.Chloride, message.Chloride),
								BuildTrait(TraitCodes.Chromium, TraitDescriptions.Chromium, message.Chromium),
								BuildTrait(TraitCodes.VitaminK, TraitDescriptions.VitaminK, message.VitaminK),
								BuildTrait(TraitCodes.Manganese, TraitDescriptions.Manganese, message.Manganese),
								BuildTrait(TraitCodes.Molybdenum, TraitDescriptions.Molybdenum, message.Molybdenum),
								BuildTrait(TraitCodes.Selenium, TraitDescriptions.Selenium, message.Selenium),
								BuildTrait(TraitCodes.TransfatWeight, TraitDescriptions.TransfatWeight, message.TransfatWeight),
								BuildTrait(TraitCodes.CaloriesFromTransFat, TraitDescriptions.CaloriesFromTransFat, message.CaloriesFromTransfat),
								BuildTrait(TraitCodes.CaloriesSaturatedFat, TraitDescriptions.CaloriesSaturatedFat, message.CaloriesSaturatedFat),
								BuildTrait(TraitCodes.ServingPerContainer, TraitDescriptions.ServingPerContainer, message.ServingPerContainer),
								BuildTrait(TraitCodes.ServingSizeDesc, TraitDescriptions.ServingSizeDesc, message.ServingSizeDesc),
								BuildTrait(TraitCodes.ServingsPerPortion, TraitDescriptions.ServingsPerPortion, message.ServingsPerPortion),
								BuildTrait(TraitCodes.ServingUnits, TraitDescriptions.ServingUnits, message.ServingUnits),
								BuildTrait(TraitCodes.SizeWeight, TraitDescriptions.SizeWeight, message.SizeWeight),
								BuildTrait(TraitCodes.SizeWeight, TraitDescriptions.SizeWeight, message.SizeWeight),
						};

			//As per Bobbie: Action is required when Nutrition info is deleted and all TraitValueType.value should be empty string.
			if(IsNutritionRemoved(message))
			{
				nutritionTraits.ForEach(x =>
				{
					x.ActionSpecified = true;
					x.Action = Contracts.ActionEnum.Delete;
					x.type.value[0].value = String.Empty;
				});
			}
			return nutritionTraits;
		}

		private Contracts.ConsumerInformationType BuildConsumerInformation(MessageQueueProduct messageQueuProduct)
		{
			var message = messageQueuProduct.MessageQueueNutrition.FirstOrDefault();

			if(message == null)
			{
				return null;
			}

			//As per Bobbie: ConsumerInformation Nutrition should always have Action.
			return IsNutritionRemoved(message)
				? new Contracts.ConsumerInformationType
				{
					stockItemConsumerProductLabel = new Contracts.StockProductLabelType
					{
						Action = Contracts.ActionEnum.Delete,
						ActionSpecified = true
					}
				}
				: new Contracts.ConsumerInformationType
				{
					stockItemConsumerProductLabel = new Contracts.StockProductLabelType
					{
						Action = Contracts.ActionEnum.AddOrUpdate,
						ActionSpecified = true,
						consumerLabelTypeCode = null,
						servingSizeUom = null,
						servingSizeUomCount = Decimal.Zero,
                        servingSizeUomCountSpecified = true,
						servingsInRetailSaleUnitCount = null,
						caloriesCount = message.Calories.ToDecimal(),
                        caloriesCountSpecified = true,
						caloriesFromFatCount = message.CaloriesFat.ToDecimal(),
                        caloriesFromFatCountSpecified = true,
						totalFatGramsAmount = message.TotalFatWeight.ToDecimal(),
                        totalFatGramsAmountSpecified = true,
						totalFatDailyIntakePercent = message.TotalFatPercentage.ToDecimal(),
                        totalFatDailyIntakePercentSpecified = true,
						saturatedFatGramsAmount = message.SaturatedFatWeight.ToDecimal(),
                        saturatedFatGramsAmountSpecified = true,
						saturatedFatPercent = message.SaturatedFatPercent.ToDecimal(),
                        saturatedFatPercentSpecified = true,
						cholesterolMilligramsCount = message.CholesterolWeight.ToDecimal(),
                        cholesterolMilligramsCountSpecified = true,
						cholesterolPercent = message.CholesterolPercent.ToDecimal(),
                        cholesterolPercentSpecified = true,
						sodiumMilligramsCount = message.SodiumWeight.ToDecimal(),
                        sodiumMilligramsCountSpecified = true,
						sodiumPercent = message.SodiumPercent.ToDecimal(),
                        sodiumPercentSpecified = true,
						totalCarbohydrateMilligramsCount = message.TotalCarbohydrateWeight.ToDecimal(),
                        totalCarbohydrateMilligramsCountSpecified = true,
						totalCarbohydratePercent = message.TotalCarbohydratePercent.ToDecimal(),
                        totalCarbohydratePercentSpecified = true,
						dietaryFiberGramsCount = message.DietaryFiberWeight.ToDecimal(),
                        dietaryFiberGramsCountSpecified = true,
						sugarsGramsCount = message.Sugar.ToDecimal(),
                        sugarsGramsCountSpecified = true,
						proteinGramsCount = message.ProteinWeight.ToDecimal(),
                        proteinGramsCountSpecified = true,
						vitaminADailyMinimumPercent = message.VitaminA.ToDecimal(),
                        vitaminADailyMinimumPercentSpecified = true,
						vitaminBDailyMinimumPercent = Decimal.Zero,
                        vitaminBDailyMinimumPercentSpecified = true,
						vitaminCDailyMinimumPercent = message.VitaminC.ToDecimal(),
                        vitaminCDailyMinimumPercentSpecified = true,
						calciumDailyMinimumPercent = message.Calcium.ToDecimal(),
                        calciumDailyMinimumPercentSpecified = true,
						ironDailyMinimumPercent = message.Iron.ToDecimal(),
                        ironDailyMinimumPercentSpecified = true,
						nutritionalDescriptionText = null,
						isHazardousMaterial = message.HazardousMaterialFlag == null ? false : (message.HazardousMaterialFlag.Value == 1 ? true : false),
						hazardousMaterialTypeCode = null
					}
				};
		}

		private List<Contracts.TraitType> BuildSignAttributes(MessageQueueProduct message)
		{
			return new List<Contracts.TraitType>
						{
								BuildTrait(TraitCodes.AnimalWelfareRating, TraitDescriptions.AnimalWelfareRating, message.AnimalWelfareRating),
								BuildTrait(TraitCodes.Biodynamic, TraitDescriptions.Biodynamic, message.Biodynamic),
								BuildTrait(TraitCodes.CheeseMilkType, TraitDescriptions.CheeseMilkType, message.CheeseMilkType),
								BuildTrait(TraitCodes.CheeseRaw, TraitDescriptions.CheeseRaw, message.CheeseRaw),
								BuildTrait(TraitCodes.EcoScaleRating, TraitDescriptions.EcoScaleRating, message.EcoScaleRating),
								BuildTrait(TraitCodes.GlutenFree, TraitDescriptions.GlutenFree, message.GlutenFreeAgency),
								BuildTrait(TraitCodes.HealthyEatingRating, TraitDescriptions.HealthyEatingRating, message.HealthyEatingRating),
								BuildTrait(TraitCodes.Kosher, TraitDescriptions.Kosher, message.KosherAgency),
								BuildTrait(TraitCodes.Msc, TraitDescriptions.Msc, message.Msc),
								BuildTrait(TraitCodes.NonGmo, TraitDescriptions.NonGmo, message.NonGmoAgency),
								BuildTrait(TraitCodes.Organic, TraitDescriptions.Organic, message.OrganicAgency),
								BuildTrait(TraitCodes.PremiumBodyCare, TraitDescriptions.PremiumBodyCare, message.PremiumBodyCare),
								BuildTrait(TraitCodes.FreshOrFrozen, TraitDescriptions.FreshOrFrozen, message.SeafoodFreshOrFrozen),
								BuildTrait(TraitCodes.SeafoodCatchType, TraitDescriptions.SeafoodCatchType, message.SeafoodCatchType),
								BuildTrait(TraitCodes.Vegan, TraitDescriptions.Vegan, message.VeganAgency),
								BuildTrait(TraitCodes.Vegetarian, TraitDescriptions.Vegetarian, message.Vegetarian),
								BuildTrait(TraitCodes.WholeTrade, TraitDescriptions.WholeTrade, message.WholeTrade),
								BuildTrait(TraitCodes.GrassFed, TraitDescriptions.GrassFed, message.GrassFed),
								BuildTrait(TraitCodes.PastureRaised, TraitDescriptions.PastureRaised, message.PastureRaised),
								BuildTrait(TraitCodes.FreeRange, TraitDescriptions.FreeRange, message.FreeRange),
								BuildTrait(TraitCodes.DryAged, TraitDescriptions.DryAged, message.DryAged),
								BuildTrait(TraitCodes.AirChilled, TraitDescriptions.AirChilled, message.AirChilled),
								BuildTrait(TraitCodes.MadeInHouse, TraitDescriptions.MadeInHouse, message.MadeInHouse),
								BuildTrait(TraitCodes.CustomerFriendlyDescription, TraitDescriptions.CustomerFriendlyDescription, message.CustomerFriendlyDescription),
								BuildTrait(TraitCodes.NutritionRequired, TraitDescriptions.NutritionRequired, message.NutritionRequired),
						};
		}

		private List<Contracts.TraitType> BuildPhysicalTraits(MessageQueueProduct message)
		{
			var physicalCharacteristicTraits = new List<Contracts.TraitType>
						{
								BuildTrait(TraitCodes.PackageUnit, TraitDescriptions.PackageUnit, message.PackageUnit),
								BuildTrait(TraitCodes.RetailSize , TraitDescriptions.RetailSize , message.RetailSize ),
								BuildTrait(TraitCodes.RetailUom  , TraitDescriptions.RetailUom  , message.RetailUom, BuildUomType(message.RetailUom)),
						};
			return physicalCharacteristicTraits;
		}

		private Contracts.TraitType BuildTrait(string traitCode, string traitDescription, string value, Contracts.UomType uom)
		{
			var trait = BuildTrait(traitCode, traitDescription, value);
			trait.type.value.FirstOrDefault().uom = uom;
			return trait;
		}

		private Contracts.TraitType BuildTrait(string traitCode, string traitDescription, string value)
		{
			var trait = new Contracts.TraitType
			{
				code = traitCode,
				type = new Contracts.TraitTypeType
				{
					description = traitDescription,
					value = new Contracts.TraitValueType[]
							{
												new Contracts.TraitValueType
												{
														value = value ?? string.Empty,
												}
							}
				}
			};
			return trait;
		}

		private Contracts.TraitType BuildTrait(string traitCode, string traitDescription, bool value)
		{
			return BuildTrait(traitCode, traitDescription, value ? "1" : "0");
		}

		private Contracts.TraitType BuildTrait(string traitCode, string traitDescription, bool? value)
		{
			return BuildTrait(traitCode, traitDescription, value.GetValueOrDefault(false));
		}

		private Contracts.TraitType BuildTraitLeaveBlankIfNull(string traitCode, string traitDescription, bool? value)
		{
			return BuildTrait(traitCode, traitDescription, value.HasValue
					? value.Value ? "1" : "0"
					: string.Empty);
		}

		private Contracts.TraitType BuildTrait(string traitCode, string traitDescription, int? value)
		{
			return BuildTrait(traitCode, traitDescription, value?.ToString());
		}

		private Contracts.TraitType BuildTrait(string traitCode, string traitDescription, double? value)
		{
			return BuildTrait(traitCode, traitDescription, value?.ToString());
		}

		private Contracts.TraitType BuildTrait(string traitCode, string traitDescription, decimal? value)
		{
			return BuildTrait(traitCode, traitDescription, value?.ToString());
		}

		private Contracts.UomType BuildUomType(string productMessageUom)
		{
			var uomType = new Contracts.UomType
			{
				code = uomMapper.GetEsbUomCode(productMessageUom),
				codeSpecified = true,
				name = uomMapper.GetEsbUomDescription(productMessageUom),
				nameSpecified = true
			};
			return uomType;
		}

		private Contracts.ScanCodeType BuildScanCodeType(MessageQueueProduct message)
		{
			return new Contracts.ScanCodeType
			{
				id = message.ScanCodeId,
				code = message.ScanCode,
				typeId = message.ScanCodeTypeId,
				typeIdSpecified = true,
				typeDescription = message.ScanCodeTypeDesc
			};
		}

		private Contracts.HierarchyType BuildMerchandiseHierarchy(MessageQueueProduct message)
		{
			return new Contracts.HierarchyType
			{
				id = Hierarchies.Merchandise,
				@class = new Contracts.HierarchyClassType[]
					{
										new Contracts.HierarchyClassType
										{
												id = message.MerchandiseClassId.ToString(),
												name = message.MerchandiseClassName,
												level = message.MerchandiseLevel,
												parentId = new Contracts.hierarchyParentClassType
												{
														Value = message.MerchandiseParentId.HasValue ? message.MerchandiseParentId.Value : default(int)
												}
										}
					},
				name = HierarchyNames.Merchandise
			};
		}

		private Contracts.HierarchyType BuildBrandHierarchy(MessageQueueProduct message)
		{
			return new Contracts.HierarchyType
			{
				id = Hierarchies.Brands,
				@class = new Contracts.HierarchyClassType[]
					{
										new Contracts.HierarchyClassType
										{
												id = message.BrandId.ToString(),
												name = message.BrandName,
												level = message.BrandLevel,
												parentId = new Contracts.hierarchyParentClassType
												{
														Value = message.BrandParentId.HasValue ? message.BrandParentId.Value : default(int)
												}
										}
					},
				name = HierarchyNames.Brands
			};
		}

		private Contracts.HierarchyType BuildTaxHierarchy(MessageQueueProduct message)
		{
			return new Contracts.HierarchyType
			{
				id = Hierarchies.Tax,
				@class = new Contracts.HierarchyClassType[]
					{
										new Contracts.HierarchyClassType
										{
												id = message.TaxClassName.Split(' ')[0],
												name = message.TaxClassName,
												level = message.TaxLevel,
												parentId = new Contracts.hierarchyParentClassType
												{
														Value = message.TaxParentId.HasValue ? message.TaxParentId.Value : default(int)
												}
										}
					},
				name = HierarchyNames.Tax
			};
		}

		private Contracts.HierarchyType BuildFinancialHierarchy(MessageQueueProduct message)
		{
			return new Contracts.HierarchyType
			{
				id = Hierarchies.Financial,
				@class = new Contracts.HierarchyClassType[]
					{
										new Contracts.HierarchyClassType
										{
												id = message.FinancialClassId,
												name = message.FinancialClassName,
												level = message.FinancialLevel,
												parentId = new Contracts.hierarchyParentClassType
												{
														Value = message.FinancialParentId.HasValue ? message.FinancialParentId.Value : default(int)
												}
										}
					},
				name = HierarchyNames.Financial
			};
		}

		private Contracts.HierarchyType BuildNationalHierarchy(MessageQueueProduct message)
		{
			return new Contracts.HierarchyType
			{
				id = Hierarchies.National,
				@class = new Contracts.HierarchyClassType[]
					{
										new Contracts.HierarchyClassType
										{
												id = message.NationalClassId.ToString(),
												name = message.NationalClassName,
												level = message.NationalLevel.Value,
												parentId = new Contracts.hierarchyParentClassType
												{
														Value = message.NationalParentId.HasValue ? message.NationalParentId.Value : default(int)
												}
										}
					},
				name = HierarchyNames.National
			};
		}

		private static bool ShouldSendPhysicalCharacteristicTraits(MessageQueueProduct message)
		{
			// These three traits must all have values for them to be included in the message.
			// R10 will reject products with a 0 PackageUnit so we should not send products with a 0 package unit
			return (!string.IsNullOrWhiteSpace(message.PackageUnit) && message.PackageUnit != "0")
					&& !string.IsNullOrWhiteSpace(message.RetailSize)
					&& !string.IsNullOrWhiteSpace(message.RetailUom);
		}

		private bool ItemAlreadyExistsInMiniBulk(HashSet<int> groupedItemsById, int itemId)
		{
			return groupedItemsById.Contains(itemId);
		}

		private bool ItemHasDifferentDepartmentSaleValue(string baseDepartmentSale, string departmentSale)
		{
			return baseDepartmentSale != departmentSale;
		}

		private bool ItemHasRetailNonRetailMismatch(string baseItemType, string itemType)
		{
			if(baseItemType.Equals(ItemTypeCodes.NonRetail))
			{
				return baseItemType != itemType;
			}
			else
			{
				return itemType == ItemTypeCodes.NonRetail;
			}
		}

		private void HandleMiniBulkException(MessageQueueProduct message, Exception ex)
		{
			logger.Error(string.Format("MessageQueueId: {0}.  An error occurred when adding the message to the mini-bulk.  The message status will be marked as Failed.",
					message.MessageQueueId));

			ExceptionLogger<ProductQueueReader> exceptionLogger = new ExceptionLogger<ProductQueueReader>(logger);
			exceptionLogger.LogException(ex, this.GetType(), MethodBase.GetCurrentMethod());

			var command = new UpdateMessageQueueStatusCommand<MessageQueueProduct>
			{
				QueuedMessages = new List<MessageQueueProduct> { message },
				MessageStatusId = MessageStatusTypes.Failed,
				ResetInProcessBy = true
			};

			updateMessageQueueStatusCommandHandler.Execute(command);

			string errorMessage = string.Format(Resource.FailedToAddQueuedMessageToMiniBulkMessage, ControllerType.Type, ControllerType.Instance);
			string emailSubject = Resource.FailedToAddQueuedMessageToMiniBulkEmailSubject;
			string emailBody = EmailHelper.BuildMessageBodyForMiniBulkError(errorMessage, message.MessageQueueId, ex.ToString());

			try
			{
				emailClient.Send(emailBody, emailSubject);
			}
			catch(Exception mailEx)
			{
				string mailErrorMessage = "A failure occurred while attempting to send the alert email.";
				exceptionLogger.LogException(mailErrorMessage, mailEx, this.GetType(), MethodBase.GetCurrentMethod());
			}
		}

		bool IsNutritionRemoved(MessageQueueNutrition message)
		{
			return message != null && String.Compare(message.RecipeName, DELETED, true) == 0;
		}
	}
}