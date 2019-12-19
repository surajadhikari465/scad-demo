using FluentValidation;
using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Models;
using System.Collections.Generic;
using Icon.Common;
using Icon.Common.Validators.ItemAttributes;
using System.Linq;

namespace Icon.Web.Mvc.Validators
{

    /// <summary>
    /// Validates the ItemCreateViewModel for when a user creates an item.
    /// </summary>
    public class ItemCreateViewModelValidator : AbstractValidator<ItemCreateViewModel>
    {
        private IQueryHandler<DoesScanCodeExistParameters, bool> doesScanCodeExistQueryHandler;
        private IQueryHandler<GetBarcodeTypeParameters, List<BarcodeTypeModel>> getBarcodeTypeQueryHandler;
        private IItemAttributesValidatorFactory factory;
        private List<BarcodeTypeModel> barcodeTypeModelList;

        public ItemCreateViewModelValidator(
            IQueryHandler<DoesScanCodeExistParameters, bool> doesScanCodeExistQueryHandler,
            IQueryHandler<GetBarcodeTypeParameters, List<BarcodeTypeModel>> getBarcodeTypeQueryHandler,
            IItemAttributesValidatorFactory factory)
        {
            this.doesScanCodeExistQueryHandler = doesScanCodeExistQueryHandler;
            this.getBarcodeTypeQueryHandler = getBarcodeTypeQueryHandler;
            this.factory = factory;
            this.barcodeTypeModelList = getBarcodeTypeQueryHandler.Search(new GetBarcodeTypeParameters());

            RuleFor(vm => vm.BarcodeTypeId)
                .GreaterThan(0)
                .WithMessage("Barcode Type is required.");
            RuleFor(vm => vm.BrandHierarchyClassId)
                .GreaterThan(0)
                .WithMessage("Brand is required. Please select a Brand.");
            RuleFor(vm => vm.TaxHierarchyClassId)
                .GreaterThan(0)
                .WithMessage("Tax is required. Please select a Tax.");
            RuleFor(vm => vm.MerchandiseHierarchyClassId)
                .GreaterThan(0)
                .WithMessage("Merchandise is required. Please select a Merchandise.");
            RuleFor(vm => vm.NationalHierarchyClassId)
                .GreaterThan(0)
                .WithMessage("National is required. Please select a National.");
            this.When(vm => vm.BarcodeTypeId == this.barcodeTypeModelList.Where(b => b.BarcodeType.ToUpper() == "UPC").Select(s => s.BarcodeTypeId).FirstOrDefault(), () =>
            {
                RuleFor(vm => vm.ScanCode)
                    .Must(sc => !string.IsNullOrWhiteSpace(sc))
                    .WithMessage("Scan Code is required when choosing UPC.")
                    .Must(sc => !doesScanCodeExistQueryHandler.Search(new DoesScanCodeExistParameters { ScanCode = sc }))
                    .WithMessage(vm => $"'{vm.ScanCode}' is already associated to an item. Please use another scan code.")
                    .Must(sc => !IsUpcInBarcodeTypeRanges(sc))
                    .WithMessage(vm => $"'{vm.ScanCode}' exists in a Barcode Type range. Please enter a scan code not within a Barcode Type range.")
                    .Matches("^[1-9]\\d{0,12}$")
                    .WithMessage("Scan Code must contain only digits, not start with a 0, and must be 1 to 13 characters long.");
            });
            this.When(vm => vm.BarcodeTypeId != this.barcodeTypeModelList.Where(b => b.BarcodeType.ToUpper() == "UPC").Select(s => s.BarcodeTypeId).FirstOrDefault(), () =>
            {
                When(vm => !string.IsNullOrWhiteSpace(vm.ScanCode), () =>
                {
                    RuleFor(vm => new { vm.ScanCode, vm.BarcodeTypeId })
                        .Must(sc => !doesScanCodeExistQueryHandler.Search(new DoesScanCodeExistParameters { ScanCode = sc.ScanCode }))
                        .WithMessage(vm => $"'{vm.ScanCode}' is already associated to an item. Please use another scan code.")
                        .Must(sc => IsScanCodeInSelectedBarcodeTypeRange(sc.ScanCode, sc.BarcodeTypeId))
                        .WithMessage(vm => $"'{vm.ScanCode}' should be in selected Barcode Type range. Please enter a scan code within selected Barcode Type range.");

                });
                RuleFor(vm => vm.ScanCode)
                .Matches("^[1-9]\\d{0,12}$")
                .WithMessage("Scan Code must contain only digits, not start with a 0, and must be 1 to 13 characters long.");
            });
            RuleFor(vm => vm.ItemAttributes)
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

        /// <summary>
        /// Checks if a scancode falls within selected barcode type BeginRange and EndRange.
        /// </summary>
        private bool IsScanCodeInSelectedBarcodeTypeRange(string scanCode, int barcodeTypeId)
        {
            long scanCodeLong = 0;
            if (long.TryParse(scanCode, out scanCodeLong))
            {
                var barcodeSelected = this.barcodeTypeModelList.Where(s => s.BarcodeTypeId == barcodeTypeId).FirstOrDefault();
                long beginRange = long.Parse(barcodeSelected.BeginRange);
                long endRange = long.Parse(barcodeSelected.EndRange);
                if (scanCodeLong >= beginRange && scanCodeLong <= endRange)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="upc"></param>
        /// <returns></returns>
        private bool IsUpcInBarcodeTypeRanges(string upc)
        {
            long upcLong = 0;
            if (long.TryParse(upc, out upcLong))
            {
                var barcodeTypes = getBarcodeTypeQueryHandler.Search(new GetBarcodeTypeParameters())
                    .Where(b => !string.IsNullOrWhiteSpace(b.BeginRange) && !string.IsNullOrWhiteSpace(b.EndRange));

                foreach (var barcodeType in barcodeTypes)
                {
                    long beginRange = long.Parse(barcodeType.BeginRange);
                    long endRange = long.Parse(barcodeType.EndRange);
                    if (upcLong >= beginRange && upcLong <= endRange)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}