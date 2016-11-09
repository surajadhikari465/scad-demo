namespace WebSupport.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;

    using DataAccess;

    public class EventCreatorViewModel
    {
        /// <summary>
        /// Available regions abbreviations.
        /// </summary>
        private static string[] WholeFoodsRegions = new[]
            {
                "FL", "MA", "MW", "NA", "RM", "SO", "NC", "NE", "PN", "SP", "SW", "UK"
            };

        private static string[] MammothEventTypes = new[]
            {
                EventConstants.ItemLocaleAddOrUpdateEvent,
                EventConstants.ItemPriceEvent
            };

        [Required]
        [DataType(DataType.MultilineText)]
        [RegularExpression(@"^[1-9][0-9]{0,12}(\r?\n[1-9][0-9]{0,12})*(\r?\n)*$", ErrorMessage = "Invalid scan code format.")]
        public string ScanCodesText { get; set; }

        public SelectList AvailableRegions
        {
            get
            {
                return new SelectList(WholeFoodsRegions);
            }
        }

        public string SelectedRegion { get; set; }

        public SelectList AvailableEventTypes
        {
            get
            {
                return new SelectList(MammothEventTypes);
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
