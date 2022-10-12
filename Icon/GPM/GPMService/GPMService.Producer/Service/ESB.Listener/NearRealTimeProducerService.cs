using GPMService.Producer.ESB.Listener.NearRealTime;
using Icon.Esb.ListenerApplication;

namespace GPMService.Producer.Service.ESB.Listener
{
    internal class NearRealTimeProducerService : IGPMProducerService
    {
        private readonly IListenerApplication nearRealTimeMessageListener;
        public NearRealTimeProducerService(IListenerApplication listenerApplication)
        {
            nearRealTimeMessageListener = listenerApplication;
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
