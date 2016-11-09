using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mammoth.Esb.HierarchyClassListener.Models
{
    public class HierarchyClassModel
    {
        public int HierarchyClassId { get; set; }
        public int HierarchyId { get; set; }
        public string HierarchyClassName { get; set; }
        public int HierarchyClassParentId { get; set; }
        public DateTime Timestamp { get; set; }
        public string HierarchyLevelName { get; set; }
    }
}
