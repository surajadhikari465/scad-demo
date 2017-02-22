using Icon.Monitoring.Common.PagerDuty;
using Icon.Monitoring.Common.Settings;
using Icon.Common.DataAccess;
using Icon.Monitoring.DataAccess.Queries;
using System.Collections.Generic;
using System;
using Icon.Logging;

namespace Icon.Monitoring.Monitors
{
    public class TLogConJobMonitor : TimedControllerMonitor
    {
        private const string TLogConServiceNotRunningMessage = "TLog Controller Service stopped running.";
        private const string ItemMovementTableRowCountExceededMessage = "Item Movement table has more than ";

        private ITLogConJobMonitorSettings tLogConMonitorSettings;
        private readonly IPagerDutyTrigger pagerDutyTrigger;
        private IQueryHandler<GetTConLogServiceLastLogDateParameters, string> getTLogConJobMonitorStatusQueryHandler;
        private IQueryHandler<GetItemMovementTableRowCountParameters, string> getItemMovementTableRowCountQueryHandler;
        private Boolean _alertSendForItemTableMovement = false;
        private int currentRowCount = 0;
        private DateTime lastLogDateTime;
     
        public TLogConJobMonitor
            (
             IMonitorSettings settings,
            ITLogConJobMonitorSettings TLogConMonitorSettings,
             IQueryHandler<GetTConLogServiceLastLogDateParameters, string> getTLogConJobMonitorStatusQueryHandler,
                  IQueryHandler<GetItemMovementTableRowCountParameters, string> getItemMovementTableRowCountQueryHandler,
             IPagerDutyTrigger pagerDutyTrigger,
              ILogger logger
            )
        {
            this.settings = settings;
            this.tLogConMonitorSettings = TLogConMonitorSettings;
            this.pagerDutyTrigger = pagerDutyTrigger;
            this.getTLogConJobMonitorStatusQueryHandler = getTLogConJobMonitorStatusQueryHandler;
            this.getItemMovementTableRowCountQueryHandler = getItemMovementTableRowCountQueryHandler;
            this.logger = logger;
        }

        public bool AlertSendForItemTableMovement
        {
            get
            {
                return _alertSendForItemTableMovement;
            }
            set
            {
                _alertSendForItemTableMovement = value;
            }
        }

        protected override void TimedCheckStatusAndNotify()
        {
            if (ShouldCheckStatusAndNotify())
            {
                if (!IsTLogConServiceRunning())
                {
                    logger.Info("TLog Controller Monitor has detected that TLog Controller service is not running.");
                    TriggerPagerDutyForTLogConJob(TLogConServiceNotRunningMessage, "Last Log Date Time: ", lastLogDateTime.ToString());

                }
                // AlertSendForItemTableMovement -- to make sure we only send one
                // only do item movement row count check when setting is set to true in app config
                else if (!AlertSendForItemTableMovement && ShouldCheckItemMovementTableRowCount() && !IsItemMovementTableRowCountLessThanConfiguredSetting())
                {
                    logger.Info("TLog Controller Monitor has detected that Item Movement Table is getting processed slowly.");

                    TriggerPagerDutyForTLogConJob(ItemMovementTableRowCountExceededMessage + tLogConMonitorSettings.ItemMovementMaximumRows.ToString() + " Rows.", "Current Row Count: ", currentRowCount.ToString());
                    AlertSendForItemTableMovement = true;
                }
                else
                {
                    AlertSendForItemTableMovement = false;

                }
            }
        }

        private bool ShouldCheckStatusAndNotify()
        {
            return tLogConMonitorSettings.EnableTLogConJobMonitor;
        }

        private bool ShouldCheckItemMovementTableRowCount()
        {
            return tLogConMonitorSettings.EnableItemMovementTableCheck;
        }
        // this method will check last log by tlog controller and compare it with app settings
        private bool IsTLogConServiceRunning()
        {
            string lastLoggedDateTime = getTLogConJobMonitorStatusQueryHandler.Search(new GetTConLogServiceLastLogDateParameters());

            if (!String.IsNullOrEmpty(lastLoggedDateTime))
            {
                lastLogDateTime = Convert.ToDateTime(lastLoggedDateTime);
                TimeSpan span = DateTime.Now - lastLogDateTime;
                if (span.TotalMinutes > tLogConMonitorSettings.MinutesAllowedSinceLastTconLog)
                    return false;
                else
                    return true;
            }
            else
            {
                return false;
            }

        }
        // thie method will check no of rows in item movement and compare to app settings count
        private bool IsItemMovementTableRowCountLessThanConfiguredSetting()
        {
            currentRowCount = Convert.ToInt32(getItemMovementTableRowCountQueryHandler.Search(new GetItemMovementTableRowCountParameters()));
            if (currentRowCount > tLogConMonitorSettings.ItemMovementMaximumRows)
                return false;
            else
                return true;
        }

        private void TriggerPagerDutyForTLogConJob(string errorMessage, string key, String value)
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