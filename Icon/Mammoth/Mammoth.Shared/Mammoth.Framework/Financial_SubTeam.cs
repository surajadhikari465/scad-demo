namespace Mammoth.Framework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Financial_SubTeam
    {
        [Key]
        public int FinancialSubTeamID { get; set; }

        public int FinancialHCID { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        public int? PSNumber { get; set; }

        public int? POSDeptNumber { get; set; }

        public DateTime AddedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}
