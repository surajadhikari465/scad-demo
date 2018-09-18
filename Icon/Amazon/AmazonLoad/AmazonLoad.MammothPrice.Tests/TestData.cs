using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonLoad.MammothPrice.Tests
{
    public class TestData
    {
        public PriceModelGpm PriceGpm_REG_10414_7777777777 = new PriceModelGpm
        {
            Region = "MA",
            GpmId = Guid.Parse("83AA8D04-B66D-40EC-A15E-91C536FCE363"),
            ItemId = 363444,
            ItemTypeCode = "RTL",
            ItemTypeDesc = "Retail Sale",
            ScanCode = "7777777777",
            BusinessUnitId = 10414,
            LocaleName = "Foggy Bottom",
            PriceType = "REG",
            PriceTypeDesc = "Regular Price",
            PriceTypeId = "1",
            CurrencyCode = "USD",
            SellableUOM = "EA",
            UomName = "EACH",
            Multiple = 1,
            Price = 4.44m,
            SubPriceTypeCode = null,
            SubPriceTypeId = null,
            SubPriceTypeDesc = null,
            PercentOff = null,
            TagExpirationDate = new DateTime(2019, 4, 24).Date,
            ModifiedDateUtc = DateTime.Now.AddDays(10),
            StartDate = new DateTime(2018, 6, 21).Date,
            EndDate = null,
        };

        public PriceModelGpm PriceGpm_REG_10414_666666666 = new PriceModelGpm
        {
            Region = "MA",
            GpmId = Guid.Parse("62516AD2-9467-40A6-ABC6-7DE152C6954E"),
            ItemId = 445674,
            ItemTypeCode = "RTL",
            ItemTypeDesc = "Retail Sale",
            ScanCode = "666666666",
            BusinessUnitId = 10414,
            LocaleName = "Foggy Bottom",
            PriceType = "REG",
            PriceTypeDesc = "Regular Price",
            PriceTypeId = "1",
            Price = 12.77m,
            CurrencyCode = "USD",
            SellableUOM = "LB",
            UomName = "POUND",
            Multiple = 1,
            SubPriceTypeCode = null,
            SubPriceTypeId = null,
            SubPriceTypeDesc = null,
            PercentOff = null,
            TagExpirationDate = new DateTime(2020, 6, 6).Date,
            ModifiedDateUtc = new DateTime(2018, 5, 29).Date,
            StartDate = new DateTime(2018, 7, 29).Date,
            EndDate = null,
        };        

        public PriceModelGpm PriceGpm_TPR_10414_6700760076 = new PriceModelGpm
        {
            Region = "MA",
            GpmId = Guid.Parse("7B78B4E9-FBDC-4404-B3AF-CFBD56481A35"),
            ItemId = 123288,
            ItemTypeCode = "RTL",
            ItemTypeDesc = "Retail Sale",
            ScanCode = "6700760076",
            BusinessUnitId = 10414,
            LocaleName = "Foggy Bottom",
            PriceType = "TPR",
            PriceTypeDesc = "Temporary Price Reduction",
            PriceTypeId = "2",
            Price = 15.05m,
            CurrencyCode = "USD",
            SellableUOM = "LB",
            UomName = "POUND",
            Multiple = 1,
            SubPriceTypeCode = "REG",
            SubPriceTypeId = "1",
            SubPriceTypeDesc = "Regular Price",
            PercentOff = null,
            TagExpirationDate = new DateTime(2020, 7, 7).Date,
            ModifiedDateUtc = new DateTime(2018, 2, 28).Date,
            StartDate = new DateTime(2018, 4, 24).Date,
            EndDate = new DateTime(2019, 12, 24).Date,
        }; 

        public PriceModel PriceNonGpm_REG_10414_999999999 = new PriceModel
        {
            Region = "MA",
            ItemId = 565555,
            ScanCode = "999999999",
            ItemTypeCode = "RTL",
            ItemTypeDesc = "Retail Sale",
            BusinessUnitId = 10414,
            LocaleName = "Foggy Bottom",
            PriceTypeCode = "REG",
            Price = 2.910m,
            PriceTypeDesc = "Regular Price",
            PriceTypeId = "1",
            CurrencyCode = "USD",
            UomCode = "EA",
            UomName = "EACH",
            Multiple = 1,
            SubPriceTypeCode = null,
            SubPriceTypeId = null,
            SubPriceTypeDesc = null,
            PercentOff = null,
            StartDate = new DateTime(2018, 8, 8).Date,
            EndDate = null,
        };
        
        public PriceModel PriceNonGpm_REG_10414_888888888 = new PriceModel
        {
            Region = "MA",
            ItemId = 785555,
            ScanCode = "888888888",
            ItemTypeCode = "RTL",
            ItemTypeDesc = "Retail Sale",
            BusinessUnitId = 10414,
            LocaleName = "Foggy Bottom",
            PriceTypeCode = "REG",
            PriceTypeDesc = "Regular Price",
            PriceTypeId = "1",
            Price = 2.28m,
            CurrencyCode = "USD",
            UomCode = "EA",
            UomName = "EACH",
            Multiple = 1,
            SubPriceTypeCode = null,
            SubPriceTypeId = null,
            SubPriceTypeDesc = null,
            PercentOff = null,
            StartDate = new DateTime(2018, 8, 8).Date,
            EndDate = null,
        };

        public PriceModel PriceNonGpm_TPR_10414_70017007 = new PriceModel
        {
            Region = "MA",
            ItemId = 785555,
            ScanCode = "70017007",
            ItemTypeCode = "RTL",
            ItemTypeDesc = "Retail Sale",
            BusinessUnitId = 10414,
            LocaleName = "Foggy Bottom",
            PriceTypeCode = "TPR",
            PriceTypeDesc = "Temporary Price Reduction",
            PriceTypeId = "1",
            Price = 3.3300m,
            CurrencyCode = "USD",
            UomCode = "EA",
            UomName = "EACH",
            Multiple = 1,
            SubPriceTypeCode = "EDV",
            SubPriceTypeId = "4",
            SubPriceTypeDesc = "Everyday Value",
            PercentOff = null,
            StartDate = new DateTime(2018, 9, 3).Date,
            EndDate = new DateTime(2020, 12, 24).Date,
        };
    }
}
