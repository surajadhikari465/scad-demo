using Icon.Monitoring.Common.PagerDuty;
using Icon.Monitoring.Common.Settings;
using Icon.Common.DataAccess;
using Icon.Monitoring.DataAccess.Queries;
using System.Collections.Generic;
using System;
using Icon.Logging;
using Icon.Monitoring.DataAccess.Model;

namespace Icon.Monitoring.Monitors
{
    public class VIMLocaleControllerMonitor : TimedControllerMonitor
    {
        private const string VimLocaleConServiceNotRunningMessage = "VIM Locale Controller Service stopped running.";
        private const string AppName = "Vim Locale Controller";
        private IVimLocaleConJobMonitorSettings vimLocaleConMonitorSettings;
        private readonly IPagerDutyTrigger pagerDutyTrigger;
        private IQueryHandler<GetLatestAppLogByAppNameParameters, AppLog> getLatestAppLogByAppNameQueryHandler;
        private DateTime lastLogDateTime;

        public VIMLocaleControllerMonitor
            (
             IMonitorSettings settings,
             IVimLocaleConJobMonitorSettings vimLocaleConMonitorSettings,
             IQueryHandler<GetLatestAppLogByAppNameParameters, AppLog> getLatestAppLogByAppNameQueryHandler,
             IPagerDutyTrigger pagerDutyTrigger,
              ILogger logger
            )
        {
            this.settings = settings;
            this.vimLocaleConMonitorSettings = vimLocaleConMonitorSettings;
            this.pagerDutyTrigger = pagerDutyTrigger;
            this.getLatestAppLogByAppNameQueryHandler = getLatestAppLogByAppNameQueryHandler;
            this.logger = logger;
        }

        protected override void TimedCheckStatusAndNotify()
        {
            if (ShouldCheckStatusAndNotify())
            {
                if (!HasVimLocaleConLoggedSinceConfiguredAllowedMinutes(AppName))
                {
                    logger.Info("VIM Locale Controller Monitor has detected that VIM Locale Controller service is not running.");
                    TriggerPagerDutyForVimLocaleConJob(VimLocaleConServiceNotRunningMessage, "Last Log Date Time: ", lastLogDateTime.ToString());

                }
            }
        }

        private bool ShouldCheckStatusAndNotify()
        {
            return vimLocaleConMonitorSettings.EnableVimLocaleConJobMonitor;
        }

        private bool HasVimLocaleConLoggedSinceConfiguredAllowedMinutes(string appName)
        {
            AppLog appLog = getLatestAppLogByAppNameQueryHandler.Search(new GetLatestAppLogByAppNameParameters(appName));

            if (appLog != null)
            {
                lastLogDateTime = appLog.LogDate;
                TimeSpan span = DateTime.Now - lastLogDateTime;
                if (span.TotalMinutes > vimLocaleConMonitorSettings.MinutesAllowedSinceLastVimLocaleCon)
                    return false;
                else
                    return true;
            }
            else
            {
                return false;
            }
        }

        private void TriggerPagerDutyForVimLocaleConJob(string errorMessage, string key, String value)
        {
            logger.Info(errorMessage);

            var response = this.pagerDutyTrigger.TriggerIncident(
                            errorMessage,
                            new Dictionary<string, string>()
                            {
                                { key, value }
                            });
        }
    }
}