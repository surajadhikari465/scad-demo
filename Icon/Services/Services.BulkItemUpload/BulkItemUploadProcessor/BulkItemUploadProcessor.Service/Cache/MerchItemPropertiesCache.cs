using BulkItemUploadProcessor.Common.Models;
using BulkItemUploadProcessor.Common.Utility;
using BulkItemUploadProcessor.Service.Cache.Interfaces;
using Icon.Common.DataAccess;
using System.Collections.Generic;
using System.Linq;

namespace BulkItemUploadProcessor.Service.Cache
{
    public class MerchItemPropertiesCache : IMerchItemPropertiesCache
    {
        private readonly
            IQueryHandler<EmptyQueryParameters<IEnumerable<MerchPropertiesModel>>, IEnumerable<MerchPropertiesModel>>
            getItemPropertiesForAllMerchHierarchiesQueryHandler;

        public Dictionary<int, MerchPropertiesModel> Properties { get; set; }

        public MerchItemPropertiesCache(
            IQueryHandler<EmptyQueryParameters<IEnumerable<MerchPropertiesModel>>, IEnumerable<MerchPropertiesModel>> 
                getItemPropertiesForAllMerchHierarchiesQueryHandler)
        {
            this.getItemPropertiesForAllMerchHierarchiesQueryHandler = getItemPropertiesForAllMerchHierarchiesQueryHandler;
        }

        public void Refresh()
        {
            Properties = getItemPropertiesForAllMerchHierarchiesQueryHandler
                .Search(new EmptyQueryParameters<IEnumerable<MerchPropertiesModel>>())
                .ToDictionary(
                    d => d.MerchandiseHierarchyClassId,
                    d => new MerchPropertiesModel
                        {
                            FinancialHierarcyClassId = d.FinancialHierarcyClassId,
                            NonMerchandiseTraitValue = d.NonMerchandiseTraitValue,
                            ProhibitDiscount = d.ProhibitDiscount, 
                            MerchandiseHierarchyClassId = d.MerchandiseHierarchyClassId, 
                            ItemTypeCode = MerchToItemTypeCodeMapper.GetItemTypeCode(d.NonMerchandiseTraitValue)
                        });
        }
    }
}