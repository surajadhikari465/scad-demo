using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using WebSupport.Helpers;
using WebSupport.Models;

namespace WebSupport.ViewModels
{
	public class PriceRefreshViewModel
	{
		public const int MaxNumberOfItems = 100;

		public PriceRefreshViewModel()
		{
			var initialOption = new string[] { "- select region first -" };
			OptionsForStores = SelectListHelper.ArrayToSelectList(initialOption);
			OptionsForRegion = new List<SelectListItem>();
			OptionsForDestinationSystem = new List<SelectListItem>();
		}

		public void SetRegionAndSystemOptions(IEnumerable<string> regionSelections,
			IEnumerable<string> downstreamSystemSelections)
		{
			if(regionSelections != null)
			{
				OptionsForRegion = SelectListHelper.ArrayToSelectList(regionSelections.ToArray(), 0);
			}
			if(downstreamSystemSelections != null)
			{
				OptionsForDestinationSystem = SelectListHelper.ArrayToSelectList(downstreamSystemSelections.ToArray());
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
		[Display(Name = "Downstream System(s)")]
		[DataType(DataType.MultilineText)]
		public int[] DownstreamSystems { get; set; }

		[Required]
		[Display(Name = "Business Unit(s)")]
		[DataType(DataType.MultilineText)]
		public string[] Stores { get; set; }

		[Required]
		[Display(Name = "Scan Codes(s) or Item ID(s) (one per line)")]
		[DataType(DataType.MultilineText)]
		[RegularExpression(ValidationConstants.RegExForValidScanCode, ErrorMessage = ValidationConstants.ErrorMsgForInvalidScanCode)]
		public string Items { get; set; }

		public IEnumerable<SelectListItem> OptionsForRegion { get; set; }

		public IEnumerable<SelectListItem> OptionsForDestinationSystem { get; set; }

		public IEnumerable<SelectListItem> OptionsForStores { get; set; }

		public List<string> Errors { get; set; }
		public bool? Success { get; set; }
		public bool IsItemId { get; set; }

		public List<string> Codes
		{
			get
			{
				return String.IsNullOrWhiteSpace(Items)
					? new List<string>()
					: Items.Replace(" ", String.Empty)
						.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
						.Distinct().ToList();
			}
		}
	}
}