using Icon.Web.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Icon.Web.Mvc.Models
{
    [AtLeastOneRequired]
    public class PluSearchViewModel
    {
        [ScanCode]
        [Display(Name = "National PLU")]
        public string NationalPlu { get; set; }
        [ScanCode]
        [Display(Name = "Regional PLU")]
        public string RegionalPlu { get; set; }
        [Display(Name = "PLU Description")]
        public string PluDescription { get; set; }
        [Display(Name = "Brand Name")]
        public string BrandName { get; set; }
        public List<PluMappingViewModel> PluMapping { get; set; }
    }
}