namespace Mammoth.PrimeAffinity.Library.Esb
{
    public class EsbConnectionCacheFactory : IEsbConnectionCacheFactory
    {
        public ICacheEsbProducer CreateProducer()
        {
            return EsbConnectionCache.CreateProducer();
        }
    }
}
