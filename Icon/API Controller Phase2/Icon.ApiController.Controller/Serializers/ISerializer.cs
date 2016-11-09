using System.IO;

namespace Icon.ApiController.Controller.Serializers
{
    public interface ISerializer<T>
    {
        string Serialize(T miniBulk, TextWriter writer);
    }
}
