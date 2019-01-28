using System;
using System.Collections.Generic;
using System.Text;

namespace KitBuilder.DataAccess.Dto
{
	public class ItemStorePriceModel
	{
		public int ItemId { get; set; }
		public string ScanCode { get; set; }
		public int BusinessUnitID { get; set; }
		public bool Authorized { get; set; }
		public string PriceType { get; set; }
		public int Multiple { get; set; }
		public decimal Price { get; set; }
		public string Currency { get; set; }
	}
}
