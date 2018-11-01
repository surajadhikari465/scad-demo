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
        public void BuildMessage_OneRegPrice_ReturnsPriceMessage()
        {
            //Given
            var expectedMessage = File.ReadAllText(@"TestMessages\PriceResetMessageRegPrice.xml");
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
            Assert.AreEqual(expectedMessage, message);
            var a = JsonConvert.SerializeObject(new { Message = message });
            Console.WriteLine(a);
        }

        [TestMethod]
        public void BuildMessage_RegAndTprPrices_ReturnsPriceMessage()
        {
            //Given
            var expectedMessage = File.ReadAllText(@"TestMessages\PriceResetMessageRegTprPrices.xml");
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
                    },
                    new PriceResetPrice
                    {
                        GpmId = new Guid("8f06087a-621e-4a2d-8441-01a1f5d01d8f"),
                        BusinessUnitId = 10654,
                        ItemId = 1,
                        ScanCode = "4011",
                        ItemTypeCode = "RTL",
                        StoreName = "Lake Oswego",
                        PriceType = "TPR",
                        PriceTypeAttribute = "ISS",
                        SellableUom = "EA",
                        CurrencyCode = "USD",
                        Price = 4.00M,
                        Multiple = 1,
                        SequenceId = "1",
                        PatchFamilyId = "1-10654",
                        StartDate = new DateTime(2018, 5, 24),
                        EndDate = new DateTime(2018, 5, 24, 23, 59, 59).AddDays(14)
                    }
                }
            };

            //When
            var message = builder.BuildMessage(request);

            //Then
            Assert.AreEqual(expectedMessage, message);
            var a = JsonConvert.SerializeObject(new { Message = message });
            Console.WriteLine(a);
        }

        [TestMethod]
        public void BuildMessage_RegAndTwoRewardPercentOff_ReturnsPriceMessage()
        {
            //Given
            var expectedMessage = File.ReadAllText(@"TestMessages\PriceResetMessageRegAndRewards.xml");
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
                        Price = 5,
                        Multiple = 1,
                        SequenceId = "1",
                        PatchFamilyId = "1-10654",
                        StartDate = new DateTime(2018, 5, 24)
                    },
                    new PriceResetPrice
                    {
                        GpmId = new Guid("8f06087a-621e-4a2d-8441-01a1f5d01d8f"),
                        BusinessUnitId = 10654,
                        ItemId = 1,
                        ScanCode = "4011",
                        ItemTypeCode = "RTL",
                        StoreName = "Lake Oswego",
                        PriceType = "RWD",
                        PriceTypeAttribute = "PMI",
                        SellableUom = "EA",
                        CurrencyCode = "USD",
                        Price = 0.00M,
                        Multiple = 1,
                        SequenceId = "1",
                        PatchFamilyId = "1-10654",
                        StartDate = new DateTime(2018, 5, 24),
                        EndDate = new DateTime(2018, 6, 1, 23, 59, 59),
                        PercentOff = 20
                    },
                    new PriceResetPrice
                    {
                        GpmId = new Guid("8f06087a-621e-4a2d-8441-01a1f5d01d8f"),
                        BusinessUnitId = 10654,
                        ItemId = 1,
                        ScanCode = "4011",
                        ItemTypeCode = "RTL",
                        StoreName = "Lake Oswego",
                        PriceType = "RWD",
                        PriceTypeAttribute = "PMI",
                        SellableUom = "EA",
                        CurrencyCode = "USD",
                        Price = 0,
                        Multiple = 1,
                        SequenceId = "1",
                        PatchFamilyId = "1-10654",
                        StartDate = new DateTime(2018, 6, 2),
                        EndDate = new DateTime(2018, 6, 14, 23, 59, 59),
                        PercentOff = 20
                    }
                }
            };

            //When
            var message = builder.BuildMessage(request);

            //Then
            Assert.AreEqual(expectedMessage, message);
            var a = JsonConvert.SerializeObject(new { Message = message });
            Console.WriteLine(a);
        }

        [TestMethod]
        public void BuildMessage_RegAndTprAndRewardPercentOff_ReturnsPriceMessage()
        {
            //Given
            var expectedMessage = File.ReadAllText(@"TestMessages\PriceResetMessageRegTprAndRewardsPercentOff.xml");
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
                    },
                    new PriceResetPrice
                    {
                        GpmId = new Guid("8f06087a-621e-4a2d-8441-01a1f5d01d8f"),
                        BusinessUnitId = 10654,
                        ItemId = 1,
                        ScanCode = "4011",
                        ItemTypeCode = "RTL",
                        StoreName = "Lake Oswego",
                        PriceType = "TPR",
                        PriceTypeAttribute = "MSAL",
                        SellableUom = "EA",
                        CurrencyCode = "USD",
                        Price = 5.00M,
                        Multiple = 1,
                        SequenceId = "1",
                        PatchFamilyId = "1-10654",
                        StartDate = new DateTime(2018, 5, 24),
                        EndDate = new DateTime(2018, 6, 1, 23, 59, 59),
                        PercentOff = null
                    },
                    new PriceResetPrice
                    {
                        GpmId = new Guid("8f06087a-621e-4a2d-8441-01a1f5d01d8f"),
                        BusinessUnitId = 10654,
                        ItemId = 1,
                        ScanCode = "4011",
                        ItemTypeCode = "RTL",
                        StoreName = "Lake Oswego",
                        PriceType = "RWD",
                        PriceTypeAttribute = "PMI",
                        SellableUom = "EA",
                        CurrencyCode = "USD",
                        Price = 0.00M,
                        Multiple = 1,
                        SequenceId = "1",
                        PatchFamilyId = "1-10654",
                        StartDate = new DateTime(2018, 5, 24),
                        EndDate = new DateTime(2018, 6, 1, 23, 59, 59),
                        PercentOff = 20
                    }
                }
            };

            //When
            var message = builder.BuildMessage(request);

            //Then
            Assert.AreEqual(expectedMessage, message);
            var a = JsonConvert.SerializeObject(new { Message = message });
            Console.WriteLine(a);
        }

        [TestMethod]
        public void BuildMessage_RegAndTprAndRewardPercentPrice_ReturnsPriceMessage()
        {
            //Given
            var expectedMessage = File.ReadAllText(@"TestMessages\PriceResetRegTprAndRewardPrice.xml");
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
                        Price = 7.00M,
                        Multiple = 1,
                        SequenceId = "1",
                        PatchFamilyId = "1-10654",
                        StartDate = new DateTime(2018, 5, 24)
                    },
                    new PriceResetPrice
                    {
                        GpmId = new Guid("8f06087a-621e-4a2d-8441-01a1f5d01d8f"),
                        BusinessUnitId = 10654,
                        ItemId = 1,
                        ScanCode = "4011",
                        ItemTypeCode = "RTL",
                        StoreName = "Lake Oswego",
                        PriceType = "TPR",
                        PriceTypeAttribute = "MSAL",
                        SellableUom = "EA",
                        CurrencyCode = "USD",
                        Price = 6.00M,
                        Multiple = 1,
                        SequenceId = "1",
                        PatchFamilyId = "1-10654",
                        StartDate = new DateTime(2018, 5, 24),
                        EndDate = new DateTime(2018, 6, 1, 23, 59, 59),
                        PercentOff = null
                    },
                    new PriceResetPrice
                    {
                        GpmId = new Guid("8f06087a-621e-4a2d-8441-01a1f5d01d8f"),
                        BusinessUnitId = 10654,
                        ItemId = 1,
                        ScanCode = "4011",
                        ItemTypeCode = "RTL",
                        StoreName = "Lake Oswego",
                        PriceType = "RWD",
                        PriceTypeAttribute = "PMI",
                        SellableUom = "EA",
                        CurrencyCode = "USD",
                        Price = 5.00M,
                        Multiple = 1,
                        SequenceId = "1",
                        PatchFamilyId = "1-10654",
                        StartDate = new DateTime(2018, 5, 24),
                        EndDate = new DateTime(2018, 6, 1, 23, 59, 59),
                        PercentOff = null
                    }
                }
            };

            //When
            var message = builder.BuildMessage(request);

            //Then
            Assert.AreEqual(expectedMessage, message);
            var a = JsonConvert.SerializeObject(new { Message = message });
            Console.WriteLine(a);
        }
    }
}
