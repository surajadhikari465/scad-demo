using Icon.Framework;
using Icon.Web.Attributes;
using System.ComponentModel.DataAnnotations;
using Icon.Web.DataAccess.Models;
using Icon.Web.Common.Utility;

namespace Icon.Web.Mvc.Models
{
    public class MerchandiseHierarchyClassTraitViewModel
    {
        public MerchandiseHierarchyClassTraitViewModel(MerchandiseHierarchyClassTrait trait)
        {
            this.FinancialHierarchyClassId = trait.FinancialHierarchyClassId;
            this.ProhibitDiscount = trait.ProhibitDiscount;
            this.ItemType = trait.ItemType;
            this.HierarchyClassId = trait.HierarchyClassId;
        }
        public int HierarchyClassId { get; set; }
        public bool? ProhibitDiscount { get; set; }
        public int? FinancialHierarchyClassId { get; set; }
        public string ItemType { get; set; }
    }
}
