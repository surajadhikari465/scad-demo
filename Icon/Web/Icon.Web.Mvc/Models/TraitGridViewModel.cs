using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Icon.Framework;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Icon.Web.Mvc.Models
{
    public class TraitGridViewModel
    {
        [Display(Name = "Trait Group")]
        public int TraitGroupSelectedId { get; set; }
        public IEnumerable<SelectListItem> TraitGroup { get; set; }
        public List<TraitViewModel> Traits { get; set; }
    }
}
