using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebSupport.DataAccess;
using WebSupport.MessageBuilders;

namespace WebSupport.Tests.MessageBuilders
{
    [TestClass]
    public class PriceRefreshMessageBuilderFactoryTests
    {
        private PriceRefreshMessageBuilderFactory factory;

        [TestInitialize]
        public void Initialize()
        {
            factory = new PriceRefreshMessageBuilderFactory();
        }

        [TestMethod]
        public void CreateMessageBuilder_Irma_ReturnIrmaPriceMessageBuilder()
        {
            //When
            var messageBuilder = factory.CreateMessageBuilder(PriceRefreshConstants.IRMA);

            //Then
            Assert.IsInstanceOfType(messageBuilder, typeof(PriceRefreshMessageBuilder));
        }

        [TestMethod]
        public void CreateMessageBuilder_R10_ReturnR10PriceMessageBuilder()
        {
            //When
            var messageBuilder = factory.CreateMessageBuilder(PriceRefreshConstants.R10);

            //Then
            Assert.IsInstanceOfType(messageBuilder, typeof(PriceRefreshMessageBuilder));
        }
    }
}
