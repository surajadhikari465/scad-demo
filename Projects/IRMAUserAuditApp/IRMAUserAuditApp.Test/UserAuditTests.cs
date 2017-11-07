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
        public void UserAudit_DetermineAuditAction_WhenRunDateIsImportDate_ReturnsActionImport()
        {
            //Arrange
            this.auditRunTime = DateTime.Now;
            string exportDates = "";
            string exportDate1 = DateTime.Today.AddDays(-21).ToShortDateString();
            string exportDate2 = DateTime.Today.AddDays(-21).AddYears(1).ToShortDateString();
            string folderName1 = "FY2018Q4";
            string folderName2 = "FY2019Q4";
            string importDates = "";
            string importDate1 = DateTime.Today.ToShortDateString(); 
            string importDate2 = DateTime.Today.AddYears(1).ToShortDateString(); 
            string delimiter = ";";

            //The exportDates and importDates should like this:
            //    FY2017Q4:11/3/2017;FY2018Q4:11/3/2018
            //    FY2017Q4:11/24/2017;FY2018Q4:11/24/201

            exportDates = folderName1 + ":" + exportDate1 + delimiter + folderName2 + ":" + exportDate2;
            importDates = folderName1 + ":" + importDate1 + delimiter + folderName2 + ":" + importDate2;
            mockConfigRepo.Setup(c => c.ConfigurationGetValue(It.Is<string>(s => s == "ExportDates")))
                .Returns(exportDates);
            mockConfigRepo.Setup(c => c.ConfigurationGetValue(It.Is<string>(s => s == "ImportDates")))
                .Returns(importDates);
      
            var userAudit = new UserAudit(options, testLog, auditRunTime, testConfigRepo, testDateRepo);
            //Act
            var functionEnum = userAudit.DetermineAuditAction(ref folderName1);
            //Assert
            Assert.AreEqual(UserAuditFunctionEnum.Import, functionEnum);
        }

        [TestMethod]
        public void UserAudit_DetermineAuditAction_WhenRunDateIsExportDate_ReturnsActionExport()
        {
            //Arrange
            this.auditRunTime = DateTime.Now;
            string exportDates = "";
            string exportDate1 = DateTime.Today.ToShortDateString();
            string exportDate2 = DateTime.Today.AddYears(1).ToShortDateString();
            string folderName1 = "FY2017Q4";
            string folderName2 = "FY2018Q4";
            string importDates = "";
            string importDate1 = DateTime.Today.AddDays(21).ToShortDateString();
            string importDate2 = DateTime.Today.AddDays(21).AddYears(1).ToShortDateString();
            string delimiter = ";";

            //The exportDates and importDates should like this:
            //    FY2017Q4:11/3/2017;FY2018Q4:11/3/2018
            //    FY2017Q4:11/24/2017;FY2018Q4:11/24/201

            exportDates = folderName1 + ":" + exportDate1 + delimiter + folderName2 + ":" + exportDate2;
            importDates = folderName1 + ":" + importDate1 + delimiter + folderName2 + ":" + importDate2;

            mockConfigRepo.Setup(c => c.ConfigurationGetValue(It.Is<string>(s => s == "ExportDates")))
                .Returns(exportDates);
            mockConfigRepo.Setup(c => c.ConfigurationGetValue(It.Is<string>(s => s == "ImportDates")))
                .Returns(importDates);
         
            var userAudit = new UserAudit(options, testLog, auditRunTime, testConfigRepo, testDateRepo);
            //Act
            var functionEnum = userAudit.DetermineAuditAction(ref folderName1);
            //Assert
            Assert.AreEqual(UserAuditFunctionEnum.Export, functionEnum);
        }

        [TestMethod]
        public void UserAudit_DetermineAuditAction_WhenRunDateIsExportDateAndSingleExportDateExists_ReturnsActionExport()
        {
            //Arrange
            this.auditRunTime = DateTime.Now;
            string exportDates = "";
            string exportDate1 = DateTime.Today.ToShortDateString();
            string folderName1 = "FY2017Q4";
            string importDates = "";
            string importDate1 = DateTime.Today.AddDays(21).ToShortDateString();

            //The exportDates and importDates should like this:
            //    FY2017Q4:11/3/2017
            //    FY2017Q4:11/24/2017

            exportDates = folderName1 + ":" + exportDate1;
            importDates = folderName1 + ":" + importDate1;

            mockConfigRepo.Setup(c => c.ConfigurationGetValue(It.Is<string>(s => s == "ExportDates")))
                .Returns(exportDates);
            mockConfigRepo.Setup(c => c.ConfigurationGetValue(It.Is<string>(s => s == "ImportDates")))
                .Returns(importDates);

            var userAudit = new UserAudit(options, testLog, auditRunTime, testConfigRepo, testDateRepo);
            //Act
            var functionEnum = userAudit.DetermineAuditAction(ref folderName1);
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
