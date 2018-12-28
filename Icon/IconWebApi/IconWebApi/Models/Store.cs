using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace IconWebApi.Models
{
	public class Store
	{
		public Store()
		{
			//Venues = new HashSet<Venue>();
		}

		public int StoreId { get; set; }
		public string StoreName { get; set; }
		public int MetroId { get; set; }
		public int BusinessUnitId { get; set; }
		public string StoreAbbreviation { get; set; }
		public string CurrencyCode { get; set; }
		public DateTime? OpenDate { get; set; }
		public DateTime? CloseDate { get; set; }
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public Address StoreAddress { get; set; }
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public IEnumerable<Venue> Venues { get; set; }
	}
}