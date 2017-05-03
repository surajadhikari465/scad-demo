CREATE PROCEDURE dbo.GetPriceBatchDetailSumReport
    @StoreList varchar(8000),
    @StoreListSeparator char(1),
    @SubTeam_No int,
    @StartDate smalldatetime,
    @EndDate smalldatetime
AS
-- =============================================================
-- -------------------------------------------------------------
-- Revision History
-- -------------------------------------------------------------
-- 09/18/2013  MZ   TFS 13667 - Added SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
-- =============================================================
BEGIN
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
    SET NOCOUNT ON
    
    DECLARE @Store TABLE (Store_No int)

    IF @StoreList IS NOT NULL
        INSERT INTO @Store
        SELECT Key_Value
        FROM dbo.fn_Parse_List(@StoreList, @StoreListSeparator) S
    ELSE
        INSERT INTO @Store
        SELECT Store_No FROM Store (nolock)

	/*
		Tom Lux
		TFS 11468
		12/17/2009
		Price changes (with no item change) were being shown on report as "New", so I changed to be "Price" because if there is no item change type,
		it is a price change.
	*/
    SELECT COUNT(1) AS Total, Results.SubTeam_No, Subteam.SubTeam_Name, Results.ItemChgTypeID, ISNULL(ItemChgTypeDesc, 'Price') AS ItemChgTypeDesc,	
			PriceChgTypeID, PriceChgTypeDesc, ISNULL(StartDate,@StartDate) AS StartDate

	FROM (
		----------------------------------------------------------------------------------
		--ITEM CHG TYPE = NEW
		SELECT DISTINCT PBD.Item_Key, Item.Item_Description, Item.SubTeam_No, NULL AS ItemChgTypeID, PBD.StartDate,				
				CASE WHEN PBD.PriceChgTypeID IS NULL THEN Price.PriceChgTypeID
					 ELSE PBD.PriceChgTypeID
					 END AS PriceChgTypeID, 
				CASE WHEN PBD.PriceChgTypeID IS NULL THEN PCT_Price.PriceChgTypeDesc
					 ELSE PCT.PriceChgTypeDesc
					 END AS PriceChgTypeDesc
		FROM PriceBatchDetail PBD (nolock)
		INNER JOIN
			@Store S
			ON S.Store_No = PBD.Store_No
		INNER JOIN
			Item (nolock)
			ON Item.Item_Key = PBD.Item_Key
		INNER JOIN
			Price (nolock)
			ON Price.Item_Key = PBD.Item_Key AND Price.Store_No = PBD.Store_No		
		LEFT JOIN
			PriceChgType PCT (nolock)
			ON PCT.PriceChgTypeID = PBD.PriceChgTypeID  --JOIN TO PRICE_BATCH_DETAIL
		LEFT JOIN
			PriceChgType PCT_Price (nolock)
			ON PCT_Price.PriceChgTypeID = Price.PriceChgTypeID --JOIN TO PRICE
		WHERE PriceBatchHeaderID IS NULL
			AND Item.SubTeam_No = ISNULL(@SubTeam_No, Item.SubTeam_No)
			AND PBD.ItemChgTypeID IS NULL
			AND EXISTS (SELECT *	--ENSURE THAT ANY PRICE HAS BEEN DEFINED FOR NEW ITEM
						FROM PriceBatchDetail D (nolock)
						WHERE D.PriceBatchHeaderID IS NULL
							AND D.Item_Key = PBD.Item_Key AND D.Store_No = PBD.Store_No
							AND D.ItemChgTypeID = 1
							AND PBD.Expired = 0)		--NEW ITEM CHG TYPE ONLY
			AND StartDate >= ISNULL(@StartDate, StartDate)
			AND StartDate <= ISNULL(@EndDate, StartDate)
			AND PBD.Expired = 0 --EXCLUDE EXPIRED BATCH DATA
		
		UNION
		----------------------------------------------------------------------------------
		--ITEM CHG TYPE = ITEM
		SELECT DISTINCT PBD.Item_Key, Item.Item_Description, Item.SubTeam_No, PBD.ItemChgTypeID, PBD.StartDate,
				CASE WHEN PBD.PriceChgTypeID IS NULL THEN Price.PriceChgTypeID
					 ELSE PBD.PriceChgTypeID
					 END AS PriceChgTypeID, 
				CASE WHEN PBD.PriceChgTypeID IS NULL THEN PCT_Price.PriceChgTypeDesc
					 ELSE PCT.PriceChgTypeDesc
					 END AS PriceChgTypeDesc				
		FROM PriceBatchDetail PBD (nolock)
		INNER JOIN
			@Store S
			ON S.Store_No = PBD.Store_No
		INNER JOIN
			Item (nolock)
			ON Item.Item_Key = PBD.Item_Key
		INNER JOIN
			Price (nolock)
			ON Price.Item_Key = PBD.Item_Key AND Price.Store_No = PBD.Store_No		
		LEFT JOIN
			PriceChgType PCT (nolock)
			ON PCT.PriceChgTypeID = PBD.PriceChgTypeID
		LEFT JOIN
			PriceChgType PCT_Price (nolock)
			ON PCT_Price.PriceChgTypeID = Price.PriceChgTypeID
		WHERE PriceBatchHeaderID IS NULL
			AND Item.SubTeam_No = ISNULL(@SubTeam_No, Item.SubTeam_No) --FILTER FOR ALL TYPES
			AND PBD.ItemChgTypeID = 2 --ITEM CHG TYPE ONLY; DO NOT FILTER ON DATES
			AND PBD.PriceChgTypeID IS NULL
			AND PBD.Expired = 0 --EXCLUDE EXPIRED BATCH DATA

		UNION
		----------------------------------------------------------------------------------
		--ITEM CHG TYPE = DELETE
		SELECT DISTINCT PBD.Item_Key, Item.Item_Description, Item.SubTeam_No, PBD.ItemChgTypeID, PBD.StartDate,
				NULL AS PriceChgTypeID, '' AS PriceChgTypeDesc --DELETES NOT ASSOCIATED W/ PRICE TYPE
		FROM PriceBatchDetail PBD (nolock)
		INNER JOIN
			@Store S
			ON S.Store_No = PBD.Store_No
		INNER JOIN
			Item (nolock)
			ON Item.Item_Key = PBD.Item_Key
		INNER JOIN
			Price (nolock)
			ON Price.Item_Key = PBD.Item_Key AND Price.Store_No = PBD.Store_No		
		LEFT JOIN
			PriceChgType PCT (nolock)
			ON PCT.PriceChgTypeID = PBD.PriceChgTypeID
		LEFT JOIN
			PriceChgType PCT_Price (nolock)
			ON PCT_Price.PriceChgTypeID = Price.PriceChgTypeID
		WHERE PriceBatchHeaderID IS NULL
			AND Item.SubTeam_No = ISNULL(@SubTeam_No, Item.SubTeam_No) --FILTER FOR ALL TYPES
			AND PBD.ItemChgTypeID = 3 --DELETE ITEMS ONLY
			AND StartDate >= ISNULL(@StartDate, StartDate) --DATE IS ENTERED BY USER WHEN DELETE OCCURS
			AND StartDate <= ISNULL(@EndDate, StartDate)
			AND PBD.Expired = 0 --EXCLUDE EXPIRED BATCH DATA
			
		UNION
		----------------------------------------------------------------------------------
		--CHG TYPE = PRICE
		SELECT DISTINCT PBD.Item_Key, Item.Item_Description, Item.SubTeam_No, PBD.ItemChgTypeID, PBD.StartDate,
				PBD.PriceChgTypeID, PCT.PriceChgTypeDesc
        FROM PriceBatchDetail PBD (nolock)
        INNER JOIN
            @Store S
            ON S.Store_No = PBD.Store_No
        INNER JOIN
            Item (nolock)
            ON Item.Item_Key = PBD.Item_Key
        INNER JOIN
			Price (nolock)
			ON Price.Item_Key = PBD.Item_Key AND Price.Store_No = PBD.Store_No		
		LEFT JOIN
			PriceChgType PCT (nolock)
			ON PCT.PriceChgTypeID = PBD.PriceChgTypeID    
        WHERE PriceBatchHeaderID IS NULL			
            AND ItemChgTypeID IS NULL
			AND PBD.PriceChgTypeID IS NOT NULL
			AND NOT EXISTS (SELECT *				--EXCLUDE NEW ITEMS; THIS SHOULD RETURN ONLY "PRICE" CHG TYPE RECORDS AVAILABLE
							FROM PriceBatchDetail D
							WHERE D.Item_Key = PBD.Item_Key AND D.Store_No = PBD.Store_No 
								AND PriceBatchHeaderID IS NULL
								AND ItemChgTypeID = 1
								AND PBD.Expired = 0)
			AND StartDate >= ISNULL(@StartDate, StartDate)
			AND StartDate <= ISNULL(@EndDate, StartDate)
			AND Item.SubTeam_No = ISNULL(@SubTeam_No, Item.SubTeam_No)
			AND PBD.Expired = 0 --EXCLUDE EXPIRED BATCH DATA
		----------------------------------------------------------------------------------

		-- ** HANDLE THESE??? **
		--	AND PBD.ItemChgTypeID = 4 --PROMO OFFER 
		--	AND PBD.ItemChgTypeID = 6 --OFF PROMO COST
	) Results
	INNER JOIN
		SubTeam (nolock)
		ON SubTeam.SubTeam_No = Results.SubTeam_No
	LEFT JOIN
		ItemChgType (nolock)
		ON ItemChgType.ItemChgTypeID = Results.ItemChgTypeID	
	GROUP BY Results.SubTeam_No, SubTeam_Name, Results.ItemChgTypeID, ItemChgTypeDesc, PriceChgTypeID, PriceChgTypeDesc, StartDate
	ORDER BY SubTeam_No, ItemChgTypeDesc, StartDate

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPriceBatchDetailSumReport] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPriceBatchDetailSumReport] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPriceBatchDetailSumReport] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPriceBatchDetailSumReport] TO [IRMAReportsRole]
    AS [dbo];

