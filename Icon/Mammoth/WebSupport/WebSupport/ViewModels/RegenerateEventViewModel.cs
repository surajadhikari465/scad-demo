using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using WebSupport.DataAccess;
using System.Collections.Generic;
using WebSupport.Helpers;
using WebSupport.Models;

namespace WebSupport.ViewModels
{
    public class RegenerateEventViewModel
    {
        public string Error { get; set; }
        public bool IsSuccess { get { return String.IsNullOrEmpty(Error); } }
        public DataTable ResultTable { get; set; }
		public IEnumerable<SelectListItem> OptionsForRegion { get; set; }
		public IEnumerable<SelectListItem> OptionsForStores { get; set; }
		public SelectListItem[] Events { get; set; }
        public SelectListItem[] Regions { get; set; }

		[DataType(DataType.Date)]
		[Display(Name = "Start Datetime")]
		public DateTime? StartDatetime { get; set; }

		[DataType(DataType.Date)]
		[Display(Name = "End Datetime")]
		public DateTime? EndDatetime { get; set; }

		[Required]
        [Display(Name = "Event Type")]
        public string EventType { get; set; }

		[Required]
		[Display(Name = "Region")]
		public int RegionIndex { get; set; }

		[Display(Name = "Store")]
		public int StoreIndex { get; set; }
		public bool? EventsGenerationSuccess { get; set; }
		public RegenerateEventViewModel()
		{
			StartDatetime = EndDatetime = DateTime.Now;

			var initialOption = new string[] { "- Select a region first -" };

			if (OptionsForStores == null || OptionsForStores.Count() <= 0)
			{
				OptionsForStores = SelectListHelper.ArrayToSelectList(initialOption);
			}

			if (OptionsForRegion == null || OptionsForRegion.Count() <= 0)
			{
				OptionsForRegion = SelectListHelper.ArrayToSelectList(StaticData.WholeFoodsRegions.ToArray(), 0);
			}

            Events = QueueEventTypes.Events.OrderBy(x => x.Value)
                .Select((x, i) => new SelectListItem { Text = x.Value, Value = x.Key .ToString(), Selected = (i == 0) })
                .OrderBy(x => x.Text).ToArray();
        }


		public void SetStoreOptions(IEnumerable<StoreViewModel> storeSelections)
		{
			OptionsForStores = SelectListHelper.StoreArrayToSelectList(storeSelections.ToArray());

		}
	}
}