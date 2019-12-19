using System.Collections.Generic;

namespace Icon.Web.DataAccess.Models
{
    public class ItemHierarchyClassHistoryAllModel
    {
        public List<ItemHierarchyClassHistoryModel> MerchHierarchy { get; set; } = new List<ItemHierarchyClassHistoryModel>();
        public List<ItemHierarchyClassHistoryModel> TaxHierarchy { get; set; } = new List<ItemHierarchyClassHistoryModel>();
        public List<ItemHierarchyClassHistoryModel> BrandHierarchy { get; set; } = new List<ItemHierarchyClassHistoryModel>();
        public List<ItemHierarchyClassHistoryModel> FinancialHierarchy { get; set; } = new List<ItemHierarchyClassHistoryModel>();
        public List<ItemHierarchyClassHistoryModel> NationalHierarchy { get; set; } = new List<ItemHierarchyClassHistoryModel>();
        public List<ItemHierarchyClassHistoryModel> ManufacturerHierarchy { get; set; } = new List<ItemHierarchyClassHistoryModel>();

    }
}
