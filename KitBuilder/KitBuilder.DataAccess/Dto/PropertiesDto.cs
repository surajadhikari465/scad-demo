using System;
using System.Collections.Generic;
using System.Text;

namespace KitBuilder.DataAccess.Dto
{
	public class PropertiesDto
	{
		public int KitLinkGroupLocaleId { get; set; }
		public int KitLinkGroupItemLocaleId { get; set; }
		public int KitLinkGroupId { get; set; }
		public int KitLinkGroupItemId { get; set; }
		public string Name { get; set; }
		public int? DisplaySequence { get; set; }
		public int? MinimumCalories { get; set; }
		public int? MaximumCalories { get; set; }
		public string LastModifiedBy { get; set; }

		public string Properties { get; set; }
		public bool? Excluded { get; set; }
	}
}