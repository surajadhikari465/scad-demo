using Icon.Web.Mvc.Utility;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Linq;
using Icon.Web.Mvc.Attributes;

namespace Icon.Web.Mvc.Models
{
    public class ItemSignAttributesSearchViewModel
    {
        public ItemSignAttributesSearchViewModel()
        {
            CheeseMilkTypes = ViewModelHelpers.BuildSelectListFromDictionary(Icon.Framework.MilkTypes.AsDictionary, true);
            EcoScaleRatings = ViewModelHelpers.BuildSelectListFromDictionary(Icon.Framework.EcoScaleRatings.AsDictionary, true);
            AnimalWelfareRatings = ViewModelHelpers.BuildSelectListFromDictionary(Icon.Framework.AnimalWelfareRatings.AsDictionary, true);            
            SeafoodFreshOrFrozen = ViewModelHelpers.BuildSelectListFromDictionary(Icon.Framework.SeafoodFreshOrFrozenTypes.AsDictionary, true, true);
            SeafoodCatchTypes = ViewModelHelpers.BuildSelectListFromDictionary(Icon.Framework.SeafoodCatchTypes.AsDictionary, true);
            BiodynamicOptions = ViewModelHelpers.BuildYesOrNoSelectList();
            CheeseRawOptions = ViewModelHelpers.BuildYesOrNoSelectList();
            PremiumBodyCareOptions = ViewModelHelpers.BuildYesOrNoSelectList();
            VegetarianOptions = ViewModelHelpers.BuildYesOrNoSelectList();
            WholeTradeOptions = ViewModelHelpers.BuildYesOrNoSelectList();
            GrassFedOptions = ViewModelHelpers.BuildYesOrNoSelectList();
            FreeRangeOptions = ViewModelHelpers.BuildYesOrNoSelectList();
            PastureRaisedOptions = ViewModelHelpers.BuildYesOrNoSelectList();
            AirChilledOptions = ViewModelHelpers.BuildYesOrNoSelectList();
            MadeInHouseOptions = ViewModelHelpers.BuildYesOrNoSelectList();
            DryAgedOptions = ViewModelHelpers.BuildYesOrNoSelectList();
            MscOptions = ViewModelHelpers.BuildYesOrNoSelectList();
        }

        [Display(Name = "Animal Welfare Rating")]
        [IgGridRouteValue]
        [AtLeastOneRequiredProperty]
        public int? SelectedAnimalWelfareRatingId { get; set; }
        public SelectList AnimalWelfareRatings { get; set; }

        [Display(Name = "Biodynamic")]
        [IgGridRouteValue]
        [AtLeastOneRequiredProperty]
        public string SelectedBiodynamicOption { get; set; }
        public SelectList BiodynamicOptions { get; set; }

        [Display(Name = "Cheese Attribute: Milk Type")]
        [IgGridRouteValue]
        [AtLeastOneRequiredProperty]
        public int? SelectedCheeseMilkTypeId { get; set; }
        public SelectList CheeseMilkTypes { get; set; }

        [Display(Name = "Cheese Attribute: Raw")]
        [IgGridRouteValue]
        [AtLeastOneRequiredProperty]
        public string SelectedCheeseRawOption { get; set; }
        public SelectList CheeseRawOptions { get; set; }

        [Display(Name = "Eco-Scale Rating")]
        [IgGridRouteValue]
        [AtLeastOneRequiredProperty]
        public int? SelectedEcoScaleRatingId { get; set; }
        public SelectList EcoScaleRatings { get; set; }

        [Display(Name = "Gluten-Free")]
        [IgGridRouteValue]
        [AtLeastOneRequiredProperty]
        public string GlutenFreeAgency { get; set; }

        [Display(Name = "Kosher")]
        [IgGridRouteValue]
        [AtLeastOneRequiredProperty]
        public string KosherAgency { get; set; }

        [Display(Name = "MSC")]
        [IgGridRouteValue]
        [AtLeastOneRequiredProperty]
        public string SelectedMscOption { get; set; }
        public SelectList MscOptions { get; set; }

        [Display(Name = "Non-GMO")]
        [IgGridRouteValue]
        [AtLeastOneRequiredProperty]
        public string NonGmoAgency { get; set; }

        [Display(Name = "Organic")]
        [IgGridRouteValue]
        [AtLeastOneRequiredProperty]
        public string OrganicAgency { get; set; }

        [Display(Name = "Premium Body Care")]
        [IgGridRouteValue]
        [AtLeastOneRequiredProperty]
        public string SelectedPremiumBodyCareOption { get; set; }
        public SelectList PremiumBodyCareOptions { get; set; }        

        [Display(Name = "Fresh or Frozen")]
        [IgGridRouteValue]
        [AtLeastOneRequiredProperty]
        public int? SelectedSeafoodFreshOrFrozenId { get; set; }
        public SelectList SeafoodFreshOrFrozen { get; set; }

        [Display(Name = "Seafood: Wild or Farm Raised")]
        [IgGridRouteValue]
        [AtLeastOneRequiredProperty]
        public int? SelectedSeafoodCatchTypeId { get; set; }
        public SelectList SeafoodCatchTypes { get; set; }

        [Display(Name = "Vegan")]
        [IgGridRouteValue]
        [AtLeastOneRequiredProperty]
        public string VeganAgency { get; set; }

        [Display(Name = "Vegetarian")]
        [IgGridRouteValue]
        [AtLeastOneRequiredProperty]
        public string SelectedVegetarianOption { get; set; }
        public SelectList VegetarianOptions { get; set; }

        [Display(Name = "Whole Trade")]
        [IgGridRouteValue]
        [AtLeastOneRequiredProperty]
        public string SelectedWholeTradeOption { get; set; }
        public SelectList WholeTradeOptions { get; set; }

        [Display(Name = "Grass Fed")]
        [IgGridRouteValue]
        [AtLeastOneRequiredProperty]
        public string SelectedGrassFedOption { get; set; }
        public SelectList GrassFedOptions { get; set; }

        [Display(Name = "Pasture Raised")]
        [IgGridRouteValue]
        [AtLeastOneRequiredProperty]
        public string SelectedPastureRaisedOption { get; set; }
        public SelectList PastureRaisedOptions { get; set; }

        [Display(Name = "Free Range")]
        [IgGridRouteValue]
        [AtLeastOneRequiredProperty]
        public string SelectedFreeRangeOption { get; set; }
        public SelectList FreeRangeOptions { get; set; }

        [Display(Name = "Dry Aged")]
        [IgGridRouteValue]
        [AtLeastOneRequiredProperty]
        public string SelectedDryAgedOption { get; set; }
        public SelectList DryAgedOptions { get; set; }

        [Display(Name = "Air Chilled")]
        [IgGridRouteValue]
        [AtLeastOneRequiredProperty]
        public string SelectedAirChilledOption { get; set; }
        public SelectList AirChilledOptions { get; set; }

        [Display(Name = "Made In House")]
        [IgGridRouteValue]
        [AtLeastOneRequiredProperty]
        public string SelectedMadeInHouseOption { get; set; }
        public SelectList MadeInHouseOptions { get; set; }
    }
}