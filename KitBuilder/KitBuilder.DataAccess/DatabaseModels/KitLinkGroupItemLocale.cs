using System;
using System.ComponentModel.DataAnnotations;

namespace KitBuilder.DataAccess.DatabaseModels
{
    public partial class KitLinkGroupItemLocale
    {
        public int KitLinkGroupItemLocaleId { get; set; }
        [Required]
        public int KitLinkGroupItemId { get; set; }
        [Required]
		public int KitLinkGroupLocaleId { get; set; }
		public string Properties { get; set; }
        [Required]
        public int DisplaySequence { get; set; }
        public bool? Exclude { get; set; }
		public DateTime InsertDateUtc { get; set; }
		public DateTime? LastUpdatedDateUtc { get; set; }
		public string LastModifiedBy { get; set; }

        public KitLinkGroupItem KitLinkGroupItem { get; set; }
		public KitLinkGroupLocale KitLinkGroupLocale { get; set; }
	}
}
