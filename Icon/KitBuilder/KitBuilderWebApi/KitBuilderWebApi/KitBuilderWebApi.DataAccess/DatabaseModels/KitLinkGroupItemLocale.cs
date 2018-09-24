using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KitBuilderWebApi.DatabaseModels
{
    public partial class KitLinkGroupItemLocale
    {
        public int KitLinkGroupItemLocaleId { get; set; }
        [Required]
        public int KitLinkGroupItemId { get; set; }
        [Required]
        public int KitLocaleId { get; set; }
        public string Properties { get; set; }
        [Required]
        public int DisplaySequence { get; set; }
        public bool? Exclude { get; set; }
        public DateTime InsertDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string LastModifiedBy { get; set; }

        public KitLinkGroupItem KitLinkGroupItem { get; set; }
        public KitLocale KitLocale { get; set; }
    }
}
