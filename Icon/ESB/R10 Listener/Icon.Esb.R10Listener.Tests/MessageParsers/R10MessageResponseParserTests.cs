using Icon.Esb.R10Listener.Constants;
using Icon.Esb.R10Listener.MessageParsers;
using Icon.Esb.Subscriber;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Icon.Esb.R10Listener.Tests.MessageParsers
{
    [TestClass]
    public class R10MessageResponseParserTests
    {
        private R10MessageResponseParser parser;
        private Mock<IEsbMessage> esbMessage;

        [TestInitialize]
        public void Initialize()
        {
            esbMessage = new Mock<IEsbMessage>();
            parser = new R10MessageResponseParser();
        }

        [TestMethod]
        public void ParseMessage_ValidMessage_ReturnsMessageResponse()
        {
            //Given
            var message = File.ReadAllText(@"TestMessages/valid_message_response.xml");
            esbMessage.SetupGet(m => m.MessageText).Returns(message);
            esbMessage.Setup(m => m.GetProperty(It.Is<string>(s => s == "TransactionID"))).Returns("1");

            //When
            var result = parser.ParseMessage(esbMessage.Object);

            //Then
            Assert.AreEqual(1, result.MessageHistoryId);
            Assert.IsTrue(result.RequestSuccess);
            Assert.IsFalse(result.SystemError);
            Assert.AreEqual(XElement.Parse(message).ToString(), result.ResponseText); 
            Assert.IsNull(result.FailureReasonCode);
            Assert.IsNull(result.BusinessErrors);
        }

        [TestMethod]
        public void ParseMessage_RequestUnsuccessful_ReturnsMessageResponse()
        {
            //Givn
            var message = File.ReadAllText(@"TestMessages/fail_bottle_deposit_maintenance_message.xml");
            esbMessage.SetupGet(m => m.MessageText).Returns(message);
            esbMessage.Setup(m => m.GetProperty(It.Is<string>(s => s == "TransactionID"))).Returns("1");

            ////When
            var result = parser.ParseMessage(esbMessage.Object);

            //Then
            Assert.AreEqual(1, result.MessageHistoryId);
            Assert.IsFalse(result.RequestSuccess);
            Assert.IsFalse(result.SystemError);
            Assert.AreEqual(XElement.Parse(message).ToString(), result.ResponseText);
            Assert.AreEqual("InvalidRequest", result.FailureReasonCode);
            Assert.AreEqual(1, result.BusinessErrors.Count());
            Assert.AreEqual("InvalidRequest", result.BusinessErrors.First().Code);
            Assert.AreEqual(-1, result.BusinessErrors.First().MainId);
        }

        [TestMethod]
        public void ParseMessage_RequestUnsuccessfulAndSystemError_ReturnsMessageResponse()
        {
            //Givn
            var message = File.ReadAllText(@"TestMessages/fail_system_timeout-price.xml");
            esbMessage.SetupGet(m => m.MessageText).Returns(message);
            esbMessage.Setup(m => m.GetProperty(It.Is<string>(s => s == "TransactionID"))).Returns("1");

            ////When
            var result = parser.ParseMessage(esbMessage.Object);

            //Then
            Assert.AreEqual(1, result.MessageHistoryId);
            Assert.IsFalse(result.RequestSuccess);
            Assert.IsTrue(result.SystemError);
            Assert.AreEqual(XElement.Parse(message).ToString(), result.ResponseText);
            Assert.AreEqual("WFM_SYS_HTTP_TIMEOUT", result.FailureReasonCode);
            Assert.IsNull(result.BusinessErrors);
        }

        [TestMethod]
        public void ParseMessage_ThresholdExceededError_ReturnsMessageResponse()
        {
            //Givn
            var message = File.ReadAllText(@"TestMessages/fail_threshold_exceeded.xml");
            esbMessage.SetupGet(m => m.MessageText).Returns(message);
            esbMessage.Setup(m => m.GetProperty(It.Is<string>(s => s == "TransactionID"))).Returns("1");

            ////When
            var result = parser.ParseMessage(esbMessage.Object);

            //Then
            Assert.AreEqual(1, result.MessageHistoryId);
            Assert.IsFalse(result.RequestSuccess);
            Assert.IsFalse(result.SystemError);
            Assert.AreEqual(XElement.Parse(message).ToString(), result.ResponseText);
            Assert.AreEqual("ProductIdentifierNotUnique, ThresholdExceededError", result.FailureReasonCode);
            
            Assert.AreEqual(21, result.BusinessErrors.Count());            
            Assert.AreEqual("ProductIdentifierNotUnique", result.BusinessErrors.First().Code);
            Assert.AreEqual(384276, result.BusinessErrors.First().MainId);

            var thresholdExceededBusinessErrors = result.BusinessErrors.Skip(1).ToList();
            foreach (var error in thresholdExceededBusinessErrors)
            {
                Assert.AreEqual("ThresholdExceededError", error.Code);
            }

            thresholdExceededBusinessErrors[0].MainId = 1873480;
            thresholdExceededBusinessErrors[1].MainId = 1873480;
            thresholdExceededBusinessErrors[2].MainId = 1873234;
            thresholdExceededBusinessErrors[3].MainId = 273055;
            thresholdExceededBusinessErrors[4].MainId = 329068;
        }

        [TestMethod]
        public void ParseMessage_MessageHasSourceSystemInFrontOfId_ShouldSuccessfullyParseMessage()
        {
            //Given
            var message = File.ReadAllText(@"TestMessages/fail_unsuccessful_parse_from_prod.xml");
            esbMessage.SetupGet(m => m.MessageText).Returns(message);
            esbMessage.Setup(m => m.GetProperty(It.Is<string>(s => s == "TransactionID"))).Returns("Icon_1");

            //When
            var result = parser.ParseMessage(esbMessage.Object);

            //Then
            Assert.AreEqual(1, result.MessageHistoryId);
            Assert.IsTrue(result.RequestSuccess);
            Assert.IsFalse(result.SystemError);
            Assert.AreEqual(XElement.Parse(message).ToString(), result.ResponseText);
            Assert.IsNull(result.FailureReasonCode);
            Assert.IsNull(result.BusinessErrors);
        }
    }
}
