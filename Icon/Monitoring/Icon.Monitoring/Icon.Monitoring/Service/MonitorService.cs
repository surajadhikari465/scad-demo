namespace Icon.Monitoring.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Timers;

    using Icon.Common.Email;
    using Icon.Monitoring.Common.Settings;
    using Icon.Monitoring.Monitors;
    using NodaTime;

    public class MonitorService : IMonitorService
    {
        private Timer monitorServiceTimer;
        private IMonitorSettings settings;
        private readonly IEmailClient emailClient;
        private readonly List<IMonitor> monitors;
        private IClock clock;
        private const string TimeZone = "America/Chicago";
        private IDateTimeZoneProvider dateTimeZoneProvider;

        public MonitorService(
            IEnumerable<IMonitor> monitors,
            IMonitorSettings settings,
            IEmailClient emailClient,
            IDateTimeZoneProvider dateTimeZoneProvider,
            IClock clock)

        {
            this.monitors = monitors.ToList();
            this.settings = settings;
            this.emailClient = emailClient;
            this.monitorServiceTimer = new Timer(settings.MonitorServiceTimer);
            this.dateTimeZoneProvider = dateTimeZoneProvider;
            this.clock = clock;
        }

        public void Start()
        {
            this.monitorServiceTimer.Start();
            this.monitorServiceTimer.Elapsed += (s, e) =>
            {
                this.monitorServiceTimer.Stop();
                this.RunService();
                this.monitorServiceTimer.Start();
            };
        }

        public void Stop()
        {
            this.monitorServiceTimer.Stop();
        }

        private void RunService()
        {
            if (!settings.SendPagerDutyNotifications) return;

            DayOfWeek blackOutDay;
            LocalTime currentTime = GetLocalDateTimeInCentralTime(this.clock.Now).TimeOfDay;
            if (!Enum.TryParse(settings.MaintenanceDay, out blackOutDay))
            {
                blackOutDay = DayOfWeek.Sunday;
            }

            if ((currentTime.LocalDateTime.TimeOfDay >= settings.MaintenanceStartTime && currentTime.LocalDateTime.TimeOfDay <= settings.MaintenanceEndTime)
                && DateTime.Now.DayOfWeek == blackOutDay)
            {
                return;
            }

            try
            {
                this.monitors.ForEach(m => m.CheckStatusAndNotify());
            }
            catch (Exception ex)
            {
                this.emailClient.Send(
                    "The ICON monitoring service is experiencing unexpected failures and all monitors may not be running." + ex,
                    "ICON Monitoring Failures");
            }
        }

        private LocalDateTime GetLocalDateTimeInCentralTime(Instant instant)
        {
            DateTimeZone centralDateTimeZone = this.dateTimeZoneProvider[TimeZone]; // TimeZone is a private constant
            LocalDateTime localDateTime = instant.InZone(centralDateTimeZone).LocalDateTime;

            return localDateTime;
        }

    }
}
