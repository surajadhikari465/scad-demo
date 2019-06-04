using Icon.Dashboard.Mvc.Models;
using Icon.Dashboard.Mvc.Services;
using Icon.Dashboard.Mvc.ViewModels;
using Icon.Dashboard.RemoteServicesAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Dashboard.Mvc.UnitTests.ServiceTests
{
    [TestClass]
    public class RemoteWmiServiceWrapperUnitTests
    {
        TestData testData = new TestData();

        Mock<IRemoteWmiAccessService> mockWMiSvc = new Mock<IRemoteWmiAccessService>();
        Mock<IIconDatabaseServiceWrapper> mockIconDbService = new Mock<IIconDatabaseServiceWrapper>();
        Mock<IMammothDatabaseServiceWrapper> mockMammothDbService = new Mock<IMammothDatabaseServiceWrapper>();
        Mock<IEsbEnvironmentManager> mockEsbEnvironmentManager = new Mock<IEsbEnvironmentManager>();


        [TestMethod]
        public void CreateViewModel_WhenReturnsGloconServiceObject_PopulatesViewModelWithExpectedProperties()
        {
            // Arrange
            string server = "vm-test1";
            string application = "GlobalEventControllerService";
            bool commandsEnabled = true;

            mockIconDbService.Setup(s => s.GetApps()).Returns(testData.IconApps);
            mockMammothDbService.Setup(s => s.GetApps()).Returns(testData.MammothApps);
            mockEsbEnvironmentManager.Setup(s => s.GetEsbEnvironmentDefinitions()).Returns(testData.EsbEnvironments);
            mockWMiSvc.Setup(s => s.LoadRemoteService(server, application)) .Returns(testData.SampleGloconService);

            var wmiServiceWrapper = new RemoteWmiServiceWrapper(true, mockWMiSvc.Object,
                mockIconDbService.Object, mockMammothDbService.Object, mockEsbEnvironmentManager.Object);

            // Act
            var actualViewModel = wmiServiceWrapper.CreateViewModel(server, testData.SampleGloconService, commandsEnabled);

            // Assert
            Assert.AreEqual(testData.SampleGloconService.FullName, actualViewModel.Name);
            Assert.AreEqual(testData.SampleGloconService.DisplayName, actualViewModel.DisplayName);
            Assert.AreEqual(server, actualViewModel.Server);
            Assert.AreEqual(@"SampleAppConfig_A.exe.config", actualViewModel.ConfigFilePath);
            Assert.AreEqual(commandsEnabled, actualViewModel.CommandsEnabled);
            Assert.AreEqual(testData.SampleGloconService.State, actualViewModel.Status);
            Assert.AreEqual(1, actualViewModel.ValidCommands.Count);
            Assert.AreEqual("Stop", actualViewModel.ValidCommands[0]);
            Assert.AreEqual(true, actualViewModel.StatusIsGreen);
            Assert.AreEqual(testData.SampleGloconService.SystemName, actualViewModel.HostName);
            Assert.AreEqual("IconTestUserDev", actualViewModel.AccountName);
            //from sample config file
            Assert.AreEqual(true, actualViewModel.ConfigFilePathIsValid);
            Assert.AreEqual(48, actualViewModel.AppSettings.Count);
            Assert.AreEqual(0, actualViewModel.EsbConnectionSettings.Count);
            Assert.AreEqual(7, actualViewModel.LoggingID);
            Assert.AreEqual("Global Controller", actualViewModel.LoggingName);
        }

        [TestMethod]
        public void CreateViewModel_WhenMammothDatabaseDisabled_ReturnsBlankLoggingName()
        {
            // Arrange
            string server = "vm-test1";
            string application = "Mammoth.ItemLocale.Controller$MA";
            bool commandsEnabled = true;

            mockIconDbService.Setup(s => s.GetApps()).Returns(testData.IconApps);
            mockEsbEnvironmentManager.Setup(s => s.GetEsbEnvironmentDefinitions())
                .Returns(testData.EsbEnvironments);
            mockWMiSvc.Setup(s => s.LoadRemoteService(server, application))
                .Returns(testData.SampleMammothItemLocaleControllerMAService);

            var wmiServiceWrapper = new RemoteWmiServiceWrapper(false, mockWMiSvc.Object,
                mockIconDbService.Object, null, mockEsbEnvironmentManager.Object);

            // Act
            var actualViewModel = wmiServiceWrapper.CreateViewModel(server, testData.SampleMammothItemLocaleControllerMAService, commandsEnabled);

            // Assert
            Assert.AreEqual("", actualViewModel.LoggingName);
        }

        [TestMethod]
        public void GetConfigUncPath_WhenReturnsApiControllerRemoteServiceObject_PopulatesViewModelWithExpectedProperties()
        {
            // Arrange
            string server = "vm-test1";
            string pathName = @"""E:\Icon\API Controller Phase 2\Hierarchy\Icon.ApiController.Controller.exe""  -displayname ""Icon API Controller - Hierarchy"" -servicename ""IconAPIController-Hierarchy""";
            string expectedUncPath = @"\\vm-test1\E$\Icon\API Controller Phase 2\Hierarchy\Icon.ApiController.Controller.exe.config";
            
            var wmiServiceWrapper = new RemoteWmiServiceWrapper(true, mockWMiSvc.Object,
                mockIconDbService.Object, mockMammothDbService.Object, mockEsbEnvironmentManager.Object);

            // Act
            var uncPath = wmiServiceWrapper.GetConfigUncPath(server, pathName);

            // Assert
            Assert.AreEqual(expectedUncPath, uncPath);
        }

        [TestMethod]
        public void DetermineDatabaseEnvironment_WhenIconDev_ReturnDevEnvironment()
        {
            // Arrange
            var expectedEnvironment = EnvironmentEnum.Dev;
            var wmiServiceWrapper = new RemoteWmiServiceWrapper(true, mockWMiSvc.Object,
                mockIconDbService.Object, mockMammothDbService.Object, mockEsbEnvironmentManager.Object);
            // Act 
            var actualEnvironment = wmiServiceWrapper.DetermineDatabaseEnvironment(
                DatabaseCategoryEnum.Icon, @"cewd1815\SqlSHARED2012D", "IconDev");

            // Assert
            Assert.AreEqual(expectedEnvironment, actualEnvironment);
        }

        [TestMethod]
        public void DetermineDatabaseEnvironment_WhenIconDev_ReturnDevEnvironment_IgnoresCase()
        {
            // Arrange
            var expectedEnvironment = EnvironmentEnum.Dev;
            var wmiServiceWrapper = new RemoteWmiServiceWrapper(true, mockWMiSvc.Object,
                mockIconDbService.Object, mockMammothDbService.Object, mockEsbEnvironmentManager.Object);
            // Act 
            var actualEnvironment = wmiServiceWrapper.DetermineDatabaseEnvironment(
                DatabaseCategoryEnum.Icon, @"cewd1815\sqlshared2012d", "icondev");

            // Assert
            Assert.AreEqual(expectedEnvironment, actualEnvironment);
        }

        [TestMethod]
        public void DetermineDatabaseEnvironment_WhenIconPerf_ReturnPerfEnvironment()
        {
            // Arrange
            var expectedEnvironment = EnvironmentEnum.Perf;
            var wmiServiceWrapper = new RemoteWmiServiceWrapper(true, mockWMiSvc.Object,
                mockIconDbService.Object, mockMammothDbService.Object, mockEsbEnvironmentManager.Object);
            // Act 
            var actualEnvironment = wmiServiceWrapper.DetermineDatabaseEnvironment(
                DatabaseCategoryEnum.Icon, @"CEWD1815\SQLSHARED2012D", "iCONLoadTest");

            // Assert
            Assert.AreEqual(expectedEnvironment, actualEnvironment);
        }


        [TestMethod]
        public void DetermineDatabaseEnvironment_WhenMammothDev_ReturnsDevEnvironment()
        {
            // Arrange
            var expectedEnvironment = EnvironmentEnum.Dev;
            var wmiServiceWrapper = new RemoteWmiServiceWrapper(true, mockWMiSvc.Object,
                mockIconDbService.Object, mockMammothDbService.Object, mockEsbEnvironmentManager.Object);
            // Act 
            var actualEnvironment = wmiServiceWrapper.DetermineDatabaseEnvironment(
                DatabaseCategoryEnum.Mammoth, @"mammoth-db01-dev\mammoth", "Mammoth_Dev");

            // Assert
            Assert.AreEqual(expectedEnvironment, actualEnvironment);
        }

        [TestMethod]
        public void DetermineDatabaseEnvironment_WhenMammothTest_ReturnsTestEnvironment()
        {
            // Arrange
            var expectedEnvironment = EnvironmentEnum.Test;
            var wmiServiceWrapper = new RemoteWmiServiceWrapper(true, mockWMiSvc.Object,
                mockIconDbService.Object, mockMammothDbService.Object, mockEsbEnvironmentManager.Object);
            // Act 
            var actualEnvironment = wmiServiceWrapper.DetermineDatabaseEnvironment(
                DatabaseCategoryEnum.Mammoth, @"MAMMOTH-DB01-DEV\MAMMOTH", "Mammoth");

            // Assert
            Assert.AreEqual(expectedEnvironment, actualEnvironment);
        }

        [TestMethod]
        public void DetermineDatabaseEnvironment_WhenMammothTst1_ReturnsTst1Environment()
        {
            // Arrange
            var expectedEnvironment = EnvironmentEnum.Tst1;
            var wmiServiceWrapper = new RemoteWmiServiceWrapper(true, mockWMiSvc.Object,
                mockIconDbService.Object, mockMammothDbService.Object, mockEsbEnvironmentManager.Object);
            // Act 
            var actualEnvironment = wmiServiceWrapper.DetermineDatabaseEnvironment(
                DatabaseCategoryEnum.Mammoth, @"mammoth-db01-tst01", "Mammoth");

            // Assert
            Assert.AreEqual(expectedEnvironment, actualEnvironment);
        }

        [TestMethod]
        public void DetermineDatabaseEnvironment_WhenIrmaDev_ReturnsDevEnvironment()
        {
            // Arrange
            var expectedEnvironment = EnvironmentEnum.Dev;
            var wmiServiceWrapper = new RemoteWmiServiceWrapper(true, mockWMiSvc.Object,
                mockIconDbService.Object, mockMammothDbService.Object, mockEsbEnvironmentManager.Object);
            // Act 
            var actualEnvironment = wmiServiceWrapper.DetermineDatabaseEnvironment(
                DatabaseCategoryEnum.IRMA, @"idd-ma\mad", "ItemCatalog_Test");

            // Assert
            Assert.AreEqual(expectedEnvironment, actualEnvironment);
        }

        [TestMethod]
        public void DetermineDatabaseEnvironment_WhenIrmaTest_ReturnsTestEnvironment()
        {
            // Arrange
            var expectedEnvironment = EnvironmentEnum.Test;
            var wmiServiceWrapper = new RemoteWmiServiceWrapper(true, mockWMiSvc.Object,
                mockIconDbService.Object, mockMammothDbService.Object, mockEsbEnvironmentManager.Object);
            // Act 
            var actualEnvironment = wmiServiceWrapper.DetermineDatabaseEnvironment(
                DatabaseCategoryEnum.IRMA, @"idt-pn\pnt", "ItemCatalog_Test");

            // Assert
            Assert.AreEqual(expectedEnvironment, actualEnvironment);
        }

        [TestMethod]
        public void DetermineDatabaseEnvironment_WhenIrmaTst1_ReturnsTst1Environment()
        {
            // Arrange
            var expectedEnvironment = EnvironmentEnum.Tst1;
            var wmiServiceWrapper = new RemoteWmiServiceWrapper(true, mockWMiSvc.Object,
                mockIconDbService.Object, mockMammothDbService.Object, mockEsbEnvironmentManager.Object);
            // Act 
            var actualEnvironment = wmiServiceWrapper.DetermineDatabaseEnvironment(
                DatabaseCategoryEnum.IRMA, @"ma-db01-tst01", "ItemCatalog");

            // Assert
            Assert.AreEqual(expectedEnvironment, actualEnvironment);
        }

        [TestMethod]
        public void DetermineDatabaseEnvironment_WhenIrmaQA_ReturnsQAEnvironment()
        {
            // Arrange
            var expectedEnvironment = EnvironmentEnum.QA;
            var wmiServiceWrapper = new RemoteWmiServiceWrapper(true, mockWMiSvc.Object,
                mockIconDbService.Object, mockMammothDbService.Object, mockEsbEnvironmentManager.Object);
            // Act 
            var actualEnvironment = wmiServiceWrapper.DetermineDatabaseEnvironment(
                DatabaseCategoryEnum.IRMA, @"idq-fl\flq", "ItemCatalog");

            // Assert
            Assert.AreEqual(expectedEnvironment, actualEnvironment);
        }

        [TestMethod]
        public void DetermineDatabaseEnvironment_WhenVim_ReturnsUndefinedEnvironment()
        {
            // Arrange
            var expectedEnvironment = EnvironmentEnum.Undefined;
            var wmiServiceWrapper = new RemoteWmiServiceWrapper(true, mockWMiSvc.Object,
                mockIconDbService.Object, mockMammothDbService.Object, mockEsbEnvironmentManager.Object);
            // Act 
            var actualEnvironment = wmiServiceWrapper.DetermineDatabaseEnvironment(
                DatabaseCategoryEnum.Vim, @"((anyWeirdVimdb_string)(port5930))", "vim_vim_vim");

            // Assert
            Assert.AreEqual(expectedEnvironment, actualEnvironment);
        }

        [TestMethod]
        public void DetermineDatabaseEnvironment_WhenIcon2016_ReturnsDevEnvironment()
        {
            // Arrange
            var expectedEnvironment = EnvironmentEnum.Dev;
            var wmiServiceWrapper = new RemoteWmiServiceWrapper(true, mockWMiSvc.Object,
                mockIconDbService.Object, mockMammothDbService.Object, mockEsbEnvironmentManager.Object);
            // Act 
            var actualEnvironment = wmiServiceWrapper.DetermineDatabaseEnvironment(
                DatabaseCategoryEnum.Icon, @"sql-icon16-tst", "Icon2016");
            // Assert
            Assert.AreEqual(expectedEnvironment, actualEnvironment);
        }

        [TestMethod]
        public void ReadDatabaseConfiguration_WhenAppHasIconSqlWithIconLogging_ReadsDbConnection()
        {
            // Arrange
            var configPath = "SampleDbConfig_IconSqlWithIconLogging.config";
            var wmiServiceWrapper = new RemoteWmiServiceWrapper(true, mockWMiSvc.Object,
                mockIconDbService.Object, mockMammothDbService.Object, mockEsbEnvironmentManager.Object);
            // Act
            var dbConfig = wmiServiceWrapper.ReadDatabaseConfiguration(configPath);
            // Assert
            Assert.IsNotNull(dbConfig);
            Assert.IsTrue(dbConfig.HasNonLoggingConnectionOfCategory(DatabaseCategoryEnum.Icon));
            var iconConnection = dbConfig.GetSingleNonLoggingConnectionOfCategory(DatabaseCategoryEnum.Icon);
            Assert.IsNotNull(iconConnection);
            Assert.AreEqual("Icon", iconConnection.DatabaseName);
            Assert.AreEqual(@"cewd1815\SQLSHARED2012D", iconConnection.ServerName);
            Assert.AreEqual("Icon", iconConnection.ConnectionStringName);
            Assert.AreEqual(EnvironmentEnum.Test, iconConnection.Environment);
            Assert.IsFalse(iconConnection.IsEntityFramework);
        }

        [TestMethod]
        public void ReadDatabaseConfiguration_WhenAppHasIconSqlWithIconLogging_ReadsLoggingConnection()
        {
            // Arrange
            var configPath = "SampleDbConfig_IconSqlWithIconLogging.config";
            var wmiServiceWrapper = new RemoteWmiServiceWrapper(true, mockWMiSvc.Object,
                mockIconDbService.Object, mockMammothDbService.Object, mockEsbEnvironmentManager.Object);
            // Act
            var dbConfig = wmiServiceWrapper.ReadDatabaseConfiguration(configPath);
            // Assert
            Assert.IsNotNull(dbConfig);
            Assert.IsNotNull(dbConfig.LoggingConnection);
            Assert.AreSame(dbConfig.LoggingConnection, dbConfig.Connections.Single(c=>c.IsUsedForLogging));
            Assert.AreEqual("Icon", dbConfig.LoggingConnection.DatabaseName);
            Assert.AreEqual(@"cewd1815\SQLSHARED2012D", dbConfig.LoggingConnection.ServerName);
            Assert.AreEqual("dbLogVimLocaleController", dbConfig.LoggingConnection.ConnectionStringName);
            Assert.AreEqual(EnvironmentEnum.Test, dbConfig.LoggingConnection.Environment);
            Assert.IsFalse(dbConfig.LoggingConnection.IsEntityFramework);
        }

        [TestMethod]
        public void ReadDatabaseConfiguration_WhenAppHasIconEfWithIconLogging_ReadsDbConnection()
        {
            // Arrange
            var configPath = "SampleDbConfig_IconEfWithIconLogging.config";
            var wmiServiceWrapper = new RemoteWmiServiceWrapper(true, mockWMiSvc.Object,
                mockIconDbService.Object, mockMammothDbService.Object, mockEsbEnvironmentManager.Object);
            // Act
            var dbConfig = wmiServiceWrapper.ReadDatabaseConfiguration(configPath);
            // Assert
            Assert.IsNotNull(dbConfig);
            Assert.IsTrue(dbConfig.HasNonLoggingConnectionOfCategory(DatabaseCategoryEnum.Icon));
            var iconConnection = dbConfig.GetSingleNonLoggingConnectionOfCategory(DatabaseCategoryEnum.Icon);
            Assert.IsNotNull(iconConnection);
            Assert.AreEqual("Icon", iconConnection.DatabaseName);
            Assert.AreEqual(@"cewd1815\SQLSHARED2012D", iconConnection.ServerName);
            Assert.AreEqual("IconContext", iconConnection.ConnectionStringName);
            Assert.AreEqual(EnvironmentEnum.Test, iconConnection.Environment);
            Assert.IsTrue(iconConnection.IsEntityFramework);
        }

        [TestMethod]
        public void ReadDatabaseConfiguration_WhenAppHasIconEfWithIconLogging_ReadsLoggingConnection()
        {
            // Arrange
            var configPath = "SampleDbConfig_IconEfWithIconLogging.config";
            var wmiServiceWrapper = new RemoteWmiServiceWrapper(true, mockWMiSvc.Object,
                mockIconDbService.Object, mockMammothDbService.Object, mockEsbEnvironmentManager.Object);
            // Act
            var dbConfig = wmiServiceWrapper.ReadDatabaseConfiguration(configPath);
            // Assert
            Assert.IsNotNull(dbConfig);
            Assert.IsNotNull(dbConfig.LoggingConnection);
            Assert.AreSame(dbConfig.LoggingConnection, dbConfig.Connections.Single(c=>c.IsUsedForLogging));
            Assert.AreEqual("Icon", dbConfig.LoggingConnection.DatabaseName);
            Assert.AreEqual(@"cewd1815\SQLSHARED2012D", dbConfig.LoggingConnection.ServerName);
            Assert.AreEqual("dbLogIconEsb", dbConfig.LoggingConnection.ConnectionStringName);
            Assert.AreEqual(EnvironmentEnum.Test, dbConfig.LoggingConnection.Environment);
            Assert.IsFalse(dbConfig.LoggingConnection.IsEntityFramework);
        }

        [TestMethod]
        public void ReadDatabaseConfiguration_WhenAppHasIconSqlAndIconEfAndMammothSqlWithIconLogging_ReadsDbConnections()
        {
            // Arrange
            var configPath = "SampleDbConfig_IconSqlAndIconEfAndMammothSqlWithIconLogging.config";
            var wmiServiceWrapper = new RemoteWmiServiceWrapper(true, mockWMiSvc.Object,
                mockIconDbService.Object, mockMammothDbService.Object, mockEsbEnvironmentManager.Object);

            // Act
            var dbConfig = wmiServiceWrapper.ReadDatabaseConfiguration(configPath);

            // Assert
            Assert.IsNotNull(dbConfig);
            Assert.IsTrue(dbConfig.HasNonLoggingConnectionOfCategory(DatabaseCategoryEnum.Icon));

            var iconConnection = dbConfig.GetSingleNonLoggingConnectionOfCategory(DatabaseCategoryEnum.Icon);
            Assert.IsNotNull(iconConnection);
            Assert.AreEqual("Icon", iconConnection.DatabaseName);
            Assert.AreEqual(@"cewd1815\SQLSHARED2012D", iconConnection.ServerName);
            Assert.AreEqual("IconContext", iconConnection.ConnectionStringName);
            Assert.AreEqual(EnvironmentEnum.Test, iconConnection.Environment);
            Assert.IsTrue(iconConnection.IsEntityFramework);

            Assert.IsTrue(dbConfig.HasNonLoggingConnectionOfCategory(DatabaseCategoryEnum.Mammoth));
            var mammothConnection = dbConfig.GetSingleNonLoggingConnectionOfCategory(DatabaseCategoryEnum.Mammoth);
            Assert.IsNotNull(mammothConnection);
            Assert.AreEqual("Mammoth", mammothConnection.DatabaseName);
            Assert.AreEqual(@"MAMMOTH-DB01-DEV\MAMMOTH", mammothConnection.ServerName);
            Assert.AreEqual("Mammoth", mammothConnection.ConnectionStringName);
            Assert.AreEqual(EnvironmentEnum.Test, mammothConnection.Environment);
            Assert.IsFalse(mammothConnection.IsEntityFramework);
        }

        [TestMethod]
        public void ReadDatabaseConfiguration_WhenAppHasIconSqlAndIconEfAndMammothSqlWithIconLogging_ReadsLoggingConnections()
        {
            // Arrange
            var configPath = "SampleDbConfig_IconSqlAndIconEfAndMammothSqlWithIconLogging.config";
            var wmiServiceWrapper = new RemoteWmiServiceWrapper(true, mockWMiSvc.Object,
                mockIconDbService.Object, mockMammothDbService.Object, mockEsbEnvironmentManager.Object);

            // Act
            var dbConfig = wmiServiceWrapper.ReadDatabaseConfiguration(configPath);

            // Assert
            Assert.IsNotNull(dbConfig);
            Assert.IsNotNull(dbConfig.LoggingConnection);
            Assert.AreSame(dbConfig.LoggingConnection, dbConfig.Connections.Single(c=>c.IsUsedForLogging));
            Assert.AreEqual("Icon", dbConfig.LoggingConnection.DatabaseName);
            Assert.AreEqual(@"cewd1815\SQLSHARED2012D", dbConfig.LoggingConnection.ServerName);
            Assert.AreEqual("dbLogIconEsb", dbConfig.LoggingConnection.ConnectionStringName);
            Assert.AreEqual(EnvironmentEnum.Test, dbConfig.LoggingConnection.Environment);
            Assert.IsFalse(dbConfig.LoggingConnection.IsEntityFramework);
        }

        [TestMethod]
        public void ReadDatabaseConfiguration_WhenAppHasMammothSqlWithMammothLogging_ReadsDbConnection()
        {
            // Arrange
            var configPath = "SampleDbConfig_MammothSqlWithMammothLogging.config";
            var wmiServiceWrapper = new RemoteWmiServiceWrapper(true, mockWMiSvc.Object,
                mockIconDbService.Object, mockMammothDbService.Object, mockEsbEnvironmentManager.Object);

            // Act
            var dbConfig = wmiServiceWrapper.ReadDatabaseConfiguration(configPath);

            // Assert
            Assert.IsNotNull(dbConfig);
            Assert.IsTrue(dbConfig.HasNonLoggingConnectionOfCategory(DatabaseCategoryEnum.Mammoth));
            var mammothConnection = dbConfig.GetSingleNonLoggingConnectionOfCategory(DatabaseCategoryEnum.Mammoth);
            Assert.IsNotNull(mammothConnection);
            Assert.AreEqual("Mammoth", mammothConnection.DatabaseName);
            Assert.AreEqual(@"MAMMOTH-DB01-DEV\MAMMOTH", mammothConnection.ServerName);
            Assert.AreEqual("MammothContext", mammothConnection.ConnectionStringName);
            Assert.AreEqual(EnvironmentEnum.Test, mammothConnection.Environment);
            Assert.IsFalse(mammothConnection.IsEntityFramework);
        }

        [TestMethod]
        public void ReadDatabaseConfiguration_WhenAppHasMammothSqlWithMammothLogging_ReadsLoggingConnection()
        {
            // Arrange
            var configPath = "SampleDbConfig_MammothSqlWithMammothLogging.config";
            var wmiServiceWrapper = new RemoteWmiServiceWrapper(true, mockWMiSvc.Object,
                mockIconDbService.Object, mockMammothDbService.Object, mockEsbEnvironmentManager.Object);

            // Act
            var dbConfig = wmiServiceWrapper.ReadDatabaseConfiguration(configPath);

            // Assert
            Assert.IsNotNull(dbConfig);
            Assert.IsNotNull(dbConfig.LoggingConnection);
            Assert.AreSame(dbConfig.LoggingConnection, dbConfig.Connections.Single(c=>c.IsUsedForLogging));
            Assert.AreEqual("Mammoth", dbConfig.LoggingConnection.DatabaseName);
            Assert.AreEqual(@"MAMMOTH-DB01-DEV\MAMMOTH", dbConfig.LoggingConnection.ServerName);
            Assert.AreEqual("dbLogMammoth", dbConfig.LoggingConnection.ConnectionStringName);
            Assert.AreEqual(EnvironmentEnum.Test, dbConfig.LoggingConnection.Environment);
            Assert.IsFalse(dbConfig.LoggingConnection.IsEntityFramework);
        }

        [TestMethod]
        public void ReadDatabaseConfiguration_WhenAppHasIrmaSqlWithMammothLogging_ReadsDbConnections()
        {
            // Arrange
            var configPath = "SampleDbConfig_IrmaSqlWithMammothLogging.config";
            var wmiServiceWrapper = new RemoteWmiServiceWrapper(true, mockWMiSvc.Object,
                mockIconDbService.Object, mockMammothDbService.Object, mockEsbEnvironmentManager.Object);

            // Act
            var dbConfig = wmiServiceWrapper.ReadDatabaseConfiguration(configPath);

            // Assert
            Assert.IsNotNull(dbConfig);
            Assert.IsTrue(dbConfig.HasNonLoggingConnectionOfCategory(DatabaseCategoryEnum.IRMA));
            var irmaConnections = dbConfig.GetAllNonLoggingConnectionsOfCategory(DatabaseCategoryEnum.IRMA);
            Assert.AreEqual(12, irmaConnections.Count);

            var irmaFlConnection = dbConfig.Connections
                .Single(c => c.Category == DatabaseCategoryEnum.IRMA && c.ServerName.Contains("fl"));
            Assert.IsNotNull(irmaFlConnection);
            Assert.AreEqual("ItemCatalog_Test", irmaFlConnection.DatabaseName);
            Assert.AreEqual(@"idd-fl\fld", irmaFlConnection.ServerName);
            Assert.AreEqual("ItemCatalog_FL", irmaFlConnection.ConnectionStringName);
            Assert.AreEqual(EnvironmentEnum.Dev, irmaFlConnection.Environment);
            Assert.IsFalse(irmaFlConnection.IsEntityFramework);

            var irmaPnConnection = dbConfig.Connections
                .Single(c => c.Category == DatabaseCategoryEnum.IRMA && c.ServerName.Contains("pn"));
            Assert.IsNotNull(irmaPnConnection);
            Assert.AreEqual("ItemCatalog_Test", irmaPnConnection.DatabaseName);
            Assert.AreEqual(@"idt-pn\pnt", irmaPnConnection.ServerName);
            Assert.AreEqual("ItemCatalog_PN", irmaPnConnection.ConnectionStringName);
            Assert.AreEqual(EnvironmentEnum.Test, irmaPnConnection.Environment);
            Assert.IsFalse(irmaPnConnection.IsEntityFramework);
        }

        [TestMethod]
        public void ReadDatabaseConfiguration_WhenAppHasIrmaSqlWithMammothLogging_ReadsLoggingConnection()
        {
            // Arrange
            var configPath = "SampleDbConfig_IrmaSqlWithMammothLogging.config";
            var wmiServiceWrapper = new RemoteWmiServiceWrapper(true, mockWMiSvc.Object,
                mockIconDbService.Object, mockMammothDbService.Object, mockEsbEnvironmentManager.Object);

            // Act
            var dbConfig = wmiServiceWrapper.ReadDatabaseConfiguration(configPath);

            // Assert
            Assert.IsNotNull(dbConfig);
            Assert.IsNotNull(dbConfig.LoggingConnection);
            Assert.AreSame(dbConfig.LoggingConnection, dbConfig.Connections.Single(c=>c.IsUsedForLogging));
            Assert.AreEqual("Mammoth", dbConfig.LoggingConnection.DatabaseName);
            Assert.AreEqual(@"MAMMOTH-DB01-DEV\MAMMOTH", dbConfig.LoggingConnection.ServerName);
            Assert.AreEqual("dbLogMammoth", dbConfig.LoggingConnection.ConnectionStringName);
            Assert.AreEqual(EnvironmentEnum.Test, dbConfig.LoggingConnection.Environment);
            Assert.IsFalse(dbConfig.LoggingConnection.IsEntityFramework);
        }

        [TestMethod]
        public void ReadDatabaseConfiguration_WhenAppHasIconEfAndIrmaSqlWithIconLogging_ReadsDbConnections()
        {
            // Arrange
            var configPath = "SampleDbConfig_IconEfAndIrmaSqlWithIconLogging.config";
            var wmiServiceWrapper = new RemoteWmiServiceWrapper(true, mockWMiSvc.Object,
                mockIconDbService.Object, mockMammothDbService.Object, mockEsbEnvironmentManager.Object);

            // Act
            var dbConfig = wmiServiceWrapper.ReadDatabaseConfiguration(configPath);

            // Assert
            Assert.IsNotNull(dbConfig);
            Assert.IsTrue(dbConfig.HasNonLoggingConnectionOfCategory(DatabaseCategoryEnum.Icon));
            var iconConnection = dbConfig.GetSingleNonLoggingConnectionOfCategory(DatabaseCategoryEnum.Icon);
            Assert.IsNotNull(iconConnection);
            Assert.AreEqual("Icon", iconConnection.DatabaseName);
            Assert.AreEqual(@"cewd1815\SQLSHARED2012D", iconConnection.ServerName);
            Assert.AreEqual("IconContext", iconConnection.ConnectionStringName);
            Assert.AreEqual(EnvironmentEnum.Test, iconConnection.Environment);
            Assert.IsTrue(iconConnection.IsEntityFramework);

            Assert.IsTrue(dbConfig.HasNonLoggingConnectionOfCategory(DatabaseCategoryEnum.IRMA));
            var irmaConnections = dbConfig.GetAllNonLoggingConnectionsOfCategory(DatabaseCategoryEnum.IRMA);
            Assert.AreEqual(12, irmaConnections.Count);

            var irmaFlConnection = dbConfig.Connections
                .Single(c => c.Category == DatabaseCategoryEnum.IRMA && c.ServerName.Contains("fl"));
            Assert.IsNotNull(irmaFlConnection);
            Assert.AreEqual("ItemCatalog_Test", irmaFlConnection.DatabaseName);
            Assert.AreEqual(@"idd-fl\fld", irmaFlConnection.ServerName);
            Assert.AreEqual("ItemCatalog_FL", irmaFlConnection.ConnectionStringName);
            Assert.AreEqual(EnvironmentEnum.Dev, irmaFlConnection.Environment);
            Assert.IsFalse(irmaFlConnection.IsEntityFramework);

            var irmaPnConnection = dbConfig.Connections
                .Single(c => c.Category == DatabaseCategoryEnum.IRMA && c.ServerName.Contains("pn"));
            Assert.IsNotNull(irmaPnConnection);
            Assert.AreEqual("ItemCatalog_Test", irmaPnConnection.DatabaseName);
            Assert.AreEqual(@"idt-pn\pnt", irmaPnConnection.ServerName);
            Assert.AreEqual("ItemCatalog_PN", irmaPnConnection.ConnectionStringName);
            Assert.AreEqual(EnvironmentEnum.Test, irmaPnConnection.Environment);
            Assert.IsFalse(irmaPnConnection.IsEntityFramework);
        }

        [TestMethod]
        public void ReadDatabaseConfiguration_WhenAppHasIconEfAndIrmaSqlWithIconLogging_ReadsLoggingConnection()
        {
            // Arrange
            var configPath = "SampleDbConfig_IconEfAndIrmaSqlWithIconLogging.config";
            var wmiServiceWrapper = new RemoteWmiServiceWrapper(true, mockWMiSvc.Object,
                mockIconDbService.Object, mockMammothDbService.Object, mockEsbEnvironmentManager.Object);

            // Act
            var dbConfig = wmiServiceWrapper.ReadDatabaseConfiguration(configPath);

            // Assert
            Assert.IsNotNull(dbConfig);
            Assert.IsNotNull(dbConfig.LoggingConnection);
            Assert.AreSame(dbConfig.LoggingConnection, dbConfig.Connections.Single(c=>c.IsUsedForLogging));
            Assert.AreEqual("Icon", dbConfig.LoggingConnection.DatabaseName);
            Assert.AreEqual("dbLogIconGlobalController", dbConfig.LoggingConnection.ConnectionStringName);
            Assert.AreEqual(@"cewd1815\SQLSHARED2012D", dbConfig.LoggingConnection.ServerName);
            Assert.AreEqual(EnvironmentEnum.Test, dbConfig.LoggingConnection.Environment);
            Assert.IsFalse(dbConfig.LoggingConnection.IsEntityFramework);
        }

        [TestMethod]
        public void ReadDatabaseConfiguration_WhenAppHasIconEfAndIrmaEfWithIconLogging_ReadsDbConnections()
        {
            // Arrange
            var configPath = "SampleDbConfig_IconEfAndIrmaEfWithIconLogging.config";
            var wmiServiceWrapper = new RemoteWmiServiceWrapper(true, mockWMiSvc.Object,
                mockIconDbService.Object, mockMammothDbService.Object, mockEsbEnvironmentManager.Object);

            // Act
            var dbConfig = wmiServiceWrapper.ReadDatabaseConfiguration(configPath);

            // Assert
            Assert.IsNotNull(dbConfig);
            Assert.IsTrue(dbConfig.HasNonLoggingConnectionOfCategory(DatabaseCategoryEnum.Icon));
            var iconConnection = dbConfig.GetSingleNonLoggingConnectionOfCategory(DatabaseCategoryEnum.Icon);
            Assert.IsNotNull(iconConnection);
            Assert.AreEqual("Icon", iconConnection.DatabaseName);
            Assert.AreEqual(@"cewd1815\SQLSHARED2012D", iconConnection.ServerName);
            Assert.AreEqual("IconContext", iconConnection.ConnectionStringName);
            Assert.AreEqual(EnvironmentEnum.Test, iconConnection.Environment);
            Assert.IsTrue(iconConnection.IsEntityFramework);

            Assert.IsTrue(dbConfig.HasNonLoggingConnectionOfCategory(DatabaseCategoryEnum.IRMA));
            var irmaConnections = dbConfig.GetAllNonLoggingConnectionsOfCategory(DatabaseCategoryEnum.IRMA);
            Assert.AreEqual(12, irmaConnections.Count);

            var irmaFlConnection = dbConfig.Connections
                .Single(c => c.Category == DatabaseCategoryEnum.IRMA && c.ServerName.Contains("fl"));
            Assert.IsNotNull(irmaFlConnection);
            Assert.AreEqual("ItemCatalog_Test", irmaFlConnection.DatabaseName);
            Assert.AreEqual(@"idd-fl\fld", irmaFlConnection.ServerName);
            Assert.AreEqual("ItemCatalog_FL", irmaFlConnection.ConnectionStringName);
            Assert.AreEqual(EnvironmentEnum.Dev, irmaFlConnection.Environment);
            Assert.IsTrue(irmaFlConnection.IsEntityFramework);

            var irmaPnConnection = dbConfig.Connections
                .Single(c => c.Category == DatabaseCategoryEnum.IRMA && c.ServerName.Contains("pn"));
            Assert.IsNotNull(irmaPnConnection);
            Assert.AreEqual("ItemCatalog_Test", irmaPnConnection.DatabaseName);
            Assert.AreEqual(@"idt-pn\pnt", irmaPnConnection.ServerName);
            Assert.AreEqual("ItemCatalog_PN", irmaPnConnection.ConnectionStringName);
            Assert.AreEqual(EnvironmentEnum.Test, irmaPnConnection.Environment);
            Assert.IsTrue(irmaPnConnection.IsEntityFramework);
        }

        [TestMethod]
        public void ReadDatabaseConfiguration_WhenAppHasIconEfAndIrmaEfWithIconLogging_ReadsLoggingConnection()
        {
            // Arrange
            var configPath = "SampleDbConfig_IconEfAndIrmaEfWithIconLogging.config";
            var wmiServiceWrapper = new RemoteWmiServiceWrapper(true, mockWMiSvc.Object,
                mockIconDbService.Object, mockMammothDbService.Object, mockEsbEnvironmentManager.Object);

            // Act
            var dbConfig = wmiServiceWrapper.ReadDatabaseConfiguration(configPath);

            // Assert
            Assert.IsNotNull(dbConfig);
            Assert.IsNotNull(dbConfig.LoggingConnection);
            Assert.AreSame(dbConfig.LoggingConnection, dbConfig.Connections.Single(c=>c.IsUsedForLogging));
            Assert.AreEqual("Icon", dbConfig.LoggingConnection.DatabaseName);
            Assert.AreEqual(@"cewd1815\SQLSHARED2012D", dbConfig.LoggingConnection.ServerName);
            Assert.AreEqual("dbLogIconRegionalController", dbConfig.LoggingConnection.ConnectionStringName);
            Assert.AreEqual(EnvironmentEnum.Test, dbConfig.LoggingConnection.Environment);
            Assert.IsFalse(dbConfig.LoggingConnection.IsEntityFramework);
        }

        [TestMethod]
        public void ReadDatabaseConfiguration_WhenAppHasIconSqlAndIrmaSqlAndMammothSqlWithIconLogging_ReadsDbConnections()
        {
            // Arrange
            var configPath = "SampleDbConfig_IconSqlAndIrmaSqlAndMammothSqlWithIconLogging.config";
            var wmiServiceWrapper = new RemoteWmiServiceWrapper(true, mockWMiSvc.Object,
                mockIconDbService.Object, mockMammothDbService.Object, mockEsbEnvironmentManager.Object);

            // Act
            var dbConfig = wmiServiceWrapper.ReadDatabaseConfiguration(configPath);

            // Assert
            Assert.IsNotNull(dbConfig);
            Assert.IsTrue(dbConfig.HasNonLoggingConnectionOfCategory(DatabaseCategoryEnum.Icon));
            var iconConnection = dbConfig.GetSingleNonLoggingConnectionOfCategory(DatabaseCategoryEnum.Icon);
            Assert.IsNotNull(iconConnection);
            Assert.AreEqual("Icon", iconConnection.DatabaseName);
            Assert.AreEqual(@"CEWD1815\SQLSHARED2012D", iconConnection.ServerName);
            Assert.AreEqual("Icon", iconConnection.ConnectionStringName);
            Assert.AreEqual(EnvironmentEnum.Test, iconConnection.Environment);
            Assert.IsFalse(iconConnection.IsEntityFramework);

            Assert.IsTrue(dbConfig.HasNonLoggingConnectionOfCategory(DatabaseCategoryEnum.Mammoth));
            var mammothConnection = dbConfig.GetSingleNonLoggingConnectionOfCategory(DatabaseCategoryEnum.Mammoth);
            Assert.IsNotNull(mammothConnection);
            Assert.AreEqual("Mammoth", mammothConnection.DatabaseName);
            Assert.AreEqual(@"MAMMOTH-DB01-DEV\MAMMOTH", mammothConnection.ServerName);
            Assert.AreEqual("Mammoth", mammothConnection.ConnectionStringName);
            Assert.AreEqual(EnvironmentEnum.Test, mammothConnection.Environment);
            Assert.IsFalse(mammothConnection.IsEntityFramework);

            Assert.IsTrue(dbConfig.HasNonLoggingConnectionOfCategory(DatabaseCategoryEnum.IRMA));
            var irmaConnections = dbConfig.GetAllNonLoggingConnectionsOfCategory(DatabaseCategoryEnum.IRMA);
            Assert.AreEqual(12, irmaConnections.Count);

            var irmaFlConnection = dbConfig.Connections
                .Single(c => c.Category == DatabaseCategoryEnum.IRMA && c.ServerName.Contains("fl"));
            Assert.IsNotNull(irmaFlConnection);
            Assert.AreEqual("ItemCatalog_Test", irmaFlConnection.DatabaseName);
            Assert.AreEqual(@"idd-fl\fld", irmaFlConnection.ServerName);
            Assert.AreEqual("ItemCatalog_FL", irmaFlConnection.ConnectionStringName);
            Assert.AreEqual(EnvironmentEnum.Dev, irmaFlConnection.Environment);
            Assert.IsFalse(irmaFlConnection.IsEntityFramework);

            var irmaPnConnection = dbConfig.Connections
                .Single(c => c.Category == DatabaseCategoryEnum.IRMA && c.ServerName.Contains("pn"));
            Assert.IsNotNull(irmaPnConnection);
            Assert.AreEqual("ItemCatalog_Test", irmaPnConnection.DatabaseName);
            Assert.AreEqual(@"idt-pn\pnt", irmaPnConnection.ServerName);
            Assert.AreEqual("ItemCatalog_PN", irmaPnConnection.ConnectionStringName);
            Assert.AreEqual(EnvironmentEnum.Test, irmaPnConnection.Environment);
            Assert.IsFalse(irmaPnConnection.IsEntityFramework);
        }

        [TestMethod]
        public void ReadDatabaseConfiguration_WhenAppHasIconSqlAndIrmaSqlAndMammothSqlWithIconLogging_ReadsLoggingConnection()
        {
            // Arrange
            var configPath = "SampleDbConfig_IconSqlAndIrmaSqlAndMammothSqlWithIconLogging.config";
            var wmiServiceWrapper = new RemoteWmiServiceWrapper(true, mockWMiSvc.Object,
                mockIconDbService.Object, mockMammothDbService.Object, mockEsbEnvironmentManager.Object);

            // Act
            var dbConfig = wmiServiceWrapper.ReadDatabaseConfiguration(configPath);

            // Assert
            Assert.IsNotNull(dbConfig);
            Assert.IsNotNull(dbConfig.LoggingConnection);
            Assert.AreSame(dbConfig.LoggingConnection, dbConfig.Connections.Single(c=>c.IsUsedForLogging));
            Assert.AreEqual("Icon", dbConfig.LoggingConnection.DatabaseName);
            Assert.AreEqual(@"cewd1815\SQLSHARED2012D", dbConfig.LoggingConnection.ServerName);
            Assert.AreEqual("dbLogMonitor", dbConfig.LoggingConnection.ConnectionStringName);
            Assert.AreEqual(EnvironmentEnum.Test, dbConfig.LoggingConnection.Environment);
            Assert.IsFalse(dbConfig.LoggingConnection.IsEntityFramework);
        }

        [TestMethod]
        public void ReadDatabaseConfiguration_WhenAppHasVimSqlWithFileLogging_ReadsDbConnections()
        {
            // Arrange
            var configPath = "SampleDbConfig_VimSqlWithFileLogging.config";
            var wmiServiceWrapper = new RemoteWmiServiceWrapper(true, mockWMiSvc.Object,
                mockIconDbService.Object, mockMammothDbService.Object, mockEsbEnvironmentManager.Object);

            // Act
            var dbConfig = wmiServiceWrapper.ReadDatabaseConfiguration(configPath);

            // Assert
            Assert.IsNotNull(dbConfig);

            Assert.IsTrue(dbConfig.HasNonLoggingConnectionOfCategory(DatabaseCategoryEnum.Vim));
            var vimConnection = dbConfig.GetSingleNonLoggingConnectionOfCategory(DatabaseCategoryEnum.Vim);
            Assert.IsNotNull(vimConnection);
            Assert.AreEqual("", vimConnection.DatabaseName);
            Assert.AreEqual(@"(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=vim_dt.wfm.pvt)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=vim_dt)))", vimConnection.ServerName);
            Assert.AreEqual("VIM", vimConnection.ConnectionStringName);
            Assert.AreEqual(EnvironmentEnum.Undefined, vimConnection.Environment);
            Assert.IsFalse(vimConnection.IsEntityFramework);

            Assert.IsTrue(dbConfig.HasNonLoggingConnectionOfCategory(DatabaseCategoryEnum.Other));
            var promoConnection = dbConfig.GetSingleNonLoggingConnectionOfCategory(DatabaseCategoryEnum.Other);
            Assert.IsNotNull(promoConnection);
            Assert.AreEqual("wfmpromotions", promoConnection.DatabaseName);
            Assert.AreEqual(@"promodb-dev", promoConnection.ServerName);
            Assert.AreEqual("PROMO", promoConnection.ConnectionStringName);
            Assert.AreEqual(EnvironmentEnum.Undefined, promoConnection.Environment);
            Assert.IsFalse(promoConnection.IsEntityFramework);
        }

        [TestMethod]
        public void ReadDatabaseConfiguration_WhenAppHasVimSqlWithFileLogging_LoggingConnectionIsNull()
        {
            // Arrange
            var configPath = "SampleDbConfig_VimSqlWithFileLogging.config";
            var wmiServiceWrapper = new RemoteWmiServiceWrapper(true, mockWMiSvc.Object,
                mockIconDbService.Object, mockMammothDbService.Object, mockEsbEnvironmentManager.Object);

            // Act
            var dbConfig = wmiServiceWrapper.ReadDatabaseConfiguration(configPath);

            // Assert
            Assert.IsNotNull(dbConfig);
            Assert.IsNull(dbConfig.LoggingConnection);
            Assert.AreEqual("None", dbConfig.LoggingSummary);
        }

        [TestMethod]
        public void ReadDatabaseConfiguration_WhenEncryptedConnection_ReadsDbConnection()
        {
            // Arrange
            var configPath = "SampleDbConfig_EncryptedDatabaseConnection.config";
            var wmiServiceWrapper = new RemoteWmiServiceWrapper(true, mockWMiSvc.Object,
                mockIconDbService.Object, mockMammothDbService.Object, mockEsbEnvironmentManager.Object);

            // Act
            var dbConfig = wmiServiceWrapper.ReadDatabaseConfiguration(configPath);

            // Assert
            Assert.IsNotNull(dbConfig);
            Assert.IsTrue(dbConfig.HasNonLoggingConnectionOfCategory(DatabaseCategoryEnum.Encrypted));
            var encryptedConnection = dbConfig.GetSingleNonLoggingConnectionOfCategory(DatabaseCategoryEnum.Encrypted);
            Assert.IsNotNull(encryptedConnection);
            Assert.AreEqual("{Encrypted}", encryptedConnection.DatabaseName);
            Assert.AreEqual("{Encrypted}", encryptedConnection.ServerName);
            Assert.AreEqual("{Encrypted}", encryptedConnection.ConnectionStringName);
            Assert.AreEqual(EnvironmentEnum.Undefined, encryptedConnection.Environment);
            Assert.IsFalse(encryptedConnection.IsEntityFramework);
        }

        [TestMethod]
        public void ReadDatabaseConfiguration_WhenIconAndIrmaEfWithNewSql16Db_ReadsDbConnection()
        {
            // Arrange
            var configPath = "SampleDbConfig_IconEfAndIrmaEfWithIconLoggingSql16Tst.config";
            var wmiServiceWrapper = new RemoteWmiServiceWrapper(true, mockWMiSvc.Object,
                mockIconDbService.Object, mockMammothDbService.Object, mockEsbEnvironmentManager.Object);

            // Act
            var dbConfig = wmiServiceWrapper.ReadDatabaseConfiguration(configPath);

            // Assert
            Assert.IsNotNull(dbConfig);
            Assert.IsTrue(dbConfig.HasNonLoggingConnectionOfCategory(DatabaseCategoryEnum.Icon));
            var iconConnection = dbConfig.GetSingleNonLoggingConnectionOfCategory(DatabaseCategoryEnum.Icon);
            Assert.IsNotNull(iconConnection);
            Assert.AreEqual("Icon2016", iconConnection.DatabaseName);
            Assert.AreEqual(@"sql-icon16-tst", iconConnection.ServerName);
            Assert.AreEqual("IconContext", iconConnection.ConnectionStringName);
            Assert.AreEqual(EnvironmentEnum.Dev, iconConnection.Environment);
            Assert.IsTrue(iconConnection.IsEntityFramework);

            Assert.IsTrue(dbConfig.HasNonLoggingConnectionOfCategory(DatabaseCategoryEnum.IRMA));
            var irmaConnections = dbConfig.GetAllNonLoggingConnectionsOfCategory(DatabaseCategoryEnum.IRMA);
            Assert.AreEqual(12, irmaConnections.Count);

            var irmaFlConnection = dbConfig.Connections
                .Single(c => c.Category == DatabaseCategoryEnum.IRMA && c.ServerName.Contains("fl"));
            Assert.IsNotNull(irmaFlConnection);
            Assert.AreEqual("ItemCatalog_Test", irmaFlConnection.DatabaseName);
            Assert.AreEqual(@"idd-fl\fld", irmaFlConnection.ServerName);
            Assert.AreEqual("ItemCatalog_FL", irmaFlConnection.ConnectionStringName);
            Assert.AreEqual(EnvironmentEnum.Dev, irmaFlConnection.Environment);
            Assert.IsTrue(irmaFlConnection.IsEntityFramework);

            var irmaPnConnection = dbConfig.Connections
                .Single(c => c.Category == DatabaseCategoryEnum.IRMA && c.ServerName.Contains("pn"));
            Assert.IsNotNull(irmaPnConnection);
            Assert.AreEqual("ItemCatalog_Test", irmaPnConnection.DatabaseName);
            Assert.AreEqual(@"idt-pn\pnt", irmaPnConnection.ServerName);
            Assert.AreEqual("ItemCatalog_PN", irmaPnConnection.ConnectionStringName);
            Assert.AreEqual(EnvironmentEnum.Test, irmaPnConnection.Environment);
            Assert.IsTrue(irmaPnConnection.IsEntityFramework);
        }
    }
}
