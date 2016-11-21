using Icon.Esb.Schemas.Wfm.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.Listeners.HierarchyClass.Models
{
    public class VimHierarchyClassModel
    {
        public ActionEnum Action { get; set; }
        public int HierarchyClassId { get; set; }
        public string HierarchyClassName { get; set; }
        public string HierarchyName { get; set; }
        public int ParentHierarchyClassId { get; set; }
        public string HierarchyLevelName { get; set; }
        public Dictionary<string, string> HierarchyClassTraits { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorDetails { get; set; }
        public string MessageId { get; set; }

        public VimHierarchyClassModel() { }

        public VimHierarchyClassModel(InforHierarchyClassModel hc)
        {
            MessageId = hc.InforMessageId;
            Action = hc.Action;
            HierarchyClassId = hc.HierarchyClassId;
            HierarchyClassName = hc.HierarchyClassName;
            HierarchyName = hc.HierarchyName;
            ParentHierarchyClassId = hc.ParentHierarchyClassId;
            HierarchyLevelName = hc.HierarchyLevelName;
            HierarchyClassTraits = hc.HierarchyClassTraits;
        }
    }
}
