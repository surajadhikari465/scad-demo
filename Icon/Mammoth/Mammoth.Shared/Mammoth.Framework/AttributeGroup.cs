namespace Mammoth.Framework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AttributeGroup
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AttributeGroupID { get; set; }

        [Required]
        [StringLength(3)]
        public string AttributeGroupCode { get; set; }

        [StringLength(255)]
        public string AttributeGroupDesc { get; set; }

        public DateTime AddedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}
