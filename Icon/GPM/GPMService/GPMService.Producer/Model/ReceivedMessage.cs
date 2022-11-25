using Icon.Esb.Subscriber;

namespace GPMService.Producer.Model
{
    internal class ReceivedMessage
    {
        public IEsbMessage esbMessage { get; set; }
    }
}
