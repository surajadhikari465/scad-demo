using Icon.Framework;
using Icon.Web.Attributes;
using System.ComponentModel.DataAnnotations;
using Icon.Web.DataAccess.Models;

namespace Icon.Web.Mvc.Models
{
    public class NationalClassViewModel
    {
        public int HierarchyId { get; set; }
        [Display(Name = "Hierarchy")]
        public string HierarchyName { get; set; }
        public int HierarchyClassId { get; set; }
        public int? HierarchyLevel { get; set; }
        public int? HierarchyParentClassId { get; set; }
        [Display(Name = "Parent Class")]
        public string HierarchyParentClassName { get; set; }
        public string HierarchyClassLineage { get; set; }

        [Required]
        [HierarchyClassName]
        [Display(Name = "Class Name")]
        public virtual string HierarchyClassName { get; set; }

        [Required]
        [Display(Name = "Class Code")]
        [RegularExpression(@"^[0-9]{5}$", ErrorMessage = "Please enter a 5 digit numerical value.")]
        public string NationalClassCode { get; set; }

        public string HierarchyLevelName { get; set; }     

        public NationalClassViewModel()
        {           
        }

        public NationalClassViewModel(HierarchyClass hierarchyClass)
        {
            HierarchyId = hierarchyClass.hierarchyID;
            HierarchyLevel = hierarchyClass.hierarchyLevel;
            HierarchyClassId = hierarchyClass.hierarchyClassID;
            HierarchyParentClassId = hierarchyClass.hierarchyParentClassID;
            HierarchyClassName = hierarchyClass.hierarchyClassName;

            HierarchyParentClassName = HierarchyClassAccessor.GetHierarchyParentName(hierarchyClass);
            HierarchyClassLineage = HierarchyClassAccessor.GetHierarchyClassLineage(hierarchyClass);
            HierarchyLevelName = HierarchyClassAccessor.GetHierarchyLevelName(hierarchyClass);
        }


        public NationalClassViewModel(HierarchyClassModel hierarchyLineageModel)
        {
            HierarchyId = hierarchyLineageModel.HierarchyId;
            HierarchyClassId = hierarchyLineageModel.HierarchyClassId;
            HierarchyParentClassId = hierarchyLineageModel.HierarchyParentClassId;
            HierarchyClassName = hierarchyLineageModel.HierarchyClassName;
            HierarchyLevel = hierarchyLineageModel.HierarchyLevel;
            HierarchyClassLineage = hierarchyLineageModel.HierarchyLineage;
        }
    }
}
