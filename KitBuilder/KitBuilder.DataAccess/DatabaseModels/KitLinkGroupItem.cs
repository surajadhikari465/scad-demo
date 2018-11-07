using System;
using System.Collections.Generic;

namespace KitBuilder.DataAccess.DatabaseModels
{
    public partial class KitLinkGroupItem
    {
        public KitLinkGroupItem()
        {
            KitLinkGroupItemLocale = new HashSet<KitLinkGroupItemLocale>();
        }

        public int KitLinkGroupItemId { get; set; }
		public int KitLinkGroupId { get; set; }
		public int LinkGroupItemId { get; set; }
		public DateTime InsertDateUtc { get; set; }
		public DateTime? LastUpdatedDateUtc { get; set; }

		public KitLinkGroup KitLinkGroup { get; set; }
		public LinkGroupItem LinkGroupItem { get; set; }
        public ICollection<KitLinkGroupItemLocale> KitLinkGroupItemLocale { get; set; }
    }
}
