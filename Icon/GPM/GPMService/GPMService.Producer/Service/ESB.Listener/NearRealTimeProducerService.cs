using GPMService.Producer.ESB.Listener.NearRealTime;

namespace GPMService.Producer.Service.ESB.Listener
{
    internal class NearRealTimeProducerService : IGPMProducerService
    {
        private readonly NearRealTimeMessageListener nearRealTimeMessageListener;
        public NearRealTimeProducerService()
        {
            nearRealTimeMessageListener = NearRealTimeMessageListenerBuilder.Build();
        }

        public void Start()
        {
            if (nearRealTimeMessageListener != null)
            {
                nearRealTimeMessageListener.Run();
            }
        }

        public void Stop()
        {
            if (nearRealTimeMessageListener != null)
            {
                nearRealTimeMessageListener.Close();
            }
        }
    }
}
