CREATE FUNCTION [dbo].[fn_GetPLUMCorpOrScalePriceChangeKeys]
   (@Date            datetime,
    @KeyType	     varchar(5))
RETURNS @ItemKeys TABLE 
	(Item_Key   int)
AS
BEGIN
	DECLARE @Result               VARCHAR(MAX)
    DECLARE @PluDigitsSentToScale VARCHAR(20)
            
    SELECT @PluDigitsSentToScale = PluDigitsSentToScale FROM InstanceData

    IF @KeyType = 'PLUM'
        BEGIN
			-- Return all of the item changes that were processed by this run of Scale Push and that do not have a
			-- price change that is in a 'Sent' state batch.  
			-- These items are  in the TMP queue because it does not get cleared out until after Scale Push succeeds 
			-- for the stores.
            INSERT INTO @ItemKeys (Item_Key)
                SELECT DISTINCT Item_Key
                FROM   PLUMCorpChgQueueTmp 
                WHERE  (ActionCode = 'A' OR ActionCode = 'C') AND
                       Item_Key NOT IN (SELECT DISTINCT Item_Key
                                        FROM   [dbo].[fn_GetPriceBatchDetailPrices](@Date, 0, 65535, 1, @PluDigitsSentToScale))
        END        
    ELSE
        BEGIN  
			-- Return all of the price changes that are in a 'Sent' state batch and that do not have a corresponding
			-- scale item change that is being processed by this run of Scale Push. 
            INSERT INTO @ItemKeys (Item_Key)
                SELECT DISTINCT Item_Key
                FROM   [dbo].[fn_GetPriceBatchDetailPrices](@Date, 0, 65535, 1, @PluDigitsSentToScale)
                WHERE  Item_Key NOT IN (SELECT DISTINCT Item_Key FROM PLUMCorpChgQueue WHERE ActionCode = 'A' OR ActionCode = 'C') AND
					   Item_Key NOT IN (SELECT DISTINCT Item_Key FROM PLUMCorpChgQueueTmp WHERE ActionCode = 'A' OR ActionCode = 'C')
        END
        
    RETURN
END
GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_GetPLUMCorpOrScalePriceChangeKeys] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_GetPLUMCorpOrScalePriceChangeKeys] TO [IRMASchedJobsRole]
    AS [dbo];

