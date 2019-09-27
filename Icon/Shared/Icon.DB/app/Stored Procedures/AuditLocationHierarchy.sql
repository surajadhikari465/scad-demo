CREATE PROCEDURE app.AuditLocationHierarchy
	 @action VARCHAR(25)
	,@region VARCHAR(2) = NULL
	,@groupSize INT = 250000
	,@groupId INT = 0
AS
IF @action = 'Get'
BEGIN
	IF IsNull(@groupSize, 0) <= 0  SET @groupSize = 250000;

	DECLARE @RegionAbbrTraitId INT = (SELECT traitId FROM Trait WHERE traitDesc = 'Region Abbreviation'),
			@CurrencyTraitId INT = (SELECT traitId FROM Trait WHERE traitDesc = 'Currency Code'),
			@phoneTraitId INT = (SELECT traitId FROM Trait WHERE traitDesc = 'Phone Number'),
			@faxTraitId INT = (SELECT traitId FROM Trait WHERE traitDesc = 'Fax'),
			@contactTraitId INT = (SELECT traitId FROM Trait WHERE traitDesc = 'Contact Person'),
			@abrTraitId INT = (SELECT traitId FROM Trait WHERE traitDesc = 'Store Abbreviation'),
			@irmaTraitId INT = (SELECT traitId FROM Trait WHERE traitDesc = 'IRMA Store ID'),
			@posTraitId INT = (SELECT traitId FROM Trait WHERE traitDesc = 'Store POS Type'),
			@identTraitId INT = (SELECT traitId FROM Trait WHERE traitDesc = 'Ident'),
			@zoneTraitId INT = (SELECT traitId FROM Trait WHERE traitDesc = 'Local Zone'),
			@minId INT = (@groupId * @groupSize) + (CASE WHEN @groupID = 0 THEN 0 ELSE 1 END);

	IF (object_id('tempdb..#locale') IS NOT NULL) DROP TABLE #locale;

	SELECT store.localeID
		,store.parentLocaleID
		,store.localeName
		,store.localeOpenDate
		,store.localeCloseDate
	INTO #locale
	FROM dbo.Locale store
	JOIN dbo.Locale metro ON store.parentLocaleID = metro.localeID
	JOIN dbo.Locale region ON metro.parentLocaleID = region.localeID
	JOIN dbo.Locale chain ON region.parentLocaleID = chain.localeID
	WHERE store.localeTypeID = 4;

	CREATE INDEX ix_localeId ON #locale (localeID);

	WITH Chain AS (
		SELECT localeID
			  ,localeName
		FROM Locale
		WHERE parentLocaleID IS NULL),

		BuzUnit AS (
			SELECT lt.localeID
				  ,lt.traitValue
				  ,l.parentLocaleID
			FROM LocaleTrait lt
			JOIN Trait t ON lt.traitId = t.traitId
			JOIN #locale l ON l.localeID = lt.localeID
			WHERE t.traitDesc = 'PS Business Unit ID'),

		Metro AS (
			SELECT localeId
				  ,localeName
				  ,parentLocaleID
			FROM Locale l
			JOIN LocaleType lt ON l.localeTypeID = lt.localeTypeID
			WHERE lt.localeTypeDesc = 'Metro'),

		Region AS (
			SELECT l.localeId
				  ,parentLocaleID
			FROM Locale l
			JOIN LocaleType lt ON l.localeTypeID = lt.localeTypeID
			WHERE lt.localeTypeDesc = 'Region'),

		StoreAddress AS (
			SELECT la.localeID
				  ,pa.addressLine1
				  ,pa.addressLine2
				  ,cityName
				  ,t.territoryCode
				  ,pc.postalCode
				  ,ctr.countryID
				  ,ctr.countryCode
				  ,ctr.countryName
				  ,pa.latitude
				  ,pa.longitude
				  ,tz.posTimeZoneName
			FROM LocaleAddress la
			JOIN #locale g ON g.localeID = la.localeID
			JOIN PhysicalAddress pa ON la.addressID = pa.addressID
			JOIN Country ctr ON pa.countryID = ctr.countryID
			JOIN City c ON pa.cityID = c.cityID
			JOIN Territory t ON t.territoryID = pa.territoryID
			JOIN PostalCode pc ON pc.postalCodeID = pa.postalCodeID
			LEFT JOIN Timezone tz ON tz.timezoneID = pa.timezoneID)

	SELECT TOP (@groupSize) STORE_NUMBER
		,LOCATION_LABEL
		,METRO_ID
		,METRO_LABEL
		,REGION_ID
		,REGION_ABBR
		,LOCATION_ADDR
		,LOCATION_ADDR2
		,LOCATION_CITY
		,STATE_PROVINCE
		,POSTAL_CODE
		,CHAIN_ID
		,CHAIN_LABEL
		,LOCATION_OPENING_DATE
		,LOCATION_CLOSING_DATE
		,COUNTRY_ID
		,COUNTRY_ABBR
		,CURRENCY_CODE
		,PHONE_NUMBER
		,FAX
		,CONTACT_PERSON
		,STORE_ABBREVIATION
		,IRMA_STORE_ID
		,STORE_POS_TYPE
		,IDENT
		,STORE_ZONE
		,LATITUDE
		,LONGTITUDE
		,TIME_ZONE
	FROM (
		SELECT bu.traitValue AS STORE_NUMBER
			  ,l.localeName AS LOCATION_LABEL
			  ,m.localeID AS METRO_ID
			  ,m.localeName AS METRO_LABEL
			  ,r.localeID AS REGION_ID
			  ,IsNull(ltt.traitValue, 'TS') AS REGION_ABBR
			  ,sa.addressLine1 AS LOCATION_ADDR
			  ,sa.addressLine2 AS LOCATION_ADDR2
			  ,sa.cityName AS LOCATION_CITY
			  ,sa.territoryCode AS STATE_PROVINCE
			  ,sa.postalCode AS POSTAL_CODE
			  ,c.localeID AS CHAIN_ID
			  ,c.localeName AS CHAIN_LABEL
			  ,Convert(VARCHAR(10), l.localeOpenDate, 120) AS LOCATION_OPENING_DATE
			  ,Convert(VARCHAR(10), l.localeCloseDate, 120) AS LOCATION_CLOSING_DATE
			  ,sa.countryID AS COUNTRY_ID
			  ,sa.countryCode AS COUNTRY_ABBR
			  ,IsNull(ltc.traitValue, 'USD') AS CURRENCY_CODE
			  ,ltPh.traitValue AS PHONE_NUMBER
			  ,ltFx.traitValue AS  FAX
			  ,ltCon.traitValue AS CONTACT_PERSON
			  ,lta.traitValue AS STORE_ABBREVIATION
			  ,lti.traitValue AS IRMA_STORE_ID
			  ,ltPos.traitValue AS STORE_POS_TYPE
			  ,ltIdn.traitValue AS IDENT
			  ,ltz.traitValue AS STORE_ZONE
			  ,sa.latitude AS LATITUDE
			  ,sa.longitude AS LONGTITUDE
			  ,sa.posTimeZoneName AS TIME_ZONE
			  ,Row_Number() OVER(ORDER BY l.localeID ASC) rowID
		FROM #locale l
		JOIN BuzUnit bu ON l.localeID = bu.localeId
		JOIN Metro m ON l.parentLocaleID = m.localeID
		JOIN StoreAddress sa ON sa.localeID = l.localeID
		JOIN Region r ON m.parentLocaleID = r.localeID
		JOIN Chain c ON c.localeID = r.parentLocaleID
		LEFT JOIN LocaleTrait ltc ON ltc.localeID = l.localeID AND ltc.traitID = @CurrencyTraitId
		LEFT JOIN LocaleTrait ltt ON ltt.localeID = r.localeID AND ltt.traitID = @RegionAbbrTraitId
		LEFT JOIN LocaleTrait ltPh ON ltPh.localeID = l.localeID AND ltPh.traitID = @phoneTraitId
		LEFT JOIN LocaleTrait ltFx ON ltFx.localeID = l.localeID AND ltFx.traitID = @faxTraitId
		LEFT JOIN LocaleTrait ltCon ON ltCon.localeID = l.localeID AND ltCon.traitID = @contactTraitId
		LEFT JOIN LocaleTrait lta ON lta.localeID = l.localeID AND lta.traitID = @abrTraitId
		LEFT JOIN LocaleTrait lti ON lti.localeID = l.localeID AND lti.traitID = @irmaTraitId
		LEFT JOIN LocaleTrait ltPos ON ltPos.localeID = l.localeID AND ltPos.traitID = @posTraitId
		LEFT JOIN LocaleTrait ltIdn ON ltIdn.localeID = l.localeID AND ltIdn.traitID = @identTraitId
		LEFT JOIN LocaleTrait ltz ON ltz.localeID = l.localeID AND ltz.traitID = @zoneTraitId
		) A
	WHERE rowID >= @minId;

	DROP TABLE #locale;
END
GO