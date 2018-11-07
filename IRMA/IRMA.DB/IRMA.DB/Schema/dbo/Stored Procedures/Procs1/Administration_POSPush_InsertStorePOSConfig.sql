CREATE PROCEDURE dbo.Administration_POSPush_InsertStorePOSConfig
@Store_No int, 
@POSFileWriterKey int, 
@ConfigType varchar(20)

AS
-- Insert a new configuration record into the StorePOSConfig table for the
-- POS Push process.
BEGIN
   INSERT INTO StorePOSConfig (Store_No, POSFileWriterKey, ConfigType) 
   VALUES (@Store_No, @POSFileWriterKey, @ConfigType)
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_InsertStorePOSConfig] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_InsertStorePOSConfig] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_InsertStorePOSConfig] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_InsertStorePOSConfig] TO [IRMAReportsRole]
    AS [dbo];

