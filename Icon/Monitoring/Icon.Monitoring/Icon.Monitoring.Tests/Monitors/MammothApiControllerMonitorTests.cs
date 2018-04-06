using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Icon.Monitoring.Common.PagerDuty;
using Moq;
using Icon.Monitoring.DataAccess.Queries;
using Icon.Monitoring.Common.Settings;
using Icon.Monitoring.Monitors;
using Icon.Logging;
using Icon.Monitoring.Common;
using System.Collections.Generic;
using Icon.Monitoring.Common.ApiController;

namespace Icon.Monitoring.Tests.Monitors
{
    [TestClass]
    public class MammothApiControllerMonitorTests
    {
        private Mock<IPagerDutyTrigger> mockPagerDutyTrigger;
        private Mock<IQueryHandlerMammoth<GetApiMessageQueueIdParameters, int>> mockMessageQueueQuery;
        private Mock<IMonitorSettings> mockSettings;
        private MammothApiControllerMonitor mammothApiControllerMonitor;
        private MammothMessageQueueCache messageQueueCache;

        [TestInitialize]
        public void Initialize()
        {
            this.mockPagerDutyTrigger = new Mock<IPagerDutyTrigger>();
            this.mockMessageQueueQuery = new Mock<IQueryHandlerMammoth<GetApiMessageQueueIdParameters, int>>();
            this.mockSettings = new Mock<IMonitorSettings>();
            this.messageQueueCache = new MammothMessageQueueCache();

            this.mammothApiControllerMonitor = new MammothApiControllerMonitor(
                this.mockSettings.Object,
                this.mockMessageQueueQuery.Object,
                this.mockPagerDutyTrigger.Object,
                new Mock<ILogger>().Object,
                messageQueueCache);

            mammothApiControllerMonitor.ByPassConfiguredRunInterval = true;
        }

        private string BuildTriggerDescription(string queueType)
        {
            return string.Format("{0} Mammoth API Controller appears to be stuck or not processing data.", queueType);
        }

        private Dictionary<string, string> BuildTriggerDetails(int? messageQueueId)
        {
            Dictionary<string, string> details = new Dictionary<string, string>
            {
                { "Stuck MessageQueueId", messageQueueId.ToString() }
            };
            return details;
        }

        #region MessageQueueIdMatchesCacheFirstTime

        [TestMethod]
        public void ApiControllerMonitorCheckStatusAndNotify_PriceMessageQueueIdMatchesCacheFirstTime_ShouldSendsPagerDutyAlert()
        {
            MessageQueueIdMatchesFirstTime_ShouldSendPagerDutyAlert(MessageQueueTypes.Price);
        }

        [TestMethod]
        public void ApiControllerMonitorCheckStatusAndNotify_ItemLocaleMessageQueueIdMatchesCacheFirstTime_ShouldSendsPagerDutyAlert()
        {
            MessageQueueIdMatchesFirstTime_ShouldSendPagerDutyAlert(MessageQueueTypes.ItemLocale);
        }

        private void MessageQueueIdMatchesFirstTime_ShouldSendPagerDutyAlert(string messagQueueType)
        {
            // Given
            int expectedQueueId = new Random(1).Next(27000);
            messageQueueCache.QueueTypeToIdMapper[messagQueueType] = new QueueData { LastMessageQueueId = expectedQueueId, NumberOfTimesMatched = 0 };
            this.mockMessageQueueQuery.Setup(q => q.Search(It.Is<GetApiMessageQueueIdParameters>(v => v.MessageQueueType == messagQueueType))).Returns(expectedQueueId);
            this.mockPagerDutyTrigger.Setup(pd => pd.TriggerIncident(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>())).Returns(new PagerDutyResponse());

            string expectedDescription = BuildTriggerDescription(messagQueueType);
            string expectedDetailKey = "Stuck MessageQueueId";

            // When
            this.mammothApiControllerMonitor.CheckStatusAndNotify();

            // Then
            mockPagerDutyTrigger.Verify(t =>
                t.TriggerIncident(It.Is<string>(d => d == expectedDescription),
                    It.Is<Dictionary<string, string>>(d => d[expectedDetailKey] == expectedQueueId.ToString())),
                Times.Once);
            Assert.AreEqual(expectedQueueId, messageQueueCache.QueueTypeToIdMapper[messagQueueType].LastMessageQueueId);
            Assert.AreEqual(1, messageQueueCache.QueueTypeToIdMapper[messagQueueType].NumberOfTimesMatched);
        }

        #endregion MessageQueueIdMatchesCacheFirstTime

        #region MessageQueueIdMatchesCacheASecondTime
        [TestMethod]
        public void ApiControllerMonitorCheckStatusAndNotify_PriceMessageQueueIdMatchesCacheASecondTime_ShouldNotSendsPagerDutyAlert()
        {
            MessageQueueIdMatchesCacheSecondTime_ShouldNotSendPagerDutyAlert(MessageQueueTypes.Price);
        }

        [TestMethod]
        public void ApiControllerMonitorCheckStatusAndNotify_ItemLocaleMessageQueueIdMatchesCacheASecondTime_ShouldNotSendsPagerDutyAlert()
        {
            MessageQueueIdMatchesCacheSecondTime_ShouldNotSendPagerDutyAlert(MessageQueueTypes.ItemLocale);
        }

        private void MessageQueueIdMatchesCacheSecondTime_ShouldNotSendPagerDutyAlert(string messageQueueType)
        {
            // Given
            int expectedQueueId = new Random(1).Next(27000);
            int expectedNumberOfTimesMatched = 2;
            messageQueueCache.QueueTypeToIdMapper[messageQueueType] = new QueueData { LastMessageQueueId = expectedQueueId, NumberOfTimesMatched = 1 };
            this.mockMessageQueueQuery.Setup(q => q.Search(It.Is<GetApiMessageQueueIdParameters>(v => v.MessageQueueType == messageQueueType))).Returns(expectedQueueId);
            this.mockPagerDutyTrigger.Setup(pd => pd.TriggerIncident(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>())).Returns(new PagerDutyResponse());

            string expectedDescription = BuildTriggerDescription(messageQueueType);
            string expectedDetailKey = "Stuck MessageQueueId";

            // When
            this.mammothApiControllerMonitor.CheckStatusAndNotify();

            // Then
            mockPagerDutyTrigger.Verify(t =>
                t.TriggerIncident(It.Is<string>(d => d == expectedDescription),
                    It.Is<Dictionary<string, string>>(d => d[expectedDetailKey] == expectedQueueId.ToString())),
                Times.Never);
            Assert.AreEqual(expectedQueueId, messageQueueCache.QueueTypeToIdMapper[messageQueueType].LastMessageQueueId);
            Assert.AreEqual(expectedNumberOfTimesMatched, messageQueueCache.QueueTypeToIdMapper[messageQueueType].NumberOfTimesMatched);
        }

        #endregion MessageQueueIdMatchesCacheASecondTime

        #region MessageQueueIdIsZeroAndCacheIdIsZero

        [TestMethod]
        public void ApiControllerMonitorCheckStatusAndNotify_PriceMessageQueueIdIsZeroAndCacheIdIsZero_ShouldNotSendsPagerDutyAlert()
        {
            MessageQueueIdIsZeroAndCacheIdIsZero_ShouldNotSendPagerDutyAlert(MessageQueueTypes.Price);
        }

        [TestMethod]
        public void ApiControllerMonitorCheckStatusAndNotify_ItemLocaleMessageQueueIdIsZeroAndCacheIdIsZero_ShouldNotSendsPagerDutyAlert()
        {
            MessageQueueIdIsZeroAndCacheIdIsZero_ShouldNotSendPagerDutyAlert(MessageQueueTypes.ItemLocale);
        }

        private void MessageQueueIdIsZeroAndCacheIdIsZero_ShouldNotSendPagerDutyAlert(string messageQueueType)
        {
            //Given
            int expectedQueueId = 0;
            int expectedNumberOfTimesMatched = 0;
            messageQueueCache.QueueTypeToIdMapper[messageQueueType] = new QueueData { LastMessageQueueId = expectedQueueId, NumberOfTimesMatched = 0 };
            this.mockMessageQueueQuery.Setup(q => q.Search(It.Is<GetApiMessageQueueIdParameters>(v => v.MessageQueueType == messageQueueType))).Returns(expectedQueueId);
            this.mockPagerDutyTrigger.Setup(pd => pd.TriggerIncident(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>())).Returns(new PagerDutyResponse());

            string expectedDescription = BuildTriggerDescription(messageQueueType);
            string expectedDetailKey = "Stuck MessageQueueId";

            // When
            this.mammothApiControllerMonitor.CheckStatusAndNotify();

            // Then
            mockPagerDutyTrigger.Verify(t =>
                t.TriggerIncident(It.Is<string>(d => d == expectedDescription),
                    It.Is<Dictionary<string, string>>(d => d[expectedDetailKey] == expectedQueueId.ToString())),
                Times.Never);
            Assert.AreEqual(expectedQueueId, messageQueueCache.QueueTypeToIdMapper[messageQueueType].LastMessageQueueId);
            Assert.AreEqual(expectedNumberOfTimesMatched, messageQueueCache.QueueTypeToIdMapper[messageQueueType].NumberOfTimesMatched);
        }

        #endregion MessageQueueIdIsZeroAndCacheIdIsZero

        #region MessageQueueIdAndCacheIdAreDifferent

        [TestMethod]
        public void ApiControllerMonitorCheckStatusAndNotify_PriceMessageQueueIdAndCacheIdAreDifferent_ShouldNotSendsPagerDutyAlert()
        {
            MessageQueueIdAndCacheIdAreDifferent_ShouldNotSendsPagerDutyAlert(MessageQueueTypes.Price, 0, 323);
            MessageQueueIdAndCacheIdAreDifferent_ShouldNotSendsPagerDutyAlert(MessageQueueTypes.Price, 311, 0);
            MessageQueueIdAndCacheIdAreDifferent_ShouldNotSendsPagerDutyAlert(MessageQueueTypes.Price, 556, 2);
        }

        [TestMethod]
        public void ApiControllerMonitorCheckStatusAndNotify_ItemLocaleMessageQueueIdAndCacheIdAreDifferent_ShouldNotSendsPagerDutyAlert()
        {
            MessageQueueIdAndCacheIdAreDifferent_ShouldNotSendsPagerDutyAlert(MessageQueueTypes.ItemLocale, 0, 323);
            MessageQueueIdAndCacheIdAreDifferent_ShouldNotSendsPagerDutyAlert(MessageQueueTypes.ItemLocale, 311, 0);
            MessageQueueIdAndCacheIdAreDifferent_ShouldNotSendsPagerDutyAlert(MessageQueueTypes.ItemLocale, 556, 2);
        }

        private void MessageQueueIdAndCacheIdAreDifferent_ShouldNotSendsPagerDutyAlert(string messageQueueType, int queueId, int cacheId)
        {
            //Given
            int expectedQueueId = queueId;
            int expectedNumberOfTimesMatched = 0;
            messageQueueCache.QueueTypeToIdMapper[messageQueueType] = new QueueData { LastMessageQueueId = cacheId, NumberOfTimesMatched = 0 };
            this.mockMessageQueueQuery.Setup(q => q.Search(It.Is<GetApiMessageQueueIdParameters>(v => v.MessageQueueType == messageQueueType))).Returns(queueId);
            this.mockPagerDutyTrigger.Setup(pd => pd.TriggerIncident(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>())).Returns(new PagerDutyResponse());

            string expectedDescription = BuildTriggerDescription(messageQueueType);
            string expectedDetailKey = "Stuck MessageQueueId";

            // When
            this.mammothApiControllerMonitor.CheckStatusAndNotify();

            // Then
            mockPagerDutyTrigger.Verify(t =>
                t.TriggerIncident(It.Is<string>(d => d == expectedDescription),
                    It.Is<Dictionary<string, string>>(d => d[expectedDetailKey] == expectedQueueId.ToString())),
                Times.Never);
            Assert.AreEqual(expectedQueueId, messageQueueCache.QueueTypeToIdMapper[messageQueueType].LastMessageQueueId);
            Assert.AreEqual(expectedNumberOfTimesMatched, messageQueueCache.QueueTypeToIdMapper[messageQueueType].NumberOfTimesMatched);
        }

        #endregion MessageQueueIdAndCacheIdAreDifferent
    }
}
