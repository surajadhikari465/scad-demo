using GPMService.Producer.ESB.Listener.NearRealTime;
using Icon.Esb.ListenerApplication;
using Icon.Logging;
using Wfm.Aws.ExtendedClient.Listener.SQS;

namespace GPMService.Producer.Service.ESB.Listener
{
    internal class NearRealTimeProducerService : IGPMProducerService
    {
        private readonly SQSExtendedClientListener<NearRealTimeMessageListener> nearRealTimeMessageListener;
        private readonly ILogger<NearRealTimeProducerService> logger;
        public NearRealTimeProducerService(
            SQSExtendedClientListener<NearRealTimeMessageListener> listenerApplication,
            ILogger<NearRealTimeProducerService> logger
            )
        {
            this.nearRealTimeMessageListener = listenerApplication;
            this.logger = logger;
        }

        public void Start()
        {
            logger.Info("Starting NearRealTime Listener");
            nearRealTimeMessageListener.Start();
            logger.Info("Started NearRealTime Listener");
        }

        public void Stop()
        {
            logger.Info("Stopping NearRealTime Listener");
            nearRealTimeMessageListener.Stop();
            logger.Info("Stopped NearRealTime Listener");
        }
    }
}
