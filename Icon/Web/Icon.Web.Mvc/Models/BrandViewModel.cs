using Icon.Web.Attributes;
using Icon.Web.Common;
using Icon.Web.DataAccess.Models;
using System.ComponentModel.DataAnnotations;

namespace Icon.Web.Mvc.Models
{
    public class BrandViewModel : HierarchyClassViewModel
    {
        [IconPropertyValidation(ValidatorPropertyNames.BrandName, CanBeNullOrEmprty = false)]
        [Required(ErrorMessage = "Please enter a brand name.")]
        [Display(Name = "Brand Name")]
        public string BrandName { get; set; }

        [Display(Name = "Brand Abbreviation")]
        [IconPropertyValidation(ValidatorPropertyNames.BrandAbbreviation, CanBeNullOrEmprty = true)]
        public string BrandAbbreviation { get; set; }

        public BrandViewModel() { }

        public BrandViewModel(BrandModel brand)
        {
            base.HierarchyClassId = brand.HierarchyClassId;
            base.HierarchyId = brand.HierarchyId;
            base.HierarchyLevel = brand.HierarchyLevel;
            base.HierarchyParentClassId = brand.HierarchyParentClassId;
            BrandName = brand.HierarchyClassName;
            BrandAbbreviation = brand.BrandAbbreviation;
        }
    }
}