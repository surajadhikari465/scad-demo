using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Icon.Esb.Producer;
using System.IO;

namespace AmazonLoad.IconItemLocale.Tests
{
    [TestClass]
    public class IconItemLocaleBuilderTests
    {
        TestData testData = new TestData();

        string region = "MA";
        Mock<IEsbProducer> mockEsbProducer = new Mock<IEsbProducer>();
        string actualXmlMsg = string.Empty;

        string TestRegion
        {
            get { return region; }
            set { region = value; }
        }

        string iconConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["Icon"].ConnectionString;
            }
        }

        string irmaConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["ItemCatalog_" + TestRegion].ConnectionString;
            }
        }

        [TestInitialize]
        public void TestInitialize()
        {
            var psgMapper = new FakeIconItemLocalePsgMapper(testData.TestPsgs);

            MessageBuilderForIconItemLocale.PsgMapper = psgMapper;
            IconItemLocaleBuilder.PsgMapper = psgMapper;

            IconItemLocaleBuilder.NumberOfMessagesSent = 0;
            IconItemLocaleBuilder.NumberOfRecordsSent = 0;
        }

        [TestMethod]
        public void IconItemLocaleBuilder_LoadIconStoreData_ReturnsExpectedModelCount()
        {
            // Given
            TestRegion = "MA";
            int expectedValidStoreCount = 60;

            // When
            var iconStoreData = IconItemLocaleBuilder.LoadIconStoreData(TestRegion, iconConnectionString);

            // Then
            Assert.IsNotNull(iconStoreData);
            Assert.AreEqual(expectedValidStoreCount, iconStoreData.Count);
        }

        [TestMethod]
        public void IconItemLocaleBuilder_LoadIrmaItemLocales_ReturnsExpectedModelCount()
        {
            // Given
            TestRegion = "MA";
            int maxNumberOfRows = 10;
            var store = testData.GetTestStore(TestRegion);

            // When 
            using (SqlConnection irmaSqlConnection = new SqlConnection(irmaConnectionString))
            {
                var irmaItemLocaleData = IconItemLocaleBuilder.LoadIrmaItemLocales(irmaSqlConnection, store.RegionCode, store.BusinessUnit, maxNumberOfRows).ToList();

                // Then
                Assert.IsNotNull(irmaItemLocaleData);
                Assert.AreEqual(maxNumberOfRows, irmaItemLocaleData.Count());
            }
        }

        [TestMethod]
        public void IconItemLocaleBuilder_LoadItemLocalesForWormhole_ReturnsExpectedModelCount()
        {
            // Given
            TestRegion = "MA";
            var store = testData.GetTestStore(TestRegion);
            int maxNumberOfRows = 10;

            // When
            using (SqlConnection irmaSqlConnection = new SqlConnection(irmaConnectionString))
            {
                var itemLocaleData = IconItemLocaleBuilder.LoadItemLocalesForWormhole(irmaSqlConnection, store, maxNumberOfRows);

                // Then
                Assert.IsNotNull(itemLocaleData);
                Assert.AreEqual(maxNumberOfRows, itemLocaleData.Count());
            }
        }

        [TestMethod]
        public void IconItemLocaleBuilder_LoadItemLocalesForWormhole_ReturnsModelsWithIconAndIrmaData()
        {
            // Given
            TestRegion = "MA";
            int maxNumberOfRows = 10;
            var store = testData.GetTestStore(TestRegion);

            // When
            using (SqlConnection irmaSqlConnection = new SqlConnection(irmaConnectionString))
            {
                var itemLocaleData = IconItemLocaleBuilder.LoadItemLocalesForWormhole(irmaSqlConnection,  store, maxNumberOfRows);

                // Then
                var itemLocaleForWormhole = itemLocaleData.ElementAt(4);
                Assert.AreEqual(itemLocaleForWormhole.BusinessUnit, store.BusinessUnit);
                Assert.AreEqual(itemLocaleForWormhole.RegionCode, TestRegion);
                // check that an IRMA property loaded from the db into the object
                Assert.IsFalse(string.IsNullOrWhiteSpace(itemLocaleForWormhole.ScanCode));
                // check that an iCON property loaded from the db into the object
                Assert.IsFalse(string.IsNullOrWhiteSpace(itemLocaleForWormhole.LocaleName));
            }
        }

        [TestMethod]
        public void IconItemLocaleBuilder_GetFormattedSqlForIconStoreQuery_ReplacesRegion()
        {
            // Given
            string region = "XY";
            string expectedRegionSubstitution = $"regionAbbr.traitValue = '{region}'";

            // When
            var result = IconItemLocaleBuilder.GetFormattedSqlForIconStoreQuery(region);

            // Then
            Assert.IsTrue(result.Contains(expectedRegionSubstitution));
        }

        [TestMethod]
        public void IconItemLocaleBuilder_GetFormattedSqlForIrmaItemLocaleQuery_ReplacesExpectedStrings()
        {
            // Given
            string region = "XY";
            string businessUnit = "12345";
            int maxNumberOfRows = 10;
            string expectedTop = $"SELECT top {maxNumberOfRows}";
            string expectedRegion = $"'{region}' as RegionCode";
            string expectedBusinessUnit = $"s.BusinessUnit_ID = {businessUnit}";

            // When
            var result = IconItemLocaleBuilder.GetFormattedSqlForIrmaItemLocaleQuery(region, businessUnit, maxNumberOfRows);

            // Then
            Assert.IsTrue(result.Contains(expectedTop));
            Assert.IsTrue(result.Contains(expectedRegion));
            Assert.IsTrue(result.Contains(expectedBusinessUnit));
        }

        [TestMethod]
        public void IconItemLocaleBuilder_SendMessagesToEsb_SingleMessageCallsSend()
        {
            // Given
            int maxNumberOfRows = 10;
            var expectedNonReceivingSystems = "non receivers";
            string transactionType = "Item/Locale";

            var itemLocaleModels = new List<ItemLocaleModelForWormhole>
            {
                testData.ItemLocale_BU10042_UPC
            };

            // When
            IconItemLocaleBuilder.SendMessagesToEsb(itemLocaleModels, mockEsbProducer.Object,
                false, null, expectedNonReceivingSystems, maxNumberOfRows, transactionType);

            // Then
            Assert.AreEqual(1, IconItemLocaleBuilder.NumberOfMessagesSent);
            Assert.AreEqual(1, IconItemLocaleBuilder.NumberOfRecordsSent);
            mockEsbProducer.Verify(p => p.Send(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<Dictionary<string, string>>()),
            Times.Once);
        }

        [TestMethod]
        public void IconItemLocaleBuilder_SendMessagesToEsb_SendsExpectedMsgPropNonReceivingSys()
        {
            // Given
            int maxNumberOfRows = 10;
            var expectedNonReceivingSystems = "non receivers";
            string transactionType = "Item/Locale";

            var itemLocaleModels = new List<ItemLocaleModelForWormhole>
            {
                testData.ItemLocale_BU10042_UPC
            };

            // When
            IconItemLocaleBuilder.SendMessagesToEsb(itemLocaleModels, mockEsbProducer.Object,
                false, null, expectedNonReceivingSystems, maxNumberOfRows, transactionType);

            // Then
            mockEsbProducer.Verify(p => p.Send(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.Is<Dictionary<string, string>>(dict => dict["nonReceivingSysName"] == expectedNonReceivingSystems)),
            Times.Once);
        }

        [TestMethod]
        public void IconItemLocaleBuilder_SendMessagesToEsb_SendsExpectedMsgPropTransactionType()
        {
            // Given
            int maxNumberOfRows = 10;
            var expectedNonReceivingSystems = "non receivers";
            string transactionType = "Item/Locale";

            var itemLocaleModels = new List<ItemLocaleModelForWormhole>
            {
                testData.ItemLocale_BU10042_UPC
            };

            // When
            IconItemLocaleBuilder.SendMessagesToEsb(itemLocaleModels, mockEsbProducer.Object,
                false, null, expectedNonReceivingSystems, maxNumberOfRows, transactionType);

            // Then
            mockEsbProducer.Verify(p => p.Send(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.Is<Dictionary<string, string>>(dict => dict["TransactionType"] == transactionType)),
            Times.Once);
        }

        [TestMethod]
        public void IconItemLocaleBuilder_SendMessagesToEsb_ProducesExpectedAddMessageForUPC()
        {
            // Given
            int maxNumberOfRows = 10;
            string transactionType = "Item/Locale";
            var expectedMsg = File.ReadAllText("ExpectedTestMessage_10042_UPC.xml");
            var itemLocaleModels = new List<ItemLocaleModelForWormhole>
            {
                testData.ItemLocale_BU10042_UPC
            };

            mockEsbProducer.Setup(p => p.Send(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<Dictionary<string, string>>()))
            .Callback<string, string, Dictionary<string, string>>(
                ((message, messageId, messageProperties) => { actualXmlMsg = message; }));

            // When
            IconItemLocaleBuilder.SendMessagesToEsb(itemLocaleModels, mockEsbProducer.Object,
                false, null, "non receivers", maxNumberOfRows, transactionType);

            // Then
            Assert.AreEqual(expectedMsg, actualXmlMsg, "esb xml message");
        }

        [TestMethod]
        public void IconItemLocaleBuilder_SendMessagesToEsb_ProducesExpectedAddMessageForUPCWithLinkedItem()
        {
            // Given
            int maxNumberOfRows = 10;
            string transactionType = "Item/Locale";
            var expectedMsg = File.ReadAllText("ExpectedTestMessage_10042_UPC_WithLinkedItem.xml");
            var itemLocaleModels = new List<ItemLocaleModelForWormhole>
            {
                testData.ItemLocale_BU10042_UPC_WithLinkedItem
            };

            mockEsbProducer.Setup(p => p.Send(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<Dictionary<string, string>>()))
            .Callback<string, string, Dictionary<string, string>>(
                ((message, messageId, messageProperties) => { actualXmlMsg = message; }));

            // When
            IconItemLocaleBuilder.SendMessagesToEsb(itemLocaleModels, mockEsbProducer.Object,
                false, null, "non receivers", maxNumberOfRows, transactionType);

            // Then
            Assert.AreEqual(expectedMsg, actualXmlMsg, "esb xml message");
        }

        [TestMethod]
        public void IconItemLocaleBuilder_SendMessagesToEsb_ProducesExpectedDeleteMsgForNonAuthorizedUPC()
        {
            // Given
            int maxNumberOfRows = 10;
            string transactionType = "Item/Locale";
            var expectedMsg = File.ReadAllText("ExpectedTestMessage_10048_UPC_NonAuthorized.xml");
            // send an non-authorized item/locale which should create a delete message
            var itemLocaleModels = new List<ItemLocaleModelForWormhole>
            {
                testData.ItemLocale_BU10048_UPC_NonAuthorized
            };

            mockEsbProducer.Setup(p => p.Send(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<Dictionary<string, string>>()))
            .Callback<string, string, Dictionary<string, string>>(
                ((message, messageId, messageProperties) => { actualXmlMsg = message; }));

            // When
            IconItemLocaleBuilder.SendMessagesToEsb(itemLocaleModels, mockEsbProducer.Object,
                false, null, "non receivers", maxNumberOfRows, transactionType);

            // Then
            Assert.AreEqual(expectedMsg, actualXmlMsg, "esb xml message");
        }

        [TestMethod]
        public void IconItemLocaleBuilder_SendMessagesToEsb_ProducesExpectedAddMessageForPosPlu()
        {
            // Given
            int maxNumberOfRows = 10;
            string transactionType = "Item/Locale";
            var expectedMsg = File.ReadAllText("ExpectedTestMessage_10042_PosPlu.xml");
            var itemLocaleModels = new List<ItemLocaleModelForWormhole>
            {
                testData.ItemLocale_BU10042_PosPlu
            };

            mockEsbProducer.Setup(p => p.Send(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<Dictionary<string, string>>()))
            .Callback<string, string, Dictionary<string, string>>(
                ((message, messageId, messageProperties) => { actualXmlMsg = message; }));

            // When
            IconItemLocaleBuilder.SendMessagesToEsb(itemLocaleModels, mockEsbProducer.Object,
                false, null, "non receivers", maxNumberOfRows, transactionType);

            // Then
            Assert.AreEqual(expectedMsg, actualXmlMsg, "esb xml message");
        }

        [TestMethod]
        public void IconItemLocaleBuilder_SendMessagesToEsb_ProducesExpectedAddMessageForScalePlu()
        {
            // Given
            int maxNumberOfRows = 10;
            string transactionType = "Item/Locale";
            var expectedMsg = File.ReadAllText("ExpectedTestMessage_10042_ScalePlu.xml");
            var itemLocaleModels = new List<ItemLocaleModelForWormhole>
            {
                testData.ItemLocale_BU10042_ScalePlu
            };

            mockEsbProducer.Setup(p => p.Send(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<Dictionary<string, string>>()))
            .Callback<string, string, Dictionary<string, string>>(
                ((message, messageId, messageProperties) => { actualXmlMsg = message; }));

            // When
            IconItemLocaleBuilder.SendMessagesToEsb(itemLocaleModels, mockEsbProducer.Object,
                false, null, "non receivers", maxNumberOfRows, transactionType);

            // Then
            Assert.AreEqual(expectedMsg, actualXmlMsg, "esb xml message");
        }

        [TestMethod]
        public void IconItemLocaleBuilder_SendMessagesToEsb_ProducesExpectedAddMessageForMultiple()
        {
            // Given
            int maxNumberOfRows = 10;
            string transactionType = "Item/Locale";
            var expectedMsg = File.ReadAllText("ExpectedTestMessage_10042_MultipleItems.xml");
            var itemLocaleModels = new List<ItemLocaleModelForWormhole> 
            {
                testData.ItemLocale_BU10042_UPC,
                testData.ItemLocale_BU10042_PosPlu,
                testData.ItemLocale_BU10042_ScalePlu,
                testData.ItemLocale_BU10042_UPC_WithLinkedItem
            };

            mockEsbProducer.Setup(p => p.Send(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<Dictionary<string, string>>()))
            .Callback<string, string, Dictionary<string, string>>(
                ((message, messageId, messageProperties) => { actualXmlMsg = message; }));
            // When
            IconItemLocaleBuilder.SendMessagesToEsb(itemLocaleModels, mockEsbProducer.Object,
                false, null, "non receivers", maxNumberOfRows, transactionType);

            // Then
            Assert.AreEqual(1, IconItemLocaleBuilder.NumberOfMessagesSent);
            Assert.AreEqual(4, IconItemLocaleBuilder.NumberOfRecordsSent);
            Assert.AreEqual(expectedMsg, actualXmlMsg, "esb xml message");
        }
        
        [TestMethod]
        public void IconItemLocaleBuilder_SendMessagesToEsb_MaxRecordsLimitsData()
        {
            // Given
            int maxNumberOfRows = 2;
            var expectedNonReceivingSystems = "non receivers";
            string transactionType = "Item/Locale";
            var itemLocaleModels = new List<ItemLocaleModelForWormhole>
            {
                testData.ItemLocale_BU10042_UPC,
                testData.ItemLocale_BU10042_PosPlu,
                testData.ItemLocale_BU10042_ScalePlu,
                testData.ItemLocale_BU10042_UPC_WithLinkedItem
            };

            // When
            IconItemLocaleBuilder.SendMessagesToEsb(itemLocaleModels, mockEsbProducer.Object,
                false, null, expectedNonReceivingSystems, maxNumberOfRows, transactionType);

            // Then
            Assert.AreEqual(1, IconItemLocaleBuilder.NumberOfMessagesSent);
            Assert.AreEqual(2, IconItemLocaleBuilder.NumberOfRecordsSent);
        }

        [TestMethod]
        public void IconItemLocaleBuilder_SendMessagesToEsb_MaxRecordsOfZeroMeansAll()
        {
            // Given
            int maxNumberOfRows = 0;
            var expectedNonReceivingSystems = "non receivers";
            string transactionType = "Item/Locale";

            var itemLocaleModels = new List<ItemLocaleModelForWormhole>
            {
                testData.ItemLocale_BU10042_UPC,
                testData.ItemLocale_BU10042_PosPlu
            };

            // When
            IconItemLocaleBuilder.SendMessagesToEsb(itemLocaleModels, mockEsbProducer.Object,
                false, null, expectedNonReceivingSystems, maxNumberOfRows, transactionType);

            // Then
            Assert.AreEqual(1, IconItemLocaleBuilder.NumberOfMessagesSent);
            Assert.AreEqual(2, IconItemLocaleBuilder.NumberOfRecordsSent);
        }
    }
}
