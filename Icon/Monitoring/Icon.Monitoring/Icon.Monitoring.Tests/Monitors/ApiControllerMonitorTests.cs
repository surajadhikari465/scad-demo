using Icon.Logging;
using Icon.Monitoring.Common;
using Icon.Monitoring.Common.ApiController;
using Icon.Monitoring.Common.Opsgenie;
using Icon.Monitoring.Common.Settings;
using Icon.Monitoring.DataAccess.Queries;
using Icon.Monitoring.Monitors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NodaTime;
using NodaTime.Testing;
using System;
using System.Collections.Generic;

namespace Icon.Monitoring.Tests.Monitors
{
    [TestClass]
    public class ApiControllerMonitorTests
    {
        private Mock<IOpsgenieTrigger> mockOpsgenieTrigger;
        private Mock<Icon.Common.DataAccess.IQueryHandler<GetApiMessageQueueIdParameters, int>> mockMessageQueueQuery;
        private Mock<Icon.Common.DataAccess.IQueryHandler<GetApiMessageUnprocessedRowCountParameters, int>> mockMessageUnprocessedRowCountQuery;
        private List<string> testRegions;
        private Mock<IMonitorSettings> mockSettings;
        private ApiControllerMonitor apiControllerMonitor;
        private IClock fakeClock;
        private IDateTimeZoneProvider dateTimeZoneProvider;
        private MessageQueueCache messageQueueCache;

        [TestInitialize]
        public void Initialize()
        {
            this.mockOpsgenieTrigger = new Mock<IOpsgenieTrigger>();
            this.mockMessageQueueQuery = new Mock<Icon.Common.DataAccess.IQueryHandler<GetApiMessageQueueIdParameters, int>>();
            this.mockMessageUnprocessedRowCountQuery = new Mock<Icon.Common.DataAccess.IQueryHandler<GetApiMessageUnprocessedRowCountParameters, int>>();
            this.fakeClock = new FakeClock(Instant.FromDateTimeUtc(DateTime.UtcNow));
            this.dateTimeZoneProvider = DateTimeZoneProviders.Tzdb;
            this.mockSettings = new Mock<IMonitorSettings>();
            this.messageQueueCache = new MessageQueueCache();

            this.apiControllerMonitor = new ApiControllerMonitor(
                this.mockSettings.Object,
                this.mockMessageQueueQuery.Object,
                this.mockMessageUnprocessedRowCountQuery.Object,
                this.mockOpsgenieTrigger.Object,
                this.dateTimeZoneProvider,
                this.fakeClock,
                new Mock<ILogger>().Object,
                messageQueueCache);
            testRegions = new List<string> { "FL" };
            SetUpApiControllerMonitorSettings();
            apiControllerMonitor.ByPassConfiguredRunInterval = true;
        }

        private void SetUpApiControllerMonitorSettings()
        {
            mockSettings.SetupGet(m => m.ApiControllerMonitorRegions).Returns(testRegions);
            mockSettings.SetupGet(m => m.NumberOfMinutesBeforeStoreOpens).Returns(120);
            mockSettings.SetupGet(m => m.StoreOpenCentralTime_FL).Returns(new LocalTime(16, 0, 0));

            Dictionary<string, TimeSpan> MonitorTimers = new Dictionary<string, TimeSpan>();
            MonitorTimers.Add("ApiControllerMonitorTimer", TimeSpan.FromMilliseconds(900000));
            mockSettings.SetupGet(m => m.MonitorTimers).Returns(MonitorTimers);
        }

        private string BuildTriggerDescription(string queueType)
        {
            return string.Format("{0} API Controller appears to be stuck or not processing data.", queueType);
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
        public void ApiControllerMonitorCheckStatusAndNotify_ProductMessageQueueIdMatchesCacheFirstTime_ShouldSendsOpsgenieAlert()
        {
            MessageQueueIdMatchesFirstTime_ShouldSendsOpsgenieAlert(MessageQueueTypes.Product);
        }

        [TestMethod]
        public void ApiControllerMonitorCheckStatusAndNotify_PriceMessageQueueIdMatchesCacheFirstTime_ShouldSendsOpsgenieAlert()
        {
            MessageQueueIdMatchesFirstTime_ShouldSendsOpsgenieAlert(MessageQueueTypes.Price);
        }

        [TestMethod]
        public void ApiControllerMonitorCheckStatusAndNotify_ItemLocaleMessageQueueIdMatchesCacheFirstTime_ShouldSendsOpsgenieAlert()
        {
            MessageQueueIdMatchesFirstTime_ShouldSendsOpsgenieAlert(MessageQueueTypes.ItemLocale);
        }

        [TestMethod]
        public void ApiControllerMonitorCheckStatusAndNotify_LocaleMessageQueueIdMatchesCacheFirstTime_ShouldSendsOpsgenieAlert()
        {
            MessageQueueIdMatchesFirstTime_ShouldSendsOpsgenieAlert(MessageQueueTypes.Locale);
        }

        [TestMethod]
        public void ApiControllerMonitorCheckStatusAndNotify_HierarchyMessageQueueIdMatchesCacheFirstTime_ShouldSendsOpsgenieAlert()
        {
            MessageQueueIdMatchesFirstTime_ShouldSendsOpsgenieAlert(MessageQueueTypes.Hierarchy);
        }

        [TestMethod]
        public void ApiControllerMonitorCheckStatusAndNotify_ProductSelectionGroupMessageQueueIdMatchesCacheFirstTime_ShouldSendsOpsgenieAlert()
        {
            MessageQueueIdMatchesFirstTime_ShouldSendsOpsgenieAlert(MessageQueueTypes.ProductSelectionGroup);
        }

        [TestMethod]
        public void ApiControllerMonitorCheckStatusAndNotify_AttributeMessageQueueIdMatchesCacheFirstTime_ShouldSendsOpsgenieAlert()
        {
            MessageQueueIdMatchesFirstTime_ShouldSendsOpsgenieAlert(MessageQueueTypes.Attribute);
        }

        [TestMethod]
        public void ApiControllerMonitorCheckStatusAndNotify_ItemMessageQueueIdMatchesCacheFirstTime_ShouldSendsOpsgenieAlert()
        {
            MessageQueueIdMatchesFirstTime_ShouldSendsOpsgenieAlert(MessageQueueTypes.Item);
        }

        private void MessageQueueIdMatchesFirstTime_ShouldSendsOpsgenieAlert(string messagQueueType)
        {
            // Given
            int expectedQueueId = new Random(1).Next(27000);
            this.messageQueueCache.QueueTypeToIdMapper[messagQueueType] = new QueueData { LastMessageQueueId = expectedQueueId, NumberOfTimesMatched = 0 };
            this.mockMessageQueueQuery.Setup(q => q.Search(It.Is<GetApiMessageQueueIdParameters>(v => v.MessageQueueType == messagQueueType))).Returns(expectedQueueId);
            this.mockOpsgenieTrigger.Setup(pd => pd.TriggerAlert(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>())).Returns(new OpsgenieAlert.OpsgenieResponse());

            string expectedDescription = BuildTriggerDescription(messagQueueType);
            string expectedDetailKey = "Stuck MessageQueueId";
            string expectedMessage = "API Controller Issue";

            // When
            this.apiControllerMonitor.CheckStatusAndNotify();

            // Then
            mockOpsgenieTrigger.Verify(t =>
                t.TriggerAlert(It.Is<string>(d => d == expectedMessage), It.Is<string>(d => d == expectedDescription),
                    It.Is<Dictionary<string, string>>(d => d[expectedDetailKey] == expectedQueueId.ToString())),
                Times.Once);
            Assert.AreEqual(expectedQueueId, this.messageQueueCache.QueueTypeToIdMapper[messagQueueType].LastMessageQueueId);
            Assert.AreEqual(1, this.messageQueueCache.QueueTypeToIdMapper[messagQueueType].NumberOfTimesMatched);
        }

        #endregion MessageQueueIdMatchesCacheFirstTime

        #region MessageQueueIdMatchesCacheASecondTime

        [TestMethod]
        public void ApiControllerMonitorCheckStatusAndNotify_ProductMessageQueueIdMatchesCacheASecondTime_ShouldNotSendsOpsgenieAlert()
        {
            MessageQueueIdMatchesCacheSecondTime_ShouldNotSendOpsgenieAlert(MessageQueueTypes.Product);
        }

        [TestMethod]
        public void ApiControllerMonitorCheckStatusAndNotify_PriceMessageQueueIdMatchesCacheASecondTime_ShouldNotSendsOpsgenieAlert()
        {
            MessageQueueIdMatchesCacheSecondTime_ShouldNotSendOpsgenieAlert(MessageQueueTypes.Price);
        }

        [TestMethod]
        public void ApiControllerMonitorCheckStatusAndNotify_ItemLocaleMessageQueueIdMatchesCacheASecondTime_ShouldNotSendsOpsgenieAlert()
        {
            MessageQueueIdMatchesCacheSecondTime_ShouldNotSendOpsgenieAlert(MessageQueueTypes.ItemLocale);
        }

        [TestMethod]
        public void ApiControllerMonitorCheckStatusAndNotify_HierarchyMessageQueueIdMatchesCacheASecondTimeOrSubsequentTimes_ShouldNotSendsOpsgenieAlert()
        {
            MessageQueueIdMatchesCacheSecondTime_ShouldNotSendOpsgenieAlert(MessageQueueTypes.Hierarchy);
        }

        [TestMethod]
        public void ApiControllerMonitorCheckStatusAndNotify_LocaleMessageQueueIdMatchesCacheASecondTimeOrSubsequentTimes_ShouldNotSendsOpsgenieAlert()
        {
            MessageQueueIdMatchesCacheSecondTime_ShouldNotSendOpsgenieAlert(MessageQueueTypes.Locale);
        }

        [TestMethod]
        public void ApiControllerMonitorCheckStatusAndNotify_ProductSelectionGroupMessageQueueIdMatchesCacheASecondTimeOrSubsequentTimes_ShouldNotSendsOpsgenieAlert()
        {
            MessageQueueIdMatchesCacheSecondTime_ShouldNotSendOpsgenieAlert(MessageQueueTypes.ProductSelectionGroup);
        }

        [TestMethod]
        public void ApiControllerMonitorCheckStatusAndNotify_AttributeMessageQueueIdMatchesCacheASecondTimeOrSubsequentTimes_ShouldNotSendsOpsgenieAlert()
        {
            MessageQueueIdMatchesCacheSecondTime_ShouldNotSendOpsgenieAlert(MessageQueueTypes.Attribute);
        }

        [TestMethod]
        public void ApiControllerMonitorCheckStatusAndNotify_ItemMessageQueueIdMatchesCacheASecondTimeOrSubsequentTimes_ShouldNotSendsOpsgenieAlert()
        {
            MessageQueueIdMatchesCacheSecondTime_ShouldNotSendOpsgenieAlert(MessageQueueTypes.Item);
        }

        private void MessageQueueIdMatchesCacheSecondTime_ShouldNotSendOpsgenieAlert(string messageQueueType)
        {
            // Given
            int expectedQueueId = new Random(1).Next(27000);
            int expectedNumberOfTimesMatched = 2;
            this.messageQueueCache.QueueTypeToIdMapper[messageQueueType] = new QueueData { LastMessageQueueId = expectedQueueId, NumberOfTimesMatched = 1 };
            this.mockMessageQueueQuery.Setup(q => q.Search(It.Is<GetApiMessageQueueIdParameters>(v => v.MessageQueueType == messageQueueType))).Returns(expectedQueueId);
            this.mockOpsgenieTrigger.Setup(pd => pd.TriggerAlert(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>())).Returns(new OpsgenieAlert.OpsgenieResponse());

            string expectedDescription = BuildTriggerDescription(messageQueueType);
            string expectedDetailKey = "Stuck MessageQueueId";
            string expectedMessage = "API Controller Issue";

            // When
            this.apiControllerMonitor.CheckStatusAndNotify();

            // Then
            mockOpsgenieTrigger.Verify(t =>
                t.TriggerAlert(It.Is<string>(d => d == expectedMessage), It.Is<string>(d => d == expectedDescription),
                    It.Is<Dictionary<string, string>>(d => d[expectedDetailKey] == expectedQueueId.ToString())),
                Times.Never);
            Assert.AreEqual(expectedQueueId, this.messageQueueCache.QueueTypeToIdMapper[messageQueueType].LastMessageQueueId);
            Assert.AreEqual(expectedNumberOfTimesMatched, this.messageQueueCache.QueueTypeToIdMapper[messageQueueType].NumberOfTimesMatched);
        }

        #endregion MessageQueueIdMatchesCacheASecondTime

        #region MessageQueueIdIsZeroAndCacheIdIsZero

        [TestMethod]
        public void ApiControllerMonitorCheckStatusAndNotify_ProductMessageQueueIdIsZeroAndCacheIdIsZero_ShouldNotSendsOpsgenieAlert()
        {
            MessageQueueIdIsZeroAndCacheIdIsZero_ShouldNotSendOpsgenieAlert(MessageQueueTypes.Product);
        }

        [TestMethod]
        public void ApiControllerMonitorCheckStatusAndNotify_PriceMessageQueueIdIsZeroAndCacheIdIsZero_ShouldNotSendsOpsgenieAlert()
        {
            MessageQueueIdIsZeroAndCacheIdIsZero_ShouldNotSendOpsgenieAlert(MessageQueueTypes.Price);
        }

        [TestMethod]
        public void ApiControllerMonitorCheckStatusAndNotify_ItemLocaleMessageQueueIdIsZeroAndCacheIdIsZero_ShouldNotSendsOpsgenieAlert()
        {
            MessageQueueIdIsZeroAndCacheIdIsZero_ShouldNotSendOpsgenieAlert(MessageQueueTypes.ItemLocale);
        }

        [TestMethod]
        public void ApiControllerMonitorCheckStatusAndNotify_HierarchyMessageQueueIdIsZeroAndCacheIdIsZero_ShouldNotSendsOpsgenieAlert()
        {
            MessageQueueIdIsZeroAndCacheIdIsZero_ShouldNotSendOpsgenieAlert(MessageQueueTypes.Hierarchy);
        }

        [TestMethod]
        public void ApiControllerMonitorCheckStatusAndNotify_LocaleMessageQueueIdIsZeroAndCacheIdIsZero_ShouldNotSendsOpsgenieAlert()
        {
            MessageQueueIdIsZeroAndCacheIdIsZero_ShouldNotSendOpsgenieAlert(MessageQueueTypes.Locale);
        }

        [TestMethod]
        public void ApiControllerMonitorCheckStatusAndNotify_AttributeMessageQueueIdIsZeroAndCacheIdIsZero_ShouldNotSendsOpsgenieAlert()
        {
            MessageQueueIdIsZeroAndCacheIdIsZero_ShouldNotSendOpsgenieAlert(MessageQueueTypes.Attribute);
        }

        [TestMethod]
        public void ApiControllerMonitorCheckStatusAndNotify_ItemMessageQueueIdIsZeroAndCacheIdIsZero_ShouldNotSendsOpsgenieAlert()
        {
            MessageQueueIdIsZeroAndCacheIdIsZero_ShouldNotSendOpsgenieAlert(MessageQueueTypes.Item);
        }

        private void MessageQueueIdIsZeroAndCacheIdIsZero_ShouldNotSendOpsgenieAlert(string messageQueueType)
        {
            //Given
            int expectedQueueId = 0;
            int expectedNumberOfTimesMatched = 0;
            this.messageQueueCache.QueueTypeToIdMapper[messageQueueType] = new QueueData { LastMessageQueueId = expectedQueueId, NumberOfTimesMatched = 0 };
            this.mockMessageQueueQuery.Setup(q => q.Search(It.Is<GetApiMessageQueueIdParameters>(v => v.MessageQueueType == messageQueueType))).Returns(expectedQueueId);
            this.mockOpsgenieTrigger.Setup(pd => pd.TriggerAlert(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>())).Returns(new OpsgenieAlert.OpsgenieResponse());

            string expectedDescription = BuildTriggerDescription(messageQueueType);
            string expectedDetailKey = "Stuck MessageQueueId";
            string expectedMessage = "API Controller Issue";

            // When
            this.apiControllerMonitor.CheckStatusAndNotify();

            // Then
            mockOpsgenieTrigger.Verify(t =>
                t.TriggerAlert(It.Is<string>(d => d == expectedMessage), It.Is<string>(d => d == expectedDescription),
                    It.Is<Dictionary<string, string>>(d => d[expectedDetailKey] == expectedQueueId.ToString())),
                Times.Never);
            Assert.AreEqual(expectedQueueId, this.messageQueueCache.QueueTypeToIdMapper[messageQueueType].LastMessageQueueId);
            Assert.AreEqual(expectedNumberOfTimesMatched, this.messageQueueCache.QueueTypeToIdMapper[messageQueueType].NumberOfTimesMatched);
        }

        #endregion MessageQueueIdIsZeroAndCacheIdIsZero

        #region MessageQueueIdAndCacheIdAreDifferent

        [TestMethod]
        public void ApiControllerMonitorCheckStatusAndNotify_ProductMessageQueueIdAndCacheIdAreDifferent_ShouldNotSendsOpsgenieAlert()
        {
            MessageQueueIdAndCacheIdAreDifferent_ShouldNotSendsOpsgenieAlert(MessageQueueTypes.Product, 0, 323);
            MessageQueueIdAndCacheIdAreDifferent_ShouldNotSendsOpsgenieAlert(MessageQueueTypes.Product, 311, 0);
            MessageQueueIdAndCacheIdAreDifferent_ShouldNotSendsOpsgenieAlert(MessageQueueTypes.Product, 556, 2);
        }

        [TestMethod]
        public void ApiControllerMonitorCheckStatusAndNotify_PriceMessageQueueIdAndCacheIdAreDifferent_ShouldNotSendsOpsgenieAlert()
        {
            MessageQueueIdAndCacheIdAreDifferent_ShouldNotSendsOpsgenieAlert(MessageQueueTypes.Price, 0, 323);
            MessageQueueIdAndCacheIdAreDifferent_ShouldNotSendsOpsgenieAlert(MessageQueueTypes.Price, 311, 0);
            MessageQueueIdAndCacheIdAreDifferent_ShouldNotSendsOpsgenieAlert(MessageQueueTypes.Price, 556, 2);
        }

        [TestMethod]
        public void ApiControllerMonitorCheckStatusAndNotify_ItemLocaleMessageQueueIdAndCacheIdAreDifferent_ShouldNotSendsOpsgenieAlert()
        {
            MessageQueueIdAndCacheIdAreDifferent_ShouldNotSendsOpsgenieAlert(MessageQueueTypes.ItemLocale, 0, 323);
            MessageQueueIdAndCacheIdAreDifferent_ShouldNotSendsOpsgenieAlert(MessageQueueTypes.ItemLocale, 311, 0);
            MessageQueueIdAndCacheIdAreDifferent_ShouldNotSendsOpsgenieAlert(MessageQueueTypes.ItemLocale, 556, 2);
        }

        [TestMethod]
        public void ApiControllerMonitorCheckStatusAndNotify_HierarchyMessageQueueIdAndCacheIdAreDifferent_ShouldNotSendsOpsgenieAlert()
        {
            MessageQueueIdAndCacheIdAreDifferent_ShouldNotSendsOpsgenieAlert(MessageQueueTypes.Hierarchy, 0, 323);
            MessageQueueIdAndCacheIdAreDifferent_ShouldNotSendsOpsgenieAlert(MessageQueueTypes.Hierarchy, 311, 0);
            MessageQueueIdAndCacheIdAreDifferent_ShouldNotSendsOpsgenieAlert(MessageQueueTypes.Hierarchy, 556, 2);
        }

        [TestMethod]
        public void ApiControllerMonitorCheckStatusAndNotify_LocaleMessageQueueIdAndCacheIdAreDifferent_ShouldNotSendsOpsgenieAlert()
        {
            MessageQueueIdAndCacheIdAreDifferent_ShouldNotSendsOpsgenieAlert(MessageQueueTypes.Locale, 0, 323);
            MessageQueueIdAndCacheIdAreDifferent_ShouldNotSendsOpsgenieAlert(MessageQueueTypes.Locale, 311, 0);
            MessageQueueIdAndCacheIdAreDifferent_ShouldNotSendsOpsgenieAlert(MessageQueueTypes.Locale, 556, 2);
        }

        [TestMethod]
        public void ApiControllerMonitorCheckStatusAndNotify_AttributeMessageQueueIdAndCacheIdAreDifferent_ShouldNotSendsOpsgenieAlert()
        {
            MessageQueueIdAndCacheIdAreDifferent_ShouldNotSendsOpsgenieAlert(MessageQueueTypes.Attribute, 0, 323);
            MessageQueueIdAndCacheIdAreDifferent_ShouldNotSendsOpsgenieAlert(MessageQueueTypes.Attribute, 311, 0);
            MessageQueueIdAndCacheIdAreDifferent_ShouldNotSendsOpsgenieAlert(MessageQueueTypes.Attribute, 556, 2);
        }

        [TestMethod]
        public void ApiControllerMonitorCheckStatusAndNotify_ItemMessageQueueIdAndCacheIdAreDifferent_ShouldNotSendsOpsgenieAlert()
        {
            MessageQueueIdAndCacheIdAreDifferent_ShouldNotSendsOpsgenieAlert(MessageQueueTypes.Item, 0, 323);
            MessageQueueIdAndCacheIdAreDifferent_ShouldNotSendsOpsgenieAlert(MessageQueueTypes.Item, 311, 0);
            MessageQueueIdAndCacheIdAreDifferent_ShouldNotSendsOpsgenieAlert(MessageQueueTypes.Item, 556, 2);
        }

        private void MessageQueueIdAndCacheIdAreDifferent_ShouldNotSendsOpsgenieAlert(string messageQueueType, int queueId, int cacheId)
        {
            //Given
            int expectedQueueId = queueId;
            int expectedNumberOfTimesMatched = 0;
            this.messageQueueCache.QueueTypeToIdMapper[messageQueueType] = new QueueData { LastMessageQueueId = cacheId, NumberOfTimesMatched = 0 };
            this.mockMessageQueueQuery.Setup(q => q.Search(It.Is<GetApiMessageQueueIdParameters>(v => v.MessageQueueType == messageQueueType))).Returns(queueId);
            this.mockOpsgenieTrigger.Setup(pd => pd.TriggerAlert(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>())).Returns(new OpsgenieAlert.OpsgenieResponse());

            string expectedDescription = BuildTriggerDescription(messageQueueType);
            string expectedDetailKey = "Stuck MessageQueueId";
            string expectedMessage = "API Controller Issue";

            // When
            this.apiControllerMonitor.CheckStatusAndNotify();

            // Then
            mockOpsgenieTrigger.Verify(t =>
                t.TriggerAlert(It.Is<string>(d => d == expectedMessage),It.Is<string>(d => d == expectedDescription),
                    It.Is<Dictionary<string, string>>(d => d[expectedDetailKey] == expectedQueueId.ToString())),
                Times.Never);
            Assert.AreEqual(expectedQueueId, this.messageQueueCache.QueueTypeToIdMapper[messageQueueType].LastMessageQueueId);
            Assert.AreEqual(expectedNumberOfTimesMatched, this.messageQueueCache.QueueTypeToIdMapper[messageQueueType].NumberOfTimesMatched);
        }

        private void SetUpSettingsValue()
        {
            System.DateTime today = System.DateTime.Now;
            System.TimeSpan duration = new System.TimeSpan(0, 2, -2, 0);
            today = today.Add(duration);
            mockSettings.Setup(m => m.NumberOfMinutesBeforeStoreOpens).Returns(120);
            mockSettings.Setup(m => m.StoreOpenCentralTime_FL).Returns(new LocalTime(today.Hour, today.Minute));
        }

        [TestMethod]
        public void NumberOfUnprocessedPriceQueueRowsGreaterThanZero_ShouldSendsOpsgenieAlert()
        {
            //Given   
            SetUpSettingsValue();
            mockMessageUnprocessedRowCountQuery.Setup(m => m.Search(It.IsAny<GetApiMessageUnprocessedRowCountParameters>())).Returns(1);
            //When
            apiControllerMonitor.CheckStatusAndNotify();

            //Then
            mockOpsgenieTrigger.Verify(m => m.TriggerAlert(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.AtLeastOnce);
        }

        [TestMethod]
        public void NumberOfUnprocessedPriceQueueRowsEqualToZero_ShouldNotSendOpsgenieAlert()
        {
            //Given   
            SetUpSettingsValue();
            mockMessageUnprocessedRowCountQuery.Setup(m => m.Search(It.IsAny<GetApiMessageUnprocessedRowCountParameters>())).Returns(0);

            //When
            apiControllerMonitor.CheckStatusAndNotify();

            //Then
            mockOpsgenieTrigger.Verify(m => m.TriggerAlert(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Never);
        }

        #endregion MessageQueueIdAndCacheIdAreDifferent
    }
}