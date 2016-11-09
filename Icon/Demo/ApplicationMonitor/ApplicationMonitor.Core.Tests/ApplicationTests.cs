using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ApplicationMonitor.Core.Models;
using Microsoft.Win32.TaskScheduler;
using System.Linq;

namespace ApplicationMonitor.Core.Tests
{
    [TestClass]
    public class ApplicationTests
    {
        [TestMethod]
        public void MyTestMethod()
        {
            using (TaskService ts = new TaskService("vm-icon-test1"))
            {
                var tasks = ts.AllTasks.Where(t => t.Name.StartsWith("API Controller")).ToList();
            }
        }
    }
    //    [TestClass]
    //    public class ApplicationTests
    //    {
    //        private Application application;

    //        [TestInitialize]
    //        public void Initialize()
    //        {
    //            application = new Application
    //            {
    //                Name = "Test Application",
    //                MachineName = "vm-icon-test1",
    //                DirectoryPath = @"Icon\API Controller Phase 2\Test",
    //                ConfigFileName = "Icon.ApiController.Controller.exe.config",
    //                Type = ApplicationTypes.ScheduledTask
    //            };
    //        }

    //        [TestMethod]
    //        public void LoadAppSettings_ShouldPopulateAppSettings()
    //        {
    //            //When
    //            application.LoadAppSettings();

    //            //Then
    //            Assert.AreEqual(35, application.AppSettings.Count);
    //        }

    //        [TestMethod]
    //        public void LoadStatus_ShouldPopulateStatus()
    //        {
    //            //When
    //            application.LoadStatus();

    //            //Then
    //            Console.WriteLine(application.Status);
    //        }
    //    }
}
