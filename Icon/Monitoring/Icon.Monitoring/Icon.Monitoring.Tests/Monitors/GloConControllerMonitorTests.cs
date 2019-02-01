using Icon.Common.DataAccess;
using Icon.Logging;
using Icon.Monitoring.Common;
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
using System.Threading;
using System.Threading.Tasks;

namespace Icon.Monitoring.Tests.Monitors
{
    [TestClass]
    public class GloConControllerMonitorTests
    {
        private Mock<IOpsgenieTrigger> mockOpsgenieTrigger;
        private Mock<IMonitorSettings> mockSettings;
        private Mock<ILogger> mockLogger;

        [TestInitialize()]
        public void Initialize()
        {
            this.mockSettings = new Mock<IMonitorSettings>();
            this.mockOpsgenieTrigger = new Mock<IOpsgenieTrigger>();
            this.mockLogger = new Mock<ILogger>();
        }

        // 

        [TestMethod]
        public void GloConMonitorService_GloConHasEmptyQueue_OpsgenieAlertNotSentAndGloConTrackerCountIsZero()
        {
            // Given
            var GloConIdQuery = new Mock<Icon.Common.DataAccess.IQueryHandler<GetGloConItemQueueIdParameters, int>>();
            GloConIdQuery.SetupSequence(q => q.Search(It.IsAny<GetGloConItemQueueIdParameters>()))
                 .Returns(1)
                 .Returns(0);

            // When
            var testee = new GloConControllerMonitor(
                this.mockSettings.Object,
                GloConIdQuery.Object,
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

            Assert.IsTrue(testee.GloConJobTracker.Count == 0);
        }

        [TestMethod]
        public void GloConMonitorService_GloConHasRowInQueueForFirstCheck_OpsgenieAlertNotSent()
        {
            // Given
            var GloConIdQuery = new Mock<Icon.Common.DataAccess.IQueryHandler<GetGloConItemQueueIdParameters, int>>();
            GloConIdQuery.Setup(q => q.Search(It.IsAny<GetGloConItemQueueIdParameters>()))
                 .Returns(1);

            // When
            var testee = new GloConControllerMonitor(
                this.mockSettings.Object,
                GloConIdQuery.Object,
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

            Assert.IsTrue(testee.GloConJobTracker.Count == 1);
        }


        [TestMethod]
        public void GloConMonitorService_GloConHasSameRowInQueueOnSecondCheck_OpsgenieNotifiedOnce()
        {
            // Given
            var GloConIdQuery = new Mock<Icon.Common.DataAccess.IQueryHandler<GetGloConItemQueueIdParameters, int>>();
            GloConIdQuery.SetupSequence(q => q.Search(It.IsAny<GetGloConItemQueueIdParameters>()))
                 .Returns(1)
                 .Returns(1);
                 

            // When
            var testee = new GloConControllerMonitor(
                this.mockSettings.Object,
                GloConIdQuery.Object,
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
                Times.AtLeastOnce);

            Assert.IsTrue(testee.GloConJobTracker[1] == 2);
        }


        [TestMethod]
        public void GloConMonitorService_GloConHasSameRowMultipleTimes_OpsgenieNotifiedOnlyOnce()
        {
            // Given
            var GloConIdQuery = new Mock<Icon.Common.DataAccess.IQueryHandler<GetGloConItemQueueIdParameters, int>>();
            GloConIdQuery.SetupSequence(q => q.Search(It.IsAny<GetGloConItemQueueIdParameters>()))
                 .Returns(1)
                 .Returns(1)
                 .Returns(1);


            // When
            var testee = new GloConControllerMonitor(
                this.mockSettings.Object,
                GloConIdQuery.Object,
                this.mockOpsgenieTrigger.Object,
                this.mockLogger.Object);

            testee.CheckStatusAndNotify();
            Thread.Sleep(500); // Need to simulate time between checking.

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

            Assert.IsTrue(testee.GloConJobTracker[1] == 2);
        }

        [TestMethod]
        public void GloConMonitorService_GloConHasDifferentRowOnSecondCheck_OpsgenieAlertNotSentAndGloConTrackerReset()
        {
            // Given
            var GloConIdQuery = new Mock<Icon.Common.DataAccess.IQueryHandler<GetGloConItemQueueIdParameters, int>>();
            GloConIdQuery.SetupSequence(q => q.Search(It.IsAny<GetGloConItemQueueIdParameters>()))
                 .Returns(1)
                 .Returns(2);


            // When
            var testee = new GloConControllerMonitor(
                this.mockSettings.Object,
                GloConIdQuery.Object,
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

            Assert.IsTrue(testee.GloConJobTracker.Count == 1);           
            Assert.IsTrue(testee.GloConJobTracker[2] == 1);

        }
    }

}