CREATE PROCEDURE dbo.GetDefaultPOSBatchIdRangeByStore
	@Store_No as int
AS

BEGIN
	-- Return the min and max batch ids defined for the POS Writer assigned to the store
	SELECT BatchIdMin, BatchIdMax 
		FROM POSWriter 
		WHERE POSFileWriterKey = 
			(SELECT DISTINCT StorePOSConfig.POSFileWriterKey 
				FROM StorePOSConfig, POSWriter 
				WHERE Store_No=@Store_No AND 
				FileWriterType='POS')
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetDefaultPOSBatchIdRangeByStore] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetDefaultPOSBatchIdRangeByStore] TO [IRMAClientRole]
    AS [dbo];

