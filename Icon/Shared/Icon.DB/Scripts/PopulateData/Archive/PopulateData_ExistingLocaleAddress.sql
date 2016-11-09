-- Date:		07/25/2014
-- TFS 2012:	3969
-- Description: This script will add all Stores and Addresses into Icon.
--				This will also make sure that each store has a LocaleTrait
--				for Business Unit ID, Store Abbreviation, Insert Date, and Modification Date

create table #addressList
(
	StoreAbbr nvarchar(3),
	StoreName nvarchar(255),
	Status nvarchar(4),
	AddressLine1 nvarchar(255),
	AddressLine2 nvarchar(255) null,
	AddressLine3 nvarchar(255) null,
	City nvarchar(50),
	County nvarchar(50),
	StateProvince nvarchar(100),
	PostalCode nvarchar(15),
	Region nvarchar(50),
	Country nvarchar(30),
	CBSA nvarchar(100),
	Longitude decimal(9,6),
	Latitude decimal(9,6),
	OpenDate datetime2,
	Metro nvarchar(30),
	BU nvarchar(5),
	TimeZone nvarchar(3)
)

DECLARE @serverName nvarchar(50)
SET @serverName = (SELECT @@SERVERNAME)

IF @serverName = 'CEWD1815\SQLSHARED2012D'
BEGIN
	bulk insert #addressList
	from '\\irmadevfile\e$\ICONData\WFM_Stores_Addresses.txt'
	with (fieldterminator = '\t', rowterminator = '\n', codepage = 'ACP', firstrow = 2);
END
ELSE IF (@serverName = 'QA-SQLSHARED3\SQLSHARED3Q' OR @serverName = 'SQLSHARED3-PRD3\SHARED3P')
BEGIN
	bulk insert #addressList
	from '\\irmaqafile\e$\IconData\WFM_Stores_Addresses.txt'
	with (fieldterminator = '\t', rowterminator = '\n', codepage = 'ACP', firstrow = 2);
END
GO

BEGIN TRY
BEGIN TRANSACTION

	-- *********************************************
	-- Delete Store from Locale hierarchy
	-- *********************************************\
	PRINT 'Start deleting any existing Address or Locale information related to existing Stores...'
	DELETE FROM PhysicalAddress
	DELETE FROM PostalCode
	DELETE FROM City
	DELETE FROM County
	DELETE FROM LocaleAddress
	DELETE FROM Address
	DELETE FROM LocaleAddress
	DELETE lt
		FROM 
			LocaleTrait lt 
			JOIN Locale l on lt.localeID = l.localeID
			JOIN LocaleType ltp on l.localeTypeID = ltp.localeTypeID
		WHERE 
			ltp.localeTypeDesc = 'Store'
	DELETE FROM Store
	DELETE FROM ItemTrait WHERE localeID <> 1
	DELETE FROM ItemPrice WHERE localeID <> 1
	DELETE l FROM Locale l JOIN LocaleType lt on l.localeTypeID = lt.localeTypeID where lt.localeTypeDesc = 'Store'
	PRINT 'Finished deleting any existing Address or Locale information related to existing Stores...'

	-- *********************************************
	-- Add Locales and LocaleTraits
	-- *********************************************
	 --Get Metro IDs so we have the ParentIDs
	select
		al.StoreAbbr,
		al.StoreName,
		al.BU,
		al.Metro,
		l.localeID		as metroLocaleID
	into #storeMetro
	from
		#addressList	al
		JOIN Locale		l	on al.Metro = l.localeName
		JOIN LocaleType lt	on l.localeTypeID = lt.localeTypeID
	where
		lt.localeTypeDesc = 'Metro'

	-- Add Stores to Locale hierarchy
	PRINT 'Start inserting dbo.Locale data...'
	DECLARE @storeLocaleType int
	SET @storeLocaleType = (SELECT localeTypeID FROM LocaleType WHERE localeTypeDesc = 'Store')

	INSERT INTO dbo.Locale
	SELECT
		1					as ownderOrgPartyID,
		al.StoreName		as localeName,
		al.OpenDate			as localeOpenDate,
		NULL				as localeCloseDate,
		@storeLocaleType	as localeTypeID,
		sm.metroLocaleID	as parentLocaleID
	FROM
		#addressList		al
		JOIN #storeMetro	sm on al.BU = sm.BU
	PRINT 'Finished inserting dbo.Locale data...'

	PRINT 'Start inserting dbo.Locale.localeID''s into dbo.Store...'
	INSERT INTO dbo.Store
	SELECT l.localeID
	FROM
		Locale			l
		JOIN LocaleType lt on l.localeTypeID = lt.localeTypeID
	WHERE
		lt.localeTypeDesc = 'Store'
	PRINT 'Finished inserting dbo.Locale.localeID''s into dbo.Store...'

	-- Add BU LocaleTrait
	PRINT 'Starting insert of PS Business Unit ID...'
	DECLARE @businessUnitTraitID int
	SET @businessUnitTraitID = (SELECT traitID FROM Trait WHERE traitDesc = 'PS Business Unit ID')

	INSERT INTO dbo.LocaleTrait
	SELECT DISTINCT
		@businessUnitTraitID	as traitID,
		l.localeID				as localeID,
		NULL					as uomID,
		al.BU					as traitValue
	FROM
		#addressList		al
		JOIN #storeMetro	sm	on	al.BU					= sm.BU
		JOIN Locale			l	on	al.StoreName			= l.localeName
									AND sm.metroLocaleID	= l.parentLocaleID
	PRINT 'Finished inserting of PS Business Unit ID...'

	PRINT 'Starting insert of Store Abbreviation...'
	-- Add Store Abbreviation LocaleTrait
	DECLARE @storeAbbreviationTraitID int
	SET @storeAbbreviationTraitID = (SELECT traitID FROM Trait WHERE traitDesc = 'Store Abbreviation')

	INSERT INTO dbo.LocaleTrait
	SELECT DISTINCT
		@storeAbbreviationTraitID	as traitID,
		l.localeID					as localeID,
		NULL						as uomID,
		al.StoreAbbr				as traitValue
	FROM
		#addressList		al
		JOIN #storeMetro	sm	on	al.BU					= sm.BU
		JOIN Locale			l	on	al.StoreName			= l.localeName
									AND sm.metroLocaleID	= l.parentLocaleID
	PRINT 'Finished inserting of Store Abbreviation...'

	-- Add Insert Date LocaleTrait
	PRINT 'Starting insert InsertDate...'
	DECLARE @insertDateTraitID int
	DECLARE @now nvarchar(27)
	SET @insertDateTraitID = (SELECT traitID FROM Trait WHERE traitDesc = 'Insert Date')
	SET @now = (CONVERT(nvarchar(27), SYSDATETIME(), 121))

	INSERT INTO dbo.LocaleTrait
	SELECT DISTINCT
		@insertDateTraitID	as traitID,
		l.localeID			as localeID,
		NULL				as uomID,
		@now				as traitValue
	FROM
		#addressList		al
		JOIN #storeMetro	sm	on	al.BU					= sm.BU
		JOIN Locale			l	on	al.StoreName			= l.localeName
									AND sm.metroLocaleID	= l.parentLocaleID
	PRINT 'Finished inserting InsertDate...'

	-- Add Modification Date LocaleTrait
	PRINT 'Starting insert InsertDate...'
	DECLARE @modifiedDateTraitID int
	SET @modifiedDateTraitID = (SELECT traitID FROM Trait WHERE traitDesc = 'Modified Date')

	INSERT INTO dbo.LocaleTrait
	SELECT DISTINCT
		@modifiedDateTraitID	as traitID,
		l.localeID				as localeID,
		NULL					as uomID,
		NULL					as traitValue
	FROM
		#addressList		al
		JOIN #storeMetro	sm	on	al.BU					= sm.BU
		JOIN Locale			l	on	al.StoreName			= l.localeName
									AND sm.metroLocaleID	= l.parentLocaleID
	PRINT 'Finished inserting InsertDate...'

	-- *********************************************
	-- Get and Add address information
	-- *********************************************
	DECLARE @localeCountryTerritory table
	(
		BU				nvarchar(5)	NOT NULL,
		countryID		int			NOT NULL,
		territoryID		int			NOT NULL,
		timezoneID		int			NOT NULL
	)

	-- Get Country and Territory IDs
	PRINT 'Start getting Country and Territory IDs matched to BU...'
	INSERT INTO @localeCountryTerritory
	SELECT
		al.BU,
		c.countryID,
		t.territoryID,
		tz.timezoneID
	FROM
		#addressList		al
		JOIN Country		c	on al.Country		= c.countryCode
		JOIN Territory		t	on al.StateProvince = t.territoryCode
		JOIN Timezone		tz	on al.TimeZone		= tz.timezoneCode
	PRINT 'Finished getting Country and Territory IDs matched to BU...'

	-- Add Unique County entries
	PRINT 'Start inserting into dbo.County...'
	INSERT INTO dbo.County
	SELECT DISTINCT
		al.County as countyName,
		ct.territoryID
	FROM 
		#addressList					al
		JOIN @localeCountryTerritory	ct	on al.BU = ct.BU
	PRINT 'Finished inserting into dbo.County...'

	-- Add Unique City entries
	PRINT 'Start inserting into dbo.City...'
	INSERT INTO dbo.City
	SELECT DISTINCT
		al.City			as cityName,
		ct.territoryID	as territoryID,
		c.countyID		as countyID
	FROM
		#addressList					al
		JOIN @localeCountryTerritory	ct	on al.BU = ct.BU
		JOIN dbo.County					c	on al.County = c.countyName
	PRINT 'Finished inserting into dbo.City...'

	-- Add Unique Postal Code entries
	PRINT 'Start inserting into dbo.PostalCode...'
	INSERT INTO dbo.PostalCode
	SELECT
		al.PostalCode as postalCode,
		lct.countryID as countryID,
		co.countyID as countyID
	FROM
		#addressList					al
		JOIN @localeCountryTerritory	lct	on	al.BU				= lct.BU
		JOIN dbo.County					co	on	al.County			= co.countyName
												AND lct.territoryID = co.territoryID
	GROUP BY
		al.PostalCode,
		lct.countryID,
		co.countyID
	PRINT 'Finished inserting into dbo.PostalCode...'

	-- *********************************************
	-- Add Address, LocaleAddress, & PhysicalAddress
	-- *********************************************
	declare @outputAddressBU table (addressID int, BU nvarchar(5))

	PRINT 'Merging into dbo.Address for addressIDs matched to BU'
	merge into 
		dbo.Address
	using 
		#addressList al on 1=0
	when not matched
	then insert (addressTypeID) values (1)
	output 
		inserted.addressID,
		al.BU
	into
		@outputAddressBU (addressID, BU);
	PRINT 'Done with Merge.'

	-- Add Locale Address data
	DECLARE @addressUsageID int
	SET @addressUsageID = (SELECT au.addressUsageID FROM AddressUsage au WHERE au.addressUsageDesc = 'Shipping')

	PRINT 'Starting inserting LocaleAddress data...'
	INSERT INTO dbo.LocaleAddress
	SELECT
		l.localeID,
		a.addressID,
		@addressUsageID as addressUsageID
	FROM
		Locale					l
		JOIN LocaleTrait		lt	on	l.localeID		= lt.localeID
										AND lt.traitID	= @businessUnitTraitID
		JOIN @outputAddressBU	a	on	lt.traitValue	= a.BU
	PRINT 'Finished inserting LocaleAddress data...'

	-- finally add PhysicalAddress data
	PRINT 'Finally inserting everything into dbo.PhysicalAddress'
	INSERT INTO dbo.PhysicalAddress
	SELECT
		a.addressID			as addressID,
		lct.countryID		as countryID,
		lct.territoryID		as territoryID,
		ci.cityID			as cityID,
		pc.postalCodeID		as postalCodeID,
		al.Latitude			as latitude,
		al.Longitude		as longitude,
		al.AddressLine1		as addressLine1,
		al.AddressLine2		as addressLine2,
		al.AddressLine3		as addressLine3,
		lct.timezoneID		as timezoneID
	FROM
		@outputAddressBU				a
		JOIN #addressList				al	on	a.BU				= al.BU
		JOIN @localeCountryTerritory	lct	on	al.BU				= lct.BU
		JOIN dbo.County					co	on	al.County			= co.countyName
												AND lct.territoryID = co.territoryID
		JOIN dbo.City					ci	on	al.City				= ci.cityName
												AND lct.territoryID = ci.territoryID
												AND co.countyID		= ci.countyID
		JOIN dbo.PostalCode				pc	on	al.PostalCode		= pc.postalCode
												AND lct.countryID	= pc.countryID
												AND co.countyID		= pc.countyID

	PRINT 'Done inserting everything into dbo.PhysicalAddress'
	PRINT 'Committing Transaction'
	COMMIT TRAN
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0
    		ROLLBACK TRAN
	DECLARE @err_no int, @err_sev int, @err_msg varchar(MAX)
	SELECT @err_no = ERROR_NUMBER(), @err_sev = ERROR_SEVERITY(), @err_msg = ERROR_MESSAGE()
	RAISERROR ('Adding Stores and Addresses Script failed with error no: %d and message: %s', @err_sev, 1, @err_no, @err_msg)
END CATCH