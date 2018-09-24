using System;
using System.Collections.Generic;

namespace KitBuilderWebApi.DatabaseModels
{
    public partial class KitLinkGroupItem
    {
        public KitLinkGroupItem()
        {
            KitLinkGroupItemLocale = new HashSet<KitLinkGroupItemLocale>();
        }

        public int KitLinkGroupItemId { get; set; }
        public int KitId { get; set; }
        public int LinkGroupItemId { get; set; }
        public DateTime InsertDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public Kit Kit { get; set; }
        public LinkGroupItem LinkGroupItem { get; set; }
        public ICollection<KitLinkGroupItemLocale> KitLinkGroupItemLocale { get; set; }
    }
}
