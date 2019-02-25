using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KitBuilderWebApi.QueryParameters
{
	public class ItemNutritionAttributes
	{
		public int ItemId { get; set; }
		public int Calories { get; set; }
		public decimal ServingsPerPortion { get; set; }
		public string ServingSizeDesc { get; set; }
		public string ServingPerContainer { get; set; }
	}
}
