CREATE PROCEDURE dbo.Administration_POSPush_UpdateStorePOSConfig
@Store_No int, 
@POSFileWriterKey int, 
@ConfigType varchar(20)
AS
-- Update an existing configuration record in the StorePOSConfig table for the
-- POS Push process.
BEGIN
   UPDATE StorePOSConfig SET
		Store_No = @Store_No, 
		POSFileWriterKey = @POSFileWriterKey, 
		ConfigType = @ConfigType 
   WHERE Store_No = @Store_No
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_UpdateStorePOSConfig] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_UpdateStorePOSConfig] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_UpdateStorePOSConfig] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_UpdateStorePOSConfig] TO [IRMAReportsRole]
    AS [dbo];

