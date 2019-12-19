using System.ComponentModel.DataAnnotations;
using Icon.Web.Attributes;


namespace Icon.Web.Mvc.Models
{
    public class BarcodeTypeViewModel
    {
        [Required]
        [Display(Name = "Barcode Type")]       
        [MaxLength(255, ErrorMessage = "Barcode type must be 255 characters or less")]
        public string BarcodeType { get; set; }

        [Display(Name = "Start")]
        [RegularExpression(@"^\d{1,11}$", ErrorMessage = "Please enter only numbers")]
        [PluCategoryStart]
        public string BeginRange { get; set; }

        [Required]
        [Display(Name="End")]
        [RegularExpression(@"^\d{1,11}$", ErrorMessage = "Please enter only numbers")]       
        public string EndRange { get; set; }

        public int BarcodeTypeId { get; set; }

        [Required]
        [Display(Name = "Scale PLU")]
        public bool ScalePlu { get; set; }
    }
}