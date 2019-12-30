using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Icon.Web.DataAccess.Models;
using Icon.Web.Common;

namespace Icon.Web.Mvc.Models
{
    public class ContactViewModel
    {
        public int ContactId { get; set; }
        public int HierarchyClassId { get; set; }
        public string HierarchyName { get; set; }
        public string HierarchyClassName { get; set; }
        public List<ContactTypeViewModel> ContactTypes { get; set; }

        [Display(Name = "Contact Type*")]
        public int ContactTypeId { get; set; }
        
        [Display(Name = "Contact Name*")]
        public string ContactName { get; set; }
        
        [Display(Name = "Email*")]
        public string Email { get; set; }

        [Display(Name = "Contact Type Name*")]
        public string ContactTypeName { get; set; }

        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Address Line 1")]
        public string AddressLine1 { get; set; }

        [Display(Name = "Address Line 2")]
        public string AddressLine2 { get; set; }

        [Display(Name = "City")]
        public string City { get; set; }

        [Display(Name = "State")]
        public string State { get; set; }

        [Display(Name = "ZipCode")]
        public string ZipCode { get; set; }

        [Display(Name = "Country")]
        public string Country { get; set; }

        [Display(Name = "Phone Number 1")]
        public string PhoneNumber1 { get; set; }

        [Display(Name = "Phone Number 2")]
        public string PhoneNumber2 { get; set; }

        [Display(Name = "Website URL")]
        public string WebsiteURL { get; set; }

        public ContactViewModel(){}

        public ContactViewModel(ContactModel model)
        {
            ContactId = model.ContactId;
            ContactTypeId = model.ContactTypeId;
            HierarchyClassId = model.HierarchyClassId;
            ContactTypeName = model.ContactTypeName;
            ContactName = model.ContactName;
            Email = model.Email;
            Title = model.Title;
            AddressLine1 = model.AddressLine2;
            AddressLine2 = model.AddressLine2;
            City = model.City;
            State = model.State;
            ZipCode = model.ZipCode;
            Country = model.Country;
            PhoneNumber1 = model.PhoneNumber1;
            PhoneNumber2 = model.PhoneNumber2;
            WebsiteURL = model.WebsiteURL;
            HierarchyName = model.HierarchyName;
            HierarchyClassName = model.HierarchyClassName;
        }
    }
}