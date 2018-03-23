namespace Mammoth.Framework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("dbo.RegionGpmStatus")]
    public partial class RegionGpmStatus
    {
        [Key]
        [Required]
        [StringLength(2)]
        public string Region { get; set; }

        public bool IsGpmEnabled { get; set; }
    }
}
