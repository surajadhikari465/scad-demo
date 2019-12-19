using BulkItemUploadProcessor.Common;
using BulkItemUploadProcessor.Common.Models;
using Icon.Common.Models;
using System;
using System.Collections.Generic;

namespace BulkItemUploadProcessor.Service.Mappers.Interfaces
{
    public interface IRowObjectToAddItemModelMapper
    {
        RowObjectToItemMapperResponse<AddItemModel> Map(List<RowObject> rowObjects, List<ColumnHeader> columnHeaders, List<AttributeModel> attributeModels, string uploadedBy);
    }
}