﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KitBuilderWebApi.DataAccess.Dto
{
	public partial class KitLinkGroupItemLocaleDto
	{
		public int KitLinkGroupItemLocaleId { get; set; }
		[Required]
		public int KitLinkGroupItemId { get; set; }
		[Required]
		public int KitLinkGroupLocaleId { get; set; }
		public string Properties { get; set; }
		[Required]
		public int DisplaySequence { get; set; }
		public bool? Exclude { get; set; }
		public DateTime InsertDateUtc { get; set; }
		public DateTime? LastUpdatedDateUtc { get; set; }
		public string LastModifiedBy { get; set; }

		public KitLinkGroupItemDto KitLinkGroupItem { get; set; }
		public KitLocaleDto KitLocale { get; set; }
	}
}
