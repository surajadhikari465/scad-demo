
namespace MammothR10Price.Mapper
{
    public interface IMapper<FromType, ToType>
    {
        ToType Transform(FromType fromType);
        string ToXml(ToType toType);
    }
}
