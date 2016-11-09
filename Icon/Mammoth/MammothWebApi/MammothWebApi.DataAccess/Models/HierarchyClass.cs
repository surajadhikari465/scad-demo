using System;
using System.Collections.Generic;

namespace MammothWebApi.DataAccess.Models
{
    public partial class HierarchyClass
    {
        public int HierarchyClassID { get; set; }
        public Nullable<int> HierarchyID { get; set; }
        public string HierarchyClassName { get; set; }
        public System.DateTime AddedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public virtual Hierarchy Hierarchy { get; set; }
    }
}
