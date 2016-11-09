using MammothWebApi.Extensions;
using MammothWebApi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MammothWebApi.Tests
{
    [TestClass]
    public class ServiceModelMappingExtensionsTest
    {
        [TestMethod]
        public void ToPriceServiceModel_CancelAllSalesTrue_ShouldSetEndDateToStartOfDay()
        {
            // Given
            var newPrice = new PriceModel
            {
                CancelAllSales = true,
                EndDate = DateTime.Now
            };

            // When
            var newPriceServiceModel = new List<PriceModel> { newPrice }.ToPriceServiceModel();

            // Then
            Assert.AreEqual(DateTime.Now.Date, newPriceServiceModel.First().EndDate);
        }

        [TestMethod]
        public void ToPriceServiceModel_CancelAllSalesFalse_ShouldSetEndDateToEndOfDay()
        {
            // Given
            var newPrice = new PriceModel
            {
                CancelAllSales = false,
                EndDate = new DateTime(2016, 6, 1, 14, 20, 23),

            };

            // When
            var newPriceServiceModel = new List<PriceModel> { newPrice }.ToPriceServiceModel();

            // Then
            Assert.AreEqual(new DateTime(2016, 6, 1, 23, 59, 59, 997).TimeOfDay, newPriceServiceModel.First().EndDate.Value.TimeOfDay);
        }

        [TestMethod]
        public void ToPriceServiceModel_CancelAllSalesFalseAndEndDateIsNull_ShouldSetEndDateToNull()
        {
            // Given
            var newPrice = new PriceModel
            {
                CancelAllSales = false,
                EndDate = null,

            };

            // When
            var newPriceServiceModel = new List<PriceModel> { newPrice }.ToPriceServiceModel();

            // Then
            Assert.IsNull(newPriceServiceModel.First().EndDate);
        }
    }
}
