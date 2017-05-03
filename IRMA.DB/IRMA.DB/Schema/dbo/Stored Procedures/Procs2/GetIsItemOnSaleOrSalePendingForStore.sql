CREATE PROCEDURE dbo.GetIsItemOnSaleOrSalePendingForStore
    @Item_Key int,
    @Store_No int
AS

BEGIN
    SET NOCOUNT ON
	DECLARE @IsOnSaleOrSalePending BIT
	SET @IsOnSaleOrSalePending = 0

	-- RETURN TRUE IF:
	-- 1. THE ITEM IS CURRENTLY ON SALE IN IRMA FOR THE SELECTED STORE OR
	-- 2. THERE IS AT LEAST ONE PENDING (NOT ALREADY PROCESSED) SALE PRICE CHANGE FOR THE ITEM-STORE  
	SELECT @IsOnSaleOrSalePending =
		CASE WHEN EXISTS (	SELECT 1 FROM
							Price (nolock) P
							INNER JOIN
								PriceChgType (nolock) PCT
								ON PCT.PriceChgTypeID = P.PriceChgTypeID
							WHERE
								P.Item_Key = @Item_Key 
								AND P.Store_No = @Store_No
								AND PCT.On_Sale = 1				-- THE ITEM IS ASSIGNED TO A SALE PRICE TYPE
								AND P.Sale_End_Date > GetDate() -- THE SALE IS STILL ONGOING
						  ) 
			THEN 1 -- ITEM IS CURRENTLY ON SALE IN IRMA
			ELSE 0 -- ITEM IS NOT CURRENTLY ON SALE IN IRMA
		END

	IF @IsOnSaleOrSalePending = 0 
	BEGIN
		SELECT @IsOnSaleOrSalePending =
			CASE WHEN EXISTS (	SELECT 1 FROM
								PriceBatchDetail PBD (nolock)
								INNER JOIN
									Store (nolock)
									ON Store.Store_No = PBD.Store_No
								INNER JOIN
									PriceChgType (nolock) PCT
									ON PCT.PriceChgTypeID = PBD.PriceChgTypeID
								LEFT JOIN
									PriceBatchHeader PBH (nolock)
									ON PBD.PriceBatchHeaderID = PBH.PriceBatchHeaderID
								WHERE PBD.Item_Key = @Item_Key
									AND PBD.Store_No = @Store_No
									AND ISNULL(PBH.PriceBatchStatusID, 0) < 6 -- UNPROCESSED BATCHES ONLY
									AND PBD.PriceChgTypeID IS NOT NULL		  -- PRICE CHANGE REQUIRED
									AND PCT.On_Sale = 1						  -- MUST BE A "SALE" PRICE CHANGE TYPE
									AND PBD.Expired = 0						  -- EXCLUDE EXPIRED PRICE BATCHES
									AND PBD.Sale_End_Date > GetDate()		  -- SALE IS STILL ONGOING	
							  )		
				THEN 1 -- SALE(S) ARE PENDING FOR THE ITEM 
				ELSE 0 -- SALE(S) ARE NOT PENDING FOR THE ITEM
			END
	END

	SELECT IsOnSaleOrSalePending = @IsOnSaleOrSalePending 
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetIsItemOnSaleOrSalePendingForStore] TO [IRMAClientRole]
    AS [dbo];

