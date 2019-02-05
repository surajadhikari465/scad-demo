using Icon.Logging;
using Icon.Monitoring.Common.Opsgenie;
using Icon.Monitoring.Common.Settings;
using Icon.Monitoring.DataAccess.Queries;
using Icon.Monitoring.Monitors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading;

namespace Icon.Monitoring.Tests.Monitors
{
    [TestClass]
    public class PushControllerMonitorTests
    {
        private Mock<IOpsgenieTrigger> mockOpsgenieTrigger;
        private Mock<IMonitorSettings> mockSettings;
        private Mock<ILogger> mockLogger;

        [TestMethod]
        public void PushControllerMonitor_NoRowsInPushQueue_OpsgenieAlertNotSentAndCacheIsZero()
        {
            // Given
            var irmaPushIdQuery = new Mock<Icon.Common.DataAccess.IQueryHandler<GetIrmaPushIdParameters, int>>();
            irmaPushIdQuery.SetupSequence(q => q.Search(It.IsAny<GetIrmaPushIdParameters>()))
                           .Returns(777)
                           .Returns(0);

            // When
            var testee = new PushControllerMonitor(
                this.mockSettings.Object,
                irmaPushIdQuery.Object,
                this.mockOpsgenieTrigger.Object,
                this.mockLogger.Object);
            testee.CheckStatusAndNotify();
            Thread.Sleep(500); // Need to simulate time between checking.
            testee.CheckStatusAndNotify();

            // Then
            this.mockOpsgenieTrigger.Verify(
                x => x.TriggerAlert(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, string>>()),
                Times.Never);

            Assert.IsTrue(testee.IrmaPushJobTracker.Count == 0);
        }

        [TestMethod]
        public void PushControllerMonitor_CheckFirstTimeAndRowsInQueue_OpsgenieAlertNotSentAndCacheIsZero()
        {
            // Given
            var pushId = 1001;
            var irmaPushIdQuery = new Mock<Icon.Common.DataAccess.IQueryHandler<GetIrmaPushIdParameters, int>>();
            irmaPushIdQuery.Setup(q => q.Search(It.IsAny<GetIrmaPushIdParameters>()))
                           .Returns(pushId);

            // When
            var testee = new PushControllerMonitor(
                this.mockSettings.Object,
                irmaPushIdQuery.Object,
                this.mockOpsgenieTrigger.Object,
                this.mockLogger.Object);

            testee.CheckStatusAndNotify();

            // Then
            this.mockOpsgenieTrigger.Verify(
                x => x.TriggerAlert(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, string>>()),
                Times.Never);

            Assert.IsTrue(testee.IrmaPushJobTracker[pushId] == 1);
        }

        [TestMethod]
        public void PushControllerMonitor_SameRowInQueueFoundInSecondCheck_OpsgenieAlertSentOnlyOnceAndNumberOfTimesDetectedIsTwo()
        {
            // Given
            var pushId = 1001;
            
            var irmaPushIdQuery = new Mock<Icon.Common.DataAccess.IQueryHandler<GetIrmaPushIdParameters, int>>();
            irmaPushIdQuery.SetupSequence(q => q.Search(It.IsAny<GetIrmaPushIdParameters>()))
                           .Returns(pushId)
                           .Returns(pushId);

            // When
            var testee = new PushControllerMonitor(
                this.mockSettings.Object,
                irmaPushIdQuery.Object,
                this.mockOpsgenieTrigger.Object,
                this.mockLogger.Object);

            testee.CheckStatusAndNotify();
            Thread.Sleep(500); // Need to simulate time between checking.
            testee.CheckStatusAndNotify();

            // Then
            this.mockOpsgenieTrigger.Verify(
                x => x.TriggerAlert(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, string>>()),
                Times.Once);

            Assert.IsTrue(testee.IrmaPushJobTracker[pushId] == 2);
        }

        [TestMethod]
        public void PushControllerMonitor_SameRowInQueueFoundThirdTime_OpsgenieAlertSentOnlyOnceAndNumberOfTimesDetectedIsTwo()
        {
            // Given
            var pushId = 1001;
            var irmaPushIdQuery = new Mock<Icon.Common.DataAccess.IQueryHandler<GetIrmaPushIdParameters, int>>();
            irmaPushIdQuery.SetupSequence(q => q.Search(It.IsAny<GetIrmaPushIdParameters>()))
                           .Returns(pushId)
                           .Returns(pushId)
                           .Returns(pushId);


            // When
            var testee = new PushControllerMonitor(
                this.mockSettings.Object,
                irmaPushIdQuery.Object,
                this.mockOpsgenieTrigger.Object,
                this.mockLogger.Object);

            testee.CheckStatusAndNotify();
            Thread.Sleep(500); // Need to simulate time between checking.
            testee.CheckStatusAndNotify();
            Thread.Sleep(500);
            testee.CheckStatusAndNotify();

            // Then
            this.mockOpsgenieTrigger.Verify(
                x => x.TriggerAlert(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, string>>()),
                Times.Once);

            Assert.IsTrue(testee.IrmaPushJobTracker[pushId] == 2);
        }

        [TestMethod]
        public void PushControllerMonitor_NewRowIdFoundInSecondCheck_OpsgenieAlertNotSentAndTrackerDictionaryUpdated()
        {
            // Given
            var previousPushId = 1001;
            var pushId = 1002;
            var irmaPushIdQuery = new Mock<Icon.Common.DataAccess.IQueryHandler<GetIrmaPushIdParameters, int>>();
            irmaPushIdQuery.SetupSequence(q => q.Search(It.IsAny<GetIrmaPushIdParameters>()))
                .Returns(previousPushId)
                .Returns(pushId);

            // When
            var testee = new PushControllerMonitor(
                this.mockSettings.Object,
                irmaPushIdQuery.Object,
                this.mockOpsgenieTrigger.Object,
                this.mockLogger.Object);

            testee.CheckStatusAndNotify();
            Thread.Sleep(500); // Need to simulate time between checking.
            testee.CheckStatusAndNotify();

            // Then
            this.mockOpsgenieTrigger.Verify(
                x => x.TriggerAlert(
                     It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, string>>()),
                Times.Never);

            Assert.IsFalse(testee.IrmaPushJobTracker.ContainsKey(previousPushId));
            Assert.IsTrue(testee.IrmaPushJobTracker[pushId] == 1);
        }

        [TestInitialize()]
        public void Initialize()
        {
            this.mockSettings = new Mock<IMonitorSettings>();
            this.mockOpsgenieTrigger = new Mock<IOpsgenieTrigger>();
            this.mockLogger = new Mock<ILogger>();
        }
    }
}
