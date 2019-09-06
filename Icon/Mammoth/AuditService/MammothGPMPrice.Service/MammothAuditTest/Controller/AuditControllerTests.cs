using System;
using System.IO;
using System.Linq;
using System.Configuration;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Audit.Test
{
    [TestClass]
    public class PriceDataControllerTests
    {
        AuditConfigItem[] audits;
        UploadConfigItem[] uploads;

        [TestInitialize]
        public void Initialize()
        {
            this.audits = AuditConfigSection.Config.SettingsList.ToArray();
            this.uploads = UploadConfigSection.Config.SettingsList.ToArray();
        }

        [TestMethod]
        public void Verify_AllAuditItemsConfigItems_Has2Items()
        {
            //Given
            //AuditItemsConfigItems specified in config file. Currently one active and one inactive.

            //Then
            Assert.AreEqual(this.audits.Length, 2, "AuditConfigSection is invalid. Expecting 2 items.");
        }

        [TestMethod]
        public void Verify_ActiveAuditItemsConfigItems_Has1Item()
        {
            //Given
            //AuditItemsConfigItems specified in config file. Currently one active and one inactive.
            //Then
            Assert.AreEqual(this.audits.Where(x => x.IsActive).Count(), 1, "AuditConfigSection is invalid. Expecting one active item");
        }

        [TestMethod]
        public void Verify_UploadConfigItems_Has1Item()
        {
            //Given
            //AuditItemsConfigItems specified in config file. Currently one active and one inactive.
            //Then

            Assert.AreEqual(this.uploads.Length, 1, "UploadConfigSection is invalid. Expecting one item.");
        }

        [TestMethod]
        [Ignore] // This test seems to take a very long time so ignoring it for now until we refactor
        public void Execute_AditController()
        {
            //Given
            var sqlConnection = ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString;
            var dirPath = ConfigurationManager.AppSettings["TempDir"];
            if (string.IsNullOrWhiteSpace(dirPath)) { dirPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "AuditService"); }

            var Regions = new HashSet<string>(){"FL" };
            var audit = audits.Where(x => x.IsActive).FirstOrDefault();
            if (audit is null) throw new Exception("Audit item is not found");

            if (Directory.Exists(dirPath))
            {
                Directory.Delete(dirPath, true);
            }
            Directory.CreateDirectory(dirPath);

            var controller = new AuditController(new SpecInfo(configItem: audit,
                                                              profileItem: uploads.First(),
                                                              sourceRegions: Regions,
                                                              tempDirPath: dirPath,
                                                              sqlConnection: ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString));
            //When
            controller.Execute();

            //Then
            Assert.AreEqual(Directory.GetFiles(dirPath).Count(), 1);
            Directory.Delete(dirPath, true);
        }
    }
}