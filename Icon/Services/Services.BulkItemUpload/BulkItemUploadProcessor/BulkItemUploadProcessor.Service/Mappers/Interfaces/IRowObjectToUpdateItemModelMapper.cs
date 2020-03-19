using System.Collections.Generic;
using BulkItemUploadProcessor.Common;
using BulkItemUploadProcessor.Common.Models;
using Icon.Common.Models;

namespace BulkItemUploadProcessor.Service.Mappers.Interfaces
{
    public interface IRowObjectToUpdateItemModelMapper
    {
        RowObjectToItemMapperResponse<UpdateItemModel> Map(List<RowObject> rowObjects, List<ColumnHeader> columnHeaders, List<AttributeModel> attributeModels, string uploadedBy);
    }
}