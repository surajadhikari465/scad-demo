using Newtonsoft.Json;
using System.Collections.Generic;

namespace IconWebApi.Models
{
	public class Region
	{
		public Region()
		{
			//Metros = new HashSet<Metro>();
		}
		public int RegionId { get; set; }
		public string RegionName { get; set; }
		public string RegionCode { get; set; }
		public int ChainId { get; set; }
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public IEnumerable<Metro> Metros { get; set; }
	}
}