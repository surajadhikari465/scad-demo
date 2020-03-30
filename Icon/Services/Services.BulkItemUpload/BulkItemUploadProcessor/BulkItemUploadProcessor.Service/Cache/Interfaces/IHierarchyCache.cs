using System.Collections.Generic;

namespace BulkItemUploadProcessor.Service.Cache.Interfaces
{
    public interface IHierarchyCache
    {
        void LoadAll();
        List<int> LoadHierarchyIds(string hierarchyName, int hierarchyLevel);
        bool IsValidNationalHierarchyClassId(int hierarchyClassId);
        bool IsValidMerchandiseHierarchyClassId(int hierarchyClassId);
        bool IsValidBrandHierarchyClassId(int hierarchyClassId);
        bool IsValidManufacturerHierarchyClassId(int hierarchyClassId);
        bool IsValidTaxHierarchyClassId(int hierarchyClassId);
        void Clear();
        void Refresh();

    }
}

    