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

        public string SelectedEventType { get; set; }

        public IEnumerable<string> GetScanCodes()
        {
            var parsedScanCodes = this.ScanCodesText.Split(
                new string[] { Environment.NewLine },
                StringSplitOptions.RemoveEmptyEntries);

            return parsedScanCodes.Select(s => s.Trim()).Distinct();
        }
    }
}
