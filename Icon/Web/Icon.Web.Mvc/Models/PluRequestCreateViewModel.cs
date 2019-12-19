using Icon.Web.Attributes;
using Icon.Web.Common;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Collections.Generic;

namespace Icon.Web.Mvc.Models
{
    public class PluRequestCreateViewModel
    {
        public PluRequestCreateViewModel()
        {
            var types  = new List<string>
            {
                "POS","Scale"
            };

            PluTypes = new SelectList(types, "Scale");
        }   

        [Display(Name = "National PLU")]       
        [ScanCode]
        public string NationalPLU { get; set; }

        [Display(Name = "Brand Name")]
        [Required]
        [IconPropertyValidation(ValidatorPropertyNames.BrandName, CanBeNullOrEmpty = false)]
        public string BrandName { get; set; }

        [Display(Name = "Product Description")]
        [Required]
        [IconPropertyValidation(ValidatorPropertyNames.ProductDescription, CanBeNullOrEmpty = false)]
        public string ProductDescription { get; set; }

        [Display(Name = "POS Description")]
        [Required]
        [IconPropertyValidation(ValidatorPropertyNames.PosDescription, CanBeNullOrEmpty = false)]
        public string PosDescription { get; set; }

        [Display(Name = "Item Pack")]
        [PackageUnit]
        [Required]
        public string PackageUnit { get; set; }

        [Display(Name = "Size")]
        [RetailSize]
        [Required]
        public string RetailSize { get; set; }

        [Display(Name = "UOM")]
        public string RetailUom { get; set; }
        public SelectList RetailUoms { get; set; }

        [Display(Name = "PLU Type")]        
        public string PluType { get; set; }
        public SelectList PluTypes { get; set; }

        [Display (Name="Notes")]
        public string Notes { get; set; }

        public int? FinancialHierarchyClassId { get; set; }
        public IEnumerable<SelectListItem> FinanacialHierarchyClasses { get; set; }

        [Display(Name = "SubTeam")]
        public string FinancialHierarchyName { get; set; }
       
        public int? NationalHierarchyClassId { get; set; }
        public IEnumerable<SelectListItem> NationalHierarchyClasses { get; set; }

        [Display(Name = "National Class")]
        public string NationalHierarchyName { get; set; }
    }
}