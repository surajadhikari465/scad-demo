namespace Mammoth.Framework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Hierarchy")]
    public partial class Hierarchy
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Hierarchy()
        {
            HierarchyClasses = new HashSet<HierarchyClass>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int hierarchyID { get; set; }

        [Required]
        [StringLength(255)]
        public string hierarchyName { get; set; }

        public DateTime AddedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HierarchyClass> HierarchyClasses { get; set; }
    }
}
