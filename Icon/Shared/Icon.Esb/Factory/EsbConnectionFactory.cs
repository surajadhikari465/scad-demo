using Icon.Esb.Producer;
using Icon.Esb.Subscriber;

namespace Icon.Esb.Factory
{
    public class EsbConnectionFactory : IEsbConnectionFactory
    {
        public EsbConnectionSettings Settings { get; set; }

        public IEsbSubscriber CreateSubscriber()
        {
            return new EsbSubscriber(Settings);
        }

        public IEsbSubscriber CreateSubscriber(EsbConnectionSettings settings)
        {
            return new EsbSubscriber(settings);
        }

        public IEsbProducer CreateProducer(string clientId, bool openConnection = true)
        {
            var producer = new EsbProducer(Settings);
            if(openConnection)
            {
                producer.OpenConnection(clientId);
            }
            return producer;
        }

        public IEsbProducer CreateProducer(EsbConnectionSettings settings)
        {
            return new EsbProducer(settings);
        }
    }
}
