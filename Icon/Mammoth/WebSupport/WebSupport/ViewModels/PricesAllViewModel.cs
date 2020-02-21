using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using WebSupport.Helpers;

namespace WebSupport.ViewModels
{
    public class PricesAllViewModel
    {
        public List<SelectListItem> Downstreams { get; set; }
		public List<SelectListItem> Regions { get; set; }
		public List<SelectListItem> Stores { get; set; }

		[Required]
		[Display(Name = "Business Units")]
		[DataType(DataType.MultilineText)]
		public string[] SelectedStores { get; set; }

        [Required]
		[Display(Name = "Downstream Systems")]
		[DataType(DataType.MultilineText)]
		public int[] DownstreamSystems { get; set; }

        [Required]
		[Display(Name = "Region")]
		public int RegionIndex { get; set; }

        public PricesAllViewModel()
        {
            Regions = new List<SelectListItem>();
            Stores = new List<SelectListItem>();
            Downstreams = new List<SelectListItem>();
        }

        public void SetRegionAndSystems(IEnumerable<string> regions, IEnumerable<string> systems)
		{
			this.Regions = regions == null || !regions.Any()
				? new List<SelectListItem>()
				: SelectListHelper.ArrayToSelectList(regions.ToArray(), 0).ToList();

			this.Downstreams = systems == null || !regions.Any()
				? new List<SelectListItem>()
				: SelectListHelper.ArrayToSelectList(systems.ToArray()).ToList();
		}

		public void SetStoreOptions(StoreViewModel[] stores)
		{
			this.Stores = SelectListHelper.StoreArrayToSelectList(stores).ToList();
		}
    }
}