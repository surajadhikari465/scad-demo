using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Icon.Esb.Producer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace AmazonLoad.MammothItemLocale.Tests
{
    [TestClass]
    public class MammothItemLocaleBuilderTests
    {
        TestData testData = new TestData();
        Mock<IEsbProducer> mockEsbProducer = new Mock<IEsbProducer>();
        string actualXmlMsg = string.Empty;

        string connectionString = string.Empty;

        [TestInitialize]
        public void Initialize()
        {
            connectionString = ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString;

            MammothItemLocaleBuilder.NumberOfMessagesSent = 0;
            MammothItemLocaleBuilder.NumberOfRecordsSent = 0;
        }
        string mammothConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString;
            }
        }


        string region = "MA";
        string TestRegion
        {
            get { return region; }
            set { region = value; }
        }

     

        [TestMethod]
        public void MammothItemLocaleBuilder_SendMessagesToEsb_SingleMessageCallsSend()
        {
            // Given
            int maxNumberOfRows = 10;
            var expectedNonReceivingSystems = "non receivers";
            string transactionType = "Item/Locale";

            var itemLocaleModels = new List<MammothItemLocaleModel>
            {
                testData.ItemLocale_10220_CamembertLocal
            };

            // When
            MammothItemLocaleBuilder.SendMessagesToEsb(itemLocaleModels, mockEsbProducer.Object,
                false, null, expectedNonReceivingSystems, maxNumberOfRows, transactionType);

            // Then
            Assert.AreEqual(1, MammothItemLocaleBuilder.NumberOfMessagesSent);
            Assert.AreEqual(1, MammothItemLocaleBuilder.NumberOfRecordsSent);
            mockEsbProducer.Verify(p => p.Send(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<Dictionary<string, string>>()),
            Times.Once);
        }

        [TestMethod]
        public void MammothItemLocaleBuilder_SendMessagesToEsb_SendsExpectedMsgPropNonReceivingSys()
        {
            // Given
            int maxNumberOfRows = 10;
            var expectedNonReceivingSystems = "non receivers";
            string transactionType = "Item/Locale";

            var itemLocaleModels = new List<MammothItemLocaleModel>
            {
                testData.ItemLocale_10220_PaleAleWithDep
            };

            // When
            MammothItemLocaleBuilder.SendMessagesToEsb(itemLocaleModels, mockEsbProducer.Object,
                false, null, expectedNonReceivingSystems, maxNumberOfRows, transactionType);

            // Then
            mockEsbProducer.Verify(p => p.Send(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.Is<Dictionary<string, string>>(dict => dict["nonReceivingSysName"] == expectedNonReceivingSystems)),
            Times.Once);
        }

        [TestMethod]
        public void MammothItemLocaleBuilder_SendMessagesToEsb_SendsExpectedMsgPropTransactionType()
        {
            // Given
            int maxNumberOfRows = 10;
            var expectedNonReceivingSystems = "non receivers";
            string expectedTransactionType = "Item/Locale";

            var itemLocaleModels = new List<MammothItemLocaleModel>
            {
                testData.ItemLocale_10220_PaleAleWithDep
            };

            // When
            MammothItemLocaleBuilder.SendMessagesToEsb(itemLocaleModels, mockEsbProducer.Object,
                false, null, expectedNonReceivingSystems, maxNumberOfRows, expectedTransactionType);

            // Then
            mockEsbProducer.Verify(p => p.Send(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.Is<Dictionary<string, string>>(dict => dict["TransactionType"] == expectedTransactionType)),
            Times.Once);
        }


        [TestMethod]
        public void MammothItemLocaleBuilder_SendMessagesToEsb_ProducesExpectedAddMessageForSingleLocalItem()
        {
            // Given
            int maxNumberOfRows = 10;
            var expectedMsg = File.ReadAllText("ExpectedTestMessage_10220_CamembertLocal.xml");
            var itemLocaleModels = new List<MammothItemLocaleModel>
            {
                testData.ItemLocale_10220_CamembertLocal
            };

            mockEsbProducer.Setup(p => p.Send(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<Dictionary<string, string>>()))
            .Callback<string, string, Dictionary<string, string>>(
                ((message, messageId, messageProperties) => { actualXmlMsg = message; }));

            // When
            MammothItemLocaleBuilder.SendMessagesToEsb(itemLocaleModels, mockEsbProducer.Object,
                false, null, "non receivers", maxNumberOfRows, "Item/Locale");

            // Then
            Assert.AreEqual(expectedMsg, actualXmlMsg, "esb xml message");
        }

        [TestMethod]
        public void MammothItemLocaleBuilder_SendMessagesToEsb_ProducesExpectedAddMessageForSingleLinkedItem()
        {
            // Given
            int maxNumberOfRows = 10;
            string transactionType = "Item/Locale";
            var expectedMsg = File.ReadAllText("ExpectedTestMessage_10220_PaleAleWithDep.xml");
            var itemLocaleModels = new List<MammothItemLocaleModel>
            {
                testData.ItemLocale_10220_PaleAleWithDep
            };

            mockEsbProducer.Setup(p => p.Send(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<Dictionary<string, string>>()))
            .Callback<string, string, Dictionary<string, string>>(
                ((message, messageId, messageProperties) => { actualXmlMsg = message; }));

            // When
            MammothItemLocaleBuilder.SendMessagesToEsb(itemLocaleModels, mockEsbProducer.Object,
                false, null, "non receivers", maxNumberOfRows, transactionType);

            // Then
            Assert.AreEqual(expectedMsg, actualXmlMsg, "esb xml message");
        }

        [TestMethod]
        public void MammothItemLocaleBuilder_SendMessagesToEsb_ProducesExpectedAddMessageForSingleMsrpItem()
        {
            // Given
            int maxNumberOfRows = 10;
            string transactionType = "Item/Locale";
            var expectedMsg = File.ReadAllText("ExpectedTestMessage_10220_Pillow.xml");
            var itemLocaleModels = new List<MammothItemLocaleModel>
            {
                testData.ItemLocale_10220_Pillow
            };

            mockEsbProducer.Setup(p => p.Send(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<Dictionary<string, string>>()))
            .Callback<string, string, Dictionary<string, string>>(
                ((message, messageId, messageProperties) => { actualXmlMsg = message; }));

            // When
            MammothItemLocaleBuilder.SendMessagesToEsb(itemLocaleModels, mockEsbProducer.Object,
                false, null, "non receivers", maxNumberOfRows, transactionType);

            // Then
            Assert.AreEqual(expectedMsg, actualXmlMsg, "esb xml message");
        }

        [TestMethod]
        public void MammothItemLocaleBuilder_SendMessagesToEsb_ProducesExpectedAddMessageForMultipleItems()
        {
            // Given
            int maxNumberOfRows = 10;
            string transactionType = "Item/Locale";
            var expectedMsg = File.ReadAllText("ExpectedTestMessage_MultipleItems.xml");
            var itemLocaleModels = new List<MammothItemLocaleModel>
            {
                testData.ItemLocale_10220_CamembertLocal,
                testData.ItemLocale_10220_PaleAleWithDep,
                testData.ItemLocale_10220_Pillow
            };

            mockEsbProducer.Setup(p => p.Send(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<Dictionary<string, string>>()))
            .Callback<string, string, Dictionary<string, string>>(
                ((message, messageId, messageProperties) => { actualXmlMsg = message; }));
            // When
            MammothItemLocaleBuilder.SendMessagesToEsb(itemLocaleModels, mockEsbProducer.Object,
                false, null, "non receivers", maxNumberOfRows, transactionType);

            // Then
            Assert.AreEqual(1, MammothItemLocaleBuilder.NumberOfMessagesSent);
            Assert.AreEqual(3, MammothItemLocaleBuilder.NumberOfRecordsSent);
            Assert.AreEqual(expectedMsg, actualXmlMsg, "esb xml message");
        }

        [TestMethod]
        public void MammothItemLocaleBuilder_SendMessagesToEsb_MaxRecordsLimitsData()
        {
            // Given
            int maxNumberOfRows = 2;
            string transactionType = "Item/Locale";
            var expectedNonReceivingSystems = "non receivers";
            var itemLocaleModels = new List<MammothItemLocaleModel>
            {
                testData.ItemLocale_10220_CamembertLocal,
                testData.ItemLocale_10220_PaleAleWithDep,
                testData.ItemLocale_10220_Pillow
            };

            // When
            MammothItemLocaleBuilder.SendMessagesToEsb(itemLocaleModels, mockEsbProducer.Object,
                false, null, expectedNonReceivingSystems, maxNumberOfRows, transactionType);

            // Then
            Assert.AreEqual(1, MammothItemLocaleBuilder.NumberOfMessagesSent);
            Assert.AreEqual(2, MammothItemLocaleBuilder.NumberOfRecordsSent);
        }

        [TestMethod]
        public void MammothItemLocaleBuilder_SendMessagesToEsb_MaxRecordsOfZeroMeansAll()
        {
            // Given
            int maxNumberOfRows = 0;
            var expectedNonReceivingSystems = "non receivers";
            string transactionType = "Item/Locale";

            var itemLocaleModels = new List<MammothItemLocaleModel>
            {
                testData.ItemLocale_10220_CamembertLocal,
                testData.ItemLocale_10220_PaleAleWithDep,
                testData.ItemLocale_10220_Pillow
            };

            // When
            MammothItemLocaleBuilder.SendMessagesToEsb(itemLocaleModels, mockEsbProducer.Object,
                false, null, expectedNonReceivingSystems, maxNumberOfRows, transactionType);

            // Then
            Assert.AreEqual(1, MammothItemLocaleBuilder.NumberOfMessagesSent);
            Assert.AreEqual(3, MammothItemLocaleBuilder.NumberOfRecordsSent);
        }
    }
}
