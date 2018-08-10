using MammothWebApi.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MammothWebApi.Tests.Helpers
{
    internal static class PriceTestData
    {
        internal static class Valid
        {
            public const int FiveDigitBusinessUnit = 12345;
            public const string BananasScanCode = "4011";
            public const string ThirteenDigitScanCode = "2061424933821";
            public static readonly DateTime DateIn2018April = new DateTime(2018, 4, 24, 20, 20, 20);
        }

        internal static class Bad
        {
            public const int NegativeBusinessUnit = -1;
            public const string FourteenDigitScanCode = "34567890123456";
            public static readonly DateTime DateIn2009June = new DateTime(2009, 6, 21, 8, 45, 0);
        }
        public static ItemStorePriceModel CreateItemStorePriceModel(int itemID, string scanCode, int businessUnit)
        {
            return CreateItemStorePriceModel(itemID, scanCode, businessUnit,
                2.48M, "REG", "REG", DateTime.Now.AddDays(-3));
        }

        public static ItemStorePriceModel CreateItemStorePriceModel(int itemID, string scanCode, int businessUnit,
            decimal price, string priceType, string priceTypeAttributem, DateTime startDate, DateTime? endDate = null)
        {
            var testItemStorePriceModel = new ItemStorePriceModel
            {
                ItemId = itemID,
                ScanCode = scanCode,
                BusinessUnitID = businessUnit,
                Authorized = true,
                BrandName = "Test Brand",
                ItemDescription = "Test ItemDescription",
                PackageUnit = "1",
                RetailSize = "1",
                RetailUom = "EA",
                FoodStamp = false,
                SubTeam = "Test SubTeam",
                SignDescription = "Test Sign Description",
                PriceType = priceType,
                Multiple = 1,
                Price = price,
                SellableUom = "EA",
                StartDate = startDate.Date,
                EndDate = endDate?.Date,
                Currency = "USD",
                PriceAttribute = priceTypeAttributem,
            };
            return testItemStorePriceModel;
        }
    }
}
