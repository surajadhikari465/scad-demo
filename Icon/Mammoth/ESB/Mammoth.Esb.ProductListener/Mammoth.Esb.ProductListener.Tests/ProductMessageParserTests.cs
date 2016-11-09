using Icon.Esb.Subscriber;
using Icon.Framework;
using Mammoth.Esb.ProductListener.MessageParsers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.IO;
using System.Linq;

namespace Mammoth.Esb.ProductListener.Tests
{
    [TestClass]
    public class ProductMessageParserTests
    {
        private ProductMessageParser messageParser;
        private Mock<IEsbMessage> mockEsbMessage;

        [TestInitialize]
        public void Initialize()
        {
            messageParser = new ProductMessageParser();
            mockEsbMessage = new Mock<IEsbMessage>();
        }

        [TestMethod]
        public void ParseMessage_MessageHasOneProduct_ShouldParseMessage()
        {
            //Given
            string messageText = File.ReadAllText("TestMessages/product_message_single.xml");
            mockEsbMessage.SetupGet(m => m.MessageText).Returns(messageText);

            //When
            var models = messageParser.ParseMessage(mockEsbMessage.Object);

            //Then
            var model = models.Single();
            Assert.AreEqual(2216306, model.ItemID);
            Assert.AreEqual(ItemTypes.RetailSale, model.ItemTypeID);
            Assert.AreEqual("18053720936", model.ScanCode);
            Assert.AreEqual("CAULIFLOWER AU GRATIN", model.Desc_Product);
            Assert.AreEqual("WFM CAULIFLWR GRATIN", model.Desc_POS);
            Assert.AreEqual("1", model.PackageUnit);
            Assert.AreEqual("32", model.RetailSize);
            Assert.AreEqual("OZ", model.RetailUOM);
            Assert.AreEqual(true, model.FoodStampEligible);
            Assert.AreEqual("0100001", model.MessageTaxClassHCID);
            Assert.AreEqual(79232, model.BrandHCID);
            Assert.AreEqual(4900, model.PSNumber);
            Assert.AreEqual(84190, model.SubBrickID);
            Assert.AreEqual(null, model.NationalClassID);
        }
    }
}
