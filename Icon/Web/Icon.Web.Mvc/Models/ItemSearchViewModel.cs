using Icon.Web.Attributes;
using Icon.Web.Common;
using Icon.Web.Mvc.Extensions;
using Icon.Web.Mvc.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace Icon.Web.Mvc.Models
{
    [AtLeastOneRequiredSpecified]
    public class ItemSearchViewModel
    {
        private IEnumerable<DropDownViewModel> statuses;
        private IEnumerable<DropDownViewModel> hiddenStatuses;
        private SelectList foodStampOptions;
        private SelectList deptSaleOptions;
        private SelectList caseinFreeOptions;
        private SelectList hempOptions;
        private SelectList organicPersonalCareOptions;
        private SelectList paleoOptions;
        private SelectList localLoanProducerOptions;
        private SelectList nutritionRequiredOptions;

        public ItemSearchViewModel()
        {
            statuses = new List<DropDownViewModel>
            {
                new DropDownViewModel { Id = 1, Name = "All" },
                new DropDownViewModel { Id = 2, Name = "Loaded" },
                new DropDownViewModel { Id = 3, Name = "Validated" }
            };

            SelectedStatusId = statuses.First().Id;

            hiddenStatuses = new List<DropDownViewModel>
            {
                new DropDownViewModel {Id = 1, Name = "Visible"},
                new DropDownViewModel {Id = 2, Name = "Hidden"},
                new DropDownViewModel {Id = 3, Name = "All"}
            };

            SelectedHiddenItemStatusId = hiddenStatuses.Last().Id;

            var YesNoOptions = new List<string>() { String.Empty, "No", "Yes" };
            foodStampOptions = new SelectList(YesNoOptions, String.Empty);
            deptSaleOptions = new SelectList(YesNoOptions, String.Empty);
            caseinFreeOptions = new SelectList(YesNoOptions, String.Empty);
            hempOptions = new SelectList(YesNoOptions, String.Empty);
            organicPersonalCareOptions = new SelectList(YesNoOptions, String.Empty);
            paleoOptions = new SelectList(YesNoOptions, String.Empty);
            localLoanProducerOptions = new SelectList(YesNoOptions, String.Empty);
            nutritionRequiredOptions = new SelectList(YesNoOptions, String.Empty);

            ItemSearchResults = new ItemSearchResultsViewModel();
            ItemSignAttributes = new ItemSignAttributesSearchViewModel();
        }

        [ScanCode]
        [Display(Name = "Scan Code")]
        [IgGridRouteValue]
        [AtLeastOneRequiredProperty]
        public string ScanCode { get; set; }

        [IconPropertyValidation(ValidatorPropertyNames.ProductDescription, CanBeNullOrEmpty = true)]
        [Display(Name = "Product Description")]
        [IgGridRouteValue]
        [AtLeastOneRequiredProperty]
        public string ProductDescription { get; set; }

        [RetailSize]
        [Display(Name = "Size")]
        [IgGridRouteValue]
        [AtLeastOneRequiredProperty]
        public string RetailSize { get; set; }

        [Display(Name = "UOM")]
        [IgGridRouteValue]
        [AtLeastOneRequiredProperty]
        public string SelectedRetailUom { get; set; }

        [Display(Name = "Delivery System")]
        [IgGridRouteValue]
        [AtLeastOneRequiredProperty]
        public string DeliverySystem { get; set; }

        [IconPropertyValidation(ValidatorPropertyNames.BrandName, CanBeNullOrEmpty = true)]
        [Display(Name = "Brand")]
        [IgGridRouteValue]
        [AtLeastOneRequiredProperty]
        public string BrandName { get; set; }

        [Display(Name = "Allow Partial Searches")]
        [IgGridRouteValue]
        public bool PartialBrandName { get; set; }

        [Display(Name = "Merchandise Hierarchy")]
        [IgGridRouteValue]
        [AtLeastOneRequiredProperty]
        public string MerchandiseHierarchy { get; set; }

        [Display(Name = "Tax Class")]
        [IgGridRouteValue]
        [AtLeastOneRequiredProperty]
        public string TaxHierarchy { get; set; }

        [Display(Name = "National Class")]
        [IgGridRouteValue]
        [AtLeastOneRequiredProperty]
        public string NationalHierarchy { get; set; }

        [IconPropertyValidation(ValidatorPropertyNames.PosDescription, CanBeNullOrEmpty = true)]
        [Display(Name = "POS Description")]
        [IgGridRouteValue]
        [AtLeastOneRequiredProperty]
        public string PosDescription { get; set; }

        public IEnumerable<SelectListItem> Status
        {
            get
            {
                return statuses.ToSelectListItem();
            }
        }

        [Display(Name = "Item Status")]
        [IgGridRouteValue]
        public int SelectedStatusId { get; set; }

        public SelectList DeptSaleOptions
        {
            get
            {
                return deptSaleOptions;
            }
        }

        public IEnumerable<SelectListItem> HiddenStatus
        {
            get
            {
                return hiddenStatuses.ToSelectListItem();
            }
        }

        [Display(Name = "Hidden Status")]
        [IgGridRouteValue]
        public int SelectedHiddenItemStatusId { get; set; }

        [Display(Name = "Food Stamp Eligible")]
        [IgGridRouteValue]
        [AtLeastOneRequiredProperty]
        public string SelectedFoodStampId { get; set; }

        public SelectList FoodStampOptions
        {
            get
            {
                return foodStampOptions;
            }
        }

        [PosScaleTare]
        [Display(Name = "POS Scale Tare")]
        [IgGridRouteValue]
        [AtLeastOneRequiredProperty]
        public string PosScaleTare { get; set; }

        [Display(Name = "Item Pack")]
        [PackageUnit]
        [IgGridRouteValue]
        [AtLeastOneRequiredProperty]
        public string PackageUnit { get; set; }

        [Display(Name = "Department Sale")]
        [IgGridRouteValue]
        [AtLeastOneRequiredProperty]
        public string SelectedDepartmentSaleId { get; set; }

        [Display(Name = "Allow Partial Searches")]
        [PartialScanCode("ScanCode")]
        [IgGridRouteValue]
        public bool PartialScanCode { get; set; }

        public SelectList RetailUoms { get; set; }

        public string DeliverySystems { get; set; }

        [Display(Name = "Notes")]
        [IconPropertyValidation(ValidatorPropertyNames.Notes, CanBeNullOrEmpty = true)]
        [IgGridRouteValue]
        [AtLeastOneRequiredProperty]
        public string Notes { get; set; }

        [Display(Name = "Casein Free")]
        [IgGridRouteValue]
        [AtLeastOneRequiredProperty]
        public string CaseinFree { get; set; }
        public SelectList CaseinFreeOptions
        {
            get
            {
                return caseinFreeOptions;
            }
        }

        [Display(Name = "Drained Weight")]
        [IgGridRouteValue]
        [AtLeastOneRequiredProperty]
        public string DrainedWeight { get; set; }

        [Display(Name = "Drained Weight UOM")]
        [IgGridRouteValue]
        [AtLeastOneRequiredProperty]
        public string DrainedWeightUom { get; set; }

        [Display(Name = "Product Flavor Type")]
        [IgGridRouteValue]
        [AtLeastOneRequiredProperty]
        public string ProductFlavorType { get; set; }

        [Display(Name = "Main Product Name")]
        [IgGridRouteValue]
        [AtLeastOneRequiredProperty]
        public string MainProductName { get; set; }

        [Display(Name = "Alcohol By Volume")]
        [IgGridRouteValue]
        [AtLeastOneRequiredProperty]
        public string AlcoholByVolume { get; set; }

        [Display(Name = "Fair Trade Certified")]
        [IgGridRouteValue]
        [AtLeastOneRequiredProperty]
        public string FairTradeCertified { get; set; }
        public SelectList FairTradeCertifiedOptions { get; set; }

        [Display(Name = "Hemp")]
        [IgGridRouteValue]
        [AtLeastOneRequiredProperty]
        public string Hemp { get; set; }
        public SelectList HempOptions
        {
            get
            {
                return hempOptions;
            }
        }
        [Display(Name = "Organic Personal Care")]
        [IgGridRouteValue]
        [AtLeastOneRequiredProperty]
        public string OrganicPersonalCare { get; set; }
        public SelectList OrganicPersonalCareOptions
        {
            get
            {
                return organicPersonalCareOptions;
            }
        }

        [Display(Name = "Nutrition Required")]
        [IgGridRouteValue]
        [AtLeastOneRequiredProperty]
        public string NutritionRequired { get; set; }
        public SelectList NutritionRequiredOptions
        {
            get
            {
                return nutritionRequiredOptions;
            }
        }

        [Display(Name = "Paleo")]
        [IgGridRouteValue]
        [AtLeastOneRequiredProperty]
        public string Paleo { get; set; }
        public SelectList PaleoOptions
        {
            get
            {
                return paleoOptions;
            }
        }

        [Display(Name = "Local Loan Producer")]
        [IgGridRouteValue]
        [AtLeastOneRequiredProperty]
        public string LocalLoanProducer { get; set; }
        public SelectList LocalLoanProducerOptions
        {
            get
            {
                return localLoanProducerOptions;
            }
        }

        [Display(Name = "Created Date (mm/dd/yyyy)")]
        [IconPropertyValidation(ValidatorPropertyNames.CreatedDate, CanBeNullOrEmpty = true)]
        [IgGridRouteValue]
        [AtLeastOneRequiredProperty]
        public string CreatedDate { get; set; }

        [Display(Name = "Last Modified Date (mm/dd/yyyy)")]
        [IconPropertyValidation(ValidatorPropertyNames.ModifiedDate, CanBeNullOrEmpty = true)]
        [IgGridRouteValue]
        [AtLeastOneRequiredProperty]
        public string LastModifiedDate { get; set; }

        [Display(Name = "Last Modified User")]
        [IconPropertyValidation(ValidatorPropertyNames.ModifiedUser, CanBeNullOrEmpty = true)]
        [IgGridRouteValue]
        [AtLeastOneRequiredProperty]
        public string LastModifiedUser { get; set; }

        public int Page { get; set; }

        public int PageSize { get; set; }

        public ItemSearchResultsViewModel ItemSearchResults { get; set; }

        public ItemSignAttributesSearchViewModel ItemSignAttributes { get; set; }

        [IgGridRouteValue]
        public int TotalRecordsCount { get; set; }

        public RouteValueDictionary GetRouteValuesObject()
        {
            var routeValues = new RouteValueDictionary();

            var properties = this.GetType()
                .GetProperties()
                .Where(p => p.IsDefined(typeof(IgGridRouteValue), false));

            foreach (var property in properties)
            {
                AddRouteValueIfExists(property.Name, property.GetValue(this), routeValues);
            }

            var itemSignAttributesSearchProperties = typeof(ItemSignAttributesSearchViewModel)
                .GetProperties()
                .Where(p => p.IsDefined(typeof(IgGridRouteValue), false));

            foreach (var property in itemSignAttributesSearchProperties)
            {
                AddRouteValueIfExists("ItemSignAttributes." + property.Name, property.GetValue(ItemSignAttributes), routeValues);
            }

            return routeValues;
        }

        private void AddRouteValueIfExists(string propertyName, object propertyValue, RouteValueDictionary routeValues)
        {
            if (propertyValue != null)
            {
                if (propertyValue.GetType() == typeof(bool))
                {
                    if ((bool)propertyValue)
                    {
                        routeValues.Add(propertyName, propertyValue);
                    }
                }
                else if (!string.IsNullOrWhiteSpace(propertyValue.ToString()))
                {
                    routeValues.Add(propertyName, propertyValue);
                }
            }
        }
    }
}