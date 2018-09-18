using Icon.Esb.Producer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace AmazonLoad.MammothPrice.Tests
{
    [TestClass]
    public class MammothPriceBuilderForGpmTests
    {
        string connectionString = string.Empty;
        TestData testData = new TestData();

        [TestInitialize]
        public void Initialize()
        {
            connectionString = ConfigurationManager.ConnectionStrings["MammothContext"].ConnectionString;

            MammothPriceBuilder.NumberOfMessagesSent = 0;
            MammothPriceBuilder.NumberOfRecordsSent = 0;
        }

        [TestMethod]
        public void MammothPriceBuilder_IsGpmActive_ReturnsExpectedValueForGpmRegion()
        {
            //Given
            var region = "FL";
            // When
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                var result = MammothPriceBuilder.IsGpmActive(sqlConnection, region);
                // Then
                Assert.AreEqual(true, result);
            }
        }

        [TestMethod]
        public void MammothPriceBuilder_GetFormattedSqlQueryFoQueryRegionGpmStatus_ReplacesExpectedTokens()
        {
            //Given
            var region = "MA";
            int maxNumberOfRows = 50;
            string expectedTop = $"SELECT top {maxNumberOfRows}";
            string expectedRegion = $"gpm.Price_{region}";
            // When
            var result = MammothPriceBuilder.GetFormattedSqlQueryForGpmPrices(region, maxNumberOfRows);
            // Then
            Assert.IsTrue(result.Contains(expectedTop));
            Assert.IsTrue(result.Contains(expectedRegion));
        }

        [TestMethod]
        public void MammothPriceBuilder_GetFormattedSqlQueryForGpmPrices_ReplacesExpectedTokens()
        {
            //Given
            var region = "CA";
            string expectedRegion = $"Region = '{region}'";
            // When
            var result = MammothPriceBuilder.GetFormattedSqlQueryFoQueryRegionGpmStatus(region);
            // Then
            Assert.IsTrue(result.Contains(expectedRegion));
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
                var priceResults = MammothPriceBuilder.LoadMammothGpmPrices(sqlConnection, region, maxNumberOfRows);
                
                // Then
                Assert.IsNotNull(priceResults);
                Assert.AreEqual(maxNumberOfRows, priceResults.Count());
            }
        }

        [TestMethod]
        public void MammothPriceBuilder_SendMessagesToEsb_WhenGpmActive_SendsSingleMessage()
        {
            //Given
            int maxNumberOfRows = 10;
            bool shouldSaveMessages = false;
            string saveMessageDir = string.Empty;
            string nonReceivingSysName = string.Empty;
            var mockEsbProducer = new Mock<IEsbProducer>();

            var gpmPriceModels = new List<PriceModelGpm>
            {
                testData.PriceGpm_REG_10414_7777777777,
            };

            // When
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                MammothPriceBuilder.SendMessagesToEsb(
                   gpmPriceModels: gpmPriceModels.AsEnumerable(),
                   esbProducer: mockEsbProducer.Object,
                   saveMessages: shouldSaveMessages,
                   saveMessagesDirectory: saveMessageDir,
                   nonReceivingSysName: nonReceivingSysName,
                   maxNumberOfRows: maxNumberOfRows);

                // Then
                Assert.AreEqual(1, MammothPriceBuilder.NumberOfRecordsSent);
                Assert.AreEqual(1, MammothPriceBuilder.NumberOfMessagesSent);
                mockEsbProducer.Verify(p =>
                    p.Send(
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<Dictionary<string, string>>()
                    ), Times.Once);
            }
        }

        [TestMethod]
        public void MammothPriceBuilder_SendMessagesToEsb_WhenGpmActive_SendsMultipleMessages()
        {
            //Given
            int maxNumberOfRows = 10;
            bool shouldSaveMessages = false;
            string saveMessageDir = string.Empty;
            string nonReceivingSysName = "non receiving systems";
            var mockEsbProducer = new Mock<IEsbProducer>();

            var gpmPriceModels = new List<PriceModelGpm>
            {
                testData.PriceGpm_REG_10414_7777777777,
                testData.PriceGpm_REG_10414_666666666,
            };


            // When
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                MammothPriceBuilder.SendMessagesToEsb(
                   gpmPriceModels: gpmPriceModels,
                   esbProducer: mockEsbProducer.Object,
                   saveMessages: shouldSaveMessages,
                   saveMessagesDirectory: saveMessageDir,
                   nonReceivingSysName: nonReceivingSysName,
                   maxNumberOfRows: maxNumberOfRows);

                // Then
                Assert.AreEqual(2, MammothPriceBuilder.NumberOfRecordsSent);
                Assert.AreEqual(1, MammothPriceBuilder.NumberOfMessagesSent);
                mockEsbProducer.Verify(p =>
                    p.Send(
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<Dictionary<string, string>>()
                    ), Times.Once);
            }
        }

        [TestMethod]
        public void MammothPriceBuilder_SendMessagesToEsb_SendsExpectedMsgPropNonReceivingSys()
        {
            // Given
            int maxNumberOfRows = 10;
            bool shouldSaveMessages = false;
            string saveMessageDir = string.Empty;
            string nonReceivingSysName = "non receiving systems";
            var mockEsbProducer = new Mock<IEsbProducer>();

            var gpmPriceModels = new List<PriceModelGpm>
            {
                testData.PriceGpm_REG_10414_7777777777,
            };

            // When
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                MammothPriceBuilder.SendMessagesToEsb(
                    gpmPriceModels: gpmPriceModels,
                    esbProducer: mockEsbProducer.Object,
                    saveMessages: shouldSaveMessages,
                    saveMessagesDirectory: saveMessageDir,
                    nonReceivingSysName: nonReceivingSysName,
                    maxNumberOfRows: maxNumberOfRows);

                // Then
                mockEsbProducer.Verify(p =>
                    p.Send(
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.Is<Dictionary<string, string>>(dict => dict["nonReceivingSysName"] == nonReceivingSysName)
                    ), Times.Once);
            }
        }

        [TestMethod]
        public void MammothPriceBuilder_SendMessagesToEsb_CallsSendForGpmRegMessage_WithExpectedXml()
        {
            // Given
            int maxRows = 10;
            var mockEsbProducer = new Mock<IEsbProducer>();

            var gpmPriceModels = new List<PriceModelGpm>
            {
                testData.PriceGpm_REG_10414_7777777777,
            };
            string actualMsg = string.Empty;
            var expectedMsg = File.ReadAllText("ExpectedGpmTestMessage_REG_10414_7777777777.xml");

            mockEsbProducer.Setup(p => p.Send(
                       It.IsAny<string>(),
                       It.IsAny<string>(),
                       It.IsAny<Dictionary<string, string>>()))
                .Callback<string, string, Dictionary<string, string>>(
                    ((message, messageId, messageProperties) =>
                    {
                        actualMsg = message;
                    }));

            // When
            MammothPriceBuilder.SendMessagesToEsb(
                gpmPriceModels: gpmPriceModels,
                esbProducer: mockEsbProducer.Object,
                saveMessages: false,
                saveMessagesDirectory: null,
                nonReceivingSysName: "non receivers",
                maxNumberOfRows: maxRows,
                sendToEsb: true);

            // Then
            Assert.AreEqual(expectedMsg, actualMsg, "esb xml message");
        }

        [TestMethod]
        public void MammothPriceBuilder_SendMessagesToEsb_CallsSendForGpmTprMsg_WithExpectedXml()
        {
            // Given
            int maxRows = 10;
            var mockEsbProducer = new Mock<IEsbProducer>();

            // send an non-authorized item/locale which should create a delete message
            var gpmPriceModels = new List<PriceModelGpm>
            {
                testData.PriceGpm_TPR_10414_6700760076,
            };
            var expectedMsg = File.ReadAllText("ExpectedGpmTestMessage_TPR_10414_6700760076.xml");
            string actualMsg = string.Empty;
            mockEsbProducer.Setup(p => p.Send(
                       It.IsAny<string>(),
                       It.IsAny<string>(),
                       It.IsAny<Dictionary<string, string>>()))
                .Callback<string, string, Dictionary<string, string>>(
                    ((message, messageId, messageProperties) =>
                    {
                        actualMsg = message;
                    }));

            // When
            MammothPriceBuilder.SendMessagesToEsb(
                gpmPriceModels: gpmPriceModels,
                esbProducer: mockEsbProducer.Object,
                saveMessages: false,
                saveMessagesDirectory: null,
                nonReceivingSysName: "non receivers",
                maxNumberOfRows: maxRows);

            // Then
            Assert.AreEqual(expectedMsg, actualMsg, "esb xml message");
        }

        [TestMethod]
        public void MammothPriceBuilder_SendMessagesToEsb_CallsSendForGpmMultiplePrices_WithExpectedXml()
        {
            // Given
            int maxRows = 10;
            var mockEsbProducer = new Mock<IEsbProducer>();

            var gpmPriceModels = new List<PriceModelGpm>
            {
                testData.PriceGpm_REG_10414_7777777777,
                testData.PriceGpm_REG_10414_666666666
            };
            var expectedMsg = File.ReadAllText("ExpectedGpmTestMessage_REG_10414_7777777777and666666666.xml");
            string actualMsg = string.Empty;
            mockEsbProducer.Setup(p => p.Send(
                       It.IsAny<string>(),
                       It.IsAny<string>(),
                       It.IsAny<Dictionary<string, string>>()))
                .Callback<string, string, Dictionary<string, string>>(
                    ((message, messageId, messageProperties) =>
                    {
                        actualMsg = message;
                    }));

            // When
            MammothPriceBuilder.SendMessagesToEsb(
                gpmPriceModels: gpmPriceModels,
                esbProducer: mockEsbProducer.Object,
                saveMessages: false,
                saveMessagesDirectory: null,
                nonReceivingSysName: "non receivers",
                maxNumberOfRows: maxRows);

            // Then
            Assert.AreEqual(1, MammothPriceBuilder.NumberOfMessagesSent);
            Assert.AreEqual(2, MammothPriceBuilder.NumberOfRecordsSent);
            Assert.AreEqual(expectedMsg, actualMsg, "esb xml message");
        }

        [TestMethod]
        public void MammothPriceBuilder_SendMessagesToEsb_MaxRecordsLimitsDataForGpmPrices()
        {
            // Given
            int maxRows = 1;
            var mockEsbProducer = new Mock<IEsbProducer>();

            var gpmPriceModels = new List<PriceModelGpm>
            {
                testData.PriceGpm_REG_10414_7777777777,
                testData.PriceGpm_REG_10414_666666666
            };

            var expectedNonReceivingSystems = "non receivers";

            // When
            MammothPriceBuilder.SendMessagesToEsb(
                gpmPriceModels: gpmPriceModels,
                esbProducer: mockEsbProducer.Object,
                saveMessages: false,
                saveMessagesDirectory: null,
                nonReceivingSysName: expectedNonReceivingSystems,
                maxNumberOfRows: maxRows);

            // Then
            Assert.AreEqual(1, MammothPriceBuilder.NumberOfMessagesSent);
            Assert.AreEqual(1, MammothPriceBuilder.NumberOfRecordsSent);
        }

        [TestMethod]
        public void MammothPriceBuilder_SendMessagesToEsb_MaxRecordsOfZeroMeansAllForGpmPrices()
        {
            // Given
            int maxRows = 0;
            var mockEsbProducer = new Mock<IEsbProducer>();

            var gpmPriceModels = new List<PriceModelGpm>
            {
                testData.PriceGpm_REG_10414_7777777777,
                testData.PriceGpm_REG_10414_666666666,
                testData.PriceGpm_TPR_10414_6700760076
            };

            var expectedNonReceivingSystems = "non receivers";

            // When
            MammothPriceBuilder.SendMessagesToEsb(
                gpmPriceModels: gpmPriceModels,
                esbProducer: mockEsbProducer.Object,
                saveMessages: false,
                saveMessagesDirectory: null,
                nonReceivingSysName: expectedNonReceivingSystems,
                maxNumberOfRows: maxRows);

            // Then
            Assert.AreEqual(1, MammothPriceBuilder.NumberOfMessagesSent);
            Assert.AreEqual(3, MammothPriceBuilder.NumberOfRecordsSent);
        }
    }
}
