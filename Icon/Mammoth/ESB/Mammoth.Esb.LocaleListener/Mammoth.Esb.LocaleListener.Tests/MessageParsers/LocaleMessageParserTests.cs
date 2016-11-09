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
        }
    }
}
