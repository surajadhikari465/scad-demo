
CREATE PROCEDURE [dbo].[Replenishment_POSPush_GetPriceBatchSent]
    @Date datetime,
    @Deletes bit,
    @MaxBatchItems int,
    @IsScaleZoneData bit,  -- USED TO LIMIT OUTPUT TO SCALE ITEMS 
	@IsItemNonBatchableChanges bit = 0 -- USED TO LIMIT OUTPUT TO NON BATCHABLE ITEM CHANGES
AS

/*********************************************************************************************************************************************
CHANGE LOG
DEV				DATE					TASK				DESCRIPTION
----------------------------------------------------------------------------------------------
DBS				2011-03-02				13835				Remove cursors populating @TaxFlagValues for performance
KM				2015-02-04				15774 (7103)		Call the stored procedure version of GetPriceBatchDetailPrices instead of TVF
															in order to improve performance;
***********************************************************************************************************************************************/

BEGIN

    SET NOCOUNT ON
    
	--Determine how region wants to send down data to scales
	DECLARE @PluDigitsSentToScale varchar(20)
	SELECT @PluDigitsSentToScale = PluDigitsSentToScale FROM InstanceData

	-- The POS Push process limits the total number of records that are sent from IRMA to the POS in a 
	-- single POS batch file.  
	-- Note: the StoreItemAuthorizationID will always be set to NULL for batched changes.  This is needed to let the
	-- POS Push processor know if this is batched item delete or an automatically communicated de-auth record.
    DECLARE @Send TABLE (PriceBatchHeaderID int, Store_No int, StartDate smalldatetime, AutoApplyFlag bit, ApplyDate smalldatetime, BatchDescription varchar(30), POSBatchID int, StoreItemAuthorizationID int)
    INSERT @Send
		(PriceBatchHeaderID, Store_No, StartDate, AutoApplyFlag, ApplyDate, BatchDescription, POSBatchID)
		SELECT PriceBatchHeaderID, Store_No, StartDate, AutoApplyFlag, ApplyDate, BatchDescription, POSBatchID
		FROM dbo.fn_GetPriceBatchHeadersForPushing(@Date, @Deletes, @MaxBatchItems)
		ORDER BY Store_No, PriceBatchHeaderID
	
    -- First resultset - list of PriceBatchHeader records affected - so they can be updated once processed completely
    SELECT PriceBatchHeaderID, Store_No, StartDate, AutoApplyFlag, ApplyDate, BatchDescription , POSBatchID, StoreItemAuthorizationID FROM @Send

    -- Parse each PriceBatchHeader record that it is being returned to get it's tax details
    DECLARE @TaxFlagValues TABLE (Store_No int, Item_Key int, TaxFlagKey char(1), TaxFlagValue bit, TaxPercent decimal(9,4), POSID int)
  
			INSERT INTO @TaxFlagValues 
				SELECT  TF.Store_No, TF.Item_Key, TF.TaxFlagKey, TF.TaxFlagValue, TF.TaxPercent, TF.POSID
				FROM PriceBatchDetail PBD 
				INNER JOIN
					@Send S
					ON PBD.PriceBatchHeaderID = S.PriceBatchHeaderID
				CROSS APPLY  (SELECT Store_No, Item_Key, TaxFlagKey, TaxFlagValue, TaxPercent, POSID
						FROM dbo.fn_TaxFlagData(PBD.Item_Key, PBD.Store_No)) AS TF
				ORDER BY PBD.Store_No

    
    -- Also insert a record into the TaxFlagValues table for each of the POS de-auth records that will be included in the
    -- fourth result set.
    IF @Deletes = 1
	BEGIN		
		--Using the regional scale file?
		DECLARE @UseRegionalScaleFile bit
		SELECT @UseRegionalScaleFile = (SELECT FlagValue FROM InstanceDataFlags WHERE FlagKey='UseRegionalScaleFile')

			INSERT INTO @TaxFlagValues 
				SELECT TF.Store_No, TF.Item_Key, TF.TaxFlagKey, TF.TaxFlagValue, TF.TaxPercent, TF.POSID
				FROM StoreItem si
				CROSS APPLY  (SELECT Store_No, Item_Key, TaxFlagKey, TaxFlagValue, TaxPercent, POSID
										FROM dbo.fn_TaxFlagData(si.Item_Key, si.Store_No)) AS TF
				WHERE 
				   ((@IsScaleZoneData = 0 AND si.POSDeAuth = 1) OR
					(@IsScaleZoneData = 1 AND si.ScaleDeAuth = 1 AND @UseRegionalScaleFile = 0))

	END

	-- Also insert a record into the TaxFlagValues table for each of the Item Non-Batchable changes that will be included
	-- in the fourth result set
	IF @IsItemNonBatchableChanges = 1
	BEGIN
		INSERT INTO @TaxFlagValues
			SELECT TF.Store_No, TF.Item_Key, TF.TaxFlagKey, TF.TaxFlagValue, TF.TaxPercent, TF.POSID
			FROM ItemNonBatchableChanges inbc 
			INNER JOIN StoreItem si on inbc.Item_Key = si.Item_Key
			CROSS APPLY  (SELECT Store_No, Item_Key, TaxFlagKey, TaxFlagValue, TaxPercent, POSID
										FROM dbo.fn_TaxFlagData(si.Item_Key, si.Store_No)) AS TF
			WHERE NOT EXISTS
				(SELECT 1 
				FROM @TaxFlagValues tfv
				WHERE tfv.Item_Key = si.Item_Key 
					AND tfv.Store_No = si.Store_No)
	END

    -- Second resultset - tax hosting details for each item contained in the third resultset
    SELECT Store_No, Item_Key, TaxFlagKey, TaxFlagValue, TaxPercent, POSID FROM @TaxFlagValues ORDER BY Store_No, Item_Key, TaxFlagKey

	  -- Third resultset - list of items and their details for batched changes.
      -- Call GetPriceBatchDetailPrices, which returns all of the PBD details for the items being sent from IRMA to the POS/Scales.
      -- Sort order is different for deletes, for which Sale_Start_Date is irrelevant.
	IF @Deletes = 1
		BEGIN 
			EXEC dbo.GetPriceBatchDetailPrices @Date, @Deletes, @MaxBatchItems, @IsScaleZoneData, @PluDigitsSentToScale
			SELECT * FROM PosPushStagingPriceBatchDetail ORDER BY Store_No, PriceBatchHeaderID, Identifier, Item_Key
		END
	ELSE
		BEGIN
			EXEC dbo.GetPriceBatchDetailPrices @Date, @Deletes, @MaxBatchItems, @IsScaleZoneData, @PluDigitsSentToScale
			SELECT * FROM PosPushStagingPriceBatchDetail ORDER BY Store_No, Sale_Start_Date, PriceBatchHeaderID, Identifier, Item_Key
		END

	TRUNCATE TABLE PosPushStagingPriceBatchDetail


	IF @Deletes = 1
	-- DELETE RECORDS
	BEGIN
		-- Fourth resultset - list of items and their details for automatically communicated store de-auth changes
		-- All of the details for the items being sent from IRMA to the POS and/or Scales.
		EXEC dbo.Dynamic_POSSearchForNonBatchedChanges
			@NewItemVal = 0,
			@ItemChangeVal = 0,
			@RemoveItemVal = 1,
			@PIRUSHeaderActionVal = 'D ',
			@Deletes = @Deletes,
			@IsPOSPush = 1,
			@IsScaleZoneData = @IsScaleZoneData,
			@POSDeAuthData = 1,	-- this query should return POS de-auth records (based on @IsScaleZoneData flag)
			@ScaleDeAuthData = 1, -- this query should return Scale de-auth records (based on @IsScaleZoneData flag)
			@ScaleAuthData = 0,
			@IdentifierAdds = 0,
			@IdentifierDeletes = 0,
			@Date = @Date
	END
	ELSE IF @IsItemNonBatchableChanges = 1 AND @IsScaleZoneData = 0
	BEGIN 
		-- Fourth resultset - list of items and their details for automatically communicated corporate de-auth changes
		EXEC dbo.Dynamic_POSSearchForNonBatchedChanges
				@NewItemVal = 0,
				@ItemChangeVal = 0,
				@RemoveItemVal = 0,
				@PIRUSHeaderActionVal = 'A ',
				@Deletes = 0,
				@IsPOSPush = 1,
				@IsScaleZoneData = 0,
				@POSDeAuthData = 0,	-- this query should return POS de-auth records (based on @IsScaleZoneData flag)
				@ScaleDeAuthData = 0, -- this query should return Scale de-auth records (based on @IsScaleZoneData flag)
				@ScaleAuthData = 0,
				@IdentifierAdds = 0,
				@IdentifierDeletes = 0,
				@Date = @Date,
				@IsItemNonBatchableChanges = @IsItemNonBatchableChanges
	END
	
    SET NOCOUNT OFF
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_GetPriceBatchSent] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_GetPriceBatchSent] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_GetPriceBatchSent] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_GetPriceBatchSent] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_GetPriceBatchSent] TO [IRMAReportsRole]
    AS [dbo];

