using Icon.Infor.Listeners.HierarchyClass.EsbService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Icon.Esb.Factory;
using Esb.Core.MessageBuilders;
using Icon.Infor.Listeners.HierarchyClass.Requests;
using Moq;
using Icon.Esb;
using Icon.Esb.Producer;
using Esb.Core.EsbServices;
using Icon.Infor.Listeners.HierarchyClass.Constants;

namespace Icon.Infor.Listeners.HierarchyClass.Tests.EsbService
{
    [TestClass]
    public class HierarchyClassEsbServiceTests
    {
        private HierarchyClassEsbService service;
        private Mock<IEsbConnectionFactory> mockEsbConnectionFactory;
        private Mock<IMessageBuilder<HierarchyClassEsbServiceRequest>> mockMessageBuilder;
        private HierarchyClassEsbServiceRequest request;

        [TestInitialize]
        public void Initialize()
        {
            mockEsbConnectionFactory = new Mock<IEsbConnectionFactory>();
            mockMessageBuilder = new Mock<IMessageBuilder<HierarchyClassEsbServiceRequest>>();
            service = new HierarchyClassEsbService(mockEsbConnectionFactory.Object, mockMessageBuilder.Object);

            request = new HierarchyClassEsbServiceRequest();
        }
        [TestMethod]
        public void Send_MessageIsSentSuccessfully_ShouldReturnResponseWithSentStatus()
        {
            //Given
            mockEsbConnectionFactory.Setup(m => m.CreateProducer(It.IsAny<EsbConnectionSettings>()))
                .Returns(new Mock<IEsbProducer>().Object);
            mockMessageBuilder.Setup(m => m.BuildMessage(request))
                .Returns("test");
            request.MessageId = Guid.NewGuid().ToString();

            //When
            var response = service.Send(request);

            //Then
            Assert.AreEqual(EsbServiceResponseStatus.Sent, response.Status);
            Assert.AreEqual("test", response.Message.Text);
            Assert.AreEqual(request.MessageId, response.Message.MessageId);
            Assert.IsNull(response.ErrorCode);
            Assert.IsNull(response.ErrorDetails);
        }

        [TestMethod]
        public void Send_ExceptionOccursWhenSendMessage_ShouldReturnResponseWithFailedStatus()
        {
            //Given
            var exception = new Exception("Test Exception");
            mockEsbConnectionFactory.Setup(m => m.CreateProducer(It.IsAny<EsbConnectionSettings>()))
                .Throws(exception);
            request.MessageId = Guid.NewGuid().ToString();

            //When
            var response = service.Send(request);

            //Then
            Assert.AreEqual(EsbServiceResponseStatus.Failed, response.Status);
            Assert.AreEqual(request.MessageId, response.Message.MessageId);
            Assert.AreEqual(ApplicationErrors.Codes.UnableToSendHierarchyClassesToVim, response.ErrorCode);
            Assert.AreEqual(exception.ToString(), response.ErrorDetails);
            Assert.IsNull(response.Message.Text);
        }
    }
}
