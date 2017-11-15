using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using WebSupport.DataAccess;
using WebSupport.Helpers;
using WebSupport.Models;

namespace WebSupport.ViewModels
{
    public class CheckPointRequestViewModel
    {
        public IEnumerable<SelectListItem> OptionsForRegion { get; set; }

        public IEnumerable<SelectListItem> OptionsForStores { get; set; }

        public CheckPointRequestViewModel()
        {
            var initialOption = new string[] { "- Select Region First -" };
            OptionsForStores = SelectListHelper.ArrayToSelectList(initialOption);
            OptionsForRegion = new List<SelectListItem>();
        }

        [Required]
        [Display(Name = "Region")]
        public int RegionIndex { get; set; }

        [Required]
        [Display(Name = "Business Unit")]
        [DataType(DataType.MultilineText)]
        public string Store { get; set; }

        [Required]
        [Display(Name = "Scan Code")]
        [RegularExpression(ValidationConstants.RegExForValidScanCode, ErrorMessage = ValidationConstants.ErrorMsgForInvalidScanCode)]
        public string ScanCode { get; set; }
        public void SetRegions(IEnumerable<string> regionSelections)
        {
            if (regionSelections != null)
            {
                OptionsForRegion = SelectListHelper.ArrayToSelectList(regionSelections.ToArray(), 0);
            } 
        }

        public void SetStoreOptions(IEnumerable<StoreViewModel> storeSelections)
        {
            OptionsForStores = SelectListHelper.StoreArrayToSelectList(storeSelections.ToArray());
        }
    }
}