using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mammoth.Esb.LocaleListener.MessageParsers;
using Icon.Esb.Subscriber;
using Moq;
using System.IO;

namespace Mammoth.Esb.LocaleListener.Tests.MessageParsers
{
    [TestClass]
    public class LocaleMessageParserTests
    {
        private LocaleMessageParser messageParser;
        private Mock<IEsbMessage> mockMessage;

        [TestInitialize]
        public void Initialize()
        {
            messageParser = new LocaleMessageParser();
            mockMessage = new Mock<IEsbMessage>();
        }

        [TestMethod]
        public void ParseMessage_ValidMessage_ShouldReturnLocaleModel()
        {
            //Given
            mockMessage.SetupGet(m => m.MessageText).Returns(File.ReadAllText(@"TestMessages\valid_message.xml"));

            //When
            var models = messageParser.ParseMessage(mockMessage.Object);

            //Then
            Assert.AreEqual(1, models.Count);
            Assert.AreEqual(10130, models[0].BusinessUnitID);
            Assert.AreEqual("Boca Raton", models[0].StoreName);
            Assert.AreEqual("BCA", models[0].StoreAbbrev);
            Assert.AreEqual("FL", models[0].Region);
            Assert.AreEqual("1400 Glades Road Ste 110", models[0].Address1);
            Assert.AreEqual(null, models[0].Address2);
            Assert.AreEqual(null, models[0].Address3);
            Assert.AreEqual("Boca Raton", models[0].City);
            Assert.AreEqual("United States", models[0].Country);
            Assert.AreEqual("USA", models[0].CountryAbbrev);
            Assert.AreEqual("33486", models[0].PostalCode);
            Assert.AreEqual("Florida", models[0].Territory);
            Assert.AreEqual("FL", models[0].TerritoryAbbrev);
            Assert.AreEqual("Eastern Standard Time", models[0].Timezone);
            Assert.AreEqual(new DateTime(2001, 4, 25), models[0].LocaleOpenDate.Value);
            Assert.IsFalse(models[0].LocaleCloseDate.HasValue);
        }

        [TestMethod]
        public void ParseMessage_ValidMessageWithMinCloseDate_ShouldReturnLocaleModel()
        {
            //Given
            mockMessage.SetupGet(m => m.MessageText).Returns(File.ReadAllText(@"TestMessages\valid_message_with_min_close_date.xml"));

            //When
            var models = messageParser.ParseMessage(mockMessage.Object);

            //Then
            Assert.IsFalse(models[0].LocaleCloseDate.HasValue);
        }
    }
}
