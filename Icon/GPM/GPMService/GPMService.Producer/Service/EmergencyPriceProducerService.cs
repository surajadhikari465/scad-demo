using GPMService.Producer.Message.Processor;
using GPMService.Producer.Settings;
using Icon.Logging;
using System.Threading;
using System.Timers;

namespace GPMService.Producer.Service
{
    internal class EmergencyPriceProducerService : IGPMProducerService
    {
        private readonly IMessageProcessor messageProcessor;
        private readonly GPMProducerServiceSettings gpmProducerServiceSettings;
        private readonly ILogger<EmergencyPriceProducerService> logger;
        private readonly System.Timers.Timer timer = null;
        private bool isServiceRunning = false;

        public EmergencyPriceProducerService (IMessageProcessor messageProcessor, GPMProducerServiceSettings gpmProducerServiceSettings, ILogger<EmergencyPriceProducerService> logger)
        {
            this.messageProcessor = messageProcessor;
            this.gpmProducerServiceSettings = gpmProducerServiceSettings;
            this.logger = logger;
            this.timer = new System.Timers.Timer(gpmProducerServiceSettings.TimerProcessRunIntervalInMilliseconds);
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
                messageProcessor.Process();
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
