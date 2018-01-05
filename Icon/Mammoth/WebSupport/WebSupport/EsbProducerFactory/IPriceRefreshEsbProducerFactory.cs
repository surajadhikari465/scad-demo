using Icon.Esb.Producer;

namespace WebSupport.EsbProducerFactory
{
    public interface IPriceRefreshEsbProducerFactory
    {
        IEsbProducer CreateEsbProducer(string system, string region);
    }
}
