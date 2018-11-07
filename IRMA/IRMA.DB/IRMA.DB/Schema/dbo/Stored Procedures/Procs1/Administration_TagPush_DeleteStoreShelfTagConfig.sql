CREATE PROCEDURE [dbo].[Administration_TagPush_DeleteStoreShelfTagConfig]
@Store_No int
AS
-- Delete the entry in the StoreShelfTagConfig table, which is used for the
-- ShelfTag process.
BEGIN
   delete from StoreShelfTagConfig  
   where Store_No = @Store_No
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_TagPush_DeleteStoreShelfTagConfig] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_TagPush_DeleteStoreShelfTagConfig] TO [IRMAClientRole]
    AS [dbo];

