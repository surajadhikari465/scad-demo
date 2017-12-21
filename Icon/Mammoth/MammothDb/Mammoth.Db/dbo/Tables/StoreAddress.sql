CREATE TABLE dbo.StoreAddress
(
	BusinessUnitID INT NOT NULL,
	Address1 NVARCHAR(255),
	Address2 NVARCHAR(255),
	Address3 NVARCHAR(255),
	City NVARCHAR(100),
	Territory NVARCHAR(100),
	TerritoryAbbrev NVARCHAR(5),
	PostalCode NVARCHAR(20),
	Country NVARCHAR(255),
	CountryAbbrev NVARCHAR(10),
	Timezone NVARCHAR(255),
	ModifiedDate DATETIME2(7),
	AddedDate DATETIME2(7),
    CONSTRAINT PK_StoreAddress PRIMARY KEY CLUSTERED (BusinessUnitID ASC) WITH (FILLFACTOR = 100),
)