CREATE PROCEDURE [dbo].[Administration_TagPush_DeleteStoreElectronicShelfTagConfig]
@Store_No int
AS
-- Delete the entry in the StoreShelfTagConfig table, which is used for the
-- ShelfTag process.
BEGIN
   delete from StoreElectronicShelfTagConfig  
   where Store_No = @Store_No
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_TagPush_DeleteStoreElectronicShelfTagConfig] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_TagPush_DeleteStoreElectronicShelfTagConfig] TO [IRMAClientRole]
    AS [dbo];

