using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KitBuilder.DataAccess.DatabaseModels
{
    public partial class Kit
    {
        public Kit()
        {
			KitInstructionList = new HashSet<KitInstructionList>();
			KitLinkGroup = new HashSet<KitLinkGroup>();
            KitLocale = new HashSet<KitLocale>();
        }

        public int KitId { get; set; }
        [Required]
        public int ItemId { get; set; }
        [StringLength(255, ErrorMessage = "Description can have maximum length of 255.")]
        public string Description { get; set; }
		public DateTime InsertDateUtc { get; set; }
		public DateTime? LastUpdatedDateUtc { get; set; }

        public Items Item { get; set; }
		public ICollection<KitInstructionList> KitInstructionList { get; set; }
		public ICollection<KitLinkGroup> KitLinkGroup { get; set; }
        public ICollection<KitLocale> KitLocale { get; set; }
    }
}
