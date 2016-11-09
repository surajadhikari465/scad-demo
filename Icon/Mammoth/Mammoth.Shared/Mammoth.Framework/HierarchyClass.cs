namespace Mammoth.Framework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HierarchyClass")]
    public partial class HierarchyClass
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int HierarchyClassID { get; set; }

        public int? HierarchyID { get; set; }

        [StringLength(255)]
        public string HierarchyClassName { get; set; }

        public DateTime AddedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public virtual Hierarchy Hierarchy { get; set; }
    }
}
