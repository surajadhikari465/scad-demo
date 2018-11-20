using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KitBuilder.Esb.LocaleListener.MessageParsers;
using Icon.Esb.Subscriber;
using Moq;
using System.IO;

namespace KitBuilder.Esb.LocaleListener.Tests.MessageParsers
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
        public void ParseMessage_ValidVenueMessage_ShouldReturnLocaleModel()
        {
            //Given xml containing venue added schema
            mockMessage.SetupGet(m => m.MessageText).Returns(File.ReadAllText(@"TestMessages\venue_valid_message.xml"));

            //When
            var models = messageParser.ParseMessage(mockMessage.Object);

            //Then
            Assert.AreEqual(1, models.Count);
            Assert.AreEqual(2478, models[0].LocaleID);
			Assert.AreEqual("New Venue", models[0].LocaleName);
			Assert.AreEqual(5, models[0].LocaleTypeID);
			Assert.AreEqual(1298, models[0].StoreID);
			Assert.AreEqual(44, models[0].MetroID);
			Assert.AreEqual(7, models[0].RegionID);
			Assert.AreEqual(1, models[0].ChainID);
			Assert.AreEqual("NE", models[0].RegionCode);
			Assert.AreEqual(new DateTime(2001, 4, 25), models[0].LocaleOpenDate.Value);
			Assert.AreEqual(null, models[0].LocaleCloseDate);
			Assert.AreEqual(null, models[0].StoreAbbreviation);
			Assert.AreEqual(null, models[0].BusinessUnitID);
			Assert.AreEqual(null, models[0].CurrencyCode);
			Assert.AreEqual(true, models[0].Hospitality);

        }
		[TestMethod]
		public void ParseMessage_ValidStoreMessage_ShouldReturnLocaleModel()
		{
			//Given xml with store related schema
			mockMessage.SetupGet(m => m.MessageText).Returns(File.ReadAllText(@"TestMessages\valid_message.xml"));

			//When
			var models = messageParser.ParseMessage(mockMessage.Object);

			//Then
			Assert.AreEqual(1, models.Count);
			Assert.AreEqual(10130, models[0].LocaleID);
			Assert.AreEqual("Boca Raton", models[0].LocaleName);
			Assert.AreEqual(4, models[0].LocaleTypeID);
			Assert.AreEqual(null, models[0].StoreID);
			Assert.AreEqual(14, models[0].MetroID);
			Assert.AreEqual(2, models[0].RegionID);
			Assert.AreEqual(1, models[0].ChainID);
			Assert.AreEqual("FL", models[0].RegionCode);
			Assert.AreEqual(new DateTime(2001, 4, 25), models[0].LocaleOpenDate.Value);
			Assert.AreEqual(null, models[0].LocaleCloseDate);
			Assert.AreEqual("BCA", models[0].StoreAbbreviation);
			Assert.AreEqual(10130, models[0].BusinessUnitID);
			Assert.AreEqual("USD", models[0].CurrencyCode);
			Assert.AreEqual(false, models[0].Hospitality);

		}
	}
}
