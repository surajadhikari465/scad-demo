using FluentValidation;
using Icon.Common.DataAccess;
using Icon.Web.Mvc.Models;
using Icon.Web.Mvc.Utility;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Icon.Common;
using Icon.Common.Models;
using System;
using Icon.Web.DataAccess.Queries;
using Icon.Common.Validators.ItemAttributes;

namespace Icon.Web.Mvc.Validators
{
    public class SkuEditViewModelValidator : AbstractValidator<SkuEditViewModel>
    {
        private IQueryHandler<GetAttributeByNameGroupParameters, AttributeModel> getAttributesQueryHandler;
        private IAttributesHelper attributesHelper;
        private const string skuDescriptionAttributeName = "SKUDescription";
        private const string sku = "Sku";

        public SkuEditViewModelValidator(
             IQueryHandler<GetAttributeByNameGroupParameters, AttributeModel> getAttributesQueryHandler,
             IAttributesHelper attributesHelper
            )
        {
            this.getAttributesQueryHandler = getAttributesQueryHandler;
            this.attributesHelper = attributesHelper;

            RuleFor(vm => vm.SkuDescription)
                .Must((vm, n) => IsValidSkuDescription(vm))
                .WithMessage(ValidationMessages.InvalidSkuDescriptionValue);
        }

        private bool IsValidSkuDescription(SkuEditViewModel viewModel)
        {
            if (String.IsNullOrEmpty(viewModel.SkuDescription))
            {
                return false;
            }

            var attributeModel = getAttributesQueryHandler.Search(new GetAttributeByNameGroupParameters { AttributeName = skuDescriptionAttributeName, AttributeGroupName =sku });
            ItemAttributesTextValidator itemAttributesTextValidator = new ItemAttributesTextValidator(attributeModel);

            ItemAttributesValidationResult result = itemAttributesTextValidator.Validate(viewModel.SkuDescription);

            if (result.IsValid)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}