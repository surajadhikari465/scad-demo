using System.Collections.Generic;

namespace Icon.Web.Mvc.Models
{
    public class ProductSelectionGroupGridViewModel
    {
        public List<ProductSelectionGroupViewModel> ProductSelectionGroups { get; set; }
        public List<TraitViewModel> Traits { get; set; }
        public List<ProductSelectionGroupTypeViewModel> ProductSelectionGroupTypes { get; set; }
        public List<HierarchyClassViewModel> MerchandiseHierarchyClasses { get; set; }
        public IEnumerable<AttributeViewModel> Attributes { get; set; }
    }
}