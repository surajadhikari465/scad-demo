
CREATE PROCEDURE dbo.GetIsBatched
    @Item_Key int,
    @StoreList varchar(8000),
    @StoreListSeparator char(1)
    ,
    @IsExistingUnprocessedBatch bit OUTPUT
AS

BEGIN
	-- This returns true if there is an unfinished batch containing this store/item combo
	-- Fully Processed batches and unbatched records don't count
    SET NOCOUNT ON

	DECLARE @IsBatched bit

	SELECT @IsBatched = CASE WHEN EXISTS (SELECT 1 
											FROM PriceBatchDetail PBD
											LEFT JOIN PriceBatchHeader PBH
												ON PBD.PriceBatchHeaderId = PBH.PriceBatchHeaderId
											INNER JOIN
												fn_Parse_List(@StoreList, @StoreListSeparator) Store
												ON Store.Key_Value = PBD.Store_No
											WHERE PBD.Item_Key = @Item_Key 
												AND ISNULL(PBH.PriceBatchStatusId,6) < 6 --WHEN PriceBatchStatusId IS NULL EXCLUDE (BY SPOOFING AS 6) 
																						 --BECAUSE THIS IS NOT YET IN A BATCH
												AND	PBD.PriceChgTypeID IS NOT NULL --ONLY CHECK PRICE CHANGES (EXCLUDE ITEM CHG TYPES)
												AND Expired = 0) THEN 1 
							ELSE 0 END

	--SET OUTPUT PARAM AS WELL AS RETURN VALUE
	SET @IsExistingUnprocessedBatch = @IsBatched

	SELECT IsBatched = @IsBatched 

    SET NOCOUNT OFF
END


GO



GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetIsBatched] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetIsBatched] TO [IRMAClientRole]
    AS [dbo];

