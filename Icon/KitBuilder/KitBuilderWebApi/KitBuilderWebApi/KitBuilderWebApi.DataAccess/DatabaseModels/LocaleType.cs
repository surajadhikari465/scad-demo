using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KitBuilderWebApi.DatabaseModels
{
    public partial class LocaleType
    {
        public LocaleType()
        {
            Locale = new HashSet<Locale>();
        }

        public int LocaleTypeId { get; set; }
        [StringLength(3, ErrorMessage = "Locale Type Code can have maximum length of 3.")]
        public string LocaleTypeCode { get; set; }
        [StringLength(255, ErrorMessage = "Locale Type Description can have maximum length of 255.")]
        public string LocaleTypeDesc { get; set; }

        public ICollection<Locale> Locale { get; set; }
    }
}
