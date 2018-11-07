CREATE PROCEDURE [dbo].[Administration_TagPush_InsertStoreElectronicShelfTagConfig]
@Store_No int, 
@POSFileWriterKey int, 
@ConfigType varchar(20)

AS
-- Insert a new configuration record into the StoreShelfTagConfig table for the
-- POS Push process.
BEGIN
   INSERT INTO StoreElectronicShelfTagConfig (Store_No, POSFileWriterKey, ConfigType) 
   VALUES (@Store_No, @POSFileWriterKey, @ConfigType)
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_TagPush_InsertStoreElectronicShelfTagConfig] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_TagPush_InsertStoreElectronicShelfTagConfig] TO [IRMAClientRole]
    AS [dbo];

