﻿using Esb.Core.MessageBuilders;
using Icon.Common.DataAccess;
using Icon.Esb.Producer;
using Icon.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using WebSupport.Clients;
using WebSupport.DataAccess.Models;
using WebSupport.DataAccess.Queries;
using WebSupport.Managers;
using WebSupport.MessageBuilders;
using WebSupport.Services;


namespace WebSupport.Tests.Services
{
    [TestClass]
    public class RefreshPriceServiceTests
    {
        private RefreshPriceService service;
        private Mock<ILogger> logger;
        private Mock<IPriceRefreshMessageBuilderFactory> priceRefreshMessageBuilderFactory;
        private Mock<IQueryHandler<GetGpmPricesParameters, List<GpmPrice>>> getGpmPricesQuery;
        private Mock<IQueryHandler<DoesScanCodeExistParameters, bool>> doesScanCodeExistQuery;
        private Mock<IQueryHandler<DoesStoreExistParameters, bool>> doesStoreExistQuery;
        private string testRegion;
        private List<string> testSystems;
        private List<string> testBusinessUnitIds;
        private List<string> testScanCodes;
        private Mock<IMessageBuilder<List<GpmPrice>>> messageBuilder;
        private Mock<IEsbProducer> producer;
        private IClientIdManager clientIdManager;
        private Mock<IMammothGpmBridgeClient> mockMammothGpmClient;

        [TestInitialize]
        public void Initialize()
        {
            logger = new Mock<ILogger>();
            priceRefreshMessageBuilderFactory = new Mock<IPriceRefreshMessageBuilderFactory>();
            getGpmPricesQuery = new Mock<IQueryHandler<GetGpmPricesParameters, List<GpmPrice>>>();
            doesScanCodeExistQuery = new Mock<IQueryHandler<DoesScanCodeExistParameters, bool>>();
            doesStoreExistQuery = new Mock<IQueryHandler<DoesStoreExistParameters, bool>>();
            clientIdManager = new ClientIdManager();
            mockMammothGpmClient = new Mock<IMammothGpmBridgeClient>();
            clientIdManager.Initialize("WebSupportTests");

            service = new RefreshPriceService(
                logger.Object,
                priceRefreshMessageBuilderFactory.Object,
                getGpmPricesQuery.Object,
                doesScanCodeExistQuery.Object,
                doesStoreExistQuery.Object,
                clientIdManager,
                mockMammothGpmClient.Object);

            testRegion = "FL";
            testSystems = new List<string> { "R10" };
            testBusinessUnitIds = new List<string> { "11111" };
            testScanCodes = new List<string> { "4011" };
            messageBuilder = new Mock<IMessageBuilder<List<GpmPrice>>>();
            producer = new Mock<IEsbProducer>();
        }

        [TestMethod]
        public void RefreshPrices_ValidRequest_SendsPrices()
        {
            //Given
            doesStoreExistQuery.Setup(m => m.Search(It.IsAny<DoesStoreExistParameters>()))
                .Returns(true);
            doesScanCodeExistQuery.Setup(m => m.Search(It.IsAny<DoesScanCodeExistParameters>()))
                .Returns(true);
            getGpmPricesQuery.Setup(m => m.Search(It.IsAny<GetGpmPricesParameters>()))
                .Returns(new List<GpmPrice> { new GpmPrice { PatchFamilyId = "1-1", SequenceId = "1" } });
            priceRefreshMessageBuilderFactory.Setup(m => m.CreateMessageBuilder(It.IsAny<string>()))
                .Returns(messageBuilder.Object);

            //When
            var response = service.RefreshPrices(
                testRegion,
                testSystems,
                testBusinessUnitIds,
                testScanCodes);

            //Then
            Assert.AreEqual(0, response.Errors.Count);
        }
    }
}
