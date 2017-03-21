using Icon.Common.DataAccess;
using Icon.Logging;
using Icon.Monitoring.Common;
using Icon.Monitoring.Common.PagerDuty;
using Icon.Monitoring.Common.Settings;
using Icon.Monitoring.DataAccess;
using Icon.Monitoring.DataAccess.Queries;
using Icon.Monitoring.Monitors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NodaTime;
using NodaTime.Testing;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading;

namespace Icon.Monitoring.Tests.Monitors
{
    [TestClass]
    public class ApiControllerMonitorTests
    {
        private Mock<IPagerDutyTrigger> mockPagerDutyTrigger;
        private Mock<Icon.Common.DataAccess.IQueryHandler<GetApiMessageQueueIdParameters, int>> mockMessageQueueQuery;
        private Mock<Icon.Common.DataAccess.IQueryHandler<GetApiMessageUnprocessedRowCountParameters, int>> mockMessageUnprocessedRowCountQuery;

        private SqlDbProvider db;
        private List<string> testRegions;
        private Mock<IMonitorSettings> mockSettings;
        private ApiControllerMonitor apiControllerMonitor;
 
        private IClock fakeClock;
        private IDateTimeZoneProvider dateTimeZoneProvider;
        [TestInitialize]
        public void Initialize()
        {
            this.mockPagerDutyTrigger = new Mock<IPagerDutyTrigger>();
            this.mockMessageQueueQuery = new Mock<Icon.Common.DataAccess.IQueryHandler<GetApiMessageQueueIdParameters, int>>();
            this.mockMessageUnprocessedRowCountQuery = new Mock<Icon.Common.DataAccess.IQueryHandler<GetApiMessageUnprocessedRowCountParameters, int>>();
            this.fakeClock = new FakeClock(Instant.FromDateTimeUtc(DateTime.UtcNow));
            this.dateTimeZoneProvider = DateTimeZoneProviders.Tzdb;
            this.mockSettings = new Mock<IMonitorSettings>();
           
            this.apiControllerMonitor = new ApiControllerMonitor(
                this.mockSettings.Object,
                this.mockMessageQueueQuery.Object,
                 this.mockMessageUnprocessedRowCountQuery.Object,
                this.mockPagerDutyTrigger.Object,
                  this.dateTimeZoneProvider,
                this.fakeClock,
                new Mock<ILogger>().Object);
            testRegions = new List<string> { "FL" };
            SetUpApiControllerMonitorSettings();
            apiControllerMonitor.ByPassConfiguredRunInterval = true;
            SetMessageQueueToIdMapperToZeroValues();
        }


        private void SetUpApiControllerMonitorSettings()
        {          
            mockSettings.SetupGet(m => m.ApiControllerMonitorRegions).Returns(testRegions);
            mockSettings.SetupGet(m => m.NumberOfMinutesBeforeStoreOpens).Returns(120);
            mockSettings.SetupGet(m => m.StoreOpenCentralTime_FL).Returns(new LocalTime(16,0,0));

            Dictionary<string, TimeSpan> MonitorTimers = new Dictionary<string, TimeSpan>();
            MonitorTimers.Add("ApiControllerMonitorTimer", TimeSpan.FromMilliseconds( 900000));
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

        private void SetMessageQueueToIdMapperToZeroValues()
        {
            MessageQueueCache.QueueTypeToIdMapper[MessageQueueTypes.Product].LastMessageQueueId = 0;
            MessageQueueCache.QueueTypeToIdMapper[MessageQueueTypes.Product].NumberOfTimesMatched = 0;
            MessageQueueCache.QueueTypeToIdMapper[MessageQueueTypes.Price].LastMessageQueueId = 0;
            MessageQueueCache.QueueTypeToIdMapper[MessageQueueTypes.Price].NumberOfTimesMatched = 0;
            MessageQueueCache.QueueTypeToIdMapper[MessageQueueTypes.ItemLocale].LastMessageQueueId = 0;
            MessageQueueCache.QueueTypeToIdMapper[MessageQueueTypes.ItemLocale].NumberOfTimesMatched = 0;
            MessageQueueCache.QueueTypeToIdMapper[MessageQueueTypes.Hierarchy].LastMessageQueueId = 0;
            MessageQueueCache.QueueTypeToIdMapper[MessageQueueTypes.Hierarchy].NumberOfTimesMatched = 0;
            MessageQueueCache.QueueTypeToIdMapper[MessageQueueTypes.Locale].LastMessageQueueId = 0;
            MessageQueueCache.QueueTypeToIdMapper[MessageQueueTypes.Locale].NumberOfTimesMatched = 0;
            MessageQueueCache.QueueTypeToIdMapper[MessageQueueTypes.ProductSelectionGroup].LastMessageQueueId = 0;
            MessageQueueCache.QueueTypeToIdMapper[MessageQueueTypes.ProductSelectionGroup].NumberOfTimesMatched = 0;

        }

        #region MessageQueueIdMatchesCacheFirstTime

        [TestMethod]
        public void ApiControllerMonitorCheckStatusAndNotify_ProductMessageQueueIdMatchesCacheFirstTime_ShouldSendsPagerDutyAlert()
        {
            MessageQueueIdMatchesFirstTime_ShouldSendPagerDutyAlert(MessageQueueTypes.Product);
        }

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

        [TestMethod]
        public void ApiControllerMonitorCheckStatusAndNotify_LocaleMessageQueueIdMatchesCacheFirstTime_ShouldSendsPagerDutyAlert()
        {
            MessageQueueIdMatchesFirstTime_ShouldSendPagerDutyAlert(MessageQueueTypes.Locale);
        }

        [TestMethod]
        public void ApiControllerMonitorCheckStatusAndNotify_HierarchyMessageQueueIdMatchesCacheFirstTime_ShouldSendsPagerDutyAlert()
        {
            MessageQueueIdMatchesFirstTime_ShouldSendPagerDutyAlert(MessageQueueTypes.Hierarchy);
        }

        [TestMethod]
        public void ApiControllerMonitorCheckStatusAndNotify_ProductSelectionGroupMessageQueueIdMatchesCacheFirstTime_ShouldSendsPagerDutyAlert()
        {
            MessageQueueIdMatchesFirstTime_ShouldSendPagerDutyAlert(MessageQueueTypes.ProductSelectionGroup);
        }

        private void MessageQueueIdMatchesFirstTime_ShouldSendPagerDutyAlert(string messagQueueType)
        {
            // Given
            int expectedQueueId = new Random(1).Next(27000);
            MessageQueueCache.QueueTypeToIdMapper[messagQueueType] = new QueueData { LastMessageQueueId = expectedQueueId, NumberOfTimesMatched = 0 };
            this.mockMessageQueueQuery.Setup(q => q.Search(It.Is<GetApiMessageQueueIdParameters>(v => v.MessageQueueType == messagQueueType))).Returns(expectedQueueId);
            this.mockPagerDutyTrigger.Setup(pd => pd.TriggerIncident(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>())).Returns(new PagerDutyResponse());

            string expectedDescription = BuildTriggerDescription(messagQueueType);
            string expectedDetailKey = "Stuck MessageQueueId";

            // When
            this.apiControllerMonitor.CheckStatusAndNotify();

            // Then
            mockPagerDutyTrigger.Verify(t =>
                t.TriggerIncident(It.Is<string>(d => d == expectedDescription),
                    It.Is<Dictionary<string, string>>(d => d[expectedDetailKey] == expectedQueueId.ToString())),
                Times.Once);
            Assert.AreEqual(expectedQueueId, MessageQueueCache.QueueTypeToIdMapper[messagQueueType].LastMessageQueueId);
            Assert.AreEqual(1, MessageQueueCache.QueueTypeToIdMapper[messagQueueType].NumberOfTimesMatched);
        }

        #endregion MessageQueueIdMatchesCacheFirstTime

        #region MessageQueueIdMatchesCacheASecondTime

        [TestMethod]
        public void ApiControllerMonitorCheckStatusAndNotify_ProductMessageQueueIdMatchesCacheASecondTime_ShouldNotSendsPagerDutyAlert()
        {
            MessageQueueIdMatchesCacheSecondTime_ShouldNotSendPagerDutyAlert(MessageQueueTypes.Product);
        }

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

        [TestMethod]
        public void ApiControllerMonitorCheckStatusAndNotify_HierarchyMessageQueueIdMatchesCacheASecondTimeOrSubsequentTimes_ShouldNotSendsPagerDutyAlert()
        {
            MessageQueueIdMatchesCacheSecondTime_ShouldNotSendPagerDutyAlert(MessageQueueTypes.Hierarchy);
        }

        [TestMethod]
        public void ApiControllerMonitorCheckStatusAndNotify_LocaleMessageQueueIdMatchesCacheASecondTimeOrSubsequentTimes_ShouldNotSendsPagerDutyAlert()
        {
            MessageQueueIdMatchesCacheSecondTime_ShouldNotSendPagerDutyAlert(MessageQueueTypes.Locale);
        }

        [TestMethod]
        public void ApiControllerMonitorCheckStatusAndNotify_ProductSelectionGroupMessageQueueIdMatchesCacheASecondTimeOrSubsequentTimes_ShouldNotSendsPagerDutyAlert()
        {
            MessageQueueIdMatchesCacheSecondTime_ShouldNotSendPagerDutyAlert(MessageQueueTypes.ProductSelectionGroup);
        }

        private void MessageQueueIdMatchesCacheSecondTime_ShouldNotSendPagerDutyAlert(string messageQueueType)
        {
            // Given
            int expectedQueueId = new Random(1).Next(27000);
            int expectedNumberOfTimesMatched = 2;
            MessageQueueCache.QueueTypeToIdMapper[messageQueueType] = new QueueData { LastMessageQueueId = expectedQueueId, NumberOfTimesMatched = 1 };
            this.mockMessageQueueQuery.Setup(q => q.Search(It.Is<GetApiMessageQueueIdParameters>(v => v.MessageQueueType == messageQueueType))).Returns(expectedQueueId);
            this.mockPagerDutyTrigger.Setup(pd => pd.TriggerIncident(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>())).Returns(new PagerDutyResponse());

            string expectedDescription = BuildTriggerDescription(messageQueueType);
            string expectedDetailKey = "Stuck MessageQueueId";

            // When
            this.apiControllerMonitor.CheckStatusAndNotify();

            // Then
            mockPagerDutyTrigger.Verify(t =>
                t.TriggerIncident(It.Is<string>(d => d == expectedDescription),
                    It.Is<Dictionary<string, string>>(d => d[expectedDetailKey] == expectedQueueId.ToString())),
                Times.Never);
            Assert.AreEqual(expectedQueueId, MessageQueueCache.QueueTypeToIdMapper[messageQueueType].LastMessageQueueId);
            Assert.AreEqual(expectedNumberOfTimesMatched, MessageQueueCache.QueueTypeToIdMapper[messageQueueType].NumberOfTimesMatched);
        }

        #endregion MessageQueueIdMatchesCacheASecondTime

        #region MessageQueueIdIsZeroAndCacheIdIsZero

        [TestMethod]
        public void ApiControllerMonitorCheckStatusAndNotify_ProductMessageQueueIdIsZeroAndCacheIdIsZero_ShouldNotSendsPagerDutyAlert()
        {
            MessageQueueIdIsZeroAndCacheIdIsZero_ShouldNotSendPagerDutyAlert(MessageQueueTypes.Product);
        }

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

        [TestMethod]
        public void ApiControllerMonitorCheckStatusAndNotify_HierarchyMessageQueueIdIsZeroAndCacheIdIsZero_ShouldNotSendsPagerDutyAlert()
        {
            MessageQueueIdIsZeroAndCacheIdIsZero_ShouldNotSendPagerDutyAlert(MessageQueueTypes.Hierarchy);
        }

        [TestMethod]
        public void ApiControllerMonitorCheckStatusAndNotify_LocaleMessageQueueIdIsZeroAndCacheIdIsZero_ShouldNotSendsPagerDutyAlert()
        {
            MessageQueueIdIsZeroAndCacheIdIsZero_ShouldNotSendPagerDutyAlert(MessageQueueTypes.Locale);
        }

        private void MessageQueueIdIsZeroAndCacheIdIsZero_ShouldNotSendPagerDutyAlert(string messageQueueType)
        {
            //Given
            int expectedQueueId = 0;
            int expectedNumberOfTimesMatched = 0;
            MessageQueueCache.QueueTypeToIdMapper[messageQueueType] = new QueueData { LastMessageQueueId = expectedQueueId, NumberOfTimesMatched = 0 };
            this.mockMessageQueueQuery.Setup(q => q.Search(It.Is<GetApiMessageQueueIdParameters>(v => v.MessageQueueType == messageQueueType))).Returns(expectedQueueId);
            this.mockPagerDutyTrigger.Setup(pd => pd.TriggerIncident(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>())).Returns(new PagerDutyResponse());

            string expectedDescription = BuildTriggerDescription(messageQueueType);
            string expectedDetailKey = "Stuck MessageQueueId";

            // When
            this.apiControllerMonitor.CheckStatusAndNotify();

            // Then
            mockPagerDutyTrigger.Verify(t =>
                t.TriggerIncident(It.Is<string>(d => d == expectedDescription),
                    It.Is<Dictionary<string, string>>(d => d[expectedDetailKey] == expectedQueueId.ToString())),
                Times.Never);
            Assert.AreEqual(expectedQueueId, MessageQueueCache.QueueTypeToIdMapper[messageQueueType].LastMessageQueueId);
            Assert.AreEqual(expectedNumberOfTimesMatched, MessageQueueCache.QueueTypeToIdMapper[messageQueueType].NumberOfTimesMatched);
        }

        #endregion MessageQueueIdIsZeroAndCacheIdIsZero

        #region MessageQueueIdAndCacheIdAreDifferent

        [TestMethod]
        public void ApiControllerMonitorCheckStatusAndNotify_ProductMessageQueueIdAndCacheIdAreDifferent_ShouldNotSendsPagerDutyAlert()
        {
            MessageQueueIdAndCacheIdAreDifferent_ShouldNotSendsPagerDutyAlert(MessageQueueTypes.Product, 0, 323);
            MessageQueueIdAndCacheIdAreDifferent_ShouldNotSendsPagerDutyAlert(MessageQueueTypes.Product, 311, 0);
            MessageQueueIdAndCacheIdAreDifferent_ShouldNotSendsPagerDutyAlert(MessageQueueTypes.Product, 556, 2);
        }

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

        [TestMethod]
        public void ApiControllerMonitorCheckStatusAndNotify_HierarchyMessageQueueIdAndCacheIdAreDifferent_ShouldNotSendsPagerDutyAlert()
        {
            MessageQueueIdAndCacheIdAreDifferent_ShouldNotSendsPagerDutyAlert(MessageQueueTypes.Hierarchy, 0, 323);
            MessageQueueIdAndCacheIdAreDifferent_ShouldNotSendsPagerDutyAlert(MessageQueueTypes.Hierarchy, 311, 0);
            MessageQueueIdAndCacheIdAreDifferent_ShouldNotSendsPagerDutyAlert(MessageQueueTypes.Hierarchy, 556, 2);
        }

        [TestMethod]
        public void ApiControllerMonitorCheckStatusAndNotify_LocaleMessageQueueIdAndCacheIdAreDifferent_ShouldNotSendsPagerDutyAlert()
        {
            MessageQueueIdAndCacheIdAreDifferent_ShouldNotSendsPagerDutyAlert(MessageQueueTypes.Locale, 0, 323);
            MessageQueueIdAndCacheIdAreDifferent_ShouldNotSendsPagerDutyAlert(MessageQueueTypes.Locale, 311, 0);
            MessageQueueIdAndCacheIdAreDifferent_ShouldNotSendsPagerDutyAlert(MessageQueueTypes.Locale, 556, 2);
        }

        private void MessageQueueIdAndCacheIdAreDifferent_ShouldNotSendsPagerDutyAlert(string messageQueueType, int queueId, int cacheId)
        {
            //Given
            int expectedQueueId = queueId;
            int expectedNumberOfTimesMatched = 0;
            MessageQueueCache.QueueTypeToIdMapper[messageQueueType] = new QueueData { LastMessageQueueId = cacheId, NumberOfTimesMatched = 0 };
            this.mockMessageQueueQuery.Setup(q => q.Search(It.Is<GetApiMessageQueueIdParameters>(v => v.MessageQueueType == messageQueueType))).Returns(queueId);
            this.mockPagerDutyTrigger.Setup(pd => pd.TriggerIncident(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>())).Returns(new PagerDutyResponse());

            string expectedDescription = BuildTriggerDescription(messageQueueType);
            string expectedDetailKey = "Stuck MessageQueueId";

            // When
            this.apiControllerMonitor.CheckStatusAndNotify();

            // Then
            mockPagerDutyTrigger.Verify(t =>
                t.TriggerIncident(It.Is<string>(d => d == expectedDescription),
                    It.Is<Dictionary<string, string>>(d => d[expectedDetailKey] == expectedQueueId.ToString())),
                Times.Never);
            Assert.AreEqual(expectedQueueId, MessageQueueCache.QueueTypeToIdMapper[messageQueueType].LastMessageQueueId);
            Assert.AreEqual(expectedNumberOfTimesMatched, MessageQueueCache.QueueTypeToIdMapper[messageQueueType].NumberOfTimesMatched);
        }
      
        [TestMethod]
        public void NumberOfUnprocessedPriceQueueRowsGreaterThanZero_ShouldSendPagerDutyAlert()
        {
            //Given   
            System.DateTime today = System.DateTime.Now;
            System.TimeSpan duration = new System.TimeSpan(0, 2, -2, 0);
            today = today.Add(duration);
            mockSettings.Setup(m => m.NumberOfMinutesBeforeStoreOpens).Returns(120);
            mockSettings.Setup(m => m.StoreOpenCentralTime_FL).Returns(new LocalTime(today.Hour,today.Minute));
            mockMessageUnprocessedRowCountQuery.Setup(m => m.Search(It.IsAny<GetApiMessageUnprocessedRowCountParameters>())).Returns(1);

            //When
            apiControllerMonitor.CheckStatusAndNotify();

            //Then
            mockPagerDutyTrigger.Verify(m => m.TriggerIncident(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.AtLeastOnce);
        }

        [TestMethod]
        public void NumberOfUnprocessedPriceQueueRowsEqualToZero_ShouldNotSendPagerDutyAlert()
        {
            //Given   
            System.DateTime today = System.DateTime.Now;
            System.TimeSpan duration = new System.TimeSpan(0, 2, -2, 0);
            today = today.Add(duration);
            mockSettings.Setup(m => m.NumberOfMinutesBeforeStoreOpens).Returns(120);
            mockSettings.Setup(m => m.StoreOpenCentralTime_FL).Returns(new LocalTime(today.Hour, today.Minute));
            mockMessageUnprocessedRowCountQuery.Setup(m => m.Search(It.IsAny<GetApiMessageUnprocessedRowCountParameters>())).Returns(0);

            //When
            apiControllerMonitor.CheckStatusAndNotify();

            //Then
            mockPagerDutyTrigger.Verify(m => m.TriggerIncident(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Never);
        }

        #endregion MessageQueueIdAndCacheIdAreDifferent
    }
   


}
