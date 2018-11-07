CREATE PROCEDURE dbo.GetReturnOrderChanges (
@Instance INT,
@User_ID  INT)
AS
BEGIN
    SELECT * 
    FROM   ReturnOrder 
    WHERE ReturnOrder.[Instance] = @Instance AND
          ReturnOrder.[User_ID]  = @User_ID  AND
          (Quantity > 0 OR Total_Weight > 0)
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetReturnOrderChanges] TO [IRMAClientRole]
    AS [dbo];

