using Icon.Esb.ListenerApplication;
using Icon.Logging;

namespace GPMService.Producer.Service.ESB.Listener
{
    internal class NearRealTimeProducerService : IGPMProducerService
    {
        private readonly IListenerApplication nearRealTimeMessageListener;
        private readonly ILogger<NearRealTimeProducerService> logger;
        public NearRealTimeProducerService(
            IListenerApplication listenerApplication,
            ILogger<NearRealTimeProducerService> logger
            )
        {
            this.nearRealTimeMessageListener = listenerApplication;
            this.logger = logger;
        }

        public void Start()
        {
            logger.Info("Starting NearRealTime Listener");
            nearRealTimeMessageListener.Run();
            logger.Info("Started NearRealTime Listener");
        }

        public void Stop()
        {
            logger.Info("Stopping NearRealTime Listener");
            nearRealTimeMessageListener.Close();
            logger.Info("Stopped NearRealTime Listener");
        }
    }
}
