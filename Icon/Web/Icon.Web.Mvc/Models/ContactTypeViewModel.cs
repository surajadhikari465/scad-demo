using System.ComponentModel.DataAnnotations;

namespace Icon.Web.DataAccess.Models
{
    public class ContactTypeViewModel
    {
        public bool Archived { get; set; }
        public int ContactTypeId { get; set; }

        [MaxLength(255)]
        [Display(Name = "Contact Name")]
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