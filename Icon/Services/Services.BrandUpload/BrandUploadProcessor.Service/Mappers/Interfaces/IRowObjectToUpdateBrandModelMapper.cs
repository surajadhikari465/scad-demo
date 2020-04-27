using System.Collections.Generic;
using BrandUploadProcessor.Common.Models;

namespace BrandUploadProcessor.Service.Mappers.Interfaces
{
    public interface IRowObjectToUpdateBrandModelMapper
    {
        RowObjectToBrandMapperResponse<UpdateBrandModel> Map(List<RowObject> rowObjects, List<ColumnHeader> columnHeaders, List<BrandAttributeModel> brandAttributeModels, string uploadedBy);
    }
}