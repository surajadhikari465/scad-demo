CREATE PROCEDURE dbo.DeleteReturnOrderUserRecords (
@Instance INT,
@User_ID  INT)
AS
BEGIN
    DELETE FROM ReturnOrder
    WHERE ReturnOrder.[Instance] = @Instance AND
          ReturnOrder.[User_ID]  = @User_ID
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteReturnOrderUserRecords] TO [IRMAClientRole]
    AS [dbo];

