using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WholeFoods.Common.IRMALib;

namespace IRMALibTest
{
    [TestClass]
    public class ConfigRepositoryTests: IRMALibTestBase
    {
        protected ConfigRepository configRepo;

        [TestInitialize]
        public void TestInit()
        {
            configRepo = new ConfigRepository(base.ConnectionString_FLD);
        }

        [TestMethod]
        public void CanLoadConfig()
        {
            bool configLoaded = configRepo.LoadConfig(base.AppId, base.EnvId);
            Assert.IsTrue(configLoaded);
        }

        [TestMethod]
        public void CanFindBasePath()
        {
            bool configLoaded = configRepo.LoadConfig(base.AppId, base.EnvId);
            string basePath = "";
            bool gotBasePath = configRepo.ConfigurationGetValue("BasePath", ref basePath);
            Assert.IsTrue(gotBasePath);
        }

        [TestMethod]
        public void CanUpdateKey()
        {
            // add key
            bool canUpdate = configRepo.UpdateKeyValue(new Guid("2898A7FC-1CD3-4132-8A69-ED07CC526B14"), new Guid("20C5DDAC-659C-4B81-84F6-5F79CC390D10"), "test", "test", 0);
            Assert.IsTrue(canUpdate);
        }
    }
}
