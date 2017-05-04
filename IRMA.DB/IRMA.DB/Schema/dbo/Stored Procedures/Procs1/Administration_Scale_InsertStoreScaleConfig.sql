CREATE PROCEDURE [dbo].[Administration_Scale_InsertStoreScaleConfig]
@Store_No int, 
@ScaleFileWriterKey int

AS
-- Insert a new configuration record into the StoreScaleConfig table for the
-- POS Push process.
BEGIN
   INSERT INTO StoreScaleConfig (Store_No, ScaleFileWriterKey) 
   VALUES (@Store_No, @ScaleFileWriterKey)
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_Scale_InsertStoreScaleConfig] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_Scale_InsertStoreScaleConfig] TO [IRMAClientRole]
    AS [dbo];

