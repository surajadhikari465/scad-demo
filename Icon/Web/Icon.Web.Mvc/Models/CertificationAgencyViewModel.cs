using Icon.Web.Attributes;
using Icon.Web.Common;
using Icon.Web.DataAccess.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Icon.Web.Mvc.Models
{
    public class CertificationAgencyViewModel : HierarchyClassViewModel
    {
        [IconPropertyValidation(ValidatorPropertyNames.CertificationAgencyName, CanBeNullOrEmprty = false)]
        [Required(ErrorMessage = "Please enter an agency name.")]
        [Display(Name = "Agency Name")]
        public string AgencyName { get; set; }

        public bool GlutenFree { get; set; }
        public bool Kosher { get; set; }
        public bool NonGMO { get; set; }
        public bool Organic { get; set; }
        public bool Vegan { get; set; }
        public bool DefaultOrganic { get; set; }

        public CertificationAgencyViewModel() { }

        public CertificationAgencyViewModel(CertificationAgencyModel agency)
        {
            base.HierarchyClassId = agency.HierarchyClassId;
            base.HierarchyId = agency.HierarchyId;
            base.HierarchyLevel = agency.HierarchyLevel;
            base.HierarchyParentClassId = agency.HierarchyParentClassId;
            AgencyName = agency.HierarchyClassName;
            GlutenFree = Convert.ToBoolean(Convert.ToInt16(agency.GlutenFree));
            Kosher = Convert.ToBoolean(Convert.ToInt16(agency.Kosher));
            NonGMO = Convert.ToBoolean(Convert.ToInt16(agency.NonGMO));
            Organic = Convert.ToBoolean(Convert.ToInt16(agency.Organic));
            Vegan = Convert.ToBoolean(Convert.ToInt16(agency.Vegan));
            DefaultOrganic = Convert.ToBoolean(Convert.ToInt16(agency.DefaultOrganic)); 
        }
    }
}