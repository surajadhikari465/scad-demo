CREATE PROCEDURE dbo.GetIsBatchedByStatus
    @Item_Key int,
    @StoreList varchar(8000),
    @StoreListSeparator char(1),
    @BatchStatus varchar(20)
    ,
    @IsExistingUnprocessedBatch bit OUTPUT
AS

BEGIN
	-- This returns true if there is an existing batch found for the passed in @BatchStatus
	-- NOTE: @BatchStatus can be passed in a list of statuses; [ie: 2,3,4,5 (all but Building and Processed)]
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
												AND PBH.PriceBatchStatusId IN (SELECT Key_value FROM dbo.fn_Parse_List(@BatchStatus, ','))	
												AND	PBD.PriceChgTypeID IS NOT NULL --ONLY CHECK PRICE CHANGES (EXCLUDE ITEM CHG TYPES)
												AND Expired = 0) THEN 1 
							ELSE 0 END

	--SET OUTPUT PARAM AS WELL AS RETURN VALUE
	SET @IsExistingUnprocessedBatch = @IsBatched

	SELECT IsBatched = @IsBatched 

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetIsBatchedByStatus] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetIsBatchedByStatus] TO [IRMAClientRole]
    AS [dbo];

