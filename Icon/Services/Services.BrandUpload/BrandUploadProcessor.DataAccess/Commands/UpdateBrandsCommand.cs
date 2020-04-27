using System.Collections.Generic;
using BrandUploadProcessor.Common;
using BrandUploadProcessor.Common.Models;

namespace BrandUploadProcessor.DataAccess.Commands
{
    public class UpdateBrandsCommand
    {
        public UpdateBrandsCommand()
        {
            Brands = new List<UpdateBrandModel>();
            UpdatedBrandIds = new List<int>();
            InvalidBrands = new List<ErrorItem<UpdateBrandModel>>();
        }

        public List<UpdateBrandModel> Brands { get; set; }
        public List<int> UpdatedBrandIds { get; set; }
        public List<ErrorItem<UpdateBrandModel>> InvalidBrands { get; set; }

    }
}