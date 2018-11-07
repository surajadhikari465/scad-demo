using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KitBuilder.DataAccess.DatabaseModels
{
    public partial class LinkGroup
    {
        public LinkGroup()
        {
            KitLinkGroup = new HashSet<KitLinkGroup>();
            LinkGroupItem = new HashSet<LinkGroupItem>();
        }

        public int LinkGroupId { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Group Name can have maximum length of 100.")]
        public string GroupName { get; set; }
        [Required]
        [StringLength(20, ErrorMessage = "Group Description can have maximum length of 20.")]
        public string GroupDescription { get; set; }
		public DateTime InsertDateUtc { get; set; }
		public DateTime? LastUpdatedDateUtc { get; set; }

		public ICollection<KitLinkGroup> KitLinkGroup { get; set; }
        public ICollection<LinkGroupItem> LinkGroupItem { get; set; }
    }
}
