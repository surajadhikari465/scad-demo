using System.Collections.Generic;
using BrandUploadProcessor.Common;
using BrandUploadProcessor.Common.Models;

namespace BrandUploadProcessor.Service.Validation.Interfaces
{
    public interface IRowObjectsValidator
    {
        RowObjectValidatorResponse Validate(
            Enums.FileModeTypeEnum fileModeType,
            List<RowObject> rowObjects,
            List<ColumnHeader> columnHeaders,
            List<BrandAttributeModel> brandAttributeModels);

        RowObjectValidatorResponse ValidateCreateNew(List<RowObject> rowObjects, List<ColumnHeader> columnHeaders,
            List<BrandAttributeModel> brandAttributeModels);
    }
}