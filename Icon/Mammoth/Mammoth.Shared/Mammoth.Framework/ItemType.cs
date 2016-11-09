namespace Mammoth.Framework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ItemType
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int itemTypeID { get; set; }

        [Required]
        [StringLength(3)]
        public string itemTypeCode { get; set; }

        [StringLength(255)]
        public string itemTypeDesc { get; set; }

        public DateTime AddedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}
