using Icon.Esb.ListenerApplication;
using Icon.Logging;

namespace MammothR10Price.Service
{
    public class MammothR10PriceService: IProducerService
    {
        private readonly IListenerApplication mammothR10PriceListener;
        private readonly ILogger<MammothR10PriceService> logger;

        public MammothR10PriceService(IListenerApplication mammothR10PriceListener, ILogger<MammothR10PriceService> logger)
        {
            this.mammothR10PriceListener = mammothR10PriceListener;
            this.logger = logger;
        }

        public void Start()
        {
            logger.Info("Starting Mammoth R10 Price Listener");
            mammothR10PriceListener.Run();
            logger.Info("Started Mammoth R10 Price Listener");
        }

        public void Stop()
        {
            logger.Info("Stopping Mammoth R10 Price Listener");
            mammothR10PriceListener.Close();
            logger.Info("Stopped Mammoth R10 Price Listener");
        }
    }
}
