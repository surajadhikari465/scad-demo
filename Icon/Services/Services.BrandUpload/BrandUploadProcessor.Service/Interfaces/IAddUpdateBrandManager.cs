using System.Collections.Generic;
using BrandUploadProcessor.Common;
using BrandUploadProcessor.Common.Models;

namespace BrandUploadProcessor.Service.Interfaces
{
    public interface IAddUpdateBrandManager
    {
        void CreateBrands(List<AddBrandModel> addBrandModels,
            List<ErrorItem<AddBrandModel>> invalidBrands,
            List<int> addedBrandIds);

        void UpdateBrands(List<UpdateBrandModel> updateBrandModels,
            List<ErrorItem<UpdateBrandModel>> invalidBrands,
            List<int> updatedBrandIds);

    }
}