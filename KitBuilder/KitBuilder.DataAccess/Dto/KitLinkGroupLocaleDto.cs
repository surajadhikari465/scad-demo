using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace KitBuilder.DataAccess.Dto
{
	public class KitLinkGroupLocaleDto
	{
		public KitLinkGroupLocaleDto()
		{
			KitLinkGroupItemLocales = new HashSet<KitLinkGroupItemLocaleDto>();
		}

		public int KitLinkGroupLocaleId { get; set; }
		[Required]
		public int KitLinkGroupId { get; set; }
		[Required]
		public int KitLocaleId { get; set; }
		public string Properties { get; set; }
		public int Minimum { get; set; }
		public int Maximum { get; set; }
		public int? NumOfFreeToppings { get; set; }
		public int? DisplaySequence { get; set; }
		public int? MinimumCalories { get; set; }
		public int? MaximumCalories { get; set; }
		public bool? Exclude { get; set; }
		public DateTime InsertDateUtc { get; set; }
		public DateTime? LastUpdatedDateUtc { get; set; }
		public string LastModifiedBy { get; set; }
		public KitLinkGroupDto KitLinkGroup { get; set; }
		public KitLocaleDto KitLocale { get; set; }
		public ICollection<KitLinkGroupItemLocaleDto> KitLinkGroupItemLocales { get; set; }
	}
}
