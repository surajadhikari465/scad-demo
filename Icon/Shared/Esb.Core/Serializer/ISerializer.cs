using System.IO;

namespace Esb.Core.Serializer
{
    public interface ISerializer<T>
    {
        string Serialize(T t);
        string Serialize(T t, TextWriter writer);
    }
}
