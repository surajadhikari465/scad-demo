using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Icon.Framework;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Icon.Web.Mvc.Models
{
    public class TraitViewModel
    {
        public int TraitId { get; set; }
        public string TraitPattern { get; set; }
        [Display(Name = "Trait Description")]
        public string TraitDesc { get; set; }
        public int TraitGroupId { get; set; }
        [Display(Name = "Trait Group")]
        public string TraitGroupDescription { get; set; }

        public TraitViewModel(){ }

        public TraitViewModel(Trait trait)
        {
            TraitId = trait.traitID;
            TraitPattern = trait.traitPattern;
            TraitDesc = trait.traitDesc;
            TraitGroupId = trait.traitGroupID.HasValue ? trait.traitGroupID.Value : default(int);
            TraitGroupDescription = trait.TraitGroup.traitGroupDesc;
        }
    }
}