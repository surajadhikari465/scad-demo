using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSupport.DataAccess.Models;
using WebSupport.MessageBuilders;

namespace WebSupport.Tests.MessageBuilders
{
    [TestClass]
    public class PriceRefreshMessageBuilderTests
    {
        private PriceRefreshMessageBuilder builder;

        [TestInitialize]
        public void Initialize()
        {
            builder = new PriceRefreshMessageBuilder();
        }

        [TestMethod]
        public void BuildMessage_1Price_ReturnsPriceMessage()
        {
            //Given
            List<GpmPrice> request = new List<GpmPrice>
            {
                new GpmPrice
                {
                    BusinessUnitId = 123,
                    CurrencyCode = "USD",
                    EndDate = DateTime.Parse("2018-01-03T16:33:46.6246444-06:00"),
                    GpmId = Guid.Parse("fc7cfdf1-e934-4f01-baaf-2e0e1d038219"),
                    InsertDateUtc = DateTime.Now,
                    ItemId = 1,
                    ItemTypeCode = "Test",
                    ModifiedDateUtc = DateTime.Now,
                    Multiple = 1,
                    TagExpirationDate = DateTime.Now,
                    PatchFamilyId = "1-123",
                    Price = 1m,
                    PriceId = 12345,
                    PriceType = "TPR",
                    PriceTypeAttribute = "REG",
                    Region = "FL",
                    ScanCode = "4011",
                    SellableUOM = "EA",
                    SequenceId = "2",
                    StartDate = DateTime.Parse("2018-01-03T16:33:46.6366771-06:00"),
                    StoreName = "SBV"
                }
            };

            //When
            var message = builder.BuildMessage(request);

            //Then
            Assert.AreEqual(File.ReadAllText(@"TestMessages\PriceRefreshMessage.xml"), message);
            var a = JsonConvert.SerializeObject(new { Message = message });
            Console.WriteLine(a);
        }
    }
}
