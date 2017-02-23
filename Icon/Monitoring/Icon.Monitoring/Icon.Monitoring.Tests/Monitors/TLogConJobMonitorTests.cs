using Icon.Logging;
using Icon.Common.DataAccess;
using Icon.Monitoring.Common.PagerDuty;
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
    public class TLogConJobMonitorTests
    {
        private Mock<IMonitorSettings> mockSettings;
        private Mock<ITLogConJobMonitorSettings> mockTLogConMonitorSettings;
        private Mock<IQueryHandler<GetLatestAppLogByAppNameParameters, AppLog>> mockGetTLogConJobMonitorStatusQueryHandler;
        private Mock<IQueryHandler<GetItemMovementTableRowCountParameters, int>> mockGetItemMovementTableRowCountQueryHandler;
        private Mock<IPagerDutyTrigger> mockPagerDutyTrigger;
        private Mock<ILogger> mockLogger;
        private TLogConJobMonitor monitor;
        private const string AppName = "TLog Controller";
        private AppLog applog;
        private SqlDbProvider db;

        [TestInitialize]
        public void Initialize()
        {
            mockSettings = new Mock<IMonitorSettings>();
            mockTLogConMonitorSettings = new Mock<ITLogConJobMonitorSettings>();
            mockPagerDutyTrigger = new Mock<IPagerDutyTrigger>();
            mockGetTLogConJobMonitorStatusQueryHandler = new Mock<IQueryHandler<GetLatestAppLogByAppNameParameters, AppLog>>();
            mockGetItemMovementTableRowCountQueryHandler = new Mock<IQueryHandler<GetItemMovementTableRowCountParameters, int>>();
            mockLogger = new Mock<ILogger>();
            applog = new AppLog();

            this.db = new SqlDbProvider();
            this.db.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Icon"].ConnectionString);

            monitor = new TLogConJobMonitor(
                mockSettings.Object,
                mockTLogConMonitorSettings.Object,
               mockGetTLogConJobMonitorStatusQueryHandler.Object,
                mockGetItemMovementTableRowCountQueryHandler.Object,
               mockPagerDutyTrigger.Object,
                mockLogger.Object
                );
        }


        // EnableTLogConJobMonitor flag is false--it should not monitor
        [TestMethod]
        public void TimedCheckStatusAndNotify_EnableTLogConJobMonitorIsFalse_DoesNotCheckStatusAndNotify()
        {
            //Given
            mockTLogConMonitorSettings.SetupGet(m => m.EnableTLogConJobMonitor).Returns(false);
          
            //When
            monitor.CheckStatusAndNotify();

            //Then
            mockTLogConMonitorSettings.VerifyGet(m => m.MinutesAllowedSinceLastTLogCon, Times.Never);
        }

        // EnableTLogConJobMonitor flag is true--it should  monitor
        [TestMethod]
        public void TimedCheckStatusAndNotify_EnableTLogConJobMonitorIsTrue_DoesCheckStatusAndNotify()
        {
            //Given
            mockTLogConMonitorSettings.SetupGet(m => m.EnableTLogConJobMonitor).Returns(true);
            mockTLogConMonitorSettings.SetupGet(m => m.MinutesAllowedSinceLastTLogCon).Returns(30);
            applog.LogDate = DateTime.Now.AddHours(-1);
          mockGetTLogConJobMonitorStatusQueryHandler.Setup(m => m.Search(It.IsAny<GetLatestAppLogByAppNameParameters>()))
            .Returns(applog);

            //When
            monitor.CheckStatusAndNotify();

            //Then
            mockTLogConMonitorSettings.VerifyGet(m => m.MinutesAllowedSinceLastTLogCon, Times.Once);
        }

        // EnableTLogConJobMonitor flag is true--Set up so that last log by Tlog controller is more than configured one --will send pager alert
        [TestMethod]
        public void TimedCheckStatusAndNotify_TLogConServiceNotRunning()
        {
            //Given
            SetUpSettingsValue(true, false, 10, -2, 500000, 400000);
   
            monitor.AlertSentForItemTableMovement = false;
            //When
            monitor.CheckStatusAndNotify();

            //Then
            mockPagerDutyTrigger.Verify(m => m.TriggerIncident(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Once);
        }

        // EnableTLogConJobMonitor flag is true--Set up so that number of row in app movement table is more than configured one --will send pager alert
        [TestMethod]
        public void TimedCheckStatusAndNotify_TLogConServiceRunning_ItemMovementRowsCountMoreThanConfigured()
        {
            //Given
            SetUpSettingsValue(true, true, 200, -1, 500000, 1000000);
            monitor.AlertSentForItemTableMovement = false;

            //When
            monitor.CheckStatusAndNotify();

            //Then
            mockPagerDutyTrigger.Verify(m => m.TriggerIncident(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Once);
        }

        // EnableTLogConJobMonitor flag is true--Set up so that both app movement row count and last log valies are less than configurd value
        // in this case program will not send pager alert
        [TestMethod]
        public void TimedCheckStatusAndNotify_TLogConServiceRunning_ItemMovementRowsCountLessThanConfigured()
        {
            //Given
            SetUpSettingsValue(true, true, 200, -1, 500000, 400000);
            monitor.AlertSentForItemTableMovement = false;

            //When
            monitor.CheckStatusAndNotify();

            //Then
            mockPagerDutyTrigger.Verify(m => m.TriggerIncident(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Never);
        }

        // make sure it only send pager duty alert once if row count in app movement table is greater than configured value
        [TestMethod]
        public void TimedCheckStatusAndNotify_TLogConServiceRunning_ItemMovementRowsCountMoreThanConfigured_PagerAlertAlreadySend()
        {
            //Given
            SetUpSettingsValue(true, true, 200, -1, 500000, 1000000);
            monitor.AlertSentForItemTableMovement = false;

            //When
            // call this method twice --alert should only be send first time.
            monitor.CheckStatusAndNotify();
            monitor.CheckStatusAndNotify();

            //Then
            mockPagerDutyTrigger.Verify(m => m.TriggerIncident(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Once);
        }

        // both conditions fail--it should send pager duty alert only once
        [TestMethod]
        public void TimedCheckStatusAndNotify_TLogConServiceNotRunning_ItemMovementRowsCountMoreThanConfigured()
        {
            //Given   
            SetUpSettingsValue(true, true, 10, -1, 500000, 1000000);
            monitor.AlertSentForItemTableMovement = false;

            //When
            monitor.CheckStatusAndNotify();
   
            //Then
            mockPagerDutyTrigger.Verify(m => m.TriggerIncident(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Once);
        }


        private void SetUpSettingsValue(Boolean EnableTLogConJobMonitor, Boolean EnableItemMovementTableCheck, int MinutesAllowedSinceLastTLogCon, int AddHours, int ItemMovementMaxRows, int setUpItemMovementRows)
        {
            mockTLogConMonitorSettings.SetupGet(m => m.EnableTLogConJobMonitor).Returns(EnableTLogConJobMonitor);
            mockTLogConMonitorSettings.SetupGet(m => m.EnableItemMovementTableCheck).Returns(EnableItemMovementTableCheck);
            mockTLogConMonitorSettings.SetupGet(m => m.MinutesAllowedSinceLastTLogCon).Returns(MinutesAllowedSinceLastTLogCon);
            applog.LogDate = DateTime.Now.AddHours(AddHours);
            mockGetTLogConJobMonitorStatusQueryHandler.Setup(m => m.Search(It.IsAny<GetLatestAppLogByAppNameParameters>()))
            .Returns(applog);
            mockTLogConMonitorSettings.SetupGet(m => m.ItemMovementMaximumRows).Returns(ItemMovementMaxRows);
            mockGetItemMovementTableRowCountQueryHandler.Setup(m => m.Search(It.IsAny<GetItemMovementTableRowCountParameters>()))
          .Returns(setUpItemMovementRows);

        }

    }
}
