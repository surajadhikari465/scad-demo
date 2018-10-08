using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KitBuilderWebApi.DatabaseModels
{
    public partial class KitLinkGroup
    {
        public KitLinkGroup()
        {
			KitLinkGroupItem = new HashSet<KitLinkGroupItem>();
			KitLinkGroupLocale = new HashSet<KitLinkGroupLocale>();
        }

        public int KitLinkGroupId { get; set; }
        [Required]
        public int KitId { get; set; }
        [Required]
        public int LinkGroupId { get; set; }
		public DateTime InsertDateUtc { get; set; }
		public DateTime? LastUpdatedDateUtc { get; set; }

		public Kit Kit { get; set; }
        public LinkGroup LinkGroup { get; set; }
		public ICollection<KitLinkGroupItem> KitLinkGroupItem { get; set; }
		public ICollection<KitLinkGroupLocale> KitLinkGroupLocale { get; set; }
    }
}
