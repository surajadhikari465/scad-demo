using BulkItemUploadProcessor.Common;
using BulkItemUploadProcessor.Common.Models;
using Icon.Common.Models;
using System.Collections.Generic;
using static BulkItemUploadProcessor.Common.Enums;

namespace BulkItemUploadProcessor.Service.Validation.Interfaces
{
    public interface IRowObjectsValidator
    {
        RowObjectValidatorResponse Validate(
            FileModeTypeEnum fileModeType, 
            List<RowObject> rowObjects, 
            List<ColumnHeader> columnHeaders, 
            List<AttributeModel> attributeModels);
    }
}
