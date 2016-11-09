using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MessageGenerationWeb.MessageBuilders;
using MessageGenerationWeb.Models;
using MessageGenerationWeb.Services;
using System.Collections.Generic;

namespace MessageGenerationWeb.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            ItemPriceMessageBuilder builder = new ItemPriceMessageBuilder();
            builder.BuildDeleteMessages(new System.Collections.Generic.List<Models.ItemPriceDeleteModel>
                {
                    new ItemPriceDeleteModel
                    {
                        ScanCode = "1429",
                        BusinessUnit = "10130",
                        Price = 1m,
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now,
                        Uom = "EA"
                    },
                    new ItemPriceDeleteModel
                    {
                        ScanCode = "1427",
                        BusinessUnit = "10130",
                        Price = 1m,
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now,
                        Uom = "EA"
                    },
                    new ItemPriceDeleteModel
                    {
                        ScanCode = "1426",
                        BusinessUnit = "10130",
                        Price = 1m,
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now,
                        Uom = "EA"
                    },
                });
        }

        [TestMethod]
        public void MyTestMethod2()
        {
            ItemPriceService service = new ItemPriceService();
            service.DeleteItemPrices(new List<ItemPriceDeleteModel>
                {
                    new ItemPriceDeleteModel
                    {
                        ScanCode = "1429",
                        BusinessUnit = "10130",
                        Price = 1m,
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now,
                        Uom = "EA"
                    },
                    new ItemPriceDeleteModel
                    {
                        ScanCode = "1427",
                        BusinessUnit = "10130",
                        Price = 1m,
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now,
                        Uom = "EA"
                    },
                    new ItemPriceDeleteModel
                    {
                        ScanCode = "1426",
                        BusinessUnit = "10130",
                        Price = 1m,
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now,
                        Uom = "EA"
                    },
                });
        }
    }
}
