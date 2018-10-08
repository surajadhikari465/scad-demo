using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KitBuilder.DataAccess.Dto
{
    public class LocaleDto
    {
        public LocaleDto()
        {
        }

        public int LocaleId { get; set; }
        [StringLength(255, ErrorMessage = "Locale Name Description can have maximum length of 255.")]
        public string LocaleName { get; set; }
        [Required]
        public int LocaleTypeId { get; set; }
        public int? StoreId { get; set; }
        public int? MetroId { get; set; }
        public int? RegionId { get; set; }
        public int? ChainId { get; set; }
        public DateTime? LocaleOpenDate { get; set; }
        public DateTime? LocaleCloseDate { get; set; }
        public string RegionCode { get; set; }
        public int? BusinessUnitId { get; set; }
        public string StoreAbbreviation { get; set; }
        [StringLength(5, ErrorMessage = "Currency Code can have maximum length of 5.")]
        public string CurrencyCode { get; set; }
        public bool? Hospitality { get; set; }

        public LocaleTypeDto LocaleType { get; set; }
    }
}