using FluentValidation;
using Icon.Common.DataAccess;
using Icon.Esb.Schemas.Wfm.Contracts;
using Icon.Framework;
using Icon.Infor.Listeners.HierarchyClass.Commands;
using Icon.Infor.Listeners.HierarchyClass.Constants;
using Icon.Infor.Listeners.HierarchyClass.Extensions;
using Icon.Infor.Listeners.HierarchyClass.Models;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Infor.Listeners.HierarchyClass.Validators
{
    public class HierarchyClassModelValidator : AbstractValidator<InforHierarchyClassModel>, ICollectionValidator<InforHierarchyClassModel>
    {
        private ICommandHandler<ValidateHierarchyClassesCommand> validateHierarchyClassesCommandHandler;

        public HierarchyClassModelValidator(ICommandHandler<ValidateHierarchyClassesCommand> validateHierarchyClassesCommandHandler)
        {
            this.validateHierarchyClassesCommandHandler = validateHierarchyClassesCommandHandler;

            CascadeMode = CascadeMode.StopOnFirstFailure;

            // Action code
            RuleFor(hc => hc.Action)
                .NotEmpty()
                    .WithErrorCode(ValidationErrors.Codes.RequiredAction)
                    .WithMessage(ValidationErrors.Descriptions.RequiredProperty)
                .Must(a => a == ActionEnum.AddOrUpdate || a == ActionEnum.Delete)
                    .WithErrorCode(ValidationErrors.Codes.InvalidAction)
                    .WithMessage(ValidationErrors.Descriptions.InvalidAction);

            // HierarchyClassId
            RuleFor(hc => hc.HierarchyClassId)
                .NotEmpty()
                .WithErrorCode(ValidationErrors.Codes.RequiredHierarchyClassId)
                .WithMessage(ValidationErrors.Descriptions.RequiredProperty);

            // HierarchyClassName
            RuleFor(hc => hc.HierarchyClassName)
                .NotEmpty()
                .WithErrorCode(ValidationErrors.Codes.RequiredHierarchyClassName)
                .WithMessage(ValidationErrors.Descriptions.RequiredProperty);

            // HierarchyName
            RuleFor(hc => hc.HierarchyName)
                .NotEmpty()
                    .WithErrorCode(ValidationErrors.Codes.RequiredHierarchyName)
                    .WithMessage(ValidationErrors.Descriptions.RequiredProperty)
                .Must(hc => Hierarchies.Names.AsArray.Contains(hc))
                    .WithErrorCode(ValidationErrors.Codes.InvalidHierarchyName)
                    .WithMessage(ValidationErrors.Descriptions.InvalidHierarchyName);

            // ParentHierarchyClassId
            RuleFor(hc => hc.ParentHierarchyClassId)
                .NotNull()
                    .WithErrorCode(ValidationErrors.Codes.RequiredParentHierarchyClassId)
                    .WithMessage(ValidationErrors.Descriptions.RequiredProperty)
                .Must(p => p >= 0)
                    .WithErrorCode(ValidationErrors.Codes.InvalidParentHierarchyClassId)
                    .WithMessage(ValidationErrors.Descriptions.InvalidId);

            // HierarchyLevelName
            RuleFor(hc => hc.HierarchyLevelName)
                .NotEmpty()
                .WithErrorCode(ValidationErrors.Codes.RequiredHierarchyLevelName)
                .WithMessage(ValidationErrors.Descriptions.RequiredProperty);

            // Only Validate other properties for AddOrUpdate messages
            When(hc => hc.Action == ActionEnum.AddOrUpdate, () =>
            {
                // Tax
                When(hc => hc.HierarchyName == Hierarchies.Names.Tax, () =>
                {
                    // Tax Class Name
                    RuleFor(hc => hc.HierarchyClassName)
                        .Matches(HierarchyConstants.TaxNamePattern)
                        .WithErrorCode(ValidationErrors.Codes.InvalidTaxClassName)
                        .WithMessage(ValidationErrors.Descriptions.InvalidTaxClassName);

                    // Required Traits
                    RuleFor(hc => hc.HierarchyClassTraits)
                        .Must(hct => hct.ContainsKey(Traits.Codes.TaxAbbreviation))
                            .WithErrorCode(ValidationErrors.Codes.RequiredTaxAbbreviation)
                            .WithMessage(ValidationErrors.Descriptions.RequiredTaxAbbreviation)
                        .Must(hct => hct.ContainsKey(Traits.Codes.TaxRomance))
                            .WithErrorCode(ValidationErrors.Codes.RequiredTaxRomance)
                            .WithMessage(ValidationErrors.Descriptions.RequiredTaxRomance);

                    // Trait value validation
                    When(hc => hc.HierarchyClassTraits.ContainsKey(Traits.Codes.TaxAbbreviation), () =>
                    {
                        RuleFor(hct => hct.HierarchyClassTraits[Traits.Codes.TaxAbbreviation])
                            .Matches(TraitPatterns.TaxAbbreviation)
                            .WithErrorCode(ValidationErrors.Codes.InvalidTaxAbbreviation)
                            .WithMessage(ValidationErrors.Descriptions.InvalidTaxAbbreviation);
                    });

                    When(hc => hc.HierarchyClassTraits.ContainsKey(Traits.Codes.TaxRomance), () =>
                    {
                        RuleFor(hct => hct.HierarchyClassTraits[Traits.Codes.TaxRomance])
                            .Matches(TraitPatterns.TaxRomance)
                            .WithErrorCode(ValidationErrors.Codes.InvalidTaxRomance)
                            .WithMessage(ValidationErrors.Descriptions.InvalidTaxRomance);
                    });

                    // Hierarchy Level name validation
                    RuleFor(hc => hc.HierarchyLevelName)
                        .Must(ln => HierarchyConstants.TaxHierarchyLevelNames.Contains(ln))
                            .WithErrorCode(ValidationErrors.Codes.InvalidTaxHierarchyLevelName)
                            .WithMessage(ValidationErrors.Descriptions.InvalidTaxHierarchyLevelName);
                });

                // Merchandise
                When(hc => hc.HierarchyName == Hierarchies.Names.Merchandise, () =>
                {
                    // HierarchyClass Name length
                    RuleFor(hc => hc.HierarchyClassName)
                        .Length(1, 255)
                        .WithErrorCode(ValidationErrors.Codes.InvalidMerchandiseName)
                        .WithMessage(ValidationErrors.Descriptions.InvalidMerchandiseName);

                    // HierarchyLevelName is a valid Merchandise Level
                    RuleFor(m => m.HierarchyLevelName)
                        .Must(ln => HierarchyConstants.MerchandiseHierarchyLevelNames.Contains(ln))
                        .WithErrorCode(ValidationErrors.Codes.InvalidMerchandiseHierarchyLevelName)
                        .WithMessage(ValidationErrors.Descriptions.InvalidMerchandiseHierarchyLevelName);

                    // SubBrick specific requirements:
                    When(m => m.HierarchyLevelName == Icon.Framework.HierarchyLevelNames.SubBrick, () =>
                    {
                        // SubBrickCode Required
                        RuleFor(sb => sb.HierarchyClassTraits)
                            .Must(sb => sb.ContainsKey(Traits.Codes.SubBrickCode))
                            .WithErrorCode(ValidationErrors.Codes.RequiredSubBrickCode)
                            .WithMessage(ValidationErrors.Descriptions.RequiredSubBrickCode);
                    });

                    When(t => t.HierarchyClassTraits.ContainsKey(Traits.Codes.SubBrickCode), () =>
                    {
                        // SubBrick code RegEx validation
                        RuleFor(sbc => sbc.HierarchyClassTraits[Traits.Codes.SubBrickCode])
                            .Matches(TraitPatterns.SubBrickCode)
                            .WithErrorCode(ValidationErrors.Codes.InvalidSubBrickCode)
                            .WithMessage(ValidationErrors.Descriptions.InvalidSubBrickCode);
                    });

                    // Non-required trait validation:
                    // MerchFinMapping
                    When(t => t.HierarchyClassTraits.ContainsKey(Traits.Codes.MerchFinMapping), () =>
                    {
                        RuleFor(sbc => sbc.HierarchyClassTraits[Traits.Codes.MerchFinMapping])
                            .Matches(TraitPatterns.MerchFinMapping)
                            .WithErrorCode(ValidationErrors.Codes.InvalidMerchFinMapping)
                            .WithMessage(ValidationErrors.Descriptions.InvalidMerchFinMapping);
                    });

                    // Prohibit Discount
                    When(t => t.HierarchyClassTraits.ContainsKey(Traits.Codes.ProhibitDiscount), () =>
                    {
                        RuleFor(m => m.HierarchyClassTraits[Traits.Codes.ProhibitDiscount])
                            .Matches(TraitPatterns.ProhibitDiscount)
                            .WithErrorCode(ValidationErrors.Codes.InvalidProhibitDiscount)
                            .WithMessage(ValidationErrors.Descriptions.InvalidProhibitDiscount);
                    });

                    // Default Tax Association
                    When(t => t.HierarchyClassTraits.ContainsKey(Traits.Codes.MerchDefaultTaxAssociatation), () =>
                    {
                        RuleFor(m => m.HierarchyClassTraits[Traits.Codes.MerchDefaultTaxAssociatation])
                            .Matches(TraitPatterns.MerchDefaultTaxAssociatation)
                            .WithErrorCode(ValidationErrors.Codes.InvalidDefaultTaxClass)
                            .WithMessage(ValidationErrors.Descriptions.InvalidDefaultTaxClass);
                    });

                    // NonMerchandise
                    When(t => t.HierarchyClassTraits.ContainsKey(Traits.Codes.NonMerchandise), () =>
                    {
                        RuleFor(m => m.HierarchyClassTraits.GetValueOrDefault(Traits.Codes.NonMerchandise, string.Empty))
                            .Matches(TraitPatterns.NonMerchandise)
                            .WithErrorCode(ValidationErrors.Codes.InvalidNonMerchTrait)
                            .WithMessage(ValidationErrors.Descriptions.InvalidNonMerchTrait);
                    });
                });

                // National
                When(hc => hc.HierarchyName == Hierarchies.Names.National, () =>
                {
                    When(n => n.HierarchyLevelName == HierarchyLevelNames.NationalClass, () =>
                    {
                        // Required National Class Code
                        RuleFor(n => n.HierarchyClassTraits)
                            .Must(nt => nt.ContainsKey(Traits.Codes.NationalClassCode))
                            .WithErrorCode(ValidationErrors.Codes.RequiredNationalClassCode)
                            .WithMessage(ValidationErrors.Descriptions.RequiredNationalClassCode);

                        // Invalid National Class Code
                        When(hc => hc.HierarchyClassTraits.ContainsKey(Traits.Codes.NationalClassCode), () =>
                        {
                            RuleFor(hct => hct.HierarchyClassTraits[Traits.Codes.NationalClassCode])
                                .Matches(TraitPatterns.NationalClassCode)
                                .WithErrorCode(ValidationErrors.Codes.InvalidNationalClassCode)
                                .WithMessage(ValidationErrors.Descriptions.InvalidNationalClassCode);
                        });
                    });

                    // Invalid National Class Name
                    RuleFor(n => n.HierarchyClassName)
                        .Length(min: 1, max: 255)
                        .WithErrorCode(ValidationErrors.Codes.InvalidNationalClassName)
                        .WithMessage(ValidationErrors.Descriptions.InvalidNationalClassName);

                    // National Hierarchy Level
                    RuleFor(m => m.HierarchyLevelName)
                        .Must(ln => HierarchyConstants.NationalHierarchyLevelNames.Contains(ln))
                        .WithErrorCode(ValidationErrors.Codes.InvalidNationalHierarchyLevelName)
                        .WithMessage(ValidationErrors.Descriptions.InvalidNationalHierarchyLevelName);
                });

                // Brand
                When(hc => hc.HierarchyName == Hierarchies.Names.Brands, () =>
                {
                    // Required Brand Abbreviation
                    RuleFor(b => b.HierarchyClassTraits)
                        .Must(t => t.ContainsKey(Traits.Codes.BrandAbbreviation))
                        .WithErrorCode(ValidationErrors.Codes.RequiredBrandAbbreviation)
                        .WithMessage(ValidationErrors.Descriptions.RequiredBrandAbbreviation);

                    // Invalid Brand Abbreviation
                    When(hc => hc.HierarchyClassTraits.ContainsKey(Traits.Codes.BrandAbbreviation), () =>
                    {
                        RuleFor(hct => hct.HierarchyClassTraits[Traits.Codes.BrandAbbreviation])
                            .Matches(TraitPatterns.BrandAbbreviation)
                            .WithErrorCode(ValidationErrors.Codes.InvalidBrandAbbreviation)
                            .WithMessage(ValidationErrors.Descriptions.InvalidBrandAbbreviation);
                    });

                    // Invalid Brand Name
                    RuleFor(b => b.HierarchyClassName)
                        .Matches(HierarchyConstants.BrandNamePattern)
                        .WithErrorCode(ValidationErrors.Codes.InvalidBrandName)
                        .WithMessage(ValidationErrors.Descriptions.InvalidBrandName);

                    // HierarchyLevel must be 'Brand'
                    RuleFor(m => m.HierarchyLevelName)
                        .Must(ln => HierarchyConstants.BrandHierarchyLevelNames.Contains(ln))
                        .WithErrorCode(ValidationErrors.Codes.InvalidBrandHierarchyLevelName)
                        .WithMessage(ValidationErrors.Descriptions.InvalidBrandHierarchyLevelName);
                });

                // Financial
                When(hc => hc.HierarchyName == Hierarchies.Names.Financial, () =>
                {
                    RuleFor(f => f.HierarchyClassName)
                        .Length(min: 0, max: 255)
                        .WithErrorCode(ValidationErrors.Codes.InvalidFinancialName)
                        .WithMessage(ValidationErrors.Descriptions.InvalidFinancialName);

                    RuleFor(m => m.HierarchyLevelName)
                        .Must(ln => HierarchyConstants.FinancialHierarchyLevelNames.Contains(ln))
                        .WithErrorCode(ValidationErrors.Codes.InvalidFinancialHierarchyLevelName)
                        .WithMessage(ValidationErrors.Descriptions.InvalidFinancialHierarchyLevelName);
                });
            });
        }

        private bool IsNumeric(string id)
        {
            int result = 0;
            return int.TryParse(id, out result);
        }

        public void ValidateCollection(IEnumerable<InforHierarchyClassModel> collection)
        {
            foreach (var hierarchyClass in collection)
            {
                var result = Validate(hierarchyClass);
                if (!result.IsValid)
                {
                    hierarchyClass.ErrorCode = result.Errors.First().ErrorCode;
                    hierarchyClass.ErrorDetails = result.Errors.First().ErrorMessage;
                }
            }

            validateHierarchyClassesCommandHandler.Execute(new ValidateHierarchyClassesCommand { HierarchyClasses = collection });
        }
    }
}
