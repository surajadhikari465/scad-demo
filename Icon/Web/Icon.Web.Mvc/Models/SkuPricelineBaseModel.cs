using System;
using System.ComponentModel.DataAnnotations;

namespace Icon.Web.Mvc.Models
{
    public class SkuPricelineBaseModel
    {
        [Display(Name = "Created Date")]
        public string CreatedDate { get; set; }

        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        [Display(Name = "Last Modified Date")]
        public string LastModifiedDate { get; set; }

        [Display(Name = "Created Date")]
        public string LastModifiedBy { get; set; }
    }
}