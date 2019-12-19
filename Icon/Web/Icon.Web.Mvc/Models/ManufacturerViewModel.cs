using System.ComponentModel.DataAnnotations;
using Icon.Web.Common;
using Icon.Web.DataAccess.Models;

namespace Icon.Web.Mvc.Models
{
    public class ManufacturerViewModel : HierarchyClassViewModel
    {
        [Required(ErrorMessage = "Please enter a manufacturer name.")]
        [RegularExpression(@"^.{1,255}$", ErrorMessage = "Manufacturer Name should be up to 255 characters length.")]
        [Display(Name = "Manufacturer Name")]
        public string ManufacturerName { get; set; }
        [Display(Name = "Zip Code")]
        [RegularExpression(@"^[A-Za-z0-9\s-]+$", ErrorMessage = "ZipCode allows letters,numbers,space and hyphens only.")]
        [MaxLength(10, ErrorMessage = "ZipCode must be 10 characters or less")]
        public string ZipCode { get; set; }
        [Display(Name = "AR Customer Code")]
        [RegularExpression(@"^.{1,255}$", ErrorMessage = "AR Customer ID should be up to 255 characters length.")]
        public string ArCustomerId { get; set; }
        public Enums.WriteAccess UserWriteAccess { get; set; }

        public ManufacturerViewModel(ManufacturerModel manufacturer)
        {
            base.HierarchyClassId = manufacturer.HierarchyClassId;
            base.HierarchyId = manufacturer.HierarchyId;
            base.HierarchyLevel = manufacturer.HierarchyLevel;
            base.HierarchyParentClassId = manufacturer.HierarchyParentClassId;
            ManufacturerName = manufacturer.HierarchyClassName;
            ZipCode = manufacturer.ZipCode;
            ArCustomerId = manufacturer.ArCustomerId;
        }

        public ManufacturerViewModel()
        {

        }
    }
}