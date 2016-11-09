
CREATE PROCEDURE dbo.IsSameDayPromoChgConflict
    @Item_Key int,
    @PriceChgTypeID int,
    @StartDate smalldatetime,
    @StoreList varchar(8000),
    @StoreListSeparator char(1)
    ,
    @IsSameDayPromoConflict bit OUTPUT
AS

BEGIN
	-- This returns true if there is an existing PriceBatchDetail record for the same PriceChgTypeID and StartDate passed in;
	-- Fully Processed batches don't count, but unbatched records DO count
    SET NOCOUNT ON

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
											WHERE PBD.Item_Key = @Item_Key				  --SPECIFIC ITEM KEY
												AND ISNULL(PBH.PriceBatchStatusID, 0) < 6 --ONLY UNPROCESSED BATCHES
												AND PBD.PriceChgTypeID = @PriceChgTypeID  --SPECIFIC PRICE CHANGE TYPE
												AND PBD.StartDate = @StartDate			  --SPECIFIC START DATE
												AND Expired = 0) THEN 1 
							ELSE 0 END

	--SET OUTPUT PARAM AS WELL AS RETURN VALUE
	SET @IsSameDayPromoConflict = @IsConflict

	SELECT @IsConflict

    SET NOCOUNT OFF
END


GO



GO
GRANT EXECUTE
    ON OBJECT::[dbo].[IsSameDayPromoChgConflict] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[IsSameDayPromoChgConflict] TO [IRMAClientRole]
    AS [dbo];

