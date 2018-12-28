using Newtonsoft.Json;
using System.Collections.Generic;

namespace IconWebApi.Models
{
	public class Metro
	{
		public Metro()
		{
			//Stores = new HashSet<Store>();
		}
		public int MetroId { get; set; }
		public string MetroName { get; set; }
		public int RegionId { get; set; }
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public IEnumerable<Store> Stores { get; set; }
	}
}