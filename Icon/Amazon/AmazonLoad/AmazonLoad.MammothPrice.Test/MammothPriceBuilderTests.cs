using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace AmazonLoad.MammothPrice.Test
{
    [TestClass]
    public class MammothPricebuilderTests
    {
        [TestMethod]
        public void MammothPriceBuilder_LoadMammothPrices_Test1()
        {
            //Given
            string connectionString = ConfigurationManager.ConnectionStrings["MammothContext"].ConnectionString;
            string region = "MA";
            int maxNumberOfRows = 10;

            // When
            var prices = MammothPriceBuilder.LoadMammothPrices(connectionString, region, maxNumberOfRows);

            // Then
            Assert.IsNotNull(prices); //TOO manually debug
        }

    }
}
