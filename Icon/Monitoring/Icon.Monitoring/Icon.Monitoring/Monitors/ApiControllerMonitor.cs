namespace Icon.Monitoring.Monitors
{
    using System.Collections.Generic;

    using Icon.Common.DataAccess;
    using Icon.Logging;
    using Icon.Monitoring.Common;
    using Icon.Monitoring.Common.PagerDuty;
    using Icon.Monitoring.Common.Settings;
    using Icon.Monitoring.DataAccess.Queries;
    using System;
    using NodaTime;
    using System.Linq;

    public class ApiControllerMonitor : TimedControllerMonitor
    {
        private const string APIControllerRunningSlow = "API Controller is running very slowly. Please look into it. The cache may need to be cleared on stored procedure. Use SP_WHO to determine the stored procedure.";
        private const string StoreOpenTimeConfigPrefix = "StoreOpenCentralTime_";
        private const string TimeZone = "America/Chicago";
        private readonly IQueryHandler<GetApiMessageQueueIdParameters, int> messageQueueQuery;
        private readonly IQueryHandler<GetApiMessageUnprocessedRowCountParameters, int> messageQueueUnprocessedRowCountQuery;
        private readonly IPagerDutyTrigger pagerDutyTrigger;
        private IClock clock;
        private IDateTimeZoneProvider dateTimeZoneProvider;

        public ApiControllerMonitor(
            IMonitorSettings settings,
            IQueryHandler<GetApiMessageQueueIdParameters, int> messageQueueQuery,
            IQueryHandler<GetApiMessageUnprocessedRowCountParameters, int> messageQueueUnprocessedRowCountQuery,
            IPagerDutyTrigger pagerDutyTrigger,
            IDateTimeZoneProvider dateTimeZoneProvider,
            IClock clock,
            ILogger logger)
        {
            this.settings = settings;
            this.logger = logger;
            this.messageQueueQuery = messageQueueQuery;
            this.dateTimeZoneProvider = dateTimeZoneProvider;
            this.clock = clock;
            this.messageQueueUnprocessedRowCountQuery = messageQueueUnprocessedRowCountQuery;
            this.pagerDutyTrigger = pagerDutyTrigger;
        }

        protected override void TimedCheckStatusAndNotify()
        {
            // Get MessageQueueIDs for each type of MessageQueue
            this.CheckMessageQueueId(MessageQueueTypes.Product);
            this.CheckMessageQueueId(MessageQueueTypes.Price);
            this.CheckMessageQueueId(MessageQueueTypes.ItemLocale);
            this.CheckMessageQueueId(MessageQueueTypes.Hierarchy);
            this.CheckMessageQueueId(MessageQueueTypes.Locale);
            this.CheckMessageQueueId(MessageQueueTypes.ProductSelectionGroup);
            var regions = settings.ApiControllerMonitorRegions;
            // loop through each region
            foreach (var region in regions)
            {
                if (ShouldCheckDataInMessageQueuePriceAndItemTable(region))
                {
                    CheckForUnprocessedRows(region);
                }
            }
        }

        private void CheckForUnprocessedRows(string region)
        {  //check Message Queue Price
            int numberOfUnprocessedMessageQueuePriceRows = CheckMessageQueuePriceTableForUnprocessedRows(region);
            if (numberOfUnprocessedMessageQueuePriceRows > 0)
            {
                TriggerPagerDutyIncident(APIControllerRunningSlow,
                             new Dictionary<string, string>()
                             {
                                { "Number of unprocessed Message Queue Price Rows: ", numberOfUnprocessedMessageQueuePriceRows.ToString() }
                             });
            }
           //check item locale
            int numberOfUnprocessedMessageQueueItemLocaleRows = CheckMessageQueueItemLocaleForUnprocessedRows(region);
            if (numberOfUnprocessedMessageQueueItemLocaleRows > 0)
            {
                TriggerPagerDutyIncident(APIControllerRunningSlow,
                             new Dictionary<string, string>()
                             {
                                { "Number of unprocessed Item Locale Queue Price Rows: ", numberOfUnprocessedMessageQueueItemLocaleRows.ToString() }
                             });

            }
        }
        private bool ShouldCheckDataInMessageQueuePriceAndItemTable(string regionCode)
        {
            DayOfWeek blackOutDay;
            LocalTime currentTime = GetLocalDateTimeInCentralTime(this.clock.Now).TimeOfDay;
            if (!Enum.TryParse(settings.ApiControllerMonitorBlackoutDay, out blackOutDay))
            {
                blackOutDay = DayOfWeek.Sunday;
            }

            if ((currentTime.LocalDateTime.TimeOfDay > settings.ApiControllerMonitorBlackoutStart && currentTime.LocalDateTime.TimeOfDay < settings.ApiControllerMonitorBlackoutEnd) && DateTime.Now.DayOfWeek == blackOutDay)
            {
                return false;
            }

            TimeSpan configuredInterval = TimeSpan.FromMilliseconds(0);

            LocalTime openTime = GetConfiguredOpenTimeByRegion(regionCode);
            long numberOfMinutes = settings.NumberOfMinutesBeforeStoreOpens;

            settings.MonitorTimers.TryGetValue(this.GetType().Name + "Timer", out configuredInterval);
            // Example: current time 5 a.m, store open time 3 a.m..then only run check for unprocessed rows between 3 and 3:15-assuming job runs every 15 min)
            if (openTime.PlusMinutes(0 - numberOfMinutes) < currentTime && openTime.PlusMinutes(0 - numberOfMinutes + configuredInterval.Minutes) > currentTime)
            {
                return true;

            }
            return false;
        }

        private int CheckMessageQueuePriceTableForUnprocessedRows(string regionCode)
        {
            GetApiMessageUnprocessedRowCountParameters queryParameters = new GetApiMessageUnprocessedRowCountParameters();
            queryParameters.MessageQueueType = MessageQueueTypes.Price;
            queryParameters.RegionCode = regionCode;
            int numberOfUnprocessedMessageQueuePriceRows = messageQueueUnprocessedRowCountQuery.Search(queryParameters);

            return numberOfUnprocessedMessageQueuePriceRows;
        }

        private int CheckMessageQueueItemLocaleForUnprocessedRows(string regionCode)
        {
            GetApiMessageUnprocessedRowCountParameters queryParameters = new GetApiMessageUnprocessedRowCountParameters();
            queryParameters.MessageQueueType = MessageQueueTypes.ItemLocale;
            queryParameters.RegionCode = regionCode;
            int numberOfUnprocessedMessageQueueItemLocaleRows = messageQueueUnprocessedRowCountQuery.Search(queryParameters);
            return numberOfUnprocessedMessageQueueItemLocaleRows;
        }

        private LocalTime GetConfiguredOpenTimeByRegion(string regionCode)
        {
            var settingsProperties = this.settings.GetType().GetProperties();
            LocalTime configuredStartTime = (LocalTime)settingsProperties
                .First(p => p.Name.Contains(StoreOpenTimeConfigPrefix + regionCode))
                .GetValue(this.settings);

            return configuredStartTime;
        }

        private LocalDateTime GetLocalDateTimeInCentralTime(Instant instant)
        {
            DateTimeZone centralDateTimeZone = this.dateTimeZoneProvider[TimeZone]; // TimeZone is a private constant
            LocalDateTime localDateTime = instant.InZone(centralDateTimeZone).LocalDateTime;

            return localDateTime;
        }

        private void CheckMessageQueueId(string queueType)
        {
            GetApiMessageQueueIdParameters queryParameters = new GetApiMessageQueueIdParameters();
            queryParameters.MessageQueueType = queueType;
            int id = this.messageQueueQuery.Search(queryParameters);

            if (MessageQueueCache.QueueTypeToIdMapper[queueType].LastMessageQueueId == id
                && MessageQueueCache.QueueTypeToIdMapper[queueType].NumberOfTimesMatched == 0
                && id != 0)
            {
                string description = BuildTriggerDescription(queueType);
                Dictionary<string, string> details = BuildTriggerDetails(id);
                TriggerPagerDutyIncident(description, details);

                MessageQueueCache.QueueTypeToIdMapper[queueType].NumberOfTimesMatched++;
            }
            else if (MessageQueueCache.QueueTypeToIdMapper[queueType].LastMessageQueueId == id
                && MessageQueueCache.QueueTypeToIdMapper[queueType].NumberOfTimesMatched > 0)
            {
                MessageQueueCache.QueueTypeToIdMapper[queueType].NumberOfTimesMatched++;
            }
            else
            {
                MessageQueueCache.QueueTypeToIdMapper[queueType].LastMessageQueueId = id;
                MessageQueueCache.QueueTypeToIdMapper[queueType].NumberOfTimesMatched = 0;
            }
        }

        private void TriggerPagerDutyIncident(string description, Dictionary<string, string> jsonDetails)
        {
            PagerDutyResponse response = this.pagerDutyTrigger.TriggerIncident(description, jsonDetails);
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
    }
}
