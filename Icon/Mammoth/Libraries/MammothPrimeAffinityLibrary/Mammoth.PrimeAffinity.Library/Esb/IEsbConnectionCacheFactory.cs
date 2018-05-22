namespace Mammoth.PrimeAffinity.Library.Esb
{
    public interface IEsbConnectionCacheFactory
    {
        ICacheEsbProducer CreateProducer();
    }
}
