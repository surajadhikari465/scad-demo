using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KitBuilder.DataAccess.DatabaseModels
{
    public partial class KitLinkGroupLocale
    {
		public KitLinkGroupLocale()
		{
			KitLinkGroupItemLocale = new HashSet<KitLinkGroupItemLocale>();
		}

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
		public DateTime InsertDateUtc { get; set; }
		public DateTime? LastUpdatedDateUtc { get; set; }
		public string LastModifiedBy { get; set; }

        public KitLinkGroup KitLinkGroup { get; set; }
        public KitLocale KitLocale { get; set; }
		public ICollection<KitLinkGroupItemLocale> KitLinkGroupItemLocale { get; set; }
	}
}
