IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Administration_GetPriceBatchHeaderSearch]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP  Procedure  [dbo].[Administration_GetPriceBatchHeaderSearch]
GO

CREATE PROCEDURE dbo.[Administration_GetPriceBatchHeaderSearch]
    @StoreList varchar(MAX),
    @StoreListSeparator char(1),
    @PriceBatchStatusIDList varchar(MAX),
    @PriceBatchStatusIDSeparator char(1),
    @BatchDescription varchar(30)
AS
BEGIN
    SET NOCOUNT ON
	-----------------------------------
	-- Return the search data
	-----------------------------------
	SELECT 
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
		[AutoApplyFlag] = PBH.AutoApplyFlag, 
		[PrintedDate] = MAX(PBH.PrintedDate),
		[SentDate] = MAX(PBH.SentDate),
		[ApplyDate] = MAX(PBH.ApplyDate),
		[POSBatchId] = MAX(PBH.POSBatchID),
		[SubTeam_Name] = RTRIM(MAX(ST.SubTeam_Name)),
		[BypassPrintShelfTags] = dbo.fn_InstanceDataValue('BypassPrintShelfTags', PBD.Store_No),
		[BypassApplyBatches] = dbo.fn_InstanceDataValue('BypassApplyBatches', PBD.Store_No)
	FROM PriceBatchHeader PBH (NOLOCK)
		INNER JOIN PriceBatchDetail PBD (NOLOCK) ON 
			PBD.PriceBatchHeaderID = PBH.PriceBatchHeaderID
		INNER JOIN Store S (NOLOCK) ON 
			S.Store_No = PBD.Store_No
		INNER JOIN dbo.fn_Parse_List(@StoreList, @StoreListSeparator) LIST ON 
			LIST.Key_Value = S.Store_No
		INNER JOIN ItemIdentifier II (NOLOCK) ON 
			II.Item_Key = PBD.Item_Key
		INNER JOIN Item I (NOLOCK) ON 
			I.Item_Key = II.Item_Key				
		INNER JOIN SubTeam ST (NOLOCK) ON 
			ST.SubTeam_No = ISNULL(PBD.SubTeam_No, I.SubTeam_No)
		INNER JOIN PriceBatchStatus (NOLOCK) PBS ON 
			PBS.PriceBatchStatusID = PBH.PriceBatchStatusID
		INNER JOIN dbo.fn_Parse_List(@PriceBatchStatusIDList, @PriceBatchStatusIDSeparator) STATUS_LIST ON 
			STATUS_LIST.Key_Value = PBS.PriceBatchStatusID
		LEFT JOIN ItemChgType ICT (NOLOCK) ON 
			ICT.ItemChgTypeID = PBH.ItemChgTypeID
		LEFT JOIN PriceChgType PCT (NOLOCK) ON 
			PCT.PriceChgTypeID = PBH.PriceChgTypeID
	WHERE 
		-- Batch Description
		(@BatchDescription IS NULL 
			OR (@BatchDescription IS NOT NULL AND PBH.BatchDescription LIKE '%' + REPLACE(@BatchDescription, '[', '[[]') + '%'))
	GROUP BY PBH.PriceBatchHeaderID, PBD.Store_No, ISNULL(PBD.SubTeam_No, I.SubTeam_No), PBH.AutoApplyFlag
	ORDER BY PBH.PriceBatchHeaderID, PBD.Store_No, ISNULL(PBD.SubTeam_No, I.SubTeam_No)

    SET NOCOUNT OFF
END
GO

 