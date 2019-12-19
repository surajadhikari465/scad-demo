using FluentValidation;
using Icon.Common.Validators.ItemAttributes;
using Icon.Web.Mvc.Models;

namespace Icon.Web.Mvc.Validators
{
    public class ItemEditViewModelValidator : AbstractValidator<ItemEditViewModel>
    {
        private IItemAttributesValidatorFactory factory;

        public ItemEditViewModelValidator(IItemAttributesValidatorFactory factory)
        {
            this.factory = factory;

            RuleFor(vm => vm.ItemViewModel.BrandsHierarchyClassId)
                .GreaterThan(0)
                .WithMessage("Brand is required. Please select a Brand.");
            RuleFor(vm => vm.ItemViewModel.MerchandiseHierarchyClassId)
                .GreaterThan(0)
                .WithMessage("Merchandise is required. Please select a Merchandise.");
            RuleFor(vm => vm.ItemViewModel.TaxHierarchyClassId)
                .GreaterThan(0)
                .WithMessage("Tax is required. Please select a Tax.");
            RuleFor(vm => vm.ItemViewModel.NationalHierarchyClassId)
                .GreaterThan(0)
                .WithMessage("National is required. Please select a National.");
            RuleFor(vm => vm.ItemViewModel.ItemAttributes)
                .ForEach(r => r.Custom((kvp, context) =>
                {
                    var attributeValidator = factory.CreateItemAttributesJsonValidator(kvp.Key);
                    var validationResult = attributeValidator.Validate(kvp.Value);
                    if (!validationResult.IsValid)
                    {
                        foreach (var errorMessage in validationResult.ErrorMessages)
                        {
                            context.AddFailure(kvp.Key, errorMessage);
                        }
                    }
                }));
        }
    }
}