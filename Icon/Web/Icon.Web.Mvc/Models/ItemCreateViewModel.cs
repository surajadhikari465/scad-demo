using Icon.Web.Attributes;
using Icon.Web.Common;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Collections.Generic;
using Icon.Web.DataAccess.Models;
using Icon.Web.Mvc.Utility;
using Icon.Framework;

namespace Icon.Web.Mvc.Models
{
    public class ItemCreateViewModel
    {
        public ItemCreateViewModel()
        {
            CheeseMilkTypes = ViewModelHelpers.BuildSelectListFromDictionary(Icon.Framework.MilkTypes.AsDictionary, true);
            EcoScaleRatings = ViewModelHelpers.BuildSelectListFromDictionary(Icon.Framework.EcoScaleRatings.AsDictionary, true);
            AnimalWelfareRatings = ViewModelHelpers.BuildSelectListFromDictionary(Icon.Framework.AnimalWelfareRatings.AsDictionary, true);
            SeafoodFreshOrFrozen = ViewModelHelpers.BuildSelectListFromDictionary(Icon.Framework.SeafoodFreshOrFrozenTypes.AsDictionary, true);
            SeafoodCatchTypes = ViewModelHelpers.BuildSelectListFromDictionary(Icon.Framework.SeafoodCatchTypes.AsDictionary, true);
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
            DrainedWeightUomOptions = new SelectList(DrainedWeightUoms.Values);
        }

        [Display(Name = "Scan Code")]
        [Required]
        [ScanCode]
        public string ScanCode { get; set; }

        [Display(Name = "Brand Name")]
        [Required]
        [IconPropertyValidation(ValidatorPropertyNames.BrandName, CanBeNullOrEmprty = false)]
        public string BrandName { get; set; }

        [Display(Name = "Product Description")]
        [Required]
        [IconPropertyValidation(ValidatorPropertyNames.ProductDescription, CanBeNullOrEmprty = false)]
        public string ProductDescription { get; set; }

        [Display(Name = "POS Description")]
        [Required]
        [IconPropertyValidation(ValidatorPropertyNames.PosDescription, CanBeNullOrEmprty = false)]
        public string PosDescription { get; set; }

        [Display(Name = "Item Pack")]
        [PackageUnit]
        [Required]
        public string PackageUnit { get; set; }

        [Display(Name = "Size")]
        [RetailSize]
        [Required]
        public string RetailSize { get; set; }

        [Display(Name = "UOM")]
        [RetailUom]
        [Required]
        public string RetailUom { get; set; }
        public SelectList RetailUoms { get; set; }

        [Display(Name = "Delivery System")]
        [DeliverySystem]
        public string DeliverySystem { get; set; }
        public SelectList DeliverySystems { get; set; }

        [Display(Name = "Food Stamp Eligible")]
        [Required]
        public bool FoodStampEligible { get; set; }

        [Display(Name = "POS Scale Tare")]
        [PosScaleTare]
        public string PosScaleTare { get; set; }

        public int? MerchandiseHierarchyClassId { get; set; }
        public List<HierarchyClassViewModel> MerchandiseHierarchyClasses { get; set; }

        [Display(Name = "Merchandise Hierarchy")]
        public string MerchandiseHierarchyName { get; set; }

        [Display(Name = "Tax Hierarchy")]
        public string TaxHierarchyClassName { get; set; }

        public int? TaxHierarchyClassId { get; set; }
        public List<HierarchyClassViewModel> TaxHierarchyClasses { get; set; }

        [Display(Name = "Browsing Hierarchy")]
        public int? BrowsingHierarchyClassId { get; set; }
        public SelectList BrowsingHierarchyClasses { get; set; }

        public int? NationalHierarchyClassId { get; set; }
        public List<HierarchyClassViewModel> NationalHierarchyClasses { get; set; }

        [Display(Name = "National Hierarchy")]
        public string NationalHierarchyName { get; set; }

        [Display(Name = "Animal Welfare Rating")]
        public int? SelectedAnimalWelfareRatingId { get; set; }
        public SelectList AnimalWelfareRatings { get; set; }

        [Display(Name = "Biodynamic")]
        public string SelectedBiodynamicOption { get; set; }
        public SelectList BiodynamicOptions { get; set; }

        [Display(Name = "Cheese Attribute: Milk Type")]
        public int? SelectedCheeseMilkTypeId { get; set; }
        public SelectList CheeseMilkTypes { get; set; }

        [Display(Name = "Cheese Attribute: Raw")]
        public string SelectedCheeseRawOption { get; set; }
        public SelectList CheeseRawOptions { get; set; }

        [Display(Name = "Eco Scale Rating")]
        public int? SelectedEcoScaleRatingId { get; set; }
        public SelectList EcoScaleRatings { get; set; }

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
        public int? SelectedSeafoodFreshOrFrozenId { get; set; }
        public SelectList SeafoodFreshOrFrozen { get; set; }

        [Display(Name = "Seafood: Wild or Farm Raised")]
        public int? SelectedSeafoodCatchTypeId { get; set; }
        public SelectList SeafoodCatchTypes { get; set; }

        [Display(Name = "Vegan")]
        public string VeganAgency { get; set; }

        [Display(Name = "Vegetarian")]
        public string SelectedVegetarianOption { get; set; }
        public SelectList VegetarianOptions { get; set; }

        [Display(Name = "Whole Trade")]
        public string SelectedWholeTradeOption { get; set; }
        public SelectList WholeTradeOptions { get; set; }

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

        public SelectList GlutenFreeAgencies { get; set; }
        public SelectList KosherAgencies { get; set; }
        public SelectList NonGmoAgencies { get; set; }
        public SelectList OrganicAgencies { get; set; }
        public SelectList VeganAgencies { get; set; }

        [Display(Name = "Drained Weight UOM")]
        [IconPropertyValidation(ValidatorPropertyNames.DrainedWeightUom, CanBeNullOrEmprty = true)]
        public string DrainedWeightUom { get; set; }
        public SelectList DrainedWeightUomOptions { get; set; }

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
    }
}