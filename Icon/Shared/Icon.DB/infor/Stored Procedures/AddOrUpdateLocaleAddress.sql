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
		   INTO #tmpAddress
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
				 tmpAddress.PostalCode, 
				 NULL 
		  FROM #tmpAddress tmpAddress 
		  JOIN country c 
		  ON tmpAddress.CountryCode = c.countrycode 
		  WHERE  NOT EXISTS 
				 ( SELECT 1 
				   FROM postalcode p 
				   WHERE p.postalCode = tmpAddress.PostalCode 
				   AND c.countryID = p.countryID ) 

		  INSERT INTO dbo.city 
			   ( 
			     territoryid , 
				 cityname , 
				 countyid 
			   ) 
		  SELECT t.territoryID , 
				 tmpAddress.CityName , 
				 NULL 
		  FROM #tmpAddress tmpAddress 
		  JOIN Territory t 
		  ON tmpAddress.TerritoryCode = t.territoryCode 
		  WHERE NOT EXISTS 
				   (SELECT 1 
					FROM City c 
					WHERE c.cityname = tmpAddress.CityName 
					AND c.territoryID = t.territoryID )
				 
		  UPDATE pa 
		  SET    addressLine1 = tmpAddress.addressline1 , 
				 addressLine2 = tmpAddress.addressline2 , 
				 addressLine3 = tmpAddress.addressline3 , 
				 cityID = ci.cityid , 
				 countryID = c.countryid , 
				 latitude = Cast(tmpAddress.latitude AS   DECIMAL(9,6)) , 
				 longitude = Cast(tmpAddress.longitude AS DECIMAL(9,6)) , 
				 postalCodeID = p.postalcodeid , 
				 territoryID = t.territoryid , 
				 timezoneID = tz.timezoneid 
		  FROM physicaladdress pa 
		  INNER JOIN   #tmpAddress tmpAddress 
		  ON pa.addressID = tmpAddress.addressID 
		  INNER JOIN country c 
		  ON tmpAddress.countryCode = c.countryCode 
		  INNER JOIN territory t 
		  ON tmpAddress.territoryCode = t.territoryCode 
		  AND t.countryid = c.countryid 
		  INNER JOIN postalcode p 
		  ON tmpAddress.postalCode = p.postalCode 
		  AND p.countryid = c.countryid 
		  INNER JOIN city ci 
		  ON tmpAddress.cityName = ci.cityName 
		  AND ci.territoryID = t.territoryID 
		  INNER JOIN timezone tz 
		  ON tmpAddress.timeZoneName = tz.posTimeZoneName 

		  SET IDENTITY_INSERT address ON 

		  INSERT INTO [Address]
				( 
				  addressID , 
				  addressTypeID 
				) 
		  SELECT addressID , 
				 @physicalAddressTypeId 
		  FROM #tmpAddress 
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
		  FROM   #tmpAddress tmpAddress
		  INNER JOIN LocaleTrait lt
		  ON lt.traitValue = businessUnitID AND lt.traitID = @businessUnitIdTraitId 
		  WHERE NOT EXISTS 
		   ( SELECT la.addressID, la.addressUsageID, la.localeID
		     FROM LocaleAddress la
		     WHERE la.addressID = tmpAddress.addressID
		     AND la.localeID = tmpAddress.localeID
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
		  SELECT    tmpAddress.addressID , 
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
		  FROM #tmpAddress tmpAddress 
		  INNER JOIN country 
		  ON country.countryCode = tmpAddress.countryCode 
		  INNER JOIN territory 
		  ON territory.territoryCode = tmpAddress.territoryCode 
		  INNER JOIN postalcode 
		  ON postalcode.postalCode = tmpAddress.postalCode 
		  INNER JOIN timezone 
		  ON timezone.posTimeZoneName = tmpAddress.TimeZoneName 
		  INNER JOIN city 
		  ON city.cityName = tmpAddress.cityName 
		  WHERE addressID NOT IN 
				 ( SELECT addressID 
				   FROM   physicaladdress 
				 ) 
        END 
   END