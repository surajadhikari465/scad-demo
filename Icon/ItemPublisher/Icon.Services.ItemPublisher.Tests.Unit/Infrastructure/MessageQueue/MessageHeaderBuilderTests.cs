using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Icon.Services.ItemPublisher.Infrastructure.MessageQueue.Tests
{
    [TestClass()]
    public class MessageHeaderBuilderTests
    {
        /// <summary>
        /// Tests that when we build an DVS message header that it is returned in the correct format and has an IconMessageID set
        /// </summary>
        [TestMethod]
        public void BuildMessageHeader_NoExcludedSystems_HeaderIsReturnedInCorrectFormat()
        {
            // Given.
            MessageHeaderBuilder builder = new MessageHeaderBuilder();
            string messageId = Guid.NewGuid().ToString();

            // When.
            Dictionary<string, string> result = builder.BuildMessageHeader(new List<string>() { }, messageId);

            // Then.
            Assert.AreEqual(messageId, result["IconMessageID"]);
            Assert.AreEqual("Icon", result["Source"]);
            Assert.AreEqual("Global Item", result["TransactionType"]);
            Assert.AreEqual("", result["nonReceivingSysName"]);
        }

        [TestMethod]
        public void BuildMessageHeader_OneExcludedSystem_HeaderIsReturnedInCorrectFormat()
        {
            // Given.
            MessageHeaderBuilder builder = new MessageHeaderBuilder();
            string messageId = Guid.NewGuid().ToString();

            // When.
            Dictionary<string, string> result = builder.BuildMessageHeader(new List<string>() { "R10" }, messageId);

            // Then.
            Assert.AreEqual(messageId, result["IconMessageID"]);
            Assert.AreEqual("Icon", result["Source"]);
            Assert.AreEqual("Global Item", result["TransactionType"]);
            Assert.AreEqual("R10", result["nonReceivingSysName"]);
        }
    }
}