﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using WebSupport.Helpers;
using WebSupport.Models;

namespace WebSupport.ViewModels
{
    public class RefreshPsgViewModel
    {
        public const int MaxNumberOfItems = 1000;

        public RefreshPsgViewModel()
        {
            var initialOption = new string[] { "- select region first -" };
            OptionsForStores = SelectListHelper.ArrayToSelectList(initialOption);
            OptionsForRegion = new List<SelectListItem>();
        }

        public void SetRegion(IEnumerable<string> regionSelections)
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

        [Required]
        [Display(Name = "Region")]
        public int RegionIndex { get; set; }

        [Required]
        [Display(Name = "Business Unit(s)")]
        [DataType(DataType.MultilineText)]
        public string[] Stores { get; set; }

        [Required]
        [Display(Name = "Scan Codes(s) (one per line)")]
        [DataType(DataType.MultilineText)]
        [RegularExpression(ValidationConstants.RegExForValidScanCode, ErrorMessage = ValidationConstants.ErrorMsgForInvalidScanCode)]
        public string Items { get; set; }

        public IEnumerable<SelectListItem> OptionsForRegion { get; set; }

        public IEnumerable<SelectListItem> OptionsForStores { get; set; }

        public List<string> Errors { get; set; }
        public bool? Success { get; set; }
        public string NonExistingScanCodesMessage { get; set; }
    }
}