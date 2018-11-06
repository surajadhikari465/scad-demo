CREATE PROCEDURE [dbo].[Administration_TagPush_InsertStoreShelfTagConfig]
@Store_No int, 
@POSFileWriterKey int, 
@ConfigType varchar(20)

AS
-- Insert a new configuration record into the StoreShelfTagConfig table for the
-- POS Push process.
BEGIN
   INSERT INTO StoreShelfTagConfig (Store_No, POSFileWriterKey, ConfigType) 
   VALUES (@Store_No, @POSFileWriterKey, @ConfigType)
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_TagPush_InsertStoreShelfTagConfig] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_TagPush_InsertStoreShelfTagConfig] TO [IRMAClientRole]
    AS [dbo];

