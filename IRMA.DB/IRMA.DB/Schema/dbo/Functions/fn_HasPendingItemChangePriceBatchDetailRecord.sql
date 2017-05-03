-- When a change is processed in IRMA that should trigger an Item change in the PriceBatchDetail table,
-- the application should first check the following:
--		Do not create another PBD record for the item if there is already
--		an unexpired Item Change PBD record not assigned to a batch or 
--		assigned to a batch in the "Building" status
--		UNLESS the PBD record is for a future dated Off Promo Cost record
CREATE FUNCTION dbo.fn_HasPendingItemChangePriceBatchDetailRecord (
	@Item_Key int,
	@Store_No int
)
RETURNS bit
AS

BEGIN  
	RETURN 
		CASE WHEN (SELECT COUNT(1)							
                    FROM PriceBatchDetail (nolock) PBD		 
                    LEFT JOIN						  
                        PriceBatchHeader (nolock) PBH		 
                        ON PBD.PriceBatchHeaderID = PBH.PriceBatchHeaderID
                    WHERE PBD.Item_Key = @Item_Key 
						AND PBD.Store_No = @Store_No
                        AND ISNULL(PBH.PriceBatchStatusID, 0) < 2
                        AND PBD.ItemChgTypeID IS NOT NULL
                        AND ISNULL(PBD.Expired, 0) = 0 
                        AND NOT(PBD.ItemChgTypeID = 6 AND PBD.StartDate > GetDate())) > 0
		THEN 1 -- there IS a pending record
		ELSE 0 -- there IS NOT a pending record
		END
END