using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MammothWebApi.Models
{
    public class StoreItem
    {
        [Required]
        [Range(minimum: 1, maximum: int.MaxValue)]
        public int BusinessUnitId { get; set; }

        [Required]
        [MaxLength(13)]
        public string ScanCode { get; set; }
    }
}