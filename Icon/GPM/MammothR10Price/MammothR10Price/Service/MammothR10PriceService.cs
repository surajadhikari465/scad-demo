using Icon.Esb.ListenerApplication;
using Icon.Logging;

namespace MammothR10Price.Service
{
    public class MammothR10PriceService: IProducerService
    {
        private IListenerApplication mammothR10PriceListener;
        private ILogger<MammothR10PriceService> logger;

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
