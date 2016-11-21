using Icon.Infor.Listeners.HierarchyClass.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Icon.Esb.Schemas.Wfm.Contracts;

namespace Icon.Infor.Listeners.HierarchyClass.Requests
{
    public class HierarchyClassEsbServiceRequest
    {
        public ActionEnum Action { get; set; }
        public IEnumerable<VimHierarchyClassModel> HierarchyClasses { get; set; }
        public string HierarchyLevelName { get; set; }
        public string HierarchyName { get; set; }
        public string MessageId { get; set; }
    }
}
