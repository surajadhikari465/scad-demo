using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace KitBuilderWebApi.DataAccess.Dto
{
    public class KitLinkGroupDto
    {
        public KitLinkGroupDto()
        {
            KitLinkGroupLocale = new HashSet<KitLinkGroupLocaleDto>();
        }

        public int KitLinkGroupId { get; set; }
        [Required]
        public int KitId { get; set; }
        [Required]
        public int LinkGroupId { get; set; }
        public DateTime InsertDate { get; set; }

        public LinkGroupDto LinkGroup { get; set; }
        public ICollection<KitLinkGroupLocaleDto> KitLinkGroupLocale { get; set; }
    }
}
