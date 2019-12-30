using Icon.Web.Attributes;
using Icon.Web.Common;
using System.Collections.Generic;
using Icon.Web.DataAccess.Models;
using System.ComponentModel.DataAnnotations;

namespace Icon.Web.Mvc.Models
{
    public class BrandViewModel : HierarchyClassViewModel
    {
        [IconPropertyValidation(ValidatorPropertyNames.BrandName, CanBeNullOrEmpty = false)]
        [Required(ErrorMessage = "Please enter a brand name.")]
        [Display(Name = "Brand Name")]
        public string BrandName { get; set; }

        [Required(ErrorMessage = "Please enter a brand abbreviation.")]
        [Display(Name = "Brand Abbreviation")]
        [IconPropertyValidation(ValidatorPropertyNames.BrandAbbreviation, CanBeNullOrEmpty = true)]
        public string BrandAbbreviation { get; set; }

        [Display(Name = "Designation")]
        [RegularExpression(@"^(Global|Regional)$", ErrorMessage = "Valid values are Global or Regional")]
        public string Designation { get; set; }

        public static IEnumerable<string> DesignationList { get { return new string[] { "Global", "Regional" }; }}

        [Display(Name = "Parent Company")]
        public string ParentCompany { get; set; }

        public IEnumerable<string> BrandList { get; set; }
       
        [Display(Name = "Zip Code")]
        [RegularExpression(@"^[A-Za-z0-9\s-]+$", ErrorMessage = "Zip Code allows letters,numbers,space and hyphens only.")]
        [MaxLength(10, ErrorMessage = "ZipCode must be 10 characters or less")]
        public string ZipCode { get; set; }

        [Display(Name = "Locality")]
        [RegularExpression(@"^.{1,35}$", ErrorMessage = "Locality should be up to 35 characters length.")]
        public string Locality { get; set; }

        public Enums.WriteAccess UserWriteAccess { get; set; }

        public string BrandHashKey { get; set; }
        public string TraitHashKey { get; set; }

        private bool isContactEnabled;
        public bool IsContactViewEnabled { get { return isContactEnabled && HierarchyClassId > 0; }}

        public BrandViewModel(bool enableContactView = false)
        {
           this.isContactEnabled = enableContactView; 
        }

        public BrandViewModel(BrandModel brand, bool enableContactView = true)
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
            this.isContactEnabled = enableContactView;
        }
    }
}