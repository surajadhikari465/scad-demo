using System.IO;

namespace JobScheduler.Service.Serializer
{
    internal interface ISerializer<T>
    {
        string Serialize(T canonicalObject, TextWriter writer);
        T Deserialize(TextReader reader);
    }
}
