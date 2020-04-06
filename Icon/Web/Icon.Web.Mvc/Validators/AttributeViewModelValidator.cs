using FluentValidation;
using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Models;
using Icon.Web.Mvc.Utility;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Icon.Common;
using Icon.Common.Models;
using System;
using Icon.Web.DataAccess.Infrastructure.ItemSearch;

namespace Icon.Web.Mvc.Validators
{
    public class AttributeViewModelValidator : AbstractValidator<AttributeViewModel>
    {
        private const string SPECIFIC_SPECIAL_CHARACTER_SET = "Specific";

        private IQueryHandler<EmptyQueryParameters<IEnumerable<AttributeModel>>, IEnumerable<AttributeModel>> getAttributesQueryHandler;
        private IQueryHandler<GetItemsParameters, GetItemsResult> getItemsQueryHandler;
        private IQueryHandler<GetAttributeByAttributeIdParameters, AttributeModel> getAttributeByAttributeIdQueryHandler;
        private IQueryHandler<DoesAttributeExistOnItemsParameters, bool> doesAttributeExistOnItemsQueryHandler;
        private IAttributesHelper attributesHelper;

        public AttributeViewModelValidator(
            IQueryHandler<EmptyQueryParameters<IEnumerable<AttributeModel>>, IEnumerable<AttributeModel>> getAttributesQueryHandler,
            IQueryHandler<GetItemsParameters, GetItemsResult> getItemsQueryHandler,
            IQueryHandler<GetAttributeByAttributeIdParameters, AttributeModel> getAttributeByAttributeIdQueryHandler,
            IQueryHandler<DoesAttributeExistOnItemsParameters, bool> doesAttributeExistOnItemsQueryHandler,
            IAttributesHelper attributesHelper)
        {
            this.getAttributesQueryHandler = getAttributesQueryHandler;
            this.getItemsQueryHandler = getItemsQueryHandler;
            this.getAttributeByAttributeIdQueryHandler = getAttributeByAttributeIdQueryHandler;
            this.doesAttributeExistOnItemsQueryHandler = doesAttributeExistOnItemsQueryHandler;
            this.attributesHelper = attributesHelper;

            RuleFor(vm => vm.TraitCode)
                .NotEmpty();

            RuleFor(vm => vm.TraitCode)
                .MaximumLength(3);

            RuleFor(vm => vm.TraitCode)
                .Must((vm, n) => IsUniqueTraitCode(vm))
                .WithMessage(ValidationMessages.TraitCodeUnique);

            RuleFor(vm => vm.XmlTraitDescription)
               .Must((vm, n) => IsUniqueXmlTraitDescription(vm))
               .WithMessage(ValidationMessages.XmlTraitDescriptionUnique);

            RuleFor(vm => vm.DisplayName)
                .NotEmpty();

            RuleFor(vm => vm.DisplayName)
                .MaximumLength(255);

            RuleFor(vm => vm.DisplayName)
                .Must((vm, n) => IsUniqueDisplayName(vm))
                .WithMessage(ValidationMessages.DisplayNameUnique)
                .When(vm => !string.IsNullOrWhiteSpace(vm.DisplayName));

            RuleFor(vm => vm.DefaultValue)
                .Must((vm, n) => IsValidDefault(vm))
                .WithMessage(ValidationMessages.InvalidDefaultValue);

            When(vm => vm.DataTypeId == (int)DataType.Number, () =>
            {
                RuleFor(vm => vm.NumberOfDecimals)
                    .NotEmpty();

                RuleFor(vm => vm.NumberOfDecimals)
                    .Must(i => int.TryParse(i, out _))
                    .WithMessage(ValidationMessages.NumberOfDecimalsValidInteger)
                    .When(vm => !string.IsNullOrWhiteSpace(vm.NumberOfDecimals));

                When(vm => int.TryParse(vm.NumberOfDecimals, out _), () =>
                {
                    RuleFor(vm => int.Parse(vm.NumberOfDecimals))
                        .InclusiveBetween(Constants.NumberOfDecimalsMin, Constants.NumberOfDecimalsMax)
                        .WithName(nameof(AttributeViewModel.NumberOfDecimals))
                        .WithMessage(ValidationMessages.NumberOfDecimalsInclusiveBetween);

                    RuleFor(vm => int.Parse(vm.NumberOfDecimals))
                        .Must((vm, d) => IsValidNumberOfDigitsAfterDecimals(decimal.Parse(vm.MaximumNumber), d))
                        .WithMessage(ValidationMessages.ValidNumberOfDigits)
                        .WithName(nameof(AttributeViewModel.NumberOfDecimals))
                        .When(vm => decimal.TryParse(vm.MaximumNumber, out _));

                    RuleFor(vm => int.Parse(vm.NumberOfDecimals))
                        .Must((vm, d) => IsValidNumberOfDigitsAfterDecimals(decimal.Parse(vm.MinimumNumber), d))
                        .WithMessage(ValidationMessages.ValidNumberOfDigits)
                        .WithName(nameof(AttributeViewModel.NumberOfDecimals))
                        .When(vm => decimal.TryParse(vm.MinimumNumber, out _));

                    RuleFor(vm => int.Parse(vm.NumberOfDecimals))
                        .Must((vm, n) => IsNumberOfDecimalsGreaterthanCurrentValue(vm))
                        .WithMessage(ValidationMessages.CurrentNumberOfDecimalsGreaterThanNewNumberOfDecimals)
                        .WithName(nameof(AttributeViewModel.NumberOfDecimals))
                        .When(vm => vm.Action == ActionEnum.Update);
                });

                RuleFor(vm => vm.MaximumNumber)
                    .NotEmpty();

                RuleFor(vm => vm.MaximumNumber)
                    .Must(n => decimal.TryParse(n, out _))
                    .When(vm => !string.IsNullOrWhiteSpace(vm.MaximumNumber))
                    .WithMessage(ValidationMessages.MaximumNumberValidDecimal);

                When(vm => decimal.TryParse(vm.MaximumNumber, out _), () =>
                {
                    RuleFor(a => decimal.Parse(a.MaximumNumber))
                        .GreaterThanOrEqualTo(a => decimal.Parse(a.MinimumNumber))
                        .WithMessage(ValidationMessages.MaximumNumberGreaterThanMinimumNumber)
                        .When(vm => decimal.TryParse(vm.MinimumNumber, out _));

                    RuleFor(vm => decimal.Parse(vm.MaximumNumber))
                        .InclusiveBetween(Constants.MaximumNumberMin, Constants.MaximumNumberMax)
                        .WithMessage(ValidationMessages.MaximumNumberInclusiveBetween);

                    RuleFor(vm => decimal.Parse(vm.MaximumNumber))
                        .Must((vm, n) => IsMaximumNumberGreaterthanCurrentValue(vm))
                        .WithMessage(ValidationMessages.CurrentMaximumNumberIsGreaterThanNewMaximumNumber)
                        .WithName(nameof(AttributeViewModel.MaximumNumber))
                        .When(vm => vm.Action == ActionEnum.Update);
                });

                RuleFor(vm => vm.MinimumNumber)
                    .NotEmpty();

                RuleFor(vm => vm.MinimumNumber)
                    .Must(n => decimal.TryParse(n, out _))
                    .When(vm => !string.IsNullOrWhiteSpace(vm.MinimumNumber))
                    .WithMessage(ValidationMessages.MinimumNumberValidDecimal);

                When(vm => decimal.TryParse(vm.MinimumNumber, out _), () =>
                {
                    RuleFor(vm => decimal.Parse(vm.MinimumNumber))
                        .InclusiveBetween(Constants.MinimumNumberMin, Constants.MinimumNumberMax)
                        .WithMessage(ValidationMessages.MinimumNumberInclusiveBetween);

                    RuleFor(vm => decimal.Parse(vm.MinimumNumber))
                        .Must((vm, n) => IsMinimumNumberLessthanCurrentValue(vm))
                        .WithMessage(ValidationMessages.CurrentMinimumNumberMustBeLessThanNewMinimumNumber)
                        .When(vm => vm.Action == ActionEnum.Update);
                });
            });

            When(vm => vm.DataTypeId == (int)DataType.Text, () =>
            {
                RuleFor(vm => vm.AvailableCharacterSets)
                    .Must(cs => cs.Any(s => s.IsSelected))
                    .WithMessage(ValidationMessages.SelectCharacterSets);

                RuleFor(vm => vm.MaxLengthAllowed)
                    .NotNull()
                    .InclusiveBetween(Constants.MaxLengthAllowedMin, Constants.MaxLengthAllowedMax);

                RuleFor(vm => vm.MaxLengthAllowed)
                    .Must((vm, n) => IsMaxLengthAllowedGreaterthanCurrentValue(vm))
                    .WithMessage(ValidationMessages.CurrentMaxLengthLessThanNewMaxLength)
                    .When(vm => vm.Action == ActionEnum.Update);

                RuleFor(vm => vm.SpecialCharactersAllowed)
                    .NotEmpty()
                    .When(vm => vm.IsSpecialCharactersSelected && vm.SpecialCharacterSetSelected == SPECIFIC_SPECIAL_CHARACTER_SET);

                When(vm => vm.IsPickList, () =>
                {
                    RuleFor(vm => vm.MaxLengthAllowed)
                        .NotNull()
                        .InclusiveBetween(Constants.MaxLengthAllowedMin, 50);

                    When(vm => vm.Action == ActionEnum.Update && IsTextAttributeChangingToPickList(vm), () =>
                    {
                        RuleFor(vm => vm)
                            .Must(vm => !DoesAttributeExistOnItems(vm))
                            .WithMessage(ValidationMessages.AttributeExistsOnItemsWhenSwitchingToPickListError);
                    });
                    RuleFor(vm => vm.PickListData)
                        .Must(pld => pld.Any(pl => pl != null && !pl.IsPickListSelectedForDelete && !string.IsNullOrWhiteSpace(pl.PickListValue)))
                        .WithMessage(ValidationMessages.PickListCollectionIsEmpty)
                        .DependentRules(() =>
                        {
                            RuleFor(vm => vm.PickListData)
                                .Must(pld => pld.Any(pl => !string.IsNullOrEmpty(pl?.PickListValue)))
                                .WithMessage(ValidationMessages.PickListValueIsEmpty)
                                .DependentRules(() =>
                                {
                                    RuleFor(vm => vm.PickListData)
                                        .Must((vm, pld) => pld
                                            .Where(pl => !pl.IsPickListSelectedForDelete)
                                            .Where(pl => !string.IsNullOrWhiteSpace(pl.PickListValue))
                                            .All(pl => pl.PickListValue.Length <= vm.MaxLengthAllowed))
                                        .WithMessage(ValidationMessages.PickListValueMaxLengthAllowed);

                                    RuleFor(vm => vm.PickListData)
                                        .Must((vm, pld) => ValidatePickList(
                                            vm,
                                            vm.AvailableCharacterSets,
                                            vm.PickListData
                                                .Where(p => !p.IsPickListSelectedForDelete)
                                                .ToList()))
                                        .WithMessage(ValidationMessages.PickListInvalidValue);

                                    RuleFor(vm => vm.PickListData)
                                        .Must(pld => pld.Any(pl => !pl.IsPickListSelectedForDelete))
                                        .WithMessage(ValidationMessages.PickListMustHaveOneNonDeletedPickListValue)
                                        .When(vm => vm.Action == ActionEnum.Update);

                                    RuleFor(vm => vm.PickListData)
                                        .Must((vm, pl) => DoItemsHavePickListValues(vm, pl))
                                        .WithMessage(ValidationMessages.ValidatePickListValue);
                                });
                        });
                });
            });
        }

        private bool IsUniqueTraitCode(AttributeViewModel attributeViewModel)
        {
            var attributes = getAttributesQueryHandler.Search(new EmptyQueryParameters<IEnumerable<AttributeModel>>());
            return attributes
                .Where(a => a.AttributeId != attributeViewModel.AttributeId)
                .All(a => a.TraitCode != attributeViewModel.TraitCode);
        }

        // attribute DisplayName is stored in xmltraitdescription field
        private bool IsUniqueXmlTraitDescription(AttributeViewModel attributeViewModel)
        {
            var attributes = getAttributesQueryHandler.Search(new EmptyQueryParameters<IEnumerable<AttributeModel>>());
            return attributes
                .Where(a => a.AttributeId != attributeViewModel.AttributeId)
                .All(a => a.XmlTraitDescription != attributeViewModel.DisplayName);
        }

        private bool IsUniqueDisplayName(AttributeViewModel attributeViewModel)
        {
            var attributes = getAttributesQueryHandler.Search(new EmptyQueryParameters<IEnumerable<AttributeModel>>());
            return attributes
                .Where(a => a.AttributeId != attributeViewModel.AttributeId)
                .All(a => a.DisplayName != attributeViewModel.DisplayName);
        }

        private bool IsMinimumNumberLessthanCurrentValue(AttributeViewModel viewModel)
        {
            var attribute = getAttributeByAttributeIdQueryHandler.Search(new GetAttributeByAttributeIdParameters { AttributeId = viewModel.AttributeId });

            if (decimal.Parse(attribute.MinimumNumber) >= decimal.Parse(viewModel.MinimumNumber))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        private bool IsMaximumNumberGreaterthanCurrentValue(AttributeViewModel viewModel)
        {
            var attribute = getAttributeByAttributeIdQueryHandler.Search(new GetAttributeByAttributeIdParameters { AttributeId = viewModel.AttributeId });

            if (decimal.Parse(attribute.MaximumNumber) <= decimal.Parse(viewModel.MaximumNumber))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool IsNumberOfDecimalsGreaterthanCurrentValue(AttributeViewModel viewModel)
        {
            var attribute = getAttributeByAttributeIdQueryHandler.Search(new GetAttributeByAttributeIdParameters { AttributeId = viewModel.AttributeId });

            if (int.Parse(attribute.NumberOfDecimals) <= int.Parse(viewModel.NumberOfDecimals))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool IsMaxLengthAllowedGreaterthanCurrentValue(AttributeViewModel viewModel)
        {
            var attribute = getAttributeByAttributeIdQueryHandler.Search(new GetAttributeByAttributeIdParameters { AttributeId = viewModel.AttributeId });

            if (attribute.MaxLengthAllowed <= viewModel.MaxLengthAllowed)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool ValidatePickList(AttributeViewModel attributeModel, List<CharacterSetModel> characterSetModelList, List<PickListModel> pickListModelList)
        {
            var characterSetRegexPattern = attributesHelper.CreateCharacterSetRegexPattern(
                        attributeModel.DataTypeId,
                        attributeModel.AvailableCharacterSets,
                        attributeModel.IsSpecialCharactersSelected
                            ? (attributeModel.SpecialCharacterSetSelected == Constants.SpecialCharactersAll)
                                ? Constants.SpecialCharactersAll
                                : attributeModel.SpecialCharactersAllowed
                            : null);

            if (characterSetRegexPattern == null)
            {
                return true;
            }
            else
            {
                return pickListModelList.Where(x => !string.IsNullOrEmpty(x.PickListValue))
                    .All(p => Regex.IsMatch(p.PickListValue, characterSetRegexPattern)
                        && p.PickListValue.Length <= Constants.MaxPickListValueLength);
            }
        }

        private bool IsValidNumberOfDigitsAfterDecimals(decimal? number, int? numberOfDecimals)
        {
            string numberConvertedToString = number.ToString();

            if (numberConvertedToString.Contains("."))
            {
                int numberOfDigitsAfterDecimals = numberConvertedToString.Substring(numberConvertedToString.IndexOf(".") + 1).Length;
                if (numberOfDigitsAfterDecimals > numberOfDecimals)
                {
                    return false;
                }

            }
            return true;
        }

        private bool DoItemsHavePickListValues(AttributeViewModel attributeViewModel, List<PickListModel> pickListModelList)
        {
            var attribute = getAttributeByAttributeIdQueryHandler.Search(
                new GetAttributeByAttributeIdParameters() { AttributeId = attributeViewModel.AttributeId });

            GetItemsParameters parameters = new GetItemsParameters();

            foreach (var pickListModel in pickListModelList.Where(p => p.IsPickListSelectedForDelete == true))
            {
                var pickListValue = pickListModel.PickListValue;

                parameters.ItemAttributeJsonParameters = new List<ItemSearchCriteria>()
                {
                    new ItemSearchCriteria(attribute.AttributeName, AttributeSearchOperator.ExactlyMatchesAll,pickListValue)
                };

                var results = getItemsQueryHandler.Search(parameters);

                //if any item exist with picklist value then return false
                if (results.TotalRecordsCount > 0)
                {
                    return false;
                }
            }
            return true;
        }

        private bool DoesAttributeExistOnItems(AttributeViewModel viewModel)
        {
            return doesAttributeExistOnItemsQueryHandler.Search(new DoesAttributeExistOnItemsParameters { AttributeId = viewModel.AttributeId });
        }

        private bool IsTextAttributeChangingToPickList(AttributeViewModel viewModel)
        {
            var attribute = getAttributeByAttributeIdQueryHandler.Search(new GetAttributeByAttributeIdParameters { AttributeId = viewModel.AttributeId });

            if (!attribute.IsPickList && viewModel.IsPickList)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool IsValidDefault(AttributeViewModel viewModel)
        {
            if (String.IsNullOrEmpty(viewModel.DefaultValue))
            {
                return true;
            }

            switch (viewModel.DataTypeId)
            {
                case (int)DataType.Date:
                    DateTime date;
                    return String.IsNullOrEmpty(viewModel.DefaultValue) || DateTime.TryParse(viewModel.DefaultValue, out date);
                case (int)DataType.Number:
                    int dec;
                    decimal min, max, dflt;

                    if (!Decimal.TryParse(viewModel.DefaultValue, out dflt)
                        || !Decimal.TryParse(viewModel.MinimumNumber, out min)
                        || !Decimal.TryParse(viewModel.MaximumNumber, out max)
                        || !int.TryParse(viewModel.NumberOfDecimals, out dec))
                    {
                        return false;
                    }

                    var ar = dflt.ToString().Split('.');
                    return dflt >= min && dflt <= max && (ar.Length == 1 ? 0 : ar[1].Length) <= dec;
                case (int)DataType.Text:
                    if (viewModel.IsPickList)
                    {
                        return viewModel.PickListData.Any(x => x.PickListValue == viewModel.DefaultValue);
                    }

                    var characterSetRegexPattern = attributesHelper.CreateCharacterSetRegexPattern(
                        viewModel.DataTypeId,
                        viewModel.AvailableCharacterSets,
                        viewModel.IsSpecialCharactersSelected
                            ? (viewModel.SpecialCharacterSetSelected == Constants.SpecialCharactersAll)
                                ? Constants.SpecialCharactersAll
                                : viewModel.SpecialCharactersAllowed
                            : null);
                    bool isValid = true;

                    if (characterSetRegexPattern != null)
                    {
                        isValid = Regex.IsMatch(viewModel.DefaultValue, characterSetRegexPattern);
                    }

                    if (isValid)
                    {
                        return (viewModel.DefaultValue.Length >= Constants.MaxLengthAllowedMin && viewModel.DefaultValue.Length <= Constants.MaxLengthAllowedMax)
                            && viewModel.DefaultValue.Length <= (viewModel.MaxLengthAllowed ?? 0);
                    }
                    else
                    {
                        return false;
                    }
                default:
                    return true;
            }
        }
    }
}