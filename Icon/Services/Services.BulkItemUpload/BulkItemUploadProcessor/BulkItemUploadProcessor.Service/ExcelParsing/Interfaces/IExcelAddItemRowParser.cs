using System.Collections.Generic;
using BulkItemUploadProcessor.Common;
using Icon.Common.Models;

namespace BulkItemUploadProcessor.Service.ExcelParsing.Interfaces
{
    public interface IExcelAddItemRowParser
    {
        IEnumerable<NewItemViewModel> Parse(List<AttributeModel> attributeModels, List<RowObject> RowData);
    }
}