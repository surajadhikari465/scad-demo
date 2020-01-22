using Icon.Esb.Schemas.Wfm.Contracts;

namespace Icon.Services.ItemPublisher.Infrastructure.Models.Mappers
{
    public interface IUomMapper
    {
        WfmUomDescEnumType GetEsbUomDescription(string uomCode);
        WfmUomCodeEnumType GetEsbUomCode(string uomCode);
    }
}
