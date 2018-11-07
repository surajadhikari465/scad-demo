using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KitBuilder.DataAccess.Dto
{
    public partial class LinkGroupDto
    {
        public LinkGroupDto()
        {
            LinkGroupItemDto = new HashSet<LinkGroupItemDto>();
        }

        public int LinkGroupId { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Group Name can have maximum length of 100.")]
        public string GroupName { get; set; }
        [Required]
        [StringLength(500, ErrorMessage = "Group Description can have maximum length of 500.")]
        public string GroupDescription { get; set; }
        public DateTime InsertDateUtc { get; set; }        
        public ICollection<LinkGroupItemDto> LinkGroupItemDto { get; set; }
    }
}