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
        [Display(Name = "Business Unit(s)")]
        [DataType(DataType.MultilineText)]
        public string[] Stores { get; set; }

        [Required]
        [Display(Name = "Scan Codes(s) (one per line)")]
        [DataType(DataType.MultilineText)]
        [RegularExpression(ValidationConstants.RegExForValidScanCode, ErrorMessage = ValidationConstants.ErrorMsgForInvalidScanCode)]
        public string ScanCodes { get; set; }

        public List<string> ScanCodesList
        {
            get
            {
                if (string.IsNullOrWhiteSpace(ScanCodes))
                {
                    return new List<string>();
                }
                else
                {
                    return ScanCodes.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
                }
            }
            set
            {
                if (value == null || value.Count < 1)
                {
                    ScanCodes = string.Empty;
                }
                else
                {
                    ScanCodes = string.Join(Environment.NewLine, value);
                }
            }
        }

        public List<int> StoresAsIntList
        {
            get
            {
                int eachBusinessUnit = 0;
                var businessUnitIDs = new List<int>(Stores.Length);
                foreach ( var store in Stores)
                {
                    if (int.TryParse(store, out eachBusinessUnit))
                    {
                        businessUnitIDs.Add(eachBusinessUnit);
                    }
                }

                return businessUnitIDs;
            }
   
        }

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

        public List<string> Errors { get; set; }
        public bool? Success { get; set; }
    }
}