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

namespace KitBuilderWebApi.Tests.Services
{
	[TestClass]
	public class CaloricCalculatorTest
	{
		private CaloricCalculator caloricCalculator;
		private string projectPath;
		private Mock<IUnitOfWork> mockUnitWork;
		private Mock<IQueryHandler<GetKitByKitLocaleIdParameters, KitLocale>> mockGetKitLocaleQuery;
		private Mock<IRepository<Locale>> mockLocaleRepository;
		private Mock<IService<IEnumerable<StoreItem>, Task<IEnumerable<ItemStorePriceModel>>>> mockGetAuthorizedStatusAndPriceService;
		private Mock<IService<ItemNutritionRequestModel, Task<IEnumerable<ItemNutritionAttributesDictionary>>>> mockGetNutritionService;

		[TestInitialize]
		public void InitializeTest()
		{
			mockGetKitLocaleQuery = new Mock<IQueryHandler<GetKitByKitLocaleIdParameters, KitLocale>>();
			mockLocaleRepository = new Mock<IRepository<Locale>>();
			mockGetAuthorizedStatusAndPriceService = new Mock<IService<IEnumerable<StoreItem>, Task<IEnumerable<ItemStorePriceModel>>>>();
			mockGetNutritionService = new Mock<IService<ItemNutritionRequestModel, Task<IEnumerable<ItemNutritionAttributesDictionary>>>>();

			caloricCalculator = new CaloricCalculator(mockGetKitLocaleQuery.Object,
				mockLocaleRepository.Object,
				mockGetAuthorizedStatusAndPriceService.Object,
				mockGetNutritionService.Object);

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
			KitLocale kitLocale = JsonConvert.DeserializeObject<KitLocale>(json);

			filePath = Path.Combine(projectPath, "TestData", "ItemStorePriceModelList.Json");
			json = System.IO.File.ReadAllText(filePath);
			IEnumerable<ItemStorePriceModel> itemStorePriceModelList = JsonConvert.DeserializeObject<IEnumerable<ItemStorePriceModel>>(json);

			filePath = Path.Combine(projectPath, "TestData", "ItemCaloriesList.Json");
			json = System.IO.File.ReadAllText(filePath);
			IEnumerable<ItemNutritionAttributesDictionary> itemCaloriesList = JsonConvert.DeserializeObject<IEnumerable<ItemNutritionAttributesDictionary>>(json);		

			mockGetKitLocaleQuery.Setup(k => k.Search(It.IsAny<GetKitByKitLocaleIdParameters>())).Returns(kitLocale);
			mockGetAuthorizedStatusAndPriceService.Setup(p => p.Run(It.IsAny<IEnumerable<StoreItem>>())).ReturnsAsync(itemStorePriceModelList);
			mockGetNutritionService.Setup(n => n.Run(It.IsAny<ItemNutritionRequestModel>())).ReturnsAsync(itemCaloriesList);

			//When
			GetKitLocaleByStoreParameters parameters = new GetKitLocaleByStoreParameters();

			Task<KitLocaleDto> kitLocaleDto = caloricCalculator.Run(parameters);

			Assert.AreEqual(true, kitLocaleDto.Result.AuthorizedByStore, "Kit main item authorization status is wrong.");
			Assert.AreEqual(850, kitLocaleDto.Result.MinimumCalories, "Kit minimum calories is retrived/mapped wrong.");
			Assert.AreEqual(830, kitLocaleDto.Result.KitLinkGroupLocale.Where(k => k.KitLinkGroupLocaleId == 50).Select(k =>  k.MaximumCalories).FirstOrDefault(), "Kit first link group maximum calories is caculated wrong.");
			Assert.AreEqual(330, kitLocaleDto.Result.KitLinkGroupLocale.Where(k => k.KitLinkGroupLocaleId == 51).Select(k => k.MaximumCalories).FirstOrDefault(), "Kit second link group maximum calories is caculated wrong.");
			Assert.AreEqual(400, kitLocaleDto.Result.KitLinkGroupLocale.Where(k => k.KitLinkGroupLocaleId == 52).Select(k => k.MaximumCalories).FirstOrDefault(), "Kit third link group maximum calories is caculated wrong.");
			Assert.AreEqual(1560, kitLocaleDto.Result.MaximumCalories, "Kit max calories is calculated wrong.");
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
