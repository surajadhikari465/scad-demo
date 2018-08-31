using Icon.Esb.Schemas.Wfm.PreGpm.Contracts;
using Icon.Framework;

namespace Icon.ApiController.Controller.Mappers
{
    public interface IUomMapper
    {
        WfmUomDescEnumType GetEsbUomDescription(string uomCode);
        WfmUomCodeEnumType GetEsbUomCode(string uomCode);
    }
}
