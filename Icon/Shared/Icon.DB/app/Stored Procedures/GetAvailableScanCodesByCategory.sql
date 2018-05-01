CREATE PROCEDURE [app].[GetAvailableScanCodesByCategory]
	@CategoryId INT,
	@MaxScanCodes INT = 0
AS
BEGIN

DECLARE @beginRange nvarchar(11) = (SELECT BeginRange FROM app.PLUCategory WHERE PluCategoryID = @CategoryId),
		@endRange nvarchar(11) = (SELECT EndRange FROM app.PLUCategory WHERE PluCategoryID = @CategoryId);

IF(@MaxScanCodes = 0)
	SET @MaxScanCodes = 10000;

WITH ScanCodes (ScanCode)
AS 
(
	SELECT @beginRange ScanCode
	UNION ALL
	SELECT CONVERT(nvarchar(11), CONVERT(bigint, ScanCode) + 1)
	FROM ScanCodes
	WHERE ScanCode < @endRange
)

SELECT TOP (@MaxScanCodes)
	0							as [irmaItemID],
	null						as [regioncode],
	ScanCode					as [identifier],
	cast(1 as bit)				as [defaultIdentifier],
	null						as [brandName],
	null						as [itemDescription],
	null						as [posDescription],
    1							as [packageUnit],
    null						as [retailSize],
    null						as [retailUom],
	null						as [deliverySystem],
    cast(0 as bit)				as [foodStamp],
    cast(0 as decimal)			as [posScaleTare],
    cast(0 as bit)				as [departmentSale],
    null						as [giftCard],
    null						as [taxClassID],
    null						as [merchandiseClassID],
    SYSDATETIME()				as [insertDate],
	null						as [irmaSubTeamName],
	null						as [nationalClassId],
	null						as [AnimalWelfareRatingId],
	null						as [Biodynamic],
	null						as [CheeseMilkTypeId],
	null						as [CheeseRaw],
	null						as [EcoScaleRatingId],
	null						as [GlutenFreeAgencyName],
	null						as [HealthyEatingRatingId],
	null						as [KosherAgencyName],
	null						as [Msc],
	null						as [NonGmoAgencyName],
	null						as [OrganicAgencyName],
	null						as [PremiumBodyCare],
	null						as [SeafoodFreshOrFrozenId],
	null						as [SeafoodCatchTypeId],
	null						as [VeganAgencyName],
	null						as [Vegetarian],
	null						as [WholeTrade],
	null						as [GrassFed],
	null						as [PastureRaised],
	null						as [FreeRange],
	null						as [DryAged],
	null						as [AirChilled],
	null						as [MadeinHouse],
	null						as [AlcoholByVolume]
FROM ScanCodes
WHERE ScanCode NOT IN (SELECT sc.scanCode FROM ScanCode sc)
OPTION (MAXRECURSION 20000)

END