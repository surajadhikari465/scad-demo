-- 7/11/2014, TFS 3790, Sprint 18
-- Populates the dbo.Country and dbo.Territory tables
-- These mostly came from the official USPS site

DECLARE @countryID int

IF NOT EXISTS (select * from Country c where c.countryCode = 'USA')
BEGIN
	INSERT INTO dbo.Country (countryCode, countryName)
	VALUES ('USA','United States')
END

IF NOT EXISTS (select * from dbo.Territory t where t.territoryName = 'Wyoming')
BEGIN
	SET @countryID = SCOPE_IDENTITY()
	INSERT INTO dbo.Territory (territoryName, territoryCode ,countryID)
	VALUES
		('Alabama','AL',@countryID),
		('Alaska','AK',@countryID),
		('American Samoa','AS',@countryID),
		('Arizona','AZ',@countryID),
		('Arkansas','AR',@countryID),
		('California','CA',@countryID),
		('Colorado','CO',@countryID),
		('Connecticut','CT',@countryID),
		('Delaware','DE',@countryID),
		('District Of Columbia','DC',@countryID),
		('Federated States Of Micronesia','FM',@countryID),
		('Florida','FL',@countryID),
		('Georgia','GA',@countryID),
		('Guam','GU',@countryID),
		('Hawaii','HI',@countryID),
		('Idaho','ID',@countryID),
		('Illinois','IL',@countryID),
		('Indiana','IN',@countryID),
		('Iowa','IA',@countryID),
		('Kansas','KS',@countryID),
		('Kentucky','KY',@countryID),
		('Louisiana','LA',@countryID),
		('Maine','ME',@countryID),
		('Marshall Islands','MH',@countryID),
		('Maryland','MD',@countryID),
		('Massachusetts','MA',@countryID),
		('Michigan','MI',@countryID),
		('Minnesota','MN',@countryID),
		('Mississippi','MS',@countryID),
		('Missouri','MO',@countryID),
		('Montana','MT',@countryID),
		('Nebraska','NE',@countryID),
		('Nevada','NV',@countryID),
		('New Hampshire','NH',@countryID),
		('New Jersey','NJ',@countryID),
		('New Mexico','NM',@countryID),
		('New York','NY',@countryID),
		('North Carolina','NC',@countryID),
		('North Dakota','ND',@countryID),
		('Northern Mariana Islands','MP',@countryID),
		('Ohio','OH',@countryID),
		('Oklahoma','OK',@countryID),
		('Oregon','OR',@countryID),
		('Palau','PW',@countryID),
		('Pennsylvania','PA',@countryID),
		('Puerto Rico','PR',@countryID),
		('Rhode Island','RI',@countryID),
		('South Carolina','SC',@countryID),
		('South Dakota','SD',@countryID),
		('Tennessee','TN',@countryID),
		('Texas','TX',@countryID),
		('Utah','UT',@countryID),
		('Vermont','VT',@countryID),
		('Virgin Islands','VI',@countryID),
		('Virginia','VA',@countryID),
		('Washington','WA',@countryID),
		('West Virginia','WV',@countryID),
		('Wisconsin','WI',@countryID),
		('Wyoming','WY',@countryID)
END

IF NOT EXISTS (select * from Country c where c.countryCode = 'CAN')
BEGIN
	INSERT INTO [dbo].[Country] (countryCode, countryName)
	VALUES ('CAN','Canada')
END

IF NOT EXISTS (select * from dbo.Territory t where t.territoryName = 'Ontario')
BEGIN
	SET @countryID = SCOPE_IDENTITY()
	INSERT INTO dbo.Territory (territoryName, territoryCode, countryID)
	VALUES
		('Alberta','AB',@countryID),
		('British Columbia','BC', @countryID),
		('Manitoba','MB', @countryID),
		('New Brunswick','NB', @countryID),
		('Newfoundland and Labrador','NL', @countryID),
		('Nova Scotia','NS', @countryID),
		('Northwest Territories','NT', @countryID),
		('Nunavut','NU', @countryID),
		('Ontario','ON', @countryID),
		('Prince Edward Island','PE', @countryID),
		('Quebec','QC', @countryID),
		('Saskatchewan','SK', @countryID),
		('Yukon','YT', @countryID)
END

IF EXISTS (select * from Country c where c.countryCode = 'UK')
BEGIN
	UPDATE Country
	SET
		countryCode = 'GBR',
		countryName = 'Great Britain'
	WHERE
		countryCode = 'UK'
END
ELSE IF NOT EXISTS (select * from Country c where c.countryCode = 'GBR')
BEGIN
	INSERT INTO [dbo].[Country] (countryCode, countryName)
	VALUES ('GBR', 'Great Britain')
END

IF EXISTS (select * from Territory t where (t.territoryName = 'England' OR t.territoryName = 'United Kingdom'))
BEGIN
	UPDATE Territory
	SET
		territoryCode = 'GBR',
		territoryName = 'Great Britain'
	WHERE
		(territoryName = 'England' OR territoryName = 'United Kingdom')
END
ELSE IF NOT EXISTS (select * from Territory t where t.territoryCode = 'GBR')
BEGIN
	SET @countryID = (select countryID from Country c where c.countryCode = 'GBR')
	INSERT INTO dbo.Territory (territoryName, territoryCode, countryID)
	VALUES
		('Great Britain', 'GBR', @countryID)
END