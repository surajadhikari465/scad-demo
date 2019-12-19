using AttributePublisher.MessageBuilders;
using AttributePublisher.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AttributePublisher.Tests.Unit.MessageBuilders
{
    [TestClass]
    public class AttributeMessageHeaderBuilderTests
    {
        private AttributeMessageHeaderBuilder messageHeaderBuilder;
        private AttributePublisherServiceSettings settings;

        [TestInitialize]
        public void Initialize()
        {
            settings = new AttributePublisherServiceSettings { NonReceivingSystems = "Test" };
            messageHeaderBuilder = new AttributeMessageHeaderBuilder(settings);
        }

        [TestMethod]
        public void AttributeMessageHeaderBuilder_BuildHeader_ReturnsMessageHeaders()
        {
            //When
            var result = messageHeaderBuilder.BuildHeader();

            //Then
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(result["nonReceivingSysName"], "Test");
            Assert.AreEqual(result["TransactionType"], AttributePublisherResources.MessageTransactionType);
            Assert.AreEqual(result["Source"], AttributePublisherResources.MessageSource);
        }
    }
}
