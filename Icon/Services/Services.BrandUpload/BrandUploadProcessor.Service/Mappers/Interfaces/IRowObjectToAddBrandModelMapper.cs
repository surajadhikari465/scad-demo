using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrandUploadProcessor.Common.Models;

namespace BrandUploadProcessor.Service.Mappers.Interfaces
{
    public interface IRowObjectToAddBrandModelMapper
    {
        RowObjectToBrandMapperResponse<AddBrandModel> Map(List<RowObject> rowObjects, List<ColumnHeader> columnHeaders, List<BrandAttributeModel> brandAttributeModels, string uploadedBy);
    }
}
