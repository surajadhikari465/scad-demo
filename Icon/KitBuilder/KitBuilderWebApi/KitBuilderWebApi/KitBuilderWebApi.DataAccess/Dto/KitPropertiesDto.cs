﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace KitBuilderWebApi.DataAccess.Dto
{
    public class KitPropertiesDto
    {
        public int KitId { get; set; }
        [Required]
        public int ItemId { get; set; }
        [StringLength(255, ErrorMessage = "Description can have maximum length of 255.")]
        public string Description { get; set; }
        public int LocaleId { get; set; }
        public int LocaleIdAtWhichKitExists { get; set; }
        public int KitLocaleId { get; set; }

        public List<PropertiesDto> KitLinkGroupLocaleList{ get; set; }
        public List<PropertiesDto> KitLinkGroupItemLocaleList { get; set; }
    }
}