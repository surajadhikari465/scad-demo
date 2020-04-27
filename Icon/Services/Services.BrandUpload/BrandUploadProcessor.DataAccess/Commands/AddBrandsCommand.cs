using System.Collections.Generic;
using BrandUploadProcessor.Common;
using BrandUploadProcessor.Common.Models;

namespace BrandUploadProcessor.DataAccess.Commands
{
    public class AddBrandsCommand
    {
        public AddBrandsCommand()
        {
            Brands = new List<AddBrandModel>();
            AddedBrandIds = new List<int>();
            InvalidBrands = new List<ErrorItem<AddBrandModel>>();
        }

        public List<AddBrandModel> Brands { get; set; }
        public List<int> AddedBrandIds { get; set; }
        public List<ErrorItem<AddBrandModel>> InvalidBrands { get; set; }

    }
}