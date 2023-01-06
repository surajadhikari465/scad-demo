using Icon.Logging;
using System.Threading;
using System.Timers;
using ErrorMessagesMonitor.Message.Processor;
using ErrorMessagesMonitor.Settings;

namespace ErrorMessagesMonitor.Service
{
    internal class ErrorMessagesMonitorService : IErrorMessagesMonitorService
    {
        private readonly IErrorMessagesProcessor errorMessageProcessor;
        private readonly ErrorMessagesMonitorServiceSettings errorMessagesMonitorServiceSettings;
        private readonly ILogger<ErrorMessagesMonitorService> logger;
        private readonly System.Timers.Timer timer = null;
        private bool isServiceRunning = false;

        public ErrorMessagesMonitorService(IErrorMessagesProcessor errorMessageProcessor, ErrorMessagesMonitorServiceSettings errorMessagesMonitorServiceSettings, ILogger<ErrorMessagesMonitorService> logger)
        {
            this.errorMessageProcessor = errorMessageProcessor;
            this.errorMessagesMonitorServiceSettings = errorMessagesMonitorServiceSettings;
            this.logger = logger;
            this.timer = new System.Timers.Timer(errorMessagesMonitorServiceSettings.TimerProcessRunIntervalInMilliseconds);
        }

        public void Start()
        {
            timer.Elapsed += RunService;
            timer.Start();
        }

        private void RunService(object sender, ElapsedEventArgs e)
        {
            timer.Stop();
            try
            {
                isServiceRunning = true;
                errorMessageProcessor.Process();
            }
            finally
            {
                timer.Start();
                isServiceRunning = false;
            }
        }

        public void Stop()
        {
            while (isServiceRunning)
            {
                logger.Info($"Waiting for service run to complete before stopping.");
                Thread.Sleep(15000);
            }
            timer.Stop();
            timer.Elapsed -= RunService;
        }
    }
}
