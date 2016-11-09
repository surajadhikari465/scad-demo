using System;
using System.Collections.Generic;

namespace MammothWebApi.DataAccess.Models
{
    public partial class Hierarchy_Merchandise
    {
        public int HierarchyMerchandiseID { get; set; }
        public Nullable<int> SegmentHCID { get; set; }
        public Nullable<int> FamilyHCID { get; set; }
        public Nullable<int> ClassHCID { get; set; }
        public Nullable<int> BrickHCID { get; set; }
        public Nullable<int> SubBrickHCID { get; set; }
        public System.DateTime AddedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}
