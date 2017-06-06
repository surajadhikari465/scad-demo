CREATE TYPE infor.LocaleAddressAddOrUpdateType AS TABLE
(	AddressId INT NULL,
	AddressLine1 NVARCHAR(255) NULL,
	AddressLine2 NVARCHAR(255) NULL,
	AddressLine3 NVARCHAR(255) NULL,
	CityName NVARCHAR(255) NULL,
	PostalCode NVARCHAR(15) NULL,
	CountryCode NVARCHAR(3) NULL,
	TerritoryCode NVARCHAR(3) NULL,
	TimeZoneName NVARCHAR(255) NULL,
	Latitude NVARCHAR(15) NULL,
	Longitude NVARCHAR(15) NULL,
	BusinessUnitId INT NULL
)