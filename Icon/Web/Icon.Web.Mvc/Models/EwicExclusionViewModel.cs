using Icon.Web.Attributes;
using Icon.Web.DataAccess.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Icon.Web.Mvc.Models
{
    public class EwicExclusionViewModel
    {
        [ScanCode]
        [ValueRequired]
        public string NewExclusion { get; set; }
        public string RemovedExclusionSelectedId { get; set; }
        public IEnumerable<SelectListItem> RemovableEwicExclusions { get; set; }
        public List<EwicExclusionModel> CurrentExclusions { get; set; }
    }
}