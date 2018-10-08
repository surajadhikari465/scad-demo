using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KitBuilder.DataAccess.Dto
{
    public class LocaleTypeDto
    {
        public LocaleTypeDto()
        {
            Locale = new HashSet<LocaleDto>();
        }

        public int LocaleTypeId { get; set; }
        [StringLength(3, ErrorMessage = "Locale Type Code can have maximum length of 3.")]
        public string LocaleTypeCode { get; set; }
        [StringLength(255, ErrorMessage = "Locale Type Description can have maximum length of 255.")]
        public string LocaleTypeDesc { get; set; }

        public ICollection<LocaleDto> Locale { get; set; }
    }
}