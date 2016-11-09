using Icon.Esb.Schemas.Wfm.Contracts;

namespace Esb.Core.Mappers
{
    public interface IUomMapper
    {
        WfmUomDescEnumType GetEsbUomDescription(string uomCode);
        WfmUomCodeEnumType GetEsbUomCode(string uomCode);
    }
}
