CREATE PROCEDURE [dbo].[GetPriceBatchSearch]
    @StoreList varchar(MAX),
    @StoreListSeparator char(1),
    @ItemChgTypeID tinyint,
    @PriceChgTypeID tinyint,
    @PriceBatchStatusID tinyint,
    @SubTeam_No int,
    @FromStartDate smalldatetime,
    @ToStartDate smalldatetime,
    @Identifier varchar(255),
    @Item_Description varchar(255),
    @BatchDescription varchar(30),
	@AutoApplyFlag bit,
	@ApplyDate datetime,
	@MaxRowsToReturn int = 0

AS

BEGIN
    SET NOCOUNT ON

	DECLARE @MaxRows int

--------------------------------------------------------------------------------------------------------
-- TEMPORARY!!! (2007-09-19)
-- hardcode to 1001 until the VB code is changed to add the parameter @MaxRowsToReturn
-- reason: currently the VB code limits the displayed rows to 1000 in the loop processing the result set
	IF @MaxRowsToReturn = 0
		SELECT @MaxRowsToReturn = 1001	-- use 1001 instead of 1000 so code knows if more rows exist
--------------------------------------------------------------------------------------------------------

	-----------------------------------
	-- Limit the number of rows returned
	-----------------------------------
	IF ISNULL(@MaxRowsToReturn, 0) > 0
		SELECT @MaxRows = @MaxRowsToReturn
	ELSE
		-- no limit specified; set to max int value (2,147,483,647)
		SELECT @MaxRows = 2147483647
    
	-----------------------------------
	-- Return the search data
	-----------------------------------
	SELECT TOP(@MaxRows)
		[PriceBatchHeaderID] = PBH.PriceBatchHeaderID, 
		[Store_No] = PBD.Store_No,
		[Store_Name] = RTRIM(MAX(S.Store_Name)),
		[SubTeam_No] = ISNULL(PBD.SubTeam_No, I.SubTeam_No),
		[StartDate] = MAX(PBH.StartDate), 
		[PriceBatchStatusID] = MAX(PBH.PriceBatchStatusID), 
		[PriceBatchStatusDesc] = RTRIM(MAX(PBS.PriceBatchStatusDesc)), 
		[ItemChgTypeID] = MAX(PBH.ItemChgTypeID), 
		[ItemChgTypeDesc] = ISNULL(RTRIM(MAX(ICT.ItemChgTypeDesc)), 'Price'), 
		[PriceChgTypeID] = MAX(PBH.PriceChgTypeID), 
		[PriceChgTypeDesc] = ISNULL(RTRIM(MAX(PCT.PriceChgTypeDesc)), ''), 
		[TotalItems] = dbo.fn_PriceBatchDetailCount(PBH.PriceBatchHeaderID),
		[BatchDescription] = RTRIM(MAX(PBH.BatchDescription)), 
		[Auto] = CASE WHEN PBH.AutoApplyFlag = 0 THEN '' ELSE '*' END, 
		[ApplyDate] = MAX(PBH.ApplyDate),
		[POSBatchId] = MAX(PBH.POSBatchID),
		[SubTeam_Name] = RTRIM(MAX(ST.SubTeam_Name)),
		[PrintShelfTags] = CASE WHEN dbo.fn_InstanceDataValue('BypassPrintShelfTags', PBD.Store_No) = 0 THEN '*' ELSE '' END,
		[ApplyBatches] = CASE WHEN dbo.fn_InstanceDataValue('BypassApplyBatches', PBD.Store_No) = 0 THEN '*' ELSE '' END
	FROM PriceBatchHeader PBH (NOLOCK)
		INNER JOIN PriceBatchDetail PBD (NOLOCK) ON PBD.PriceBatchHeaderID = PBH.PriceBatchHeaderID
		INNER JOIN Store S (NOLOCK) ON S.Store_No = PBD.Store_No
		INNER JOIN dbo.fn_Parse_List(@StoreList, @StoreListSeparator) LIST ON LIST.Key_Value = S.Store_No
		INNER JOIN ItemIdentifier II (NOLOCK) ON II.Item_Key = PBD.Item_Key
		INNER JOIN Item I (NOLOCK) ON I.Item_Key = II.Item_Key				
		INNER JOIN SubTeam ST (NOLOCK) ON ST.SubTeam_No = ISNULL(PBD.SubTeam_No, I.SubTeam_No)
		INNER JOIN PriceBatchStatus (NOLOCK) PBS ON PBS.PriceBatchStatusID = PBH.PriceBatchStatusID
		LEFT JOIN ItemChgType ICT (NOLOCK) ON ICT.ItemChgTypeID = PBH.ItemChgTypeID
		LEFT JOIN PriceChgType PCT (NOLOCK) ON PCT.PriceChgTypeID = PBH.PriceChgTypeID
	WHERE 
		--item description
		(@Item_Description IS NULL 
			OR (@Item_Description IS NOT NULL AND ISNULL(PBD.Item_Description, I.Item_Description) LIKE '%' + REPLACE(@Item_Description, '[', '[[]') + '%'))
		-- identifier
		AND (@Identifier IS NULL 
			OR (@Identifier IS NOT NULL AND ISNULL(PBD.Identifier, II.Identifier) LIKE '%' + @Identifier + '%'))
		-- Price Batch Status
		AND ((@PriceBatchStatusID IS NULL AND PBH.PriceBatchStatusID < 6)		-- (6 = Processed)
			OR (@PriceBatchStatusID IS NOT NULL AND PBH.PriceBatchStatusID = @PriceBatchStatusID)) 
		-- Batch Description
		AND (@BatchDescription IS NULL 
			OR (@BatchDescription IS NOT NULL AND PBH.BatchDescription LIKE '%' + REPLACE(@BatchDescription, '[', '[[]') + '%'))
		-- Auto Apply Flag
		AND PBH.AutoApplyFlag = ISNULL(@AutoApplyFlag, PBH.AutoApplyFlag)
		-- Apply Date
		AND ISNULL(PBH.ApplyDate, 0) >= COALESCE(@ApplyDate, PBH.ApplyDate, 0)
		-- date range
		AND ISNULL(PBH.StartDate, 0) BETWEEN COALESCE(@FromStartDate, PBH.StartDate, 0) AND COALESCE(@ToStartDate, PBH.StartDate, 0)
		-- PriceChgTypeId
		AND (ISNULL(PBH.PriceChgTypeID, 0) = CASE WHEN ISNULL(@ItemChgTypeID, 0) = 5 THEN ISNULL(PBH.PriceChgTypeID, 0) ELSE ISNULL(@PriceChgTypeID, 0) END)
		--itemchgtype
		AND ((@ItemChgTypeID = 2 AND PBH.ItemChgTypeID IN (2,6)) -- ITEM CHANGES(2) ALSO INCLUDE 'OFF PROMO COST'(6) CHANGES
			OR (@ItemChgTypeID = 5 
				AND (PBH.ItemChgTypeID IS NULL OR PBH.ItemChgTypeID IN (1,2,3,4,6))) -- STATUS 5 = ALL
			OR (@ItemChgTypeID NOT IN (2,5) AND PBH.ItemChgTypeID = @ItemChgTypeID)
			OR (@ItemChgTypeID IS NULL AND PBH.ItemChgTypeID IS NULL))
		-- sub team
		AND (@SubTeam_No IS NULL 
			OR (ISNULL(PBD.SubTeam_No, I.SubTeam_No) = @SubTeam_No))   			
	GROUP BY PBH.PriceBatchHeaderID, PBD.Store_No, ISNULL(PBD.SubTeam_No, I.SubTeam_No), PBH.AutoApplyFlag
	ORDER BY PBH.PriceBatchHeaderID, PBD.Store_No, ISNULL(PBD.SubTeam_No, I.SubTeam_No)
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPriceBatchSearch] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPriceBatchSearch] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPriceBatchSearch] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPriceBatchSearch] TO [IRMAReportsRole]
    AS [dbo];

