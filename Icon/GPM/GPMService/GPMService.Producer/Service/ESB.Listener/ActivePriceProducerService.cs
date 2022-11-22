using Icon.Esb.ListenerApplication;
using Icon.Logging;

namespace GPMService.Producer.Service.ESB.Listener
{
    internal class ActivePriceProducerService : IGPMProducerService
    {
        private readonly IListenerApplication activePriceMessageListener;
        private readonly ILogger<ActivePriceProducerService> logger;
        public ActivePriceProducerService(
            IListenerApplication listenerApplication,
            ILogger<ActivePriceProducerService> logger
            )
        {
            this.activePriceMessageListener = listenerApplication;
            this.logger = logger;
        }

        public void Start()
        {
            logger.Info("Starting JustInTime-ActivePrice Listener");
            activePriceMessageListener.Run();
            logger.Info("Started JustInTime-ActivePrice Listener");
        }

        public void Stop()
        {
            logger.Info("Stopping JustInTime-ActivePrice Listener");
            activePriceMessageListener.Close();
            logger.Info("Stopped JustInTime-ActivePrice Listener");
        }
    }
}
