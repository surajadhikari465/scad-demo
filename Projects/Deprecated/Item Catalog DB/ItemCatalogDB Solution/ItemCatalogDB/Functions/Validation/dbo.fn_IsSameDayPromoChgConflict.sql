IF EXISTS (SELECT * from dbo.sysobjects WHERE id = object_id(N'[dbo].[fn_IsSameDayPromoChgConflict]') AND xtype IN (N'FN', N'IF', N'TF'))
	DROP FUNCTION [dbo].[fn_IsSameDayPromoChgConflict]
GO

CREATE FUNCTION dbo.fn_IsSameDayPromoChgConflict (
    @Item_Key int,
    @PriceChgTypeID int,
    @StartDate smalldatetime,
    @StoreList varchar(8000),
    @StoreListSeparator char(1),
    @PriceBatchDetailId int = NULL
    )
RETURNS bit
AS

BEGIN
	-- This returns true if there is an existing PriceBatchDetail record for the same PriceChgTypeID and StartDate passed in;
	-- Fully Processed batches don't count, but unbatched records DO count
	DECLARE @IsConflict bit

	SELECT @IsConflict = CASE WHEN EXISTS (SELECT 1
											FROM PriceBatchDetail PBD (nolock)
											INNER JOIN
												fn_Parse_List(@StoreList, @StoreListSeparator) Store
												ON Store.Key_Value = PBD.Store_No
											INNER JOIN
												PriceChgType (nolock) PCT
												ON PCT.PriceChgTypeID = PBD.PriceChgTypeID
											LEFT JOIN
												PriceBatchHeader PBH (nolock)
												ON PBD.PriceBatchHeaderID = PBH.PriceBatchHeaderID
											WHERE PBD.Item_Key = @Item_Key							--SPECIFIC ITEM KEY
												AND ISNULL(PBH.PriceBatchStatusID, 0) < 6			--ONLY UNPROCESSED BATCHES
												AND PBD.PriceChgTypeID = @PriceChgTypeID			--SPECIFIC PRICE CHANGE TYPE
												AND PBD.StartDate = @StartDate						--SPECIFIC START DATE
												AND PBD.PriceBatchDetailId <> ISNULL(@PriceBatchDetailId,0)	--NOT CURRENT BATCH (if any)
												AND Expired = 0) THEN 1 
							ELSE 0 END

	-- RETURN VALUE
	RETURN @IsConflict
END
GO
