using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KitBuilderWebApi.DatabaseModels
{
    public partial class LinkGroupItem
    {
        public LinkGroupItem()
        {
            KitLinkGroupItem = new HashSet<KitLinkGroupItem>();
        }

        public int LinkGroupItemId { get; set; }
        [Required]
        public int LinkGroupId { get; set; }
        [Required]
        public int ItemId { get; set; }
        public int? InstructionListId { get; set; }
        public DateTime InsertDate { get; set; }

        public InstructionList InstructionList { get; set; }
        public Items Item { get; set; }
        public LinkGroup LinkGroup { get; set; }
        public ICollection<KitLinkGroupItem> KitLinkGroupItem { get; set; }
    }
}
