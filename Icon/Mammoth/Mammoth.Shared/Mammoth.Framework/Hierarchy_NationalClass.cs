namespace Mammoth.Framework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Hierarchy_NationalClass
    {
        [Key]
        public int HierarchyNationalClassID { get; set; }

        public int? FamilyHCID { get; set; }

        public int? CategoryHCID { get; set; }

        public int? SubcategoryHCID { get; set; }

        public int? ClassHCID { get; set; }

        public DateTime AddedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}
