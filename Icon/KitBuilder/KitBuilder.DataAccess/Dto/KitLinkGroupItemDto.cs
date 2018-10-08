using System;
using System.Collections.Generic;
using System.Text;

namespace KitBuilder.DataAccess.Dto
{
	public class KitLinkGroupItemDto
	{
		public KitLinkGroupItemDto()
		{
			KitLinkGroupItemLocales = new HashSet<KitLinkGroupItemLocaleDto>();
		}

		public int KitLinkGroupItemId { get; set; }
		public int KitLinkGroupId { get; set; }
		public int LinkGroupItemId { get; set; }
		public DateTime InsertDateUtc { get; set; }
		public LinkGroupItemDto LinkGroupItem { get; set; }
		public ICollection<KitLinkGroupItemLocaleDto> KitLinkGroupItemLocales { get; set; }
	}
}
