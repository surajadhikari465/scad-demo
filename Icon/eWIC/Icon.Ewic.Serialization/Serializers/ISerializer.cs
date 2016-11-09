
namespace Icon.Ewic.Serialization.Serializers
{
    public interface ISerializer<T>
    {
        string Serialize(T messageModel);
    }
}
