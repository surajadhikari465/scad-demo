using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Icon.Infor.Listeners.HierarchyClass.Validators;
using Icon.Infor.Listeners.HierarchyClass.Models;
using Icon.Esb.Schemas.Wfm.Contracts;
using Icon.Infor.Listeners.HierarchyClass.Constants;
using System.Collections.Generic;
using Icon.Framework;
using System.Linq;
using Moq;
using Icon.Common.DataAccess;
using Icon.Infor.Listeners.HierarchyClass.Commands;
using Icon.Common.Email;
using Icon.Esb.Subscriber;
using Icon.Infor.Listeners.HierarchyClass.Notifier;

namespace Icon.Infor.Listeners.HierarchyClass.Tests.Validators
{
    [TestClass]
    public class HierarchyClassValidatorTests
    {
        private List<HierarchyClassModel> testHierarchyClasses;
        private HierarchyClassModel testHierarchyClass;
        private HierarchyClassModelValidator validator;
        private Mock<ICommandHandler<ValidateHierarchyClassesCommand>> mockValidateHierarchyClassesCommandHandler;
        
        [TestInitialize]
        public void InitializeTests()
        {
            this.testHierarchyClasses = new List<HierarchyClassModel>();
            this.testHierarchyClass = new HierarchyClassModel
            {
                HierarchyClassId = 1,
                HierarchyClassName = "Test HierarchyClass Unit Test",
                HierarchyLevelName = HierarchyLevelNames.Brand,
                HierarchyName = Hierarchies.Names.Brands,
                ParentHierarchyClassId = 0,
                Action = ActionEnum.AddOrUpdate,
                HierarchyClassTraits = new Dictionary<string, string>()
            };

            this.testHierarchyClasses.Add(this.testHierarchyClass);

            this.mockValidateHierarchyClassesCommandHandler = new Mock<ICommandHandler<ValidateHierarchyClassesCommand>>();
            this.validator = new HierarchyClassModelValidator(mockValidateHierarchyClassesCommandHandler.Object);
        }

        [TestMethod]
        public void HierarchyClassModelValidator_ActionIsInvalid_InvalidActionError()
        {
            // Given
            this.testHierarchyClass.Action = ActionEnum.Archive;

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidAction,
                ValidationErrors.Descriptions.InvalidAction.GetFormattedValidationMessage(
                    nameof(this.testHierarchyClass.Action),
                    this.testHierarchyClass.Action.ToString()));
        }

        [TestMethod]
        public void HierarchyClassModelValidator_ActionIsValid_NoError()
        {
            // Given
            this.testHierarchyClass.Action = ActionEnum.AddOrUpdate;
            this.testHierarchyClasses.Add(CopyTestHierarchyClass((a) => a.Action = ActionEnum.Delete));
            this.testHierarchyClass.HierarchyName = Hierarchies.Names.Brands;
            this.testHierarchyClass.HierarchyClassTraits.Add(Traits.Codes.BrandAbbreviation, "BABV");

            PerformValidateCollectionWhenAndThenSteps(null, null);
        }

        [TestMethod]
        public void HierarchyClassModelValidator_HierarchyClassIdIsEmpty_RequiredHierarchyClassIdError()
        {
            // Given
            this.testHierarchyClass.HierarchyClassId = 0;

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.RequiredHierarchyClassId,
                ValidationErrors.Descriptions.RequiredProperty.GetFormattedValidationMessage(
                    nameof(this.testHierarchyClass.HierarchyClassId),
                    this.testHierarchyClass.HierarchyClassId.ToString()));
        }

        [TestMethod]
        public void HierarchyClassModelValidator_HierarchyClassIdIsValid_NoError()
        {
            // Given
            this.testHierarchyClass.HierarchyClassId = 1;
            this.testHierarchyClass.HierarchyClassTraits.Add(Traits.Codes.BrandAbbreviation, "ABAV");

            PerformValidateCollectionWhenAndThenSteps(null, null);
        }

        [TestMethod]
        public void HierarchyClassModelValidator_HierarchyClassNameIsEmpty_RequiredHierarchyClassIdError()
        {
            // Given
            this.testHierarchyClass.HierarchyClassName = String.Empty;

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.RequiredHierarchyClassName,
                ValidationErrors.Descriptions.RequiredProperty.GetFormattedValidationMessage(
                    nameof(this.testHierarchyClass.HierarchyClassName),
                    this.testHierarchyClass.HierarchyClassName));
        }

        [TestMethod]
        public void HierarchyClassModelValidator_HierarchyClassNameIsValid_NoError()
        {
            // Given
            this.testHierarchyClass.HierarchyClassName = "Valid Test Name";
            this.testHierarchyClass.HierarchyClassTraits.Add(Traits.Codes.BrandAbbreviation, "ABAV");

            PerformValidateCollectionWhenAndThenSteps(null, null);
        }

        [TestMethod]
        public void HierarchyClassModelValidator_HierarchyNameIsInvalid_InvalidHierarchyNameError()
        {
            // Given
            this.testHierarchyClass.HierarchyName = "Invalid Hierarchy Name";

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidHierarchyName,
                ValidationErrors.Descriptions.InvalidHierarchyName.GetFormattedValidationMessage(
                    nameof(this.testHierarchyClass.HierarchyName),
                    this.testHierarchyClass.HierarchyName));
        }

        [TestMethod]
        public void HierarchyClassModelValidator_HierarchyNameIsEmpty_RequiredHierarchyNameError()
        {
            // Given
            this.testHierarchyClass.HierarchyName = String.Empty;

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.RequiredHierarchyName,
                ValidationErrors.Descriptions.RequiredProperty.GetFormattedValidationMessage(
                    nameof(this.testHierarchyClass.HierarchyName),
                    this.testHierarchyClass.HierarchyName));
        }

        [TestMethod]
        public void HierarchyClassModelValidator_HierarchyNameIsValid_NoError()
        {
            // Given
            this.testHierarchyClass.HierarchyName = Hierarchies.Names.Brands;
            this.testHierarchyClass.HierarchyLevelName = HierarchyLevelNames.Brand;
            this.testHierarchyClass.HierarchyClassTraits.Add(Traits.Codes.BrandAbbreviation, "ABAV");

            PerformValidateCollectionWhenAndThenSteps(null, null);
        }

        [TestMethod]
        public void HierarchyClassModelValidator_TaxClassIsInvalid_InvalidTaxClassError()
        {
            // Given
            this.testHierarchyClass.HierarchyName = Hierarchies.Names.Tax;
            this.testHierarchyClass.HierarchyLevelName = HierarchyLevelNames.Tax;
            this.testHierarchyClass.HierarchyClassName = "INVALID TAX CLASS NAME WITH NO TAX CODE";
            this.testHierarchyClass.HierarchyClassTraits = new Dictionary<string, string>
            {
                { TraitCodes.TaxAbbreviation, "1234567 Test Tax Abbrev"},
                { TraitCodes.TaxRomance, "1234567 Test Tax Romance" }
            };

            PerformValidateCollectionWhenAndThenSteps(ValidationErrors.Codes.InvalidTaxClassName,
                ValidationErrors.Descriptions.InvalidTaxClassName.GetFormattedValidationMessage(nameof(this.testHierarchyClass.HierarchyClassName), this.testHierarchyClass.HierarchyClassName));
        }

        [TestMethod]
        public void HierarchyClassModelValidator_TaxClassIsInvalidWithUnderscores_InvalidTaxClassError()
        {
            // Given
            this.testHierarchyClass.HierarchyName = Hierarchies.Names.Tax;
            this.testHierarchyClass.HierarchyLevelName = HierarchyLevelNames.Tax;
            this.testHierarchyClass.HierarchyClassName = "1234567_INVALID TAX CLASS NAME WITH UNDERSCORES_";
            this.testHierarchyClass.HierarchyClassTraits = new Dictionary<string, string>
            {
                { TraitCodes.TaxAbbreviation, "1234567 Test Tax Abbrev"},
                { TraitCodes.TaxRomance, "1234567 Test Tax Romance" }
            };

            PerformValidateCollectionWhenAndThenSteps(ValidationErrors.Codes.InvalidTaxClassName,
                ValidationErrors.Descriptions.InvalidTaxClassName.GetFormattedValidationMessage(nameof(this.testHierarchyClass.HierarchyClassName), this.testHierarchyClass.HierarchyClassName));
        }

        [TestMethod]
        public void HierarchyClassModelValidator_TaxAbbreviationIsInvalid_InvalidTaxClassError()
        {
            // Given
            this.testHierarchyClass.HierarchyName = Hierarchies.Names.Tax;
            this.testHierarchyClass.HierarchyLevelName = HierarchyLevelNames.Tax;
            this.testHierarchyClass.HierarchyClassName = "1212121 VALID TEST TAX CLASS NAME WITH NO TAX CODE";
            this.testHierarchyClass.HierarchyClassTraits = new Dictionary<string, string>
            {
                { TraitCodes.TaxAbbreviation, "1234567 Test Tax Abbrev !@"},
                { TraitCodes.TaxRomance, "1234567 Test Tax Romance" }
            };

            PerformValidateCollectionWhenAndThenSteps(ValidationErrors.Codes.InvalidTaxAbbreviation,
                ValidationErrors.Descriptions.InvalidTaxAbbreviation.GetFormattedValidationMessage(
                    nameof(this.testHierarchyClass.HierarchyClassTraits),
                    this.testHierarchyClass.HierarchyClassTraits[TraitCodes.TaxAbbreviation]));
        }

        [TestMethod]
        public void HierarchyClassModelValidator_TaxRomanceIsInvalid_InvalidTaxClassError()
        {
            // Given
            this.testHierarchyClass.HierarchyName = Hierarchies.Names.Tax;
            this.testHierarchyClass.HierarchyLevelName = HierarchyLevelNames.Tax;
            this.testHierarchyClass.HierarchyClassName = "1212121 VALID TEST TAX CLASS NAME WITH NO TAX CODE";
            this.testHierarchyClass.HierarchyClassTraits = new Dictionary<string, string>
            {
                { TraitCodes.TaxAbbreviation, "1234567 Test Tax Abbrev"},
                { TraitCodes.TaxRomance, "1234567 Invalid Tax Romance !@" }
            };

            PerformValidateCollectionWhenAndThenSteps(ValidationErrors.Codes.InvalidTaxRomance,
                ValidationErrors.Descriptions.InvalidTaxRomance.GetFormattedValidationMessage(
                    nameof(this.testHierarchyClass.HierarchyClassTraits),
                    this.testHierarchyClass.HierarchyClassTraits[TraitCodes.TaxRomance]));
        }

        [TestMethod]
        public void HierarchyClassModelValidator_TaxHierarchyLevelNameIsInvalid_InvalidTaxHierarchyLevelError()
        {
            // Given
            this.testHierarchyClass.HierarchyName = Hierarchies.Names.Tax;
            this.testHierarchyClass.HierarchyLevelName = "Invalid Tax Level";
            this.testHierarchyClass.HierarchyClassName = "1212121 VALID TEST TAX CLASS NAME WITH NO TAX CODE";
            this.testHierarchyClass.HierarchyClassTraits = new Dictionary<string, string>
            {
                { TraitCodes.TaxAbbreviation, "1234567 Valid Tax Abbrev"},
                { TraitCodes.TaxRomance, "1234567 Valid Tax Romance" }
            };

            PerformValidateCollectionWhenAndThenSteps(ValidationErrors.Codes.InvalidTaxHierarchyLevelName,
                ValidationErrors.Descriptions.InvalidTaxHierarchyLevelName.GetFormattedValidationMessage(
                    nameof(this.testHierarchyClass.HierarchyLevelName),
                    this.testHierarchyClass.HierarchyLevelName));
        }

        [TestMethod]
        public void HierarchyClassModelValidator_TaxAbbreviationIsMissing_RequiredTaxAbbreviationError()
        {
            // Given
            this.testHierarchyClass.HierarchyName = Hierarchies.Names.Tax;
            this.testHierarchyClass.HierarchyLevelName = HierarchyLevelNames.Tax;
            this.testHierarchyClass.HierarchyClassName = "1212121 VALID TEST TAX CLASS NAME WITH NO TAX CODE";
            this.testHierarchyClass.HierarchyClassTraits = new Dictionary<string, string>
            {
                { TraitCodes.TaxRomance, "1234567 Valid Tax Romance" }
            };

            PerformValidateCollectionWhenAndThenSteps(ValidationErrors.Codes.RequiredTaxAbbreviation,
                ValidationErrors.Descriptions.RequiredTaxAbbreviation.GetFormattedValidationMessage(
                    Traits.Descriptions.TaxAbbreviation, null));
        }

        [TestMethod]
        public void HierarchyClassModelValidator_TaxRomanceIsMissing_RequiredTaxAbbreviationError()
        {
            // Given
            this.testHierarchyClass.HierarchyName = Hierarchies.Names.Tax;
            this.testHierarchyClass.HierarchyLevelName = HierarchyLevelNames.Tax;
            this.testHierarchyClass.HierarchyClassName = "1212121 VALID TEST TAX CLASS NAME WITH NO TAX CODE";
            this.testHierarchyClass.HierarchyClassTraits = new Dictionary<string, string>
            {
                { TraitCodes.TaxAbbreviation, "1234567 Valid Tax Abbrev"}
            };

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.RequiredTaxRomance,
                ValidationErrors.Descriptions.RequiredTaxRomance.GetFormattedValidationMessage(Traits.Descriptions.TaxRomance, null));
        }

        [TestMethod]
        public void HierarchyClassModelValidator_MerchandiseNameIsInvalid_InvalidMerchandiseNameError()
        {
            // Given
            this.testHierarchyClass.HierarchyName = Hierarchies.Names.Merchandise;
            this.testHierarchyClass.HierarchyLevelName = HierarchyLevelNames.SubBrick;
            this.testHierarchyClass.HierarchyClassName = new string('m',256);
            this.testHierarchyClass.HierarchyClassTraits = new Dictionary<string, string>
            {
                { TraitCodes.ProhibitDiscount, "1"},
                { TraitCodes.MerchDefaultTaxAssociatation, "1111777" },
                { TraitCodes.SubBrickCode, "7777777" }
            };

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidMerchandiseName,
                ValidationErrors.Descriptions.InvalidMerchandiseName.GetFormattedValidationMessage(
                    nameof(this.testHierarchyClass.HierarchyClassName),
                    this.testHierarchyClass.HierarchyClassName));
        }

        [TestMethod]
        public void HierarchyClassModelValidator_SubBrickMissingSubBrickcodeTrait_RequiredSubBrickCodeTraitError()
        {
            // Given
            this.testHierarchyClass.HierarchyName = Hierarchies.Names.Merchandise;
            this.testHierarchyClass.HierarchyLevelName = HierarchyLevelNames.SubBrick;
            this.testHierarchyClass.HierarchyClassName = "Valid SubBrick";
            this.testHierarchyClass.HierarchyClassTraits = new Dictionary<string, string>
            {
                { TraitCodes.ProhibitDiscount, "1"},
                { TraitCodes.MerchDefaultTaxAssociatation, "1111777" }
            };

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.RequiredSubBrickCode,
                ValidationErrors.Descriptions.RequiredSubBrickCode.GetFormattedValidationMessage(
                    Traits.Descriptions.SubBrickCode,
                    null));
        }

        [TestMethod]
        public void HierarchyClassModelValidator_SubBrickCodeTraitIsInvalid_InvalidSubBrickCodeError()
        {
            // Given
            this.testHierarchyClass.HierarchyName = Hierarchies.Names.Merchandise;
            this.testHierarchyClass.HierarchyLevelName = HierarchyLevelNames.SubBrick;
            this.testHierarchyClass.HierarchyClassName = "Valid SubBrick";
            this.testHierarchyClass.HierarchyClassTraits = new Dictionary<string, string>
            {
                { TraitCodes.MerchFinMapping, "1" },
                { TraitCodes.SubBrickCode, "invalid!" },
                { TraitCodes.ProhibitDiscount, "1"},
            };

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidSubBrickCode,
                ValidationErrors.Descriptions.InvalidSubBrickCode.GetFormattedValidationMessage(
                    Traits.Descriptions.SubBrickCode,
                    this.testHierarchyClass.HierarchyClassTraits[Traits.Codes.SubBrickCode]));
        }

        [TestMethod]
        public void HierarchyClassModelValidator_MerchFinMappingTraitIsInvalid_InvalidMerchFinMappingError()
        {
            // Given
            this.testHierarchyClass.HierarchyName = Hierarchies.Names.Merchandise;
            this.testHierarchyClass.HierarchyLevelName = HierarchyLevelNames.SubBrick;
            this.testHierarchyClass.HierarchyClassName = "Valid SubBrick";
            this.testHierarchyClass.HierarchyClassTraits = new Dictionary<string, string>
            {
                { TraitCodes.MerchFinMapping, "invalid!" },
                { TraitCodes.SubBrickCode, "12345" },
                { TraitCodes.ProhibitDiscount, "1"},
            };

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidMerchFinMapping,
                ValidationErrors.Descriptions.InvalidMerchFinMapping.GetFormattedValidationMessage(
                    Traits.Descriptions.MerchFinMapping,
                    this.testHierarchyClass.HierarchyClassTraits[Traits.Codes.MerchFinMapping]));
        }

        [TestMethod]
        public void HierarchyClassModelValidator_ProhibitDiscountTraitIsInvalid_InvalidProhibitDiscountError()
        {
            // Given
            this.testHierarchyClass.HierarchyName = Hierarchies.Names.Merchandise;
            this.testHierarchyClass.HierarchyLevelName = HierarchyLevelNames.SubBrick;
            this.testHierarchyClass.HierarchyClassName = "Valid SubBrick";
            this.testHierarchyClass.HierarchyClassTraits = new Dictionary<string, string>
            {
                { TraitCodes.MerchFinMapping, "1" },
                { TraitCodes.SubBrickCode, "12345" },
                { TraitCodes.ProhibitDiscount, "invalid"},
            };

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidProhibitDiscount,
                ValidationErrors.Descriptions.InvalidProhibitDiscount.GetFormattedValidationMessage(
                    Traits.Descriptions.ProhibitDiscount,
                    this.testHierarchyClass.HierarchyClassTraits[Traits.Codes.ProhibitDiscount]));
        }

        [TestMethod]
        public void HierarchyClassModelValidator_DefaultTaxAssociationTraitIsInvalid_InvalidDefaultTaxAssociationError()
        {
            // Given
            this.testHierarchyClass.HierarchyName = Hierarchies.Names.Merchandise;
            this.testHierarchyClass.HierarchyLevelName = HierarchyLevelNames.SubBrick;
            this.testHierarchyClass.HierarchyClassName = "Valid SubBrick";
            this.testHierarchyClass.HierarchyClassTraits = new Dictionary<string, string>
            {
                { TraitCodes.MerchFinMapping, "1" },
                { TraitCodes.SubBrickCode, "12345" },
                { TraitCodes.MerchDefaultTaxAssociatation, "invalid no tax code" }
            };

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidDefaultTaxClass,
                ValidationErrors.Descriptions.InvalidDefaultTaxClass.GetFormattedValidationMessage(
                    String.Empty,
                    this.testHierarchyClass.HierarchyClassTraits[Traits.Codes.MerchDefaultTaxAssociatation]));
        }

        [TestMethod]
        public void HierarchyClassModelValidator_NonMerchandiseTraitIsInvalid_InvalidNonMerchandiseError()
        {
            // Given
            this.testHierarchyClass.HierarchyName = Hierarchies.Names.Merchandise;
            this.testHierarchyClass.HierarchyLevelName = HierarchyLevelNames.SubBrick;
            this.testHierarchyClass.HierarchyClassName = "Valid SubBrick";
            this.testHierarchyClass.HierarchyClassTraits = new Dictionary<string, string>
            {
                { TraitCodes.MerchFinMapping, "1" },
                { TraitCodes.SubBrickCode, "12345" },
                { TraitCodes.NonMerchandise, "invalid" }
            };

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidNonMerchTrait,
                ValidationErrors.Descriptions.InvalidNonMerchTrait.GetFormattedValidationMessage(
                    String.Empty,
                    this.testHierarchyClass.HierarchyClassTraits[Traits.Codes.NonMerchandise]));
        }

        [TestMethod]
        public void HierarchyClassModelValidator_MerchHierarchyLevelNameIsInvalid_InvalidMerchandiseHierarchyLevelNameError()
        {
            // Given
            this.testHierarchyClass.HierarchyName = Hierarchies.Names.Merchandise;
            this.testHierarchyClass.HierarchyLevelName = HierarchyLevelNames.SubSub;
            this.testHierarchyClass.HierarchyClassName = "Valid SubBrick";
            this.testHierarchyClass.HierarchyClassTraits = new Dictionary<string, string>
            {
                { TraitCodes.MerchFinMapping, "1" },
                { TraitCodes.SubBrickCode, "12345" },
                { TraitCodes.NonMerchandise, "1000" }
            };

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidMerchandiseHierarchyLevelName,
                ValidationErrors.Descriptions.InvalidMerchandiseHierarchyLevelName.GetFormattedValidationMessage(
                    nameof(this.testHierarchyClass.HierarchyLevelName),
                    this.testHierarchyClass.HierarchyLevelName));
        }

        [TestMethod]
        public void HierarchyClassModelValidator_NationaHierarchyClassIsValid_NoError()
        {
            // Given
            this.testHierarchyClass.HierarchyName = Hierarchies.Names.National;
            this.testHierarchyClass.HierarchyLevelName = HierarchyLevelNames.NationalClass;
            this.testHierarchyClass.HierarchyClassName = "Valid National Class";
            this.testHierarchyClass.HierarchyClassTraits = new Dictionary<string, string>
            {
                { TraitCodes.NationalClassCode, "1" }
            };

            PerformValidateCollectionWhenAndThenSteps(
                null,
                null);
        }

        [TestMethod]
        public void HierarchyClassModelValidator_NationalClassCodeIsMissing_RequiredNationalClassCodeMessage()
        {
            // Given
            this.testHierarchyClass.HierarchyName = Hierarchies.Names.National;
            this.testHierarchyClass.HierarchyLevelName = HierarchyLevelNames.NationalClass;
            this.testHierarchyClass.HierarchyClassName = "Valid National Class";

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.RequiredNationalClassCode,
                ValidationErrors.Descriptions.RequiredNationalClassCode.GetFormattedValidationMessage(
                    Traits.Codes.NationalClassCode,
                    null));
        }

        [TestMethod]
        public void HierarchyClassModelValidator_NationalClassCodeIsInvalid_InvalidNationalClassCodeMessage()
        {
            // Given
            this.testHierarchyClass.HierarchyName = Hierarchies.Names.National;
            this.testHierarchyClass.HierarchyLevelName = HierarchyLevelNames.NationalClass;
            this.testHierarchyClass.HierarchyClassName = "Valid National Class";
            this.testHierarchyClass.HierarchyClassTraits = new Dictionary<string, string>
            {
                { TraitCodes.NationalClassCode, "invalid!" }
            };

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidNationalClassCode,
                ValidationErrors.Descriptions.InvalidNationalClassCode.GetFormattedValidationMessage(
                    Traits.Codes.NationalClassCode,
                    this.testHierarchyClass.HierarchyClassTraits[Traits.Codes.NationalClassCode]));
        }

        [TestMethod]
        public void HierarchyClassModelValidator_NationalClassIsCategoryAndNoNationalClassCodeExists_NoError()
        {
            // Given
            this.testHierarchyClass.HierarchyName = Hierarchies.Names.National;
            this.testHierarchyClass.HierarchyLevelName = HierarchyLevelNames.NationalCategory;
            this.testHierarchyClass.HierarchyClassName = "Valid National Class";
            this.testHierarchyClass.HierarchyClassTraits = new Dictionary<string, string>();

            PerformValidateCollectionWhenAndThenSteps(null, null);
        }

        [TestMethod]
        public void HierarchyClassModelValidator_NationalClassIsFamilyAndNoNationalClassCodeExists_NoError()
        {
            // Given
            this.testHierarchyClass.HierarchyName = Hierarchies.Names.National;
            this.testHierarchyClass.HierarchyLevelName = HierarchyLevelNames.NationalFamily;
            this.testHierarchyClass.HierarchyClassName = "Valid National Class";
            this.testHierarchyClass.HierarchyClassTraits = new Dictionary<string, string>();

            PerformValidateCollectionWhenAndThenSteps(null, null);
        }

        [TestMethod]
        public void HierarchyClassModelValidator_NationalClassIsSubCategoryAndNoNationalClassCodeExists_NoError()
        {
            // Given
            this.testHierarchyClass.HierarchyName = Hierarchies.Names.National;
            this.testHierarchyClass.HierarchyLevelName = HierarchyLevelNames.NationalSubCategory;
            this.testHierarchyClass.HierarchyClassName = "Valid National Class";
            this.testHierarchyClass.HierarchyClassTraits = new Dictionary<string, string>();

            PerformValidateCollectionWhenAndThenSteps(null, null);
        }

        [TestMethod]
        public void HierarchyClassModelValidator_NationalClassNameIsInvalid_InvalidNationalClassNameMessage()
        {
            // Given
            this.testHierarchyClass.HierarchyName = Hierarchies.Names.National;
            this.testHierarchyClass.HierarchyLevelName = HierarchyLevelNames.NationalClass;
            this.testHierarchyClass.HierarchyClassName = new string('a', 256);
            this.testHierarchyClass.HierarchyClassTraits = new Dictionary<string, string>
            {
                { TraitCodes.NationalClassCode, "1" }
            };

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidNationalClassName,
                ValidationErrors.Descriptions.InvalidNationalClassName.GetFormattedValidationMessage(
                    nameof(this.testHierarchyClass.HierarchyClassName),
                    this.testHierarchyClass.HierarchyClassName));
        }

        [TestMethod]
        public void HierarchyClassModelValidator_NationalClassLevelNameIsInvalid_InvalidNationalClassLevelMessage()
        {
            // Given
            this.testHierarchyClass.HierarchyName = Hierarchies.Names.National;
            this.testHierarchyClass.HierarchyLevelName = HierarchyLevelNames.Brand;
            this.testHierarchyClass.HierarchyClassName = "Valid National Class Name";
            this.testHierarchyClass.HierarchyClassTraits = new Dictionary<string, string>
            {
                { TraitCodes.NationalClassCode, "1" }
            };

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidNationalHierarchyLevelName,
                ValidationErrors.Descriptions.InvalidNationalHierarchyLevelName.GetFormattedValidationMessage(
                    nameof(this.testHierarchyClass.HierarchyLevelName),
                    this.testHierarchyClass.HierarchyLevelName));
        }

        [TestMethod]
        public void HierarchyClassModelValidator_BrandNameIsInvalid_InvalidBrandNameMessage()
        {
            // Given
            this.testHierarchyClass.HierarchyName = Hierarchies.Names.Brands;
            this.testHierarchyClass.HierarchyLevelName = HierarchyLevelNames.Brand;
            this.testHierarchyClass.HierarchyClassName = new string('a', 36); // more than 35 characters
            this.testHierarchyClass.HierarchyClassTraits = new Dictionary<string, string>
            {
                { TraitCodes.BrandAbbreviation, "BAS" }
            };

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidBrandName,
                ValidationErrors.Descriptions.InvalidBrandName.GetFormattedValidationMessage(
                    nameof(this.testHierarchyClass.HierarchyClassName),
                    this.testHierarchyClass.HierarchyClassName));
        }

        [TestMethod]
        public void HierarchyClassModelValidator_BrandAbbreviationIsMissing_RequiredBrandAbbreviationMessage()
        {
            // Given
            this.testHierarchyClass.HierarchyName = Hierarchies.Names.Brands;
            this.testHierarchyClass.HierarchyLevelName = HierarchyLevelNames.Brand;
            this.testHierarchyClass.HierarchyClassName = "My Valid Brand Name";

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.RequiredBrandAbbreviation,
                ValidationErrors.Descriptions.RequiredBrandAbbreviation.GetFormattedValidationMessage(
                    Traits.Codes.BrandAbbreviation,
                    null));
        }

        [TestMethod]
        public void HierarchyClassModelValidator_BrandAbbreviationIsInvalid_InvalidBrandAbbreviationMessage()
        {
            // Given
            this.testHierarchyClass.HierarchyName = Hierarchies.Names.Brands;
            this.testHierarchyClass.HierarchyLevelName = HierarchyLevelNames.Brand;
            this.testHierarchyClass.HierarchyClassName = "My Valid Brand Name";
            this.testHierarchyClass.HierarchyClassTraits = new Dictionary<string, string>
            {
                { TraitCodes.BrandAbbreviation, "Invalid$#" }
            };

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidBrandAbbreviation,
                ValidationErrors.Descriptions.InvalidBrandAbbreviation.GetFormattedValidationMessage(
                    Traits.Codes.BrandAbbreviation,
                    this.testHierarchyClass.HierarchyClassTraits[Traits.Codes.BrandAbbreviation]));
        }

        [TestMethod]
        public void HierarchyClassModelValidator_BrandHierarhyLevelNameIsInvalid_InvalidBrandHierarchyLevelNameMessage()
        {
            // Given
            this.testHierarchyClass.HierarchyName = Hierarchies.Names.Brands;
            this.testHierarchyClass.HierarchyLevelName = HierarchyLevelNames.Tax;
            this.testHierarchyClass.HierarchyClassName = "My Valid Brand Name";
            this.testHierarchyClass.HierarchyClassTraits = new Dictionary<string, string>
            {
                { TraitCodes.BrandAbbreviation, "BAS" }
            };

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidBrandHierarchyLevelName,
                ValidationErrors.Descriptions.InvalidBrandHierarchyLevelName.GetFormattedValidationMessage(
                    nameof(this.testHierarchyClass.HierarchyLevelName),
                    this.testHierarchyClass.HierarchyLevelName));
        }

        [TestMethod]
        public void HierarchyClassModelValidator_FinancialIsValid_NoError()
        {
            // Given
            this.testHierarchyClass.HierarchyName = Hierarchies.Names.Financial;
            this.testHierarchyClass.HierarchyLevelName = HierarchyLevelNames.Financial;
            this.testHierarchyClass.HierarchyClassName = "My Valid Subteam Name";

            PerformValidateCollectionWhenAndThenSteps(
                null,
                null);
        }

        [TestMethod]
        public void HierarchyClassModelValidator_FinancialNameIsInvalid_InvalidFinancialNameError()
        {
            // Given
            this.testHierarchyClass.HierarchyName = Hierarchies.Names.Financial;
            this.testHierarchyClass.HierarchyLevelName = HierarchyLevelNames.Financial;
            this.testHierarchyClass.HierarchyClassName = new string('f', 256);

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidFinancialName,
                ValidationErrors.Descriptions.InvalidFinancialName.GetFormattedValidationMessage(
                    nameof(this.testHierarchyClass.HierarchyClassName),
                    this.testHierarchyClass.HierarchyClassName));
        }

        [TestMethod]
        public void HierarchyClassModelValidator_FinancialHierarchyLevelNameIsInvalid_InvalidFinancialHierarchyLevelNameError()
        {
            // Given
            this.testHierarchyClass.HierarchyName = Hierarchies.Names.Financial;
            this.testHierarchyClass.HierarchyLevelName = HierarchyLevelNames.Tax;
            this.testHierarchyClass.HierarchyClassName = "My valid subteam";

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidFinancialHierarchyLevelName,
                ValidationErrors.Descriptions.InvalidFinancialHierarchyLevelName.GetFormattedValidationMessage(
                    nameof(this.testHierarchyClass.HierarchyLevelName),
                    this.testHierarchyClass.HierarchyLevelName));
        }

        private void PerformValidateCollectionWhenAndThenSteps(string expectedErrorCode, string expectedErrorDetails)
        {
            //When
            validator.ValidateCollection(testHierarchyClasses);

            //Then
            foreach (var hc in this.testHierarchyClasses)
            {
                Assert.AreEqual(expectedErrorCode, hc.ErrorCode);
                Assert.AreEqual(expectedErrorDetails, hc.ErrorDetails);
            }
        }

        private HierarchyClassModel CopyTestHierarchyClass(Action<HierarchyClassModel> setter = null)
        {
            HierarchyClassModel newTestHierarchyClassModel = new HierarchyClassModel();

            foreach (var property in typeof(HierarchyClassModel).GetProperties())
            {
                property.SetValue(
                    newTestHierarchyClassModel,
                    property.GetValue(this.testHierarchyClass));
            }

            if (setter != null)
                setter(newTestHierarchyClassModel);

            return newTestHierarchyClassModel;
        }
    }
}
