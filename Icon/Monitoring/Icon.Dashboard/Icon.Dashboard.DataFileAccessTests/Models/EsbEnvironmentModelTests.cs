namespace Icon.Dashboard.DataFileAccess.Tests.Models
{
    using DataFileAccess.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    [TestClass]
    public class EsbEnvironmentModelTests
    {

        //[TestMethod]
        //public void WhenExisitngApplicationInEsbEnvironmentModel_ThenCallAddApplication_ShouldAddToApplications()
        //{
        //    // Given
        //    const string name1 = "icon.App.Name.1";
        //    const string server1 = "vvv-xxxx-11";
        //    const string configPath1 = @"\\vvv-xxxx-11\e$\Stuff\My Thingy_NA\Argle.Bargle.Controller.exe.config";
        //    const string displayName1 = "App 1";
        //    const string name2 = "icon.App.Name.2";
        //    const string server2 = "vvv-yyyy-22";
        //    const string configPath2 = @"\\vvv-xxxx-22\e$\Stuff\My Thingy_NA\Argle.Bargle.Controller.exe.config";
        //    const string displayName2 = "App 2";
            
            //var environment = new EsbEnvironmentDefinition()
            //{
            //    AppSummaries = new List<IconApplicationSummary>()
            //    {
            //        new IconApplicationSummary(name1, server1, configPath1, true, displayName1)
            //    }
            //};

            //// When
            //var app = environment.AddApplication(name2, server2, configPath2, displayName2);

            //// Then
            //Assert.IsNotNull(environment.AppSummaries);
            //Assert.AreEqual(2, environment.AppSummaries.Count);
            //Assert.IsNotNull(environment.AppSummaries[0]);
            //Assert.AreEqual(name1, environment.AppSummaries[0].Name);
            //Assert.AreEqual(server1, environment.AppSummaries[0].Server);
            //Assert.IsNotNull(environment.AppSummaries[1]);
            //Assert.AreEqual(name2, environment.AppSummaries[1].Name);
            //Assert.AreEqual(server2, environment.AppSummaries[1].Server);
        //}
    }
}
