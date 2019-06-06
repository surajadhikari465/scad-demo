using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KitBuilder.Esb.LocaleListener.MessageParsers;
using Icon.Esb.Subscriber;
using Moq;
using System.IO;
using System.Linq;

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

            // venue has a storeid assoicated with it. store does not. store.localeid is used instead. 
            var venue = models.FirstOrDefault(m => m.StoreID.HasValue);
            var store = models.FirstOrDefault(m => !m.StoreID.HasValue);

            //Then
            Assert.AreEqual(1, models.Count);
            Assert.IsNotNull(venue);
            Assert.IsNull(store);
            Assert.AreEqual(2364, venue.LocaleID);
            Assert.AreEqual("Alta_Venue", venue.LocaleName);
            Assert.AreEqual(5, venue.LocaleTypeID);
            Assert.AreEqual(14, venue.MetroID);
            Assert.AreEqual(2, venue.RegionID);
            Assert.AreEqual(1, venue.ChainID);
            Assert.AreEqual(999, venue.StoreID);
            Assert.AreEqual("FL", venue.RegionCode);
            Assert.AreEqual(new DateTime(2019, 5, 23), venue.LocaleOpenDate.Value);
            Assert.AreEqual(false, venue.LocaleCloseDate.HasValue);
            Assert.AreEqual(null, venue.StoreAbbreviation);
            Assert.AreEqual(null, venue.BusinessUnitID);
            Assert.AreEqual(null, venue.CurrencyCode);
            Assert.AreEqual(true, venue.Hospitality);

        }
		[TestMethod]
		public void ParseMessage_ValidStoreMessage_ShouldReturnLocaleModel()
		{
			//Given xml with store related schema
			mockMessage.SetupGet(m => m.MessageText).Returns(File.ReadAllText(@"TestMessages\valid_message.xml"));

			//When
			var models = messageParser.ParseMessage(mockMessage.Object);
            var store = models.FirstOrDefault(m => !m.StoreID.HasValue);

            //Then
            Assert.AreEqual(1, models.Count);
            Assert.IsNotNull(store);
            Assert.AreEqual(999, store.LocaleID);
            Assert.AreEqual("Altamonte Springs", store.LocaleName);
            Assert.AreEqual(4, store.LocaleTypeID);
            Assert.AreEqual(14, store.MetroID);
            Assert.AreEqual(2, store.RegionID);
            Assert.AreEqual(1, store.ChainID);
            Assert.AreEqual("FL", store.RegionCode);
            Assert.AreEqual(new DateTime(2016, 1, 6), store.LocaleOpenDate.Value);
            Assert.AreEqual(new DateTime(2019, 5, 6), store.LocaleCloseDate.Value);
            Assert.AreEqual("APS", store.StoreAbbreviation);
            Assert.AreEqual(10516, store.BusinessUnitID);
            Assert.AreEqual("USD", store.CurrencyCode);
            Assert.AreEqual(false, models[0].Hospitality);

		}
	}
}
