using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Icon.Services.ItemPublisher.Infrastructure.Esb.Tests
{
    [TestClass()]
    public class EsbHeaderBuilderTests
    {
        /// <summary>
        /// Tests that when we build an ESB message header that it is returned in the correct format and has an IconMessageID set
        /// </summary>
        [TestMethod]
        public void BuildMessageHeader_NoExcludedSystems_HeaderIsReturnedInCorrectFormat()
        {
            // Given.
            EsbHeaderBuilder builder = new EsbHeaderBuilder();

            // When.
            Dictionary<string, string> result = builder.BuildMessageHeader(new List<string>() { });

            // Then.
            Assert.IsTrue(!string.IsNullOrWhiteSpace(result["IconMessageID"]));
            Assert.AreEqual("Icon", result["Source"]);
            Assert.AreEqual("Global Item", result["TransactionType"]);
            Assert.AreEqual("", result["nonReceivingSysName"]);
        }

        [TestMethod]
        public void BuildMessageHeader_OneExcludedSystem_HeaderIsReturnedInCorrectFormat()
        {
            // Given.
            EsbHeaderBuilder builder = new EsbHeaderBuilder();

            // When.
            Dictionary<string, string> result = builder.BuildMessageHeader(new List<string>() { "R10" });

            // Then.
            Assert.IsTrue(!string.IsNullOrWhiteSpace(result["IconMessageID"]));
            Assert.AreEqual("Icon", result["Source"]);
            Assert.AreEqual("Global Item", result["TransactionType"]);
            Assert.AreEqual("R10", result["nonReceivingSysName"]);
        }
    }
}