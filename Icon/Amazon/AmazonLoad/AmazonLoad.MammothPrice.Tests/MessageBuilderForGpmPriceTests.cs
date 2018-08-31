using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace AmazonLoad.MammothPrice.Test
{
    [TestClass]
    public class MessageBuilderForGpmPriceTests
    {
        [TestMethod]
        public void MessageBuilderForGpmPrice_BuildGpmMessage_SerializesGpmPriceModelMessage()
        {
            //Given
            var testPriceModels = new List<PriceModelGpm>
            {
                new PriceModelGpm
                {
                    EndDate = null,
                    StartDate = DateTime.Now.AddDays(-3).Date,
                    Multiple = 1,
                    Price = 1.99m,
                    PriceType = "REG",
                    CurrencyCode = "USD",
                    SellableUOM = "EA",
                    LocaleName = "XXX",
                    BusinessUnitId = 10234,
                    ItemTypeDesc = "blahl;sdfkl;sdfkljasl;djksdjf",
                    ItemTypeCode = null,
                    ItemId = 565555,
                    ScanCode = "999999999",
                    GpmId = null,
                    PercentOff = null,
                    PriceTypeAttribute = null
                },
            };
            // When
            var result = MessageBuilderForGpmPrice.BuildGpmMessage(testPriceModels);
            // Then
            Assert.IsNotNull(result); //TOO manually debug
        }

    }
}
