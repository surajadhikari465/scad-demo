using Icon.Web.DataAccess.Models;
using System.ComponentModel.DataAnnotations;

namespace Icon.Web.Mvc.Models
{
    public class ContactTypeViewModel
    {
        public bool Archived { get; set; }
        public int ContactTypeId { get; set; }
        public string IsDisabled { get { return Archived ? "Enable" : "Disable"; }}

        [MaxLength(255)]
        [Display(Name = "Contact Name*")]
        [Required(ErrorMessage = "Contact name is required")]
        public string ContactTypeName { get; set; }

        public ContactTypeViewModel(){}

        public ContactTypeViewModel(ContactTypeModel model)
        {
            Archived = model.Archived;
            ContactTypeId = model.ContactTypeId;
            ContactTypeName = model.ContactTypeName;
        }
    }
}