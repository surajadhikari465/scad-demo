using FluentValidation;
using Icon.Common.DataAccess;
using Icon.Framework;
using Infor.Services.NewItem.Cache;
using Infor.Services.NewItem.Constants;
using Infor.Services.NewItem.Models;
using Infor.Services.NewItem.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infor.Services.NewItem.Validators
{
    public class NewItemModelCollectionValidator : AbstractValidator<NewItemModel>, ICollectionValidator<NewItemModel>
    {
        private IQueryHandler<GetItemIdsQuery, Dictionary<string, int>> getItemIdsQueryHandler;
        private IIconCache iconCache;

        public NewItemModelCollectionValidator(IQueryHandler<GetItemIdsQuery, Dictionary<string, int>> getItemIdsQueryHandler,
            IIconCache iconCache)
        {
            this.getItemIdsQueryHandler = getItemIdsQueryHandler;
            this.iconCache = iconCache;

            RuleFor(i => i.IconBrandId)
                .Must(b => BrandExistsInIcon(b))
                .WithErrorCode(ApplicationErrors.Codes.InvalidBrand)
                .WithMessage((i) => string.Format(ApplicationErrors.Details.InvalidBrand, i.BrandName, i.IconBrandId));
            RuleFor(i => i.TaxClassCode)
                .Must(tc => TaxClassExistsInIcon(tc))
                .WithErrorCode(ApplicationErrors.Codes.InvalidTaxClassCode)
                .WithMessage(ApplicationErrors.Details.InvalidTaxClassCode);
            RuleFor(i => i.NationalClassCode)
                .Must(nc => NationalClassExistsInIcon(nc))
                .WithErrorCode(ApplicationErrors.Codes.InvalidNationalClassCode)
                .WithMessage(ApplicationErrors.Details.InvalidNationalClassCode);
            RuleFor(i => i.ItemDescription)
                .Matches(TraitPatterns.ProductDescription)
                .WithErrorCode(ApplicationErrors.Codes.InvalidProductDescription)
                .WithMessage(ApplicationErrors.Details.InvalidProductDescription);
            RuleFor(i => i.PosDescription)
                .Matches(TraitPatterns.PosDescription)
                .WithErrorCode(ApplicationErrors.Codes.InvalidPosDescription)
                .WithMessage(ApplicationErrors.Details.InvalidPosDescription);
            RuleFor(i => i.RetailUom)
                .NotEmpty()
                .WithErrorCode(ApplicationErrors.Codes.InvalidRetailUom)
                .WithMessage(ApplicationErrors.Details.InvalidRetailUom)
                .Must(i => UomCodes.ByName.Values.Contains(i))
                .WithErrorCode(ApplicationErrors.Codes.InvalidRetailUom)
                .WithMessage(ApplicationErrors.Details.InvalidRetailUom);
            RuleFor(i => i.CustomerFriendlyDescription)
                .Length(0, LengthConstants.CustomerFriendlyDescriptionMaxLength)
                .WithErrorCode(ApplicationErrors.Codes.InvalidCustomerFriendlyDescription)
                .WithMessage(ApplicationErrors.Details.InvalidCustomerFriendlyDescription);
        }

        public CollectionValidatorResult<NewItemModel> ValidateCollection(IEnumerable<NewItemModel> collection)
        {
            ValidatePlu(collection);

            foreach(var item in collection.Where(i => i.ErrorCode == null))
            {
                var newItemValidationResult = Validate(item);
                if(!newItemValidationResult.IsValid)
                {
                    item.ErrorCode = newItemValidationResult.Errors.First().ErrorCode;
                    item.ErrorDetails = newItemValidationResult.Errors.First().ErrorMessage;
                }
            }

            return new CollectionValidatorResult<NewItemModel>()
            {
                ValidEntities = collection.Where(i => i.ErrorCode == null),
                InvalidEntities = collection.Where(i => i.ErrorCode != null)
            }; ;
        }

        private void ValidatePlu(IEnumerable<NewItemModel> collection)
        {
            var pluItems = collection
                            .Where(i => i.ErrorCode == null && IsPluItem(i))
                            .Distinct()
                            .ToList();

            if (pluItems.Any())
            {
                var itemsExistingInIcon = getItemIdsQueryHandler.Search(new GetItemIdsQuery
                {
                    ScanCodes = pluItems.Select(i => i.ScanCode).ToList()
                });

                foreach (var item in pluItems.Where(i => !itemsExistingInIcon.ContainsKey(i.ScanCode)))
                {
                    item.ErrorCode = ApplicationErrors.Codes.PluDoesNotExistInIconError;
                    item.ErrorDetails = ApplicationErrors.Details.PluDoesNotExistInIconError;
                }
            }
        }

        private bool IsPluItem(NewItemModel item)
        {
            if (item.IsRetailSale)
            {
                if (item.ScanCode.Length < 7)
                {
                    return true;
                }
                else if (item.ScanCode.Length == 11 && item.ScanCode.StartsWith("2") && item.ScanCode.EndsWith("00000"))
                {
                    return true;
                }
            }
            else if (item.ScanCode.Length == 11 && (item.ScanCode.StartsWith("46") || item.ScanCode.StartsWith("48")))
            {
                return true;
            }

            return false;
        }

        private bool NationalClassExistsInIcon(string nationalClassCode)
        {
            if (string.IsNullOrEmpty(nationalClassCode) || !iconCache.NationalClassModels.ContainsKey(nationalClassCode))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool TaxClassExistsInIcon(string taxClassCode)
        {
            if (string.IsNullOrEmpty(taxClassCode) || !iconCache.TaxDictionary.ContainsKey(taxClassCode))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool BrandExistsInIcon(int? iconBrandId)
        {
            if (!iconBrandId.HasValue || iconBrandId < 0 || !iconCache.BrandDictionary.ContainsKey(iconBrandId.Value))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
