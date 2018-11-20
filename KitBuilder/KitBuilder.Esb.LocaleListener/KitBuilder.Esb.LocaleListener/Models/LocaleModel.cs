using System;

namespace KitBuilder.Esb.LocaleListener.Models
{
	public class LocaleModel
	{
		public int LocaleID { get; set; }//storeId when venue exists else venueId 
		public string LocaleName { get; set; }//storeName when venue exists else venueName
		public int LocaleTypeID { get; set; }
		public int? StoreID { get; set; }//storeId if venue related locale info is being filled
		public int MetroID { get; set; }
		public int RegionID { get; set; }
		public int ChainID { get; set; }
		public string RegionCode { get; set; }// region Name
		public DateTime? LocaleOpenDate { get; set; }
		public DateTime? LocaleCloseDate { get; set; }
		public int? BusinessUnitID { get; set; }// store related trait
		public string StoreAbbreviation { get; set; }// store related trait
		public string CurrencyCode { get; set; }// store related trait
		public bool Hospitality { get; set; }//venue related trait
	}
}
