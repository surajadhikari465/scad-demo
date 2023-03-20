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
            long serviceRunIntervalInMilliseconds = jobSchedulerServiceSettings.ServiceRunIntervalInMinutes * 60 * 1000;
            this.timer = new System.Timers.Timer(serviceRunIntervalInMilliseconds);
        }

        public void Start()
        {
            logger.Info($"Starting JobScheduler service with InstanceId: {jobSchedulerServiceSettings.InstanceId}.");
            timer.Elapsed += RunService;
            timer.Start();
            logger.Info($"Started JobScheduler service with InstanceId: {jobSchedulerServiceSettings.InstanceId}.");
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
            logger.Info($"Stopping JobScheduler service with InstanceId: {jobSchedulerServiceSettings.InstanceId}.");
            while (isServiceRunning)
            {
                logger.Info($"Waiting for JobScheduler service run with InstanceId: {jobSchedulerServiceSettings.InstanceId} to complete before stopping.");
                Thread.Sleep(15000);
            }
            timer.Stop();
            timer.Elapsed -= RunService;
            logger.Info($"Stopped JobScheduler service with InstanceId: {jobSchedulerServiceSettings.InstanceId}.");
        }
    }
}
