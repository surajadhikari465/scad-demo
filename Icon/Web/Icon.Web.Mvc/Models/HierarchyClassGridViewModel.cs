using System.Collections.Generic;

namespace Icon.Web.Mvc.Models
{
    public class HierarchyClassGridViewModel : HierarchyClassViewModel
    {
        public string AddNodeLink { get; set; }
        public string EditNodeLink { get; set; }
        public string DeleteNodeLink { get; set; }
        public IEnumerable<HierarchyClassGridViewModel> HierarchySubClasses { get; set; }
    }
}