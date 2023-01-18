using System.IO;

namespace IrmaPriceListenerService.Serializer
{
    public interface ISerializer<T>
    {
        string Serialize(T canonicalObject, TextWriter writer);
    }
}
