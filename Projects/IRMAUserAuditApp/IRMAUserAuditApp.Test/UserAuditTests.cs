using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IRMAUserAuditConsole;
using log4net;
using Moq;
using WholeFoods.Common.IRMALib;
using WholeFoods.Common.IRMALib.Dates;
using System.Collections.Generic;

namespace IRMAUserAuditApp.Test
{
    [TestClass]
    public class UserAuditTests : UserAuditTestBase
    {
        protected AuditOptions options = null;
        protected const string testRegion = "XX";
        protected const string testEnvironment = "TEST";
        protected const string testConnectionString = "me talk to database";
        protected DateTime? auditRunTime = null;
        protected Mock<ILog> mockLogger = new Mock<ILog>();
        protected Mock<IConfigRepository> mockConfigRepo = new Mock<IConfigRepository>();
        protected Mock<IDateRepository> mockDateRepo = new Mock<IDateRepository>();

        protected ILog testLog { get { return mockLogger.Object; } }
        protected IConfigRepository testConfigRepo { get { return mockConfigRepo.Object; } }
        protected IDateRepository testDateRepo { get { return mockDateRepo.Object; } }

        [TestInitialize]
        public void TestInit()
        {
            options = new AuditOptions(testRegion, testEnvironment, testConnectionString);
            var dummyEnvironments = new List<WholeFoods.Common.IRMALib.AppConfigEnv>
            {
                new WholeFoods.Common.IRMALib.AppConfigEnv { Name = testEnvironment, EnvironmentID = base.EnvId }
            };
            var dummyApps = new List<WholeFoods.Common.IRMALib.AppConfigApp>
            {
                new WholeFoods.Common.IRMALib.AppConfigApp { Name = "user audit", ApplicationID = base.AppId }
            };
            mockConfigRepo.Setup(c => c.GetEnvironmentList())
                .Returns(dummyEnvironments);
            mockConfigRepo.Setup(c => c.GetApplicationList(It.IsAny<Guid>()))
                .Returns(dummyApps);

            auditRunTime = new DateTime(2017, 4, 24);
        }

        [TestMethod]
        public void UserAudit_LoadConfig_ShouldLoadExpectedAppId()
        {
            //Arrange
            var userAudit = new UserAudit(options, testLog, auditRunTime, testConfigRepo, testDateRepo);
            //Act
            userAudit.LoadConfig();
            //Assert
            Assert.AreEqual(base.AppId, userAudit.AppId);
        }

        [TestMethod]
        public void UserAudit_LoadConfig_ShouldLoadExpectedEnvId()
        {
            //Arrange
            var userAudit = new UserAudit(options, testLog, auditRunTime, testConfigRepo, testDateRepo);
            //Act
            userAudit.LoadConfig();
            //Assert
            Assert.AreEqual(base.EnvId, userAudit.EnvId);
        }

        [TestMethod]
        public void UserAudit_DetermineAuditAction_WhenNoValuesForDates_ReturnsActionNone()
        {
            //Arrange
            string exportDates = "";
            string importDates = "";
            string folderName = "";
            mockConfigRepo.Setup(c => c.ConfigurationGetValue(It.Is<string>(s=>s=="ExportDates"), ref exportDates))
                .Returns(false);
            mockConfigRepo.Setup(c => c.ConfigurationGetValue(It.Is<string>(s => s == "ImportDates"), ref importDates))
                .Returns(false);
            var userAudit = new UserAudit(options, testLog, auditRunTime, testConfigRepo, testDateRepo);
            //Act
            var functionEnum = userAudit.DetermineAuditAction(ref folderName);
            //Assert
            Assert.AreEqual(UserAuditFunctionEnum.None, functionEnum);
        }

        [TestMethod]
        public void UserAudit_DetermineAuditAction_WhenNoValuesForDates_LogsError()
        {
            //Arrange
            string exportDates = "";
            string importDates = "";
            string folderName = "";

            mockConfigRepo.Setup(c => c.ConfigurationGetValue(It.Is<string>(s => s == "ExportDates"), ref exportDates))
                .Returns(false);
            mockConfigRepo.Setup(c => c.ConfigurationGetValue(It.Is<string>(s => s == "ImportDates"), ref importDates))
                .Returns(false);
            var userAudit = new UserAudit(options, testLog, auditRunTime, testConfigRepo, testDateRepo);
            //Act
            var functionEnum = userAudit.DetermineAuditAction(ref folderName);
                //Assert
            mockLogger.Verify(m => m.Error(It.IsAny<object>()), Times.Once);
        }

        [TestMethod]
        public void UserAudit_DetermineAuditAction_WhenHasImportDate_ReturnsActionImport()
        {
            //Arrange
            this.auditRunTime = new DateTime(2013, 6, 6);
            string exportDates = "";
            string importDates = "2013-6-6";
            string folderName = "";
            string delimiter = ";";
            mockConfigRepo.Setup(c => c.ConfigurationGetValue(It.Is<string>(s => s == "ExportDates")))
                .Returns(exportDates);
            mockConfigRepo.Setup(c => c.ConfigurationGetValue(It.Is<string>(s => s == "ImportDates")))
                .Returns(importDates);
            mockConfigRepo.Setup(c => c.ConfigurationGetValue(It.Is<string>(s => s == "delimiter")))
                .Returns(delimiter);
            var userAudit = new UserAudit(options, testLog, auditRunTime, testConfigRepo, testDateRepo);
            //Act
            var functionEnum = userAudit.DetermineAuditAction(ref folderName);
            //Assert
            Assert.AreEqual(UserAuditFunctionEnum.Import, functionEnum);
        }

        [TestMethod]
        public void UserAudit_DetermineAuditAction_WhenHasExportDate_ReturnsActionExport()
        {
            //Arrange
            this.auditRunTime = new DateTime(2013, 6, 6);
            string exportDates = "2013-06-06";
            string folderName = "";
            string importDates = "";
            string delimiter = "|";
            mockConfigRepo.Setup(c => c.ConfigurationGetValue(It.Is<string>(s => s == "ExportDates")))
                .Returns(exportDates);
            mockConfigRepo.Setup(c => c.ConfigurationGetValue(It.Is<string>(s => s == "ImportDates")))
                .Returns(importDates);
            mockConfigRepo.Setup(c => c.ConfigurationGetValue(It.Is<string>(s => s == "delimiter")))
                .Returns(delimiter);
            var userAudit = new UserAudit(options, testLog, auditRunTime, testConfigRepo, testDateRepo);
            //Act
            var functionEnum = userAudit.DetermineAuditAction(ref folderName);
            //Assert
            Assert.AreEqual(UserAuditFunctionEnum.Export, functionEnum);
        }

        [TestMethod]
        public void UserAudit_DetermineAuditAction_WhenHasMultipleExportDates_ReturnsActionExport()
        {
            //Arrange
            this.auditRunTime = new DateTime(2013, 6, 6);
            string folderName = "";
            string exportDates = "2014-1-1|2001-12-31|2013-06-06";
            string importDates = "";
            string delimiter = "|";
            mockConfigRepo.Setup(c => c.ConfigurationGetValue(It.Is<string>(s => s == "ExportDates")))
                .Returns(exportDates);
            mockConfigRepo.Setup(c => c.ConfigurationGetValue(It.Is<string>(s => s == "ImportDates")))
                .Returns(importDates);
            mockConfigRepo.Setup(c => c.ConfigurationGetValue(It.Is<string>(s => s == "delimiter")))
                .Returns(delimiter);
            var userAudit = new UserAudit(options, testLog, auditRunTime, testConfigRepo, testDateRepo);
            //Act
            var functionEnum = userAudit.DetermineAuditAction(ref folderName);
            //Assert
            Assert.AreEqual(UserAuditFunctionEnum.Export, functionEnum);
        }

        [TestMethod]
        public void UserAudit_DetermineAuditAction_WhenExportDatesDoNotMatch_ReturnsActionNone()
        {
            //Arrange
            this.auditRunTime = new DateTime(2017, 6, 6);
            string folderName = "";
            string exportDates = "2014-1-1|2001-12-31|2013-06-06";
            string importDates = "";
            string delimiter = "|";
            mockConfigRepo.Setup(c => c.ConfigurationGetValue(It.Is<string>(s => s == "ExportDates")))
                .Returns(exportDates);
            mockConfigRepo.Setup(c => c.ConfigurationGetValue(It.Is<string>(s => s == "ImportDates")))
                .Returns(importDates);
            mockConfigRepo.Setup(c => c.ConfigurationGetValue(It.Is<string>(s => s == "delimiter")))
                .Returns(delimiter);
            var userAudit = new UserAudit(options, testLog, auditRunTime, testConfigRepo, testDateRepo);
            //Act
            var functionEnum = userAudit.DetermineAuditAction(ref folderName);
            //Assert
            Assert.AreEqual(UserAuditFunctionEnum.None, functionEnum);
        }
    }
}
