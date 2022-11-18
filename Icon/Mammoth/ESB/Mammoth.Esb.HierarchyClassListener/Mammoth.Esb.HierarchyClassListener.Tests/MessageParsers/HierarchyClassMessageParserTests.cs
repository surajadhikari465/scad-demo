using Icon.Dvs.Model;
using Mammoth.Common.DataAccess;
using Mammoth.Esb.HierarchyClassListener.MessageParsers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;

namespace Mammoth.Esb.HierarchyClassListener.Tests.MessageParsers
{
    [TestClass]
    public class HierarchyClassMessageParserTests
    {
        private HierarchyClassMessageParser messageParser;

        [TestInitialize]
        public void Initialize()
        {
            messageParser = new HierarchyClassMessageParser();
        }

        [TestMethod]
        public void ParseMessage_BrandMessage_ShouldReturnMessage()
        {
            //Given
            var messageText = File.ReadAllText(@"TestMessages/BrandMessage.xml");
            var message = new DvsMessage(new DvsSqsMessage(), messageText);

            //When
            var brand = messageParser.ParseMessage(message).Single();

            //Then
            Assert.AreEqual(68950, brand.HierarchyClassId);
            Assert.AreEqual("ANGOVE", brand.HierarchyClassName);
            Assert.AreEqual(0, brand.HierarchyClassParentId);
            Assert.AreEqual(Hierarchies.Brands, brand.HierarchyId);
            Assert.AreEqual(Constants.Brands.HierarchyLevels.Brand, brand.HierarchyLevelName);
        }

        [TestMethod]
        public void ParseMessage_MerchandiseMessage_ShouldReturnMessage()
        {
            //Given
            var messageText = File.ReadAllText(@"TestMessages/MerchandiseMessage.xml");
            var message = new DvsMessage(new DvsSqsMessage(), messageText);

            //When
            var merchandise = messageParser.ParseMessage(message).Single();

            //Then
            Assert.AreEqual(83426, merchandise.HierarchyClassId);
            Assert.AreEqual("Wine - Still", merchandise.HierarchyClassName);
            Assert.AreEqual(82867, merchandise.HierarchyClassParentId);
            Assert.AreEqual(Hierarchies.Merchandise, merchandise.HierarchyId);
            Assert.AreEqual(Constants.Merchandise.HierarchyLevels.Gs1Brick, merchandise.HierarchyLevelName);
        }
    }
}
