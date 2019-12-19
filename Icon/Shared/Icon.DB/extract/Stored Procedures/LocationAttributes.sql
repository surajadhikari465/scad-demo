CREATE PROCEDURE [extract].LocationAttributes
AS
BEGIN
	DECLARE @localeIds TABLE (localeId INT)
	DECLARE @RegionAbbrTraitId INT
		,@CurrencyTraitId INT
		,@IdentTraitId INT
		,@LiquorLicensingTraitId INT
		,@PrimeNowMerchIdTraitId INT
		,@PrimeNowMerchIdTraitEncryptedId INT
		,@LocalZoneTraitId INT
		,@StoreAbbrevTraitId INT
		,@StoreLocaleTypeId INT

	SELECT @StoreLocaleTypeId = localetypeId
	FROM localetype
	WHERE localetypedesc = 'Store'

	SELECT @RegionAbbrTraitId = traitId
	FROM Trait
	WHERE traitDesc = 'Region Abbreviation'

	SELECT @CurrencyTraitId = traitId
	FROM Trait
	WHERE traitDesc = 'Currency Code'

	SELECT @IdentTraitId = traitId
	FROM Trait
	WHERE traitDesc = 'Ident'

	SELECT @LiquorLicensingTraitId = traitId
	FROM Trait
	WHERE traitDesc = 'Liquor Licensing'

	SELECT @PrimeNowMerchIdTraitId = traitId
	FROM Trait
	WHERE traitDesc = 'PrimeNow Merchant ID'

	SELECT @PrimeNowMerchIdTraitEncryptedId = traitId
	FROM Trait
	WHERE traitDesc = 'PrimeNow Merchant ID Encrypted'

	SELECT @LocalZoneTraitId = traitId
	FROM Trait
	WHERE traitDesc = 'Local Zone'

	SELECT @LocalZoneTraitId = traitId
	FROM Trait
	WHERE traitDesc = 'Local Zone'

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
		,IsNull(ltt.traitValue, 'TS') AS REGION_ABBR
		,sa.addressLine1 AS LOCATION_ADDR
		,sa.addressLine2 AS LOCATION_ADDR2
		,sa.cityName AS LOCATION_CITY
		,sa.territoryCode AS STATE_PROVINCE
		,sa.postalCode AS POSTAL_CODE
		,c.localeID AS CHAIN_ID
		,c.localeName AS CHAIN_LABEL
		,l.localeOpenDate AS LOCATION_OPENING_DATE
		,l.localeCloseDate AS LOCATION_CLOSING_DATE
		,sa.countryID AS COUNTRY_ID
		,sa.countryCode AS COUNTRY_ABBR
		,IsNull(ltc.traitValue, 'USD') AS CURRENCY_CODE
		,ltid.traitValue AS Ident
		,lll.traitValue AS LiquorLicensing
		,ltp.traitValue AS PrimeNowMerchantId
		,ltpe.traitValue AS PrimeNowMerchantIdEncrypted
		,llz.traitValue AS LocaleZone
		,lsa.traitValue AS StoreAbbreviation
	FROM Locale l
	INNER JOIN BuzUnit bu ON l.localeID = bu.localeId
	INNER JOIN Metro m ON l.parentLocaleID = m.localeID
	INNER JOIN StoreAddress sa ON sa.localeID = l.localeID
	INNER JOIN Region r ON m.parentLocaleID = r.localeID
	INNER JOIN Chain c ON c.localeID = r.parentLocaleID
	LEFT JOIN LocaleTrait ltc ON ltc.localeID = l.localeID
		AND ltc.traitID = @CurrencyTraitId
	LEFT JOIN LocaleTrait ltt ON ltt.localeID = r.localeID
		AND ltt.traitID = @RegionAbbrTraitId
	LEFT JOIN LocaleTrait ltid ON ltid.localeID = l.localeID
		AND ltid.traitID = @IdentTraitId
	LEFT JOIN LocaleTrait lll ON lll.localeID = l.localeID
		AND lll.traitID = @LiquorLicensingTraitId
	LEFT JOIN LocaleTrait ltp ON ltp.localeID = l.localeID
		AND ltp.traitID = @PrimeNowMerchIdTraitId
	LEFT JOIN LocaleTrait ltpe ON ltpe.localeID = l.localeID
		AND ltpe.traitID = @PrimeNowMerchIdTraitEncryptedId
	LEFT JOIN LocaleTrait llz ON llz.localeID = l.localeID
		AND llz.traitID = @LocalZoneTraitId
	LEFT JOIN LocaleTrait lsa ON lsa.localeID = l.localeID
		AND lsa.traitID = @StoreAbbrevTraitId
END
GO
