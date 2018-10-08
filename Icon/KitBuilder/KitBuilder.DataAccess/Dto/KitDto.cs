using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KitBuilder.DataAccess.Dto
{
    public class KitDto
    {
		public KitDto()
		{
			KitInstructionList = new HashSet<KitInstructionListDto>();
			KitLinkGroup = new HashSet<KitLinkGroupDto>();
			KitLocale = new HashSet<KitLocaleDto>();
		}

		public int KitId { get; set; }
		[Required]
		public int ItemId { get; set; }
		[StringLength(255, ErrorMessage = "Description can have maximum length of 255.")]
		public string Description { get; set; }
		public DateTime InsertDateUtc { get; set; }
		public DateTime? LastUpdatedDateUtc { get; set; }
		public ItemsDto Item { get; set; }
		public ICollection<KitInstructionListDto> KitInstructionList { get; set; }
		public ICollection<KitLinkGroupDto> KitLinkGroup { get; set; }
		public ICollection<KitLocaleDto> KitLocale { get; set; }
	}
}
