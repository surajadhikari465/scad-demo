CREATE PROCEDURE infor.AddOrUpdateLocaleAddress 
  @address infor.LocaleAddressAddOrUpdateType readonly 
AS 
  BEGIN 

     DECLARE @physicalAddressTypeId INT = 
		(SELECT addresstypeid 
		 FROM   addresstype 
		 WHERE  addresstypecode = 'PHY'
		 ), 
	 @shippingAddressTypeId INT = 
		 (SELECT addressusageid 
		  FROM   addressusage 
		  WHERE  addressusagecode = 'SHP' 
		 ),	
	  @businessUnitIdTraitId INT = 
		   (SELECT traitID
			FROM Trait
			WHERE traitCode = 'BU'
			)
      /* Add or update Stores */ 
    IF EXISTS  (SELECT TOP 1 1 FROM @address) 
    BEGIN 
	      SELECT AddressId,
				 AddressLine1,
				 AddressLine2,
				 AddressLine3,
				 CityName,
				 PostalCode,
				 CountryCode,
				 TerritoryCode,
				 TimeZoneName,
				 Latitude,
				 Longitude,
		         [BusinessUnitId],
				 lt.localeID
		   INTO #tmp
		   FROM @address
		   INNER JOIN LocaleTrait lt
		   ON lt.traitValue = businessunitid AND lt.traitID = @businessUnitIdTraitId 

		  INSERT INTO dbo.postalcode 
			  (
			    countryid, 
				postalcode, 
				countyid 
			  ) 
		  SELECT c.countryid, 
				 tmp.PostalCode, 
				 NULL 
		  FROM #tmp tmp 
		  JOIN country c 
		  ON tmp.CountryCode = c.countrycode 
		  WHERE  NOT EXISTS 
				 ( SELECT 1 
				   FROM postalcode p 
				   WHERE p.postalCode = tmp.PostalCode 
				   AND c.countryID = p.countryID ) 

		  INSERT INTO dbo.city 
			   ( 
			     territoryid , 
				 cityname , 
				 countyid 
			   ) 
		  SELECT t.territoryID , 
				 tmp.CityName , 
				 NULL 
		  FROM #tmp tmp 
		  JOIN Territory t 
		  ON tmp.TerritoryCode = t.territoryCode 
		  WHERE NOT EXISTS 
				   (SELECT 1 
					FROM City c 
					WHERE c.cityname = tmp.CityName 
					AND c.territoryID = t.territoryID )
				 
		  UPDATE pa 
		  SET    addressLine1 = tmp.addressline1 , 
				 addressLine2 = tmp.addressline2 , 
				 addressLine3 = tmp.addressline3 , 
				 cityID = ci.cityid , 
				 countryID = c.countryid , 
				 latitude = Cast(tmp.latitude AS   DECIMAL(9,6)) , 
				 longitude = Cast(tmp.longitude AS DECIMAL(9,6)) , 
				 postalCodeID = p.postalcodeid , 
				 territoryID = t.territoryid , 
				 timezoneID = tz.timezoneid 
		  FROM physicaladdress pa 
		  INNER JOIN   #tmp tmp 
		  ON pa.addressID = tmp.addressID 
		  INNER JOIN country c 
		  ON tmp.countryCode = c.countryCode 
		  INNER JOIN territory t 
		  ON tmp.territoryCode = t.territoryCode 
		  AND t.countryid = c.countryid 
		  INNER JOIN postalcode p 
		  ON tmp.postalCode = p.postalCode 
		  AND p.countryid = c.countryid 
		  INNER JOIN city ci 
		  ON tmp.cityName = ci.cityName 
		  AND ci.territoryID = t.territoryID 
		  INNER JOIN timezone tz 
		  ON tmp.timeZoneName = tz.posTimeZoneName 

		  SET IDENTITY_INSERT address ON 

		  INSERT INTO [Address]
				( 
				  addressID , 
				  addressTypeID 
				) 
		  SELECT addressID , 
				 @physicalAddressTypeId 
		  FROM #tmp 
		  WHERE addressID NOT IN 
				 ( SELECT addressID 
				   FROM address 
				 ) 

		  SET IDENTITY_INSERT address OFF 

		  INSERT INTO LocaleAddress 
				( 
				  addressID , 
				  addressUsageID , 
				  localeID 
				 ) 
		  SELECT addressID , 
				 @shippingAddressTypeId , 
				 lt.localeID 
		  FROM   #tmp 
		  INNER JOIN LocaleTrait lt
		  ON lt.traitValue = businessUnitID AND lt.traitID = @businessUnitIdTraitId 
		  WHERE NOT EXISTS 
		   ( SELECT la.addressID, la.addressUsageID, la.localeID
		     FROM LocaleAddress la
		     INNER JOIN #tmp tmp
		     ON la.addressID = tmp.addressID
		     AND la.localeID = tmp.localeID
		     AND la.addressUsageID = @shippingAddressTypeId
		   )

		  INSERT INTO physicaladdress 
				  ( 	
				    addressID , 
					addressLine1 , 
					addressLine2 , 
					addressLine3 , 
					cityID , 
					countryID , 
					latitude , 
					longitude , 
					postalCodeID , 
					territoryID , 
					timezoneID 
				   ) 
		  SELECT    tmp.addressID , 
					addressLine1 , 
					addressLine2 , 
					addressLine3 , 
					cityID , 
					country.countryID , 
				    Cast(latitude AS  DECIMAL(9,6)) , 
					Cast(longitude AS DECIMAL(9,6)) , 
					postalCodeID , 
					territory.territoryID , 
					timezoneID 
		  FROM #tmp tmp 
		  INNER JOIN country 
		  ON country.countryCode = tmp.countryCode 
		  INNER JOIN territory 
		  ON territory.territoryCode = tmp.territoryCode 
		  INNER JOIN postalcode 
		  ON postalcode.postalCode = tmp.postalCode 
		  INNER JOIN timezone 
		  ON timezone.posTimeZoneName = tmp.TimeZoneName 
		  INNER JOIN city 
		  ON city.cityName = tmp.cityName 
		  WHERE addressID NOT IN 
				 ( SELECT addressID 
				   FROM   physicaladdress 
				 ) 
        END 
   END