﻿
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace Mammoth.Common.DataAccess
{
    /// <summary>
    /// dbo.Attributes auto generated Ids
    /// </summary>

    [GeneratedCode("TextTemplatingFileGenerator", "10")]
    public static class Attributes
    {
		public const int AgeRestrict = 1;
		public const int AuthorizedForSale = 2;
		public const int CaseDiscountEligible = 3;
		public const int ChicagoBaby = 4;
		public const int ColorAdded = 5;
		public const int CountryOfProcessing = 6;
		public const int Discontinued = 7;
		public const int ElectronicShelfTag = 8;
		public const int Exclusive = 9;
		public const int LabelTypeDesc = 10;
		public const int LinkedScanCode = 11;
		public const int LocalItem = 12;
		public const int Locality = 13;
		public const int Msrp = 14;
		public const int NumberOfDigitsSentToScale = 15;
		public const int Origin = 16;
		public const int ProductCode = 17;
		public const int RestrictedHours = 18;
		public const int ScaleExtraText = 19;
		public const int SignCaption = 20;
		public const int SoldByWeight = 21;
		public const int TagUom = 22;
		public const int TmDiscountEligible = 23;
		public const int SignRomanceShort = 24;
		public const int SignRomanceLong = 25;
		public const int RetailUnit = 26;
		public const int CustomerFriendlyDescription = 27;
		public const int FairTrade = 28;
		public const int FlexibleText = 29;
		public const int MadeWithOrganicGrapes = 30;
		public const int MadeWithBiodynamicGrapes = 31;
		public const int NutritionRequired = 32;
		public const int PrimeBeef = 33;
		public const int RainforestAlliance = 34;
		public const int RefrigeratedOrShelfStable = 35;
		public const int SmithsonianBirdFriendly = 36;
		public const int Wic = 37;
		
		public class Descriptions
		{
			public const string AgeRestrict = "Age Restrict";
			public const string AuthorizedForSale = "Authorized For Sale";
			public const string CaseDiscountEligible = "Case Discount Eligible";
			public const string ChicagoBaby = "Chicago Baby";
			public const string ColorAdded = "Color Added";
			public const string CountryOfProcessing = "Country of Processing";
			public const string Discontinued = "Discontinued";
			public const string ElectronicShelfTag = "Electronic Shelf Tag";
			public const string Exclusive = "Exclusive";
			public const string LabelTypeDesc = "Label Type Desc";
			public const string LinkedScanCode = "Linked Scan Code";
			public const string LocalItem = "Local Item";
			public const string Locality = "Locality";
			public const string Msrp = "MSRP";
			public const string NumberOfDigitsSentToScale = "Number of Digits Sent To Scale";
			public const string Origin = "Origin";
			public const string ProductCode = "Product Code";
			public const string RestrictedHours = "Restricted Hours";
			public const string ScaleExtraText = "Scale Extra Text";
			public const string SignCaption = "Sign Caption";
			public const string SoldByWeight = "Sold by Weight";
			public const string TagUom = "Tag UOM";
			public const string TmDiscountEligible = "TM Discount Eligible";
			public const string SignRomanceShort = "Sign Romance Short";
			public const string SignRomanceLong = "Sign Romance Long";
			public const string RetailUnit = "Retail Unit";
			public const string CustomerFriendlyDescription = "Customer Friendly Description";
			public const string FairTrade = "Fair Trade";
			public const string FlexibleText = "Flexible Text";
			public const string MadeWithOrganicGrapes = "Made With Organic Grapes";
			public const string MadeWithBiodynamicGrapes = "Made with Biodynamic Grapes";
			public const string NutritionRequired = "Nutrition Required";
			public const string PrimeBeef = "Prime Beef";
			public const string RainforestAlliance = "Rainforest Alliance";
			public const string RefrigeratedOrShelfStable = "Refrigerated or Shelf Stable";
			public const string SmithsonianBirdFriendly = "Smithsonian Bird Friendly";
			public const string Wic = "WIC";
		
			private static Dictionary<string, string> codeToDescriptionsDictionary = new Dictionary<string, string>
			{
				{ "AGE", "Age Restrict" },
				{ "NA", "Authorized For Sale" },
				{ "CSD", "Case Discount Eligible" },
				{ "CHB", "Chicago Baby" },
				{ "CLA", "Color Added" },
				{ "COP", "Country of Processing" },
				{ "DSC", "Discontinued" },
				{ "EST", "Electronic Shelf Tag" },
				{ "EX", "Exclusive" },
				{ "LTD", "Label Type Desc" },
				{ "LSC", "Linked Scan Code" },
				{ "LI", "Local Item" },
				{ "LCY", "Locality" },
				{ "SRP", "MSRP" },
				{ "NDS", "Number of Digits Sent To Scale" },
				{ "ORN", "Origin" },
				{ "PCD", "Product Code" },
				{ "RES", "Restricted Hours" },
				{ "SET", "Scale Extra Text" },
				{ "SC", "Sign Caption" },
				{ "SBW", "Sold by Weight" },
				{ "TU", "Tag UOM" },
				{ "TMD", "TM Discount Eligible" },
				{ "SHT", "Sign Romance Short" },
				{ "LNG", "Sign Romance Long" },
				{ "RTU", "Retail Unit" },
				{ "CFD", "Customer Friendly Description" },
				{ "FT", "Fair Trade" },
				{ "FXT", "Flexible Text" },
				{ "MOG", "Made With Organic Grapes" },
				{ "MBG", "Made with Biodynamic Grapes" },
				{ "NR", "Nutrition Required" },
				{ "PRB", "Prime Beef" },
				{ "RFA", "Rainforest Alliance" },
				{ "RFD", "Refrigerated or Shelf Stable" },
				{ "SBF", "Smithsonian Bird Friendly" },
				{ "WIC", "WIC" }
			};
			public static Dictionary<string, string> ByCode { get { return codeToDescriptionsDictionary; } }
		}

		public class Codes
		{
			public const string AgeRestrict = "AGE";
			public const string AuthorizedForSale = "NA";
			public const string CaseDiscountEligible = "CSD";
			public const string ChicagoBaby = "CHB";
			public const string ColorAdded = "CLA";
			public const string CountryOfProcessing = "COP";
			public const string Discontinued = "DSC";
			public const string ElectronicShelfTag = "EST";
			public const string Exclusive = "EX";
			public const string LabelTypeDesc = "LTD";
			public const string LinkedScanCode = "LSC";
			public const string LocalItem = "LI";
			public const string Locality = "LCY";
			public const string Msrp = "SRP";
			public const string NumberOfDigitsSentToScale = "NDS";
			public const string Origin = "ORN";
			public const string ProductCode = "PCD";
			public const string RestrictedHours = "RES";
			public const string ScaleExtraText = "SET";
			public const string SignCaption = "SC";
			public const string SoldByWeight = "SBW";
			public const string TagUom = "TU";
			public const string TmDiscountEligible = "TMD";
			public const string SignRomanceShort = "SHT";
			public const string SignRomanceLong = "LNG";
			public const string RetailUnit = "RTU";
			public const string CustomerFriendlyDescription = "CFD";
			public const string FairTrade = "FT";
			public const string FlexibleText = "FXT";
			public const string MadeWithOrganicGrapes = "MOG";
			public const string MadeWithBiodynamicGrapes = "MBG";
			public const string NutritionRequired = "NR";
			public const string PrimeBeef = "PRB";
			public const string RainforestAlliance = "RFA";
			public const string RefrigeratedOrShelfStable = "RFD";
			public const string SmithsonianBirdFriendly = "SBF";
			public const string Wic = "WIC";
		}
	}
}
