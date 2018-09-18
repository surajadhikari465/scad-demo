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
    public class MammothPriceBuilderForPreGpmTests
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
        public void MammothPriceBuilder_IsGpmActive_ReturnsExpectedValueForNonGpmRegion()
        {
            //Given
            var region = "MA";
            // When
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                var result = MammothPriceBuilder.IsGpmActive(sqlConnection, region);

                // Then
                Assert.AreEqual(false, result);
            }
        }

        [TestMethod]
        public void MammothPriceBuilder_GetFormattedSqlQueryForNonGpmPrices_ReplacesExpectedTokens()
        {
            //Given
            var region = "XY";
            int maxNumberOfRows = 10;
            string expectedTop = $"SELECT top {maxNumberOfRows}";
            string expectedRegion = $"dbo.Price_{region}";
            // When
            var result = MammothPriceBuilder.GetFormattedSqlQueryForNonGpmPrices(region, maxNumberOfRows);
            // Then
            Assert.IsTrue(result.Contains(expectedTop));
            Assert.IsTrue(result.Contains(expectedRegion));
        }

        [TestMethod]
        public void MammothPriceBuilder_LoadMammothPrices_WhenNonGpm_ReturnsPrices()
        {
            //Given
            string region = "MA";
            int maxNumberOfRows = 10;

            // When
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                var priceResults = MammothPriceBuilder.LoadMammothNonGpmPrices(sqlConnection, region, maxNumberOfRows);

                // Then
                Assert.IsNotNull(priceResults);
                Assert.AreEqual(maxNumberOfRows, priceResults.Count());
            }
        }

        [TestMethod]
        public void MammothPriceBuilder_SendMessagesToEsb_WhenNonGpm_SendsSingleRegMessage()
        {
            //Given
            int maxNumberOfRows = 10;
            bool shouldSaveMessages = false;
            string saveMessageDir = string.Empty;
            string nonReceivingSysName = "non receiving systems";
            var mockEsbProducer = new Mock<IEsbProducer>();

            var nonGpmPriceModels = new List<PriceModel>
            {
                testData.PriceNonGpm_REG_10414_999999999,
            };

            // When
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                MammothPriceBuilder.SendMessagesToEsb(
                   legacyPriceModels: nonGpmPriceModels,
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
        public void MammothPriceBuilder_SendMessagesToEsb_WhenNonGpm_SendsSingleTprMessage()
        {
            //Given
            int maxNumberOfRows = 10;
            bool shouldSaveMessages = false;
            string saveMessageDir = string.Empty;
            string nonReceivingSysName = "non receiving systems";
            var mockEsbProducer = new Mock<IEsbProducer>();

            var nonGpmPriceModels = new List<PriceModel>
            {
                testData.PriceNonGpm_TPR_10414_70017007,
            };

            // When
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                MammothPriceBuilder.SendMessagesToEsb(
                   legacyPriceModels: nonGpmPriceModels,
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
        public void MammothPriceBuilder_SendMessagesToEsb_WhenNonGpm_SendsMultipleRegMessages()
        {
            //Given
            int maxNumberOfRows = 10;
            bool shouldSaveMessages = false;
            string saveMessageDir = string.Empty;
            string nonReceivingSysName = "non receiving systems";
            var mockEsbProducer = new Mock<IEsbProducer>();

            var nonGpmPriceModels = new List<PriceModel>
            {
                testData.PriceNonGpm_REG_10414_999999999,
                testData.PriceNonGpm_REG_10414_888888888
            };

            // When
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                MammothPriceBuilder.SendMessagesToEsb(
                   legacyPriceModels: nonGpmPriceModels,
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
        public void MammothPriceBuilder_SendMessagesToEsb_WhenNonGpm_SendsExpectedMsgPropNonReceivingSys()
        {
            // Given
            int maxNumberOfRows = 10;
            bool shouldSaveMessages = false;
            string saveMessageDir = string.Empty;
            string nonReceivingSysName = "non receiving systems";
            var mockEsbProducer = new Mock<IEsbProducer>();

            var nonGpmPriceModels = new List<PriceModel>
            {
                testData.PriceNonGpm_REG_10414_999999999,
            };

            // When
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                MammothPriceBuilder.SendMessagesToEsb(
                    legacyPriceModels: nonGpmPriceModels,
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
        public void MammothPriceBuilder_SendMessagesToEsb_CallsSendForNonGpmRegMessage_WithExpectedXml()
        {
            // Given
            int maxRows = 10;
            var mockEsbProducer = new Mock<IEsbProducer>();

            var nonGpmPriceModels = new List<PriceModel>
            {
                testData.PriceNonGpm_REG_10414_888888888,
            };
            var expectedMsg = File.ReadAllText("ExpectedNonGpmMessage_REG_10414_888888888.xml");
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
                legacyPriceModels: nonGpmPriceModels,
                esbProducer: mockEsbProducer.Object,
                saveMessages: false,
                saveMessagesDirectory: null,
                nonReceivingSysName: "non receivers",
                maxNumberOfRows: maxRows);

            // Then
            Assert.AreEqual(expectedMsg, actualMsg, "esb xml message");
        }

        [TestMethod]
        public void MammothPriceBuilder_SendMessagesToEsb_CallsSendForNonGpmTprMsg_WithExpectedXml()
        {
            // Given
            int maxRows = 10;
            var mockEsbProducer = new Mock<IEsbProducer>();

            // send an non-authorized item/locale which should create a delete message
            var nonGpmPriceModels = new List<PriceModel>
            {
                testData.PriceNonGpm_TPR_10414_70017007,
            };
            var expectedMsg = File.ReadAllText("ExpectedNonGpmMessage_TPR_10414_70017007.xml");
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
                legacyPriceModels: nonGpmPriceModels,
                esbProducer: mockEsbProducer.Object,
                saveMessages: false,
                saveMessagesDirectory: null,
                nonReceivingSysName: "non receivers",
                maxNumberOfRows: maxRows);

            // Then
            Assert.AreEqual(expectedMsg, actualMsg, "esb xml message");
        }

        [TestMethod]
        public void MammothPriceBuilder_SendMessagesToEsb_CallsSendForNonGpmRegMultiplePrices_WithExpectedXml()
        {
            // Given
            int maxRows = 10;
            var mockEsbProducer = new Mock<IEsbProducer>();

            var nonGpmPriceModels = new List<PriceModel>
            {
                testData.PriceNonGpm_REG_10414_999999999,
                testData.PriceNonGpm_REG_10414_888888888
            };
            var expectedMsg = File.ReadAllText(
                "ExpectedNonGpmMessage_REG_10414_888888888and999999999.xml");
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
                legacyPriceModels: nonGpmPriceModels,
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
        public void MammothPriceBuilder_SendMessagesToEsb_MaxRecordsLimitsDataForNonGpmPrices()
        {
            // Given
            int maxNumberOfRows = 1;
            bool shouldSaveMessages = false;
            string saveMessageDir = string.Empty;
            string nonReceivingSysName = "non receiving systems";
            var mockEsbProducer = new Mock<IEsbProducer>();

            var nonGpmPriceModels = new List<PriceModel>
            {
                testData.PriceNonGpm_REG_10414_999999999,
                testData.PriceNonGpm_REG_10414_888888888
            };

            // When
            MammothPriceBuilder.SendMessagesToEsb(
                legacyPriceModels: nonGpmPriceModels,
                esbProducer: mockEsbProducer.Object,
                saveMessages: shouldSaveMessages,
                saveMessagesDirectory: saveMessageDir,
                nonReceivingSysName: nonReceivingSysName,
                maxNumberOfRows: maxNumberOfRows);

            // Then
            Assert.AreEqual(1, MammothPriceBuilder.NumberOfMessagesSent);
            Assert.AreEqual(1, MammothPriceBuilder.NumberOfRecordsSent);
        }

        [TestMethod]
        public void MammothPriceBuilder_SendMessagesToEsb_MaxRecordsOfZeroMeansAllForNonGpmPrices()
        {
            // Given
            int maxRows = 0;
            var mockEsbProducer = new Mock<IEsbProducer>();


            var nonGpmPriceModels = new List<PriceModel>
            {
                testData.PriceNonGpm_REG_10414_999999999,
                testData.PriceNonGpm_REG_10414_888888888
            };

            var expectedNonReceivingSystems = "non receivers";

            // When
            MammothPriceBuilder.SendMessagesToEsb(
                legacyPriceModels: nonGpmPriceModels,
                esbProducer: mockEsbProducer.Object,
                saveMessages: false,
                saveMessagesDirectory: null,
                nonReceivingSysName: expectedNonReceivingSystems,
                maxNumberOfRows: maxRows);

            // Then
            Assert.AreEqual(1, MammothPriceBuilder.NumberOfMessagesSent);
            Assert.AreEqual(2, MammothPriceBuilder.NumberOfRecordsSent);
        }
    }
}
