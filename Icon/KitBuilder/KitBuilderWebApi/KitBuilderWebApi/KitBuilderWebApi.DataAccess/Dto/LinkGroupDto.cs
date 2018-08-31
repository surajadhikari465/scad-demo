using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KitBuilderWebApi.DatabaseModels
{
    public partial class LinkGroupDto
    {
        public LinkGroupDto()
        {
            KitLinkGroup = new HashSet<KitLinkGroup>();
            LinkGroupItem = new HashSet<LinkGroupItem>();
        }

        public int LinkGroupId { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Group Name can have maximum length of 100.")]
        public string GroupName { get; set; }
        [Required]
        [StringLength(500, ErrorMessage = "Group Description can have maximum length of 500.")]
        public string GroupDescription { get; set; }
        public DateTime InsertDate { get; set; }

        public ICollection<KitLinkGroup> KitLinkGroup { get; set; }
        public ICollection<LinkGroupItem> LinkGroupItem { get; set; }
    }
}
