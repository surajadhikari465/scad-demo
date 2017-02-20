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
        private ITLogConJobMonitorSettings tLogConMonitorSettings;
        private readonly IPagerDutyTrigger pagerDutyTrigger;
        private IQueryHandler<GetTConLogServiceLastLogDateParameters, string> getTLogConJobMonitorStatusQueryHandler;
        private IQueryHandler<GetItemMovementTableRowCountParameters, string> getItemMovementTableRowCountQueryHandler;
        private Boolean _alertSendForItemTableMovement = false;
        private const string TLogConServiceNotRunningMessage = "TLog Controller Service stopped running.";
        private const string ItemMovementTableRowCountExceededMessage = "Item Movement table has more than ";
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

        public Boolean AlertSendForItemTableMovement
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
                if (!CheckTLogConServiceRunning())
                {
                    logger.Info("TLog Controller Monitor has detected that TLog Controller service is not running.");
                    TriggerPagerDutyForTLogConJob(TLogConServiceNotRunningMessage);

                }
                else if (!AlertSendForItemTableMovement && ShouldCheckItemMovementTableRowCount() && !CheckItemMovementTableRowCount())
                {
                    logger.Info("TLog Controller Monitor has detected that Item Movement Table is getting processed slowly.");

                    TriggerPagerDutyForTLogConJob(ItemMovementTableRowCountExceededMessage + tLogConMonitorSettings.ItemMovementMaxRows.ToString() + " Rows.");
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

        private Boolean CheckTLogConServiceRunning()
        {
            TimeSpan span = DateTime.Now - Convert.ToDateTime(getTLogConJobMonitorStatusQueryHandler.Search(new GetTConLogServiceLastLogDateParameters()));

            if (span.TotalMinutes > tLogConMonitorSettings.MaxLastTLogConJobLogTime)
                return false;
            else
                return true;

        }

        private Boolean CheckItemMovementTableRowCount()
        {
            if (Convert.ToInt32(getItemMovementTableRowCountQueryHandler.Search(new GetItemMovementTableRowCountParameters())) > tLogConMonitorSettings.ItemMovementMaxRows)
                return false;
            else
                return true;
        }

        private void TriggerPagerDutyForTLogConJob(string Message)
        {
            logger.Info(Message);

            var response = this.pagerDutyTrigger.TriggerIncident(
                            Message,
                            new Dictionary<string, string>()
                            {
                                { Message, System.DateTime.Now.ToString() }
                            });
        }
    }
}