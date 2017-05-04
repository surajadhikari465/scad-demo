CREATE PROCEDURE [dbo].[UpdateCompetitivePriceInfo] 
	@Item_Key int,
	@Store_No int,
	@CompetitivePriceTypeID int,
	@BandwidthPercentageHigh tinyint,
	@BandwidthPercentageLow tinyint
AS
BEGIN

UPDATE Price
SET
	CompetitivePriceTypeID = @CompetitivePriceTypeID,
	BandwidthPercentageHigh = @BandwidthPercentageHigh,
	BandwidthPercentageLow = @BandwidthPercentageLow,
	CompFlag = CASE ISNULL(@CompetitivePriceTypeID, 0)
		WHEN 0 THEN 0
		ELSE 1 END
WHERE
	Item_Key = @Item_Key
	AND
	Store_No = @Store_No

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateCompetitivePriceInfo] TO [IRMAClientRole]
    AS [dbo];

