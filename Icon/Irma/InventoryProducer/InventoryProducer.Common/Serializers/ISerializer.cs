using System.IO;

namespace InventoryProducer.Common.Serializers
{
    public interface ISerializer<T>
    {
        string Serialize(T canonicalObject, TextWriter writer);
    }
}
