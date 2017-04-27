 if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UpdateCompetitivePriceInfo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[UpdateCompetitivePriceInfo]
GO

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
go    