using Icon.Dashboard.Mvc.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Dashboard.Mvc.UnitTests.ModelUnitTests
{
    [TestClass]
    public class ApplicationDatabaseConfigurationModelUnitTests
    {
        [TestMethod]
        public void Constructor_InitializesConnectionsCollection()
        {
            // Arrange, Act
            var dbConfig = new ApplicationDatabaseConfiguration();

            // Assert
            Assert.IsNotNull(dbConfig.Connections);
            Assert.AreEqual(0, dbConfig.Connections.Count);
        }

        [TestMethod]
        public void LoggingSummary_WhenNoEntryForLoggingDb_ReturnsNone()
        {
            // Arrange
            var dbConfig = new ApplicationDatabaseConfiguration();
            Assert.AreEqual(0, dbConfig.Connections.Count);

            // Act
            var loggingSummary = dbConfig.LoggingSummary;

            // Assert
            Assert.AreEqual("None", loggingSummary);
        }

        [TestMethod]
        public void LoggingSummary_WhenIconLoggingDb_ReturnsDbNameAndCategory()
        {
            // Arrange
            var dbConfig = new ApplicationDatabaseConfiguration();
            var loggingDb = CreateFakeLoggingDbModelIcon();
            dbConfig.Connections.Add(loggingDb);

            // Act
            var loggingSummary = dbConfig.LoggingSummary;

            // Assert
            Assert.AreEqual("Icon-Dev", loggingSummary);
        }

        [TestMethod]
        public void LoggingSummary_WhenMammothLoggingDb_ReturnsDbNameAndCategory()
        {
            // Arrange
            var dbConfig = new ApplicationDatabaseConfiguration();
            var loggingDb = CreateFakeLoggingDbModelMammoth();
            dbConfig.Connections.Add(loggingDb);

            // Act
            var loggingSummary = dbConfig.LoggingSummary;

            // Assert
            Assert.AreEqual("Mammoth-Test", loggingSummary);
        }

        [TestMethod]
        public void LoggingConnection_WhenNoEntryForLoggingDb_IsNull()
        {
            // Arrange
            var dbConfig = new ApplicationDatabaseConfiguration();

            // Act
            var loggingDb = dbConfig.LoggingConnection;

            // Assert
            Assert.IsNull(loggingDb);
        }

        [TestMethod]
        public void Summary_WhenNoDb_ReturnsNone()
        {
            // Arrange
            var expectedSummary = "None";
            var dbConfig = new ApplicationDatabaseConfiguration();

            // Act
            var actualSummary = dbConfig.Summary;

            // Assert
            Assert.AreEqual(expectedSummary, actualSummary);
        }

        [TestMethod]
        public void Summary_WhenOnlyLoggingDb_ReturnsNone()
        {
            // Arrange
            var expectedSummary = "None";
            var dbConfig = new ApplicationDatabaseConfiguration();
            var loggingDb = CreateFakeLoggingDbModelIcon();
            dbConfig.Connections.Add(loggingDb);

            // Act
            var actualSummary = dbConfig.Summary;

            // Assert
            Assert.AreEqual(expectedSummary, actualSummary);
        }

        [TestMethod]
        public void Summary_WhenIconDbInDev_ReturnsCategoryAndEnvironemnt()
        {
            // Arrange
            var expectedSummary = "Icon-Dev";
            var dbConfig = new ApplicationDatabaseConfiguration();
            var iconDb = CreateFakeIconDbModel(isEntityFramework: true);
            dbConfig.Connections.Add(iconDb);

            // Act
            var actualSummary = dbConfig.Summary;

            // Assert
            Assert.AreEqual(expectedSummary, actualSummary);
        }

        [TestMethod]
        public void Summary_WhenIconDbInDevWithLogging_IgnoresLoggingDb()
        {
            // Arrange
            var expectedSummary = "Icon-QA";
            var dbConfig = new ApplicationDatabaseConfiguration();
            var iconDb = CreateFakeIconDbModel(isEntityFramework: true);
            iconDb.Environment = EnvironmentEnum.QA;
            dbConfig.Connections.Add(iconDb);
            var loggingDb = CreateFakeLoggingDbModelIcon();
            loggingDb.Environment = EnvironmentEnum.Perf;
            dbConfig.Connections.Add(loggingDb);

            // Act
            var actualSummary = dbConfig.Summary;

            // Assert
            Assert.AreEqual(expectedSummary, actualSummary);
        }

        [TestMethod]
        public void Summary_WhenIrmaDbsInDevAndTest_ReturnsCategoryAndEnvironemnt()
        {
            // Arrange
            var expectedSummary = "IRMA-Dev/Test";
            var dbConfig = new ApplicationDatabaseConfiguration();
            // following creates mix of Dev and Test environment objects
            var irmaDbs = CreateFakeIrmaDbModels();
            dbConfig.Connections.AddRange(irmaDbs);

            // Act
            var actualSummary = dbConfig.Summary;

            // Assert
            Assert.AreEqual(expectedSummary, actualSummary);
        }

        [TestMethod]
        public void Summary_WhenIrmaDbsInQa_ReturnsCategoryAndEnvironemnt()
        {
            // Arrange
            var expectedSummary = "IRMA-QA";
            var dbConfig = new ApplicationDatabaseConfiguration();
            var irmaDbs = CreateFakeIrmaDbModels();
            foreach(var irmaDb in irmaDbs)
            {
                irmaDb.Environment = EnvironmentEnum.QA;
            }
            dbConfig.Connections.AddRange(irmaDbs);

            // Act
            var actualSummary = dbConfig.Summary;

            // Assert
            Assert.AreEqual(expectedSummary, actualSummary);
        }

        [TestMethod]
        public void Summary_WhenMammothDbInTest_ReturnsCategoryAndEnvironemnt()
        {
            // Arrange
            var expectedSummary = "Mammoth-Test";
            var dbConfig = new ApplicationDatabaseConfiguration();
            var mammothDb = CreateFakeMammothbModel();
            dbConfig.Connections.Add(mammothDb);

            // Act
            var actualSummary = dbConfig.Summary;

            // Assert
            Assert.AreEqual(expectedSummary, actualSummary);
        }

        [TestMethod]
        public void HasNonLoggingConnectionOfCategory_WhenIsEntryForIconDb_ReturnsTrueForIconType()
        {
            // Arrange
            var expected = true;
            var dbConfig = new ApplicationDatabaseConfiguration();
            var iconDb = CreateFakeIconDbModel(isEntityFramework: true);
            dbConfig.Connections.Add(iconDb);

            // Act
            var actual = dbConfig.HasNonLoggingConnectionOfCategory(Models.DatabaseCategoryEnum.Icon);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void HasNonLoggingConnectionOfCategory_WhenIsEntryForIconDbAndLogging_ReturnsTrueForIconType()
        {
            // Arrange
            var expected = true;
            var dbConfig = new ApplicationDatabaseConfiguration();
            var iconDb = CreateFakeIconDbModel(isEntityFramework: true);
            dbConfig.Connections.Add(iconDb);
            var loggingDb = CreateFakeLoggingDbModelIcon();
            dbConfig.Connections.Add(loggingDb);

            // Act
            var actual = dbConfig.HasNonLoggingConnectionOfCategory(Models.DatabaseCategoryEnum.Icon);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void HasNonLoggingConnectionOfCategory_WhenIsEntryOnlyForLogging_ReturnsFalseForIconType()
        {
            // Arrange
            var expected = false;
            var dbConfig = new ApplicationDatabaseConfiguration();
            var loggingDb = CreateFakeLoggingDbModelIcon();
            dbConfig.Connections.Add(loggingDb);

            // Act
            var actual = dbConfig.HasNonLoggingConnectionOfCategory(Models.DatabaseCategoryEnum.Icon);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void HasNonLoggingConnectionOfCategory_WhenIsEntryForIconDb_ReturnsFalseForIrmaType()
        {
            // Arrange
            var expected = false;
            var dbConfig = new ApplicationDatabaseConfiguration();
            var iconDb = CreateFakeIconDbModel(isEntityFramework: true);
            dbConfig.Connections.Add(iconDb);

            // Act
            var actual = dbConfig.HasNonLoggingConnectionOfCategory(Models.DatabaseCategoryEnum.IRMA);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void HasNonLoggingConnectionOfCategory_WhenIsEntryForMammothAndIrmaDb_ReturnsTrueForMammothAndIrmaType()
        {
            // Arrange
            var expectedIrma = true;
            var expectedMammoth = true;
            var dbConfig = new ApplicationDatabaseConfiguration();
            var mammothDb = CreateFakeMammothbModel();
            var irmaDbs = CreateFakeIrmaDbModels();
            dbConfig.Connections.Add(mammothDb);
            dbConfig.Connections.AddRange(irmaDbs);

            // Act
            var actualIrma = dbConfig.HasNonLoggingConnectionOfCategory(Models.DatabaseCategoryEnum.IRMA);
            var actualMammoth = dbConfig.HasNonLoggingConnectionOfCategory(Models.DatabaseCategoryEnum.Mammoth);

            // Assert
            Assert.AreEqual(expectedIrma, actualIrma);
            Assert.AreEqual(expectedMammoth, actualMammoth);
        }

        [TestMethod]
        public void HasNonLoggingConnectionOfCategory_WhenIsEntryForMammothAndIrmaDb_ReturnsFalseForIconType()
        {
            // Arrange
            var expectedIcon = false;
            var dbConfig = new ApplicationDatabaseConfiguration();
            var mammothDb = CreateFakeMammothbModel();
            var irmaDbs = CreateFakeIrmaDbModels();
            dbConfig.Connections.Add(mammothDb);
            dbConfig.Connections.AddRange(irmaDbs);
            // Arrange

            // Act
            var actualIcon = dbConfig.HasNonLoggingConnectionOfCategory(Models.DatabaseCategoryEnum.Icon);

            // Assert
            Assert.AreEqual(expectedIcon, actualIcon);
        }

        [TestMethod]
        public void GetSingleNonLoggingConnectionOfCategory_WhenNoIconDb_ReturnsNull()
        {
            // Arrange
            var dbConfig = new ApplicationDatabaseConfiguration();

            // Act
            var connection = dbConfig.GetSingleNonLoggingConnectionOfCategory(DatabaseCategoryEnum.Icon);

            // Assert
            Assert.IsNull(connection);
        }

        [TestMethod]
        public void GetSingleNonLoggingConnectionOfCategory_WhenIsIconDb_ReturnsIconDbWithMatchingProperties()
        {
            // Arrange
            var dbConfig = new ApplicationDatabaseConfiguration();
            var iconDb = CreateFakeIconDbModel(isEntityFramework: true);
            dbConfig.Connections.Add(iconDb);

            // Act
            var connection = dbConfig.GetSingleNonLoggingConnectionOfCategory(DatabaseCategoryEnum.Icon);

            // Assert
            Assert.IsNotNull(connection);
            Assert.AreEqual(iconDb.DatabaseName, connection.DatabaseName);
            Assert.AreEqual(iconDb.Environment, connection.Environment);
            Assert.AreEqual(iconDb.Category, connection.Category);
            Assert.AreEqual(iconDb.ServerName, connection.ServerName);
            Assert.AreEqual(iconDb.IsEntityFramework, connection.IsEntityFramework);
            Assert.AreEqual(iconDb.IsUsedForLogging, connection.IsUsedForLogging);
        }

        [TestMethod]
        public void GetSingleNonLoggingConnectionOfCategory_WhenIsIconDbWithLogging_ReturnsIconDb()
        {
            // Arrange
            var dbConfig = new ApplicationDatabaseConfiguration();
            var iconDb = CreateFakeIconDbModel(isEntityFramework: true);
            dbConfig.Connections.Add(iconDb);
            var loggingDb = CreateFakeLoggingDbModelIcon();
            dbConfig.Connections.Add(loggingDb);

            // Act
            var connection = dbConfig.GetSingleNonLoggingConnectionOfCategory(DatabaseCategoryEnum.Icon);

            // Assert
            Assert.IsNotNull(connection);
            Assert.IsFalse(connection.IsUsedForLogging);
            Assert.AreEqual(DatabaseCategoryEnum.Icon, connection.Category);
        }

        [TestMethod]
        public void GetSingleNonLoggingConnectionOfCategory_WhenOnlyLoggingDb_ReturnsNull()
        {
            // Arrange
            var dbConfig = new ApplicationDatabaseConfiguration();
            var loggingDb = CreateFakeLoggingDbModelIcon();
            dbConfig.Connections.Add(loggingDb);

            // Act
            var connection = dbConfig.GetSingleNonLoggingConnectionOfCategory(DatabaseCategoryEnum.Icon);

            // Assert
            Assert.IsNull(connection);
        }

        [TestMethod]
        public void GetSingleNonLoggingConnectionOfCategory_WhenAreIrmaDbsAndMammothWithLogging_ReturnsMammothConnection()
        {
            // Arrange
            var dbConfig = new ApplicationDatabaseConfiguration();
            var mammothDb = CreateFakeMammothbModel();
            dbConfig.Connections.Add(mammothDb);
            var irmaDbs = CreateFakeIrmaDbModels();
            dbConfig.Connections.AddRange(irmaDbs);
            var loggingDb = CreateFakeLoggingDbModelMammoth();
            dbConfig.Connections.Add(loggingDb);

            // Act
            var connection = dbConfig.GetSingleNonLoggingConnectionOfCategory(DatabaseCategoryEnum.Mammoth);

            // Assert
            Assert.IsNotNull(connection);
            Assert.AreEqual(mammothDb.DatabaseName, connection.DatabaseName);
            Assert.AreEqual(mammothDb.Environment, connection.Environment);
            Assert.AreEqual(mammothDb.Category, connection.Category);
            Assert.AreEqual(mammothDb.ServerName, connection.ServerName);
            Assert.AreEqual(mammothDb.IsEntityFramework, connection.IsEntityFramework);
            Assert.AreEqual(mammothDb.IsUsedForLogging, connection.IsUsedForLogging);
        }

        [TestMethod]
        public void GetSingleNonLoggingConnectionOfCategory_WhenAreIrmaDbsAndMammothWithLogging_ReturnsOneIrmaDb()
        {
            // Arrange
            var dbConfig = new ApplicationDatabaseConfiguration();
            var mammothDb = CreateFakeMammothbModel();
            dbConfig.Connections.Add(mammothDb);
            var irmaDbs = CreateFakeIrmaDbModels();
            dbConfig.Connections.AddRange(irmaDbs);
            var loggingDb = CreateFakeLoggingDbModelMammoth();
            dbConfig.Connections.Add(loggingDb);

            // Act
            var connection = dbConfig.GetSingleNonLoggingConnectionOfCategory(DatabaseCategoryEnum.IRMA);

            // Assert
            Assert.IsNotNull(connection);
            Assert.IsFalse(connection.IsUsedForLogging);
            Assert.AreEqual(DatabaseCategoryEnum.IRMA, connection.Category);
        }

        [TestMethod]
        public void GetAllNonLoggingConnectionsOfCategory_WhenNoIrmaDb_ReturnsEmptyList()
        {
            // Arrange
            var dbConfig = new ApplicationDatabaseConfiguration();

            // Act
            var connections = dbConfig.GetAllNonLoggingConnectionsOfCategory(DatabaseCategoryEnum.IRMA);

            // Assert
            Assert.IsNotNull(connections);
            Assert.AreEqual(0, connections.Count);
        }

        [TestMethod]
        public void GetAllNonLoggingConnectionsOfCategory_WhenIsIrmaDb_ReturnsExpectedConnections()
        {
            // Arrange
            var dbConfig = new ApplicationDatabaseConfiguration();
            var irmaDbs = CreateFakeIrmaDbModels();
            dbConfig.Connections.AddRange(irmaDbs);

            // Act
            var connections = dbConfig.GetAllNonLoggingConnectionsOfCategory(DatabaseCategoryEnum.IRMA);

            // Assert
            Assert.IsNotNull(connections);
            Assert.AreEqual(irmaDbs.Count, connections.Count);
        }

        private DatabaseDefinition CreateFakeLoggingDbModelIcon(bool isEntityFramework = false)
        {
            var loggingDb = new DatabaseDefinition
            {
                ServerName = @"server_X",
                DatabaseName = "myLoggingDb",
                ConnectionStringName = "dbLog",
                Environment = Models.EnvironmentEnum.Dev,
                IsEntityFramework = isEntityFramework,
                Category = Models.DatabaseCategoryEnum.Icon,
                IsUsedForLogging = true
            };
            return loggingDb;
        }

        private DatabaseDefinition CreateFakeLoggingDbModelMammoth(bool isEntityFramework = false)
        {
            var loggingDb = new DatabaseDefinition
            {
                ServerName = @"MAMMOTH-SERVER\MAMMOTH",
                DatabaseName = "mammothDb",
                ConnectionStringName = "dbLogMammoth",
                Environment = Models.EnvironmentEnum.Test,
                IsEntityFramework = isEntityFramework,
                Category = Models.DatabaseCategoryEnum.Mammoth,
                IsUsedForLogging = true
            };
            return loggingDb;
        }

        private DatabaseDefinition CreateFakeMammothbModel(bool isEntityFramework = false)
        {
            var mammothDb = new DatabaseDefinition
            {
                ServerName = @"MAMMOTH-SERVER\MAMMOTH",
                DatabaseName = "mammothDb",
                ConnectionStringName = isEntityFramework ? "MammothContext" : "Mammoth",
                Environment = Models.EnvironmentEnum.Test,
                IsEntityFramework = isEntityFramework,
                Category = Models.DatabaseCategoryEnum.Mammoth,
                IsUsedForLogging = false
            };
            return mammothDb;
        }

        private DatabaseDefinition CreateFakeIconDbModel(bool isEntityFramework = false)
        {
            var iconDb = new DatabaseDefinition
            {
                ServerName = @"icon-server\icon",
                DatabaseName = "iconDb",
                ConnectionStringName = isEntityFramework ? "IconContext" : "Icon",
                Environment = Models.EnvironmentEnum.Dev,
                IsEntityFramework = isEntityFramework,
                Category = Models.DatabaseCategoryEnum.Icon,
                IsUsedForLogging = false
            };
            return iconDb;
        }

        private List<DatabaseDefinition> CreateFakeIrmaDbModels(bool isEntityFramework = false)
        {
            var irmaDbs = new List<DatabaseDefinition>
            {
                new DatabaseDefinition
                {
                    ServerName = @"irma-fl-server\fld",
                    DatabaseName = "fakeItemCatalog",
                    ConnectionStringName = "ItemCatalog_FL",
                    Environment = Models.EnvironmentEnum.Dev,
                    IsEntityFramework = isEntityFramework,
                    Category = Models.DatabaseCategoryEnum.IRMA,
                    IsUsedForLogging = false
                },
                new DatabaseDefinition
                {
                    ServerName = @"irma-ma-server\mad",
                    DatabaseName = "fakeItemCatalog",
                    ConnectionStringName = "ItemCatalog_MA",
                    Environment = Models.EnvironmentEnum.Dev,
                    IsEntityFramework = isEntityFramework,
                    Category = Models.DatabaseCategoryEnum.IRMA,
                    IsUsedForLogging = false
                },
                new DatabaseDefinition
                {
                    ServerName = @"irma-nc-server\nct",
                    DatabaseName = "fakeItemCatalog",
                    ConnectionStringName = "ItemCatalog_NC",
                    Environment = Models.EnvironmentEnum.Test,
                    IsEntityFramework = isEntityFramework,
                    Category = Models.DatabaseCategoryEnum.IRMA,
                    IsUsedForLogging = false
                },
                new DatabaseDefinition
                {
                    ServerName = @"irma-sw-server\swt",
                    DatabaseName = "fakeItemCatalog",
                    ConnectionStringName = "ItemCatalog_SW",
                    Environment = Models.EnvironmentEnum.Test,
                    IsEntityFramework = isEntityFramework,
                    Category = Models.DatabaseCategoryEnum.IRMA,
                    IsUsedForLogging = false
                },
            };
            return irmaDbs;
        }
    }
}
