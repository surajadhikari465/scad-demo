using System.Collections.Generic;
using System.Web.Mvc;

namespace Icon.Web.Mvc.Models
{
    public class MerchTaxMappingCreateModel : MerchTaxMappingViewModel
    {
        public IEnumerable<SelectListItem> MerchandiseHierarchyClasses { get; set; }
        public IEnumerable<SelectListItem> TaxHierarchyClasses { get; set; }
    }
}
