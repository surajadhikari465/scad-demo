using System.Collections.Generic;

namespace Icon.Web.DataAccess.Models
{
    public class HierarchyClassListModel
    {
        public List<HierarchyClassModel> BrandHierarchyList { get; set; }
        public List<HierarchyClassModel> TaxHierarchyList { get; set; }
        public List<HierarchyClassModel> MerchandiseHierarchyList { get; set; }
        public List<HierarchyClassModel> BrowsingHierarchyList { get; set; }
        public List<HierarchyClassModel> NationalHierarchyList { get; set; }
        public List<HierarchyClassModel> FinancialHierarchyList { get; set; }
    }
}
