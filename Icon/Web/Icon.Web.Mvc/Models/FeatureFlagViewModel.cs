using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Icon.Web.Attributes;

namespace Icon.Web.Mvc.Models
{
    public class FeatureFlagViewModel
    {
        [ReadOnly(true)]
        [Display(Name = "Feature Flag Id")]
        public int FeatureFlagId { get; set; }

        [Required]
        [Display(Name = "Name")]
        [MaxLength(255, ErrorMessage = "Name must be 255 characters or less")]
        public string FlagName { get; set; }

        [Required]
        [Display(Name = "Enabled")]
        public Boolean Enabled { get; set; }

        [Required]
        [Display(Name = "Description")]
        [MaxLength(255, ErrorMessage = "Description must be 255 characters or less")]
        public string Description { get; set; }

        [ReadOnly(true)]
        [Display(Name = "Created Date")]
        public DateTime? CreatedDateUtc { get; set; }

        [ReadOnly(true)]
        [Display(Name = "Last Modified Date")]
        public DateTime? LastModifiedDateUtc { get; set; }

        [ReadOnly(true)]
        [Display(Name = "Last Modified By")]
        public string LastModifiedBy { get; set; }

    }
}