namespace Icon.Monitoring.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Timers;

    using Icon.Common.Email;
    using Icon.Monitoring.Common.Settings;
    using Icon.Monitoring.Monitors;
    
    public class MonitorService : IMonitorService
    {
        private Timer monitorServiceTimer;
        private IMonitorSettings settings;
        private readonly IEmailClient emailClient;
        private readonly List<IMonitor> monitors;

        public MonitorService(
            IEnumerable<IMonitor> monitors,
            IMonitorSettings settings,
            IEmailClient emailClient)
        {
            this.monitors = monitors.ToList();
            this.settings = settings;
            this.emailClient = emailClient;
            this.monitorServiceTimer = new Timer(settings.MonitorServiceTimer);
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

            try
            {
                this.monitors.ForEach(m => m.CheckStatusAndNotify());
            }
            catch(Exception ex)
            {
                this.emailClient.Send(
                    "The ICON monitoring service is experiencing unexpected failures and all monitors may not be running." + ex,
                    "ICON Monitoring Failures");
            }
        }
    }
}
