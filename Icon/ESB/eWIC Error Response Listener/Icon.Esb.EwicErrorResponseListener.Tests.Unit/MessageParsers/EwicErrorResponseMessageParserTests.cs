using Icon.Esb.EwicErrorResponseListener.Common.Models;
using Icon.Esb.EwicErrorResponseListener.MessageParsers;
using Icon.Esb.Subscriber;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.IO;

namespace Icon.Esb.EwicErrorResponseListener.Tests.Unit.MessageParsers
{
    [TestClass]
    public class EwicErrorResponseMessageParserTests
    {
        private ErrorResponseMessageParser parser;
        private Mock<IEsbMessage> esbMessage;
        private string messageHistoryId;

        [TestInitialize]
        public void Initialize()
        {
            esbMessage = new Mock<IEsbMessage>();
            parser = new ErrorResponseMessageParser();

            messageHistoryId = "1234";
        }

        [TestMethod]
        public void ParseMessage_OkResponse_ReturnsPopulatedModel()
        {
            // Given.
            var message = File.ReadAllText(@"TestMessages/ewic_ok_response.xml");

            esbMessage.Setup(m => m.GetProperty(It.Is<string>(p => p == "TransactionID"))).Returns(messageHistoryId);
            esbMessage.SetupGet(m => m.MessageText).Returns(message);

            // When.
            EwicErrorResponseModel result = parser.ParseMessage(esbMessage.Object);

            // Then.
            Assert.AreEqual(Int32.Parse(messageHistoryId), result.MessageHistoryId);
            Assert.IsFalse(result.SystemError);
            Assert.IsTrue(result.RequestSuccess);
            Assert.IsNotNull(result.ResponseText);
            Assert.IsNotNull(result.ResponseReason);
            Assert.AreNotEqual(String.Empty, result.ResponseText);
            Assert.AreNotEqual(String.Empty, result.ResponseReason);
        }

        [TestMethod]
        public void ParseMessage_NoSequenceReturnedResponse_ReturnsPopulatedModel()
        {
            // Given.
            var message = File.ReadAllText(@"TestMessages/ewic_error_response.xml");

            esbMessage.Setup(m => m.GetProperty(It.Is<string>(p => p == "TransactionID"))).Returns(messageHistoryId);
            esbMessage.SetupGet(m => m.MessageText).Returns(message);

            // When.
            EwicErrorResponseModel result = parser.ParseMessage(esbMessage.Object);

            // Then.
            Assert.AreEqual(Int32.Parse(messageHistoryId), result.MessageHistoryId);
            Assert.IsTrue(result.SystemError);
            Assert.IsFalse(result.RequestSuccess);
            Assert.IsNotNull(result.ResponseText);
            Assert.IsNotNull(result.ResponseReason);
            Assert.AreNotEqual(String.Empty, result.ResponseText);
            Assert.AreNotEqual(String.Empty, result.ResponseReason);
        }
    }
}
