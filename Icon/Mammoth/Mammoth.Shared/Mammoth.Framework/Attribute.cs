namespace Mammoth.Framework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Attribute
    {
        public int AttributeID { get; set; }

        public int? AttributeGroupID { get; set; }

        [Required]
        [StringLength(3)]
        public string AttributeCode { get; set; }

        [StringLength(255)]
        public string AttributeDesc { get; set; }

        public DateTime AddedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}
