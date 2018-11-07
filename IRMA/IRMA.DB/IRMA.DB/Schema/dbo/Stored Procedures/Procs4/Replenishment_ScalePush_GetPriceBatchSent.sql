
CREATE PROCEDURE [dbo].[Replenishment_ScalePush_GetPriceBatchSent]
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
Jamali			2016-10-26				PBI 18909			Removed the Store dataset as this dataset was not used by Scale Push
Jamali			2016-10-26				PBI 18906			Removed the Taxflag dataset as this dataset was not used by Scale Push
***********************************************************************************************************************************************/

BEGIN

    SET NOCOUNT ON
    
	--Determine how region wants to send down data to scales
	DECLARE @PluDigitsSentToScale varchar(20)
	SELECT @PluDigitsSentToScale = PluDigitsSentToScale FROM InstanceData

	  -- First resultset - list of items and their details for batched changes.
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
		-- Second resultset - list of items and their details for automatically communicated store de-auth changes
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
			@Date = @Date,
			@LegacyStoresOnly = 0
	END
	ELSE IF @IsItemNonBatchableChanges = 1 AND @IsScaleZoneData = 0
	BEGIN 
		-- Second resultset - list of items and their details for automatically communicated corporate de-auth changes
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
    ON OBJECT::[dbo].[Replenishment_ScalePush_GetPriceBatchSent] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_ScalePush_GetPriceBatchSent] TO [IRSUser]
    AS [dbo];

