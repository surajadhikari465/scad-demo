﻿using Icon.Framework;
using Icon.Web.Attributes;
using Icon.Web.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Icon.Web.DataAccess.Models;
using Icon.Web.Common;
using System.Linq;

namespace Icon.Web.Mvc.Models
{
    public class HierarchyClassViewModel
    {
        private IEnumerable<DropDownViewModel> nonMerchandiseTraits;

        public int HierarchyId { get; set; }
        [Display(Name = "Hierarchy")]
        public string HierarchyName { get; set; }
        public int HierarchyClassId { get; set; }
        public int? HierarchyLevel { get; set; }
        public int? HierarchyParentClassId { get; set; }
        [Display(Name = "Parent Class")]
        public string HierarchyParentClassName { get; set; }
        public string HierarchyClassLineage { get; set; }

        [Required]
        [HierarchyClassName]
        [MaxLength(255)]
        [Display(Name = "Class Name")]
        public virtual string HierarchyClassName { get; set; }

        public string HierarchyLevelName { get; set; }

        // Specific Hierarchy Class Traits
        [Display(Name = "SubTeam")]
        public string SubTeam { get; set; }

        [Required]
        [Display(Name = "SubTeam")]
        public int SelectedSubTeam { get; set; }

        public IEnumerable<SelectListItem> SubTeamList { get; set; }

        [Display(Name = "Tax Abbreviation")]
        [IconPropertyValidation(ValidatorPropertyNames.TaxAbbreviation, CanBeNullOrEmpty = true)]
        public string TaxAbbreviation { get; set; }

        [Display(Name = "Tax Romance")]
        [IconPropertyValidation(ValidatorPropertyNames.TaxRomance, CanBeNullOrEmpty = true)]
        public string TaxRomance { get; set; }

        public IEnumerable<SelectListItem> NonMerchandiseTraitList
        {
            get
            {
                return nonMerchandiseTraits.ToSelectListItem();
            }
        }
        public int SelectedNonMerchandiseTrait { get; set; }

        [Display(Name = "Non-Merchandise Trait")]
        public string NonMerchandiseTrait { get; set; }

        [Display(Name = "Prohibit Discount")]
        public bool ProhibitDiscount { get; set; }

        [Display(Name = "SubBrick Code")]
        [MaxLength(255)]
        public string SubBrickCode { get; set; }
        public string ContactLastUpdatedBy { get; private set; }

        public HierarchyClassViewModel()
        {
            // Populating the NonMerchandise traits for IEnuemerable NonMerchandiseList
            nonMerchandiseTraits = new List<DropDownViewModel>
            {
                new DropDownViewModel { Id = 0, Name = String.Empty },
                new DropDownViewModel { Id = 1, Name = NonMerchandiseTraits.BlackhawkFee },
                new DropDownViewModel { Id = 2, Name = NonMerchandiseTraits.BottleDeposit },
                new DropDownViewModel { Id = 3, Name = NonMerchandiseTraits.BottleReturn },
                new DropDownViewModel { Id = 4, Name = NonMerchandiseTraits.Coupon },
                new DropDownViewModel { Id = 5, Name = NonMerchandiseTraits.Crv },
                new DropDownViewModel { Id = 6, Name = NonMerchandiseTraits.CrvCredit },
                new DropDownViewModel { Id = 7, Name = NonMerchandiseTraits.LegacyPosOnly },
                new DropDownViewModel { Id = 8, Name = NonMerchandiseTraits.NonRetail }
            };

            // Default
            SelectedNonMerchandiseTrait = 0;
        }

        public HierarchyClassViewModel(HierarchyClass hierarchyClass)
        {
            HierarchyId = hierarchyClass.hierarchyID;
            HierarchyLevel = hierarchyClass.hierarchyLevel;
            HierarchyClassId = hierarchyClass.hierarchyClassID;
            HierarchyParentClassId = hierarchyClass.hierarchyParentClassID;
            HierarchyClassName = hierarchyClass.hierarchyClassName;

            HierarchyParentClassName = HierarchyClassAccessor.GetHierarchyParentName(hierarchyClass);
            HierarchyClassLineage = HierarchyClassAccessor.GetHierarchyClassLineage(hierarchyClass);
            HierarchyLevelName = HierarchyClassAccessor.GetHierarchyLevelName(hierarchyClass);

            SubTeam = HierarchyClassAccessor.GetSubTeam(hierarchyClass);
            TaxAbbreviation = HierarchyClassAccessor.GetTaxAbbreviation(hierarchyClass);
            NonMerchandiseTrait = HierarchyClassAccessor.GetNonMerchandiseTrait(hierarchyClass);
            ProhibitDiscount = HierarchyClassAccessor.GetProhibitDiscount(hierarchyClass);
            SubBrickCode = HierarchyClassAccessor.GetSubBrickCode(hierarchyClass);
            ContactLastUpdatedBy = hierarchyClass.HierarchyClassTrait
                .Where(x => String.Compare(x.Trait.traitCode, "HCU", true) == 0)
                .FirstOrDefault() ? .traitValue;
        }

        public HierarchyClassViewModel(HierarchyClassModel hierarchyClassModel)
        {
            HierarchyId = hierarchyClassModel.HierarchyId;
            HierarchyClassId = hierarchyClassModel.HierarchyClassId;
            HierarchyParentClassId = hierarchyClassModel.HierarchyParentClassId;
            HierarchyClassName = hierarchyClassModel.HierarchyClassName;
            HierarchyLevel = hierarchyClassModel.HierarchyLevel;
            HierarchyClassLineage = hierarchyClassModel.HierarchyLineage;
        }
    }
}
