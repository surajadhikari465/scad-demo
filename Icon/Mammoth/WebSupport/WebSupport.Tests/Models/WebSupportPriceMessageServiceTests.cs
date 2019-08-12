using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSupport.Models;
using WebSupport.ViewModels;
using Icon.Esb.Factory;
using Icon.Esb;
using Esb.Core.MessageBuilders;
using Icon.Common.DataAccess;
using WebSupport.DataAccess.Models;
using WebSupport.DataAccess.Queries;
using Moq;
using Esb.Core.EsbServices;
using Icon.Esb.Producer;
using WebSupport.DataAccess.Commands;
using WebSupport.Services;

namespace WebSupport.Tests.Models
{
    [TestClass]
    public class WebSupportPriceMessageServiceTests
    {
        private WebSupportPriceMessageService webSupportPriceMessageService;
        private PriceResetRequestViewModel request;
        private Mock<IEsbConnectionFactory> mockEsbConnectionFactory;
        private EsbConnectionSettings settings;
        private Mock<IMessageBuilder<PriceResetMessageBuilderModel>> mockPriceResetMessageBuilder;
        private Mock<IQueryHandler<GetPriceResetPricesParameters, List<PriceResetPrice>>> mockGetPriceResetPricesQuery;
        private Mock<ICommandHandler<SaveSentMessageCommand>> mockSaveSentMessageCommandHandler;
        private Mock<IEsbProducer> mockProducer;
        private Mock<IQueryHandler<GetMammothItemIdsToScanCodesParameters, List<string>>> mockSearchScanCodes;

        [TestInitialize]
        public void Initialize()
        {
            mockEsbConnectionFactory = new Mock<IEsbConnectionFactory>();
            settings = new EsbConnectionSettings();
            mockPriceResetMessageBuilder = new Mock<IMessageBuilder<PriceResetMessageBuilderModel>>();
            mockGetPriceResetPricesQuery = new Mock<IQueryHandler<GetPriceResetPricesParameters, List<PriceResetPrice>>>();
            mockSaveSentMessageCommandHandler = new Mock<ICommandHandler<SaveSentMessageCommand>>();
            mockProducer = new Mock<IEsbProducer>();
            mockSearchScanCodes = new Mock<IQueryHandler<GetMammothItemIdsToScanCodesParameters, List<string>>>();

            webSupportPriceMessageService = new WebSupportPriceMessageService(
                mockEsbConnectionFactory.Object,
                settings,
                mockPriceResetMessageBuilder.Object,
                mockGetPriceResetPricesQuery.Object,
                mockSaveSentMessageCommandHandler.Object,
                mockSearchScanCodes.Object);
            request = new PriceResetRequestViewModel
            {
                RegionIndex = 1,
                DownstreamSystems = new int[] { 1 },
                Stores = new[] { "1234" },
                Items = "12345"
            };

            mockEsbConnectionFactory.Setup(m => m.CreateProducer(settings))
                .Returns(mockProducer.Object);
        }

        [TestMethod]
        public void Send_PricesExist_ShouldSendPrices()
        {
            //Given
            mockGetPriceResetPricesQuery.Setup(m => m.Search(It.IsAny<GetPriceResetPricesParameters>()))
                .Returns(new List<PriceResetPrice> { new PriceResetPrice { PatchFamilyId = "111-11111", SequenceId = "1" } });

            //When
            var response = webSupportPriceMessageService.Send(request);

            //Then
            Assert.AreEqual(EsbServiceResponseStatus.Sent, response.Status);
            mockSaveSentMessageCommandHandler.Verify(m => m.Execute(It.IsAny<SaveSentMessageCommand>()), Times.Once);
        }

        [TestMethod]
        public void Send_PricesDontExist_ShouldNotSendPrices()
        {
            //Given
            mockGetPriceResetPricesQuery.Setup(m => m.Search(It.IsAny<GetPriceResetPricesParameters>()))
                .Returns(new List<PriceResetPrice>());

            //When
            var response = webSupportPriceMessageService.Send(request);

            //Then
            Assert.AreEqual(EsbServiceResponseStatus.Failed, response.Status);
            Assert.AreEqual(ErrorConstants.Codes.NoPricesExist, response.ErrorCode);
            Assert.AreEqual(ErrorConstants.Details.NoPricesExist, response.ErrorDetails);
            mockSaveSentMessageCommandHandler.Verify(m => m.Execute(It.IsAny<SaveSentMessageCommand>()), Times.Never);
        }

        [TestMethod]
        public void Send_PricesDontHavePatchFamilyOrSequenceId_ShouldNotSendPrices()
        {
            //Given
            mockGetPriceResetPricesQuery.Setup(m => m.Search(It.IsAny<GetPriceResetPricesParameters>()))
                .Returns(new List<PriceResetPrice> { new PriceResetPrice { ItemId = 1, ScanCode = "1111", BusinessUnitId = 2222 } });

            //When
            var response = webSupportPriceMessageService.Send(request);

            //Then
            Assert.AreEqual(EsbServiceResponseStatus.Failed, response.Status);
            Assert.AreEqual(ErrorConstants.Codes.SequenceIdOrPatchFamilyIdNotExist, response.ErrorCode);
            Assert.IsTrue(response.ErrorDetails.Contains("{ScanCode:1111,BusinessUnitID:2222}"));
            mockSaveSentMessageCommandHandler.Verify(m => m.Execute(It.IsAny<SaveSentMessageCommand>()), Times.Never);
        }
    }
}
