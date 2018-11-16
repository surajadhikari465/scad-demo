using Icon.Monitoring.Common.Settings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NodaTime.Text;
using System;
using System.Linq;

namespace Icon.Monitoring.Common.Tests.Settings
{
    [TestClass]
    public class MammothExpiringTprServiceMonitorSettingsTests
    {
        private MammothExpiringTprServiceMonitorSettings settings;

        [TestInitialize]
        public void Initialize()
        {
            settings = new MammothExpiringTprServiceMonitorSettings();
        }

        [TestMethod]
        public void ExpiringTprService_Load_ConfigurationSettingsExist_ShouldPopulateTheSettings()
        {
            //When
            settings.Load();

            //Then
            Assert.AreEqual(11, settings.ExpiringTprServiceMonitorEnabledByRegion.Count());
            Assert.AreEqual(true, settings.ExpiringTprServiceMonitorEnabledByRegion["FL"]);
            Assert.AreEqual(true, settings.ExpiringTprServiceMonitorEnabledByRegion["MA"]);
            Assert.AreEqual(true, settings.ExpiringTprServiceMonitorEnabledByRegion["MW"]);
            Assert.AreEqual(true, settings.ExpiringTprServiceMonitorEnabledByRegion["NA"]);
            Assert.AreEqual(true, settings.ExpiringTprServiceMonitorEnabledByRegion["NC"]);
            Assert.AreEqual(true, settings.ExpiringTprServiceMonitorEnabledByRegion["NE"]);
            Assert.AreEqual(true, settings.ExpiringTprServiceMonitorEnabledByRegion["PN"]);
            Assert.AreEqual(true, settings.ExpiringTprServiceMonitorEnabledByRegion["RM"]);
            Assert.AreEqual(true, settings.ExpiringTprServiceMonitorEnabledByRegion["SO"]);
            Assert.AreEqual(true, settings.ExpiringTprServiceMonitorEnabledByRegion["SP"]);
            Assert.AreEqual(true, settings.ExpiringTprServiceMonitorEnabledByRegion["SW"]);

            Assert.AreEqual(11, settings.ExpiringTprServiceCompletionUtcTimeByRegion.Count());
            Assert.AreEqual(DateTime.UtcNow.Date.Add(new TimeSpan(6, 12, 00)), settings.ExpiringTprServiceCompletionUtcTimeByRegion["FL"]);
            Assert.AreEqual(DateTime.UtcNow.Date.Add(new TimeSpan(6, 14, 00)), settings.ExpiringTprServiceCompletionUtcTimeByRegion["MA"]);
            Assert.AreEqual(DateTime.UtcNow.Date.Add(new TimeSpan(6, 16, 00)), settings.ExpiringTprServiceCompletionUtcTimeByRegion["MW"]);
            Assert.AreEqual(DateTime.UtcNow.Date.Add(new TimeSpan(7, 18, 00)), settings.ExpiringTprServiceCompletionUtcTimeByRegion["NA"]);
            Assert.AreEqual(DateTime.UtcNow.Date.Add(new TimeSpan(7, 20, 00)), settings.ExpiringTprServiceCompletionUtcTimeByRegion["NC"]);
            Assert.AreEqual(DateTime.UtcNow.Date.Add(new TimeSpan(7, 22, 00)), settings.ExpiringTprServiceCompletionUtcTimeByRegion["NE"]);
            Assert.AreEqual(DateTime.UtcNow.Date.Add(new TimeSpan(8, 24, 00)), settings.ExpiringTprServiceCompletionUtcTimeByRegion["PN"]);
            Assert.AreEqual(DateTime.UtcNow.Date.Add(new TimeSpan(8, 26, 00)), settings.ExpiringTprServiceCompletionUtcTimeByRegion["RM"]);
            Assert.AreEqual(DateTime.UtcNow.Date.Add(new TimeSpan(9, 28, 00)), settings.ExpiringTprServiceCompletionUtcTimeByRegion["SO"]);
            Assert.AreEqual(DateTime.UtcNow.Date.Add(new TimeSpan(9, 30, 00)), settings.ExpiringTprServiceCompletionUtcTimeByRegion["SP"]);
            Assert.AreEqual(DateTime.UtcNow.Date.Add(new TimeSpan(10, 32, 00)), settings.ExpiringTprServiceCompletionUtcTimeByRegion["SW"]);
        }
    }
}
