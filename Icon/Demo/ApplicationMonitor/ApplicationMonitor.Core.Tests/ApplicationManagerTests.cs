using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ApplicationMonitor.Core.Models;

namespace ApplicationMonitor.Core.Tests
{
    [TestClass]
    public class ApplicationManagerTests
    {
        [TestMethod]
        public void Load_PopulatesApplications()
        {
            //When
            var application = ApplicationManager.GetApplication(@"TestFiles/test_applications.xml", "API Controller", false);

            //Then
            //var application1 = ApplicationManager.Applications["API Controller"][0];
            //Assert.AreEqual("API Controller", application1.Name);
            //Assert.AreEqual("vm-icon-test1", application1.MachineName);
            //Assert.AreEqual(@"E:\Icon\API Controller Phase 2\Test", application1.DirectoryPath);
            //Assert.AreEqual("Icon.ApiController.Controller.exe.config", application1.ConfigFileName);

            //var application2 = ApplicationManager.Applications["API Controller"][1];
            //Assert.AreEqual("API Controller", application2.Name);
            //Assert.AreEqual("vm-icon-test2", application2.MachineName);
            //Assert.AreEqual(@"E:\Icon\API Controller Phase 2\Test", application2.DirectoryPath);
            //Assert.AreEqual("Icon.ApiController.Controller.exe.config", application2.ConfigFileName);

            //var application3 = ApplicationManager.Applications["GloCon"][0];
            //Assert.AreEqual("GloCon", application3.Name);
            //Assert.AreEqual("vm-icon-test1", application3.MachineName);
            //Assert.AreEqual(@"E:\Icon\Global Controller\Test", application3.DirectoryPath);
            //Assert.AreEqual("GlobalEventController.Controller.exe", application3.ConfigFileName);
        }
    }
}
