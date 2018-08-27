using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KitBuilderWebApi.DatabaseModels
{
    public partial class KitLocale
    {
        public KitLocale()
        {
            KitLinkGroupItemLocale = new HashSet<KitLinkGroupItemLocale>();
            KitLinkGroupLocale = new HashSet<KitLinkGroupLocale>();
        }

        public int KitLocaleId { get; set; }
        [Required]
        public int KitId { get; set; }
        [Required]
        public int LocaleId { get; set; }
        public int? MinimumCalories { get; set; }
        public int? MaximumCalories { get; set; }
        public int? Exclude { get; set; }
        [Required]
        public int StatusId { get; set; }
        public DateTime InsertDate { get; set; }

        public Kit Kit { get; set; }
        public Locale Locale { get; set; }
        public Status Status { get; set; }
        public ICollection<KitLinkGroupItemLocale> KitLinkGroupItemLocale { get; set; }
        public ICollection<KitLinkGroupLocale> KitLinkGroupLocale { get; set; }
    }
}
