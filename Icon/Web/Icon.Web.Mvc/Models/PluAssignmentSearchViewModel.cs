using Icon.Framework;
using Icon.Web.Attributes;
using Icon.Web.Common;
using Icon.Web.Mvc.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Icon.Web.Mvc.Models
{
    [AtLeastOneRequiredSpecified]
    public class PluAssignmentSearchViewModel
    {  
        public PluAssignmentSearchViewModel()
        {
            SelectedPluCategoryId = 0;
        }

        [Display(Name = "PLU Category")]
        public string SelectedPluCategoryName { get; set; }

        [AtLeastOneRequiredProperty]
        public int SelectedPluCategoryId { get; set; }

        [Display(Name="Number of PLUs")]        
        [IconPropertyValidation(ValidatorPropertyNames.MaxPlus, CanBeNullOrEmprty = true)]
        public string MaxPlus { get; set; }


        public IEnumerable<SelectListItem> PluCategories { get; set; }

        public List<IrmaItemViewModel> Items { get; set; }

        public SelectList RetailUoms { get; set; }

        public List<HierarchyClassViewModel> BrandHierarchyClasses { get; set; }

        public List<HierarchyClassViewModel> MerchandiseHierarchyClasses { get; set; }

        public List<HierarchyClassViewModel> TaxHierarchyClasses { get; set; }
        public List<HierarchyClassViewModel> NationalHierarchyClasses { get; set; }
    }
}