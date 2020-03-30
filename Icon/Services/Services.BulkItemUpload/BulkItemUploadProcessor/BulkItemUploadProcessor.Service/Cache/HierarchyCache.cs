using BulkItemUploadProcessor.DataAccess.Queries;
using BulkItemUploadProcessor.Service.Cache.Interfaces;
using Icon.Common.DataAccess;
using Icon.Framework;
using System.Collections.Generic;

namespace BulkItemUploadProcessor.Service.Cache
{
    public class HierarchyCache : IHierarchyCache
    {
        public HashSet<int> NationalHierarchy { get; set; }
        public HashSet<int> MerchandiseHierarchy { get; set; }
        public HashSet<int> BrandHierarchy { get; set; }
        public HashSet<int> ManufacturerHierarchy { get; set; }
        public HashSet<int> TaxHierarchy { get; set; }

        private readonly IQueryHandler<GetHierarchyIdsParameters, List<int>> getHierarchyIdsQueryHandler;


        public HierarchyCache(IQueryHandler<GetHierarchyIdsParameters, List<int>> getHierarchyIdsQueryHandler)
        {
            NationalHierarchy = new HashSet<int>();
            MerchandiseHierarchy = new HashSet<int>();
            BrandHierarchy = new HashSet<int>();
            ManufacturerHierarchy = new HashSet<int>();
            TaxHierarchy = new HashSet<int>();

            this.getHierarchyIdsQueryHandler = getHierarchyIdsQueryHandler;
        }

        public void Clear()
        {
            NationalHierarchy.Clear();
            MerchandiseHierarchy.Clear();
            BrandHierarchy.Clear();
            TaxHierarchy.Clear();
            ManufacturerHierarchy.Clear();
        }

        public void Refresh()
        {
            Clear();
            LoadAll();
        }
        public void LoadAll()
        {
            NationalHierarchy.UnionWith(LoadHierarchyIds(HierarchyNames.National, HierarchyLevels.NationalClass));
            MerchandiseHierarchy.UnionWith(LoadHierarchyIds(HierarchyNames.Merchandise, HierarchyLevels.SubBrick));
            BrandHierarchy.UnionWith(LoadHierarchyIds(HierarchyNames.Brands, HierarchyLevels.Brand));
            ManufacturerHierarchy.UnionWith(LoadHierarchyIds("Manufacturer", HierarchyLevels.Manufacturer));
            TaxHierarchy.UnionWith(LoadHierarchyIds(HierarchyNames.Tax, HierarchyLevels.Tax));
        }

        public bool IsValidNationalHierarchyClassId(int hierarchyClassId)
        {
            return NationalHierarchy.Contains(hierarchyClassId);
        }

        public bool IsValidMerchandiseHierarchyClassId(int hierarchyClassId)
        {
            return MerchandiseHierarchy.Contains(hierarchyClassId);
        }

        public bool IsValidBrandHierarchyClassId(int hierarchyClassId)
        {
            return BrandHierarchy.Contains(hierarchyClassId);
        }

        public bool IsValidManufacturerHierarchyClassId(int hierarchyClassId)
        {
            return ManufacturerHierarchy.Contains(hierarchyClassId);
        }

        public bool IsValidTaxHierarchyClassId(int hierarchyClassId)
        {
            return TaxHierarchy.Contains(hierarchyClassId);
        }

        public List<int> LoadHierarchyIds(string hierarchyName, int hierarchyLevel)
        {
            var ids = getHierarchyIdsQueryHandler.Search(new GetHierarchyIdsParameters { HierarhcyName = hierarchyName, HierarchyLevel = hierarchyLevel });
            return ids;
        }
    }
}