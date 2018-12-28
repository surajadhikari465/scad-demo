using Newtonsoft.Json;
using System.Collections.Generic;

namespace IconWebApi.Models
{
	public class Chain
	{
		public Chain()
		{
			//Regions = new HashSet<Region>();
		}
		public int ChainId { get; set; }
		public string ChainName { get; set; }
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public IEnumerable<Region> Regions { get; set; }
	}
}