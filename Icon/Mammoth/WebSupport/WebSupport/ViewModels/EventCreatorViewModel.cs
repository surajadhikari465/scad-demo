namespace WebSupport.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;
    using WebSupport.Models;

    public class EventCreatorViewModel
    {
        [Required]
        [DataType(DataType.MultilineText)]
        [RegularExpression(ValidationConstants.RegExForValidScanCode, ErrorMessage = ValidationConstants.ErrorMsgForInvalidScanCode)]
        public string ScanCodesText { get; set; }

        public SelectList AvailableRegions
        {
            get
            {
                return new SelectList(StaticData.WholeFoodsRegions);
            }
        }

        public string SelectedRegion { get; set; }

        public SelectList AvailableEventTypes
        {
            get
            {
                return new SelectList(StaticData.MammothEventTypes);
            }
        }

        public bool IsItemId { get; set; }
        public string SelectedEventType { get; set; }

        public List<string> Codes
        {
			get
			{
				return String.IsNullOrWhiteSpace(ScanCodesText)
					? new List<string>()
					: ScanCodesText.Replace(" ", String.Empty)
						.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                        .Distinct().ToList();
			}
        }
    }
}
