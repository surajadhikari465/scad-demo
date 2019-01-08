using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MammothGpmService.Models
{
	public class DbPriceData
	{
		public string Region { get; set; }
		public string ScanCode { get; set; }
		public int? ItemId { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public int? Price { get; set; }
		public int? PercentageOff { get; set; }
		public string PriceType { get; set; }
		public string PriceReasonCode { get; set; }
		public string SellableUOM { get; set; }
		public string CurrencyCode { get; set; }
		public int? Multiple { get; set; }
		public DateTime? TagExpirationDate { get; set; }
		public DateTime? InsertDate { get; set; }
		public DateTime? ModifiedDate { get; set; }
		public bool? Authorized { get; set; }
	}
}
