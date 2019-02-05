using Icon.Logging;
using Icon.Monitoring.Common.Opsgenie;
using Icon.Monitoring.Common.Settings;
using Newtonsoft.Json;
using OpsgenieAlert;
using System;

namespace Icon.Monitoring.Monitors
{
    public abstract class TimedControllerMonitor : IMonitor
    {
        private static TimeSpan DefaultTimerInterval = TimeSpan.FromMilliseconds(60000);

        protected IMonitorSettings settings;
        protected ILogger logger;
        protected DateTime? lastTimeExecuted;

        public bool ByPassConfiguredRunInterval { get; set; }

        public virtual void CheckStatusAndNotify()
        {
            TimeSpan configuredInterval = TimeSpan.FromMilliseconds(0);
            var timerValue =
                settings.MonitorTimers != null
                    &&
                settings.MonitorTimers.TryGetValue(this.GetType().Name + "Timer", out configuredInterval)
                    ? configuredInterval
                    : DefaultTimerInterval;

            if (!ByPassConfiguredRunInterval)
            {
                var shouldRun = this.lastTimeExecuted == null || (DateTime.UtcNow - this.lastTimeExecuted) > configuredInterval;
                if (!shouldRun) return;
            }

            try
            {
                this.lastTimeExecuted = DateTime.UtcNow;
                this.TimedCheckStatusAndNotify();
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("{0} failed to execute.", this.GetType().Name));
                Console.WriteLine(ex.Message);
                logger.Error(string.Format("The {0} has thrown an error:", this.GetType().Name + ex.Message));

                throw;
            }
            finally
            {
                Console.WriteLine(string.Format("{0} finished at {1}.", this.GetType().Name, DateTime.UtcNow));
                this.logger.Info(string.Format("{0} finished at {1}.", this.GetType().Name, DateTime.UtcNow));
            }
        }

        protected abstract void TimedCheckStatusAndNotify();

        protected DateTime GetTomorrowsUtcStartDate()
        {
            return DateTime.UtcNow.Date.AddDays(1);
        }

        protected void LogInfo(string message, string region)
        {
            LogInfo(this.logger, message, region);
        }

        protected void LogInfo(string message, string region, string error)
        {
            LogInfo(this.logger, message, region, error);
        }

        protected void LogInfo(string message, string region, OpsgenieResponse opsgenieResponse)
        {
            LogInfo(this.logger, message, region, opsgenieResponse);
        }

        protected void LogError(string message, string region, Exception ex)
        {
            LogError(this.logger, message, region, ex);
        }

        protected void LogInfo(ILogger logger, string message, string region)
        {
            logger.Info(JsonConvert.SerializeObject(new
            {
                Message = message,
                Region = region
            }));
        }

        protected void LogInfo(ILogger logger, string message, string region, string error)
        {
            logger.Info(JsonConvert.SerializeObject(new
            {
                Message = message,
                Region = region,
                Error = error
            }));
        }

        protected void LogInfo(ILogger logger, string message, string region, OpsgenieResponse opsgenieResponse)
        {
            logger.Info(JsonConvert.SerializeObject(new
            {
                Message = message,
                Region = region,
                Response = opsgenieResponse
            }));
        }

        protected void LogError(ILogger logger, string message, string region, Exception ex)
        {
            logger.Error(JsonConvert.SerializeObject(new
            {
                Message = message,
                Region = region,
                Error = ex
            }));
        }
    }
}
