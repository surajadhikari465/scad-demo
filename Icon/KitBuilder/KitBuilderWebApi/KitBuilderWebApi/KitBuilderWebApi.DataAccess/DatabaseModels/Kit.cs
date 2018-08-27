using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KitBuilderWebApi.DatabaseModels
{
    public partial class Kit
    {
        public Kit()
        {
            KitLinkGroup = new HashSet<KitLinkGroup>();
            KitLinkGroupItem = new HashSet<KitLinkGroupItem>();
            KitLocale = new HashSet<KitLocale>();
        }

        public int KitId { get; set; }
        [Required]
        public int ItemId { get; set; }
        [StringLength(255, ErrorMessage = "Description can have maximum length of 255.")]
        public string Description { get; set; }
        public int? InstructionListId { get; set; }
        public DateTime InsertDate { get; set; }

        public InstructionList InstructionList { get; set; }
        public Items Item { get; set; }
        public KitInstructionList KitInstructionList { get; set; }
        public ICollection<KitLinkGroup> KitLinkGroup { get; set; }
        public ICollection<KitLinkGroupItem> KitLinkGroupItem { get; set; }
        public ICollection<KitLocale> KitLocale { get; set; }
    }
}
