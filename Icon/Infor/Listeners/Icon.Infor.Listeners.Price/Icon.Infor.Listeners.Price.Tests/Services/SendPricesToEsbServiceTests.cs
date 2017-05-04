using Esb.Core.MessageBuilders;
using Icon.Esb;
using Icon.Esb.Factory;
using Icon.Esb.Producer;
using Icon.Esb.Subscriber;
using Icon.Infor.Listeners.Price.Constants;
using Icon.Infor.Listeners.Price.EsbFactory;
using Icon.Infor.Listeners.Price.Models;
using Icon.Infor.Listeners.Price.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.Listeners.Price.Tests.Services
{
    [TestClass]
    public class SendPricesToEsbServiceTests
    {
        private SendPricesToEsbService service;
        private EsbConnectionSettings settings;
        private Mock<IEsbConnectionSettingsFactory> mockSettingsFactory;
        private Mock<IEsbConnectionFactory> mockEsbConnectionFactory;
        private Mock<IMessageBuilder<IEnumerable<PriceModel>>> mockMessageBuilder;
        private Mock<IEsbProducer> mockProducer;
        private List<PriceModel> prices;
        private Mock<IEsbMessage> message;

        [TestInitialize]
        public void Initialize()
        {
            settings = new EsbConnectionSettings();
            mockSettingsFactory = new Mock<IEsbConnectionSettingsFactory>();
            mockSettingsFactory.Setup(m => m.CreateConnectionSettings(It.IsAny<Type>())).Returns(settings);
            mockEsbConnectionFactory = new Mock<IEsbConnectionFactory>();
            mockMessageBuilder = new Mock<IMessageBuilder<IEnumerable<PriceModel>>>();
            service = new SendPricesToEsbService(settings, mockSettingsFactory.Object, mockEsbConnectionFactory.Object, mockMessageBuilder.Object);

            mockProducer = new Mock<IEsbProducer>();
            prices = new List<PriceModel>();
            message = new Mock<IEsbMessage>();
        }

        [TestMethod]
        public void SendPricesToEsbService_PricesExist_SendsPricesToEsb()
        {
            //Given
            prices.Add(new PriceModel { ErrorCode = null });
            mockEsbConnectionFactory.Setup(m => m.CreateProducer(settings))
                .Returns(mockProducer.Object);

            //When
            service.Process(prices, message.Object);

            //Then
            mockMessageBuilder.Verify(m => m.BuildMessage(It.IsAny<IEnumerable<PriceModel>>()), Times.Once);
            mockEsbConnectionFactory.Verify(m => m.CreateProducer(settings), Times.Once);
        }

        [TestMethod]
        public void SendPricesToEsbService_PricesWithNoErrorsDoNotExist_DoesNotSendPricesToEsb()
        {
            //Given
            prices.Add(new PriceModel { ErrorCode = Errors.Codes.AddPricesError });
            mockEsbConnectionFactory.Setup(m => m.CreateProducer(settings))
                .Returns(mockProducer.Object);

            //When
            service.Process(prices, message.Object);

            //Then
            mockMessageBuilder.Verify(m => m.BuildMessage(It.IsAny<IEnumerable<PriceModel>>()), Times.Never);
            mockEsbConnectionFactory.Verify(m => m.CreateProducer(settings), Times.Never);
        }

        [TestMethod]
        public void SendPricesToEsbService_ConnectionFactoryThrowsError_SetsErrorOnPrices()
        {
            //Given
            prices.Add(new PriceModel { ErrorCode = null });
            Exception testException = new Exception("Test Exception");
            mockEsbConnectionFactory.Setup(m => m.CreateProducer(It.IsAny<EsbConnectionSettings>()))
                .Throws(testException);

            //When
            service.Process(prices, message.Object);

            //Then
            mockEsbConnectionFactory.Verify(m => m.CreateProducer(It.IsAny<EsbConnectionSettings>()), Times.Once);
            foreach (var price in prices)
            {
                Assert.AreEqual(Errors.Codes.SendPricesToEsbError, price.ErrorCode);
                Assert.AreEqual(testException.ToString(), price.ErrorDetails);
            }
        }
    }
}
