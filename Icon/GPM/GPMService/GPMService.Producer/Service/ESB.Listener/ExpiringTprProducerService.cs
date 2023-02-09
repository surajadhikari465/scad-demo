using GPMService.Producer.ESB.Listener.JustInTime;
using Icon.Esb.ListenerApplication;
using Icon.Logging;
using Wfm.Aws.ExtendedClient.Listener.SQS;

namespace GPMService.Producer.Service.ESB.Listener
{
    internal class ExpiringTprProducerService : IGPMProducerService
    {
        private readonly SQSExtendedClientListener<ExpiringTprMessageListener> expiringTprMessageListener;
        private readonly ILogger<ExpiringTprProducerService> logger;
        public ExpiringTprProducerService(
            SQSExtendedClientListener<ExpiringTprMessageListener> listenerApplication,
            ILogger<ExpiringTprProducerService> logger
            )
        {
            this.expiringTprMessageListener = listenerApplication;
            this.logger = logger;
        }

        public void Start()
        {
            logger.Info("Starting JustInTime-ExpiringTpr Listener");
            expiringTprMessageListener.Start();
            logger.Info("Started JustInTime-ExpiringTpr Listener");
        }

        public void Stop()
        {
            logger.Info("Stopping JustInTime-ExpiringTpr Listener");
            expiringTprMessageListener.Stop();
            logger.Info("Stopped JustInTime-ExpiringTpr Listener");
        }
    }
}
