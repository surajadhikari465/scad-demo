CREATE PROCEDURE [dbo].[Administration_Scale_DeleteStoreScaleConfig]
@Store_No int
AS
-- Delete the entry in the StoreScaleConfig table, which is used for the
-- Scale Push process.
BEGIN
   delete from StoreScaleConfig  
   where Store_No = @Store_No
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_Scale_DeleteStoreScaleConfig] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_Scale_DeleteStoreScaleConfig] TO [IRMAClientRole]
    AS [dbo];

