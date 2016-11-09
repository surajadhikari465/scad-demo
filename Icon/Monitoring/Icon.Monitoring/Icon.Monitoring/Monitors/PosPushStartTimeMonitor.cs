namespace Icon.Monitoring.Monitors
{
    using Common.Constants;
    using Common.Enums;
    using Common.Settings;
    using Icon.Monitoring.Common.PagerDuty;
    using Icon.Monitoring.DataAccess.Model;
    using Icon.Monitoring.DataAccess.Queries;
    using Logging;
    using NodaTime;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    public class PosPushStartTimeMonitor : TimedControllerMonitor
    {
        private const string PosPushStartTimeConfigPrefix = "PosPushStartTime_";
        private const string PosPushLogMessage = "Scheduled POS Push Job is starting";
        private const string PosPushStartTimePagerDutyDescription = "POS Push has not started for the following region: ";
        private const string TimeZone = "America/Chicago";

        private readonly IQueryByRegionHandler<GetAppLogByAppAndMessageParameters, AppLog> appLogQuery;
        private readonly IPagerDutyTrigger pagerDutyTrigger;
        public Dictionary<IrmaRegions, int> PosPushStartTimeTracker;
        private IDateTimeZoneProvider dateTimeZoneProvider;
        private IClock clock;

        public PosPushStartTimeMonitor(
            IMonitorSettings settings,
            IPagerDutyTrigger pagerDutyTrigger,
            IQueryByRegionHandler<GetAppLogByAppAndMessageParameters, AppLog> appLogQuery,
            IDateTimeZoneProvider dateTimeZoneProvider,
            IClock clock,
            ILogger logger)
        {
            this.settings = settings;
            this.pagerDutyTrigger = pagerDutyTrigger;
            this.appLogQuery = appLogQuery;
            this.dateTimeZoneProvider = dateTimeZoneProvider;
            this.clock = clock;
            this.logger = logger;

            this.PosPushStartTimeTracker = new Dictionary<IrmaRegions, int>
            {
                { IrmaRegions.FL, 0 },
                { IrmaRegions.MA, 0 },
                { IrmaRegions.MW, 0 },
                { IrmaRegions.NA, 0 },
                { IrmaRegions.NC, 0 },
                { IrmaRegions.NE, 0 },
                { IrmaRegions.PN, 0 },
                { IrmaRegions.RM, 0 },
                { IrmaRegions.SO, 0 },
                { IrmaRegions.SP, 0 },
                { IrmaRegions.SW, 0 },
                { IrmaRegions.UK, 0 },
            };
        }

        /// <summary>
        /// This will query the AppLog table for the most recent log statement with the Message of 'Scheduled POS Push is starting'.
        /// We pass in a StartDate and EndDate to only query AppLog for the current day
        /// </summary>
        protected override void TimedCheckStatusAndNotify()
        {
            var pagerDutyDetails = new Dictionary<string, string>();
            var pagerDutyDescription = new StringBuilder(PosPushStartTimePagerDutyDescription);

            var queryParameters = new GetAppLogByAppAndMessageParameters
            {
                Message = PosPushLogMessage,
                AppConfigAppName = IrmaAppConfigAppNames.PosPushJob,
                StartDate = DateTime.Today,
                EndDate = DateTime.Now
            };

            int numberOfJobsNotStartedOnTime = 0;
            AppLog appLog = null;

            foreach (IrmaRegions region in Enum.GetValues(typeof(IrmaRegions)))
            {
                this.appLogQuery.TargetRegion = region;
                appLog = this.appLogQuery.Search(queryParameters);

                LocalTime configuredStartTime = GetConfiguredStartTimeByRegion(region);
                LocalTime currentTime = GetLocalDateTimeInCentralTime(this.clock.Now).TimeOfDay;

                bool hasStartedOnTime = HasPosPushStartedOnTime(appLog, configuredStartTime, currentTime);

                if (hasStartedOnTime)
                {
                    this.PosPushStartTimeTracker[region] = 0;
                }
                else if (!hasStartedOnTime && this.PosPushStartTimeTracker[region] == 0)
                {
                    BuildPagerDutyDetails(
                        pagerDutyDetails,
                        pagerDutyDescription,
                        numberOfJobsNotStartedOnTime,
                        region,
                        configuredStartTime,
                        currentTime);

                    this.PosPushStartTimeTracker[region] = 1;
                }
            }

            if (pagerDutyDetails.Any())
            {
                PagerDutyResponse response = this.pagerDutyTrigger.TriggerIncident(pagerDutyDescription.ToString(), pagerDutyDetails);
            }
        }

        private void BuildPagerDutyDetails(Dictionary<string, string> pagerDutyDetails, StringBuilder pagerDutyDescription, int numberOfJobsNotStartedOnTime, IrmaRegions region, LocalTime configuredStartTime, LocalTime currentTime)
        {
            pagerDutyDescription.Append(region).Append(" ");
            pagerDutyDetails.Add(IrmaAppConfigAppNames.PosPushJob + "_" + region.ToString(),
                "The current time is " + currentTime.ToString("HH:mm:ss", DateTimeFormatInfo.InvariantInfo) +
                " and the push should have started before the configured time: " + configuredStartTime.ToString("HH:mm:ss", DateTimeFormatInfo.InvariantInfo));
        }

        /// <summary>
        /// Determines whether or not the POS Push for a particular region has not started compared to its configured start time.
        /// NOTE: This is utilizing the NodaTime library and attempts to put all times into the central time zone for clarity.
        /// </summary>
        /// <param name="appLog">The AppLog object from the IRMA Db</param>
        /// <param name="region">The region that is being checked</param>
        /// <returns>returns true there is a log statement for the day indicating that the POS Push has started, regardless of time;
        /// returns false if there is no log statement for the day and the current time of day has passed the configured start time</returns>
        private bool HasPosPushStartedOnTime(AppLog appLog, LocalTime configuredStartTime, LocalTime currentTime)
        {
            if (appLog == null)
            {
                return currentTime < configuredStartTime;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Gets the LocalDateTime object for a NodaTime Instant.  The LocalDateTime is not aware of timezone or offset.
        /// </summary>
        /// <param name="instant">Instant in time</param>
        /// <returns>LocalDateTime of the Instant provided.</returns>
        private LocalDateTime GetLocalDateTimeInCentralTime(Instant instant)
        {
            DateTimeZone centralDateTimeZone = this.dateTimeZoneProvider[TimeZone]; // TimeZone is a private constant
            LocalDateTime localDateTime = instant.InZone(centralDateTimeZone).LocalDateTime;

            return localDateTime;
        }

        /// <summary>
        /// Gets the pos push start time of the MonitorSettings for that particular region
        /// </summary>
        /// <param name="region"></param>
        /// <returns></returns>
        private LocalTime GetConfiguredStartTimeByRegion(IrmaRegions region)
        {
            var settingsProperties = this.settings.GetType().GetProperties();
            LocalTime configuredStartTime = (LocalTime)settingsProperties
                .First(p => p.Name.Contains(PosPushStartTimeConfigPrefix + region.ToString()))
                .GetValue(this.settings);

            return configuredStartTime;
        }
    }
}
