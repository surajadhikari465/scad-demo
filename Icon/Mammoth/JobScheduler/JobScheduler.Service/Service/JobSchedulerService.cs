using Icon.Logging;
using JobScheduler.Service.Processor;
using JobScheduler.Service.Settings;
using System.Threading;
using System.Timers;

namespace JobScheduler.Service.Service
{
    internal class JobSchedulerService : IJobSchedulerService
    {
        private readonly IJobSchedulerProcessor jobSchedulerProcessor;
        private readonly JobSchedulerServiceSettings jobSchedulerServiceSettings;
        private readonly ILogger<JobSchedulerService> logger;
        private readonly System.Timers.Timer timer = null;
        private bool isServiceRunning = false;

        public JobSchedulerService(IJobSchedulerProcessor messageProcessor, JobSchedulerServiceSettings jobSchedulerServiceSettings, ILogger<JobSchedulerService> logger)
        {
            this.jobSchedulerProcessor = messageProcessor;
            this.jobSchedulerServiceSettings = jobSchedulerServiceSettings;
            this.logger = logger;
            this.timer = new System.Timers.Timer(jobSchedulerServiceSettings.TimerProcessRunIntervalInMilliseconds);
        }

        public void Start()
        {
            // Introducing delay so that multiple instances don't process the data at the same time.
            if (jobSchedulerServiceSettings.StartDelayInSeconds < 0)
            {
                logger.Warn($"Invalid value computed for StartDelayInSeconds - '{jobSchedulerServiceSettings.StartDelayInSeconds}'. Setting to 0.");
                jobSchedulerServiceSettings.StartDelayInSeconds = 0;
            }
            logger.Info($"Starting JobScheduler service in {jobSchedulerServiceSettings.StartDelayInSeconds} seconds.");
            Thread.Sleep(jobSchedulerServiceSettings.StartDelayInSeconds * 1000);
            timer.Elapsed += RunService;
            timer.Start();
            logger.Info("Started JobScheduler service.");
        }

        private void RunService(object sender, ElapsedEventArgs e)
        {
            timer.Stop();
            try
            {
                isServiceRunning = true;
                jobSchedulerProcessor.Process();
            }
            finally
            {
                timer.Start();
                isServiceRunning = false;
            }
        }

        public void Stop()
        {
            logger.Info("Stopping JobScheduler service.");
            while (isServiceRunning)
            {
                logger.Info($"Waiting for JobScheduler service run to complete before stopping.");
                Thread.Sleep(15000);
            }
            timer.Stop();
            timer.Elapsed -= RunService;
            logger.Info("Stopped JobScheduler service.");
        }
    }
}
