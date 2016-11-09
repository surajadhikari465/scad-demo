using System.IO;

namespace Esb.Core.Serializer
{
    public interface ISerializer<T>
    {
        string Serialize(T miniBulk, TextWriter writer);
    }
}
