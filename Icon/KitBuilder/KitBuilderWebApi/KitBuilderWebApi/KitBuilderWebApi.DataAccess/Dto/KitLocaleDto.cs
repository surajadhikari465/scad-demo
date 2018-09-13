using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KitBuilderWebApi.DataAccess.Dto
{
    public class KitLocaleDto
    {
        public KitLocaleDto()
        {
            KitLinkGroupItemLocale = new HashSet<KitLinkGroupItemLocaleDto>();
            KitLinkGroupLocale = new HashSet<KitLinkGroupLocaleDto>();
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

        public KitDto Kit { get; set; }
        public LocaleDto Locale { get; set; }
        public StatusDto Status { get; set; }
        public ICollection<KitLinkGroupItemLocaleDto> KitLinkGroupItemLocale { get; set; }
        public ICollection<KitLinkGroupLocaleDto> KitLinkGroupLocale { get; set; }
    }
}
