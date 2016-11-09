using Icon.Framework;
using Icon.Web.Attributes;
using Icon.Web.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Icon.Web.DataAccess.Models;
using Icon.Web.Common;

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

        [Display(Name = "Class Code")]
        [RegularExpression(@"^[0-9]{0,255}$", ErrorMessage = "Please enter a numerical value.")]
        
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
