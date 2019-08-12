using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
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
        [Display(Name = "Scan Codes(s) or Item ID(s) (one per line)")]
        [DataType(DataType.MultilineText)]
        [RegularExpression(ValidationConstants.RegExForValidScanCode, ErrorMessage = ValidationConstants.ErrorMsgForInvalidScanCode)]
        public string ScanCodes { get; set; }

        public List<string> ScanCodesList
        {
            get
            {
                return String.IsNullOrWhiteSpace(ScanCodes)
					? new List<string>()
					: ScanCodes.Replace(" ", String.Empty).Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).Distinct().ToList();
            }
            set
            {
                ScanCodes = (value == null || value.Count < 1)
					? String.Empty
					: String.Join(Environment.NewLine, value);
            }
        }

        public List<int> StoresAsIntList
        {
            get
            {
				int id = 0;
				return Stores == null ? new List<int>() : Stores.Where(x => int.TryParse(x, out id)).Select(x => id).ToList();
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
        public bool IsItemId { get; set; }
    }
}