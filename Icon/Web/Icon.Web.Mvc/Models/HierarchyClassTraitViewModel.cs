using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Icon.Web.Mvc.Models
{
    public class HierarchyClassTraitViewModel
    {
        // Hierarchy Class Properties
        public int HierarchyId { get; set; }
        public string HierarchyName { get; set; }
        public int HierarchyClassId { get; set; }
        public string HierarchyClassName { get; set; }
        public int? HierarchyParentClassId { get; set; }
        public string HierarchyParentClassName { get; set; }

        // Hierarchy Class Trait Properties
        public int TraitId { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "Maximum number of characters is 50.")]
        [Display(Name = "Trait Value")]
        public string TraitValue { get; set; }
        [Display(Name = "Trait Name")]
        public string TraitDescription { get; set; }
        public IEnumerable<SelectListItem> HierarchyTraitSelectList { get; set; }
        [Display(Name = "Hierarchy Trait")]
        public int SelectedHierarchyClassTrait { get; set; }
    }
}