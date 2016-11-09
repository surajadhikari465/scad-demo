using Icon.Web.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Icon.Web.Mvc.Models
{
    public class HierarchyClassManagementViewModel
    {
        public int HierarchyId { get; set; }
        [Display(Name = "Hierarchy")]
        public string HierarchyName { get; set; }
        public int HierarchyClassId { get; set; }
        public int? HierarchyParentClassId { get; set; }
        public int? HierarchyLevel { get; set; }
        [HierarchyClassName]
        [ValueRequired]
        [Display(Name = "Class Name")]
        public string HierarchyClassName { get; set; }
        [Display(Name = "Parent Class")]
        public string HierarchyParentClassName  { get; set; }
        public List<HierarchyClassTraitViewModel> HierarchyClassTraitAssociations { get; set; }
    }
}