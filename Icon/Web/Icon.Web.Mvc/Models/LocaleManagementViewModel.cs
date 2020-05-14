﻿using Icon.Web.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Icon.Web.Mvc.Models
{
	public class LocaleManagementViewModel
	{
		public int LocaleId { get; set; }

		[Display(Name = "Locale Name")]
		[HierarchyClassName]
		[Required]
		public string LocaleName { get; set; }

		public int ParentLocaleId { get; set; }

		[Display(Name = "Parent Locale")]
		public string ParentLocaleName { get; set; }

		[Display(Name = "Open Date")]
		[DataType(DataType.Date)]
		public DateTime? OpenDate { get; set; }

		public int OwnerOrgPartyId { get; set; }

		public int? LocaleTypeId { get; set; }

		[Display(Name = "Business Unit")]
		[BusinessUnit]
		[RegularExpression(@"^[1-9][0-9]{4}$", ErrorMessage = "Please enter a numerical value between 10000 and 99999")]
		[Required]
		public string BusinessUnit { get; set; }

		[Display(Name = "Store Abbreviation")]
		[Required]
		[MaxLength(3)]
		[MinLength(3)]
		[RegularExpression(@"^[A-Z]{3}$", ErrorMessage = "Must be three uppercase letters.")]
		public string StoreAbbreviation { get; set; }

		public string EwicAgencyId { get; set; }

		[Display(Name = "eWIC Agency")]
		public IEnumerable<SelectListItem> EwicAgencies { get; set; }


		// Physical Address Information
		[Required]
		[Display(Name = "Address Line 1")]
		[DataType(DataType.Text)]
		[MaxLength(200)]
		public string AddressLine1 { get; set; }

		[Display(Name = "Address Line 2")]
		[DataType(DataType.Text)]
		[MaxLength(200)]
		public string AddressLine2 { get; set; }

		[Display(Name = "Address Line 3")]
		[DataType(DataType.Text)]
		[MaxLength(200)]
		public string AddressLine3 { get; set; }

		[Display(Name = "Country")]
		public IEnumerable<SelectListItem> CountryList { get; set; }
		[Required]
		public string CountryId { get; set; }

		[Display(Name = "State / Province")]
		public IEnumerable<SelectListItem> TerritoryList { get; set; }
		[Required]
		public int TerritoryId { get; set; }

		[Required]
		[MaxLength(50)]
		public string City { get; set; }

		[Required]
		[Display(Name = "Postal Code")]
		[RegularExpression(@"^[\w ]+$", ErrorMessage = "Must include only letters, numbers, and spaces.")]
		[MaxLength(15)]
		[MinLength(5)]
		public string PostalCode { get; set; }

		[Display(Name = "Time Zone")]
		public IEnumerable<SelectListItem> TimeZones { get; set; }

		[Required]
		public int TimeZoneId { get; set; }

		[Required]
		[MaxLength(255)]
		[MinLength(2)]
		[RegularExpression(@"^[A-Za-z \-']+$", ErrorMessage = "County must be only letters, spaces, dashes, and apostrophes.")]
		public string County { get; set; }

		[Display(Name = "Latitude (decimal degrees)")]
		[RegularExpression(@"^-?([1-8]?[0-9]\.\d{6}$|90\.0{6}$)", ErrorMessage = "Must be a number with six decimal places between -90.000000 and 90.000000.")]
		public decimal? Latitude { get; set; }

		[Display(Name = "Longitude (decimal degrees)")]
		[RegularExpression(@"^-?((180\.[0]{6})|((1[0-7]\d)|\d?\d)\.[\d]{6}$)", ErrorMessage = "Must be a number with six decimal places between -180.000000 and 180.000000.")]
		public decimal? Longitude { get; set; }

		// Section of regex before first pipe is for USA/Canada phone numbers, e.g. (^([0-9]{3}-)[0-9]{3}-[0-9]{4}$)
		// Section of regex after first pipe is for UK phone numbers, e.g. ^((\(?0\d{4}\)?\s?\d{3}\s?\d{3})|(\(?0\d{3}\)?\s?\d{3}\s?\d{4})|(\(?0\d{2}\)?\s?\d{4}\s?\d{4}))?$
		[Required]
		[Display(Name = "Phone Number")]
		[MaxLength(20)]
		[RegularExpression(@"(^([0-9]{3}-)[0-9]{3}-[0-9]{4}$)|^((\(?0\d{4}\)?\s?\d{3}\s?\d{3})|(\(?0\d{3}\)?\s?\d{3}\s?\d{4})|(\(?0\d{2}\)?\s?\d{4}\s?\d{4}))?$",
			ErrorMessage = "Phone numbers for North America must be 10 digits with dashes, no parenthesis allowed. XXX-XXX-XXXX. " +
							"Phone numbers for United Kingdom need to be 11 digits with no dashes, parenthesis around first set of numbers. " +
							"Examples: (XXX) XXXX XXXX, (XXXX) XXXXXXX")]
		public string PhoneNumber { get; set; }

		[Display(Name = "Contact Person")]
		[MaxLength(40)]
		[RegularExpression(@"^[\w \.\-']+$", ErrorMessage = "Must consist of only letters, spaces, dashes, periods, and apostrophes.")]
		public string ContactPerson { get; set; }

		[Display(Name = "Fax")]
		[RegularExpression(@"(^([0-9]{3}-)[0-9]{3}-[0-9]{4}$)|^((\(?0\d{4}\)?\s?\d{3}\s?\d{3})|(\(?0\d{3}\)?\s?\d{3}\s?\d{4})|(\(?0\d{2}\)?\s?\d{4}\s?\d{4}))?$",
			ErrorMessage = "Fax numbers for North America must be 10 digits with dashes, no parenthesis allowed. XXX-XXX-XXXX. " +
							"Fax numbers for United Kingdom need to be 11 digits with no dashes, parenthesis around first set of numbers. " +
							"Examples: (XXX) XXXX XXXX, (XXXX) XXXXXXX")]
		public string Fax { get; set; }

		[Display(Name = "IRMA Store Number")]
		[RegularExpression(@"^$|^([0-9]){1,5}$", ErrorMessage = "IRMA Store Numbers must be numbers with no more than 5 digits.")]
		public string IrmaStoreId { get; set; }

		public string SelectedStorePosType { get; set; }
		[Display(Name = "POS Type")]
		public IEnumerable<SelectListItem> StorePosTypes { get; set; }

		[Display(Name = "Ident")]
		public bool Ident { get; set; }

		[Display(Name = "PrimeNow Merchant ID")]
		[RegularExpression(@"^[0-9]{11}$", ErrorMessage = "PrimeNow Merchant ID must be numeric with 11 digits length.")]
		public string PrimeMerchantID { get; set; }

		[Display(Name = "PrimeNow Merchant ID Encrypted")]
		[RegularExpression(@"^[a-zA-Z0-9]{1,50}$", ErrorMessage = "PrimeNow Merchant ID Encrypted must be alpha-numeric with up to 50 characters length.")]
		public string PrimeMerchantIDEncrypted { get; set; }

		[Display(Name = "Local Zone")]
		[RegularExpression(@"^.{1,35}$", ErrorMessage = "Local Zone should be alpha-numeric with up to 35 characters length.")]
		public string LocalZone { get; set; }

		public string SelectedLiquorLicense { get; set; }
		[Display(Name = "Liquor License")]
		[RegularExpression(@"^(Beer|Wine|Spirit)$", ErrorMessage = "Invalid Liquor License. Valid values are Beer, Wine or Spirit")]
		public IEnumerable<SelectListItem> LiquorLicenseTypes { get; set; }

		public string SelectedCurrency { get; set; }
		[Display(Name = "Currency")]
		[Editable(false)]
		public IEnumerable<SelectListItem> CurrencyTypes { get; set; }

		[Required]
		public int LocaleSubTypeId { get; set; }

		[Display(Name = "LocaleSubType")]
		[Editable(false)]
		public IEnumerable<SelectListItem> LocaleSubTypes { get; set; }

		[Display(Name = "Venue Code")]
		public string VenueCode { get; set; }
		[Display(Name = "Venue Occupant")]
		public string VenueOccupant { get; set; }
		[Display(Name = "LocaleS ubType")]
		public string LocaleSubType { get; set; }
		[Display(Name = "Sodium Warning Required")]
		public bool SodiumWarningRequired { get; set; }
	}
}