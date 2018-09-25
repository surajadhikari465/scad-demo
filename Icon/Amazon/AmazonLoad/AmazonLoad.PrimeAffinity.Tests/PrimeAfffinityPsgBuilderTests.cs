using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Data.SqlClient;
using System.Collections.Generic;
using Icon.Esb.Producer;
using System.IO;
using Moq;
using System.Configuration;

namespace AmazonLoad.PrimeAffinityPsg.Tests
{
    [TestClass]
    public class PrimeAfffinityPsgBuilderTests
    {
        TestData testData = new TestData();
        const string primeAffinityPSG_id = "PrimeAffinityPSG";
        const string primeAffinityPSG_name = "PrimeAffinityPSG";
        const string primeAffinityPSG_type = "Consumable";
        int df_maxNumberOfRows = 10;
        string df_nonReceivingSystems = "Slaw,Mammoth,Spice,ESL,1Plum,Info";
        bool df_saveMessagesFlag = false;
        string df_saveMessagesDir = "Messages";
        bool df_sendToEsbFlag = true;
        string df_priceTypes = "'SAL','ISS','FRZ'";
        string df_excludedPsNumbers = "2100,2200,2220";
        string actualXmlMsg = string.Empty;
        Mock<IEsbProducer> mockEsbProducer = new Mock<IEsbProducer>();
        
        string mammothConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString;
            }
        }

        [TestInitialize]
        public void TestInitialize()
        {
            PrimeAffinityPsgBuilder.NumberOfMessagesSent = 0;
            PrimeAffinityPsgBuilder.NumberOfRecordsSent = 0;
        }

        [TestMethod]
        public void PrimeAffinityPsgBuilder_GetFormattedSqlForPrimeAffinityPsgQuery_WhenGpmActive_ReplacesTop()
        {
            //Given
            var region = "FL";
            bool isGpmActive = true;
            string businessUnit = "10130";
            int maxNumberOfRows = df_maxNumberOfRows;
            string priceTypes = df_priceTypes;
            string excludedPsNumbers = df_excludedPsNumbers;

            string expectedTop = $"SELECT TOP {maxNumberOfRows}";
            // When
            var result = PrimeAffinityPsgBuilder.GetFormattedSqlForPrimeAffinityPsgQuery(region, isGpmActive,
                businessUnit, maxNumberOfRows, priceTypes, excludedPsNumbers);

            // Then
            Assert.IsTrue(result.Contains(expectedTop));
        }

        [TestMethod]
        public void PrimeAffinityPsgBuilder_GetFormattedSqlForPrimeAffinityPsgQuery_WhenGpmInactive_ReplacesTop()
        {
            //Given
            var region = "MA";
            bool isGpmActive = false;
            string businessUnit = "10181";
            int maxNumberOfRows = df_maxNumberOfRows;
            string priceTypes = df_priceTypes;
            string excludedPsNumbers = df_excludedPsNumbers;

            string expectedTop = $"SELECT TOP {maxNumberOfRows}";

            // When
            var result = PrimeAffinityPsgBuilder.GetFormattedSqlForPrimeAffinityPsgQuery(region, isGpmActive,
                businessUnit, maxNumberOfRows, priceTypes, excludedPsNumbers);

            // Then
            Assert.IsTrue(result.Contains(expectedTop));
        }

        [TestMethod]
        public void PrimeAffinityPsgBuilder_GetFormattedSqlForPrimeAffinityPsgQuery_WhenGpmActive_ReplacesRegion()
        {
            //Given
            var region = "FL";
            bool isGpmActive = true;
            string businessUnit = "10130";
            int maxNumberOfRows = 500;
            string priceTypes = df_priceTypes;
            string excludedPsNumbers = df_excludedPsNumbers;

            string expectedSubstitutionForLocale = $"dbo.Locales_{region} l";
            string expectedSubstitutionForPrice = $"gpm.Price_{region} gpm";

            // When
            var result = PrimeAffinityPsgBuilder.GetFormattedSqlForPrimeAffinityPsgQuery(region, isGpmActive,
                    businessUnit, maxNumberOfRows, priceTypes, excludedPsNumbers);

            // Then
            Assert.IsTrue(result.Contains(expectedSubstitutionForLocale));
            Assert.IsTrue(result.Contains(expectedSubstitutionForPrice));
        }

        [TestMethod]
        public void PrimeAffinityPsgBuilder_GetFormattedSqlForPrimeAffinityPsgQuery_WhenGpmActive_ReplacesBusinessUnit()
        {
            //Given
            var region = "FL";
            bool isGpmActive = true;
            string businessUnit = "10320";
            int maxNumberOfRows = 500;
            string priceTypes = df_priceTypes;
            string excludedPsNumbers = df_excludedPsNumbers;

            string expectedBusinessUnitSubForLocale = $"ON l.BusinessUnitID = {businessUnit}";

            // When
            var result = PrimeAffinityPsgBuilder.GetFormattedSqlForPrimeAffinityPsgQuery(region, isGpmActive,
                    businessUnit, maxNumberOfRows, priceTypes, excludedPsNumbers);

            // Then
            Assert.IsTrue(result.Contains(expectedBusinessUnitSubForLocale));
        }

        [TestMethod]
        public void PrimeAffinityPsgBuilder_GetFormattedSqlForPrimeAffinityPsgQuery_WhenGpmInactive_ReplacesRegion()
        {
            //Given
            var region = "MA";
            bool isGpmActive = false;
            string businessUnit = "10181";
            int maxNumberOfRows = 500;
            string priceTypes = df_priceTypes;
            string excludedPsNumbers = df_excludedPsNumbers;

            string expectedSubstitutionForLocale = $"dbo.Locales_{region} l";
            string expectedSubstitutionForPrice = $"dbo.Price_{region} prc";
            string expectedSubstitutionForPriceRegion = $"WHERE prc.Region = '{region}'";

            // When
            var result = PrimeAffinityPsgBuilder.GetFormattedSqlForPrimeAffinityPsgQuery(region, isGpmActive,
                    businessUnit, maxNumberOfRows, priceTypes, excludedPsNumbers);

            // Then
            Assert.IsTrue(result.Contains(expectedSubstitutionForLocale));
            Assert.IsTrue(result.Contains(expectedSubstitutionForPrice));
            Assert.IsTrue(result.Contains(expectedSubstitutionForPriceRegion));
        }

        [TestMethod]
        public void PrimeAffinityPsgBuilder_GetFormattedSqlForPrimeAffinityPsgQuery_WhenGpmInactive_ReplacesBusinessUnit()
        {
            //Given
            var region = "NC";
            bool isGpmActive = false;
            string businessUnit = "10320";
            int maxNumberOfRows = 500;
            string priceTypes = df_priceTypes;
            string excludedPsNumbers = df_excludedPsNumbers;

            string expectedBusinessUnitSubForLocale = $"ON l.BusinessUnitID = {businessUnit}";
            string expectedBusinessUnitSubForPrice = $"AND prc.BusinessUnitID = {businessUnit}";
            string expectedBusinessUnitSubForWhere = $"l.BusinessUnitID = {businessUnit}";

            // When
            var result = PrimeAffinityPsgBuilder.GetFormattedSqlForPrimeAffinityPsgQuery(region, isGpmActive,
                    businessUnit, maxNumberOfRows, priceTypes, excludedPsNumbers);

            // Then
            Assert.IsTrue(result.Contains(expectedBusinessUnitSubForLocale));
            Assert.IsTrue(result.Contains(expectedBusinessUnitSubForPrice));
            Assert.IsTrue(result.Contains(expectedBusinessUnitSubForWhere));
        }

        [TestMethod]
        public void PrimeAffinityPsgBuilder_GetFormattedSqlForQuery_WhenGpmInactive_ReplacesPriceTypes()
        {
            // Given
            string region = "XY";
            bool isGpmActive = false;
            string businessUnit = "12345";
            int maxNumberOfRows = 60;
            string priceTypes = "'ABC','DEF','GHI','JKL'";
            string excludedPsNumbers = df_excludedPsNumbers;

            string expectedPriceTypesSubstitution = $"prc.PriceType IN ({priceTypes})";

            // When
            var result = PrimeAffinityPsgBuilder.GetFormattedSqlForPrimeAffinityPsgQuery(region, isGpmActive,
                    businessUnit, maxNumberOfRows, priceTypes, excludedPsNumbers);

            // Then
            Assert.IsTrue(result.Contains(expectedPriceTypesSubstitution));
        }

        [TestMethod]
        public void PrimeAffinityPsgBuilder_GetFormattedSqlForPrimeAffinityPsgQuery_WhenGpmActive_ReplacesExcludedPsNumbers()
        {
            // Given
            string region = "FL";
            string businessUnit = "10130";
            bool isGpmActive = true;
            int maxNumberOfRows = 100;
            string priceTypes = df_priceTypes;
            string excludedPsNumbers = "1000,2000,3000";

            string expectedPsNumberSubstitution = $"i.PSNumber NOT IN ({excludedPsNumbers})";

            // When
            var result = PrimeAffinityPsgBuilder.GetFormattedSqlForPrimeAffinityPsgQuery(region, isGpmActive,
                    businessUnit, maxNumberOfRows, priceTypes, excludedPsNumbers);

            // Then
            Assert.IsTrue(result.Contains(expectedPsNumberSubstitution));
        }

        [TestMethod]
        public void PrimeAffinityPsgBuilder_GetFormattedSqlForPrimeAffinityPsgQuery_WhenGpmInactive_ReplacesExcludedPsNumbers()
        {
            // Given
            string region = "FL";
            bool isGpmActive = false;
            string businessUnit = "10130";
            int maxNumberOfRows = 100;
            string priceTypes = df_priceTypes;
            string excludedPsNumbers = "1000,2000,3000";

            string expectedPsNumberSubstitution = $"i.PSNumber NOT IN ({excludedPsNumbers})";

            // When
            var result = PrimeAffinityPsgBuilder.GetFormattedSqlForPrimeAffinityPsgQuery(region, isGpmActive,
                    businessUnit, maxNumberOfRows, priceTypes, excludedPsNumbers);

            // Then
            Assert.IsTrue(result.Contains(expectedPsNumberSubstitution));
        }

        [TestMethod]
        public void PrimeAffinityPsgBuilder_GetFormattedSqlForMammothLocalesQuery_ReplacesRegion()
        {
            // Given
            string region = "NC";
            string expectedRegionSubstitution = $"WHERE l.Region = '{region}' and";

            // When
            var result = PrimeAffinityPsgBuilder.GetFormattedSqlForMammothLocalesQuery(region);

            // Then
            Assert.IsTrue(result.Contains(expectedRegionSubstitution));
        }

        [TestMethod]
        public void PrimeAffinityPsgBuilder_LoadMammothLocales_ReturnsExpectedModelCount()
        {
            // Given
            string region = "FL";
            int expectedRows = 33;

                //using (SqlConnection mammothSqlConnection = new SqlConnection(mammothConnectionString))
                // {
                // When
                var localeData = PrimeAffinityPsgBuilder.LoadMammothLocales(mammothConnectionString, region);

                // Then
                Assert.IsNotNull(localeData);
                Assert.AreEqual(expectedRows, localeData.Count());
            //}
        }

        [TestMethod]
        public void PrimeAffinityPsgBuilder_LoadPrimeAffinityPsgs_WhenGpmActive_ReturnsExpectedModelCount()
        {
            // Given
            string region = "FL";
            int maxRows = 25;
            string businessUnit = "10130";
            bool isGpmActive = true;

            using (SqlConnection mammothSqlConnection = new SqlConnection(mammothConnectionString))
            {
                // When
                var primeAffinityData = PrimeAffinityPsgBuilder.LoadPrimeAffinityPsgs(mammothSqlConnection, region,
                    businessUnit, isGpmActive, maxRows, df_priceTypes, df_excludedPsNumbers);

                // Then
                Assert.IsNotNull(primeAffinityData);
                Assert.AreEqual(maxRows, primeAffinityData.Count());
            }
        }

        [TestMethod]
        public void PrimeAffinityPsgBuilder_LoadPrimeAffinityPsgs_WhenGpmInactive_ReturnsExpectedModelCount()
        {
            // Given
            string region = "MA";
            string businessUnit = "10181";
            int maxRows = 25;
            bool isGpmActive = false;

            using (SqlConnection mammothSqlConnection = new SqlConnection(mammothConnectionString))
            {
                // When
                var primeAffinityData = PrimeAffinityPsgBuilder.LoadPrimeAffinityPsgs(mammothSqlConnection, region,
                    businessUnit, isGpmActive, maxRows, df_priceTypes, df_excludedPsNumbers);

                // Then
                Assert.IsNotNull(primeAffinityData);
                Assert.AreEqual(maxRows, primeAffinityData.Count());
            }
        }

        [TestMethod]
        public void PrimeAffinityPsgBuilder_LoadPrimeAffinityPsgs_WhenGpmActive_OnlyLoadsFromExpectedRegion()
        {
            // Given
            string region = "FL";
            bool isGpmActive = true;
            string businessUnit = "10130";

            using (SqlConnection mammothSqlConnection = new SqlConnection(mammothConnectionString))
            {
                // When
                var primeAffinityData = PrimeAffinityPsgBuilder.LoadPrimeAffinityPsgs(mammothSqlConnection, region,
                    businessUnit, isGpmActive, df_maxNumberOfRows, df_priceTypes, df_excludedPsNumbers);

                // Then
                Assert.IsNotNull(primeAffinityData);
                foreach (var model in primeAffinityData)
                {
                    Assert.AreEqual(region, model.RegionCode);
                }
            }
        }

        [TestMethod]
        public void PrimeAffinityPsgBuilder_LoadPrimeAffinityPsgs_WhenGpmInactive_OnlyLoadsFromExpectedRegion()
        {
            // Given
            string region = "FL";
            bool isGpmActive = false;
            string businessUnit = "10130";

            using (SqlConnection mammothSqlConnection = new SqlConnection(mammothConnectionString))
            {
                // When
                var primeAffinityData = PrimeAffinityPsgBuilder.LoadPrimeAffinityPsgs(mammothSqlConnection, region,
                    businessUnit, isGpmActive, df_maxNumberOfRows, df_priceTypes, df_excludedPsNumbers);

                // Then
                Assert.IsNotNull(primeAffinityData);
                foreach (var model in primeAffinityData)
                {
                    Assert.AreEqual(region, model.RegionCode);
                }
            }
        }

        [TestMethod]
        public void PrimeAffinityPsgBuilder_LoadPrimeAffinityPsgs_WhenGpmActive_ReturnsModelsWithExpectedData()
        {
            // Given
            string region = "FL";
            int maxRows = 10;
            bool isGpmActive = true;
            string businessUnit = "10130";

            List<PrimeAffinityPsgModel> primeAffinityData = new List<PrimeAffinityPsgModel>(maxRows);

            using (SqlConnection mammothSqlConnection = new SqlConnection(mammothConnectionString))
            {
                // When
                primeAffinityData = PrimeAffinityPsgBuilder
                    .LoadPrimeAffinityPsgs(mammothSqlConnection, region,
                    businessUnit, isGpmActive, maxRows, df_priceTypes, df_excludedPsNumbers)
                    .ToList();
            }

            // Then
            var item = primeAffinityData.FirstOrDefault();
            Assert.IsTrue(item.BusinessUnit > 0);
            Assert.IsTrue(item.ItemId > 0);
            Assert.AreEqual("RTL", item.ItemTypeCode);
            Assert.AreEqual("Retail Sale", item.ItemTypeDesc);
            Assert.IsFalse(string.IsNullOrEmpty(item.LocaleName));
            Assert.AreEqual(region, item.RegionCode);
            Assert.IsFalse(string.IsNullOrEmpty(item.ScanCode));
        
        }

        [TestMethod]
        public void PrimeAffinityPsgBuilder_LoadPrimeAffinityPsgs_WhenGpmInactive_ReturnsModelsWithExpectedData()
        {
            // Given
            string region = "MA";
            int maxRows = 10;
            bool isGpmActive = false;
            string businessUnit = "10181";

            using (SqlConnection mammothSqlConnection = new SqlConnection(mammothConnectionString))
            {
                // When
                var primeAffinityData = PrimeAffinityPsgBuilder.LoadPrimeAffinityPsgs(mammothSqlConnection, region,
                    businessUnit, isGpmActive, maxRows, df_priceTypes, df_excludedPsNumbers).ToList();

                // Then
                var item = primeAffinityData.FirstOrDefault();
                Assert.IsTrue(item.BusinessUnit > 0);
                Assert.IsTrue(item.ItemId > 0);
                Assert.AreEqual("RTL", item.ItemTypeCode);
                Assert.AreEqual("Retail Sale", item.ItemTypeDesc);
                Assert.IsFalse(string.IsNullOrEmpty(item.LocaleName));
                Assert.AreEqual(region, item.RegionCode);
                Assert.IsFalse(string.IsNullOrEmpty(item.ScanCode));
            }
        }

        [TestMethod]
        public void PrimeAffinityPsgBuilder_LoadPrimeAffinityPsgs_WhenGpmActive_ReturnsDataWithinTimeLimit()
        {
            // Given
            string region = "FL";
            int maxRows = 100;
            bool isGpmActive = true;
            int maxAllowedMs = 2000;
            string businessUnit = "10130";

            DateTime start = DateTime.UtcNow;
            using (SqlConnection mammothSqlConnection = new SqlConnection(mammothConnectionString))
            {
                // When
                var primeAffinityData = PrimeAffinityPsgBuilder.LoadPrimeAffinityPsgs(mammothSqlConnection, region,
                    businessUnit, isGpmActive, maxRows, df_priceTypes, df_excludedPsNumbers);

                Assert.IsNotNull(primeAffinityData);
                Assert.AreEqual(maxRows, primeAffinityData.Count());
            }
            DateTime end = DateTime.UtcNow;
            var elapsedMs = (end - start).TotalMilliseconds;

            // Then
            Assert.IsTrue(elapsedMs < maxAllowedMs, $"Took too long ({elapsedMs:0}ms) to load {maxRows} records. Limit: {maxAllowedMs}ms");
        }

        [TestMethod]
        public void PrimeAffinityPsgBuilder_LoadPrimeAffinityPsgs_WhenGpmInactive_ReturnsDataWithinTimeLimit()
        {
            // Given
            string region = "MA";
            int maxRows = 100;
            bool isGpmActive = false;
            int maxAllowedMs = 2000;
            string businessUnit = "10181";

            DateTime start = DateTime.UtcNow;
            using (SqlConnection mammothSqlConnection = new SqlConnection(mammothConnectionString))
            {
                // When
                var primeAffinityData = PrimeAffinityPsgBuilder.LoadPrimeAffinityPsgs(mammothSqlConnection, region,
                    businessUnit, isGpmActive, maxRows, df_priceTypes, df_excludedPsNumbers);

                Assert.IsNotNull(primeAffinityData);
                Assert.AreEqual(maxRows, primeAffinityData.Count());
            }
            DateTime end = DateTime.UtcNow;
            var elapsedMs = (end - start).TotalMilliseconds;

            // Then
            Assert.IsTrue(elapsedMs < maxAllowedMs, $"Took too long ({elapsedMs:0}ms) to load {maxRows} records. Limit: {maxAllowedMs}ms");
        }

        [TestMethod]
        public void PrimeAffinityPsgBuilder_SendMessagesToEsb_WhenGpmNonPrimeItem_SendsSingRecord()
        {
            //Given
            var mockEsbProducer = new Mock<IEsbProducer>();

            var primeAffinityModels = new List<PrimeAffinityPsgModel>
            {
                testData.Item_FL_10130_NonPrime_A,
            };

            // When
            using (var mammothSqlConnection = new SqlConnection(mammothConnectionString))
            {
                PrimeAffinityPsgBuilder.SendMessagesToEsb(primeAffinityModels, mockEsbProducer.Object, df_nonReceivingSystems, df_maxNumberOfRows,
                    primeAffinityPSG_id, primeAffinityPSG_name, primeAffinityPSG_type, df_saveMessagesFlag, df_saveMessagesDir, df_sendToEsbFlag);

                // Then
                Assert.AreEqual(1, PrimeAffinityPsgBuilder.NumberOfRecordsSent);
                Assert.AreEqual(1, PrimeAffinityPsgBuilder.NumberOfMessagesSent);
                mockEsbProducer.Verify(p =>
                    p.Send(
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<Dictionary<string, string>>()
                    ), Times.Once);
            }
        }

        [TestMethod]
        public void PrimeAffinityPsgBuilder_SendMessagesToEsb_WhenGpmPrimeItem_SendsSingleRecord()
        {
            //Given
            var mockEsbProducer = new Mock<IEsbProducer>();

            var primeAffinityModels = new List<PrimeAffinityPsgModel>
            {
                testData.Item_FL_10130_Prime_C,
            };

            // When
            using (var mammothSqlConnection = new SqlConnection(mammothConnectionString))
            {
                PrimeAffinityPsgBuilder.SendMessagesToEsb(primeAffinityModels, mockEsbProducer.Object, df_nonReceivingSystems, df_maxNumberOfRows,
                    primeAffinityPSG_id, primeAffinityPSG_name, primeAffinityPSG_type, df_saveMessagesFlag, df_saveMessagesDir, df_sendToEsbFlag);

                // Then
                Assert.AreEqual(1, PrimeAffinityPsgBuilder.NumberOfRecordsSent);
                Assert.AreEqual(1, PrimeAffinityPsgBuilder.NumberOfMessagesSent);
                mockEsbProducer.Verify(p =>
                    p.Send(
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<Dictionary<string, string>>()
                    ), Times.Once);
            }
        }

        [TestMethod]
        public void PrimeAffinityPsgBuilder_SendMessagesToEsb_WhenNonGpmNonPrimeItem_SendsSingRecord()
        {
            //Given
            var mockEsbProducer = new Mock<IEsbProducer>();

            var primeAffinityModels = new List<PrimeAffinityPsgModel>
            {
                testData.Item_MA_10181_NonPrime_A,
            };

            // When
            using (var mammothSqlConnection = new SqlConnection(mammothConnectionString))
            {
                PrimeAffinityPsgBuilder.SendMessagesToEsb(primeAffinityModels, mockEsbProducer.Object, df_nonReceivingSystems, df_maxNumberOfRows,
                    primeAffinityPSG_id, primeAffinityPSG_name, primeAffinityPSG_type, df_saveMessagesFlag, df_saveMessagesDir, df_sendToEsbFlag);

                // Then
                Assert.AreEqual(1, PrimeAffinityPsgBuilder.NumberOfRecordsSent);
                Assert.AreEqual(1, PrimeAffinityPsgBuilder.NumberOfMessagesSent);
                mockEsbProducer.Verify(p =>
                    p.Send(
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<Dictionary<string, string>>()
                    ), Times.Once);
            }
        }

        [TestMethod]
        public void PrimeAffinityPsgBuilder_SendMessagesToEsb_WhenNonGpmPrimeItem_SendsSingleRecord()
        {
            //Given
            var mockEsbProducer = new Mock<IEsbProducer>();

            var primeAffinityModels = new List<PrimeAffinityPsgModel>
            {
                testData.Item_MA_10181_Prime_C,
            };

            // When
            using (var mammothSqlConnection = new SqlConnection(mammothConnectionString))
            {
                PrimeAffinityPsgBuilder.SendMessagesToEsb(primeAffinityModels, mockEsbProducer.Object, df_nonReceivingSystems, df_maxNumberOfRows,
                    primeAffinityPSG_id, primeAffinityPSG_name, primeAffinityPSG_type, df_saveMessagesFlag, df_saveMessagesDir, df_sendToEsbFlag);

                // Then
                Assert.AreEqual(1, PrimeAffinityPsgBuilder.NumberOfRecordsSent);
                Assert.AreEqual(1, PrimeAffinityPsgBuilder.NumberOfMessagesSent);
                mockEsbProducer.Verify(p =>
                    p.Send(
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<Dictionary<string, string>>()
                    ), Times.Once);
            }
        }


        public void PrimeAffinityPsgBuilder_SendMessagesToEsb_WhenMultipleNonGpmNonPrimeItems_SendsMultipleRecords()
        {
            //Given
            var mockEsbProducer = new Mock<IEsbProducer>();
           
            var primeAffinityModels = new List<PrimeAffinityPsgModel>
            {
                testData.Item_MA_10181_NonPrime_A,
                testData.Item_MA_10181_NonPrime_B,
            };

            // When
            using (var mammothSqlConnection = new SqlConnection(mammothConnectionString))
            {
                PrimeAffinityPsgBuilder.SendMessagesToEsb(primeAffinityModels, mockEsbProducer.Object, df_nonReceivingSystems, df_maxNumberOfRows,
                    primeAffinityPSG_id, primeAffinityPSG_name, primeAffinityPSG_type, df_saveMessagesFlag, df_saveMessagesDir, df_sendToEsbFlag);
            }

            // Then
            Assert.AreEqual(2, PrimeAffinityPsgBuilder.NumberOfRecordsSent);
            Assert.AreEqual(1, PrimeAffinityPsgBuilder.NumberOfMessagesSent);
        }

        [TestMethod]
        public void PrimeAffinityPsgBuilder_SendMessagesToEsb_WhenMultipleNonGpmPrimeItems_SendsMultipleRecords()
        {
            //Given
            var mockEsbProducer = new Mock<IEsbProducer>();

            var primeAffinityModels = new List<PrimeAffinityPsgModel>
            {
                testData.Item_MA_10181_Prime_C,
                testData.Item_MA_10181_Prime_D
            };

            // When
            using (var mammothSqlConnection = new SqlConnection(mammothConnectionString))
            {
                PrimeAffinityPsgBuilder.SendMessagesToEsb(primeAffinityModels, mockEsbProducer.Object, df_nonReceivingSystems, df_maxNumberOfRows,
                    primeAffinityPSG_id, primeAffinityPSG_name, primeAffinityPSG_type, df_saveMessagesFlag, df_saveMessagesDir, df_sendToEsbFlag);
            }

            // Then
            Assert.AreEqual(2, PrimeAffinityPsgBuilder.NumberOfRecordsSent);
            Assert.AreEqual(1, PrimeAffinityPsgBuilder.NumberOfMessagesSent);
            mockEsbProducer.Verify(p =>
                p.Send(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, string>>()
                ), Times.Once);
        }

        [TestMethod]
        public void PrimeAffinityPsgBuilder_SendMessagesToEsb_WhenMultipleGpmNonPrimeItems_SendsMultipleRecords()
        {
            //Given
            var mockEsbProducer = new Mock<IEsbProducer>();

            var primeAffinityModels = new List<PrimeAffinityPsgModel>
            {
                testData.Item_FL_10130_NonPrime_A,
                testData.Item_FL_10130_NonPrime_B
            };

            // When
            using (var mammothSqlConnection = new SqlConnection(mammothConnectionString))
            {
                PrimeAffinityPsgBuilder.SendMessagesToEsb(primeAffinityModels, mockEsbProducer.Object, df_nonReceivingSystems, df_maxNumberOfRows,
                    primeAffinityPSG_id, primeAffinityPSG_name, primeAffinityPSG_type, df_saveMessagesFlag, df_saveMessagesDir, df_sendToEsbFlag);
            }

            // Then
            Assert.AreEqual(2, PrimeAffinityPsgBuilder.NumberOfRecordsSent);
            Assert.AreEqual(1, PrimeAffinityPsgBuilder.NumberOfMessagesSent);
            mockEsbProducer.Verify(p =>
                p.Send(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, string>>()
                ), Times.Once);
        }

        [TestMethod]
        public void PrimeAffinityPsgBuilder_SendMessagesToEsb_WhenMultipleGpmPrimeItems_SendsMultipleRecords()
        {
            //Given
            var mockEsbProducer = new Mock<IEsbProducer>();
            mockEsbProducer.Setup(p => p.Send(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
              .Callback<string, string, Dictionary<string, string>>(
                  ((message, messageId, messageProperties) =>
                  {
                      this.actualXmlMsg = message;
                  }));

            var expectedMsg = File.ReadAllText("ExpectedTestMessage_FL_10130_2Prime.xml");
            var primeAffinityModels = new List<PrimeAffinityPsgModel>
            {
                testData.Item_FL_10130_Prime_C,
                testData.Item_FL_10130_Prime_D
            };

            // When
            using (var mammothSqlConnection = new SqlConnection(mammothConnectionString))
            {
                PrimeAffinityPsgBuilder.SendMessagesToEsb(primeAffinityModels, mockEsbProducer.Object, df_nonReceivingSystems, df_maxNumberOfRows,
                    primeAffinityPSG_id, primeAffinityPSG_name, primeAffinityPSG_type, df_saveMessagesFlag, df_saveMessagesDir, df_sendToEsbFlag);
            }

            // Then
            Assert.AreEqual(2, PrimeAffinityPsgBuilder.NumberOfRecordsSent);
            Assert.AreEqual(1, PrimeAffinityPsgBuilder.NumberOfMessagesSent);
            mockEsbProducer.Verify(p =>
                p.Send(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, string>>()
                ), Times.Once);
            Assert.AreEqual(expectedMsg, actualXmlMsg, "esb xml message");
        }

        [TestMethod]
        public void PrimeAffinityPsgBuilder_SendMessagesToEsb_WhenMultipleNonGpmMixedtems_SendsMultipleRecords()
        {
            //Given
            var mockEsbProducer = new Mock<IEsbProducer>();

            var primeAffinityModels = new List<PrimeAffinityPsgModel>
            {
                testData.Item_MA_10181_NonPrime_A,
                testData.Item_MA_10181_NonPrime_B,
                testData.Item_MA_10181_Prime_C,
                testData.Item_MA_10181_Prime_D
            };

            // When
            using (var mammothSqlConnection = new SqlConnection(mammothConnectionString))
            {
                PrimeAffinityPsgBuilder.SendMessagesToEsb(primeAffinityModels, mockEsbProducer.Object, df_nonReceivingSystems, df_maxNumberOfRows,
                    primeAffinityPSG_id, primeAffinityPSG_name, primeAffinityPSG_type, df_saveMessagesFlag, df_saveMessagesDir, df_sendToEsbFlag);
            }

            // Then
            Assert.AreEqual(4, PrimeAffinityPsgBuilder.NumberOfRecordsSent);
            Assert.AreEqual(1, PrimeAffinityPsgBuilder.NumberOfMessagesSent);
            mockEsbProducer.Verify(p =>
                p.Send(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, string>>()
                ), Times.Once);
        }

        [TestMethod]
        public void PrimeAffinityPsgBuilder_SendMessagesToEsb_WhenMultipleGpmMixedtems_SendsMultipleRecords()
        {
            //Given
            var mockEsbProducer = new Mock<IEsbProducer>();

            var primeAffinityModels = new List<PrimeAffinityPsgModel>
            {
                testData.Item_FL_10130_Prime_C,
                testData.Item_FL_10130_Prime_D,
                testData.Item_FL_10130_NonPrime_A,
                testData.Item_FL_10130_NonPrime_B
            };

            // When
            using (var mammothSqlConnection = new SqlConnection(mammothConnectionString))
            {
                PrimeAffinityPsgBuilder.SendMessagesToEsb(primeAffinityModels, mockEsbProducer.Object, df_nonReceivingSystems, df_maxNumberOfRows,
                    primeAffinityPSG_id, primeAffinityPSG_name, primeAffinityPSG_type, df_saveMessagesFlag, df_saveMessagesDir, df_sendToEsbFlag);
            }

            // Then
            Assert.AreEqual(4, PrimeAffinityPsgBuilder.NumberOfRecordsSent);
            Assert.AreEqual(1, PrimeAffinityPsgBuilder.NumberOfMessagesSent);
            mockEsbProducer.Verify(p =>
                p.Send(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, string>>()
                ), Times.Once);
        }

        [TestMethod]
        public void PrimeAffinityPsgBuilder_SendMessagesToEsb_WhenItemsFromMultipleStores_SendsMultipleMessagesWithRecords()
        {
            //Given
            var mockEsbProducer = new Mock<IEsbProducer>();

            var primeAffinityModels = new List<PrimeAffinityPsgModel>
            {
                testData.Item_FL_10130_Prime_C,
                testData.Item_FL_10130_Prime_D,
                testData.Item_FL_10130_NonPrime_A,
                testData.Item_FL_10130_NonPrime_B,
                testData.Item_MA_10181_NonPrime_A,
                testData.Item_MA_10181_NonPrime_B,
                testData.Item_MA_10181_Prime_C,
                testData.Item_MA_10181_Prime_D
            };

            // When
            using (var mammothSqlConnection = new SqlConnection(mammothConnectionString))
            {
                PrimeAffinityPsgBuilder.SendMessagesToEsb(primeAffinityModels, mockEsbProducer.Object, df_nonReceivingSystems, df_maxNumberOfRows,
                    primeAffinityPSG_id, primeAffinityPSG_name, primeAffinityPSG_type, df_saveMessagesFlag, df_saveMessagesDir, df_sendToEsbFlag);

                // Then
                Assert.AreEqual(8, PrimeAffinityPsgBuilder.NumberOfRecordsSent);
                Assert.AreEqual(2, PrimeAffinityPsgBuilder.NumberOfMessagesSent);
                mockEsbProducer.Verify(p =>
                    p.Send(
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<Dictionary<string, string>>()
                    ), Times.Exactly(2));
            }
        }

        [TestMethod]
        public void PrimeAffinityPsgBuilder_SendMessagesToEsb_WhenGpmNonPrimeItem_MessageForSingleRecordHasExpectedXml()
        {
            //Given
            var mockEsbProducer = new Mock<IEsbProducer>();
            mockEsbProducer.Setup(p => p.Send(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
              .Callback<string, string, Dictionary<string, string>>(
                  ((message, messageId, messageProperties) =>
                  {
                      this.actualXmlMsg = message;
                  }));

            var expectedMsg = File.ReadAllText("ExpectedTestMessage_FL_10130_NonPrime_A.xml");
            var primeAffinityModels = new List<PrimeAffinityPsgModel>
            {
                testData.Item_FL_10130_NonPrime_A,
            };

            // When
            using (var mammothSqlConnection = new SqlConnection(mammothConnectionString))
            {
                PrimeAffinityPsgBuilder.SendMessagesToEsb(primeAffinityModels, mockEsbProducer.Object, df_nonReceivingSystems, df_maxNumberOfRows,
                    primeAffinityPSG_id, primeAffinityPSG_name, primeAffinityPSG_type, df_saveMessagesFlag, df_saveMessagesDir, df_sendToEsbFlag);
            }

            // Then
            Assert.AreEqual(expectedMsg, actualXmlMsg, "esb xml message");
        }

        [TestMethod]
        public void PrimeAffinityPsgBuilder_SendMessagesToEsb_WhenGpmPrimeItem_MessageForSingleRecordHasExpectedXml()
        {
            //Given
            var mockEsbProducer = new Mock<IEsbProducer>();
            mockEsbProducer.Setup(p => p.Send(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
              .Callback<string, string, Dictionary<string, string>>(
                  ((message, messageId, messageProperties) =>
                  {
                      this.actualXmlMsg = message;
                  }));

            var expectedMsg = File.ReadAllText("ExpectedTestMessage_FL_10130_Prime_C.xml");
            var primeAffinityModels = new List<PrimeAffinityPsgModel>
            {
                testData.Item_FL_10130_Prime_C,
            };

            // When
            using (var mammothSqlConnection = new SqlConnection(mammothConnectionString))
            {
                PrimeAffinityPsgBuilder.SendMessagesToEsb(primeAffinityModels, mockEsbProducer.Object, df_nonReceivingSystems, df_maxNumberOfRows,
                    primeAffinityPSG_id, primeAffinityPSG_name, primeAffinityPSG_type, df_saveMessagesFlag, df_saveMessagesDir, df_sendToEsbFlag);
            }

            // Then
            Assert.AreEqual(expectedMsg, actualXmlMsg, "esb xml message");
        }

        [TestMethod]
        public void PrimeAffinityPsgBuilder_SendMessagesToEsb_WhenNonGpmNonPrimeItem_MessageForSingleRecordHasExpectedXml()
        {
            //Given
            var mockEsbProducer = new Mock<IEsbProducer>();
            mockEsbProducer.Setup(p => p.Send(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
              .Callback<string, string, Dictionary<string, string>>(
                  ((message, messageId, messageProperties) =>
                  {
                      this.actualXmlMsg = message;
                  }));

            var expectedMsg = File.ReadAllText("ExpectedTestMessage_MA_10181_NonPrime_A.xml");
            var primeAffinityModels = new List<PrimeAffinityPsgModel>
            {
                testData.Item_MA_10181_NonPrime_A,
            };

            // When
            using (var mammothSqlConnection = new SqlConnection(mammothConnectionString))
            {
                PrimeAffinityPsgBuilder.SendMessagesToEsb(primeAffinityModels, mockEsbProducer.Object, df_nonReceivingSystems, df_maxNumberOfRows,
                    primeAffinityPSG_id, primeAffinityPSG_name, primeAffinityPSG_type, df_saveMessagesFlag, df_saveMessagesDir, df_sendToEsbFlag);
            }

            // Then
            Assert.AreEqual(expectedMsg, actualXmlMsg, "esb xml message");
        }

        [TestMethod]
        public void PrimeAffinityPsgBuilder_SendMessagesToEsb_WhenNonGpmPrimeItem_MessageForSingleRecordHasExpectedXml()
        {
            //Given
            var mockEsbProducer = new Mock<IEsbProducer>();
            mockEsbProducer.Setup(p => p.Send( It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
              .Callback<string, string, Dictionary<string, string>>(
                  ((message, messageId, messageProperties) =>
                  {
                      this.actualXmlMsg = message;
                  }));

            var expectedMsg = File.ReadAllText("ExpectedTestMessage_MA_10181_Prime_C.xml");
            var primeAffinityModels = new List<PrimeAffinityPsgModel>
            {
                testData.Item_MA_10181_Prime_C,
            };

            // When
            using (var mammothSqlConnection = new SqlConnection(mammothConnectionString))
            {
                PrimeAffinityPsgBuilder.SendMessagesToEsb(primeAffinityModels, mockEsbProducer.Object, df_nonReceivingSystems, df_maxNumberOfRows,
                    primeAffinityPSG_id, primeAffinityPSG_name, primeAffinityPSG_type, df_saveMessagesFlag, df_saveMessagesDir, df_sendToEsbFlag);
            }

            // Then
            Assert.AreEqual(expectedMsg, actualXmlMsg, "esb xml message");
        }
        
        [TestMethod]
        public void PrimeAffinityPsgBuilder_SendMessagesToEsb_WhenMultipleNonGpmNonPrimeItems_MessageForMultipleRecordsHasExpectedXml()
        {
            //Given
            var mockEsbProducer = new Mock<IEsbProducer>();
            mockEsbProducer.Setup(p => p.Send(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
              .Callback<string, string, Dictionary<string, string>>(
                  ((message, messageId, messageProperties) =>
                  {
                      this.actualXmlMsg = message;
                  }));

            var expectedMsg = File.ReadAllText("ExpectedTestMessage_MA_10181_2NonPrime.xml");
            var primeAffinityModels = new List<PrimeAffinityPsgModel>
            {
                testData.Item_MA_10181_NonPrime_A,
                testData.Item_MA_10181_NonPrime_B,
            };

            // When
            using (var mammothSqlConnection = new SqlConnection(mammothConnectionString))
            {
                PrimeAffinityPsgBuilder.SendMessagesToEsb(primeAffinityModels, mockEsbProducer.Object, df_nonReceivingSystems, df_maxNumberOfRows,
                    primeAffinityPSG_id, primeAffinityPSG_name, primeAffinityPSG_type, df_saveMessagesFlag, df_saveMessagesDir, df_sendToEsbFlag);
            }

            // Then
            Assert.AreEqual(expectedMsg, actualXmlMsg, "esb xml message");
        }

        [TestMethod]
        public void PrimeAffinityPsgBuilder_SendMessagesToEsb_WhenMultipleNonGpmPrimeItems_MessageForMultipleRecordsHasExpectedXml()
        {
            //Given
            var mockEsbProducer = new Mock<IEsbProducer>();
            mockEsbProducer.Setup(p => p.Send(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
              .Callback<string, string, Dictionary<string, string>>(
                  ((message, messageId, messageProperties) =>
                  {
                      this.actualXmlMsg = message;
                  }));

            var expectedMsg = File.ReadAllText("ExpectedTestMessage_MA_10181_2Prime.xml");
            var primeAffinityModels = new List<PrimeAffinityPsgModel>
            {
                testData.Item_MA_10181_Prime_C,
                testData.Item_MA_10181_Prime_D
            };

            // When
            using (var mammothSqlConnection = new SqlConnection(mammothConnectionString))
            {
                PrimeAffinityPsgBuilder.SendMessagesToEsb(primeAffinityModels, mockEsbProducer.Object, df_nonReceivingSystems, df_maxNumberOfRows,
                    primeAffinityPSG_id, primeAffinityPSG_name, primeAffinityPSG_type, df_saveMessagesFlag, df_saveMessagesDir, df_sendToEsbFlag);
            }

            // Then
            Assert.AreEqual(expectedMsg, actualXmlMsg, "esb xml message");
        }

        [TestMethod]
        public void PrimeAffinityPsgBuilder_SendMessagesToEsb_WhenMultipleGpmNonPrimeItems_MessageForMultipleRecordsHasExpectedXmll()
        {
            //Given
            var mockEsbProducer = new Mock<IEsbProducer>();
            mockEsbProducer.Setup(p => p.Send(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
              .Callback<string, string, Dictionary<string, string>>(
                  ((message, messageId, messageProperties) =>
                  {
                      this.actualXmlMsg = message;
                  }));

            var expectedMsg = File.ReadAllText("ExpectedTestMessage_FL_10130_2NonPrime.xml");
            var primeAffinityModels = new List<PrimeAffinityPsgModel>
            {
                testData.Item_FL_10130_NonPrime_A,
                testData.Item_FL_10130_NonPrime_B
            };

            // When
            using (var mammothSqlConnection = new SqlConnection(mammothConnectionString))
            {
                PrimeAffinityPsgBuilder.SendMessagesToEsb(primeAffinityModels, mockEsbProducer.Object, df_nonReceivingSystems, df_maxNumberOfRows,
                    primeAffinityPSG_id, primeAffinityPSG_name, primeAffinityPSG_type, df_saveMessagesFlag, df_saveMessagesDir, df_sendToEsbFlag);
            }

            // Then
            Assert.AreEqual(expectedMsg, actualXmlMsg, "esb xml message");
        }

        [TestMethod]
        public void PrimeAffinityPsgBuilder_SendMessagesToEsb_WhenMultipleGpmPrimeItems_MessageForMultipleRecordsHasExpectedXml()
        {
            //Given
            var mockEsbProducer = new Mock<IEsbProducer>();
            mockEsbProducer.Setup(p => p.Send(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
              .Callback<string, string, Dictionary<string, string>>(
                  ((message, messageId, messageProperties) =>
                  {
                      this.actualXmlMsg = message;
                  }));

            var expectedMsg = File.ReadAllText("ExpectedTestMessage_FL_10130_2Prime.xml");
            var primeAffinityModels = new List<PrimeAffinityPsgModel>
            {
                testData.Item_FL_10130_Prime_C,
                testData.Item_FL_10130_Prime_D
            };

            // When
            using (var mammothSqlConnection = new SqlConnection(mammothConnectionString))
            {
                PrimeAffinityPsgBuilder.SendMessagesToEsb(primeAffinityModels, mockEsbProducer.Object, df_nonReceivingSystems, df_maxNumberOfRows,
                    primeAffinityPSG_id, primeAffinityPSG_name, primeAffinityPSG_type, df_saveMessagesFlag, df_saveMessagesDir, df_sendToEsbFlag);
            }

            // Then
            Assert.AreEqual(expectedMsg, actualXmlMsg, "esb xml message");
        }

        [TestMethod]
        public void PrimeAffinityPsgBuilder_SendMessagesToEsb_WhenMultipleNonGpmPrimeAndNonPrimeItems_MessageForMultipleRecordsHasExpectedXml()
        {
            //Given
            var mockEsbProducer = new Mock<IEsbProducer>();
            mockEsbProducer.Setup(p => p.Send(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
              .Callback<string, string, Dictionary<string, string>>(
                  ((message, messageId, messageProperties) =>
                  {
                      this.actualXmlMsg = message;
                  }));

            var expectedMsg = File.ReadAllText("ExpectedTestMessage_MA_10181_2Prime2Non.xml");
            var primeAffinityModels = new List<PrimeAffinityPsgModel>
            {
                testData.Item_MA_10181_NonPrime_A,
                testData.Item_MA_10181_NonPrime_B,
                testData.Item_MA_10181_Prime_C,
                testData.Item_MA_10181_Prime_D
            };

            // When
            using (var mammothSqlConnection = new SqlConnection(mammothConnectionString))
            {
                PrimeAffinityPsgBuilder.SendMessagesToEsb(primeAffinityModels, mockEsbProducer.Object, df_nonReceivingSystems, df_maxNumberOfRows,
                    primeAffinityPSG_id, primeAffinityPSG_name, primeAffinityPSG_type, df_saveMessagesFlag, df_saveMessagesDir, df_sendToEsbFlag);
            }

            // Then
            Assert.AreEqual(expectedMsg, actualXmlMsg, "esb xml message");
        }

        [TestMethod]
        public void PrimeAffinityPsgBuilder_SendMessagesToEsb_WhenMultipleGpmPrimeAndNonPrimeItems_MessageForMultipleRecordsHasExpectedXml()
        {
            //Given
            var mockEsbProducer = new Mock<IEsbProducer>();
            mockEsbProducer.Setup(p => p.Send(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
              .Callback<string, string, Dictionary<string, string>>(
                  ((message, messageId, messageProperties) =>
                  {
                      this.actualXmlMsg = message;
                  }));

            var expectedMsg = File.ReadAllText("ExpectedTestMessage_FL_10130_2Prime2Non.xml");
            var primeAffinityModels = new List<PrimeAffinityPsgModel>
            {
                testData.Item_FL_10130_Prime_C,
                testData.Item_FL_10130_Prime_D,
                testData.Item_FL_10130_NonPrime_A,
                testData.Item_FL_10130_NonPrime_B
            };

            // When
            using (var mammothSqlConnection = new SqlConnection(mammothConnectionString))
            {
                PrimeAffinityPsgBuilder.SendMessagesToEsb(primeAffinityModels, mockEsbProducer.Object, df_nonReceivingSystems, df_maxNumberOfRows,
                    primeAffinityPSG_id, primeAffinityPSG_name, primeAffinityPSG_type, df_saveMessagesFlag, df_saveMessagesDir, df_sendToEsbFlag);
            }

            // Then
            Assert.AreEqual(expectedMsg, actualXmlMsg, "esb xml message");
        }

        [TestMethod]
        public void PrimeAffinityPsgBuilder_SendMessagesToEsb_MaxRecordsLimitsData()
        {
            // Given
            int maxNumberOfRows = 2;
            var primeAffinityModels = new List<PrimeAffinityPsgModel>
            {
                testData.Item_FL_10130_NonPrime_A,
                testData.Item_FL_10130_NonPrime_B,
                testData.Item_FL_10130_Prime_C,
                testData.Item_FL_10130_Prime_D
            };

            // When
            PrimeAffinityPsgBuilder.SendMessagesToEsb(primeAffinityModels, mockEsbProducer.Object, df_nonReceivingSystems, maxNumberOfRows,
                primeAffinityPSG_id, primeAffinityPSG_name, primeAffinityPSG_type, df_saveMessagesFlag, df_saveMessagesDir, df_sendToEsbFlag);

            // Then
            Assert.AreEqual(1, PrimeAffinityPsgBuilder.NumberOfMessagesSent);
            Assert.AreEqual(maxNumberOfRows, PrimeAffinityPsgBuilder.NumberOfRecordsSent);
        }

        [TestMethod]
        public void PrimeAffinityPsgBuilder_SendMessagesToEsb_MaxRecordsOfZeroMeansAll()
        {
            // Given
            int maxNumberOfRows = 0;
            var mockEsbProducer = new Mock<IEsbProducer>();

            var primeAffinityModels = new List<PrimeAffinityPsgModel>
            {
                testData.Item_MA_10181_NonPrime_A,
                testData.Item_MA_10181_NonPrime_B,
                testData.Item_MA_10181_Prime_C,
                testData.Item_MA_10181_Prime_D
            };

            // When
            PrimeAffinityPsgBuilder.SendMessagesToEsb(primeAffinityModels, mockEsbProducer.Object, df_nonReceivingSystems, maxNumberOfRows,
                primeAffinityPSG_id, primeAffinityPSG_name, primeAffinityPSG_type, df_saveMessagesFlag, df_saveMessagesDir, df_sendToEsbFlag);

            // Then
            Assert.AreEqual(1, PrimeAffinityPsgBuilder.NumberOfMessagesSent);
            Assert.AreEqual(4, PrimeAffinityPsgBuilder.NumberOfRecordsSent);
        }

        [TestMethod]
        public void PrimeAffinityPsgBuilder_SendMessagesToEsb_SendsExpectedMsgPropNonReceivingSys()
        {
            // Given
            string nonReceivingSysName = "non receiving systems test";
            var mockEsbProducer = new Mock<IEsbProducer>();

            var primeAffinityModels = new List<PrimeAffinityPsgModel>
            {
                testData.Item_MA_10181_NonPrime_A,
            };

            // When
            using (var mammothSqlConnection = new SqlConnection(mammothConnectionString))
            {
                PrimeAffinityPsgBuilder.SendMessagesToEsb(primeAffinityModels, mockEsbProducer.Object, nonReceivingSysName, df_maxNumberOfRows,
                    primeAffinityPSG_id, primeAffinityPSG_name, primeAffinityPSG_type, df_saveMessagesFlag, df_saveMessagesDir, df_sendToEsbFlag);

                // Then
                mockEsbProducer.Verify(p =>
                    p.Send(
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.Is<Dictionary<string, string>>(dict => dict["nonReceivingSysName"] == nonReceivingSysName)
                    ), Times.Once);
            }
        }
    }
}
