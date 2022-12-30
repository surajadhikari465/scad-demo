using Icon.Esb.EwicAplListener.MessageParsers;
using Icon.Dvs.Model;
using Icon.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.IO;
using System.Collections.Generic;

namespace Icon.Esb.EwicAplListener.Tests.Unit.MessageParsers
{
    [TestClass]
    public class AplMessageParserTests
    {
        private AplMessageParser parser;
        private DvsSqsMessage sqsMessage;

        private Mock<ILogger<AplMessageParser>> mockLogger;

        [TestInitialize]
        public void Initialize()
        {
            mockLogger = new Mock<ILogger<AplMessageParser>>();
            parser = new AplMessageParser(mockLogger.Object);
            sqsMessage = new DvsSqsMessage()
            {
                MessageAttributes = new Dictionary<string, string>() {
                    { "IconMessageID", "1" },
                    { "toBeReceivedBy", "ALL" }
                },
                S3BucketName = "SampleBucket",
                S3Key = "SampleS3Key"
            };
        }

        [TestMethod]
        public void ParseMessage_MessageWithAllFieldsPopulated_ReturnsPopulatedModel()
        {
            // Given.
            DvsMessage dvsMessage = new DvsMessage(sqsMessage, System.IO.File.ReadAllText(@"TestMessages/sample_ewic_message.xml"));
            // When.
            var result = parser.ParseMessage(dvsMessage);

            // Then.
            var item = result.Items[0];

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.MessageXml);
            Assert.AreEqual("ZZ", item.AgencyId);
            Assert.AreEqual("22222222", item.ScanCode);
            Assert.AreEqual("APL Item Description", item.ItemDescription);
            Assert.AreEqual("EA", item.UnitOfMeasure);
            Assert.AreEqual(1m, item.PackageSize);
            Assert.AreEqual(11.00m, item.BenefitQuantity);
            Assert.AreEqual("EA", item.BenefitUnitDescription);
            Assert.AreEqual(1.99m, item.ItemPrice);
            Assert.AreEqual("RG", item.PriceType);
        }

        [TestMethod]
        public void ParseMessage_MessageWithOnlyRequiredFieldsPopulated_ReturnsPopulatedModel()
        {
            // Given.
            DvsMessage dvsMessage = new DvsMessage(sqsMessage, System.IO.File.ReadAllText(@"TestMessages/sample_ewic_message_optionals_excluded.xml"));

            // When.
            var result = parser.ParseMessage(dvsMessage);

            // Then.
            var item = result.Items[0];

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.MessageXml);
            Assert.AreEqual("ZZ", item.AgencyId);
            Assert.AreEqual("22222222", item.ScanCode);
            Assert.IsNull(item.ItemDescription);
            Assert.IsNull(item.UnitOfMeasure);
            Assert.IsNull(item.PackageSize);
            Assert.IsNull(item.BenefitQuantity);
            Assert.IsNull(item.BenefitUnitDescription);
            Assert.IsNull(item.ItemPrice);
            Assert.IsNull(item.PriceType);
        }
    }
}
