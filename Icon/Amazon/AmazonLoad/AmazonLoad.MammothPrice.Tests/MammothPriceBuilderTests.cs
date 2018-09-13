using Icon.Esb.Producer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace AmazonLoad.MammothPrice.Test
{
    [TestClass]
    public class MammothPricebuilderTests
    {
        string connectionString = string.Empty;

        [TestInitialize]
        public void Initialize()
        {
            connectionString = ConfigurationManager.ConnectionStrings["MammothContext"].ConnectionString;
        }

        [TestMethod]
        public void MammothPriceBuilder_IsGpmActive_ReturnsExpectedValueForGpmRegion()
        {
            //Given
            var region = "FL";
            // When
            var result = MammothPriceBuilder.IsGpmActive(connectionString, region);
            // Then
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void MammothPriceBuilder_IsGpmActive_ReturnsExpectedValueForNonGpmRegion()
        {
            //Given
            var region = "MA";
            // When
            var result = MammothPriceBuilder.IsGpmActive(connectionString, region);
            // Then
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void MammothPriceBuilder_GetFormattedSqlQueryForGpmPrices_ReturnsExpectedValidSql()
        {
            //Given
            var region = "FL";
            int maxNumberOfRows = 10;
            string expectedSqlQuery = @"SELECT top 10
                    p.ItemID            as ItemId,
	                t.ItemTypeCode		as ItemTypeCode,
	                t.ItemTypeDesc		as ItemTypeDesc,
                    p.BusinessUnitID    as BusinessUnitId,
	                l.StoreName			as LocaleName,
	                i.ScanCode			as ScanCode,
                    p.SellableUOM       as UomCode,
                    p.CurrencyCode      as CurrencyCode,
	                p.PriceType         as PriceType,
                    p.Price				as Price,
	                p.Multiple			as Multiple,
	                p.StartDate			as StartDate,
	                p.EndDate			as EndDate,
                    p.GpmID			    as GpmId,
                    p.PriceTypeAttribute as PriceTypeAttribute,
                    p.PercentOff        as PercentOff
	            FROM gpm.Price_FL p
	                JOIN dbo.Locales_FL   l	on	p.BusinessUnitId	= l.BusinessUnitID
	                JOIN dbo.Items		        i	on	p.ItemID			= i.ItemID
	                JOIN dbo.ItemTypes	        t	on	i.ItemTypeID		= t.ItemTypeID
                    JOIN dbo.Currency           c   on  p.CurrencyCode      = c.CurrencyCode
                ORDER BY p.BusinessUnitId";
            // When
            var result = MammothPriceBuilder.GetFormattedSqlQueryForGpmPrices(region, maxNumberOfRows);
            // Then
            Assert.AreEqual(expectedSqlQuery, result);
        }

        [TestMethod]
        public void MammothPriceBuilder_GetFormattedSqlQueryForNonGpmPrices_ReturnsExpectedValidSql()
        {
            //Given
            var region = "MA";
            int maxNumberOfRows = 10;
            string expectedSqlQuery = @"SELECT top 10
	                            i.ItemID			as ItemId,
	                            t.ItemTypeCode		as ItemTypeCode,
	                            t.ItemTypeDesc		as ItemTypeDesc,
	                            l.BusinessUnitID	as BusinessUnitId,
	                            l.StoreName			as LocaleName,
	                            i.ScanCode			as ScanCode,
	                            p.PriceUom			as UomCode,
	                            c.CurrencyCode		as CurrencyCode,
                                CASE WHEN p.PriceType <> 'REG' THEN 'TPR' ELSE 'REG' END as PriceTypeCode,
	                            CASE WHEN p.PriceType <> 'REG' THEN p.PriceType END as SubPriceTypeCode,
	                            p.Price				as Price,
	                            p.Multiple			as Multiple,
	                            p.StartDate			as StartDate,
	                            p.EndDate			as EndDate
                            FROM
	                            dbo.Price_MA          p
	                            JOIN dbo.Locales_MA   l	on	p.BusinessUnitId	= l.BusinessUnitID
	                            JOIN dbo.Items		        i	on	p.ItemID			= i.ItemID
	                            JOIN dbo.ItemTypes	        t	on	i.ItemTypeID		= t.ItemTypeID
                                JOIN dbo.Currency           c   on  p.CurrencyID        = c.CurrencyID
                            ORDER BY p.BusinessUnitId";
            // When
            var result = MammothPriceBuilder.GetFormattedSqlQueryForNonGpmPrices(region, maxNumberOfRows);
            // Then
            Assert.AreEqual(expectedSqlQuery, result);
        }

        [TestMethod]
        public void MammothPriceBuilder_LoadMammothPrices_WhenGpmActive_ReturnsGpmPrices()
        {
            //Given
            string region = "FL";
            int maxNumberOfRows = 10;

            // When
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                var loadResults = MammothPriceBuilder.LoadMammothGpmPrices(sqlConnection, region, maxNumberOfRows);
                var prices = (loadResults as IEnumerable<PriceModelGpm>).ToList();

                // Then
                Assert.IsNotNull(prices);
                Assert.AreEqual(maxNumberOfRows, prices.Count);
            }
        }

        [TestMethod]
        public void MammothPriceBuilder_LoadMammothPrices_WhenGpmInactive_ReturnsPrices()
        {
            //Given
            string region = "MA";
            int maxNumberOfRows = 10;

            // When
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                var loadResults = MammothPriceBuilder.LoadMammothNonGpmPrices(sqlConnection, region, maxNumberOfRows);
                var prices = (loadResults as IEnumerable<PriceModel>).ToList();

                // Then
                Assert.IsNotNull(prices);
                Assert.AreEqual(maxNumberOfRows, prices.Count);
            }
        }

        [TestMethod]
        public void MammothPriceBuilder_SendMessagesToEsb_WhenGpmActive_CallsSendMessageToEsb()
        {
            //Given
            bool shouldSaveMessages = false;
            string saveMessageDir = string.Empty;
            string nonReceivingSysName = string.Empty;
            var mockEsbProducer = new Mock<IEsbProducer>();

            var gpmPriceModels = new List<PriceModelGpm>
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
                    LocaleName = "XYZ",
                    BusinessUnitId = 10234,
                    ItemTypeDesc = "ice Item Description",
                    ItemTypeCode = null,
                    ItemId = 565555,
                    ScanCode = "999999999",
                    GpmId = null,
                    PercentOff = null,
                    PriceTypeAttribute = null
                },
            };

            // When
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                var result = MammothPriceBuilder.SendMessagesToEsb(gpmPriceModels, mockEsbProducer.Object, shouldSaveMessages, saveMessageDir, nonReceivingSysName);

                // Then
                Assert.AreEqual(result.NumberOfRecordsSent, 1);
                Assert.AreEqual(result.NumberOfMessagesSent, 1);
                mockEsbProducer.Verify(p =>
                    p.Send(
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<Dictionary<string, string>>()
                    ), Times.Once);
            }
        }

        [TestMethod]
        public void MammothPriceBuilder_SendMessagesToEsb_WhenGpmInactive_CallsSendMessageToEsb()
        {
            //Given
            bool shouldSaveMessages = false;
            string saveMessageDir = string.Empty;
            string nonReceivingSysName = string.Empty;
            var mockEsbProducer = new Mock<IEsbProducer>();

            var nonGpmPriceModels = new List<PriceModel>
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
                    LocaleName = "ABC",
                    BusinessUnitId = 10234,
                    ItemTypeDesc = "Nice Item Description",
                    ItemTypeCode = null,
                    ItemId = 565555,
                    ScanCode = "999999999",
                    GpmId = null,
                    PercentOff = null,
                    PriceTypeAttribute = null
                },
            };

            // When
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                var result = MammothPriceBuilder.SendMessagesToEsb(nonGpmPriceModels, mockEsbProducer.Object, shouldSaveMessages, saveMessageDir, nonReceivingSysName);

                // Then
                Assert.AreEqual(result.NumberOfRecordsSent, 1);
                Assert.AreEqual(result.NumberOfMessagesSent, 1);
                mockEsbProducer.Verify(p =>
                    p.Send(
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<Dictionary<string, string>>()
                    ), Times.Once);
            }
        }
    }
}
