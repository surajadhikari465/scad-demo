namespace Mammoth.Framework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Hierarchy_Merchandise
    {
        [Key]
        public int HierarchyMerchandiseID { get; set; }

        public int? SegmentHCID { get; set; }

        public int? FamilyHCID { get; set; }

        public int? ClassHCID { get; set; }

        public int? BrickHCID { get; set; }

        public int? SubBrickHCID { get; set; }

        public DateTime AddedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}
