using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Icon.Web.Mvc.Models
{
    public class ProductSelectionGroupCreateViewModel
    {
        [Display(Name = "PSG Name")]
        [Required]
        public string ProductSelectionGroupName { get; set; }

        [Display(Name = "Trait Value")]
        public string TraitValue { get; set; }

        [Display(Name = "PSG Type")]
        public int SelectedProductSelectionGroupTypeId { get; set; }

        public SelectList ProductSelectionGroupTypes { get; set; }

        [Display(Name = "Trait")]
        public int? SelectedTraitId { get; set; }

        public SelectList Traits { get; set; }

        [Display(Name = "Merchandise Sub Brick")]
        public int? SelectedMerchandiseHierarchyClassId { get; set; }
        public SelectList MerchandiseHierarchyClasses { get; set; }

        [Display(Name = "Attribute Name")]
        public int? SelectedAttributeId { get; set; }

        [Display(Name = "Attribute Value")]
        public string AttributeValue { get; set; }

        public SelectList Attributes { get; set; }
    }
}