using System.IO;

namespace MammothR10Price.Serializer
{
    public interface ISerializer<T>
    {
        string Serialize(T canonicalObject, TextWriter writer);
        T Deserialize(TextReader reader);
    }
}
