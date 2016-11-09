using Icon.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.Listeners.HierarchyClass.Models
{
    public class ValidateHierarchyClassModel
    {
        public ValidateHierarchyClassModel(HierarchyClassModel hierarchyClass)
        {
            HierarchyClassId = hierarchyClass.HierarchyClassId;
            HierarchyClassName = hierarchyClass.HierarchyClassName;
            HierarchyLevelName = hierarchyClass.HierarchyLevelName;
            HierarchyParentClassId = hierarchyClass.ParentHierarchyClassId == 0 ? (int?)null :hierarchyClass.ParentHierarchyClassId;
            HierarchyName = hierarchyClass.HierarchyName;

            if (hierarchyClass.HierarchyClassTraits != null && hierarchyClass.HierarchyClassTraits.ContainsKey(TraitCodes.SubBrickCode))
            {
                SubBrickCode = hierarchyClass.HierarchyClassTraits[TraitCodes.SubBrickCode];
            }
        }

        public int HierarchyClassId { get; set; }
        public string HierarchyClassName { get; set; }
        public string HierarchyLevelName { get; set; }
        public string HierarchyName { get; set; }
        public int? HierarchyParentClassId { get; set; }
        public string SubBrickCode { get; private set; }
    }
}
