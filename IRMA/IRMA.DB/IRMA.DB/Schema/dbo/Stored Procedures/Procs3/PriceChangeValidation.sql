
CREATE PROCEDURE dbo.PriceChangeValidation
    @Item_Key int,
    @PriceChgTypeID int,
    @StartDate smalldatetime,
    @StoreList varchar(8000),
    @StoreListSeparator char(1)
    ,
    @IsExistingUnprocessedBatch bit OUTPUT,
    @IsSameDayPromoConflict bit OUTPUT
AS

BEGIN
	-- This proc calls 2 separate validation procs and sets OUTPUT params based on those validations resulting in true or false
	-- VALIDATION 1: GetIsBatched --> This returns true if there is an unfinished batch containing this store/item combo
	--								  Fully Processed batches and unbatched records don't count
	
	-- VALIDATION 2: IsSameDayPromoChgConflict --> returns true if there is an existing PriceBatchDetail record for the same PriceChgTypeID 
	--											   and StartDate passed in; Fully Processed batches don't count, but unbatched records DO count
    SET NOCOUNT ON
    
    Set @StartDate = dbo.fn_GetDateOnly(@StartDate)

	--SET @IsExistingUnprocessedBatch
	EXEC dbo.GetIsBatched @Item_Key, @StoreList, @StoreListSeparator, @IsExistingUnprocessedBatch OUTPUT
	
	--SET @IsSameDayPromoConflict
	EXEC dbo.IsSameDayPromoChgConflict @Item_Key, @PriceChgTypeID, @StartDate, @StoreList, @StoreListSeparator, @IsSameDayPromoConflict OUTPUT
		
    SET NOCOUNT OFF
END


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[PriceChangeValidation] TO [IRMAClientRole]
    AS [dbo];

