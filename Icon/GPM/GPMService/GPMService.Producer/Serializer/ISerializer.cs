using System.IO;

namespace GPMService.Producer.Serializer
{
    public interface ISerializer<T>
    {
        string Serialize(T canonicalObject, TextWriter writer);
    }
}
