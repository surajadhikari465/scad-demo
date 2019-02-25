using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KitBuilderWebApi.QueryParameters
{
	public class ItemNutritionRequestModel
	{
		public IEnumerable<int> ItemIds { get; set; }
	}
}
