using System;

namespace IconWebApi.Models
{
	public class Venue
	{
		public int VenueId { get; set; }
		public string VenueName { get; set; }
		public int StoreId { get; set; }
		public DateTime? OpenDate { get; set; }
		public DateTime? CloseDate { get; set; }
		public string SubType { get; set; }
	}
}