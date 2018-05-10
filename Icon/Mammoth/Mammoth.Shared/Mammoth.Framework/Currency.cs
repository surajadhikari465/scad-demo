namespace Mammoth.Framework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Currency
    {
        public int CurrencyID { get; set; }

        [Required]
        [StringLength(3)]
        public string CurrencyCode { get; set; }

        [Required]
        [StringLength(25)]
        public string CurrencyDesc { get; set; }

        [StringLength(255)]
        public string IssuingEntity { get; set; }

        public int? NumericCode { get; set; }

        public int? MinorUnit { get; set; }

        [StringLength(3)]
        public string Symbol { get; set; }
    }
}
