using GPMService.Producer.Listener.JustInTime;
using Icon.Logging;
using Wfm.Aws.ExtendedClient.Listener.SQS;

namespace GPMService.Producer.Service.Listener
{
    internal class ActivePriceProducerService : IGPMProducerService
    {
        private readonly SQSExtendedClientListener<ActivePriceMessageListener> activePriceMessageListener;
        private readonly ILogger<ActivePriceProducerService> logger;
        public ActivePriceProducerService(
            SQSExtendedClientListener<ActivePriceMessageListener> listenerApplication,
            ILogger<ActivePriceProducerService> logger
            )
        {
            this.activePriceMessageListener = listenerApplication;
            this.logger = logger;
        }

        public void Start()
        {
            logger.Info("Starting JustInTime-ActivePrice Listener");
            activePriceMessageListener.Start();
            logger.Info("Started JustInTime-ActivePrice Listener");
        }

        public void Stop()
        {
            logger.Info("Stopping JustInTime-ActivePrice Listener");
            activePriceMessageListener.Stop();
            logger.Info("Stopped JustInTime-ActivePrice Listener");
        }
    }
}
