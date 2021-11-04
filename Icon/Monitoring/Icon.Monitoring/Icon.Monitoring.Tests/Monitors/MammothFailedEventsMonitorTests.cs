using Icon.Logging;
using Icon.Monitoring.Common.Opsgenie;
using Icon.Monitoring.Common.Settings;
using Icon.Monitoring.DataAccess.Model;
using Icon.Monitoring.DataAccess.Queries;
using Icon.Monitoring.Monitors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace Icon.Monitoring.Tests.Monitors
{
    [TestClass]
    public class MammothFailedEventsMonitorTests
    {
        private MammothFailedEventsMonitor monitor;
        private Mock<IMonitorSettings> mockSettings;
        private Mock<IQueryByRegionHandler<GetMammothFailedEventsParameters, List<MammothFailedEvent>>> mockGetMammothFailedEventsQueryHandler;
        private Mock<IOpsgenieTrigger> mockOpsgenieTrigger;
        private Mock<ILogger> mockLogger;

        [TestInitialize]
        public void Initialize()
        {
            mockSettings = new Mock<IMonitorSettings>();
            mockGetMammothFailedEventsQueryHandler = new Mock<IQueryByRegionHandler<GetMammothFailedEventsParameters, List<MammothFailedEvent>>>();
            mockOpsgenieTrigger = new Mock<IOpsgenieTrigger>();
            mockLogger = new Mock<ILogger>();

            monitor = new MammothFailedEventsMonitor(
                mockSettings.Object,
                mockGetMammothFailedEventsQueryHandler.Object,
                mockOpsgenieTrigger.Object,
                mockLogger.Object);

            monitor.ByPassConfiguredRunInterval = true;
            mockSettings.Setup(m => m.MonitorTimers)
                .Returns(new Dictionary<string, TimeSpan>
                {
                    { nameof(MammothFailedEventsMonitor) + "Timer", TimeSpan.FromMinutes(15) }
                });
        }

        [TestMethod]
        public void MammothFailedEventsMonitorCheckStatusAndNotify_NoFailedEvents_NoOpsGenieAlert()
        {
            //Given
            mockGetMammothFailedEventsQueryHandler.Setup(m => m.Search(It.IsAny<GetMammothFailedEventsParameters>()))
                .Returns(new List<MammothFailedEvent>());

            //When
            monitor.CheckStatusAndNotify();

            //Then
            mockGetMammothFailedEventsQueryHandler.Verify(m => m.Search(It.IsAny<GetMammothFailedEventsParameters>()), Times.Exactly(12));
            mockOpsgenieTrigger.Verify(m => m.TriggerAlert(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Never);
        }

        [TestMethod]
        public void MammothFailedEventsMonitorCheckStatusAndNotify_FailedEvents_OpsGenieAlert()
        {
            //Given
            mockGetMammothFailedEventsQueryHandler.Setup(m => m.Search(It.IsAny<GetMammothFailedEventsParameters>()))
                .Returns(new List<MammothFailedEvent> { new MammothFailedEvent() });

            //When
            monitor.CheckStatusAndNotify();

            //Then
            mockGetMammothFailedEventsQueryHandler.Verify(m => m.Search(It.IsAny<GetMammothFailedEventsParameters>()), Times.Exactly(12));
            mockOpsgenieTrigger.Verify(m => m.TriggerAlert(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Exactly(12));
        }

        [TestMethod]
        public void MammothFailedEventsMonitorCheckStatusAndNotify_ExceptionOnQuery_LogException()
        {
            //Given
            mockGetMammothFailedEventsQueryHandler.Setup(m => m.Search(It.IsAny<GetMammothFailedEventsParameters>()))
                .Throws(new Exception("Test Exception"));

            //When
            monitor.CheckStatusAndNotify();

            //Then
            mockLogger.Verify(m => m.Error(It.IsAny<string>()), Times.Exactly(12));
            mockGetMammothFailedEventsQueryHandler.Verify(m => m.Search(It.IsAny<GetMammothFailedEventsParameters>()), Times.Exactly(12));
            mockOpsgenieTrigger.Verify(m => m.TriggerAlert(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Never);
        }

        //[TestMethod]
        public void MammothFailedEventsMonitorCheckStatusAndNotify_ExceptionOnOpsGenieAlert_LogException()
        {
            //Given
            mockGetMammothFailedEventsQueryHandler.Setup(m => m.Search(It.IsAny<GetMammothFailedEventsParameters>()))
                .Returns(new List<MammothFailedEvent> { new MammothFailedEvent() });
            mockOpsgenieTrigger.Setup(m => m.TriggerAlert(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
                .Throws(new Exception("Test Exception"));

            //When
            monitor.CheckStatusAndNotify();

            //Then
            mockLogger.Verify(m => m.Error(It.IsAny<string>()), Times.Exactly(12));
            mockGetMammothFailedEventsQueryHandler.Verify(m => m.Search(It.IsAny<GetMammothFailedEventsParameters>()), Times.Exactly(12));
            mockOpsgenieTrigger.Verify(m => m.TriggerAlert(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Exactly(12));
        }

        [TestMethod]
        public void MammothFailedEventsMonitorCheckStatusAndNotify_ExceptionOnSettings_LogException()
        {
            //Given
            mockSettings.Reset();

            //When
            monitor.CheckStatusAndNotify();

            //Then
            mockLogger.Verify(m => m.Error(It.IsAny<string>()), Times.Once);
        }
    }
}
