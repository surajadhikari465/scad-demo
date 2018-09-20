using System;
using System.Collections.Generic;
using System.Text;

namespace KitBuilderWebApi.DataAccess.Dto
{
    public class KitLinkGroupItemDto
    {
        public KitLinkGroupItemDto()
        {
            KitLinkGroupItemLocale = new HashSet<KitLinkGroupItemLocaleDto>();
        }

        public int KitLinkGroupItemId { get; set; }
        public int KitId { get; set; }
        public int LinkGroupItemId { get; set; }
        public DateTime InsertDate { get; set; }

        public KitDto Kit { get; set; }
        public LinkGroupItemDto LinkGroupItem { get; set; }
        public ICollection<KitLinkGroupItemLocaleDto> KitLinkGroupItemLocale { get; set; }
    }
}
