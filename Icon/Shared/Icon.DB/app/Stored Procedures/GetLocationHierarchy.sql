CREATE PROCEDURE app.GetLocationHierarchy
	@includeAddress bit = 0
AS
BEGIN
SET NOCOUNT ON

DECLARE @RegionTraitID INT = (SELECT TraitID FROM Trait WHERE TraitGroupId = 5 AND traitDesc = 'Region Abbreviation')
DECLARE @BusinessUnitID INT = (SELECT TraitID FROM Trait WHERE TraitGroupId = 5 AND traitDesc = 'PS Business Unit ID')
DECLARE @StoreAbbreviationID INT = (SELECT TraitID FROM Trait WHERE TraitGroupId = 5 AND traitDesc = 'Store Abbreviation')
DECLARE @LocaleSubtypeID INT = (SELECT TraitID FROM Trait WHERE TraitGroupId = 5 AND traitDesc = 'Locale Subtype')
DECLARE @CurrencyTraitId INT = (SELECT TraitID FROM Trait WHERE traitDesc = 'Currency Code')

DECLARE @localeAddress TABLE
(
  localeId int NOT NULL primary key,
  addressLine1 nvarchar(255) null,
  addressLine2 nvarchar(255) null,
  addressLine3 nvarchar(255) null,
  cityName nvarchar(255) null,
  territoryCode nvarchar(3) null,
  postalCode nvarchar(15) null,
  countryCode nvarchar(3) null,
  countryName nvarchar(255) null
)

IF (@includeAddress = 1)
BEGIN
	INSERT INTO @localeAddress
	SELECT la.localeID, pa.addressLine1, pa.addressLine2, pa.addressLine3, cityName, t.territoryCode, pc.postalCode, ctr.countryCode, ctr.countryName
         from LocaleAddress la
         join PhysicalAddress pa on la.addressID = pa.addressID
		 join Country ctr on pa.countryID = ctr.countryID
         join City c on pa.cityID = c.cityID
         join Territory t on t.territoryID = pa.territoryID
         join PostalCode pc on pc.postalCodeID = pa.postalCodeID
END

SELECT l.localeID,
	   localeName,
       localeTypeID,
       parentLocaleID,
       localeOpenDate,
       localeCloseDate,
	   rc.traitValue AS regionCode,
	   bu.traitValue AS businessUnitId,
       sa.traitValue AS storeAbbreviation,
       cr.traitValue AS currencyCode,
       st.traitValue AS subtype,
	   la.addressLine1, 
	   la.addressLine2, 
	   la.addressLine3, 
	   la.cityName, 
	   la.territoryCode, 
	   la.postalCode, 
	   la.countryCode, 
	   la.countryName

FROM      Locale l
LEFT JOIN LocaleTrait rc ON rc.localeID = l.localeID and rc.traitID = @RegionTraitID
LEFT JOIN LocaleTrait bu ON bu.localeID = l.localeID and bu.traitID = @BusinessUnitID
LEFT JOIN LocaleTrait sa ON sa.localeID = l.localeID and sa.traitID = @StoreAbbreviationID
LEFT JOIN LocaleTrait cr ON cr.localeID = l.localeID and cr.traitID = @CurrencyTraitId
LEFT JOIN LocaleTrait st ON st.localeID = l.localeID and st.traitID = @LocaleSubtypeID
LEFT JOIN @localeAddress la on la.localeId = l.localeID 

SET NOCOUNT OFF

END
GO