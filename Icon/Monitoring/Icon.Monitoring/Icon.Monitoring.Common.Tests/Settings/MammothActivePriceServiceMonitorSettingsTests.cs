using Icon.Monitoring.Common.Settings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NodaTime.Text;
using System;
using System.Linq;

namespace Icon.Monitoring.Common.Tests.Settings
{
    [TestClass]
    public class MammothActivePriceServiceMonitorSettingsTests
    {
        private MammothActivePriceServiceMonitorSettings settings;

        [TestInitialize]
        public void Initialize()
        {
            settings = new MammothActivePriceServiceMonitorSettings();
        }

        [TestMethod]
        public void ActivePriceServiceMonitorSettings_Load_ConfigurationSettingsExist_ShouldPopulateTheSettings()
        {
            //When
            settings.Load();

            //Then
            Assert.AreEqual(11, settings.ActivePriceServiceMonitorEnabledByRegion.Count());
            Assert.AreEqual(true, settings.ActivePriceServiceMonitorEnabledByRegion["FL"]);
            Assert.AreEqual(true, settings.ActivePriceServiceMonitorEnabledByRegion["MA"]);
            Assert.AreEqual(true, settings.ActivePriceServiceMonitorEnabledByRegion["MW"]);
            Assert.AreEqual(true, settings.ActivePriceServiceMonitorEnabledByRegion["NA"]);
            Assert.AreEqual(true, settings.ActivePriceServiceMonitorEnabledByRegion["NC"]);
            Assert.AreEqual(true, settings.ActivePriceServiceMonitorEnabledByRegion["NE"]);
            Assert.AreEqual(true, settings.ActivePriceServiceMonitorEnabledByRegion["PN"]);
            Assert.AreEqual(true, settings.ActivePriceServiceMonitorEnabledByRegion["RM"]);
            Assert.AreEqual(true, settings.ActivePriceServiceMonitorEnabledByRegion["SO"]);
            Assert.AreEqual(true, settings.ActivePriceServiceMonitorEnabledByRegion["SP"]);
            Assert.AreEqual(true, settings.ActivePriceServiceMonitorEnabledByRegion["SW"]);

            Assert.AreEqual(11, settings.ActivePriceServiceCompletionUtcTimeByRegion.Count());
            Assert.AreEqual(DateTime.UtcNow.Date.Add(new TimeSpan(4, 5, 00)), settings.ActivePriceServiceCompletionUtcTimeByRegion["FL"]);
            Assert.AreEqual(DateTime.UtcNow.Date.Add(new TimeSpan(5, 10, 00)), settings.ActivePriceServiceCompletionUtcTimeByRegion["MA"]);
            Assert.AreEqual(DateTime.UtcNow.Date.Add(new TimeSpan(6, 15, 00)), settings.ActivePriceServiceCompletionUtcTimeByRegion["MW"]);
            Assert.AreEqual(DateTime.UtcNow.Date.Add(new TimeSpan(7, 20, 00)), settings.ActivePriceServiceCompletionUtcTimeByRegion["NA"]);
            Assert.AreEqual(DateTime.UtcNow.Date.Add(new TimeSpan(8, 25, 00)), settings.ActivePriceServiceCompletionUtcTimeByRegion["NC"]);
            Assert.AreEqual(DateTime.UtcNow.Date.Add(new TimeSpan(9, 30, 00)), settings.ActivePriceServiceCompletionUtcTimeByRegion["NE"]);
            Assert.AreEqual(DateTime.UtcNow.Date.Add(new TimeSpan(10, 35, 00)), settings.ActivePriceServiceCompletionUtcTimeByRegion["PN"]);
            Assert.AreEqual(DateTime.UtcNow.Date.Add(new TimeSpan(11, 40, 00)), settings.ActivePriceServiceCompletionUtcTimeByRegion["RM"]);
            Assert.AreEqual(DateTime.UtcNow.Date.Add(new TimeSpan(12, 45, 00)), settings.ActivePriceServiceCompletionUtcTimeByRegion["SO"]);
            Assert.AreEqual(DateTime.UtcNow.Date.Add(new TimeSpan(13, 50, 00)), settings.ActivePriceServiceCompletionUtcTimeByRegion["SP"]);
            Assert.AreEqual(DateTime.UtcNow.Date.Add(new TimeSpan(14, 55, 00)), settings.ActivePriceServiceCompletionUtcTimeByRegion["SW"]);
        }
    }
}
