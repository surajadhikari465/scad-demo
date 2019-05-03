using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using WebSupport.DataAccess;


namespace WebSupport.ViewModels
{
    public class RequeueEventViewModel
    {
        public string Error { get; set; }
        public bool IsSuccess { get { return String.IsNullOrEmpty(Error); } }
        public DataTable ResultTable { get; set; }
        public SelectListItem[] Events { get; set; }
        public SelectListItem[] Regions { get; set; }

        [DataType(DataType.Date)]
		[Display(Name = "Date From")]
		public DateTime DateFrom { get; set; }

        [DataType(DataType.Date)]
		[Display(Name = "Date To")]
		public DateTime DateTo { get; set; }

        [Required]
        [Display(Name = "Event Type")]
        public string EventType { get; set; }

        [Required]
		[Display(Name = "Region")]
		public string Region { get; set; }

        public RequeueEventViewModel()
		{
            DateFrom = DateTo = DateTime.Now;
            Regions = DataConstants.WholeFoodsRegions.Distinct(StringComparer.InvariantCultureIgnoreCase).OrderBy(x => x)
                                   //.Where(x => x != RegionNameConstants.UK)
                                   .Select((x, i) => new SelectListItem { Text = x, Value = x, Selected = (i == 0) })
                                   .ToArray();

            Events = QueueEventTypes.Events.OrderBy(x => x.Value)
                .Select((x, i) => new SelectListItem { Text = x.Value, Value = x.Key .ToString(), Selected = (i == 0) })
                .OrderBy(x => x.Text).ToArray();
        }
    }
}