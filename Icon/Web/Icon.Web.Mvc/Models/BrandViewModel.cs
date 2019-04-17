using Icon.Web.Attributes;
using Icon.Web.Common;
using System.Collections.Generic;
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

        [Display(Name = "Designation")]
        [RegularExpression(@"^(Global|Regional)$", ErrorMessage = "Valid values are Global or Regional")]
        public string Designation { get; set; }

        public static IEnumerable<string> DesignationList { get { return new string[] { "Global", "Regional" }; }}

        [Display(Name = "Parent Company")]
        public string ParentCompany { get; set; }

        public IEnumerable<string> BrandList { get; set; }
       
        [Display(Name = "Zip Code")]
        [RegularExpression(@"^[0-9]{5}(?:-[0-9]{4})?$", ErrorMessage = "Invalid Zip Code. Example: 12345 or 12345-6789")]
        public string ZipCode { get; set; }

        [Display(Name = "Locality")]
        [RegularExpression(@"^.{1,35}$", ErrorMessage = "Locality should be up to 35 characters length.")]
        public string Locality { get; set; }

        public bool IsBrandCoreUpdateAuthorized { get; set; } //BrandName and BrandAbbreviation update is restricted.

        public string BrandHashKey { get; set; }
        public string TraitHashKey { get; set; }

        public BrandViewModel() { }

        public BrandViewModel(BrandModel brand)
        {
            base.HierarchyClassId = brand.HierarchyClassId;
            base.HierarchyId = brand.HierarchyId;
            base.HierarchyLevel = brand.HierarchyLevel;
            base.HierarchyParentClassId = brand.HierarchyParentClassId;
            BrandName = brand.HierarchyClassName;
            BrandAbbreviation = brand.BrandAbbreviation;
            Designation = brand.Designation;
            ParentCompany = brand.ParentCompany;
            ZipCode = brand.ZipCode;
            Locality = brand.Locality;
        }
    }
}