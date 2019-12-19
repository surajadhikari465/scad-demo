using Icon.Framework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Icon.Web.Mvc.Models
{
    public class TraitGroupSearchViewModel
    {
        [Display(Name = "Trait Group")]
        public int SelectedTraitGroupCode { get; set; }
        public SelectList TraitGroups { get; set; }
        public string Results { get; set; }
        public List<TraitGroup> TraitGroupList { get; set; }
        public IEnumerable<TraitGroupSearchViewModel> Traits { get; set; }
    }
}