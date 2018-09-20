using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace KitBuilderWebApi.DataAccess.Dto
{
    public class KitLinkGroupLocaleDto
    {
        public int KitLinkGroupLocaleId { get; set; }
        [Required]
        public int KitLinkGroupId { get; set; }
        [Required]
        public int KitLocaleId { get; set; }
        public string Properties { get; set; }
        public int? DisplaySequence { get; set; }
        public int? MinimumCalories { get; set; }
        public int? MaximumCalories { get; set; }
        public int? Exclude { get; set; }
        public DateTime InsertDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string LastModifiedBy { get; set; }

        public KitLinkGroupDto KitLinkGroup { get; set; }
        public KitLocaleDto KitLocale { get; set; }
    }
}
