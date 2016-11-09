using System.Collections.Generic;

namespace Icon.Web.Mvc.Models
{
    public class NationalClassGridViewModel : NationalClassViewModel
    {
        public IEnumerable<NationalClassGridViewModel> HierarchySubClasses { get; set; }       
        public string AddNodeLink { get; set; }
        public string EditNodeLink { get; set; }
        public string DeleteNodeLink { get; set; }
    }
}