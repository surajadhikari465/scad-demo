using IRMAUserAuditConsole;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRMAUserAuditApp.Test
{
    [TestClass]
    public class AuditOptionsTests
    {
        [TestMethod]
        public void AuditOptions_DefaultConstructor_SetsDefaultPropertyValuesToExpected()
        {
            //Arrange
            //Act
            var options = new AuditOptions();
            //Assert
            Assert.AreEqual(IRMAEnvironmentEnum.Test, options.Environment);
            Assert.AreEqual(UserAuditFunctionEnum.None, options.Function);
            Assert.IsNull(options.ConnectionString);
            Assert.IsNull(options.Region);
            Assert.IsNull(options.ErrorMessage);
            Assert.IsNull(options.WarningMessage);
            Assert.IsFalse(options.IsError);
            Assert.IsFalse(options.IsWarning);
        }

        [TestMethod]
        public void AuditOptions_ParameterizedConstructor_SetsPropertyValuesToProvidedValues()
        {
            //Arrange
            string region = "XY";
            string env = "DEV";
            string connection = "InitialCatalog=asdlkjsa;dj98329h;fewa;";
            //Act
            var options = new AuditOptions(region, env, connection);
            //Assert
            Assert.AreEqual(IRMAEnvironmentEnum.Dev, options.Environment);
            Assert.AreEqual(UserAuditFunctionEnum.None, options.Function);
            Assert.AreEqual(connection, options.ConnectionString);
            Assert.AreEqual(region, options.Region);
            Assert.IsNull(options.ErrorMessage);
            Assert.IsNull(options.WarningMessage);
            Assert.IsFalse(options.IsError);
            Assert.IsFalse(options.IsWarning);
        }

        [TestMethod]
        public void AuditOptions_ConvertStringToFunction_WhenGiven_backup_ReturnsBackupEnum()
        {
            //Arrange
            string providedString = "backup";
            //Act
            var function = AuditOptions.ConvertStringToFunction(providedString);
            //Assert
            Assert.AreEqual(UserAuditFunctionEnum.Backup, function);
        }

        [TestMethod]
        public void AuditOptions_ConvertStringToFunction_WhenGiven_Backup_ReturnsBackupEnum()
        {
            //Arrange
            string providedString = "Backup";
            //Act
            var function = AuditOptions.ConvertStringToFunction(providedString);
            //Assert
            Assert.AreEqual(UserAuditFunctionEnum.Backup, function);
        }

        [TestMethod]
        public void AuditOptions_ConvertStringToFunction_WhenGiven_BACKUP_ReturnsBackupEnum()
        {
            //Arrange
            string providedString = "BACKUP";
            //Act
            var function = AuditOptions.ConvertStringToFunction(providedString);
            //Assert
            Assert.AreEqual(UserAuditFunctionEnum.Backup, function);
        }

        [TestMethod]
        public void AuditOptions_ConvertStringToFunction_WhenGiven_restore_ReturnsRestoreEnum()
        {
            //Arrange
            string providedString = "restore";
            //Act
            var function = AuditOptions.ConvertStringToFunction(providedString);
            //Assert
            Assert.AreEqual(UserAuditFunctionEnum.Restore, function);
        }

        [TestMethod]
        public void AuditOptions_ConvertStringToFunction_WhenGiven_Restore_ReturnsRestoreEnum()
        {
            //Arrange
            string providedString = "Restore";
            //Act
            var function = AuditOptions.ConvertStringToFunction(providedString);
            //Assert
            Assert.AreEqual(UserAuditFunctionEnum.Restore, function);
        }

        [TestMethod]
        public void AuditOptions_ConvertStringToFunction_WhenGiven_RESTORE_ReturnsRestoreEnum()
        {
            //Arrange
            string providedString = "RESTORE";
            //Act
            var function = AuditOptions.ConvertStringToFunction(providedString);
            //Assert
            Assert.AreEqual(UserAuditFunctionEnum.Restore, function);
        }

        [TestMethod]
        public void AuditOptions_ConvertStringToFunction_WhenGiven_import_ReturnsImportEnum()
        {
            //Arrange
            string providedString = "import";
            //Act
            var function = AuditOptions.ConvertStringToFunction(providedString);
            //Assert
            Assert.AreEqual(UserAuditFunctionEnum.Import, function);
        }

        [TestMethod]
        public void AuditOptions_ConvertStringToFunction_WhenGiven_Import_ReturnsImportEnum()
        {
            //Arrange
            string providedString = "Import";
            //Act
            var function = AuditOptions.ConvertStringToFunction(providedString);
            //Assert
            Assert.AreEqual(UserAuditFunctionEnum.Import, function);
        }
        
        [TestMethod]
        public void AuditOptions_ConvertStringToFunction_WhenGiven_IMPORT_ReturnsImportEnum()
        {
            //Arrange
            string providedString = "IMPORT";
            //Act
            var function = AuditOptions.ConvertStringToFunction(providedString);
            //Assert
            Assert.AreEqual(UserAuditFunctionEnum.Import, function);
        }

        [TestMethod]
        public void AuditOptions_ConvertStringToFunction_WhenGiven_export_ReturnsExportEnum()
        {
            //Arrange
            string providedString = "export";
            //Act
            var function = AuditOptions.ConvertStringToFunction(providedString);
            //Assert
            Assert.AreEqual(UserAuditFunctionEnum.Export, function);
        }

        [TestMethod]
        public void AuditOptions_ConvertStringToFunction_WhenGiven_Export_ReturnsExportEnum()
        {
            //Arrange
            string providedString = "Export";
            //Act
            var function = AuditOptions.ConvertStringToFunction(providedString);
            //Assert
            Assert.AreEqual(UserAuditFunctionEnum.Export, function);
        }

        [TestMethod]
        public void AuditOptions_ConvertStringToFunction_WhenGiven_EXPORT_ReturnsExportEnum()
        {
            //Arrange
            string providedString = "EXPORT";
            //Act
            var function = AuditOptions.ConvertStringToFunction(providedString);
            //Assert
            Assert.AreEqual(UserAuditFunctionEnum.Export, function);
        }

        [TestMethod]
        public void AuditOptions_ConvertStringToFunction_WhenGiven_EmptyString_ReturnsNone()
        {
            //Arrange
            string providedString = String.Empty;
            //Act
            var function = AuditOptions.ConvertStringToFunction(providedString);
            //Assert
            Assert.AreEqual(UserAuditFunctionEnum.None, function);
        }

        [TestMethod]
        public void AuditOptions_ConvertStringToFunction_WhenGiven_NullString_ReturnsNone()
        {
            //Arrange
            string providedString = null;
            //Act
            var function = AuditOptions.ConvertStringToFunction(providedString);
            //Assert
            Assert.AreEqual(UserAuditFunctionEnum.None, function);
        }

        [TestMethod]
        public void AuditOptions_ConvertStringToFunction_WhenGiven_UnexpectedValue_ReturnsNone()
        {
            //Arrange
            string providedString = "$lartibartfast";
            //Act
            var function = AuditOptions.ConvertStringToFunction(providedString);
            //Assert
            Assert.AreEqual(UserAuditFunctionEnum.None, function);
        }

        [TestMethod]
        public void AuditOptions_ConvertStringToEnvironment_WhenGiven_dev_ReturnsDevEnum()
        {
            //Arrange
            string providedString = "dev";
            //Act
            var env = AuditOptions.ConvertStringToEnvironment(providedString);
            //Assert
            Assert.AreEqual(IRMAEnvironmentEnum.Dev, env);
        }

        [TestMethod]
        public void AuditOptions_ConvertStringToEnvironment_WhenGiven_Dev_ReturnsDevEnum()
        {
            //Arrange
            string providedString = "Dev";
            //Act
            var env = AuditOptions.ConvertStringToEnvironment(providedString);
            //Assert
            Assert.AreEqual(IRMAEnvironmentEnum.Dev, env);
        }

        [TestMethod]
        public void AuditOptions_ConvertStringToEnvironment_WhenGiven_DEV_ReturnsDevEnum()
        {
            //Arrange
            string providedString = "DEV";
            //Act
            var env = AuditOptions.ConvertStringToEnvironment(providedString);
            //Assert
            Assert.AreEqual(IRMAEnvironmentEnum.Dev, env);
        }

        [TestMethod]
        public void AuditOptions_ConvertStringToEnvironment_WhenGiven_qa_ReturnsDevEnum()
        {
            //Arrange
            string providedString = "qa";
            //Act
            var env = AuditOptions.ConvertStringToEnvironment(providedString);
            //Assert
            Assert.AreEqual(IRMAEnvironmentEnum.QualityAssurance, env);
        }

        [TestMethod]
        public void AuditOptions_ConvertStringToEnvironment_WhenGiven_Qa_ReturnsQAEnum()
        {
            //Arrange
            string providedString = "Qa";
            //Act
            var env = AuditOptions.ConvertStringToEnvironment(providedString);
            //Assert
            Assert.AreEqual(IRMAEnvironmentEnum.QualityAssurance, env);
        }

        [TestMethod]
        public void AuditOptions_ConvertStringToEnvironment_WhenGiven_QA_ReturnsQAEnum()
        {
            //Arrange
            string providedString = "QA";
            //Act
            var env = AuditOptions.ConvertStringToEnvironment(providedString);
            //Assert
            Assert.AreEqual(IRMAEnvironmentEnum.QualityAssurance, env);
        }

        [TestMethod]
        public void AuditOptions_ConvertStringToEnvironment_WhenGiven_QualityAssurance_ReturnsQAEnum()
        {
            //Arrange
            string providedString = "QualityAssurance";
            //Act
            var env = AuditOptions.ConvertStringToEnvironment(providedString);
            //Assert
            Assert.AreEqual(IRMAEnvironmentEnum.QualityAssurance, env);
        }

        [TestMethod]
        public void AuditOptions_ConvertStringToEnvironment_WhenGiven_test_ReturnsTestEnum()
        {
            //Arrange
            string providedString = "test";
            //Act
            var env = AuditOptions.ConvertStringToEnvironment(providedString);
            //Assert
            Assert.AreEqual(IRMAEnvironmentEnum.Test, env);
        }

        [TestMethod]
        public void AuditOptions_ConvertStringToEnvironment_WhenGiven_Test_ReturnsTestEnum()
        {
            //Arrange
            string providedString = "Test";
            //Act
            var env = AuditOptions.ConvertStringToEnvironment(providedString);
            //Assert
            Assert.AreEqual(IRMAEnvironmentEnum.Test, env);
        }

        [TestMethod]
        public void AuditOptions_ConvertStringToEnvironment_WhenGiven_TEST_ReturnsTestEnum()
        {
            //Arrange
            string providedString = "TEST";
            //Act
            var env = AuditOptions.ConvertStringToEnvironment(providedString);
            //Assert
            Assert.AreEqual(IRMAEnvironmentEnum.Test, env);
        }

        [TestMethod]
        public void AuditOptions_ConvertStringToEnvironment_WhenGiven_prod_ReturnsProdEnum()
        {
            //Arrange
            string providedString = "prod";
            //Act
            var env = AuditOptions.ConvertStringToEnvironment(providedString);
            //Assert
            Assert.AreEqual(IRMAEnvironmentEnum.Production, env);
        }

        [TestMethod]
        public void AuditOptions_ConvertStringToEnvironment_WhenGiven_Prod_ReturnsProdEnum()
        {
            //Arrange
            string providedString = "Prod";
            //Act
            var env = AuditOptions.ConvertStringToEnvironment(providedString);
            //Assert
            Assert.AreEqual(IRMAEnvironmentEnum.Production, env);
        }

        [TestMethod]
        public void AuditOptions_ConvertStringToEnvironment_WhenGiven_PROD_ReturnsProdEnum()
        {
            //Arrange
            string providedString = "PROD";
            //Act
            var env = AuditOptions.ConvertStringToEnvironment(providedString);
            //Assert
            Assert.AreEqual(IRMAEnvironmentEnum.Production, env);
        }

        [TestMethod]
        public void AuditOptions_ConvertStringToEnvironment_WhenGiven_EmptyString_ReturnsProdEnum()
        {
            //Arrange
            string providedString = String.Empty;
            //Act
            var env = AuditOptions.ConvertStringToEnvironment(providedString);
            //Assert
            Assert.AreEqual(IRMAEnvironmentEnum.Production, env);
        }

        [TestMethod]
        public void AuditOptions_ConvertStringToEnvironment_WhenGiven_NullString_ReturnsProdEnum()
        {
            //Arrange
            string providedString = null;
            //Act
            var env = AuditOptions.ConvertStringToEnvironment(providedString);
            //Assert
            Assert.AreEqual(IRMAEnvironmentEnum.Production, env);
        }

        [TestMethod]
        public void AuditOptions_ConvertStringToEnvironment_WhenGiven_UnexpectedValue_ReturnsProdEnum()
        {
            //Arrange
            string providedString = " When in the courfe of human eventf";
            //Act
            var env = AuditOptions.ConvertStringToEnvironment(providedString);
            //Assert
            Assert.AreEqual(IRMAEnvironmentEnum.Production, env);
        }

        [TestMethod]
        public void AuditOptions_ConvertIRMAEnvironmentToString_WhenGiven_DevReturns_DEV()
        {
            //Arrange
            var env = IRMAEnvironmentEnum.Dev;
            const string expected = "DEV";
            //Act
            var environmentString = AuditOptions.ConvertIRMAEnvironmentToString(env);
            //Assert
            Assert.AreEqual(expected, environmentString);
        }

        [TestMethod]
        public void AuditOptions_ConvertIRMAEnvironmentToString_WhenGiven_Test_Returns_TEST()
        {
            //Arrange
            var env = IRMAEnvironmentEnum.Test;
            const string expected = "TEST";
            //Act
            var environmentString = AuditOptions.ConvertIRMAEnvironmentToString(env);
            //Assert
            Assert.AreEqual(expected, environmentString);
        }

        [TestMethod]
        public void AuditOptions_ConvertIRMAEnvironmentToString_WhenGiven_QualityAssurance_Returns_QA()
        {
            //Arrange
            var env = IRMAEnvironmentEnum.QualityAssurance;
            const string expected = "QA";
            //Act
            var environmentString = AuditOptions.ConvertIRMAEnvironmentToString(env);
            //Assert
            Assert.AreEqual(expected, environmentString);
        }

        [TestMethod]
        public void AuditOptions_ConvertIRMAEnvironmentToString_WhenGiven_Production_Returns_PROD()
        {
            //Arrange
            var env = IRMAEnvironmentEnum.Production;
            const string expected = "PROD";
            //Act
            var environmentString = AuditOptions.ConvertIRMAEnvironmentToString(env);
            //Assert
            Assert.AreEqual(expected, environmentString);
        }

        [TestMethod]
        public void AuditOptions_ConvertIRMAEnvironmentToString_WhenGiven_UnexpectedValue_PROD()
        {
            var env = (IRMAEnvironmentEnum)9;
            const string expected = "PROD";
            //Act
            var environmentString = AuditOptions.ConvertIRMAEnvironmentToString(env);
            //Assert
            Assert.AreEqual(expected, environmentString);
        }
    }
}
