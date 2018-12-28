using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IconWebApi;
using IconWebApi.Controllers;
using System.Collections.Generic;
using IconWebApi.DataAccess.Models;
using Moq;
using IconWebApi.DataAccess.Queries;
using Icon.Common.DataAccess;
using Icon.Logging;
using System.Web.Http.Results;
using System.Data.SqlClient;
using IconWebApi.Tests.ModelBuilders;
using System;
using IconWebApi.Models;
using System.Linq;

namespace IconWebApi.Tests.Controllers
{
	[TestClass]
	public class LocaleControllerTests
	{
		private LocaleController controller;
		private Mock<IQueryHandler<GetLocalesQuery, IEnumerable<GenericLocale>>> mockGetLocalesQueryHandler;
		private Mock<ILogger> mockLogger;
		private IEnumerable<GenericLocale> testGenericLocaleModels = new HashSet<GenericLocale>();

		[TestInitialize]
		public void InitializeTests()
		{
			this.mockLogger = new Mock<ILogger>();

			this.mockGetLocalesQueryHandler = new Mock<IQueryHandler<GetLocalesQuery, IEnumerable<GenericLocale>>>();

			this.controller = new LocaleController(
				this.mockGetLocalesQueryHandler.Object,
				this.mockLogger.Object);

			testGenericLocaleModels = BuildTestGenericLocales();

		}

		[TestMethod]
		public void LocaleControllerGetChains_IncludeChildrenTrueIncludeAddressFalse_ReturnsBadRequest()
		{
			// Given
			bool includeChildren = false;
			bool includeAddress = true;

			var expectedMessage = "The Chains search criteria are invalid because includeChildren is false while includeAddress is true. Address is only assoicated with stores.";

			// When
			var result = controller.Chains(includeChildren, includeAddress) as BadRequestErrorMessageResult;

			// Then
			Assert.IsNotNull(result, "The request did not return a BadRequestErrorMessageResult.");
			Assert.AreEqual(expectedMessage, result.Message, "The actual message did not match the expected message.");
			this.mockLogger.Verify(l => l.Warn(It.Is<string>(s => s == expectedMessage)));
		}

		[TestMethod]
		public void LocaleControllerGetChainsById_IncludeChildrenTrueIncludeAddressFalse_ReturnsBadRequest()
		{
			// Given
			int chainId = 1;
			bool includeChildren = false;
			bool includeAddress = true;

			var expectedMessage = "The Chain search by ID criteria are invalid because includeChildren is false while includeAddress is true. Address is only assoicated with stores.";

			// When
			var result = controller.Chains(chainId, includeChildren, includeAddress) as BadRequestErrorMessageResult;

			// Then
			Assert.IsNotNull(result, "The request did not return a BadRequestErrorMessageResult.");
			Assert.AreEqual(expectedMessage, result.Message, "The actual message did not match the expected message.");
			this.mockLogger.Verify(l => l.Warn(It.Is<string>(s => s == expectedMessage)));
		}

		[TestMethod]
		public void LocaleControllerGetRegions_IncludeChildrenTrueIncludeAddressFalse_ReturnsBadRequest()
		{
			// Given
			bool includeChildren = false;
			bool includeAddress = true;

			var expectedMessage = "The Regions search criteria are invalid because includeChildren is false while includeAddress is true. Address is only assoicated with stores.";

			// When
			var result = controller.Regions(includeChildren, includeAddress) as BadRequestErrorMessageResult;

			// Then
			Assert.IsNotNull(result, "The request did not return a BadRequestErrorMessageResult.");
			Assert.AreEqual(expectedMessage, result.Message, "The actual message did not match the expected message.");
			this.mockLogger.Verify(l => l.Warn(It.Is<string>(s => s == expectedMessage)));
		}

		[TestMethod]
		public void LocaleControllerGetRegionsById_IncludeChildrenTrueIncludeAddressFalse_ReturnsBadRequest()
		{
			// Given
			int regionId = 3;
			bool includeChildren = false;
			bool includeAddress = true;

			var expectedMessage = "The Region by ID search criteria are invalid because includeChildren is false while includeAddress is true. Address is only assoicated with stores.";

			// When
			var result = controller.Regions(regionId, includeChildren, includeAddress) as BadRequestErrorMessageResult;

			// Then
			Assert.IsNotNull(result, "The request did not return a BadRequestErrorMessageResult.");
			Assert.AreEqual(expectedMessage, result.Message, "The actual message did not match the expected message.");
			this.mockLogger.Verify(l => l.Warn(It.Is<string>(s => s == expectedMessage)));
		}

		[TestMethod]
		public void LocaleControllerChains_SqlExceptionDuringService_ReturnsInternalServerErrorWithSqlExceptionDetails()
		{
			// Given
			SqlException sqlException = CreateSqlException();
			this.mockGetLocalesQueryHandler.Setup(s => s.Search(It.IsAny<GetLocalesQuery>())).Throws(sqlException);
			var genericErrorMessage = "There was an error retrieving Chains from the Icon database. Please reach out to the support team for assistance. - ";

			// When
			var result = this.controller.Chains() as ExceptionResult;

			// Then
			Assert.IsNotNull(result, "The InternalServerError with Exception response is null.");
			Assert.AreEqual(genericErrorMessage + sqlException.Message, result.Exception.Message);
			this.mockLogger.Verify(l => l.Error(It.Is<string>(s => s == "Error performing Chains Http Get request. " + sqlException.Message)));
		}

		[TestMethod]
		public void LocaleControllerChainsById_SqlExceptionDuringService_ReturnsInternalServerErrorWithSqlExceptionDetails()
		{
			// Given
			int chainId = 1;
			SqlException sqlException = CreateSqlException();
			this.mockGetLocalesQueryHandler.Setup(s => s.Search(It.IsAny<GetLocalesQuery>())).Throws(sqlException);
			var genericErrorMessage = "There was an error retrieving Chain by ID from the Icon database. Please reach out to the support team for assistance. - ";

			// When
			var result = this.controller.Chains(chainId) as ExceptionResult;

			// Then
			Assert.IsNotNull(result, "The InternalServerError with Exception response is null.");
			Assert.AreEqual(genericErrorMessage + sqlException.Message, result.Exception.Message);
			this.mockLogger.Verify(l => l.Error(It.Is<string>(s => s == "Error performing Chain by ID Http Get request. " + sqlException.Message)));
		}

		[TestMethod]
		public void LocaleControllerRegions_SqlExceptionDuringService_ReturnsInternalServerErrorWithSqlExceptionDetails()
		{
			// Given
			SqlException sqlException = CreateSqlException();
			this.mockGetLocalesQueryHandler.Setup(s => s.Search(It.IsAny<GetLocalesQuery>())).Throws(sqlException);
			var genericErrorMessage = "There was an error retrieving Regions from the Icon database. Please reach out to the support team for assistance. - ";

			// When
			var result = this.controller.Regions() as ExceptionResult;

			// Then
			Assert.IsNotNull(result, "The InternalServerError with Exception response is null.");
			Assert.AreEqual(genericErrorMessage + sqlException.Message, result.Exception.Message);
			this.mockLogger.Verify(l => l.Error(It.Is<string>(s => s == "Error performing Regions Http Get request. " + sqlException.Message)));
		}

		[TestMethod]
		public void LocaleControllerRegionsById_SqlExceptionDuringService_ReturnsInternalServerErrorWithSqlExceptionDetails()
		{
			// Given
			int regionId = 3;
			SqlException sqlException = CreateSqlException();
			this.mockGetLocalesQueryHandler.Setup(s => s.Search(It.IsAny<GetLocalesQuery>())).Throws(sqlException);
			var genericErrorMessage = "There was an error retrieving Region by ID from the Icon database. Please reach out to the support team for assistance. - ";

			// When
			var result = this.controller.Regions(regionId) as ExceptionResult;

			// Then
			Assert.IsNotNull(result, "The InternalServerError with Exception response is null.");
			Assert.AreEqual(genericErrorMessage + sqlException.Message, result.Exception.Message);
			this.mockLogger.Verify(l => l.Error(It.Is<string>(s => s == "Error performing Region by ID Http Get request. " + sqlException.Message)));
		}

		[TestMethod]
		public void LocaleControllerStores_SqlExceptionDuringService_ReturnsInternalServerErrorWithSqlExceptionDetails()
		{
			// Given
			SqlException sqlException = CreateSqlException();
			this.mockGetLocalesQueryHandler.Setup(s => s.Search(It.IsAny<GetLocalesQuery>())).Throws(sqlException);
			var genericErrorMessage = "There was an error retrieving Stores from the Icon database. Please reach out to the support team for assistance. - ";

			// When
			var result = this.controller.Stores() as ExceptionResult;

			// Then
			Assert.IsNotNull(result, "The InternalServerError with Exception response is null.");
			Assert.AreEqual(genericErrorMessage + sqlException.Message, result.Exception.Message);
			this.mockLogger.Verify(l => l.Error(It.Is<string>(s => s == "Error performing Stores Http Get request. " + sqlException.Message)));
		}

		[TestMethod]
		public void LocaleControllerStoresById_SqlExceptionDuringService_ReturnsInternalServerErrorWithSqlExceptionDetails()
		{
			// Given
			int storeId = 555;
			SqlException sqlException = CreateSqlException();
			this.mockGetLocalesQueryHandler.Setup(s => s.Search(It.IsAny<GetLocalesQuery>())).Throws(sqlException);
			var genericErrorMessage = "There was an error retrieving Store by ID from the Icon database. Please reach out to the support team for assistance. - ";

			// When
			var result = this.controller.Stores(storeId) as ExceptionResult;

			// Then
			Assert.IsNotNull(result, "The InternalServerError with Exception response is null.");
			Assert.AreEqual(genericErrorMessage + sqlException.Message, result.Exception.Message);
			this.mockLogger.Verify(l => l.Error(It.Is<string>(s => s == "Error performing Store by ID Http Get request. " + sqlException.Message)));
		}

		[TestMethod]
		public void LocaleControllerVenues_SqlExceptionDuringService_ReturnsInternalServerErrorWithSqlExceptionDetails()
		{
			// Given
			SqlException sqlException = CreateSqlException();
			this.mockGetLocalesQueryHandler.Setup(s => s.Search(It.IsAny<GetLocalesQuery>())).Throws(sqlException);
			var genericErrorMessage = "There was an error retrieving Venues from the Icon database. Please reach out to the support team for assistance. - ";

			// When
			var result = this.controller.Venues() as ExceptionResult;

			// Then
			Assert.IsNotNull(result, "The InternalServerError with Exception response is null.");
			Assert.AreEqual(genericErrorMessage + sqlException.Message, result.Exception.Message);
			this.mockLogger.Verify(l => l.Error(It.Is<string>(s => s == "Error performing Venues Http Get request. " + sqlException.Message)));
		}

		[TestMethod]
		public void LocaleControllerVenuesById_SqlExceptionDuringService_ReturnsInternalServerErrorWithSqlExceptionDetails()
		{
			// Given
			int venueId = 555;
			SqlException sqlException = CreateSqlException();
			this.mockGetLocalesQueryHandler.Setup(s => s.Search(It.IsAny<GetLocalesQuery>())).Throws(sqlException);
			var genericErrorMessage = "There was an error retrieving Venue by ID from the Icon database. Please reach out to the support team for assistance. - ";

			// When
			var result = this.controller.Venues(venueId) as ExceptionResult;

			// Then
			Assert.IsNotNull(result, "The InternalServerError with Exception response is null.");
			Assert.AreEqual(genericErrorMessage + sqlException.Message, result.Exception.Message);
			this.mockLogger.Verify(l => l.Error(It.Is<string>(s => s == "Error performing Venue by ID Http Get request. " + sqlException.Message)));
		}

		[TestMethod]
		public void LocaleControllerChains_GetAllChainsNoChildrenRequest_ReturnsJsonContentResponseWithoutChildren()
		{
			// Given
			this.mockGetLocalesQueryHandler.Setup(s => s.Search(It.IsAny<GetLocalesQuery>()))
				.Returns(testGenericLocaleModels);

			// When
			var response = this.controller.Chains();

			// Then
			Assert.IsInstanceOfType(response, typeof(JsonResult<IEnumerable<Chain>>),
					"A JsonResult response was not returned as expected.");
			var returnedChains = (response as JsonResult<IEnumerable<Chain>>).Content.ToList();
			Assert.AreEqual(testGenericLocaleModels.Where(l => l.LocaleTypeId ==1).Count(), returnedChains.Count());

			for (int i = 0; i < returnedChains.Count(); i++)
			{
				Assert.IsNull(returnedChains[i].Regions);
			}
		}

		[TestMethod]
		public void LocaleControllerChains_GetAllChainsWithChildrenRequest_ReturnsJsonContentResponseWithChildren()
		{
			// Given
			this.mockGetLocalesQueryHandler.Setup(s => s.Search(It.IsAny<GetLocalesQuery>()))
				.Returns(testGenericLocaleModels);

			// When
			var response = this.controller.Chains(true);

			// Then
			Assert.IsInstanceOfType(response, typeof(JsonResult<IEnumerable<Chain>>),
					"A JsonResult response was not returned as expected.");
			var returnedChains = (response as JsonResult<IEnumerable<Chain>>).Content.ToList();
			Assert.AreEqual(testGenericLocaleModels.Where(l => l.LocaleTypeId == 1).Count(), returnedChains.Count());
			Assert.IsNotNull(returnedChains[0].Regions.Where(r => r.RegionCode.Contains("FL")));
			Assert.IsNotNull(returnedChains[0].Regions.Where(r => r.RegionCode.Contains("MA")));
			Assert.IsNotNull(returnedChains[0].Regions.Where(r => r.RegionCode.Contains("MW")));

		}

		[TestMethod]
		public void LocaleControllerChains_GetAChainByIdNoChildrenRequest_ReturnsJsonContentResponseWithoutChildren()
		{
			// Given
			int chainId = 1;

			this.mockGetLocalesQueryHandler.Setup(s => s.Search(It.IsAny<GetLocalesQuery>()))
				.Returns(testGenericLocaleModels);

			// When
			var response = this.controller.Chains(chainId);

			// Then
			Assert.IsInstanceOfType(response, typeof(JsonResult<IEnumerable<Chain>>),
					"A JsonResult response was not returned as expected.");
			var returnedChains = (response as JsonResult<IEnumerable<Chain>>).Content.ToList();
			Assert.AreEqual(1, returnedChains.Count());
			Assert.AreEqual(returnedChains[0].ChainId, chainId);
			Assert.IsNull(returnedChains[0].Regions);
		}

		[TestMethod]
		public void LocaleControllerChains_GetAChainByIdWithChildrenRequest_ReturnsJsonContentResponseWithChildren()
		{
			// Given
			int chainId = 1;

			this.mockGetLocalesQueryHandler.Setup(s => s.Search(It.IsAny<GetLocalesQuery>()))
				.Returns(testGenericLocaleModels);

			// When
			var response = this.controller.Chains(chainId, true);

			// Then
			Assert.IsInstanceOfType(response, typeof(JsonResult<IEnumerable<Chain>>),
					"A JsonResult response was not returned as expected.");
			var returnedChains = (response as JsonResult<IEnumerable<Chain>>).Content.ToList();
			Assert.AreEqual(1, returnedChains.Count());
			Assert.AreEqual(returnedChains[0].ChainId, chainId);
			Assert.AreEqual(testGenericLocaleModels.Where(l => l.ParentLocaleId == chainId).Count(), returnedChains[0].Regions.Count());
		}
		
		[TestMethod]
		public void LocaleControllerRegions_GetAllRegionsNoChildrenRequest_ReturnsJsonContentResponseWithoutChildren()
		{
			// Given
			this.mockGetLocalesQueryHandler.Setup(s => s.Search(It.IsAny<GetLocalesQuery>()))
				.Returns(testGenericLocaleModels);

			// When
			var response = this.controller.Regions();

			// Then
			Assert.IsInstanceOfType(response, typeof(JsonResult<IEnumerable<Region>>),
					"A JsonResult response was not returned as expected.");
			var returnedRegions = (response as JsonResult<IEnumerable<Region>>).Content.ToList();
			Assert.AreEqual(testGenericLocaleModels.Where(l => l.LocaleTypeId == 2).Count(), returnedRegions.Count());

			for (int i = 0; i < returnedRegions.Count(); i++)
			{
				Assert.IsNull(returnedRegions[i].Metros);
			}
		}

		[TestMethod]
		public void LocaleControllerRegions_GetAllRegionsWithChildrenRequest_ReturnsJsonContentResponseWithChildren()
		{
			// Given
			this.mockGetLocalesQueryHandler.Setup(s => s.Search(It.IsAny<GetLocalesQuery>()))
				.Returns(testGenericLocaleModels);

			// When
			var response = this.controller.Regions(true);

			// Then
			Assert.IsInstanceOfType(response, typeof(JsonResult<IEnumerable<Region>>),
					"A JsonResult response was not returned as expected.");
			var returnedRegions = (response as JsonResult<IEnumerable<Region>>).Content.ToList();
			Assert.AreEqual(testGenericLocaleModels.Where(l => l.LocaleTypeId == 2).Count(), returnedRegions.Count());
			Assert.IsNotNull(returnedRegions[0].Metros.Where(r => r.RegionId == 2));
			Assert.IsNotNull(returnedRegions[1].Metros.Where(r => r.RegionId == 3));
			Assert.IsNotNull(returnedRegions[2].Metros.Where(r => r.RegionId == 4));
		}

		[TestMethod]
		public void LocaleControllerRegions_GetARegionByIdNoChildrenRequest_ReturnsJsonContentResponseWithoutChildren()
		{
			// Given
			int regionId = 2;

			this.mockGetLocalesQueryHandler.Setup(s => s.Search(It.IsAny<GetLocalesQuery>()))
				.Returns(testGenericLocaleModels);

			// When
			var response = this.controller.Regions(regionId);

			// Then
			Assert.IsInstanceOfType(response, typeof(JsonResult<IEnumerable<Region>>),
					"A JsonResult response was not returned as expected.");
			var returnedRegions = (response as JsonResult<IEnumerable<Region>>).Content.ToList();
			Assert.AreEqual(1, returnedRegions.Count());
			Assert.AreEqual(returnedRegions[0].RegionId, regionId);

			for (int i = 0; i < returnedRegions.Count(); i++)
			{
				Assert.IsNull(returnedRegions[i].Metros);
			}
		}

		[TestMethod]
		public void LocaleControllerRegions_GetARegionByIdWithChildrenRequest_ReturnsJsonContentResponseWithChildren()
		{
			// Given
			int regionId = 2;

			this.mockGetLocalesQueryHandler.Setup(s => s.Search(It.IsAny<GetLocalesQuery>()))
				.Returns(testGenericLocaleModels);

			// When
			var response = this.controller.Regions(regionId, true);

			// Then
			Assert.IsInstanceOfType(response, typeof(JsonResult<IEnumerable<Region>>),
					"A JsonResult response was not returned as expected.");
			var returnedRegions = (response as JsonResult<IEnumerable<Region>>).Content.ToList();
			Assert.AreEqual(1, returnedRegions.Count());
			Assert.AreEqual(returnedRegions[0].RegionId, regionId);
			Assert.AreEqual(testGenericLocaleModels.Where(l => l.ParentLocaleId == regionId).Count(), returnedRegions[0].Metros.Count());
		}

		//

		[TestMethod]
		public void LocaleControllerStores_GetAllStoresNoChildrenRequest_ReturnsJsonContentResponseWithoutChildren()
		{
			// Given
			this.mockGetLocalesQueryHandler.Setup(s => s.Search(It.IsAny<GetLocalesQuery>()))
				.Returns(testGenericLocaleModels);

			// When
			var response = this.controller.Stores();

			// Then
			Assert.IsInstanceOfType(response, typeof(JsonResult<IEnumerable<Store>>),
					"A JsonResult response was not returned as expected.");
			var returnedStores = (response as JsonResult<IEnumerable<Store>>).Content.ToList();
			Assert.AreEqual(testGenericLocaleModels.Where(l => l.LocaleTypeId == 4).Count(), returnedStores.Count());

			for (int i = 0; i < returnedStores.Count(); i++)
			{
				Assert.IsNull(returnedStores[i].Venues);
			}
		}

		[TestMethod]
		public void LocaleControllerStores_GetAllStoresWithChildrenRequest_ReturnsJsonContentResponseWithChildren()
		{
			// Given
			this.mockGetLocalesQueryHandler.Setup(s => s.Search(It.IsAny<GetLocalesQuery>()))
				.Returns(testGenericLocaleModels);

			// When
			var response = this.controller.Stores(true);

			// Then
			Assert.IsInstanceOfType(response, typeof(JsonResult<IEnumerable<Store>>),
					"A JsonResult response was not returned as expected.");
			var returnedStores = (response as JsonResult<IEnumerable<Store>>).Content.ToList();
			Assert.AreEqual(testGenericLocaleModels.Where(l => l.LocaleTypeId == 4).Count(), returnedStores.Count());

			for (int i = 0; i < returnedStores.Count(); i++)
			{
				Assert.IsNotNull(returnedStores[i].Venues);
			}
		}

		[TestMethod]
		public void LocaleControllerStores_GetAllStoresWithChildrenByRegionCodeRequest_ReturnsJsonContentResponseWithChildren()
		{
			// Given
			string regionCode = "MW";

			this.mockGetLocalesQueryHandler.Setup(s => s.Search(It.IsAny<GetLocalesQuery>()))
				.Returns(testGenericLocaleModels);

			// When
			var response = this.controller.Stores(regionAbbr: regionCode, includeChildren: true);

			// Then
			Assert.IsInstanceOfType(response, typeof(JsonResult<IEnumerable<Store>>),
					"A JsonResult response was not returned as expected.");
			var returnedStores = (response as JsonResult<IEnumerable<Store>>).Content.ToList();
			Assert.AreEqual(4, returnedStores.Count());

			for (int i = 0; i < returnedStores.Count(); i++)
			{
				Assert.IsNotNull(returnedStores[i].Venues);
			}
		}

		[TestMethod]
		public void LocaleControllerStores_GetAllStoresWithAddressByRegionIdRequest_ReturnsJsonContentResponseWithAddress()
		{
			// Given
			int regionId = 4; //MW region

			this.mockGetLocalesQueryHandler.Setup(s => s.Search(It.IsAny<GetLocalesQuery>()))
				.Returns(testGenericLocaleModels);

			// When
			var response = this.controller.Stores(regionId: regionId, includeAddress: true);

			// Then
			Assert.IsInstanceOfType(response, typeof(JsonResult<IEnumerable<Store>>),
					"A JsonResult response was not returned as expected.");
			var returnedStores = (response as JsonResult<IEnumerable<Store>>).Content.ToList();
			Assert.AreEqual(4, returnedStores.Count());

			for (int i = 0; i < returnedStores.Count(); i++)
			{
				Assert.IsNotNull(returnedStores[i].StoreAddress);
			}
		}

		[TestMethod]
		public void LocaleControllerStores_GetAllStoresWithoutAddressByStoreAbbrRequest_ReturnsJsonContentResponseWithoutAddress()
		{
			// Given
			string storeAbbr = "EVS";

			this.mockGetLocalesQueryHandler.Setup(s => s.Search(It.IsAny<GetLocalesQuery>()))
				.Returns(testGenericLocaleModels);

			// When
			var response = this.controller.Stores(storeAbbr: storeAbbr);

			// Then
			Assert.IsInstanceOfType(response, typeof(JsonResult<IEnumerable<Store>>),
					"A JsonResult response was not returned as expected.");
			var returnedStores = (response as JsonResult<IEnumerable<Store>>).Content.ToList();
			Assert.AreEqual(1, returnedStores.Count());
			Assert.AreEqual(storeAbbr, returnedStores[0].StoreAbbreviation);
			Assert.IsNull(returnedStores[0].StoreAddress);
		}
		[TestMethod]
		public void LocaleControllerStores_GetAllStoresNoChildrenByStoreBuRequest_ReturnsJsonContentResponseWithoutChildren()
		{
			// Given
			int storeBU = 10274;

			this.mockGetLocalesQueryHandler.Setup(s => s.Search(It.IsAny<GetLocalesQuery>()))
				.Returns(testGenericLocaleModels);

			// When
			var response = this.controller.Stores(storeBu: storeBU);

			// Then
			Assert.IsInstanceOfType(response, typeof(JsonResult<IEnumerable<Store>>),
					"A JsonResult response was not returned as expected.");
			var returnedStores = (response as JsonResult<IEnumerable<Store>>).Content.ToList();
			Assert.AreEqual(1, returnedStores.Count());
			Assert.AreEqual(storeBU, returnedStores[0].BusinessUnitId);
			Assert.IsNull(returnedStores[0].Venues);
		}

		[TestMethod]
		public void LocaleControllerStores_GetAStoreByIdNoChildrenRequest_ReturnsJsonContentResponseWithoutChildren()
		{
			// Given
			int storeId = 630;

			this.mockGetLocalesQueryHandler.Setup(s => s.Search(It.IsAny<GetLocalesQuery>()))
				.Returns(testGenericLocaleModels);

			// When
			var response = this.controller.Stores(storeId);

			// Then
			Assert.IsInstanceOfType(response, typeof(JsonResult<IEnumerable<Store>>),
					"A JsonResult response was not returned as expected.");
			var returnedStores = (response as JsonResult<IEnumerable<Store>>).Content.ToList();
			Assert.AreEqual(1, returnedStores.Count());
			Assert.AreEqual(returnedStores[0].StoreId, storeId);
			Assert.IsNull(returnedStores[0].Venues);
		}

		[TestMethod]
		public void LocaleControllerStores_GetAStoreByIdWithChildrenRequest_ReturnsJsonContentResponseWithChildren()
		{
			// Given
			int storeId = 630;

			this.mockGetLocalesQueryHandler.Setup(s => s.Search(It.IsAny<GetLocalesQuery>()))
				.Returns(testGenericLocaleModels);

			// When
			var response = this.controller.Stores(storeId, true);

			// Then
			Assert.IsInstanceOfType(response, typeof(JsonResult<IEnumerable<Store>>),
					"A JsonResult response was not returned as expected.");
			var returnedStores = (response as JsonResult<IEnumerable<Store>>).Content.ToList();
			Assert.AreEqual(1, returnedStores.Count());
			Assert.AreEqual(returnedStores[0].StoreId, storeId);
			Assert.AreEqual(testGenericLocaleModels.Where(l => l.ParentLocaleId == storeId).Count(), returnedStores[0].Venues.Count());
		}

		[TestMethod]
		public void LocaleControllerVenues_GetAllStoresNoChildrenRequest_ReturnsJsonContentResponseWithoutChildren()
		{
			// Given
			this.mockGetLocalesQueryHandler.Setup(s => s.Search(It.IsAny<GetLocalesQuery>()))
				.Returns(testGenericLocaleModels);

			// When
			var response = this.controller.Stores();

			// Then
			Assert.IsInstanceOfType(response, typeof(JsonResult<IEnumerable<Store>>),
					"A JsonResult response was not returned as expected.");
			var returnedStores = (response as JsonResult<IEnumerable<Store>>).Content.ToList();
			Assert.AreEqual(testGenericLocaleModels.Where(l => l.LocaleTypeId == 4).Count(), returnedStores.Count());

			for (int i = 0; i < returnedStores.Count(); i++)
			{
				Assert.IsNull(returnedStores[i].Venues);
			}
		}

		[TestMethod]
		public void LocaleControllerVenues_GetAllVenuesRequest_ReturnsJsonContentResponse()
		{
			// Given
			this.mockGetLocalesQueryHandler.Setup(s => s.Search(It.IsAny<GetLocalesQuery>()))
				.Returns(testGenericLocaleModels);

			// When
			var response = this.controller.Venues();

			// Then
			Assert.IsInstanceOfType(response, typeof(JsonResult<IEnumerable<Venue>>),
					"A JsonResult response was not returned as expected.");
			var returnedVenues = (response as JsonResult<IEnumerable<Venue>>).Content.ToList();
			Assert.AreEqual(testGenericLocaleModels.Where(l => l.LocaleTypeId == 5).Count(), returnedVenues.Count());
		}

		[TestMethod]
		public void LocaleControllerVenues_GetAVenueByIdRequest_ReturnsJsonContentResponse()
		{
			// Given
			int venueId = 1302;

			this.mockGetLocalesQueryHandler.Setup(s => s.Search(It.IsAny<GetLocalesQuery>()))
				.Returns(testGenericLocaleModels);

			// When
			var response = this.controller.Venues(venueId);

			// Then
			Assert.IsInstanceOfType(response, typeof(JsonResult<IEnumerable<Venue>>),
					"A JsonResult response was not returned as expected.");
			var returnedVenues = (response as JsonResult<IEnumerable<Venue>>).Content.ToList();
			Assert.AreEqual(1, returnedVenues.Count());
			Assert.AreEqual(returnedVenues[0].VenueId, venueId);
		}

		private SqlException CreateSqlException()
		{
			SqlException exception = null;
			try
			{
				SqlConnection connection = new SqlConnection(@"Data Source=.;Initial Catalog=FAIL;Connection Timeout=1");
				connection.Open();
			}
			catch (SqlException sqlException)
			{
				exception = sqlException;
			}

			return exception;
		}

		private IEnumerable<GenericLocale> BuildTestGenericLocales()
		{
			var testGenericLocaleModels = new HashSet<GenericLocale>();

			//Build Chains
			testGenericLocaleModels.Add(new TestGenericLocaleBuilder()
				.WithLocaleId(1)
				.WithLocaleName("Whole Foods")
				.WithLocaleTypeId(1)
				.WithLocaleOpenDate(Convert.ToDateTime("1980-09-20"))
				.Build());

			testGenericLocaleModels.Add(new TestGenericLocaleBuilder()
				.WithLocaleId(1100)
				.WithLocaleName("365")
				.WithLocaleTypeId(1)
				.WithLocaleOpenDate(Convert.ToDateTime("2015-11-13"))
				.Build());

			//Build Regions
			testGenericLocaleModels.Add(new TestGenericLocaleBuilder()
				.WithLocaleId(2)
				.WithLocaleName("Florida")
				.WithLocaleTypeId(2)
				.WithParentLocaleId(1)
				.WithRegionCode("FL")
				.Build());

			testGenericLocaleModels.Add(new TestGenericLocaleBuilder()
				.WithLocaleId(3)
				.WithLocaleName("Mid Atlantic")
				.WithLocaleTypeId(2)
				.WithParentLocaleId(1)
				.WithRegionCode("MA")
				.Build());

			testGenericLocaleModels.Add(new TestGenericLocaleBuilder()
				.WithLocaleId(4)
				.WithLocaleName("Mid West")
				.WithLocaleTypeId(2)
				.WithParentLocaleId(1)
				.WithRegionCode("MW")
				.Build());

			//Build Metros
			testGenericLocaleModels.Add(new TestGenericLocaleBuilder()
				.WithLocaleId(14)
				.WithLocaleName("MET_FL")
				.WithLocaleTypeId(3)
				.WithParentLocaleId(2)
				.Build());

			testGenericLocaleModels.Add(new TestGenericLocaleBuilder()
				.WithLocaleId(15)
				.WithLocaleName("MET_DC")
				.WithLocaleTypeId(3)
				.WithParentLocaleId(3)
				.Build());

			testGenericLocaleModels.Add(new TestGenericLocaleBuilder()
				.WithLocaleId(16)
				.WithLocaleName("MET_KY")
				.WithLocaleTypeId(3)
				.WithParentLocaleId(3)
				.Build());

			testGenericLocaleModels.Add(new TestGenericLocaleBuilder()
				.WithLocaleId(22)
				.WithLocaleName("MET_CHI")
				.WithLocaleTypeId(3)
				.WithParentLocaleId(4)
				.Build());

			testGenericLocaleModels.Add(new TestGenericLocaleBuilder()
				.WithLocaleId(30)
				.WithLocaleName("MET_ON")
				.WithLocaleTypeId(3)
				.WithParentLocaleId(4)
				.Build());

			//Build Stores
			testGenericLocaleModels.Add(new TestGenericLocaleBuilder()
				.WithLocaleId(555)
				.WithLocaleName("Boca Raton")
				.WithLocaleTypeId(4)
				.WithParentLocaleId(14)
				.WithLocaleOpenDate(Convert.ToDateTime("2001-04-25"))
				.WithBusinessUnitId(10130)
				.WithStoreAbbreviation("BCA")
				.WithCurrencyCode("USD")
				.WithAddressLine1("1400B Glades Rd")
				.WithCityName("Boca Raton")
				.WithTerritoryCode("FL")
				.WithPostalCode("33486")
				.WithCountryCode("USA")
				.WithCountryName("United States")
				.Build());

			testGenericLocaleModels.Add(new TestGenericLocaleBuilder()
				.WithLocaleId(564)
				.WithLocaleName("Pinecrest")
				.WithLocaleTypeId(4)
				.WithParentLocaleId(14)
				.WithLocaleOpenDate(Convert.ToDateTime("2007-09-28"))
				.WithLocaleCloseDate(Convert.ToDateTime("2018-01-07"))
				.WithBusinessUnitId(10274)
				.WithStoreAbbreviation("PCR")
				.WithCurrencyCode("USD")
				.WithAddressLine1("11701 S. Dixie Hwy")
				.WithCityName("Miami")
				.WithTerritoryCode("FL")
				.WithPostalCode("33156")
				.WithCountryCode("USA")
				.WithCountryName("United States")
				.Build());

			testGenericLocaleModels.Add(new TestGenericLocaleBuilder()
				.WithLocaleId(584)
				.WithLocaleName("Georgetown")
				.WithLocaleTypeId(4)
				.WithParentLocaleId(15)
				.WithLocaleOpenDate(Convert.ToDateTime("1996-01-31"))
				.WithBusinessUnitId(10039)
				.WithStoreAbbreviation("GTN")
				.WithCurrencyCode("USD")
				.WithAddressLine1("2323 Wisconsin Avenue NW")
				.WithCityName("Washington")
				.WithTerritoryCode("DC")
				.WithPostalCode("20008")
				.WithCountryCode("USA")
				.WithCountryName("United States")
				.Build());

			testGenericLocaleModels.Add(new TestGenericLocaleBuilder()
				.WithLocaleId(586)
				.WithLocaleName("P Street")
				.WithLocaleTypeId(4)
				.WithParentLocaleId(15)
				.WithLocaleOpenDate(Convert.ToDateTime("2000-12-14"))
				.WithBusinessUnitId(10135)
				.WithStoreAbbreviation("PST")
				.WithCurrencyCode("USD")
				.WithAddressLine1("1440 P Street NW")
				.WithCityName("Washington")
				.WithTerritoryCode("DC")
				.WithPostalCode("20005")
				.WithCountryCode("USA")
				.WithCountryName("United States")
				.Build());

			testGenericLocaleModels.Add(new TestGenericLocaleBuilder()
				.WithLocaleId(589)
				.WithLocaleName("Louisville")
				.WithLocaleTypeId(4)
				.WithParentLocaleId(16)
				.WithLocaleOpenDate(Convert.ToDateTime("2004-02-12"))
				.WithBusinessUnitId(10186)
				.WithStoreAbbreviation("LOU")
				.WithCurrencyCode("USD")
				.WithAddressLine1("4944 Shelbyville Rd")
				.WithCityName("Louisville")
				.WithTerritoryCode("KY")
				.WithPostalCode("40207")
				.WithCountryCode("USA")
				.WithCountryName("United States")
				.Build());

			testGenericLocaleModels.Add(new TestGenericLocaleBuilder()
				.WithLocaleId(630)
				.WithLocaleName("Evanston")
				.WithLocaleTypeId(4)
				.WithParentLocaleId(22)
				.WithLocaleOpenDate(Convert.ToDateTime("1997-12-03"))
				.WithBusinessUnitId(10076)
				.WithStoreAbbreviation("EVN")
				.WithCurrencyCode("USD")
				.WithAddressLine1("1640 Chicago Ave")
				.WithCityName("Evanston")
				.WithTerritoryCode("IL")
				.WithPostalCode("60201")
				.WithCountryCode("USA")
				.WithCountryName("United States")
				.Build());

			testGenericLocaleModels.Add(new TestGenericLocaleBuilder()
				.WithLocaleId(631)
				.WithLocaleName("Chicago Ave")
				.WithLocaleTypeId(4)
				.WithParentLocaleId(22)
				.WithLocaleOpenDate(Convert.ToDateTime("2007-09-28"))
				.WithLocaleCloseDate(Convert.ToDateTime("2017-03-13"))
				.WithBusinessUnitId(10369)
				.WithStoreAbbreviation("EVS")
				.WithCurrencyCode("USD")
				.WithAddressLine1("1111 Chicago Avenue")
				.WithCityName("Evanston")
				.WithTerritoryCode("IL")
				.WithPostalCode("60202")
				.WithCountryCode("USA")
				.WithCountryName("United States")
				.Build());

			testGenericLocaleModels.Add(new TestGenericLocaleBuilder()
				.WithLocaleId(666)
				.WithLocaleName("Unionville")
				.WithLocaleTypeId(4)
				.WithParentLocaleId(30)
				.WithLocaleOpenDate(Convert.ToDateTime("2012-10-12"))
				.WithBusinessUnitId(10456)
				.WithStoreAbbreviation("MKM")
				.WithCurrencyCode("CAD")
				.WithAddressLine1("3997 Highway 7")
				.WithCityName("Markham")
				.WithTerritoryCode("ON")
				.WithPostalCode("L3R 5M6")
				.WithCountryCode("CAN")
				.WithCountryName("Canada")
				.Build());

			testGenericLocaleModels.Add(new TestGenericLocaleBuilder()
				.WithLocaleId(668)
				.WithLocaleName("Square One")
				.WithLocaleTypeId(4)
				.WithParentLocaleId(30)
				.WithLocaleOpenDate(Convert.ToDateTime("2011-08-10"))
				.WithBusinessUnitId(10268)
				.WithStoreAbbreviation("SQO")
				.WithCurrencyCode("CAD")
				.WithAddressLine1("155 Square One Drive")
				.WithCityName("Mississauga")
				.WithTerritoryCode("ON")
				.WithPostalCode("L5B 0E2")
				.WithCountryCode("CAN")
				.WithCountryName("Canada")
				.Build());

			//Build Venue
			testGenericLocaleModels.Add(new TestGenericLocaleBuilder()
				.WithLocaleId(1302)
				.WithLocaleName("Burger Bar")
				.WithLocaleTypeId(5)
				.WithParentLocaleId(630)
				.WithLocaleOpenDate(Convert.ToDateTime("2018-08-10"))
				.WithSubType("Hospitality")
				.Build());

			return testGenericLocaleModels;
		}
	}
}
