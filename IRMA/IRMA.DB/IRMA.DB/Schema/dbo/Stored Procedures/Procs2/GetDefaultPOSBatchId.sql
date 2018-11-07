CREATE PROCEDURE dbo.GetDefaultPOSBatchId
	@Store_No as int,
    @ItemChgTypeID tinyint,
    @PriceChgTypeID tinyint,
	@POSBatchId as int = NULL
AS

BEGIN
	-- Set the default batch id 
	DECLARE @DefaultBatchId int
	DECLARE @POSFileWriterKey int
	SET @POSFileWriterKey = (SELECT DISTINCT StorePOSConfig.POSFileWriterKey 
			FROM StorePOSConfig, POSWriter 
			WHERE Store_No=@Store_No AND 
			FileWriterType='POS')
	
	-- (1) If the user entered a default batch id, it is used
	IF @POSBatchId IS NULL 
	BEGIN
		-- (2) If there is a default batch id defined for PriceChgType for the POS Writer assigned to the store, use it
		IF @PriceChgTypeID IS NOT NULL 
		BEGIN
			SET @DefaultBatchId = (SELECT POSBatchIdDefault 
					FROM POSWriterPriceChgBatchId 
					WHERE
						POSFileWriterKey  = @POSFileWriterKey AND
						PriceChgTypeID = @PriceChgTypeID)
		END
		-- (3) If there is a default batch id defined for ItemChgType for the POS Writer assigned to the store, use it
		IF @DefaultBatchId IS NULL AND @ItemChgTypeID IS NOT NULL 
		BEGIN
			SET @DefaultBatchId = (SELECT POSBatchIdDefault  
					FROM POSWriterItemChgBatchId 
					WHERE
						POSFileWriterKey  = @POSFileWriterKey AND
						ItemChgTypeID = @ItemChgTypeID)
		END
	END
	ELSE 
	BEGIN
		SET @DefaultBatchId = (SELECT @POSBatchId)
	END
	
	SELECT @DefaultBatchId AS DefaultBatchId
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetDefaultPOSBatchId] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetDefaultPOSBatchId] TO [IRMAClientRole]
    AS [dbo];

