using Icon.Logging;
using Icon.Monitoring.Common.Settings;
using System;

namespace Icon.Monitoring.Monitors
{
    public abstract class TimedControllerMonitor : IMonitor
    {
        private static TimeSpan DefaultTimerInterval = TimeSpan.FromMilliseconds(60000);

        protected IMonitorSettings settings;
        protected ILogger logger;
        protected DateTime? lastTimeExecuted;

        public virtual void CheckStatusAndNotify()
        {
            TimeSpan configuredInterval = TimeSpan.FromMilliseconds(0);
            var timerValue =
                settings.MonitorTimers != null
                    &&
                settings.MonitorTimers.TryGetValue(this.GetType().Name + "Timer", out configuredInterval)
                    ? configuredInterval
                    : DefaultTimerInterval;

            var shouldRun = this.lastTimeExecuted == null || (DateTime.UtcNow - this.lastTimeExecuted) > configuredInterval;
            if (!shouldRun) return;

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
    }
}
