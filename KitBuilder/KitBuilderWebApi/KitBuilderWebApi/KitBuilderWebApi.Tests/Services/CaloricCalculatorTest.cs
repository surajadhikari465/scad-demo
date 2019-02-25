using AutoMapper;
using KitBuilderWebApi.Helper;
using KitBuilderWebApi.Services;
using KitBuilderWebApi.QueryParameters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using KitBuilder.DataAccess.DatabaseModels;
using KitBuilder.DataAccess.Dto;
using KitBuilder.DataAccess.Repository;
using KitBuilder.DataAccess.Queries;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Microsoft.DotNet.PlatformAbstractions;
using KitBuilder.DataAccess.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KitBuilderWebApi.Tests.Services
{
	[TestClass]
	public class CaloricCalculatorTest
	{
		private CaloricCalculator caloricCalculator;
		private string projectPath;
		private Mock<IUnitOfWork> mockUnitWork;
		private Mock<IRepository<KitLocale>> mockKitLocaleRepository;
		private Mock<IRepository<Locale>> mockLocaleRepository;
		private Mock<IService<IEnumerable<StoreItem>, Task<IEnumerable<ItemStorePriceModel>>>> mockGetAuthorizedStatusAndPriceService;
		private Mock<IService<ItemNutritionRequestModel, Task<IEnumerable<ItemNutritionAttributesDictionary>>>> mockGetNutritionService;
		private Mock<ILogger<CaloricCalculator>> mockLogger;

		private const int KITLOCALEID = 179;
		private const int STORELOCALElEID = 924;

		[TestInitialize]
		public void InitializeTest()
		{
			mockKitLocaleRepository = new Mock<IRepository<KitLocale>>();
			mockLocaleRepository = new Mock<IRepository<Locale>>();
			mockGetAuthorizedStatusAndPriceService = new Mock<IService<IEnumerable<StoreItem>, Task<IEnumerable<ItemStorePriceModel>>>>();
			mockGetNutritionService = new Mock<IService<ItemNutritionRequestModel, Task<IEnumerable<ItemNutritionAttributesDictionary>>>>();
			mockLogger = new Mock<ILogger<CaloricCalculator>>();

			caloricCalculator = new CaloricCalculator(mockKitLocaleRepository.Object,
				mockLocaleRepository.Object,
				mockGetAuthorizedStatusAndPriceService.Object,
				mockGetNutritionService.Object,
				mockLogger.Object);

			mockUnitWork = new Mock<IUnitOfWork>();

			MappingHelper.InitializeMapper();

			SetUpDataAndRepository();
		}

		[TestCleanup]
		public void Cleanup()
		{
			Mapper.Reset();
		}

		[TestMethod]
		public void CaloricCalculator_AllIncludedAndAuthorized_ReturnsMinMaxKitLocaleCalories()
		{
			//Given
			string filePath = Path.Combine(projectPath, "TestData", "KitLocale_AllIncluded.Json");
			string json = System.IO.File.ReadAllText(filePath);
			List<KitLocale> kitLocales = JsonConvert.DeserializeObject<List<KitLocale>>(json);

			filePath = Path.Combine(projectPath, "TestData", "ItemStorePriceModelList_AllAuthorized.Json");
			json = System.IO.File.ReadAllText(filePath);
			IEnumerable<ItemStorePriceModel> itemStorePriceModelList = JsonConvert.DeserializeObject<IEnumerable<ItemStorePriceModel>>(json);

			filePath = Path.Combine(projectPath, "TestData", "ItemCaloriesList.Json");
			json = System.IO.File.ReadAllText(filePath);
			IEnumerable<ItemNutritionAttributesDictionary> itemCaloriesList = JsonConvert.DeserializeObject<IEnumerable<ItemNutritionAttributesDictionary>>(json);

			var mockContext = new Mock<KitBuilderContext>();
			var mockDbSet = GetMockDbSet<KitLocale>(kitLocales);
			mockContext.Setup(c => c.Set<KitLocale>()).Returns(mockDbSet.Object);

			mockKitLocaleRepository.Setup(m => m.GetAll()).Returns(kitLocales.AsQueryable());
			mockContext.Setup(m => m.KitLocale).Returns(mockDbSet.Object);

			mockGetAuthorizedStatusAndPriceService.Setup(p => p.Run(It.IsAny<IEnumerable<StoreItem>>())).ReturnsAsync(itemStorePriceModelList);
			mockGetNutritionService.Setup(n => n.Run(It.IsAny<ItemNutritionRequestModel>())).ReturnsAsync(itemCaloriesList);

			//When
			GetKitLocaleByStoreParameters parameters = new GetKitLocaleByStoreParameters
			{
				KitLocaleId = KITLOCALEID,
				StoreLocaleId = STORELOCALElEID
			};
			Task<KitLocaleDto> kitLocaleDto = caloricCalculator.Run(parameters);

			//Then
			Assert.AreEqual(true, kitLocaleDto.Result.AuthorizedByStore, "Kit main item authorization status is wrong.");
			Assert.AreEqual(850, kitLocaleDto.Result.MinimumCalories, "Kit minimum calories is retrived/mapped wrong.");
			Assert.AreEqual(830, kitLocaleDto.Result.KitLinkGroupLocale.Where(k => k.KitLinkGroupLocaleId == 50).Select(k =>  k.MaximumCalories).FirstOrDefault(), "Kit first link group maximum calories is caculated wrong.");
			Assert.AreEqual(330, kitLocaleDto.Result.KitLinkGroupLocale.Where(k => k.KitLinkGroupLocaleId == 51).Select(k => k.MaximumCalories).FirstOrDefault(), "Kit second link group maximum calories is caculated wrong.");
			Assert.AreEqual(400, kitLocaleDto.Result.KitLinkGroupLocale.Where(k => k.KitLinkGroupLocaleId == 52).Select(k => k.MaximumCalories).FirstOrDefault(), "Kit third link group maximum calories is caculated wrong.");
			Assert.AreEqual(1560, kitLocaleDto.Result.MaximumCalories, "Kit max calories is calculated wrong.");
		}

		[TestMethod]
		public void CaloricCalculator_SomeExcludedAndUnauthorized_ReturnsMinMaxKitLocaleCalories()
		{
			//Given
			string filePath = Path.Combine(projectPath, "TestData", "KitLocale_ExcludedAndUnauthorized.Json");
			string json = System.IO.File.ReadAllText(filePath);
			List<KitLocale> kitLocales = JsonConvert.DeserializeObject<List<KitLocale>>(json);

			filePath = Path.Combine(projectPath, "TestData", "ItemStorePriceModelList_Unauthorized.Json");
			json = System.IO.File.ReadAllText(filePath);
			IEnumerable<ItemStorePriceModel> itemStorePriceModelList = JsonConvert.DeserializeObject<IEnumerable<ItemStorePriceModel>>(json);

			//Add kit main item to itemStorePriceModelList (results from the Mammoth price call), and mock it to be the authorized for the store.
			string kitMainItemString = "{\"ItemId\": 1862661,\"ScanCode\": \"28838000000\",\"BusinessUnitID\": 10316,\"Authorized\": true,\"Multiple\": 1,\"Price\": 9.99,\"Currency\": \"USD\"}";
			ItemStorePriceModel kitMainItem = JsonConvert.DeserializeObject<ItemStorePriceModel>(kitMainItemString);
			itemStorePriceModelList = itemStorePriceModelList.Append(kitMainItem);

			filePath = Path.Combine(projectPath, "TestData", "ItemCaloriesList.Json");
			json = System.IO.File.ReadAllText(filePath);
			IEnumerable<ItemNutritionAttributesDictionary> itemCaloriesList = JsonConvert.DeserializeObject<IEnumerable<ItemNutritionAttributesDictionary>>(json);

			var mockContext = new Mock<KitBuilderContext>();
			var mockDbSet = GetMockDbSet<KitLocale>(kitLocales);
			mockContext.Setup(c => c.Set<KitLocale>()).Returns(mockDbSet.Object);

			mockKitLocaleRepository.Setup(m => m.GetAll()).Returns(kitLocales.AsQueryable());
			mockContext.Setup(m => m.KitLocale).Returns(mockDbSet.Object);

			mockGetAuthorizedStatusAndPriceService.Setup(p => p.Run(It.IsAny<IEnumerable<StoreItem>>())).ReturnsAsync(itemStorePriceModelList);
			mockGetNutritionService.Setup(n => n.Run(It.IsAny<ItemNutritionRequestModel>())).ReturnsAsync(itemCaloriesList);

			//When
			GetKitLocaleByStoreParameters parameters = new GetKitLocaleByStoreParameters
			{
				KitLocaleId = KITLOCALEID,
				StoreLocaleId = STORELOCALElEID
			};
			Task<KitLocaleDto> kitLocaleDto = caloricCalculator.Run(parameters);

			//Then
			Assert.AreEqual(true, kitLocaleDto.Result.AuthorizedByStore, "Kit main item authorization status is wrong.");
			Assert.AreEqual(850, kitLocaleDto.Result.MinimumCalories, "Kit minimum calories is retrived/mapped wrong.");
			Assert.IsNull(kitLocaleDto.Result.KitLinkGroupLocale.Where(k => k.KitLinkGroupLocaleId == 50).Select(k => k.MaximumCalories).FirstOrDefault(), "Kit first link group maximum calories is caculated wrong.");
			Assert.AreEqual(200, kitLocaleDto.Result.KitLinkGroupLocale.Where(k => k.KitLinkGroupLocaleId == 51).Select(k => k.MaximumCalories).FirstOrDefault(), "Kit second link group maximum calories is caculated wrong.");
			Assert.AreEqual(360, kitLocaleDto.Result.KitLinkGroupLocale.Where(k => k.KitLinkGroupLocaleId == 52).Select(k => k.MaximumCalories).FirstOrDefault(), "Kit third link group maximum calories is caculated wrong.");
			Assert.AreEqual(560, kitLocaleDto.Result.MaximumCalories, "Kit max calories is calculated wrong.");
		}

		[TestMethod]
		public void CaloricCalculator_KitMainItemUnauthorized_LogErrorReturnKitLocaleWithAuthStatusOnly()
		{
			//Given
			string filePath = Path.Combine(projectPath, "TestData", "KitLocale_ExcludedAndUnauthorized.Json");
			string json = System.IO.File.ReadAllText(filePath);
			List<KitLocale> kitLocales = JsonConvert.DeserializeObject<List<KitLocale>>(json);

			//In this file, the kit main item is not on the file; therefore it will be treated as the main item is not authorized for the store.
			filePath = Path.Combine(projectPath, "TestData", "ItemStorePriceModelList_Unauthorized.Json");
			json = System.IO.File.ReadAllText(filePath);
			IEnumerable<ItemStorePriceModel> itemStorePriceModelList = JsonConvert.DeserializeObject<IEnumerable<ItemStorePriceModel>>(json);

			filePath = Path.Combine(projectPath, "TestData", "ItemCaloriesList.Json");
			json = System.IO.File.ReadAllText(filePath);
			IEnumerable<ItemNutritionAttributesDictionary> itemCaloriesList = JsonConvert.DeserializeObject<IEnumerable<ItemNutritionAttributesDictionary>>(json);

			var mockContext = new Mock<KitBuilderContext>();
			var mockDbSet = GetMockDbSet<KitLocale>(kitLocales);
			mockContext.Setup(c => c.Set<KitLocale>()).Returns(mockDbSet.Object);

			mockKitLocaleRepository.Setup(m => m.GetAll()).Returns(kitLocales.AsQueryable());
			mockContext.Setup(m => m.KitLocale).Returns(mockDbSet.Object);

			mockGetAuthorizedStatusAndPriceService.Setup(p => p.Run(It.IsAny<IEnumerable<StoreItem>>())).ReturnsAsync(itemStorePriceModelList);
			mockGetNutritionService.Setup(n => n.Run(It.IsAny<ItemNutritionRequestModel>())).ReturnsAsync(itemCaloriesList);

			//When
			GetKitLocaleByStoreParameters parameters = new GetKitLocaleByStoreParameters
			{
				KitLocaleId = KITLOCALEID,
				StoreLocaleId = STORELOCALElEID
			};
			Task<KitLocaleDto> kitLocaleDto = caloricCalculator.Run(parameters);

			//Then
			Assert.AreEqual(false, kitLocaleDto.Result.AuthorizedByStore, "Kit main item authorization status is wrong.");
			Assert.IsNull(kitLocaleDto.Result.MinimumCalories, "Kit minimum calories is retrived/mapped wrong.");
			Assert.IsNull(kitLocaleDto.Result.KitLinkGroupLocale.Where(k => k.KitLinkGroupLocaleId == 50).Select(k => k.MaximumCalories).FirstOrDefault(), "Kit first link group maximum calories is caculated wrong.");
			Assert.IsNull(kitLocaleDto.Result.KitLinkGroupLocale.Where(k => k.KitLinkGroupLocaleId == 51).Select(k => k.MaximumCalories).FirstOrDefault(), "Kit second link group maximum calories is caculated wrong.");
			Assert.IsNull(kitLocaleDto.Result.KitLinkGroupLocale.Where(k => k.KitLinkGroupLocaleId == 52).Select(k => k.MaximumCalories).FirstOrDefault(), "Kit third link group maximum calories is caculated wrong.");
			Assert.IsNull(kitLocaleDto.Result.MaximumCalories, "Kit max calories is calculated wrong.");
			//This will be checked when standard logging is set up.
			//this.mockLogger.Verify(l => l.Warn(It.Is<string>(s => s == "The kit main item 1862661 is not authorized for store with Locale Id of 924.")), Times.Once);
		}

		private void SetUpDataAndRepository()
		{
			string startupPath = ApplicationEnvironment.ApplicationBasePath;
			var pathItems = startupPath.Split(Path.DirectorySeparatorChar);
			var pos = pathItems.Reverse().ToList().FindIndex(x => string.Equals("bin", x));
			projectPath = String.Join(Path.DirectorySeparatorChar.ToString(), pathItems.Take(pathItems.Length - pos - 1));

			string filePath = Path.Combine(projectPath, "TestData", "LocaleRepository.Json");
			string json = System.IO.File.ReadAllText(filePath);
			List<Locale> locale = JsonConvert.DeserializeObject<List<Locale>>(json);

			var mockContext = new Mock<KitBuilderContext>();
			var mockDbSet = GetMockDbSet<Locale>(locale);
			mockContext.Setup(c => c.Set<Locale>()).Returns(mockDbSet.Object);

			mockLocaleRepository.SetupGet(s => s.UnitOfWork).Returns(mockUnitWork.Object);
			mockContext.Setup(m => m.Locale).Returns(mockDbSet.Object);
		}

		private Mock<DbSet<T>> GetMockDbSet<T>(List<T> objectPassed) where T : class
		{
			var mockSet = new Mock<DbSet<T>>();
			mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(objectPassed.AsQueryable().Provider);
			mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(objectPassed.AsQueryable().Expression);
			mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(objectPassed.AsQueryable().ElementType);
			mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(objectPassed.AsQueryable().GetEnumerator());

			return mockSet;
		}
	}
}
