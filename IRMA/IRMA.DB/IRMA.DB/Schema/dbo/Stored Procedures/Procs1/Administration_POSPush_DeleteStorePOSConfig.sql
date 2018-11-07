CREATE PROCEDURE dbo.Administration_POSPush_DeleteStorePOSConfig
@Store_No int
AS
-- Delete the entry in the StorePOSConfig table, which is used for the
-- POS Push process.
BEGIN
   delete from StorePOSConfig  
   where Store_No = @Store_No
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_DeleteStorePOSConfig] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_DeleteStorePOSConfig] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_DeleteStorePOSConfig] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_DeleteStorePOSConfig] TO [IRMAReportsRole]
    AS [dbo];

