using Icon.Framework;
using Icon.Infor.Listeners.HierarchyClass.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.Listeners.HierarchyClass.Models
{
    public class HierarchyClassDataAccessModel
    {
        public int? HierarchyClassId { get; set; }
        public string HierarchyClassName { get; set; }
        public int HierarchyId { get; set; }
        public string HierarchyLevelName { get; set; }
        public int? ParentHierarchyClassId { get; set; }
        public Dictionary<int, string> HierarchyClassTraits { get; set; }

        public HierarchyClassDataAccessModel() { }

        public HierarchyClassDataAccessModel(HierarchyClassModel model)
        {
            HierarchyClassId = model.HierarchyClassId;
            HierarchyClassName = model.HierarchyClassName;
            HierarchyId = Hierarchies.Ids[model.HierarchyName];
            ParentHierarchyClassId = model.ParentHierarchyClassId.ToHierarchyParentClassId();
            HierarchyLevelName = model.HierarchyLevelName;
            HierarchyClassTraits = model.HierarchyClassTraits
                .ToDictionary(kvp => Traits.Ids[kvp.Key], kvp => kvp.Value);
        }
    }
}
