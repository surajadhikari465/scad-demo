using System;
using System.Collections.Generic;
using System.IO;
using Esb.Core.Serializer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using WebSupport.DataAccess.Models;
using WebSupport.MessageBuilders;
using WebSupport.Models;
using Contracts = Icon.Esb.Schemas.Wfm.Contracts;

namespace WebSupport.Tests.MessageBuilders
{
    [TestClass]
    public class PriceResetMessageBuilderTest
    {
        private PriceResetMessageBuilder builder;
        private Mock<ISerializer<Contracts.items>> serializer;
        private ISerializer<Contracts.items> serializerItem;

        [TestInitialize]
        public void Initialize()
        {
            this.serializer = new Mock<ISerializer<Contracts.items>>();
            this.serializerItem = new Serializer<Contracts.items>();
            builder = new PriceResetMessageBuilder(this.serializerItem);
        }

        [TestMethod]
        public void BuildMessage_OnePriceWithNoGpmID_ReturnsPriceMessage()
        {
            //Given
            PriceResetMessageBuilderModel request = new PriceResetMessageBuilderModel
            {
                PriceResetPrices = new List<PriceResetPrice>
                {
                    new PriceResetPrice
                    {
                        BusinessUnitId = 10654,
                        ItemId = 1,
                        ScanCode = "4011",
                        ItemTypeCode = "RTL",
                        StoreName = "Lake Oswego",
                        PriceType = "REG",
                        PriceTypeAttribute = "REG",
                        SellableUom = "EA",
                        CurrencyCode = "USD",
                        Price = 5.00M,
                        Multiple = 1,
                        SequenceId = "1",
                        PatchFamilyId = "1-10654",
                        StartDate = new DateTime(2018, 5, 24)
                    }
                }
            };

            //When
            var message = builder.BuildMessage(request);

            //Then
            Assert.AreEqual(File.ReadAllText(@"TestMessages\PriceResetMessageNoGpmID.xml"), message);
            var a = JsonConvert.SerializeObject(new { Message = message });
            Console.WriteLine(a);
        }

        [TestMethod]
        public void BuildMessage_OnePriceWithGpmID_ReturnsPriceMessage()
        {
            //Given
            PriceResetMessageBuilderModel request = new PriceResetMessageBuilderModel
            {
                PriceResetPrices = new List<PriceResetPrice>
                {
                    new PriceResetPrice
                    {
                        GpmId = new Guid("8f06087a-621e-4a2d-8441-01a1f5d01d8f"),
                        BusinessUnitId = 10654,
                        ItemId = 1,
                        ScanCode = "4011",
                        ItemTypeCode = "RTL",
                        StoreName = "Lake Oswego",
                        PriceType = "REG",
                        PriceTypeAttribute = "REG",
                        SellableUom = "EA",
                        CurrencyCode = "USD",
                        Price = 5.00M,
                        Multiple = 1,
                        SequenceId = "1",
                        PatchFamilyId = "1-10654",
                        StartDate = new DateTime(2018, 5, 24)
                    }
                }
            };

            //When
            var message = builder.BuildMessage(request);

            //Then
            Assert.AreEqual(File.ReadAllText(@"TestMessages\PriceResetMessageWithGpmID.xml"), message);
            var a = JsonConvert.SerializeObject(new { Message = message });
            Console.WriteLine(a);
        }
    }
}
