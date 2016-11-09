using Icon.Web.Attributes;
using Icon.Web.DataAccess.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Icon.Web.Mvc.Models
{
    public class EwicMappingDetailViewModel
    {
        public string AplScanCode { get; set; }
        public IEnumerable<SelectListItem> RemovableMappedWfmScanCodes { get; set; }
        [ValueRequired]
        public string RemovableMappedWfmScanCodesSelectedId { get; set; }
        [ScanCode]
        [ValueRequired]
        public string NewMapping { get; set; }
        public List<EwicMappingModel> CurrentMappings { get; set; }
    }
}