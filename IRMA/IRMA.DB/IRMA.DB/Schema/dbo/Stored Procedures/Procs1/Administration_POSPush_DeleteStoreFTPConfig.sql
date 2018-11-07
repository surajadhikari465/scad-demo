CREATE PROCEDURE dbo.Administration_POSPush_DeleteStoreFTPConfig
	@Store_No int, 
	@FileWriterType varchar(10)
AS
-- DELETEs configuration record in the StoreFTPConfig table for the POS Push process.
BEGIN
   
	DELETE FROM StoreFTPConfig WHERE Store_No = @Store_No AND FileWriterType = @FileWriterType
   
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_DeleteStoreFTPConfig] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_DeleteStoreFTPConfig] TO [IRMAClientRole]
    AS [dbo];

