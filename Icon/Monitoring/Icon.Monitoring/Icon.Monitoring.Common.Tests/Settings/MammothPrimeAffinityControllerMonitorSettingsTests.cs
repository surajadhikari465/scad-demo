using Icon.Monitoring.Common.Settings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NodaTime.Text;
using System;
using System.Linq;

namespace Icon.Monitoring.Common.Tests.Settings
{
    [TestClass]
    public class MammothPrimeAffinityControllerMonitorSettingsTests
    {
        private MammothPrimeAffinityControllerMonitorSettings settings;

        [TestInitialize]
        public void Initialize()
        {
            settings = new MammothPrimeAffinityControllerMonitorSettings();
        }

        [TestMethod]
        public void PrimeAffinityController_Load_ConfigurationSettingsExist_ShouldPopulateTheSettings()
        {
            //When
            settings.Load();

            //Then
            Assert.AreEqual(11, settings.PrimeAffinityControllerMonitorEnabledByRegion.Count());
            Assert.AreEqual(true, settings.PrimeAffinityControllerMonitorEnabledByRegion["FL"]);
            Assert.AreEqual(true, settings.PrimeAffinityControllerMonitorEnabledByRegion["MA"]);
            Assert.AreEqual(true, settings.PrimeAffinityControllerMonitorEnabledByRegion["MW"]);
            Assert.AreEqual(true, settings.PrimeAffinityControllerMonitorEnabledByRegion["NA"]);
            Assert.AreEqual(true, settings.PrimeAffinityControllerMonitorEnabledByRegion["NC"]);
            Assert.AreEqual(true, settings.PrimeAffinityControllerMonitorEnabledByRegion["NE"]);
            Assert.AreEqual(true, settings.PrimeAffinityControllerMonitorEnabledByRegion["PN"]);
            Assert.AreEqual(true, settings.PrimeAffinityControllerMonitorEnabledByRegion["RM"]);
            Assert.AreEqual(true, settings.PrimeAffinityControllerMonitorEnabledByRegion["SO"]);
            Assert.AreEqual(true, settings.PrimeAffinityControllerMonitorEnabledByRegion["SP"]);
            Assert.AreEqual(true, settings.PrimeAffinityControllerMonitorEnabledByRegion["SW"]);

            Assert.AreEqual(11, settings.PrimeAffinityPsgCompletionUtcTimeByRegion.Count());
            Assert.AreEqual(DateTime.UtcNow.Date.Add(new TimeSpan(7, 10, 00)), settings.PrimeAffinityPsgCompletionUtcTimeByRegion["FL"]);
            Assert.AreEqual(DateTime.UtcNow.Date.Add(new TimeSpan(7, 11, 00)), settings.PrimeAffinityPsgCompletionUtcTimeByRegion["MA"]);
            Assert.AreEqual(DateTime.UtcNow.Date.Add(new TimeSpan(7, 22, 00)), settings.PrimeAffinityPsgCompletionUtcTimeByRegion["MW"]);
            Assert.AreEqual(DateTime.UtcNow.Date.Add(new TimeSpan(7, 43, 00)), settings.PrimeAffinityPsgCompletionUtcTimeByRegion["NA"]);
            Assert.AreEqual(DateTime.UtcNow.Date.Add(new TimeSpan(7, 44, 00)), settings.PrimeAffinityPsgCompletionUtcTimeByRegion["NC"]);
            Assert.AreEqual(DateTime.UtcNow.Date.Add(new TimeSpan(7, 15, 00)), settings.PrimeAffinityPsgCompletionUtcTimeByRegion["NE"]);
            Assert.AreEqual(DateTime.UtcNow.Date.Add(new TimeSpan(8, 16, 00)), settings.PrimeAffinityPsgCompletionUtcTimeByRegion["PN"]);
            Assert.AreEqual(DateTime.UtcNow.Date.Add(new TimeSpan(9, 17, 00)), settings.PrimeAffinityPsgCompletionUtcTimeByRegion["RM"]);
            Assert.AreEqual(DateTime.UtcNow.Date.Add(new TimeSpan(7, 18, 00)), settings.PrimeAffinityPsgCompletionUtcTimeByRegion["SO"]);
            Assert.AreEqual(DateTime.UtcNow.Date.Add(new TimeSpan(9, 19, 00)), settings.PrimeAffinityPsgCompletionUtcTimeByRegion["SP"]);
            Assert.AreEqual(DateTime.UtcNow.Date.Add(new TimeSpan(7, 40, 00)), settings.PrimeAffinityPsgCompletionUtcTimeByRegion["SW"]);
        }
    }
}
