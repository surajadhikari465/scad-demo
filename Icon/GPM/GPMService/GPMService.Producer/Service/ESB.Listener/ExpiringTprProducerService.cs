using Icon.Esb.ListenerApplication;
using Icon.Logging;

namespace GPMService.Producer.Service.ESB.Listener
{
    internal class ExpiringTprProducerService : IGPMProducerService
    {
        private readonly IListenerApplication expiringTprMessageListener;
        private readonly ILogger<ExpiringTprProducerService> logger;
        public ExpiringTprProducerService(
            IListenerApplication listenerApplication,
            ILogger<ExpiringTprProducerService> logger
            )
        {
            this.expiringTprMessageListener = listenerApplication;
            this.logger = logger;
        }

        public void Start()
        {
            logger.Info("Starting JustInTime-ExpiringTpr Listener");
            expiringTprMessageListener.Run();
            logger.Info("Started JustInTime-ExpiringTpr Listener");
        }

        public void Stop()
        {
            logger.Info("Stopping JustInTime-ExpiringTpr Listener");
            expiringTprMessageListener.Close();
            logger.Info("Stopped JustInTime-ExpiringTpr Listener");
        }
    }
}
