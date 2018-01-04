using Icon.Common.DataAccess;
using Icon.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebSupport.Controllers;
using WebSupport.DataAccess.Queries;
using WebSupport.DataAccess.TransferObjects;
using WebSupport.Models;
using WebSupport.Services;
using WebSupport.ViewModels;

namespace WebSupport.Tests.Controllers
{
    [TestClass]
    public class PriceRefreshControllerTests
    {
        private PriceRefreshController controller;
        private Mock<ILogger> mockLogger;
        private Mock<IQueryHandler<GetStoresForRegionParameters, IList<StoreTransferObject>>> mockQueryForStores;
        private Mock<IRefreshPriceService> mockRefreshPriceService;

        [TestInitialize]
        public void Initialize()
        {
            mockLogger = new Mock<ILogger>();
            mockQueryForStores = new Mock<IQueryHandler<GetStoresForRegionParameters, IList<StoreTransferObject>>>();
            mockRefreshPriceService = new Mock<IRefreshPriceService>();

            controller = new PriceRefreshController(
                mockLogger.Object,
                mockQueryForStores.Object,
                mockRefreshPriceService.Object);
        }

        [TestMethod]
        public void Index_Get_ReturnsRegionAndSystemOptions()
        {
            //When
            var view = controller.Index();

            //Then
            var viewModel = (view as ViewResult).Model as PriceRefreshViewModel;
            Assert.IsNotNull(viewModel);
            Assert.IsNull(viewModel.DownstreamSystems);
            Assert.IsNull(viewModel.Errors);
            Assert.IsNull(viewModel.Items);
            Assert.IsTrue(
                StaticData.JustInTimeDownstreamSystems.SequenceEqual(
                    viewModel.OptionsForDestinationSystem.Select(s => s.Text).ToArray()));
            Assert.IsTrue(
                StaticData.WholeFoodsRegions.SequenceEqual(
                    viewModel.OptionsForRegion.Select(s => s.Text).ToArray()));
            Assert.AreEqual("- select region first -", viewModel.OptionsForStores.Single().Text);
            Assert.IsNull(viewModel.Stores);
            Assert.IsNull(viewModel.Success);
        }

        [TestMethod]
        public void Index_GetWithErrorsTempDataSet_ReturnsErrors()
        {
            //Given
            var errors = new List<string> { "Test Error" };
            controller.TempData["Errors"] = errors;

            //When
            var view = controller.Index();

            //Then
            var viewModel = (view as ViewResult).Model as PriceRefreshViewModel;
            Assert.IsNotNull(viewModel);
            Assert.IsNull(viewModel.DownstreamSystems);
            Assert.AreEqual(errors, viewModel.Errors);
            Assert.IsNull(viewModel.Items);
            Assert.IsTrue(
                StaticData.JustInTimeDownstreamSystems.SequenceEqual(
                    viewModel.OptionsForDestinationSystem.Select(s => s.Text).ToArray()));
            Assert.IsTrue(
                StaticData.WholeFoodsRegions.SequenceEqual(
                    viewModel.OptionsForRegion.Select(s => s.Text).ToArray()));
            Assert.AreEqual("- select region first -", viewModel.OptionsForStores.Single().Text);
            Assert.IsNull(viewModel.Stores);
            Assert.IsNull(viewModel.Success);
        }

        [TestMethod]
        public void Index_GetWithSuccessTempDataSet_ReturnsSuccess()
        {
            //Given
            controller.TempData["Success"] = true;

            //When
            var view = controller.Index();

            //Then
            var viewModel = (view as ViewResult).Model as PriceRefreshViewModel;
            Assert.IsNotNull(viewModel);
            Assert.IsNull(viewModel.DownstreamSystems);
            Assert.IsNull(viewModel.Errors);
            Assert.IsNull(viewModel.Items);
            Assert.IsTrue(
                StaticData.JustInTimeDownstreamSystems.SequenceEqual(
                    viewModel.OptionsForDestinationSystem.Select(s => s.Text).ToArray()));
            Assert.IsTrue(
                StaticData.WholeFoodsRegions.SequenceEqual(
                    viewModel.OptionsForRegion.Select(s => s.Text).ToArray()));
            Assert.AreEqual("- select region first -", viewModel.OptionsForStores.Single().Text);
            Assert.IsNull(viewModel.Stores);
            Assert.IsTrue(viewModel.Success.Value);
        }

        [TestMethod]
        public void Index_PostWithValidResponse_ReturnsSuccess()
        {
            //Given
            mockRefreshPriceService
                .Setup(m => m.RefreshPrices(
                    It.IsAny<string>(),
                    It.IsAny<List<string>>(),
                    It.IsAny<List<string>>(),
                    It.IsAny<List<string>>()))
                .Returns(new RefreshPriceResponse());

            PriceRefreshViewModel parameters = new PriceRefreshViewModel();
            parameters.SetRegionAndSystemOptions(StaticData.WholeFoodsRegions, StaticData.JustInTimeDownstreamSystems);
            parameters.DownstreamSystems = new int[] { 1 };
            parameters.Stores = new string[] { "Test" };
            parameters.Items = "Test";
            parameters.RegionIndex = 1;

            //When
            var view = controller.Index(parameters);

            //Then
            Assert.IsTrue(Convert.ToBoolean(controller.TempData["Success"]));
            Assert.IsNull(controller.TempData["Errors"]);
        }

        [TestMethod]
        public void Index_PostWithInvalidResponse_ReturnsErrors()
        {
            //Given
            mockRefreshPriceService
                .Setup(m => m.RefreshPrices(
                    It.IsAny<string>(),
                    It.IsAny<List<string>>(),
                    It.IsAny<List<string>>(),
                    It.IsAny<List<string>>()))
                .Returns(new RefreshPriceResponse
                {
                    Errors = new List<string> { "Test Error" }
                });

            PriceRefreshViewModel parameters = new PriceRefreshViewModel();
            parameters.SetRegionAndSystemOptions(StaticData.WholeFoodsRegions, StaticData.JustInTimeDownstreamSystems);
            parameters.DownstreamSystems = new int[] { 1 };
            parameters.Stores = new string[] { "Test" };
            parameters.Items = "Test";
            parameters.RegionIndex = 1;

            //When
            var view = controller.Index(parameters);

            //Then
            Assert.IsNull(controller.TempData["Success"]);
            Assert.AreEqual("Test Error", ((List<string>)controller.TempData["Errors"]).Single());
        }
    }
}
