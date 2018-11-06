CREATE PROCEDURE dbo.Administration_POSPush_UpdateStoreScaleConfig (
@IsRegionalUpdate bit,
@Store_No int, 
@CorpScaleFileWriterKey int, 
@ZoneScaleFileWriterKey int,
@StoreScalefileWriterKey int
)
AS
-- Replaces the scale writer settings for this store with the input values.
BEGIN 
	-- Always delete the regional office values
	DECLARE @RegionalStoreNo int
	SELECT @RegionalStoreNo = Store_No FROM Store WHERE Regional = 1
	DELETE FROM StoreScaleConfig WHERE Store_No = @RegionalStoreNo
		
   -- Process the update
	IF (@IsRegionalUpdate = 1)
	BEGIN
		-- Are regional scale files being used?
		DECLARE @RegionalFiles bit
		SELECT @RegionalFiles = FlagValue FROM InstanceDataFlags WHERE FlagKey = 'UseRegionalScaleFile'
		
		-- If regional files are used:
		--	1. Delete all store values. These are no longer valid.
		--	2. Enter regional records for the corporate and zone writers.
		
		-- If regional files are not used, no updates are require.  
		-- The regional office values were already deleted.
		IF (@RegionalFiles = 1)
		BEGIN
			DELETE FROM StoreScaleConfig
			
			INSERT INTO StoreScaleConfig (Store_No, ScaleFileWriterKey)
			VALUES (@RegionalStoreNo, @CorpScaleFileWriterKey)
			
			INSERT INTO StoreScaleConfig (Store_No, ScaleFileWriterKey)
			VALUES (@RegionalStoreNo, @ZoneScaleFileWriterKey)
		END
	END
	
	IF (@IsRegionalUpdate = 0)
	BEGIN
		-- Delete the existing records for this store and insert a record with the
		-- current value.
		DELETE FROM StoreScaleConfig WHERE Store_No = @Store_No
			
		INSERT INTO StoreScaleConfig (Store_No, ScaleFileWriterKey)
		VALUES (@Store_No, @StoreScalefileWriterKey)
	END
   
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_UpdateStoreScaleConfig] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_UpdateStoreScaleConfig] TO [IRMAClientRole]
    AS [dbo];

