using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KitBuilderWebApi.DatabaseModels
{
    public partial class KitLocale
    {
        public KitLocale()
        {
            KitLinkGroupLocale = new HashSet<KitLinkGroupLocale>();
        }

        public int KitLocaleId { get; set; }
        [Required]
        public int KitId { get; set; }
        [Required]
        public int LocaleId { get; set; }
        public int? MinimumCalories { get; set; }
        public int? MaximumCalories { get; set; }
        public bool? Exclude { get; set; }
        [Required]
        public int StatusId { get; set; }
		public DateTime InsertDateUtc { get; set; }
		public DateTime? LastUpdatedDateUtc { get; set; }

		public Kit Kit { get; set; }
        public Locale Locale { get; set; }
        public Status Status { get; set; }
        public ICollection<KitLinkGroupLocale> KitLinkGroupLocale { get; set; }
    }
}
