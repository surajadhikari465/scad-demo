using FluentValidation;
using Icon.Web.Mvc.Models;

namespace Icon.Web.Mvc.Validators
{
    public class ContactViewModelValidator : AbstractValidator<ContactViewModel>
    {
        public ContactViewModelValidator( )
        {
            RuleFor(vm => vm.ContactTypeId)
                .NotEmpty()
                .WithMessage("Contact Type is required.");

            RuleFor(vm => vm.ContactName)
                .NotEmpty()
                .MaximumLength(255)
                .WithMessage("Contact Name is required.");

            RuleFor(vm => vm.Email)
                .NotEmpty()
                .MaximumLength(255)
                .WithMessage("Contact Email is required.");
            
            RuleFor(vm => vm.Title)
                .MaximumLength(255)
                .WithMessage("Title can't exceed 255 chraracters.");

            RuleFor(vm => vm.AddressLine1)
                .MaximumLength(255)
                .WithMessage("Address can't exceed 255 chraracters.");

            RuleFor(vm => vm.AddressLine2)
                .MaximumLength(255)
                .WithMessage("Address can't exceed 255 chraracters.");

            RuleFor(vm => vm.City)
                .MaximumLength(255)
                .WithMessage("City can't exceed 255 chraracters.");

            RuleFor(vm => vm.State)
                .MaximumLength(255)
                .WithMessage("State can't exceed 255 chraracters.");

            RuleFor(vm => vm.ZipCode)
                .MaximumLength(15)
                .WithMessage("Zip code can't exceed 15 chraracters.");

            RuleFor(vm => vm.Country)
                .MaximumLength(255)
                .WithMessage("Country can't exceed 255 chraracters.");

            RuleFor(vm => vm.PhoneNumber1)
                .MaximumLength(30)
                .WithMessage("Phone number can't exceed 255 chraracters.");

            RuleFor(vm => vm.PhoneNumber2)
                .MaximumLength(30)
                .WithMessage("PhoneNumber can't exceed 255 chraracters.");

            RuleFor(vm => vm.WebsiteURL)
                .MaximumLength(255)
                .WithMessage("Website can't exceed 255 chraracters.");
        }
    }
}