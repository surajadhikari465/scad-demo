using Icon.Web.Attributes;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Icon.Web.Mvc.Models
{
    public class EwicMappingSearchViewModel
    {
        public IEnumerable<SelectListItem> AplScanCodes { get; set; }
        [ValueRequired]
        public string AplScanCodeSelectedId { get; set; }
    }
}