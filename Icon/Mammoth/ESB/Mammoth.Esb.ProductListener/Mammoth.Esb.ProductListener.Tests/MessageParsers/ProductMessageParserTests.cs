using Icon.Esb.Subscriber;
using Mammoth.Common.DataAccess;
using Mammoth.Esb.ProductListener.MessageParsers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.IO;

namespace Mammoth.Esb.ProductListener.Tests.MessageParsers
{
	[TestClass]
	public class ProductMessageParserTests
	{
		private ProductMessageParser parser;

		[TestInitialize]
		public void Initialize()
		{
			parser = new ProductMessageParser();
		}

		[TestMethod]
		public void ParseMessage_1ItemInMessageWithNoHospitalityData_ReturnItemModelWithNoHospitalityData()
		{
			//Given
			Mock<IEsbMessage> message = new Mock<IEsbMessage>();
			message.Setup(m => m.MessageText)
				.Returns(File.ReadAllText("TestMessages/ItemWithNoHospitalityElements.xml"));

			//When
			var itemModels = parser.ParseMessage(message.Object);

			//Then
			var item = itemModels[0];
			var kitItemAttributes = item.KitItemAttributes;
			Assert.IsNotNull(kitItemAttributes);
			Assert.IsNull(kitItemAttributes.KitchenDescription);
			Assert.IsNull(kitItemAttributes.KitchenItem);
			Assert.IsNull(kitItemAttributes.HospitalityItem);
			Assert.IsNull(kitItemAttributes.ImageUrl);
		}

		[TestMethod]
		public void ParseMessage_1ItemInMessageWithAllAttributes_ReturnItemModel()
		{
			//Given
			Mock<IEsbMessage> message = new Mock<IEsbMessage>();
			message.Setup(m => m.MessageText)
				.Returns(File.ReadAllText("TestMessages/ItemWithAllAttributes.xml"));

			//When
			var itemModels = parser.ParseMessage(message.Object);

			//Then
			Assert.AreEqual(1, itemModels.Count);

			var item = itemModels[0];
			var globalAttributes = item.GlobalAttributes;
			Assert.AreEqual(4067344, globalAttributes.ItemID);
			Assert.AreEqual(ItemTypes.RetailSale, globalAttributes.ItemTypeID);
			Assert.AreEqual("46000012881", globalAttributes.ScanCode);
			Assert.AreEqual(132626, globalAttributes.SubBrickID);
			Assert.AreEqual(50883, globalAttributes.BrandHCID);
			Assert.AreEqual("9989000", globalAttributes.MessageTaxClassHCID);
			Assert.AreEqual(4900, globalAttributes.PSNumber);
			Assert.AreEqual("CHICKEN TANDOORI HB", globalAttributes.Desc_Product);
			Assert.AreEqual("INGREDIENT", globalAttributes.Desc_POS);
			Assert.AreEqual(false, globalAttributes.FoodStampEligible);
			Assert.AreEqual("1", globalAttributes.PackageUnit);
			Assert.AreEqual("1.3", globalAttributes.RetailSize);
			Assert.AreEqual("LB", globalAttributes.RetailUOM);
			Assert.AreEqual("CHICKEN TANDOORI HB CFD", globalAttributes.Desc_CustomerFriendly);

			var signAttributes = item.SignAttributes;
			Assert.IsNull(signAttributes.Rating_AnimalWelfare);
			Assert.AreEqual(false, signAttributes.IsBiodynamic);
			Assert.IsNull(signAttributes.CheeseMilkType);
			Assert.AreEqual(false, signAttributes.IsCheeseRaw);
			Assert.IsNull(signAttributes.Rating_EcoScale);
			Assert.IsNull(signAttributes.Agency_GlutenFree);
			Assert.AreEqual("None", signAttributes.Rating_HealthyEating);
			Assert.IsNull(signAttributes.Agency_Kosher);
			Assert.AreEqual(false, signAttributes.IsMsc);
			Assert.IsNull(signAttributes.Agency_NonGMO);
			Assert.IsNull(signAttributes.Agency_Organic);
			Assert.AreEqual(false, signAttributes.IsPremiumBodyCare);
			Assert.IsNull(signAttributes.Seafood_FreshOrFrozen);
			Assert.IsNull(signAttributes.Seafood_CatchType);
			Assert.IsNull(signAttributes.Agency_Vegan);
			Assert.AreEqual(false, signAttributes.IsVegetarian);
			Assert.AreEqual(false, signAttributes.IsWholeTrade);
			Assert.AreEqual(false, signAttributes.IsGrassFed);
			Assert.AreEqual(false, signAttributes.IsPastureRaised);
			Assert.AreEqual(false, signAttributes.IsMadeInHouse);
			Assert.AreEqual(false, signAttributes.IsAirChilled);
			Assert.AreEqual(false, signAttributes.IsDryAged);
			Assert.AreEqual(false, signAttributes.IsFreeRange);

			var nutritionAttributes = item.NutritionAttributes;
			Assert.AreEqual("Chicken, Tandoori HB SW", nutritionAttributes.RecipeName);
			Assert.IsNull(nutritionAttributes.Allergens);
			Assert.AreEqual("Ingredients: Chicken, Garbanzo Beans (organic garbanzo beans, water, kombu seaweed), Olive Oil, Organic Tandoori Rub (organic spices [including paprika and turmeric for color], sea salt, organic tapioca starch, organic garlic and onion powder, spice extractive, organic powdered sugar), Organic Black Pepper.", nutritionAttributes.Ingredients);
			Assert.AreEqual(0, nutritionAttributes.HshRating);
			Assert.AreEqual(0.0m, nutritionAttributes.PolyunsaturatedFat);
			Assert.AreEqual(1.5m, nutritionAttributes.MonounsaturatedFat);
			Assert.AreEqual(8.0m, nutritionAttributes.PotassiumWeight);
			Assert.AreEqual(0, nutritionAttributes.PotassiumPercent);
			Assert.AreEqual(2, nutritionAttributes.DietaryFiberPercent);
			Assert.AreEqual(0.0m, nutritionAttributes.SolubleFiber);
			Assert.AreEqual(0.0m, nutritionAttributes.InsolubleFiber);
			Assert.AreEqual(0.0m, nutritionAttributes.SugarAlcohol);
			Assert.AreEqual(0.0m, nutritionAttributes.OtherCarbohydrates);
			Assert.AreEqual(31, nutritionAttributes.ProteinPercent);
			Assert.AreEqual(0, nutritionAttributes.Betacarotene);
			Assert.AreEqual(0, nutritionAttributes.VitaminD);
			Assert.AreEqual(0, nutritionAttributes.VitaminE);
			Assert.AreEqual(0, nutritionAttributes.Thiamin);
			Assert.AreEqual(0, nutritionAttributes.Riboflavin);
			Assert.AreEqual(0, nutritionAttributes.Niacin);
			Assert.AreEqual(0, nutritionAttributes.VitaminB6);
			Assert.AreEqual(0, nutritionAttributes.VitaminB12);
			Assert.AreEqual(0, nutritionAttributes.Biotin);
			Assert.AreEqual(0, nutritionAttributes.PantothenicAcid);
			Assert.AreEqual(0, nutritionAttributes.Phosphorous);
			Assert.AreEqual(0, nutritionAttributes.Iodine);
			Assert.AreEqual(0, nutritionAttributes.Magnesium);
			Assert.AreEqual(0, nutritionAttributes.Zinc);
			Assert.AreEqual(0, nutritionAttributes.Copper);
			Assert.AreEqual(0.0m, nutritionAttributes.TransFat);
			Assert.AreEqual(0.0m, nutritionAttributes.Om6Fatty);
			Assert.AreEqual(0.0m, nutritionAttributes.Om3Fatty);
			Assert.AreEqual(0.0m, nutritionAttributes.Starch);
			Assert.AreEqual(0, nutritionAttributes.Chloride);
			Assert.AreEqual(0, nutritionAttributes.Chromium);
			Assert.AreEqual(0, nutritionAttributes.VitaminK);
			Assert.AreEqual(0, nutritionAttributes.Manganese);
			Assert.AreEqual(0, nutritionAttributes.Molybdenum);
			Assert.AreEqual(0, nutritionAttributes.Selenium);
			Assert.AreEqual(0.0m, nutritionAttributes.TransFatWeight);
			Assert.AreEqual(0, nutritionAttributes.CaloriesFromTransFat);
			Assert.AreEqual(20, nutritionAttributes.CaloriesSaturatedFat);
			Assert.AreEqual("varied", nutritionAttributes.ServingPerContainer);
			Assert.AreEqual("3 oz", nutritionAttributes.ServingSizeDesc);
			Assert.AreEqual(5.30000019073486f, nutritionAttributes.ServingsPerPortion);
			Assert.AreEqual(1, nutritionAttributes.ServingUnits);
			Assert.AreEqual(1, nutritionAttributes.SizeWeight);
			Assert.AreEqual(1, nutritionAttributes.AddedSugarsPercent);
			Assert.AreEqual(2, nutritionAttributes.AddedSugarsWeight);
			Assert.AreEqual(3, nutritionAttributes.CalciumWeight);
			Assert.AreEqual(4, nutritionAttributes.IronWeight);
			Assert.AreEqual(5, nutritionAttributes.VitaminDWeight);

			var extAttributes = item.ExtendedAttributes;
			Assert.AreEqual("Fair Trade Certified Test", extAttributes.FairTradeCertified);
			Assert.AreEqual("Flexible Text Test", extAttributes.FlexibleText);
			Assert.AreEqual("Made With Organic Grapes Test", extAttributes.MadeWithOrganicGrapes);
			Assert.AreEqual("Made with Biodynamic Grapes Test", extAttributes.MadeWithBiodynamicGrapes);
			Assert.AreEqual("1", extAttributes.NutritionRequired);
			Assert.AreEqual("Prime Beef Test", extAttributes.PrimeBeef);
			Assert.AreEqual("Rainforest Alliance Test", extAttributes.RainforestAlliance);
			Assert.AreEqual("Refrigerated or Shelf Stable Test", extAttributes.RefrigeratedOrShelfStable);
			Assert.AreEqual("Smithsonian Bird Friendly Test", extAttributes.SmithsonianBirdFriendly);
			Assert.AreEqual("WIC Test", extAttributes.Wic);
			Assert.AreEqual("Global Pricing Program Test", extAttributes.GlobalPricingProgram);

			var kitItemAttributes = item.KitItemAttributes;
			Assert.AreEqual("KitDescription", kitItemAttributes.KitchenDescription);
			Assert.AreEqual(true, kitItemAttributes.KitchenItem);
			Assert.AreEqual(false, kitItemAttributes.HospitalityItem);
			Assert.AreEqual("https://arboretum.orangetheoryfitness.com/", kitItemAttributes.ImageUrl);
		}

		[TestMethod]
		public void ParseMessage_1ItemInMessageWithNoNutritionData_ReturnItemModelWithNullNutritionAttributes()
		{

			//Given
			Mock<IEsbMessage> message = new Mock<IEsbMessage>();
			message.Setup(m => m.MessageText)
				.Returns(File.ReadAllText("TestMessages/ItemWithNoNutritionAttributes.xml"));

			//When
			var itemModels = parser.ParseMessage(message.Object);

			//Then
			Assert.AreEqual(1, itemModels.Count);

			var item = itemModels[0];
			var globalAttributes = item.GlobalAttributes;
			Assert.AreEqual(4069492, globalAttributes.ItemID);
			Assert.AreEqual(ItemTypes.RetailSale, globalAttributes.ItemTypeID);
			Assert.AreEqual("73197519964", globalAttributes.ScanCode);
			Assert.AreEqual(84563, globalAttributes.SubBrickID);
			Assert.AreEqual(56620, globalAttributes.BrandHCID);
			Assert.AreEqual("0115002", globalAttributes.MessageTaxClassHCID);
			Assert.AreEqual(7200, globalAttributes.PSNumber);
			Assert.AreEqual("MUG RED WHITE", globalAttributes.Desc_Product);
			Assert.AreEqual("JCBS MUG RED WHITE", globalAttributes.Desc_POS);
			Assert.AreEqual(true, globalAttributes.FoodStampEligible);
			Assert.AreEqual("1", globalAttributes.PackageUnit);
			Assert.AreEqual("1", globalAttributes.RetailSize);
			Assert.AreEqual("EA", globalAttributes.RetailUOM);

			var signAttributes = item.SignAttributes;
			Assert.IsNull(signAttributes.Rating_AnimalWelfare);
			Assert.AreEqual(false, signAttributes.IsBiodynamic);
			Assert.IsNull(signAttributes.CheeseMilkType);
			Assert.AreEqual(false, signAttributes.IsCheeseRaw);
			Assert.IsNull(signAttributes.Rating_EcoScale);
			Assert.IsNull(signAttributes.Agency_GlutenFree);
			Assert.IsNull(signAttributes.Rating_HealthyEating);
			Assert.IsNull(signAttributes.Agency_Kosher);
			Assert.AreEqual(false, signAttributes.IsMsc);
			Assert.IsNull(signAttributes.Agency_NonGMO);
			Assert.IsNull(signAttributes.Agency_Organic);
			Assert.AreEqual(false, signAttributes.IsPremiumBodyCare);
			Assert.IsNull(signAttributes.Seafood_FreshOrFrozen);
			Assert.IsNull(signAttributes.Seafood_CatchType);
			Assert.IsNull(signAttributes.Agency_Vegan);
			Assert.AreEqual(false, signAttributes.IsVegetarian);
			Assert.AreEqual(false, signAttributes.IsWholeTrade);
			Assert.AreEqual(false, signAttributes.IsGrassFed);
			Assert.AreEqual(false, signAttributes.IsPastureRaised);
			Assert.AreEqual(false, signAttributes.IsMadeInHouse);
			Assert.AreEqual(false, signAttributes.IsAirChilled);
			Assert.AreEqual(false, signAttributes.IsDryAged);
			Assert.AreEqual(false, signAttributes.IsFreeRange);

			var nutritionAttributes = item.NutritionAttributes;
			Assert.IsNull(item.NutritionAttributes);

			var kitItemAttributes = item.KitItemAttributes;
			Assert.AreEqual("KitDescription", kitItemAttributes.KitchenDescription);
			Assert.AreEqual(true, kitItemAttributes.KitchenItem);
			Assert.AreEqual(false, kitItemAttributes.HospitalityItem);
			Assert.AreEqual("https://arboretum.orangetheoryfitness.com/", kitItemAttributes.ImageUrl);
		}

		[TestMethod]
		public void ParseMessage_1ItemInMessageWithNULLProductLabelAttributes_ReturnItemModel()
		{
			//Given
			Mock<IEsbMessage> message = new Mock<IEsbMessage>();
			message.Setup(m => m.MessageText)
				.Returns(File.ReadAllText("TestMessages/ItemWithNullConsumerProductLabelAttributes.xml"));

			//When
			var itemModels = parser.ParseMessage(message.Object);

			//Then
			Assert.AreEqual(1, itemModels.Count);

			var item = itemModels[0];
			var globalAttributes = item.GlobalAttributes;
			Assert.AreEqual(4067344, globalAttributes.ItemID);
			Assert.AreEqual(ItemTypes.RetailSale, globalAttributes.ItemTypeID);
			Assert.AreEqual("46000012881", globalAttributes.ScanCode);
			Assert.AreEqual(132626, globalAttributes.SubBrickID);
			Assert.AreEqual(50883, globalAttributes.BrandHCID);
			Assert.AreEqual("9989000", globalAttributes.MessageTaxClassHCID);
			Assert.AreEqual(4900, globalAttributes.PSNumber);
			Assert.AreEqual("CHICKEN TANDOORI HB", globalAttributes.Desc_Product);
			Assert.AreEqual("INGREDIENT", globalAttributes.Desc_POS);
			Assert.AreEqual(false, globalAttributes.FoodStampEligible);
			Assert.AreEqual("1", globalAttributes.PackageUnit);
			Assert.AreEqual("1.3", globalAttributes.RetailSize);
			Assert.AreEqual("LB", globalAttributes.RetailUOM);
			Assert.AreEqual("CHICKEN TANDOORI HB CFD", globalAttributes.Desc_CustomerFriendly);

			var signAttributes = item.SignAttributes;
			Assert.IsNull(signAttributes.Rating_AnimalWelfare);
			Assert.AreEqual(false, signAttributes.IsBiodynamic);
			Assert.IsNull(signAttributes.CheeseMilkType);
			Assert.AreEqual(false, signAttributes.IsCheeseRaw);
			Assert.IsNull(signAttributes.Rating_EcoScale);
			Assert.IsNull(signAttributes.Agency_GlutenFree);
			Assert.AreEqual("None", signAttributes.Rating_HealthyEating);
			Assert.IsNull(signAttributes.Agency_Kosher);
			Assert.AreEqual(false, signAttributes.IsMsc);
			Assert.IsNull(signAttributes.Agency_NonGMO);
			Assert.IsNull(signAttributes.Agency_Organic);
			Assert.AreEqual(false, signAttributes.IsPremiumBodyCare);
			Assert.IsNull(signAttributes.Seafood_FreshOrFrozen);
			Assert.IsNull(signAttributes.Seafood_CatchType);
			Assert.IsNull(signAttributes.Agency_Vegan);
			Assert.AreEqual(false, signAttributes.IsVegetarian);
			Assert.AreEqual(false, signAttributes.IsWholeTrade);
			Assert.AreEqual(false, signAttributes.IsGrassFed);
			Assert.AreEqual(false, signAttributes.IsPastureRaised);
			Assert.AreEqual(false, signAttributes.IsMadeInHouse);
			Assert.AreEqual(false, signAttributes.IsAirChilled);
			Assert.AreEqual(false, signAttributes.IsDryAged);
			Assert.AreEqual(false, signAttributes.IsFreeRange);

			var nutritionAttributes = item.NutritionAttributes;
			Assert.AreEqual("Chicken, Tandoori HB SW", nutritionAttributes.RecipeName);
			Assert.IsNull(nutritionAttributes.Allergens);
			Assert.AreEqual("Ingredients: Chicken, Garbanzo Beans (organic garbanzo beans, water, kombu seaweed), Olive Oil, Organic Tandoori Rub (organic spices [including paprika and turmeric for color], sea salt, organic tapioca starch, organic garlic and onion powder, spice extractive, organic powdered sugar), Organic Black Pepper.", nutritionAttributes.Ingredients);
			Assert.AreEqual(0, nutritionAttributes.HshRating);
			Assert.AreEqual(0.0m, nutritionAttributes.PolyunsaturatedFat);
			Assert.AreEqual(1.5m, nutritionAttributes.MonounsaturatedFat);
			Assert.AreEqual(8.0m, nutritionAttributes.PotassiumWeight);
			Assert.AreEqual(0, nutritionAttributes.PotassiumPercent);
			Assert.AreEqual(2, nutritionAttributes.DietaryFiberPercent);
			Assert.AreEqual(0.0m, nutritionAttributes.SolubleFiber);
			Assert.AreEqual(0.0m, nutritionAttributes.InsolubleFiber);
			Assert.AreEqual(0.0m, nutritionAttributes.SugarAlcohol);
			Assert.AreEqual(0.0m, nutritionAttributes.OtherCarbohydrates);
			Assert.AreEqual(31, nutritionAttributes.ProteinPercent);
			Assert.AreEqual(0, nutritionAttributes.Betacarotene);
			Assert.AreEqual(0, nutritionAttributes.VitaminD);
			Assert.AreEqual(0, nutritionAttributes.VitaminE);
			Assert.AreEqual(0, nutritionAttributes.Thiamin);
			Assert.AreEqual(0, nutritionAttributes.Riboflavin);
			Assert.AreEqual(0, nutritionAttributes.Niacin);
			Assert.AreEqual(0, nutritionAttributes.VitaminB6);
			Assert.AreEqual(0, nutritionAttributes.VitaminB12);
			Assert.AreEqual(0, nutritionAttributes.Biotin);
			Assert.AreEqual(0, nutritionAttributes.PantothenicAcid);
			Assert.AreEqual(0, nutritionAttributes.Phosphorous);
			Assert.AreEqual(0, nutritionAttributes.Iodine);
			Assert.AreEqual(0, nutritionAttributes.Magnesium);
			Assert.AreEqual(0, nutritionAttributes.Zinc);
			Assert.AreEqual(0, nutritionAttributes.Copper);
			Assert.AreEqual(0.0m, nutritionAttributes.TransFat);
			Assert.AreEqual(0.0m, nutritionAttributes.Om6Fatty);
			Assert.AreEqual(0.0m, nutritionAttributes.Om3Fatty);
			Assert.AreEqual(0.0m, nutritionAttributes.Starch);
			Assert.AreEqual(0, nutritionAttributes.Chloride);
			Assert.AreEqual(0, nutritionAttributes.Chromium);
			Assert.AreEqual(0, nutritionAttributes.VitaminK);
			Assert.AreEqual(0, nutritionAttributes.Manganese);
			Assert.AreEqual(0, nutritionAttributes.Molybdenum);
			Assert.AreEqual(0, nutritionAttributes.Selenium);
			Assert.AreEqual(0, nutritionAttributes.TransFatWeight);
			Assert.AreEqual(0, nutritionAttributes.CaloriesFromTransFat);
			Assert.AreEqual(20, nutritionAttributes.CaloriesSaturatedFat);
			Assert.AreEqual("varied", nutritionAttributes.ServingPerContainer);
			Assert.AreEqual("3 oz", nutritionAttributes.ServingSizeDesc);
			Assert.AreEqual(5.30000019073486f, nutritionAttributes.ServingsPerPortion);
			Assert.AreEqual(1, nutritionAttributes.ServingUnits);
			Assert.AreEqual(1, nutritionAttributes.SizeWeight);
			Assert.IsNull(nutritionAttributes.AddedSugarsPercent);
			Assert.IsNull(nutritionAttributes.AddedSugarsWeight);
			Assert.IsNull(nutritionAttributes.CalciumWeight);
			Assert.IsNull(nutritionAttributes.IronWeight);
			Assert.IsNull(nutritionAttributes.VitaminDWeight);

			var extAttributes = item.ExtendedAttributes;
			Assert.AreEqual("Fair Trade Certified Test", extAttributes.FairTradeCertified);
			Assert.AreEqual("Flexible Text Test", extAttributes.FlexibleText);
			Assert.AreEqual("Made With Organic Grapes Test", extAttributes.MadeWithOrganicGrapes);
			Assert.AreEqual("Made with Biodynamic Grapes Test", extAttributes.MadeWithBiodynamicGrapes);
			Assert.AreEqual("1", extAttributes.NutritionRequired);
			Assert.AreEqual("Prime Beef Test", extAttributes.PrimeBeef);
			Assert.AreEqual("Rainforest Alliance Test", extAttributes.RainforestAlliance);
			Assert.AreEqual("Refrigerated or Shelf Stable Test", extAttributes.RefrigeratedOrShelfStable);
			Assert.AreEqual("Smithsonian Bird Friendly Test", extAttributes.SmithsonianBirdFriendly);
			Assert.AreEqual("WIC Test", extAttributes.Wic);
			Assert.AreEqual("Global Pricing Program Test", extAttributes.GlobalPricingProgram);

			var kitItemAttributes = item.KitItemAttributes;
			Assert.AreEqual("KitDescription", kitItemAttributes.KitchenDescription);
			Assert.AreEqual(true, kitItemAttributes.KitchenItem);
			Assert.AreEqual(false, kitItemAttributes.HospitalityItem);
			Assert.AreEqual("https://arboretum.orangetheoryfitness.com/", kitItemAttributes.ImageUrl);
		}

		[TestMethod]
		public void ParseMessage_1ItemInMessageWithZeroProductLabelAttributes_ReturnItemModel()
		{
			//Given
			Mock<IEsbMessage> message = new Mock<IEsbMessage>();
			message.Setup(m => m.MessageText)
				.Returns(File.ReadAllText("TestMessages/ItemWithZeroConsumerProductLabelAttributes.xml"));

			//When
			var itemModels = parser.ParseMessage(message.Object);

			//Then
			Assert.AreEqual(1, itemModels.Count);

			var item = itemModels[0];
			var globalAttributes = item.GlobalAttributes;
			Assert.AreEqual(4067344, globalAttributes.ItemID);
			Assert.AreEqual(ItemTypes.RetailSale, globalAttributes.ItemTypeID);
			Assert.AreEqual("46000012881", globalAttributes.ScanCode);
			Assert.AreEqual(132626, globalAttributes.SubBrickID);
			Assert.AreEqual(50883, globalAttributes.BrandHCID);
			Assert.AreEqual("9989000", globalAttributes.MessageTaxClassHCID);
			Assert.AreEqual(4900, globalAttributes.PSNumber);
			Assert.AreEqual("CHICKEN TANDOORI HB", globalAttributes.Desc_Product);
			Assert.AreEqual("INGREDIENT", globalAttributes.Desc_POS);
			Assert.AreEqual(false, globalAttributes.FoodStampEligible);
			Assert.AreEqual("1", globalAttributes.PackageUnit);
			Assert.AreEqual("1.3", globalAttributes.RetailSize);
			Assert.AreEqual("LB", globalAttributes.RetailUOM);
			Assert.AreEqual("CHICKEN TANDOORI HB CFD", globalAttributes.Desc_CustomerFriendly);

			var signAttributes = item.SignAttributes;
			Assert.IsNull(signAttributes.Rating_AnimalWelfare);
			Assert.AreEqual(false, signAttributes.IsBiodynamic);
			Assert.IsNull(signAttributes.CheeseMilkType);
			Assert.AreEqual(false, signAttributes.IsCheeseRaw);
			Assert.IsNull(signAttributes.Rating_EcoScale);
			Assert.IsNull(signAttributes.Agency_GlutenFree);
			Assert.AreEqual("None", signAttributes.Rating_HealthyEating);
			Assert.IsNull(signAttributes.Agency_Kosher);
			Assert.AreEqual(false, signAttributes.IsMsc);
			Assert.IsNull(signAttributes.Agency_NonGMO);
			Assert.IsNull(signAttributes.Agency_Organic);
			Assert.AreEqual(false, signAttributes.IsPremiumBodyCare);
			Assert.IsNull(signAttributes.Seafood_FreshOrFrozen);
			Assert.IsNull(signAttributes.Seafood_CatchType);
			Assert.IsNull(signAttributes.Agency_Vegan);
			Assert.AreEqual(false, signAttributes.IsVegetarian);
			Assert.AreEqual(false, signAttributes.IsWholeTrade);
			Assert.AreEqual(false, signAttributes.IsGrassFed);
			Assert.AreEqual(false, signAttributes.IsPastureRaised);
			Assert.AreEqual(false, signAttributes.IsMadeInHouse);
			Assert.AreEqual(false, signAttributes.IsAirChilled);
			Assert.AreEqual(false, signAttributes.IsDryAged);
			Assert.AreEqual(false, signAttributes.IsFreeRange);

			var nutritionAttributes = item.NutritionAttributes;
			Assert.AreEqual("Chicken, Tandoori HB SW", nutritionAttributes.RecipeName);
			Assert.IsNull(nutritionAttributes.Allergens);
			Assert.AreEqual("Ingredients: Chicken, Garbanzo Beans (organic garbanzo beans, water, kombu seaweed), Olive Oil, Organic Tandoori Rub (organic spices [including paprika and turmeric for color], sea salt, organic tapioca starch, organic garlic and onion powder, spice extractive, organic powdered sugar), Organic Black Pepper.", nutritionAttributes.Ingredients);
			Assert.AreEqual(0, nutritionAttributes.HshRating);
			Assert.AreEqual(0.0m, nutritionAttributes.PolyunsaturatedFat);
			Assert.AreEqual(1.5m, nutritionAttributes.MonounsaturatedFat);
			Assert.AreEqual(8.0m, nutritionAttributes.PotassiumWeight);
			Assert.AreEqual(0, nutritionAttributes.PotassiumPercent);
			Assert.AreEqual(2, nutritionAttributes.DietaryFiberPercent);
			Assert.AreEqual(0.0m, nutritionAttributes.SolubleFiber);
			Assert.AreEqual(0.0m, nutritionAttributes.InsolubleFiber);
			Assert.AreEqual(0.0m, nutritionAttributes.SugarAlcohol);
			Assert.AreEqual(0.0m, nutritionAttributes.OtherCarbohydrates);
			Assert.AreEqual(31, nutritionAttributes.ProteinPercent);
			Assert.AreEqual(0, nutritionAttributes.Betacarotene);
			Assert.AreEqual(0, nutritionAttributes.VitaminD);
			Assert.AreEqual(0, nutritionAttributes.VitaminE);
			Assert.AreEqual(0, nutritionAttributes.Thiamin);
			Assert.AreEqual(0, nutritionAttributes.Riboflavin);
			Assert.AreEqual(0, nutritionAttributes.Niacin);
			Assert.AreEqual(0, nutritionAttributes.VitaminB6);
			Assert.AreEqual(0, nutritionAttributes.VitaminB12);
			Assert.AreEqual(0, nutritionAttributes.Biotin);
			Assert.AreEqual(0, nutritionAttributes.PantothenicAcid);
			Assert.AreEqual(0, nutritionAttributes.Phosphorous);
			Assert.AreEqual(0, nutritionAttributes.Iodine);
			Assert.AreEqual(0, nutritionAttributes.Magnesium);
			Assert.AreEqual(0, nutritionAttributes.Zinc);
			Assert.AreEqual(0, nutritionAttributes.Copper);
			Assert.AreEqual(0.0m, nutritionAttributes.TransFat);
			Assert.AreEqual(0.0m, nutritionAttributes.Om6Fatty);
			Assert.AreEqual(0.0m, nutritionAttributes.Om3Fatty);
			Assert.AreEqual(0.0m, nutritionAttributes.Starch);
			Assert.AreEqual(0, nutritionAttributes.Chloride);
			Assert.AreEqual(0, nutritionAttributes.Chromium);
			Assert.AreEqual(0, nutritionAttributes.VitaminK);
			Assert.AreEqual(0, nutritionAttributes.Manganese);
			Assert.AreEqual(0, nutritionAttributes.Molybdenum);
			Assert.AreEqual(0, nutritionAttributes.Selenium);
			Assert.AreEqual(0.0m, nutritionAttributes.TransFatWeight);
			Assert.AreEqual(0, nutritionAttributes.CaloriesFromTransFat);
			Assert.AreEqual(20, nutritionAttributes.CaloriesSaturatedFat);
			Assert.AreEqual("varied", nutritionAttributes.ServingPerContainer);
			Assert.AreEqual("3 oz", nutritionAttributes.ServingSizeDesc);
			Assert.AreEqual(5.30000019073486f, nutritionAttributes.ServingsPerPortion);
			Assert.AreEqual(1, nutritionAttributes.ServingUnits);
			Assert.AreEqual(1, nutritionAttributes.SizeWeight);
			Assert.AreEqual(0, nutritionAttributes.AddedSugarsPercent);
			Assert.AreEqual(0, nutritionAttributes.AddedSugarsWeight);
			Assert.AreEqual(0, nutritionAttributes.CalciumWeight);
			Assert.AreEqual(0, nutritionAttributes.IronWeight);
			Assert.AreEqual(0, nutritionAttributes.VitaminDWeight);

			var extAttributes = item.ExtendedAttributes;
			Assert.AreEqual("Fair Trade Certified Test", extAttributes.FairTradeCertified);
			Assert.AreEqual("Flexible Text Test", extAttributes.FlexibleText);
			Assert.AreEqual("Made With Organic Grapes Test", extAttributes.MadeWithOrganicGrapes);
			Assert.AreEqual("Made with Biodynamic Grapes Test", extAttributes.MadeWithBiodynamicGrapes);
			Assert.AreEqual("1", extAttributes.NutritionRequired);
			Assert.AreEqual("Prime Beef Test", extAttributes.PrimeBeef);
			Assert.AreEqual("Rainforest Alliance Test", extAttributes.RainforestAlliance);
			Assert.AreEqual("Refrigerated or Shelf Stable Test", extAttributes.RefrigeratedOrShelfStable);
			Assert.AreEqual("Smithsonian Bird Friendly Test", extAttributes.SmithsonianBirdFriendly);
			Assert.AreEqual("WIC Test", extAttributes.Wic);
			Assert.AreEqual("Global Pricing Program Test", extAttributes.GlobalPricingProgram);

			var kitItemAttributes = item.KitItemAttributes;
			Assert.AreEqual("KitDescription", kitItemAttributes.KitchenDescription);
			Assert.AreEqual(true, kitItemAttributes.KitchenItem);
			Assert.AreEqual(false, kitItemAttributes.HospitalityItem);
			Assert.AreEqual("https://arboretum.orangetheoryfitness.com/", kitItemAttributes.ImageUrl);
		}
	}
}