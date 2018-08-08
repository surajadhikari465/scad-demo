using Icon.Web.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Icon.Web.Mvc.Models
{
    public class VenueViewModel
    {
        public VenueViewModel() { }
        public int LocaleId { get; set; }

        [Display(Name = "Venue Name")]
        [HierarchyClassName]
        [Required]
        public string LocaleName { get; set; }
        public int? ParentLocaleId { get; set; }
        public int? LocaleTypeId { get; set; }

        [Display(Name = "Open Date")]
        [DataType(DataType.Date)]
        public DateTime? OpenDate { get; set; }

        [Display(Name = "Close Date")]
        [DataType(DataType.Date)]
        public DateTime? CloseDate { get; set; }
        public string LocaleSubType { get; set; }

        [Display(Name = "Venue Code")]
        [StringLength(255, ErrorMessage = "The Venue Code value cannot exceed 255 characters. ")]
        public string VenueCode { get; set; }

        [Display(Name = "Venue Occupant")]
        [StringLength(255, ErrorMessage = "The Venue Occupant value cannot exceed 255 characters. ")]
        public string VenueOccupant { get; set; }

        [Required(ErrorMessage = "Subtype is required.")]
        public int LocaleSubTypeId { get; set; }
        public string UserName { get; set; }
        public int OwnerOrgPartyId { get; set; }

        [Display(Name = "Parent Locale")]
        public string ParentLocaleName { get; set; }

        [Display(Name = "SubType")]
        [Editable(false)]
        public IEnumerable<SelectListItem> LocaleSubTypes { get; set; }

    }
}