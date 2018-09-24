using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KitBuilderWebApi.DatabaseModels
{
    public partial class KitLinkGroupLocale
    {
        public int KitLinkGroupLocaleId { get; set; }
        [Required]
        public int KitLinkGroupId { get; set; }
        [Required]
        public int KitLocaleId { get; set; }
        public string Properties { get; set; }
        public int DisplaySequence { get; set; }
        public int? MinimumCalories { get; set; }
        public int? MaximumCalories { get; set; }
        public bool? Exclude { get; set; }
        public DateTime InsertDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string LastModifiedBy { get; set; }

        public KitLinkGroup KitLinkGroup { get; set; }
        public KitLocale KitLocale { get; set; }
    }
}
