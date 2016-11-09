using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Icon.Web.Attributes;


namespace Icon.Web.Mvc.Models
{
    public class PluCategoryViewModel
    {
        [Required]
        [Display(Name = "PLU Category")]       
        [MaxLength(60, ErrorMessage = "PLU category name must be 60 characters or less")]
        public string PluCategoryName { get; set; }

        [Display(Name = "Start")]
        [RegularExpression(@"^\d{1,11}$", ErrorMessage = "Please enter only numbers")]
        [PluCategoryStart]
        public string BeginRange { get; set; }

        [Required]
        [Display(Name="End")]
        [RegularExpression(@"^\d{1,11}$", ErrorMessage = "Please enter only numbers")]       
        public string EndRange { get; set; }

        public int PluCategoryId { get; set; }

    }
}