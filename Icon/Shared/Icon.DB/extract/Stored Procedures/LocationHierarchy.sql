CREATE PROCEDURE [extract].LocationHierarchy
AS
BEGIN
	DECLARE @localeIds TABLE (localeId INT)
	DECLARE @RegionAbbrTraitId INT
		,@StoreAbbrevTraitId INT
		,@StoreLocaleTypeId INT

	SELECT @StoreLocaleTypeId = localetypeId
	FROM localetype
	WHERE localetypedesc = 'Store'

	SELECT @RegionAbbrTraitId = traitId
	FROM Trait
	WHERE traitDesc = 'Region Abbreviation'

	SELECT @StoreAbbrevTraitId = traitId
	FROM Trait
	WHERE traitDesc = 'Store Abbreviation'

	INSERT INTO @localeIds
	SELECT store.localeID
	FROM Locale store
	INNER JOIN Locale metro ON store.parentLocaleID = metro.localeID
	INNER JOIN Locale region ON metro.parentLocaleID = region.localeID
	INNER JOIN Locale chain ON region.parentLocaleID = chain.localeID
	WHERE store.localeTypeID = @StoreLocaleTypeId;

	WITH Chain
	AS (
		SELECT localeID
			,localeName
		FROM Locale
		WHERE parentLocaleID IS NULL
		)
		,BuzUnit
	AS (
		SELECT lt.localeID
			,traitValue
			,parentLocaleID
		FROM LocaleTrait lt
		INNER JOIN Trait t ON lt.traitId = t.traitId
		INNER JOIN Locale l ON lt.localeID = l.localeID
		WHERE t.traitDesc = 'PS Business Unit ID'
			AND lt.localeID IN (
				SELECT localeId
				FROM @localeIds
				)
		)
		,Metro
	AS (
		SELECT localeId
			,localeName
			,parentLocaleID
		FROM Locale l
		INNER JOIN LocaleType lt ON l.localeTypeID = lt.localeTypeID
		WHERE lt.localeTypeDesc = 'Metro'
		)
		,Region
	AS (
		SELECT l.localeId
			,parentLocaleID
		FROM Locale l
		INNER JOIN LocaleType lt ON l.localeTypeID = lt.localeTypeID
		WHERE lt.localeTypeDesc = 'Region'
		)
		,StoreAddress
	AS (
		SELECT la.localeID
			,pa.addressLine1
			,pa.addressLine2
			,cityName
			,t.territoryCode
			,pc.postalCode
			,ctr.countryID
			,ctr.countryCode
			,ctr.countryName
		FROM LocaleAddress la
		INNER JOIN PhysicalAddress pa ON la.addressID = pa.addressID
		INNER JOIN Country ctr ON pa.countryID = ctr.countryID
		INNER JOIN City c ON pa.cityID = c.cityID
		INNER JOIN Territory t ON t.territoryID = pa.territoryID
		INNER JOIN PostalCode pc ON pc.postalCodeID = pa.postalCodeID
		WHERE la.localeID IN (
				SELECT localeId
				FROM @localeIds
				)
		)
	SELECT bu.traitValue AS STORE_NUMBER
		,l.localeName AS LOCATION_NAME
		,m.localeID AS METRO_ID
		,m.localeName AS METRO_NAME
		,r.localeID AS REGION_ID
		,ltt.traitValue AS REGION_ABBR
		,ltstore.traitvalue as STORE_ABBR
	FROM Locale l
	INNER JOIN BuzUnit bu ON l.localeID = bu.localeId
	INNER JOIN Metro m ON l.parentLocaleID = m.localeID
	INNER JOIN Region r ON m.parentLocaleID = r.localeID
	LEFT JOIN LocaleTrait ltt ON ltt.localeID = r.localeID
		AND ltt.traitID = @RegionAbbrTraitId
	LEFT JOIN LocaleTrait ltstore on ltstore.localeID = l.localeID
	and ltstore.traitID = @StoreAbbrevTraitId

END
GO


