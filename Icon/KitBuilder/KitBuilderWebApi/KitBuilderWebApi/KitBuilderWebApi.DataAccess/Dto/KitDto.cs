using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KitBuilderWebApi.DataAccess.Dto
{
    public class KitDto
    {
        public KitDto()
        {
            KitLinkGroup = new HashSet<KitLinkGroupDto>();
            KitLinkGroupItem = new HashSet<KitLinkGroupItemDto>();
            KitLocale = new HashSet<KitLocaleDto>();
        }

        public int KitId { get; set; }
        [Required]
        public int ItemId { get; set; }
        [StringLength(255, ErrorMessage = "Description can have maximum length of 255.")]
        public string Description { get; set; }
        public int? InstructionListId { get; set; }
        public DateTime InsertDate { get; set; }

        public InstructionListDto InstructionList { get; set; }
        public ItemsDto Item { get; set; }
        public KitInstructionListDto KitInstructionList { get; set; }
        public ICollection<KitLinkGroupDto> KitLinkGroup { get; set; }
        public ICollection<KitLinkGroupItemDto> KitLinkGroupItem { get; set; }
        public ICollection<KitLocaleDto> KitLocale { get; set; }
    }
}
