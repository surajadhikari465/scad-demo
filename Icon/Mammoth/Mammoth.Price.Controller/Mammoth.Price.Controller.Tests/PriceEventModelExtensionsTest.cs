using Mammoth.Price.Controller.DataAccess.Models;
using Mammoth.Price.Controller.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mammoth.Price.Controller.Tests
{
    [TestClass]
    public class PriceEventModelExtensionsTest
    {
        private List<PriceEventModel> priceEventList = new List<PriceEventModel>();


        [TestInitialize]
        public void InitializeTests()
        {
            PriceEventModel priceEvent = new PriceEventModel();
            priceEvent.BusinessUnitId = 1;
            priceEvent.ScanCode = "111";
            priceEvent.NewPriceType = "REG";
            priceEvent.CurrentPriceType = "REG";
            priceEvent.CurrentRegularPrice = 6.99M;
            priceEvent.NewRegularPrice = 6.99M;
            priceEvent.NewStartDate = new DateTime(2016, 3, 3);
            priceEvent.CurrencyCode = "USD";
            priceEvent.PriceUom = "EA";
            priceEventList.Add(priceEvent);

            PriceEventModel salePriceEvent = new PriceEventModel();
            salePriceEvent.BusinessUnitId = 2;
            salePriceEvent.ScanCode = "222";
            salePriceEvent.NewPriceType = "SAL";
            salePriceEvent.CurrentPriceType = "REG";
            salePriceEvent.CurrentRegularPrice = 6.99M;
            salePriceEvent.NewRegularPrice = 6.99M;
            salePriceEvent.NewSalePrice = 4.99M;
            salePriceEvent.NewStartDate = DateTime.Today.AddDays(7);
            salePriceEvent.NewSaleEndDate = priceEvent.NewStartDate.AddDays(14);
            salePriceEvent.CurrencyCode = "USD";
            salePriceEvent.PriceUom = "EA";
            priceEventList.Add(salePriceEvent);
        }

        [TestMethod]
        public void MapToPriceModel_PriceEventModelHasRegAndSalePriceChanges_ShouldReturnBothPricesInPriceModelList()
        {
            //Given
            int expectedPriceCount = this.priceEventList.Count;

            //When
            List<PriceModel> prices = this.priceEventList.MapToPriceModel().ToList();

            //Then
            Assert.AreEqual(expectedPriceCount, prices.Count(), "Count didn't match");
            Assert.IsTrue(prices.Any(p => p.PriceType == "REG") && prices.Any(p => p.PriceType == "SAL"));

            for (int i = 0; i < prices.Count(); i++)
            {
                Assert.AreEqual(this.priceEventList[i].BusinessUnitId, prices[i].BusinessUnitId, "Business Units didn't match");
                Assert.AreEqual(this.priceEventList[i].ScanCode, prices[i].ScanCode, "Scan Codes didn't match");
                Assert.AreEqual(this.priceEventList[i].NewSalePrice ?? this.priceEventList[i].NewRegularPrice, prices[i].Price, "Prices didn't match");
                Assert.AreEqual(this.priceEventList[i].NewSaleMultiple ?? this.priceEventList[i].NewRegularMultiple, prices[i].Multiple, "Multiples didn't match");
                Assert.AreEqual(this.priceEventList[i].NewStartDate, prices[i].StartDate, "Start Dates didn't match");
                Assert.AreEqual(this.priceEventList[i].NewSaleEndDate, prices[i].EndDate, "End Dates didn't match");
                Assert.AreEqual(this.priceEventList[i].NewPriceType, prices[i].PriceType, "Price Types didn't match");
                Assert.AreEqual(this.priceEventList[i].PriceUom, prices[i].PriceUom, "Price Uoms didn't match");
                Assert.AreEqual(this.priceEventList[i].CurrencyCode, prices[i].CurrencyCode, "Currency Code didn't match");
            }
        }

        [TestMethod]
        public void MapToPriceModel_PriceEventHasASaleWithARegularPriceChange_ShouldReturnSaleAndRegRowsInPriceModelList()
        {
            //Given
            this.priceEventList.Clear();
            PriceEventModel priceEvent = new PriceEventModel();
            priceEvent.BusinessUnitId = 3;
            priceEvent.ScanCode = "333";
            priceEvent.CurrentRegularPrice = 6.99M;
            priceEvent.CurrentSalePrice = null;
            priceEvent.CurrentPriceType = "REG";
            priceEvent.CurrentSaleStartDate = null;
            priceEvent.CurrentSaleEndDate = null;
            priceEvent.NewRegularPrice = 7.99M;
            priceEvent.NewSalePrice = 4.99M;
            priceEvent.NewSaleMultiple = 1;
            priceEvent.NewPriceType = "SAL";
            priceEvent.NewStartDate = DateTime.Today.AddDays(4);
            priceEvent.NewSaleEndDate = priceEvent.NewStartDate.AddDays(5);
            this.priceEventList.Add(priceEvent);
            int expectedPriceCount = this.priceEventList.Count + 1; // Expecting REG and TPR rows

            //When
            List<PriceModel> prices = this.priceEventList.MapToPriceModel().ToList();

            //Then
            Assert.AreEqual(expectedPriceCount, prices.Count(), "Count didn't match");
            Assert.IsTrue(prices.Any(p => p.PriceType == "REG") && prices.Any(p => p.PriceType == "SAL"));

            PriceModel regPrice = prices.Single(p => p.PriceType == "REG");
            PriceModel salPrice = prices.Single(p => p.PriceType == "SAL");

            Assert.AreEqual(priceEvent.BusinessUnitId, salPrice.BusinessUnitId, "Business Units didn't match");
            Assert.AreEqual(priceEvent.ScanCode, salPrice.ScanCode, "Scan Codes didn't match");
            Assert.AreEqual(priceEvent.NewSalePrice, salPrice.Price, "Prices didn't match");
            Assert.AreEqual(priceEvent.NewSaleMultiple, salPrice.Multiple, "Multiples didn't match");
            Assert.AreEqual(priceEvent.NewStartDate, salPrice.StartDate, "Start Dates didn't match");
            Assert.AreEqual(priceEvent.NewSaleEndDate, salPrice.EndDate, "End Dates didn't match");
            Assert.AreEqual(priceEvent.NewPriceType, salPrice.PriceType, "Price Types didn't match");
            Assert.AreEqual(priceEvent.PriceUom, salPrice.PriceUom, "Price Uoms didn't match");
            Assert.AreEqual(priceEvent.CurrencyCode, salPrice.CurrencyCode, "Currency Code didn't match");

            Assert.AreEqual(priceEvent.BusinessUnitId, regPrice.BusinessUnitId, "Business Units didn't match");
            Assert.AreEqual(priceEvent.ScanCode, regPrice.ScanCode, "Scan Codes didn't match");
            Assert.AreEqual(priceEvent.NewRegularPrice, regPrice.Price, "Prices didn't match");
            Assert.AreEqual(priceEvent.NewRegularMultiple, regPrice.Multiple, "Multiples didn't match");
            Assert.AreEqual(priceEvent.NewStartDate, regPrice.StartDate, "Start Dates didn't match");
            Assert.AreEqual(null, regPrice.EndDate, "End Dates didn't match");
            Assert.AreEqual("REG", regPrice.PriceType, "Price Types didn't match");
            Assert.AreEqual(priceEvent.PriceUom, regPrice.PriceUom, "Price Uoms didn't match");
            Assert.AreEqual(priceEvent.CurrencyCode, regPrice.CurrencyCode, "Currency Code didn't match");
        }

        [TestMethod]
        public void MapToPriceModel_PriceEventHasASaleWithAMultipleChange_ShouldReturnRowForRegAndSaleChange()
        {
            //Given
            this.priceEventList.Clear();
            PriceEventModel priceEvent = new PriceEventModel();
            priceEvent.Region = "NC";
            priceEvent.BusinessUnitId = 3;
            priceEvent.ScanCode = "333";
            priceEvent.CurrentRegularPrice = 6.99M;
            priceEvent.CurrentRegularMultiple = 1;
            priceEvent.CurrentSalePrice = null;
            priceEvent.CurrentPriceType = "REG";
            priceEvent.CurrentSaleStartDate = null;
            priceEvent.CurrentSaleEndDate = null;
            priceEvent.NewRegularPrice = 6.99M;
            priceEvent.NewRegularMultiple = 3;
            priceEvent.NewSalePrice = 4.99M;
            priceEvent.NewSaleMultiple = 1;
            priceEvent.NewPriceType = "SAL";
            priceEvent.NewStartDate = DateTime.Today.AddDays(4);
            priceEvent.NewSaleEndDate = priceEvent.NewStartDate.AddDays(5);
            priceEvent.CurrencyCode = "USD";
            priceEvent.PriceUom = "EA";
            this.priceEventList.Add(priceEvent);
            int expectedPriceCount = this.priceEventList.Count + 1; // Expecting REG and TPR rows

            //When
            List<PriceModel> prices = this.priceEventList.MapToPriceModel().ToList();

            //Then
            Assert.AreEqual(expectedPriceCount, prices.Count(), "Count didn't match");
            Assert.IsTrue(prices.Any(p => p.PriceType == "REG") && prices.Any(p => p.PriceType == "SAL"));

            PriceModel regPrice = prices.Single(p => p.PriceType == "REG");
            PriceModel salPrice = prices.Single(p => p.PriceType == "SAL");

            Assert.AreEqual(priceEvent.BusinessUnitId, salPrice.BusinessUnitId, "Business Units didn't match");
            Assert.AreEqual(priceEvent.ScanCode, salPrice.ScanCode, "Scan Codes didn't match");
            Assert.AreEqual(priceEvent.NewSalePrice, salPrice.Price, "Prices didn't match");
            Assert.AreEqual(priceEvent.NewSaleMultiple, salPrice.Multiple, "Multiples didn't match");
            Assert.AreEqual(priceEvent.NewStartDate, salPrice.StartDate, "Start Dates didn't match");
            Assert.AreEqual(priceEvent.NewSaleEndDate, salPrice.EndDate, "End Dates didn't match");
            Assert.AreEqual(priceEvent.NewPriceType, salPrice.PriceType, "Price Types didn't match");
            Assert.AreEqual(priceEvent.PriceUom, salPrice.PriceUom, "Price Uoms didn't match");
            Assert.AreEqual(priceEvent.CurrencyCode, salPrice.CurrencyCode, "Currency Code didn't match");

            Assert.AreEqual(priceEvent.BusinessUnitId, regPrice.BusinessUnitId, "Business Units didn't match");
            Assert.AreEqual(priceEvent.ScanCode, regPrice.ScanCode, "Scan Codes didn't match");
            Assert.AreEqual(priceEvent.NewRegularPrice, regPrice.Price, "Prices didn't match");
            Assert.AreEqual(priceEvent.NewRegularMultiple, regPrice.Multiple, "Multiples didn't match");
            Assert.AreEqual(priceEvent.NewStartDate, regPrice.StartDate, "Start Dates didn't match");
            Assert.AreEqual(null, regPrice.EndDate, "End Dates didn't match");
            Assert.AreEqual("REG", regPrice.PriceType, "Price Types didn't match");
            Assert.AreEqual(priceEvent.PriceUom, regPrice.PriceUom, "Price Uoms didn't match");
            Assert.AreEqual(priceEvent.CurrencyCode, regPrice.CurrencyCode, "Currency Code didn't match");
        }
    }
}
