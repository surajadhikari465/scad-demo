CREATE FUNCTION dbo.fn_PriceSuperseded
    (@Item_Key		int,
	@Store_No		int,
	@PriceChgTypeId	int,				-- The Price Type of the PBD being checked
	@StartDate		smalldatetime,		-- The start date of the PBD being checked
	@SearchDate		smalldatetime)		-- the effective date we are interested in 
										--		start date of the batch header record
RETURNS bit
AS
BEGIN
	DECLARE @result bit
	
	DECLARE @Priority int
	
	----------------------------------------------------------------------------------------
	-- 2007-09-20
	-- Some calls of this function in the procedure [GetPriceBatchItemSearch] were changed
	-- to use the equivalent SQL below instead in an effort to improve performance.  Any 
	-- changes made below must be kept in synch with the logic used in that procedure.
	----------------------------------------------------------------------------------------

	SELECT	@Priority = Priority
	FROM	PriceChgType (NOLOCK)
	WHERE	PriceChgTypeId = @PriceChgTypeId

	SELECT @Result = CASE WHEN EXISTS (	
			SELECT	1
			FROM	PriceBatchDetail PBD (NOLOCK)
			INNER JOIN PriceChgType PCT (NOLOCK)
			ON		PCT.PriceChgTypeID = PBD.PriceChgTypeId
			WHERE	PBD.Item_Key		= @Item_Key
		  	  AND	PBD.Store_No		= @Store_No
			  		-- If the other sale ends before this one starts, it won't supersede
		  	  AND	PBD.Sale_End_Date	>= @StartDate
				 	-- if the sale ends after the search date, it won't supersede
			  AND	PBD.Sale_End_Date	>= @SearchDate
					-- if the sale hasn't started yet, it won't supersede 
			  AND	PBD.StartDate		<= @SearchDate
					-- if it's a higher priority sale, then it always supersedes
		  	  AND	(	PCT.Priority	> @Priority OR
					-- if it's the same priority sale, and starts after the price in question, then it supersedes
						-- we disallow same StartDate for a given Store/Item
						-- because of that, we can use this query for existing records as well as new potential records
					(	PCT.Priority	= @Priority 
					 AND PBD.StartDate > @StartDate ))
					-- ignore expired entries
			  AND	PBD.Expired	= 0
			) THEN 1 
		ELSE 0 END

	RETURN @result
END