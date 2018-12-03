using Icon.Framework;
using Icon.Web.Attributes;
using Icon.Web.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Icon.Web.Mvc.Utility;
using Icon.Web.Extensions;
using System;
using Icon.Web.DataAccess.Models;
using Icon.Web.Common.Utility;

namespace Icon.Web.Mvc.Models
{
    public class ItemEditViewModel
    {
        private ItemSearchModel item;

        public int ItemId { get; set; }

        [Display(Name = "Scan Code")]
        public string ScanCode { get; set; }

        [Display(Name = "Brand")]
        [ValueRequired]
        public string BrandName { get; set; }

        [Display(Name = "Product Description")]
        [ValueRequired]
        [IconPropertyValidation(ValidatorPropertyNames.ProductDescription, CanBeNullOrEmprty = false)]
        public string ProductDescription { get; set; }

        [Display(Name = "POS Description")]
        [ValueRequired]
        [IconPropertyValidation(ValidatorPropertyNames.PosDescription, CanBeNullOrEmprty = false)]
        public string PosDescription { get; set; }

        [Display(Name = "Item Pack")]
        [PackageUnit]
        [ValueRequired]
        public string PackageUnit { get; set; }

        [Display(Name = "Size")]
        [RetailSize]
        public string RetailSize { get; set; }

        [Display(Name = "UOM")]
        [RetailUom]
        public string RetailUom { get; set; }
        public SelectList RetailUoms { get; set; }

        [Display(Name = "Delivery System")]
        public string DeliverySystem { get; set; }

        [Display(Name = "Food Stamp Eligible")]
        public bool FoodStampEligible { get; set; }

        [Display(Name = "POS Scale Tare")]
        [PosScaleTare]
        public string PosScaleTare { get; set; }

        [Display(Name = "Department Sale")]
        public bool DepartmentSale { get; set; }

        [Display(Name = "Browsing Hierarchy")]
        public string BrowsingHierarchyName { get; set; }
        public string BrowsingHierarchyLineage { get; set; }

        [Display(Name = "Merchandise Hierarchy")]
        public string MerchandiseHierarchyClassName { get; set; }
        public string MerchandiseHierarchyClassLineage { get; set; }

        [Display(Name = "Tax Class")]
        public string TaxHierarchyClassName { get; set; }
        public string TaxHierarchyClassLineage { get; set; }

        public int? BrowsingHierarchyClassSelectedId { get; set; }
        public IEnumerable<SelectListItem> BrowsingHierarchyClasses { get; set; }

        public int? MerchandiseHierarchyClassSelectedId { get; set; }
        public IEnumerable<SelectListItem> MerchandiseHierarchyClasses { get; set; }

        public int? TaxHierarchyClassSelectedId { get; set; }
        public IEnumerable<SelectListItem> TaxHierarchyClasses { get; set; }

        [Display(Name = "National Class")]
        public string NationalHierarchyClassName { get; set; }
        public string NationalHierarchyClassLineage { get; set; }

        public int? NationalHierarchyClassSelectedId { get; set; }
        public IEnumerable<SelectListItem> NationalHierarchyClasses { get; set; }

        [Display(Name = "Hidden Item")]
        public bool HiddenItem { get; set; }

        public string Notes { get; set; }

        public DateTime? ValidatedDate { get; set; }

        public bool IsValidated { get { return ValidatedDate != null; } }

        [Display(Name = "Animal Welfare Rating")]
        public string AnimalWelfareRating { get; set; }

        [Display(Name = "Biodynamic")]
        public string SelectedBiodynamicOption { get; set; }
        public SelectList BiodynamicOptions { get; set; }

        [Display(Name = "Cheese Attribute: Milk Type")]
        public string CheeseMilkType { get; set; }

        [Display(Name = "Cheese Attribute: Raw")]
        public string SelectedCheeseRawOption { get; set; }
        public SelectList CheeseRawOptions { get; set; }

        [Display(Name = "Eco Scale Rating")]
        public string EcoScaleRating { get; set; }

        [Display(Name = "Gluten Free")]
        public string GlutenFreeAgency { get; set; }

        [Display(Name = "Kosher")]
        public string KosherAgency { get; set; }

        [Display(Name = "MSC")]
        public string SelectedMscOption { get; set; }
        public SelectList MscOptions { get; set; }

        [Display(Name = "Non-GMO")]
        public string NonGmoAgency { get; set; }

        [Display(Name = "Organic")]
        public string OrganicAgency { get; set; }

        [Display(Name = "Premium Body Care")]
        public string SelectedPremiumBodyCareOption { get; set; }
        public SelectList PremiumBodyCareOptions { get; set; }

        [Display(Name = "Fresh or Frozen")]
        public string SeafoodFreshOrFrozen { get; set; }

        [Display(Name = "Seafood: Wild or Farm Raised")]
        public string SeafoodCatchType { get; set; }

        [Display(Name = "Vegan")]
        public string VeganAgency { get; set; }

        [Display(Name = "Vegetarian")]
        public string SelectedVegetarianOption { get; set; }
        public SelectList VegetarianOptions { get; set; }

        [Display(Name = "Whole Trade")]
        public string SelectedWholeTradeOption { get; set; }

        [Display(Name = "Grass Fed")]
        public string SelectedGrassFedOption { get; set; }
        public SelectList GrassFedOptions { get; set; }

        [Display(Name = "Pasture Raised")]
        public string SelectedPastureRaisedOption { get; set; }
        public SelectList PastureRaisedOptions { get; set; }

        [Display(Name = "Free Range")]
        public string SelectedFreeRangeOption { get; set; }

        public SelectList FreeRangeOptions { get; set; }
        [Display(Name = "Dry Aged")]
        public string SelectedDryAgedOption { get; set; }
        public SelectList DryAgedOptions { get; set; }

        [Display(Name = "Air Chilled")]
        public string SelectedAirChilledOption { get; set; }
        public SelectList AirChilledOptions { get; set; }

        [Display(Name = "Made In House")]
        public string SelectedMadeInHouseOption { get; set; }
        public SelectList MadeInHouseOptions { get; set; }

        public SelectList WholeTradeOptions { get; set; }

        [Display(Name = "Drained Weight UOM")]
        [IconPropertyValidation(ValidatorPropertyNames.DrainedWeightUom, CanBeNullOrEmprty = true)]
        public string DrainedWeightUom { get; set; }

        [Display(Name = "Fair Trade Certified")]
        [IconPropertyValidation(ValidatorPropertyNames.FairTradeCertified, CanBeNullOrEmprty = true)]
        public string FairTradeCertified { get; set; }
        public SelectList FairTradeCertifiedOptions { get; set; }

        public bool Hemp { get; set; }

        [Display(Name = "Alcohol By Volume")]
        [IconPropertyValidation(ValidatorPropertyNames.AlcoholByVolume, CanBeNullOrEmprty = true)]
        public string AlcoholByVolume { get; set; }

        [Display(Name = "Casein Free")]
        public bool CaseinFree { get; set; }

        [Display(Name = "Drained Weight")]
        [IconPropertyValidation(ValidatorPropertyNames.DrainedWeight, CanBeNullOrEmprty = true)]
        public string DrainedWeight { get; set; }

        [Display(Name = "Local Loan Producer")]
        public bool LocalLoanProducer { get; set; }

        [Display(Name = "Main Product Name")]
        [IconPropertyValidation(ValidatorPropertyNames.MainProductName, CanBeNullOrEmprty = true)]
        public string MainProductName { get; set; }

        [Display(Name = "Nutrition Required")]
        public bool NutritionRequired { get; set; }

        [Display(Name = "Product Flavor Type")]
        [IconPropertyValidation(ValidatorPropertyNames.ProductFlavorType, CanBeNullOrEmprty = true)]
        public string ProductFlavorType { get; set; }

        public bool Paleo { get; set; }

        [Display(Name = "Organic Personal Care")]
        public bool OrganicPersonalCare { get; set; }

        public ItemEditViewModel()
        {
            BuildSelectLists();
        }

        public ItemEditViewModel(ItemSearchModel item) : this()
        {
            ItemId = item.ItemId;
            ScanCode = item.ScanCode;

            BrandName = item.BrandName;
            ProductDescription = item.ProductDescription;
            PosDescription = item.PosDescription;
            PackageUnit = item.PackageUnit;
            RetailSize = item.RetailSize;
            RetailUom = item.RetailUom;
            DeliverySystem = item.DeliverySystem;
            FoodStampEligible = ConversionUtility.ConvertBitStringToBool(item.FoodStampEligible);
            PosScaleTare = item.PosScaleTare;
            DepartmentSale = ConversionUtility.ConvertBitStringToBool(item.DepartmentSale);
            HiddenItem = ConversionUtility.ConvertBitStringToBool(item.HiddenItem);
            Notes = item.Notes;
            ValidatedDate = item.ValidatedDate == null ? (DateTime?)null : DateTime.Parse(item.ValidatedDate);
            AnimalWelfareRating = item.AnimalWelfareRating;
            SelectedBiodynamicOption = ConversionUtility.ConvertNullableBoolToYesNoFull(item.Biodynamic);
            CheeseMilkType = item.CheeseMilkType;
            SelectedCheeseRawOption = ConversionUtility.ConvertNullableBoolToYesNoFull(item.CheeseRaw);
            EcoScaleRating = item.EcoScaleRating;
            GlutenFreeAgency = item.GlutenFreeAgency;
            KosherAgency = item.KosherAgency;
            SelectedMscOption = ConversionUtility.ConvertNullableBoolToYesNoFull(item.Msc);
            NonGmoAgency = item.NonGmoAgency;
            OrganicAgency = item.OrganicAgency;
            SelectedPremiumBodyCareOption = ConversionUtility.ConvertNullableBoolToYesNoFull(item.PremiumBodyCare);
            SeafoodFreshOrFrozen = item.SeafoodFreshOrFrozen;
            SeafoodCatchType = item.SeafoodCatchType;
            VeganAgency = item.VeganAgency;
            SelectedVegetarianOption = ConversionUtility.ConvertNullableBoolToYesNoFull(item.Vegetarian);
            SelectedWholeTradeOption = ConversionUtility.ConvertNullableBoolToYesNoFull(item.WholeTrade);
            SelectedGrassFedOption = ConversionUtility.ConvertNullableBoolToYesNoFull(item.GrassFed);
            SelectedPastureRaisedOption = ConversionUtility.ConvertNullableBoolToYesNoFull(item.PastureRaised);
            SelectedFreeRangeOption = ConversionUtility.ConvertNullableBoolToYesNoFull(item.FreeRange);
            SelectedDryAgedOption = ConversionUtility.ConvertNullableBoolToYesNoFull(item.DryAged);
            SelectedAirChilledOption = ConversionUtility.ConvertNullableBoolToYesNoFull(item.AirChilled);
            SelectedMadeInHouseOption = ConversionUtility.ConvertNullableBoolToYesNoFull(item.MadeInHouse);

            MerchandiseHierarchyClassName = item.MerchandiseHierarchyName;
            MerchandiseHierarchyClassSelectedId = item.MerchandiseHierarchyClassId;
            MerchandiseHierarchyClassLineage = item.MerchandiseHierarchyName;

            TaxHierarchyClassName = item.TaxHierarchyName;
            TaxHierarchyClassSelectedId = item.TaxHierarchyClassId;
            TaxHierarchyClassLineage = item.TaxHierarchyName;

            BrowsingHierarchyName = item.BrowsingHierarchyName;
            BrowsingHierarchyClassSelectedId = item.BrowsingHierarchyClassId;
            BrowsingHierarchyLineage = item.BrowsingHierarchyName;

            NationalHierarchyClassName = item.NationalHierarchyName;
            NationalHierarchyClassSelectedId = item.NationalHierarchyClassId;
            NationalHierarchyClassLineage = item.NationalHierarchyName;

            AlcoholByVolume = item.AlcoholByVolume;
            CaseinFree = ConversionUtility.ConvertBitStringToBool(item.CaseinFree);
            DrainedWeight = item.DrainedWeight;
            DrainedWeightUom = item.DrainedWeightUom;
            FairTradeCertified = item.FairTradeCertified;
            Hemp = ConversionUtility.ConvertBitStringToBool(item.Hemp);
            LocalLoanProducer = ConversionUtility.ConvertBitStringToBool(item.LocalLoanProducer);
            MainProductName = item.MainProductName;
            NutritionRequired = ConversionUtility.ConvertBitStringToBool(item.NutritionRequired);
            OrganicPersonalCare = ConversionUtility.ConvertBitStringToBool(item.OrganicPersonalCare);
            Paleo = ConversionUtility.ConvertBitStringToBool(item.Paleo);
            ProductFlavorType = item.ProductFlavorType;
        }

        private void BuildSelectLists()
        {           
            BiodynamicOptions = ViewModelHelpers.BuildYesOrNoSelectList();
            CheeseRawOptions = ViewModelHelpers.BuildYesOrNoSelectList();
            PremiumBodyCareOptions = ViewModelHelpers.BuildYesOrNoSelectList();
            VegetarianOptions = ViewModelHelpers.BuildYesOrNoSelectList();
            WholeTradeOptions = ViewModelHelpers.BuildYesOrNoSelectList();
            MscOptions = ViewModelHelpers.BuildYesOrNoSelectList();
            GrassFedOptions = ViewModelHelpers.BuildYesOrNoSelectList();
            PastureRaisedOptions = ViewModelHelpers.BuildYesOrNoSelectList();
            FreeRangeOptions = ViewModelHelpers.BuildYesOrNoSelectList();
            DryAgedOptions = ViewModelHelpers.BuildYesOrNoSelectList();
            AirChilledOptions = ViewModelHelpers.BuildYesOrNoSelectList();
            MadeInHouseOptions = ViewModelHelpers.BuildYesOrNoSelectList();
        }
    }
}