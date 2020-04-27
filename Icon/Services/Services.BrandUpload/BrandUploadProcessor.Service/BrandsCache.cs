using System.Collections.Generic;
using BrandUploadProcessor.Common.Models;
using BrandUploadProcessor.Service.Interfaces;
using Icon.Common.DataAccess;

namespace BrandUploadProcessor.Service
{
    public class BrandsCache : IBrandsCache
    {
        private readonly IQueryHandler<EmptyQueryParameters<List<BrandModel>>, List<BrandModel>> getBrandsQueryHandler;

        public BrandsCache(IQueryHandler<EmptyQueryParameters<List<BrandModel>>, List<BrandModel>> getBrandsQueryHandler)
        {
            this.getBrandsQueryHandler = getBrandsQueryHandler;
        }

        public List<BrandModel> Brands { get; set; }

        public void Refresh()
        {
            this.Brands = getBrandsQueryHandler.Search(new EmptyQueryParameters<List<BrandModel>>());
        }
    }
}