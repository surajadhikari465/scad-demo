using System;
using System.Collections.Generic;

namespace MammothWebApi.DataAccess.Models
{
    public partial class Hierarchy
    {
        public Hierarchy()
        {
            this.HierarchyClasses = new List<HierarchyClass>();
        }

        public int hierarchyID { get; set; }
        public string hierarchyName { get; set; }
        public System.DateTime AddedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public virtual ICollection<HierarchyClass> HierarchyClasses { get; set; }
    }
}
