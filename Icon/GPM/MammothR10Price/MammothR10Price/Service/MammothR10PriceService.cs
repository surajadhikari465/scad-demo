using Icon.Logging;
using Wfm.Aws.ExtendedClient.Listener.SQS;

namespace MammothR10Price.Service
{
    public class MammothR10PriceService: IProducerService
    {
        private readonly ISQSExtendedClientListener mammothR10PriceListener;
        private readonly ILogger<MammothR10PriceService> logger;

        public MammothR10PriceService(ISQSExtendedClientListener mammothR10PriceListener, ILogger<MammothR10PriceService> logger)
        {
            this.mammothR10PriceListener = mammothR10PriceListener;
            this.logger = logger;
        }

        public void Start()
        {
            logger.Info("Starting Mammoth R10 Price Listener");
            mammothR10PriceListener.Start();
            logger.Info("Started Mammoth R10 Price Listener");
        }

        public void Stop()
        {
            logger.Info("Stopping Mammoth R10 Price Listener");
            mammothR10PriceListener.Stop();
            logger.Info("Stopped Mammoth R10 Price Listener");
        }
    }
}
