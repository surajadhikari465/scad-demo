using System.Collections.Generic;
using System.Web.Mvc;

namespace Icon.Web.Mvc.Models
{
    public class MerchTaxMappingEditModel : MerchTaxMappingViewModel
    {
        public IEnumerable<SelectListItem> TaxHierarchyClasses { get; set; }
    }
}
