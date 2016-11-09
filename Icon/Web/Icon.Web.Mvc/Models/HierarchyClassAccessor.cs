using Icon.Framework;
using System;
using System.Linq;
using Icon.Web.Extensions;

namespace Icon.Web.Mvc.Models
{
    public class HierarchyClassAccessor
    {
        public static string GetSubTeam(HierarchyClass hierarchyClass)
        {
            var subTeamQuery = hierarchyClass.HierarchyClassTrait.Where(hct => hct.Trait.traitCode == TraitCodes.MerchFinMapping);
            return subTeamQuery.Count() == 0 ? String.Empty : subTeamQuery.Single().traitValue;
        }

        public static string GetGlAccount(HierarchyClass hierarchyClass)
        {
            var glAccountQuery = hierarchyClass.HierarchyClassTrait.Where(hct => hct.Trait.traitCode == TraitCodes.GlAccount);
            return glAccountQuery.Count() == 0 ? String.Empty : glAccountQuery.Single().traitValue;
        }

        public static string GetTaxAbbreviation(HierarchyClass hierarchyClass)
        {
            var taxAbbreviationQuery = hierarchyClass.HierarchyClassTrait.Where(hct => hct.Trait.traitCode == TraitCodes.TaxAbbreviation);
            return taxAbbreviationQuery.Count() == 0 ? String.Empty : taxAbbreviationQuery.Single().traitValue;
        }
        public static string GetTaxRomance(HierarchyClass hierarchyClass)
        {
            var taxRomanceQuery = hierarchyClass.HierarchyClassTrait.Where(hct => hct.Trait.traitCode == TraitCodes.TaxRomance);
            return taxRomanceQuery.Count() == 0 ? String.Empty : taxRomanceQuery.Single().traitValue;
        }

        public static string GetHierarchyParentName(HierarchyClass hierarchyClass)
        {
            return hierarchyClass.HierarchyClass2 == null ? String.Empty : hierarchyClass.HierarchyClass2.hierarchyClassName;
        }

        public static string GetHierarchyLevelName(HierarchyClass hierarchyClass)
        {
            return hierarchyClass.HierarchyPrototype == null ? String.Empty : hierarchyClass.HierarchyPrototype.hierarchyLevelName;
        }

        public static string GetNonMerchandiseTrait(HierarchyClass hierarchyClass)
        {
            var nonMerchandiseTrait = hierarchyClass.HierarchyClassTrait.Where(hct => hct.Trait.traitCode == TraitCodes.NonMerchandise);
            return nonMerchandiseTrait.Count() == 0 ? String.Empty : nonMerchandiseTrait.Single().traitValue;
        }

        public static string GetGlutenFreeTrait(HierarchyClass hierarchyClass)
        {
            var glutenFreeTrait = hierarchyClass.HierarchyClassTrait.Where(hct => hct.traitID == Traits.GlutenFree);
            return glutenFreeTrait.Count() == 0 ? String.Empty : glutenFreeTrait.Single().traitValue;
        }

        public static string GetKosherTrait(HierarchyClass hierarchyClass)
        {
            var kosherTrait = hierarchyClass.HierarchyClassTrait.Where(hct => hct.traitID == Traits.Kosher);
            return kosherTrait.Count() == 0 ? String.Empty : kosherTrait.Single().traitValue;
        }

        public static string GetNonGMOTrait(HierarchyClass hierarchyClass)
        {
            var nonGMOTrait = hierarchyClass.HierarchyClassTrait.Where(hct => hct.traitID == Traits.NonGmo);
            return nonGMOTrait.Count() == 0 ? String.Empty : nonGMOTrait.Single().traitValue;
        }

        public static string GetOrganicTrait(HierarchyClass hierarchyClass)
        {
            var organicTrait = hierarchyClass.HierarchyClassTrait.Where(hct => hct.traitID == Traits.Organic);
            return organicTrait.Count() == 0 ? String.Empty : organicTrait.Single().traitValue;
        }

        public static string GetVeganTrait(HierarchyClass hierarchyClass)
        {
            var veganTrait = hierarchyClass.HierarchyClassTrait.Where(hct => hct.traitID == Traits.Vegan);
            return veganTrait.Count() == 0 ? String.Empty : veganTrait.Single().traitValue;
        }

        public static bool IsDefaultCertificationAgency(HierarchyClass hierarchyClass)
        {
            var defaultAgnecyTrait = hierarchyClass.HierarchyClassTrait.Where(hct => hct.traitID == Traits.DefaultCertificationAgency);
            return defaultAgnecyTrait.Any();
        }
        public static bool GetProhibitDiscount(HierarchyClass hierarchyClass)
        {
            if (hierarchyClass.hierarchyLevel == HierarchyLevels.Gs1Brick)
            {
                var prohibitDiscountTrait = hierarchyClass.HierarchyClassTrait.SingleOrDefault(hct => hct.Trait.traitCode == TraitCodes.ProhibitDiscount);
                return prohibitDiscountTrait == null ? false : true;
            }
            else
            {
                return false;
            }            
        }

        public static string GetSubBrickCode(HierarchyClass hierarchyClass)
        {
            var subBrickCode = hierarchyClass.HierarchyClassTrait.Where(hct => hct.Trait.traitCode == TraitCodes.SubBrickCode);
            return subBrickCode.Count() == 0 ? String.Empty : subBrickCode.Single().traitValue;
        }

        public static string GetHierarchyClassLineage(HierarchyClass hierarchyClass)
        {
            if (hierarchyClass.hierarchyID == Hierarchies.Merchandise)
            {
                return hierarchyClass.ToFlattenedMerchandiseHierarchyClassString(includeSubTeam: true, includeSubBrickCode: true);
            }
            else
            {
                return hierarchyClass.ToFlattenedHierarchyClassString();
            }
        }

        public static string GetBrandAbbreviation(HierarchyClass hierarchyClass)
        {
            var brandAbbreviation = hierarchyClass.HierarchyClassTrait.Where(hct => hct.traitID == Traits.BrandAbbreviation);
            return brandAbbreviation.Count() == 0 ? String.Empty : brandAbbreviation.Single().traitValue;
        }
        public static string GetNationalClassCode(HierarchyClass hierarchyClass)
        {
            var nationalClassCodeQuery = hierarchyClass.HierarchyClassTrait.Where(hct => hct.traitID == Traits.NationalClassCode);
            return nationalClassCodeQuery.Count() == 0 ? String.Empty : nationalClassCodeQuery.Single().traitValue;
        }
    }
}