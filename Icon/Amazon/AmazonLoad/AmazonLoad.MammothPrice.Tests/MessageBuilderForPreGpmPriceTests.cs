using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace AmazonLoad.MammothPrice.Test
{
    [TestClass]
    public class MessageBuilderForPreGpmPriceTests
    {
        [TestMethod]
        public void MessageBuilderForPreGpmPrice_BuildGpmMessage_SerializesPreGpmPriceModelMessage()
        {
            // Given
            var testPriceModels = new List<PriceModel>
            {
                new PriceModel
                {
                    EndDate = null,
                    StartDate = DateTime.Now.AddDays(-3).Date,
                    Multiple = 1,
                    Price = 1.99m,
                    SubPriceTypeCode = null,
                    PriceTypeCode = "REG",
                    CurrencyCode = "USD",
                    UomCode = "EA",
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
            var result = MessageBuilderForPreGpmPrice.BuildPreGpmMessage(testPriceModels);
            // Then
            Assert.IsNotNull(result); //TOO manually debug
        }
    }
}
