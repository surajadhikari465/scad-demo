using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IconWebApi.Models
{
	public class Address
	{
		public string AddressLine1 { get; set; }
		public string AddressLine2 { get; set; }
		public string AddressLine3 { get; set; }
		public string cityName { get; set; }
		public string territoryCode { get; set; }
		public string postalCode { get; set; }
		public string countryCode { get; set; }
		public string countryName { get; set; }
	}
}