using Icon.Monitoring.Monitors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Icon.Monitoring.Common.Settings;
using Icon.Monitoring.Common.IO;
using Icon.Monitoring.Common.Opsgenie;
using Icon.Common.DataAccess;
using Icon.Monitoring.DataAccess.Commands;
using Icon.Monitoring.Common.Dvo;
using Icon.Monitoring.DataAccess.Queries;
using Moq;
using Icon.Logging;

namespace Icon.Monitoring.Tests.Monitors
{
    [TestClass]
    public class DvoBulkImportJobMonitorTests
    {
        private const string TestDirectoryPath = "TestDirectoryPath";
        private const int TestDvoBulkImportFileMaxMinuteThreshold = 15;
        private DvoBulkImportJobMonitor monitor;
        private Mock<IMonitorSettings> mockSettings;
        private Mock<IDvoBulkImportJobMonitorSettings> mockDvoSettings;
        private Mock<IFileInfoAccessor> mockFileInfoAccessor;
        private Mock<IOpsgenieTrigger> mockOpsgenieTrigger;
        private Mock<ICommandHandler<AddDvoErrorStatusCommand>> mockAddDvoErrorStatusCommandHandler;
        private Mock<ICommandHandler<DeleteDvoErrorStatusCommand>> mockDeleteDvoErrorStatusCommandHandler;
        private Mock<IQueryHandler<GetDvoJobStatusParameters, List<DvoRegionalJobStatus>>> mockGetDvoRegionalJobStatusQueryHandler;
        private Mock<ILogger> mockLogger;
        private List<string> testRegions;

        [TestInitialize]
        public void Initialize()
        {
            mockSettings = new Mock<IMonitorSettings>();
            mockDvoSettings = new Mock<IDvoBulkImportJobMonitorSettings>();
            mockFileInfoAccessor = new Mock<IFileInfoAccessor>();
            this.mockOpsgenieTrigger = new Mock<IOpsgenieTrigger>();
            mockAddDvoErrorStatusCommandHandler = new Mock<ICommandHandler<AddDvoErrorStatusCommand>>();
            mockDeleteDvoErrorStatusCommandHandler = new Mock<ICommandHandler<DeleteDvoErrorStatusCommand>>();
            mockGetDvoRegionalJobStatusQueryHandler = new Mock<IQueryHandler<GetDvoJobStatusParameters, List<DvoRegionalJobStatus>>>();
            mockLogger = new Mock<ILogger>();

            monitor = new DvoBulkImportJobMonitor(
                mockSettings.Object,
                mockDvoSettings.Object,
                mockFileInfoAccessor.Object,
                mockOpsgenieTrigger.Object,
                mockGetDvoRegionalJobStatusQueryHandler.Object,
                mockDeleteDvoErrorStatusCommandHandler.Object,
                mockAddDvoErrorStatusCommandHandler.Object,
                mockLogger.Object);

            testRegions = new List<string> { "FL", "MA", "MW" };
        }

        [TestMethod]
        public void TimedCheckStatusAndNotify_EnableDvoBulkImportJobMonitorIsFalse_DoesNotCheckStatusAndNotify()
        {
            //Given
            mockDvoSettings.SetupGet(m => m.EnableDvoBulkImportJobMonitor).Returns(false);

            //When
            monitor.CheckStatusAndNotify();

            //Then
            mockDvoSettings.VerifyGet(m => m.DvoDirectoryPath, Times.Never);
        }

        [TestMethod]
        public void TimedCheckStatusAndNotify_BlackOutPeriodIsNow_DoesNotCheckStatusAndNotify()
        {
            //Given
            mockDvoSettings.SetupGet(m => m.EnableDvoBulkImportJobMonitor).Returns(true);
            mockDvoSettings.SetupGet(m => m.DvoBulkImportJobMonitorBlackoutStart).Returns(DateTime.Today);
            mockDvoSettings.SetupGet(m => m.DvoBulkImportJobMonitorBlackoutEnd).Returns(DateTime.Today.AddHours(24));

            //When
            monitor.CheckStatusAndNotify();

            //Then
            mockDvoSettings.VerifyGet(m => m.DvoDirectoryPath, Times.Never);
        }

        [TestMethod]
        public void TimedCheckStatusAndNotify_FileDoesNotExist_SendsOpsgenieDuty()
        {
            //Given
            SetUpEnabledValuesOnDvoMonitorSettings();

            var mockFileInfo = new Mock<IFileInfo>();
            mockFileInfoAccessor.Setup(m => m.GetFileInfo(It.IsAny<string>()))
                .Returns(mockFileInfo.Object);
            mockGetDvoRegionalJobStatusQueryHandler.Setup(m => m.Search(It.IsAny<GetDvoJobStatusParameters>()))
                .Returns(new List<DvoRegionalJobStatus>());

            mockFileInfo.SetupGet(m => m.Exists).Returns(false);

            //When
            monitor.CheckStatusAndNotify();

            //Then
            mockOpsgenieTrigger.Verify(m => m.TriggerAlert(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Once);
            mockAddDvoErrorStatusCommandHandler.Verify(m => m.Execute(It.IsAny<AddDvoErrorStatusCommand>()), Times.Exactly(testRegions.Count));
        }

        [TestMethod]
        public void TimedCheckStatusAndNotify_DvoFileIsOlderThanMaxMinuteThreshold_SendsOpsgenieDuty()
        {
            //Given
            SetUpEnabledValuesOnDvoMonitorSettings();

            var mockFileInfo = new Mock<IFileInfo>();
            mockFileInfoAccessor.Setup(m => m.GetFileInfo(It.IsAny<string>()))
                .Returns(mockFileInfo.Object);
            mockGetDvoRegionalJobStatusQueryHandler.Setup(m => m.Search(It.IsAny<GetDvoJobStatusParameters>()))
                .Returns(new List<DvoRegionalJobStatus>());

            mockFileInfo.SetupGet(m => m.Exists).Returns(true);
            mockFileInfo.SetupGet(m => m.LastWriteTime).Returns(DateTime.Now.Subtract(TimeSpan.FromMinutes(TestDvoBulkImportFileMaxMinuteThreshold + 1)));

            //When
            monitor.CheckStatusAndNotify();

            //Then
            mockOpsgenieTrigger.Verify(m => m.TriggerAlert(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Once);
            mockAddDvoErrorStatusCommandHandler.Verify(m => m.Execute(It.IsAny<AddDvoErrorStatusCommand>()), Times.Exactly(testRegions.Count));
        }

        [TestMethod]
        public void TimedCheckStatusAndNotify_NoError_DeletesAnyCurrentErrorStatusAndDoesNotSendOpsgenie()
        {
            //Given
            SetUpEnabledValuesOnDvoMonitorSettings();

            var mockFileInfo = new Mock<IFileInfo>();
            mockFileInfoAccessor.Setup(m => m.GetFileInfo(It.IsAny<string>()))
                .Returns(mockFileInfo.Object);
            mockGetDvoRegionalJobStatusQueryHandler.Setup(m => m.Search(It.IsAny<GetDvoJobStatusParameters>()))
                .Returns(new List<DvoRegionalJobStatus>());

            mockFileInfo.SetupGet(m => m.Exists).Returns(true);
            mockFileInfo.SetupGet(m => m.LastWriteTime).Returns(DateTime.Now);

            //When
            monitor.CheckStatusAndNotify();

            //Then
            mockOpsgenieTrigger.Verify(m => m.TriggerAlert(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Never);
            mockAddDvoErrorStatusCommandHandler.Verify(m => m.Execute(It.IsAny<AddDvoErrorStatusCommand>()), Times.Never);
            mockDeleteDvoErrorStatusCommandHandler.Verify(m => m.Execute(It.IsAny<DeleteDvoErrorStatusCommand>()), Times.Exactly(testRegions.Count));
        }

        [TestMethod]
        public void TimedCheckStatusAndNotify_CurrentErrorsExist_DoesNotSendOpsgenie()
        {
            //Given
            SetUpEnabledValuesOnDvoMonitorSettings();

            var mockFileInfo = new Mock<IFileInfo>();
            mockFileInfoAccessor.Setup(m => m.GetFileInfo(It.IsAny<string>()))
                .Returns(mockFileInfo.Object);
            mockGetDvoRegionalJobStatusQueryHandler.Setup(m => m.Search(It.IsAny<GetDvoJobStatusParameters>()))
                .Returns(testRegions.Select(r => new DvoRegionalJobStatus { Region = r }).ToList());

            mockFileInfo.SetupGet(m => m.Exists).Returns(false);

            //When
            monitor.CheckStatusAndNotify();

            //Then
            mockOpsgenieTrigger.Verify(m => m.TriggerAlert(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Never);
            mockAddDvoErrorStatusCommandHandler.Verify(m => m.Execute(It.IsAny<AddDvoErrorStatusCommand>()), Times.Never);
            mockDeleteDvoErrorStatusCommandHandler.Verify(m => m.Execute(It.IsAny<DeleteDvoErrorStatusCommand>()), Times.Never);
        }

        private void SetUpEnabledValuesOnDvoMonitorSettings()
        {
            mockDvoSettings.SetupGet(m => m.EnableDvoBulkImportJobMonitor).Returns(true);
            mockDvoSettings.SetupGet(m => m.DvoBulkImportJobMonitorBlackoutStart).Returns(DateTime.Now.Subtract(TimeSpan.FromHours(5)));
            mockDvoSettings.SetupGet(m => m.DvoBulkImportJobMonitorBlackoutEnd).Returns(DateTime.Now.Subtract(TimeSpan.FromHours(4)));
            mockDvoSettings.SetupGet(m => m.DvoDirectoryPath).Returns(TestDirectoryPath);
            mockDvoSettings.SetupGet(m => m.DvoBulkImportFileMaxMinuteThreshold).Returns(TestDvoBulkImportFileMaxMinuteThreshold);
            mockDvoSettings.SetupGet(m => m.DvoBulkImportJobMonitorRegions).Returns(testRegions);
        }
    }
}
