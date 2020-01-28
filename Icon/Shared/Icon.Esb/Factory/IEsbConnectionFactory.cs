using Icon.Esb.Producer;
using Icon.Esb.Subscriber;

namespace Icon.Esb.Factory
{
    public interface IEsbConnectionFactory
    {
        EsbConnectionSettings Settings { get; set; }

        IEsbSubscriber CreateSubscriber();
        IEsbSubscriber CreateSubscriber(EsbConnectionSettings settings);

        IEsbProducer CreateProducer(string clientId,bool openConnection = true);
        IEsbProducer CreateProducer(EsbConnectionSettings settings);
    }
}
