using System;
using System.Collections.Generic;

namespace MammothWebApi.DataAccess.Models
{
    public partial class Hierarchy_NationalClass
    {
        public int HierarchyNationalClassID { get; set; }
        public Nullable<int> FamilyHCID { get; set; }
        public Nullable<int> CategoryHCID { get; set; }
        public Nullable<int> SubcategoryHCID { get; set; }
        public Nullable<int> ClassHCID { get; set; }
        public System.DateTime AddedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}
