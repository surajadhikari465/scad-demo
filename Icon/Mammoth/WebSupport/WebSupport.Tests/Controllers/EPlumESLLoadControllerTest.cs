using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Icon.Logging;
using Icon.Common.DataAccess;
using WebSupport.DataAccess.Queries;
using System.Collections.Generic;
using WebSupport.DataAccess.TransferObjects;
using WebSupport.Controllers;
using WebSupport.DataAccess.Commands;
using System.Web.Mvc;
using WebSupport.Models;
using System.Linq;
using WebSupport.ViewModels;

namespace WebSupport.Tests.Controllers
{
    [TestClass]
    public class EPlumESLLoadControllerTest
    {
        private EPlumESLLoadController controller;
        private Mock<ILogger> mockLogger;
        private Mock<IQueryHandler<GetStoresForRegionParameters, IList<StoreTransferObject>>> mockGetStoresQuery;
        private Mock<ICommandHandler<MassInsertToEPlumQueueCommand>> mockEplumCommandHandler;
        private Mock<ICommandHandler<MassInsertToESLQueueCommand>> mockEslCommandHandler;

        [TestInitialize]
        public void Initialize()
        {
            mockLogger = new Mock<ILogger>();
            mockGetStoresQuery = new Mock<IQueryHandler<GetStoresForRegionParameters, IList<StoreTransferObject>>>();
            mockEplumCommandHandler = new Mock<ICommandHandler<MassInsertToEPlumQueueCommand>>();
            mockEslCommandHandler = new Mock<ICommandHandler<MassInsertToESLQueueCommand>>();

            controller = new EPlumESLLoadController(
                mockLogger.Object,
                mockGetStoresQuery.Object,
                mockEplumCommandHandler.Object,
                mockEslCommandHandler.Object);
        }

        [TestMethod]
        public void EPlumESLLoadController_Index_GetRequest_ShouldReturnViewResult()
        {
            // When
            var result = controller.Index();

            // Then
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void EPlumESLLoadController_Index_Get_ShouldReturnViewResult_WithRegionOptions()
        {
            // Given
            var expectedRegionsOptions = StaticData.WholeFoodsRegions.ToList();

            // When
            var result = controller.Index();

            // Then
            var viewModelResult = (EPlumESLLoadViewModel)((ViewResult)result).Model;
            Assert.AreEqual(expectedRegionsOptions.Count,
                viewModelResult.OptionsForRegion.Count());
        }

        [TestMethod]
        public void EPlumESLLoadController_Index_Get_ShouldReturnViewResult_WithSystemOptions()
        {
            // Given
            var expectedDownstreamSystemsOptions = StaticData.EPlumESLSystems.ToList();

            // When
            var result = controller.Index();

            // Then
            var viewModelResult = (EPlumESLLoadViewModel)((ViewResult)result).Model;

            Assert.AreEqual(expectedDownstreamSystemsOptions.Count,
                viewModelResult.OptionsForDestinationSystem.Count());

            foreach (SelectListItem system in viewModelResult.OptionsForDestinationSystem)
                Assert.IsTrue(expectedDownstreamSystemsOptions.Contains(system.Text));
        }

        [TestMethod]
        public void EPlumESLLoadController_Index_Post_BothSystemsSelected_ShouldQueueEplumAndESLItems()
        {
            // Given
            var requestedData = new EPlumESLLoadViewModel()
            {
                RegionIndex = 0,
                DownstreamSystems = new int[] { 0, 1 },
                Stores = new string[] { "10001", "10010", "10100", "11000" },
            };
                
            // When
            var result = controller.Index(requestedData);

            // Then
            mockEplumCommandHandler.Verify(s => s.Execute(It.IsAny<MassInsertToEPlumQueueCommand>()), Times.Once);
            mockEslCommandHandler.Verify(s => s.Execute(It.IsAny<MassInsertToESLQueueCommand>()), Times.Once);
        }

        [TestMethod]
        public void EPlumESLLoadController_Index_Post_EplumSystemsSelected_ShouldOnlyQueueEplumItems()
        {
            // Given
            var requestedData = new EPlumESLLoadViewModel()
            {
                RegionIndex = 0,
                DownstreamSystems = new int[] { 0 },
                Stores = new string[] { "10001", "10010", "10100", "11000" },
            };

            // When
            var result = controller.Index(requestedData);

            // Then
            mockEplumCommandHandler.Verify(s => s.Execute(It.IsAny<MassInsertToEPlumQueueCommand>()), Times.Once);
            mockEslCommandHandler.Verify(s => s.Execute(It.IsAny<MassInsertToESLQueueCommand>()), Times.Never);
        }

        [TestMethod]
        public void EPlumESLLoadController_Index_Post_ESLSystemsSelected_ShouldOnlyQueueESLItems()
        {
            // Given
            var requestedData = new EPlumESLLoadViewModel()
            {
                RegionIndex = 0,
                DownstreamSystems = new int[] { 1 },
                Stores = new string[] { "10001", "10010", "10100", "11000" },
            };

            // When
            var result = controller.Index(requestedData);

            // Then
            mockEplumCommandHandler.Verify(s => s.Execute(It.IsAny<MassInsertToEPlumQueueCommand>()), Times.Never);
            mockEslCommandHandler.Verify(s => s.Execute(It.IsAny<MassInsertToESLQueueCommand>()), Times.Once);
        }
    }
}
