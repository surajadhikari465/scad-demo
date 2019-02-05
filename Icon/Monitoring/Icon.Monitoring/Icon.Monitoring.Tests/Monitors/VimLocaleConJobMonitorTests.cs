using Icon.Logging;
using Icon.Common.DataAccess;
using Icon.Monitoring.Common.Opsgenie;
using Icon.Monitoring.Common.Settings;
using Icon.Monitoring.DataAccess.Queries;
using Icon.Monitoring.Monitors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Icon.Monitoring.DataAccess.Model;
using Icon.Monitoring.DataAccess;
using System.Data.SqlClient;
using System.Configuration;

namespace Icon.Monitoring.Tests.Monitors
{
    [TestClass]
    public class VimLocaleConJobMonitorTests
    {
        private Mock<IMonitorSettings> mockSettings;
        private Mock<IVimLocaleConJobMonitorSettings> mockVimLocaleConJobMonitorSettings;
        private Mock<IQueryHandler<GetLatestAppLogByAppNameParameters, AppLog>> mockGetVimLocaleConJobMonitorStatusQueryHandler;
        private Mock<IOpsgenieTrigger> mockOpsgenieTrigger;
        private Mock<ILogger> mockLogger;
        private VIMLocaleControllerMonitor monitor;
        private const string AppName = "Vim Locale Controller";
        private AppLog applog;
        private SqlDbProvider db;

        [TestInitialize]
        public void Initialize()
        {
            mockSettings = new Mock<IMonitorSettings>();
            mockVimLocaleConJobMonitorSettings = new Mock<IVimLocaleConJobMonitorSettings>();
            mockOpsgenieTrigger = new Mock<IOpsgenieTrigger>();
            mockGetVimLocaleConJobMonitorStatusQueryHandler = new Mock<IQueryHandler<GetLatestAppLogByAppNameParameters, AppLog>>();
            mockLogger = new Mock<ILogger>();
            applog = new AppLog();

            this.db = new SqlDbProvider();
            this.db.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Icon"].ConnectionString);

            monitor = new VIMLocaleControllerMonitor(
                mockSettings.Object,
                mockVimLocaleConJobMonitorSettings.Object,
               mockGetVimLocaleConJobMonitorStatusQueryHandler.Object,
               mockOpsgenieTrigger.Object,
                mockLogger.Object
                );
        }

        [TestMethod]
        public void TimedCheckStatusAndNotify_EnableVimLocaleConJobMonitorIsFalse_DoesNotCheckStatusAndNotify()
        {
            //Given
            mockVimLocaleConJobMonitorSettings.SetupGet(m => m.EnableVimLocaleConJobMonitor).Returns(false);
          
            //When
            monitor.CheckStatusAndNotify();

            //Then
            mockVimLocaleConJobMonitorSettings.VerifyGet(m => m.MinutesAllowedSinceLastVimLocaleCon, Times.Never);
        }

        [TestMethod]
        public void TimedCheckStatusAndNotify_VimLocaleConServiceNotRunning_ShouldSendsOpsgenieAlert()
        {
            //Given -- set last log date to be 2 hours before the current time
            SetUpSettingsValue(true, 10, -2);
   
           //When
            monitor.CheckStatusAndNotify();

            //Then
            mockOpsgenieTrigger.Verify(m => m.TriggerAlert(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Once);
        }

        [TestMethod]
        public void TimedCheckStatusAndNotify_VimLocaleConServiceRunning_NoOpsgenieyAlert()
        {
            //Given -- set last log date to be 2 hours before the current time
            SetUpSettingsValue(true, 70, -1);

            //When
            monitor.CheckStatusAndNotify();

            //Then
            mockOpsgenieTrigger.Verify(m => m.TriggerAlert(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Never);
        }
        private void SetUpSettingsValue(Boolean enableVimLocalegConJobMonitor, int minutesAllowedSinceLastVimLocaleCon, int addHours)
        {
            mockVimLocaleConJobMonitorSettings.SetupGet(m => m.EnableVimLocaleConJobMonitor).Returns(enableVimLocalegConJobMonitor);
             mockVimLocaleConJobMonitorSettings.SetupGet(m => m.MinutesAllowedSinceLastVimLocaleCon).Returns(minutesAllowedSinceLastVimLocaleCon);
            applog.LogDate = DateTime.Now.AddHours(addHours);
            mockGetVimLocaleConJobMonitorStatusQueryHandler.Setup(m => m.Search(It.IsAny<GetLatestAppLogByAppNameParameters>()))
            .Returns(applog);
       
        }

    }
}
