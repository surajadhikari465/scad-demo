using System.IO;

namespace Slaw.ItemLocale.Serializers
{
    public interface ISerializer<T>
    {
        string Serialize(T miniBulk, TextWriter writer);
    }
}
